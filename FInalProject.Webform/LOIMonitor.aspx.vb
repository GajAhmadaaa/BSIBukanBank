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
                litMessage.Text = "<div class=""alert alert-danger"">Error loading LOIs: " & ex.Message & "</div>"
            End Try
        End Using

        gvLOIs.DataSource = lois
        gvLOIs.DataBind()
    End Sub

    Protected Sub gvLOIs_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvLOIs.RowCommand
        ' Validate that the command argument is a valid integer
        Dim loiID As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), loiID) Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid LOI ID provided.</div>"
            Return
        End If

        If e.CommandName = "ViewDetail" Then
            ' Handle View Detail button click
            ShowLOIDetails(loiID)
        ElseIf e.CommandName = "ConfirmStock" Then
            ' Handle Confirm Stock Ready button click
            ConfirmStockReady(loiID)
        ElseIf e.CommandName = "TransferInventory" Then
            ' Handle Transfer Inventory button click
            Response.Redirect("TransferInventory.aspx?loiId=" & loiID.ToString())
        End If
    End Sub

    Private Sub ShowLOIDetails(ByVal loiID As Integer)
        ' Connect to database and get LOI details using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Simplified query focusing on the core LOI information we know exists
                Dim cmd As New SqlCommand("SELECT " & vbCrLf &
                    "    loi.LOIID," & vbCrLf &
                    "    c.Name AS CustomerName," & vbCrLf &
                    "    d.Name AS DealerName," & vbCrLf &
                    "    loi.LOIDate," & vbCrLf &
                    "    loi.Status" & vbCrLf &
                    "FROM LetterOfIntent loi" & vbCrLf &
                    "INNER JOIN Customer c ON loi.CustomerID = c.CustomerID" & vbCrLf &
                    "INNER JOIN Dealer d ON loi.DealerID = d.DealerID" & vbCrLf &
                    "WHERE loi.LOIID = @LOIID", conn)
                cmd.Parameters.AddWithValue("@LOIID", loiID)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Populate labels with LOI details
                        lblLOIID.Text = reader("LOIID").ToString()
                        lblCustomerName.Text = reader("CustomerName").ToString()
                        lblDealerName.Text = reader("DealerName").ToString()
                        lblLOIDate.Text = Convert.ToDateTime(reader("LOIDate")).ToString("d")
                        lblStatus.Text = reader("Status").ToString()
                        
                        ' Set default values for additional fields that might not be in this query
                        lblCarModel.Text = "N/A"
                        lblQuantity.Text = "N/A"
                        lblAgreedPrice.Text = "N/A"
                        lblDiscount.Text = "N/A"
                        lblTotalAmount.Text = "N/A"
                        lblSpecialRequests.Text = "No special requests"
                        lblNotes.Text = "No notes"
                        
                        ' Register script to show modal using Bootstrap 5 syntax
                        ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", _
                            "$(document).ready(function() {" & vbCrLf & _
                            "    var myModal = new bootstrap.Modal(document.getElementById('loiDetailModal'));" & vbCrLf & _
                            "    myModal.show();" & vbCrLf & _
                            "});", True)
                    Else
                        litMessage.Text = "<div class=""alert alert-warning"">LOI details not found for ID: " & loiID.ToString() & "</div>"
                    End If
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading LOI details: " & ex.Message & "</div>"
            End Try
        End Using
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
                    litMessage.Text = "<div class=""alert alert-success"">Stock confirmed as ready for LOI ID: " & loiID.ToString() & "</div>"
                    BindLOIs() ' Refresh the grid
                Else
                    ' No rows affected
                    litMessage.Text = "<div class=""alert alert-warning"">Failed to confirm stock for LOI ID: " & loiID.ToString() & "</div>"
                End If
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error confirming stock: " & ex.Message & "</div>"
            End Try
        End Using
    End Sub
    
    ' New refresh button handler
    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs)
        BindLOIs()
    End Sub
    
    ' Paging handler for GridView
    Protected Sub gvLOIs_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvLOIs.PageIndex = e.NewPageIndex
        BindLOIs()
    End Sub
End Class