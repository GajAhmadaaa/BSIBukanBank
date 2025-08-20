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

        ' Perform transfer using direct SQL query
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Check if there is enough stock in the source dealer
                Dim checkCmd As New SqlCommand("
                    SELECT Stock 
                    FROM DealerInventory 
                    WHERE DealerID = @FromDealerID AND CarID = @CarID", conn)
                checkCmd.Parameters.AddWithValue("@FromDealerID", fromDealerID)
                checkCmd.Parameters.AddWithValue("@CarID", carID)
                
                Dim currentStock As Object = checkCmd.ExecuteScalar()
                If currentStock Is Nothing OrElse Convert.ToInt32(currentStock) < quantity Then
                    litMessage.Text = "<div class=""alert alert-danger"">Not enough stock available in the source dealer.</div>"
                    Return
                End If
                
                ' Begin transaction
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' Reduce stock from source dealer
                        Dim reduceCmd As New SqlCommand("
                            UPDATE DealerInventory 
                            SET Stock = Stock - @Quantity
                            WHERE DealerID = @FromDealerID AND CarID = @CarID", conn, transaction)
                        reduceCmd.Parameters.AddWithValue("@Quantity", quantity)
                        reduceCmd.Parameters.AddWithValue("@FromDealerID", fromDealerID)
                        reduceCmd.Parameters.AddWithValue("@CarID", carID)
                        reduceCmd.ExecuteNonQuery()

                        ' Increase stock in destination dealer
                        Dim increaseCmd As New SqlCommand("
                            IF EXISTS (SELECT 1 FROM DealerInventory WHERE DealerID = @ToDealerID AND CarID = @CarID)
                            BEGIN
                                UPDATE DealerInventory 
                                SET Stock = Stock + @Quantity
                                WHERE DealerID = @ToDealerID AND CarID = @CarID
                            END
                            ELSE
                            BEGIN
                                DECLARE @Price MONEY
                                SELECT @Price = BasePrice FROM Car WHERE CarID = @CarID
                                
                                INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent)
                                VALUES (@ToDealerID, @CarID, @Quantity, @Price, 0, 0)
                            END", conn, transaction)
                        increaseCmd.Parameters.AddWithValue("@Quantity", quantity)
                        increaseCmd.Parameters.AddWithValue("@ToDealerID", toDealerID)
                        increaseCmd.Parameters.AddWithValue("@CarID", carID)
                        increaseCmd.ExecuteNonQuery()

                        ' Record the transfer in InventoryTransfer table
                        Dim recordCmd As New SqlCommand("
                            INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarID, Quantity, MutationDate)
                            VALUES (@FromDealerID, @ToDealerID, @CarID, @Quantity, GETDATE())", conn, transaction)
                        recordCmd.Parameters.AddWithValue("@FromDealerID", fromDealerID)
                        recordCmd.Parameters.AddWithValue("@ToDealerID", toDealerID)
                        recordCmd.Parameters.AddWithValue("@CarID", carID)
                        recordCmd.Parameters.AddWithValue("@Quantity", quantity)
                        recordCmd.ExecuteNonQuery()

                        ' Commit transaction
                        transaction.Commit()
                        litMessage.Text = "<div class=""alert alert-success"">Transfer from " & ddlFromDealer.SelectedItem.Text & " to " & ddlToDealer.SelectedItem.Text & " for " & quantity.ToString() & " of " & ddlCar.SelectedItem.Text & " completed successfully.</div>"
                    Catch ex As Exception
                        ' Rollback transaction on error
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            Catch ex As Exception
                ' Handle error - in a real application, you might want to log this
                litMessage.Text = "<div class=""alert alert-danger"">Error performing transfer: " & ex.Message & "</div>"
            End Try
        End Using

        BindTransferHistory()
    End Sub
End Class
