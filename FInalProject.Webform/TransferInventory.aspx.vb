Imports System.Data.SqlClient
Imports System.Configuration

Public Class TransferInventory
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindDealers()
            BindCars()
            BindTransferHistory()
        End If
    End Sub

    Private Sub BindDealers()
        ' Fetch dealers from database using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dealers As New List(Of Object)

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT DealerID, Name FROM Dealer ORDER BY Name", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        dealers.Add(New With {.DealerID = reader("DealerID"), .Name = reader("Name")})
                    End While
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading dealers: " & ex.Message & "</div>"
            End Try
        End Using

        ddlFromDealer.DataSource = dealers
        ddlFromDealer.DataTextField = "Name"
        ddlFromDealer.DataValueField = "DealerID"
        ddlFromDealer.DataBind()

        ddlToDealer.DataSource = dealers
        ddlToDealer.DataTextField = "Name"
        ddlToDealer.DataValueField = "DealerID"
        ddlToDealer.DataBind()
    End Sub

    Private Sub BindCars()
        ' Fetch cars from database using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim cars As New List(Of Object)

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT CarID, Model FROM Car ORDER BY Model", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cars.Add(New With {.CarID = reader("CarID"), .Model = reader("Model")})
                    End While
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading cars: " & ex.Message & "</div>"
            End Try
        End Using

        ddlCar.DataSource = cars
        ddlCar.DataTextField = "Model"
        ddlCar.DataValueField = "CarID"
        ddlCar.DataBind()
    End Sub

    Private Sub BindTransferHistory()
        ' Fetch transfer history from database
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim history As New List(Of Object)

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    SELECT 
                        it.InventoryTransferID,
                        d1.Name AS FromDealerName,
                        d2.Name AS ToDealerName,
                        c.Model AS CarModel,
                        it.Quantity,
                        it.MutationDate
                    FROM InventoryTransfer it
                    INNER JOIN Dealer d1 ON it.FromDealerID = d1.DealerID
                    INNER JOIN Dealer d2 ON it.ToDealerID = d2.DealerID
                    INNER JOIN Car c ON it.CarID = c.CarID
                    ORDER BY it.MutationDate DESC", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        history.Add(New With {
                            .InventoryTransferID = reader("InventoryTransferID"),
                            .FromDealerName = reader("FromDealerName"),
                            .ToDealerName = reader("ToDealerName"),
                            .CarModel = reader("CarModel"),
                            .Quantity = reader("Quantity"),
                            .MutationDate = reader("MutationDate")
                        })
                    End While
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error loading transfer history: " & ex.Message & "</div>"
            End Try
        End Using

        gvTransferHistory.DataSource = history
        gvTransferHistory.DataBind()
    End Sub

    Protected Sub btnTransfer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransfer.Click
        Dim fromDealerID As Integer = Convert.ToInt32(ddlFromDealer.SelectedValue)
        Dim toDealerID As Integer = Convert.ToInt32(ddlToDealer.SelectedValue)
        Dim carID As Integer = Convert.ToInt32(ddlCar.SelectedValue)
        Dim quantity As Integer = Convert.ToInt32(txtQuantity.Text)

        ' Perform transfer using stored procedure
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Use the stored procedure for transfer
                Dim cmd As New SqlCommand("sp_TransferInventoryWithCheck", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@FromDealerID", fromDealerID)
                cmd.Parameters.AddWithValue("@ToDealerID", toDealerID)
                cmd.Parameters.AddWithValue("@CarID", carID)
                cmd.Parameters.AddWithValue("@Quantity", quantity)
                
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    litMessage.Text = "<div class=""alert alert-success"">" & result.ToString() & "</div>"
                End If
            Catch ex As SqlException
                ' Handle SQL errors (like stock not available)
                litMessage.Text = "<div class=""alert alert-danger"">Error performing transfer: " & ex.Message & "</div>"
            Catch ex As Exception
                ' Handle other errors
                litMessage.Text = "<div class=""alert alert-danger"">Error performing transfer: " & ex.Message & "</div>"
            End Try
        End Using

        BindTransferHistory()
    End Sub
    
    ' New refresh button handler
    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs)
        BindDealers()
        BindCars()
        BindTransferHistory()
    End Sub
    
    ' Paging handler for GridView
    Protected Sub gvTransferHistory_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvTransferHistory.PageIndex = e.NewPageIndex
        BindTransferHistory()
    End Sub

    Protected Sub ddl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        UpdateStockDisplay()
    End Sub

    Private Sub UpdateStockDisplay()
        Dim fromDealerID As Integer = 0
        Dim toDealerID As Integer = 0
        Dim carID As Integer = 0

        Integer.TryParse(ddlFromDealer.SelectedValue, fromDealerID)
        Integer.TryParse(ddlToDealer.SelectedValue, toDealerID)
        Integer.TryParse(ddlCar.SelectedValue, carID)

        If fromDealerID > 0 AndAlso carID > 0 Then
            lblFromDealerStock.Text = GetStock(fromDealerID, carID).ToString()
        Else
            lblFromDealerStock.Text = "-"
        End If

        If toDealerID > 0 AndAlso carID > 0 Then
            lblToDealerStock.Text = GetStock(toDealerID, carID).ToString()
        Else
            lblToDealerStock.Text = "-"
        End If
    End Sub

    Private Function GetStock(ByVal dealerID As Integer, ByVal carID As Integer) As Integer
        Dim stock As Integer = 0
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT Stock FROM DealerInventory WHERE DealerID = @DealerID AND CarID = @CarID", conn)
                cmd.Parameters.AddWithValue("@DealerID", dealerID)
                cmd.Parameters.AddWithValue("@CarID", carID)
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    stock = Convert.ToInt32(result)
                End If
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error fetching stock: " & ex.Message & "</div>"
            End Try
        End Using
        Return stock
    End Function
End Class