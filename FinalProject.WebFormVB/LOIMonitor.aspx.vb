Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class LOIMonitor
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
        End If
        
        If Not IsPostBack Then
            LoadLOIPendingStock()
            LoadDealers()
        End If
    End Sub

    Private Sub LoadLOIPendingStock()
        Dim dt As New DataTable()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT l.LOIID, c.Name AS CustomerName, d.Name AS DealerName, (SELECT COUNT(*) FROM LetterOfIntentDetail WHERE LOIID = l.LOIID) AS TotalUnits, l.LOIDate AS CreatedDate FROM LetterOfIntent l INNER JOIN Customer c ON l.CustomerID = c.CustomerID INNER JOIN Dealer d ON l.DealerID = d.DealerID WHERE l.Status = 'PendingStock' ORDER BY l.LOIDate DESC", conn)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using

        gvLOIPendingStock.DataSource = dt
        gvLOIPendingStock.DataBind()
    End Sub

    Private Sub LoadDealers()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dt As New DataTable()
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("SELECT DealerID, Name FROM Dealer ORDER BY Name", conn)
                conn.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        ddlDealerAsal.DataSource = dt
        ddlDealerAsal.DataTextField = "Name"
        ddlDealerAsal.DataValueField = "DealerID"
        ddlDealerAsal.DataBind()

        ddlDealerTujuan.DataSource = dt
        ddlDealerTujuan.DataTextField = "Name"
        ddlDealerTujuan.DataValueField = "DealerID"
        ddlDealerTujuan.DataBind()
    End Sub

    Protected Sub gvLOIPendingStock_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
            Return
        End If
        
        Dim loiid As Integer = Convert.ToInt32(e.CommandArgument)
        
        If e.CommandName = "KonfirmasiStok" Then
            ' Update status LOI menjadi ReadyForAgreement
            Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Using cmd As New SqlCommand("UPDATE LetterOfIntent SET Status = 'ReadyForAgreement' WHERE LOIID = @LOIID", conn)
                    cmd.Parameters.AddWithValue("@LOIID", loiid)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            lblMessage.Text = "Status LOI ID " & loiid & " telah diubah menjadi ReadyForAgreement."
            lblMessage.CssClass = "alert alert-success"
            
            ' Reload data setelah proses
            LoadLOIPendingStock()
        ElseIf e.CommandName = "TransferInventory" Then
            ' Populate the transfer form based on the selected LOI
            Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
            Using conn As New SqlConnection(connStr)
                conn.Open()
                ' Get LOI details to pre-fill the form
                Dim getLoiDetailCmd As New SqlCommand("SELECT TOP 1 l.DealerID, c.Model, COUNT(ld.LOIDetailID) as UnitCount FROM LetterOfIntent l JOIN LetterOfIntentDetail ld ON l.LOIID = ld.LOIID JOIN Car c ON ld.CarID = c.CarID WHERE l.LOIID = @LOIID GROUP BY l.DealerID, c.Model", conn)
                getLoiDetailCmd.Parameters.AddWithValue("@LOIID", loiid)
                Dim reader = getLoiDetailCmd.ExecuteReader()
                If reader.Read() Then
                    ddlDealerTujuan.SelectedValue = reader("DealerID").ToString()
                    txtModelMobil.Text = reader("Model").ToString()
                    txtJumlahUnit.Text = reader("UnitCount").ToString()
                    lblMessage.Text = "Form transfer inventory telah diisi untuk LOI ID " & loiid & ". Silakan pilih dealer asal."
                    lblMessage.CssClass = "alert alert-info"
                End If
            End Using
        End If
    End Sub

    Protected Sub btnProsesTransfer_Click(sender As Object, e As EventArgs) Handles btnProsesTransfer.Click
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
            Return
        End If
        
        ' Validasi input
        If String.IsNullOrEmpty(txtModelMobil.Text) OrElse String.IsNullOrEmpty(txtJumlahUnit.Text) Then
            lblMessage.Text = "Harap lengkapi semua field."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        If ddlDealerAsal.SelectedValue = ddlDealerTujuan.SelectedValue Then
            lblMessage.Text = "Dealer asal dan tujuan tidak boleh sama."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
            Using conn As New SqlConnection(connStr)
                conn.Open()
                ' NOTE: The stored procedure sp_TransferInventoryWithCheck seems to be using an outdated schema (CarModel instead of CarID).
                ' The logic is implemented here directly for now. A proper fix would involve correcting the stored procedure.
                
                Dim carID As Integer = 0
                ' Get CarID from Model. Assuming model name is unique for now.
                Using getCarIdCmd As New SqlCommand("SELECT TOP 1 CarID FROM Car WHERE Model = @Model", conn)
                    getCarIdCmd.Parameters.AddWithValue("@Model", txtModelMobil.Text.Trim())
                    Dim result = getCarIdCmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        carID = Convert.ToInt32(result)
                    Else
                        lblMessage.Text = "Model mobil tidak ditemukan."
                        lblMessage.CssClass = "alert alert-danger"
                        Return
                    End If
                End Using

                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' 1. Check stock at source dealer
                        Dim checkStockCmd As New SqlCommand("SELECT Stock FROM DealerInventory WHERE DealerID = @DealerID AND CarID = @CarID", conn, transaction)
                        checkStockCmd.Parameters.AddWithValue("@DealerID", Convert.ToInt32(ddlDealerAsal.SelectedValue))
                        checkStockCmd.Parameters.AddWithValue("@CarID", carID)
                        Dim availableStockResult = checkStockCmd.ExecuteScalar()
                        Dim availableStock As Integer = 0
                        If availableStockResult IsNot Nothing AndAlso Not IsDBNull(availableStockResult) Then
                            availableStock = Convert.ToInt32(availableStockResult)
                        End If

                        If availableStock < Convert.ToInt32(txtJumlahUnit.Text) Then
                            lblMessage.Text = "Stok tidak mencukupi di dealer asal."
                            lblMessage.CssClass = "alert alert-danger"
                            transaction.Rollback()
                            Return
                        End If

                        ' 2. Decrease stock from source dealer
                        Dim updateFromCmd As New SqlCommand("UPDATE DealerInventory SET Stock = Stock - @Quantity WHERE DealerID = @DealerID AND CarID = @CarID", conn, transaction)
                        updateFromCmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtJumlahUnit.Text))
                        updateFromCmd.Parameters.AddWithValue("@DealerID", Convert.ToInt32(ddlDealerAsal.SelectedValue))
                        updateFromCmd.Parameters.AddWithValue("@CarID", carID)
                        updateFromCmd.ExecuteNonQuery()

                        ' 3. Increase/update stock at destination dealer
                        Dim updateToCmd As New SqlCommand("IF EXISTS (SELECT 1 FROM DealerInventory WHERE DealerID = @ToDealerID AND CarID = @CarID) " &
                                                          "UPDATE DealerInventory SET Stock = Stock + @Quantity WHERE DealerID = @ToDealerID AND CarID = @CarID " &
                                                          "ELSE " &
                                                          "INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent) SELECT @ToDealerID, @CarID, @Quantity, BasePrice, 0, 0 FROM Car WHERE CarID = @CarID", conn, transaction)
                        updateToCmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtJumlahUnit.Text))
                        updateToCmd.Parameters.AddWithValue("@ToDealerID", Convert.ToInt32(ddlDealerTujuan.SelectedValue))
                        updateToCmd.Parameters.AddWithValue("@CarID", carID)
                        updateToCmd.ExecuteNonQuery()

                        ' 4. Log the transfer
                        Dim insertHistoryCmd As New SqlCommand("INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarID, Quantity, MutationDate) VALUES (@FromDealerID, @ToDealerID, @CarID, @Quantity, GETDATE())", conn, transaction)
                        insertHistoryCmd.Parameters.AddWithValue("@FromDealerID", Convert.ToInt32(ddlDealerAsal.SelectedValue))
                        insertHistoryCmd.Parameters.AddWithValue("@ToDealerID", Convert.ToInt32(ddlDealerTujuan.SelectedValue))
                        insertHistoryCmd.Parameters.AddWithValue("@CarID", carID)
                        insertHistoryCmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtJumlahUnit.Text))
                        insertHistoryCmd.ExecuteNonQuery()
                        
                        transaction.Commit()

                        lblMessage.Text = "Transfer inventory berhasil diproses."
                        lblMessage.CssClass = "alert alert-success"

                        ' Clear form
                        txtModelMobil.Text = ""
                        txtJumlahUnit.Text = ""

                    Catch ex As Exception
                        transaction.Rollback()
                        lblMessage.Text = "Terjadi kesalahan saat transfer: " & ex.Message
                        lblMessage.CssClass = "alert alert-danger"
                    End Try
                End Using
            End Using
        Catch ex As Exception
            lblMessage.Text = "Terjadi kesalahan: " & ex.Message
            lblMessage.CssClass = "alert alert-danger"
        End Try

        ' Reload data
        LoadLOIPendingStock()
    End Sub
End Class
