Imports System.Data.SqlClient
Imports System.Configuration

Public Class LOIMonitor
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindLOIs()
        End If
    End Sub

    Private Sub BindLOIs()
        ' Connect to database and get LOIs with Pending status using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim lois As New List(Of Object)

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    SELECT 
                        loi.LOIID,
                        c.Name AS CustomerName,
                        d.Name AS DealerName,
                        loi.LOIDate,
                        loi.Status
                    FROM LetterOfIntent loi
                    INNER JOIN Customer c ON loi.CustomerID = c.CustomerID
                    INNER JOIN Dealer d ON loi.DealerID = d.DealerID
                    WHERE loi.Status = 'Pending'
                    ORDER BY loi.LOIDate DESC", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lois.Add(New With {
                            .LOIID = reader("LOIID"),
                            .CustomerName = reader("CustomerName"),
                            .DealerName = reader("DealerName"),
                            .LOIDate = reader("LOIDate"),
                            .Status = reader("Status")
                        })
                    End While
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                Response.Write("<script>alert('Error loading LOIs: " & ex.Message & "');</script>")
            End Try
        End Using

        gvLOIs.DataSource = lois
        gvLOIs.DataBind()
    End Sub

    Protected Sub gvLOIs_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvLOIs.RowCommand
        Dim loiID As Integer = Convert.ToInt32(e.CommandArgument)

        If e.CommandName = "ConfirmStock" Then
            ' Handle Confirm Stock Ready button click
            ConfirmStockReady(loiID)
        ElseIf e.CommandName = "TransferInventory" Then
            ' Handle Transfer Inventory button click
            Response.Redirect("TransferInventory.aspx?loiId=" & loiID.ToString())
        End If
    End Sub

    Private Sub ConfirmStockReady(ByVal loiID As Integer)
        ' Connect to database and update LOI status to ReadyForAgreement using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    UPDATE LetterOfIntent 
                    SET Status = 'ReadyForAgreement'
                    WHERE LOIID = @LOIID AND Status = 'Pending'", conn)
                cmd.Parameters.AddWithValue("@LOIID", loiID)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    ' Success
                    Response.Write("<script>alert('Stock confirmed as ready for LOI ID: " & loiID.ToString() & "');</script>")
                    BindLOIs() ' Refresh the grid
                Else
                    ' No rows affected
                    Response.Write("<script>alert('Failed to confirm stock for LOI ID: " & loiID.ToString() & "');</script>")
                End If
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                Response.Write("<script>alert('Error confirming stock: " & ex.Message & "');</script>")
            End Try
        End Using
    End Sub
End Class
