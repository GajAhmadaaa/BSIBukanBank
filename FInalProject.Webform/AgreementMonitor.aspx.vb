Imports System.Data.SqlClient
Imports System.Configuration

Public Class AgreementMonitor
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
            BindAgreements()
        End If
    End Sub

    Private Sub BindAgreements()
        ' Connect to database and get Sales Agreements using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim agreements As New List(Of Object)

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    SELECT 
                        sa.SalesAgreementID,
                        c.Name AS CustomerName,
                        d.Name AS DealerName,
                        sp.Name AS SalesPersonName,
                        sa.TransactionDate,
                        sa.Status,
                        sa.TotalAmount
                    FROM SalesAgreement sa
                    INNER JOIN Customer c ON sa.CustomerID = c.CustomerID
                    INNER JOIN Dealer d ON sa.DealerID = d.DealerID
                    LEFT JOIN SalesPerson sp ON sa.SalesPersonID = sp.SalesPersonID
                    ORDER BY sa.TransactionDate DESC", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim totalAmount As Decimal = If(reader("TotalAmount") Is DBNull.Value, 0, Convert.ToDecimal(reader("TotalAmount")))
                        agreements.Add(New With {
                            .SalesAgreementID = reader("SalesAgreementID"),
                            .CustomerName = reader("CustomerName"),
                            .DealerName = reader("DealerName"),
                            .SalesPersonName = If(reader.IsDBNull(reader.GetOrdinal("SalesPersonName")), "N/A", reader("SalesPersonName").ToString()),
                            .TransactionDate = reader("TransactionDate"),
                            .Status = reader("Status"),
                            .TotalAmount = totalAmount.ToString("C", New System.Globalization.CultureInfo("id-ID"))
                        })
                    End While
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading agreements: " & ex.Message & "</div>"
            End Try
        End Using

        gvAgreements.DataSource = agreements
        gvAgreements.DataBind()
    End Sub

    Protected Sub gvAgreements_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAgreements.RowCommand
        ' Validate that the command argument is a valid integer
        Dim agreementID As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), agreementID) Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid Agreement ID provided.</div>"
            Return
        End If

        If e.CommandName = "ViewDetail" Then
            ' Handle View Detail button click
            ShowAgreementDetails(agreementID)
        End If
    End Sub

    Private Sub ShowAgreementDetails(ByVal agreementID As Integer)
        ' Connect to database and get Agreement details using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Query to get Agreement header information
                Dim cmd As New SqlCommand("SELECT " & vbCrLf &
                    "    sa.SalesAgreementID," & vbCrLf &
                    "    c.Name AS CustomerName," & vbCrLf &
                    "    d.Name AS DealerName," & vbCrLf &
                    "    sp.Name AS SalesPersonName," & vbCrLf &
                    "    sa.TransactionDate," & vbCrLf &
                    "    sa.Status," & vbCrLf &
                    "    sa.TotalAmount" & vbCrLf &
                    "FROM SalesAgreement sa" & vbCrLf &
                    "INNER JOIN Customer c ON sa.CustomerID = c.CustomerID" & vbCrLf &
                    "INNER JOIN Dealer d ON sa.DealerID = d.DealerID" & vbCrLf &
                    "LEFT JOIN SalesPerson sp ON sa.SalesPersonID = sp.SalesPersonID" & vbCrLf &
                    "WHERE sa.SalesAgreementID = @SalesAgreementID", conn)
                cmd.Parameters.AddWithValue("@SalesAgreementID", agreementID)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Populate labels with Agreement details
                        lblAgreementID.Text = reader("SalesAgreementID").ToString()
                        lblCustomerName.Text = reader("CustomerName").ToString()
                        lblDealerName.Text = reader("DealerName").ToString()
                        lblSalesPersonName.Text = If(reader.IsDBNull(reader.GetOrdinal("SalesPersonName")), "N/A", reader("SalesPersonName").ToString())
                        lblTransactionDate.Text = Convert.ToDateTime(reader("TransactionDate")).ToString("d")
                        lblStatus.Text = reader("Status").ToString()
                        lblTotalAmount.Text = If(reader("TotalAmount") Is DBNull.Value, 0, Convert.ToDecimal(reader("TotalAmount"))).ToString("C", New System.Globalization.CultureInfo("id-ID"))
                        
                        ' Register script to show modal using Bootstrap 5 syntax
                        ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", _
                            "$(document).ready(function() {" & vbCrLf & _
                            "    var myModal = new bootstrap.Modal(document.getElementById('agreementDetailModal'));" & vbCrLf & _
                            "    myModal.show();" & vbCrLf & _
                            "});", True)
                    Else
                        litMessage.Text = "<div class=""alert alert-warning"">Agreement details not found for ID: " & agreementID.ToString() & "</div>"
                        reader.Close() ' Close the reader before exiting
                        Return ' Exit the subroutine
                    End If
                    reader.Close() ' Close the reader before proceeding
                End Using
                
                ' Now get all the Agreement details (car information)
                Dim detailCmd As New SqlCommand("SELECT " & vbCrLf &
                    "    c.Model AS CarModel," & vbCrLf &
                    "    sad.Price," & vbCrLf &
                    "    sad.Discount" & vbCrLf &
                    "FROM SalesAgreementDetail sad" & vbCrLf &
                    "INNER JOIN Car c ON sad.CarID = c.CarID" & vbCrLf &
                    "WHERE sad.SalesAgreementID = @SalesAgreementID", conn)
                detailCmd.Parameters.AddWithValue("@SalesAgreementID", agreementID)
                
                Dim agreementDetails As New List(Of Object)
                
                Using detailReader As SqlDataReader = detailCmd.ExecuteReader()
                    While detailReader.Read()
                        Dim price As Decimal = Convert.ToDecimal(detailReader("Price"))
                        Dim discount As Decimal = If(detailReader("Discount") Is DBNull.Value, 0, Convert.ToDecimal(detailReader("Discount")))
                        Dim itemTotal As Decimal = price - discount
                        
                        agreementDetails.Add(New With {
                            .CarModel = detailReader("CarModel").ToString(),
                            .Price = price.ToString("C", New System.Globalization.CultureInfo("id-ID")),
                            .Discount = discount.ToString("C", New System.Globalization.CultureInfo("id-ID")),
                            .TotalAmount = itemTotal.ToString("C", New System.Globalization.CultureInfo("id-ID"))
                        })
                    End While
                End Using
                
                ' Bind the details to the GridView
                gvAgreementDetails.DataSource = agreementDetails
                gvAgreementDetails.DataBind()
                
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading Agreement details: " & ex.Message & "</div>"
            End Try
        End Using
    End Sub
    
    ' New refresh button handler
    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs)
        BindAgreements()
    End Sub
    
    ' Paging handler for GridView
    Protected Sub gvAgreements_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvAgreements.PageIndex = e.NewPageIndex
        BindAgreements()
    End Sub
End Class