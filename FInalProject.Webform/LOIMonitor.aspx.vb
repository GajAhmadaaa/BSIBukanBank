Imports System.Data.SqlClient
Imports System.Configuration

Public Class LOIMonitor
    Inherits Page
    
    ' Helper function to get status class for badge styling
    Protected Function GetStatusClass(status As String) As String
        Select Case status.ToLower()
            Case "pending"
                Return "warning"
            Case "readyforagreement"
                Return "primary"
            Case "unpaid"
                Return "danger"
            Case "paid"
                Return "success"
            Case Else
                Return "secondary"
        End Select
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindLOIs()
        End If
    End Sub

    Private Sub BindLOIs()
        ' Connect to database and get LOIs with Pending and ReadyForAgreement status using direct SQL query
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
                    WHERE loi.Status IN ('Pending', 'ReadyForAgreement')
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

    Private Sub ConfirmStockReady(ByVal loiID As Integer)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' 1. Update LetterOfIntent status and SalesPersonID
                Dim updateCmd As New SqlCommand("
                    UPDATE LetterOfIntent 
                    SET Status = 'ReadyForAgreement', SalesPersonID = @SalesPersonID
                    WHERE LOIID = @LOIID AND Status = 'Pending'", conn, transaction)
                updateCmd.Parameters.AddWithValue("@LOIID", loiID)
                updateCmd.Parameters.AddWithValue("@SalesPersonID", 1) ' Hardcoded SalesPersonID as requested

                Dim result As Integer = updateCmd.ExecuteNonQuery()

                If result > 0 Then
                    ' 2. Get CustomerID from the LOI
                    Dim customerID As Integer = 0
                    Dim selectCmd As New SqlCommand("SELECT CustomerID FROM LetterOfIntent WHERE LOIID = @LOIID", conn, transaction)
                    selectCmd.Parameters.AddWithValue("@LOIID", loiID)
                    Dim customerIdResult = selectCmd.ExecuteScalar()
                    If customerIdResult IsNot Nothing AndAlso Not IsDBNull(customerIdResult) Then
                        customerID = Convert.ToInt32(customerIdResult)
                    Else
                        ' If customer not found, something is wrong, rollback.
                        Throw New Exception("Customer not found for the given LOI.")
                    End If

                    ' 3. Insert notification for the customer
                    Dim notificationMsg As String = "Stock for your order #" & loiID.ToString() & " has been confirmed and is ready for the next step."
                    Dim insertCmd As New SqlCommand("
                        INSERT INTO CustomerNotification (CustomerID, LOIID, NotificationType, Message, IsRead)
                        VALUES (@CustomerID, @LOIID, @NotificationType, @Message, @IsRead)", conn, transaction)
                    insertCmd.Parameters.AddWithValue("@CustomerID", customerID)
                    insertCmd.Parameters.AddWithValue("@LOIID", loiID)
                    insertCmd.Parameters.AddWithValue("@NotificationType", "Stock Confirmed")
                    insertCmd.Parameters.AddWithValue("@Message", notificationMsg)
                    insertCmd.Parameters.AddWithValue("@IsRead", 0)
                    insertCmd.ExecuteNonQuery()

                    ' If all successful, commit the transaction
                    transaction.Commit()
                    litMessage.Text = "<div class=""alert alert-success"">Stock confirmed and notification sent for LOI ID: " & loiID.ToString() & "</div>"
                    BindLOIs() ' Refresh the grid
                Else
                    ' No rows affected, rollback
                    transaction.Rollback()
                    litMessage.Text = "<div class=""alert alert-warning"">Failed to confirm stock for LOI ID: " & loiID.ToString() & ". The LOI might not be in 'Pending' status.</div>"
                End If
            Catch ex As Exception
                ' Rollback transaction on error
                transaction.Rollback()
                litMessage.Text = "<div class=""alert alert-danger"">Error confirming stock: " & ex.Message & "</div>"
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub ShowLOIDetails(ByVal loiID As Integer)
        ' Connect to database and get LOI details using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Query to get LOI header information
                Dim cmd As New SqlCommand("SELECT " & vbCrLf &
                    "    loi.LOIID," & vbCrLf &
                    "    c.Name AS CustomerName," & vbCrLf &
                    "    d.Name AS DealerName," & vbCrLf &
                    "    sp.Name AS SalesPersonName," & vbCrLf &
                    "    loi.LOIDate," & vbCrLf &
                    "    loi.Status," & vbCrLf &
                    "    loi.Note AS SpecialRequests," & vbCrLf &  ' Using Note column for SpecialRequests
                    "    loi.Note AS Notes" & vbCrLf &          ' Using Note column for Notes
                    "FROM LetterOfIntent loi" & vbCrLf &
                    "INNER JOIN Customer c ON loi.CustomerID = c.CustomerID" & vbCrLf &
                    "INNER JOIN Dealer d ON loi.DealerID = d.DealerID" & vbCrLf &
                    "LEFT JOIN SalesPerson sp ON loi.SalesPersonID = sp.SalesPersonID" & vbCrLf &
                    "WHERE loi.LOIID = @LOIID", conn)
                cmd.Parameters.AddWithValue("@LOIID", loiID)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Populate labels with LOI details
                        lblLOIID.Text = reader("LOIID").ToString()
                        lblCustomerName.Text = reader("CustomerName").ToString()
                        lblDealerName.Text = reader("DealerName").ToString()
                        lblSalesPersonName.Text = If(reader.IsDBNull(reader.GetOrdinal("SalesPersonName")), "N/A", reader("SalesPersonName").ToString())
                        lblLOIDate.Text = Convert.ToDateTime(reader("LOIDate")).ToString("d")
                        lblStatus.Text = reader("Status").ToString()
                        
                        ' Populate additional information
                        ' Count the number of cars in this LOI instead of showing special requests
                        lblSpecialRequests.Text = GetCarCount(loiID).ToString()
                        lblNotes.Text = If(String.IsNullOrEmpty(reader("Notes").ToString()), "No notes", reader("Notes").ToString())
                        
                        ' Register script to show modal using Bootstrap 5 syntax
                        ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", _
                            "$(document).ready(function() {" & vbCrLf & _
                            "    var myModal = new bootstrap.Modal(document.getElementById('loiDetailModal'));" & vbCrLf & _
                            "    myModal.show();" & vbCrLf & _
                            "});", True)
                    Else
                        litMessage.Text = "<div class=""alert alert-warning"">LOI details not found for ID: " & loiID.ToString() & "</div>"
                        reader.Close() ' Close the reader before exiting
                        Return ' Exit the subroutine
                    End If
                    reader.Close() ' Close the reader before proceeding
                End Using
                
                ' Now get all the LOI details (car information)
                Dim detailCmd As New SqlCommand("SELECT " & vbCrLf &
                    "    c.Model AS CarModel," & vbCrLf &
                    "    lod.AgreedPrice," & vbCrLf &
                    "    lod.Discount" & vbCrLf &
                    "FROM LetterOfIntentDetail lod" & vbCrLf &
                    "INNER JOIN Car c ON lod.CarID = c.CarID" & vbCrLf &
                    "WHERE lod.LOIID = @LOIID", conn)
                detailCmd.Parameters.AddWithValue("@LOIID", loiID)
                
                Dim loiDetails As New List(Of Object)
                Dim totalAmount As Decimal = 0
                
                Using detailReader As SqlDataReader = detailCmd.ExecuteReader()
                    While detailReader.Read()
                        Dim agreedPrice As Decimal = Convert.ToDecimal(detailReader("AgreedPrice"))
                        Dim discount As Decimal = If(detailReader("Discount") Is DBNull.Value, 0, Convert.ToDecimal(detailReader("Discount")))
                        Dim itemTotal As Decimal = agreedPrice - discount
                        totalAmount += itemTotal
                        
                        loiDetails.Add(New With {
                            .CarModel = detailReader("CarModel").ToString(),
                            .AgreedPrice = agreedPrice.ToString("C", New System.Globalization.CultureInfo("id-ID")),
                            .Discount = discount.ToString("C", New System.Globalization.CultureInfo("id-ID")),
                            .TotalAmount = itemTotal.ToString("C", New System.Globalization.CultureInfo("id-ID"))
                        })
                    End While
                End Using
                
                ' Bind the details to the GridView
                gvLOIDetails.DataSource = loiDetails
                gvLOIDetails.DataBind()
                
                ' Set the total amount
                lblDetailTotalAmount.Text = totalAmount.ToString("C", New System.Globalization.CultureInfo("id-ID"))
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading LOI details: " & ex.Message & "</div>"
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
    
    ' Function to get the count of cars in an LOI
    Private Function GetCarCount(loiID As Integer) As Integer
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim carCount As Integer = 0
        
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT COUNT(*) FROM LetterOfIntentDetail WHERE LOIID = @LOIID", conn)
                cmd.Parameters.AddWithValue("@LOIID", loiID)
                
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    carCount = Convert.ToInt32(result)
                End If
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error counting cars: " & ex.Message & "</div>"
            End Try
        End Using
        
        Return carCount
    End Function
End Class