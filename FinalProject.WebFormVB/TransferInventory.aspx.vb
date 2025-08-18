Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class TransferInventory
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
        End If
        
        If Not IsPostBack Then
            LoadDealers()
            LoadRiwayatTransfer()
        End If
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

    Private Sub LoadRiwayatTransfer()
        Dim dt As New DataTable()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT it.InventoryTransferID AS TransferID, dFrom.Name AS DealerAsal, dTo.Name AS DealerTujuan, c.Model AS ModelMobil, it.Quantity AS JumlahUnit, it.MutationDate AS TanggalTransfer, 'Berhasil' AS Status FROM InventoryTransfer it INNER JOIN Dealer dFrom ON it.FromDealerID = dFrom.DealerID INNER JOIN Dealer dTo ON it.ToDealerID = dTo.DealerID INNER JOIN Car c ON it.CarID = c.CarID ORDER BY it.MutationDate DESC", conn)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using

        gvRiwayatTransfer.DataSource = dt
        gvRiwayatTransfer.DataBind()
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

        ' Reload transfer history
        LoadRiwayatTransfer()
    End Sub
End Class
