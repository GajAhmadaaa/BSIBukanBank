Imports System.Data.SqlClient
Imports System.Configuration

Public Class DealerInventory
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindDealers()
        End If
    End Sub

    Private Sub BindDealers()
        ' Connect to database and get all dealers
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT DealerID, Name FROM Dealer ORDER BY Name", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    ddlDealers.Items.Clear()
                    ddlDealers.Items.Add(New ListItem("-- Select Dealer --", "0"))
                    While reader.Read()
                        ddlDealers.Items.Add(New ListItem(reader("Name").ToString(), reader("DealerID").ToString()))
                    End While
                End Using
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error loading dealers: " & ex.Message & "</div>"
            End Try
        End Using
    End Sub

    Private Sub BindCars()
        ' Connect to database and get all cars
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("SELECT CarID, Model FROM Car ORDER BY Model", conn)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    ddlCars.Items.Clear()
                    While reader.Read()
                        ddlCars.Items.Add(New ListItem(reader("Model").ToString(), reader("CarID").ToString()))
                    End While
                End Using
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error loading cars: " & ex.Message & "</div>"
            End Try
        End Using
    End Sub

    Private Sub BindInventory(ByVal dealerID As Integer)
        ' Connect to database and get inventory for selected dealer
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    SELECT 
                        di.DealerInventoryID,
                        c.Model,
                        di.Stock,
                        di.Price,
                        di.DiscountPercent,
                        di.FeePercent
                    FROM DealerInventory di
                    INNER JOIN Car c ON di.CarID = c.CarID
                    WHERE di.DealerID = @DealerID
                    ORDER BY c.Model", conn)
                cmd.Parameters.AddWithValue("@DealerID", dealerID)

                Dim inventory As New List(Of Object)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        inventory.Add(New With {
                            .DealerInventoryID = reader("DealerInventoryID"),
                            .Model = reader("Model"),
                            .Stock = reader("Stock"),
                            .Price = Convert.ToDecimal(reader("Price")),
                            .DiscountPercent = reader("DiscountPercent"),
                            .FeePercent = reader("FeePercent")
                        })
                    End While
                End Using
                
                gvInventory.DataSource = inventory
                gvInventory.DataBind()
                pnlInventory.Visible = True
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error loading inventory: " & ex.Message & "</div>"
            End Try
        End Using
    End Sub

    Protected Sub ddlDealers_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim dealerID As Integer
        If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
            ' Get dealer name
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
            Using conn As New SqlConnection(connectionString)
                Try
                    conn.Open()
                    Dim cmd As New SqlCommand("SELECT Name FROM Dealer WHERE DealerID = @DealerID", conn)
                    cmd.Parameters.AddWithValue("@DealerID", dealerID)
                    Dim dealerName As String = cmd.ExecuteScalar().ToString()
                    lblDealerName.Text = dealerName
                Catch ex As Exception
                    litMessage.Text = "<div class=""alert alert-danger"">Error loading dealer information: " & ex.Message & "</div>"
                    Return
                End Try
            End Using

            BindInventory(dealerID)
        Else
            pnlInventory.Visible = False
        End If
    End Sub

    Protected Sub gvInventory_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvInventory.PageIndex = e.NewPageIndex
        Dim dealerID As Integer
        If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
            BindInventory(dealerID)
        End If
    End Sub

    Protected Sub gvInventory_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvInventory.EditIndex = e.NewEditIndex
        Dim dealerID As Integer
        If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
            BindInventory(dealerID)
        End If
    End Sub

    Protected Sub gvInventory_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvInventory.EditIndex = -1
        Dim dealerID As Integer
        If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
            BindInventory(dealerID)
        End If
    End Sub

    Protected Sub gvInventory_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim dealerInventoryID As Integer = Convert.ToInt32(gvInventory.DataKeys(e.RowIndex).Value)
        
        ' Get the updated values from the editing row
        Dim txtStock As TextBox = CType(gvInventory.Rows(e.RowIndex).FindControl("txtStock"), TextBox)
        Dim txtPrice As TextBox = CType(gvInventory.Rows(e.RowIndex).FindControl("txtPrice"), TextBox)
        Dim txtDiscount As TextBox = CType(gvInventory.Rows(e.RowIndex).FindControl("txtDiscount"), TextBox)
        Dim txtFee As TextBox = CType(gvInventory.Rows(e.RowIndex).FindControl("txtFee"), TextBox)

        ' Validate inputs
        Dim stock As Integer
        Dim price As Decimal
        Dim discount As Decimal
        Dim fee As Decimal

        If Not Integer.TryParse(txtStock.Text, stock) OrElse stock < 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid stock value.</div>"
            Return
        End If

        If Not Decimal.TryParse(txtPrice.Text, price) OrElse price < 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid price value.</div>"
            Return
        End If

        If Not Decimal.TryParse(txtDiscount.Text, discount) OrElse discount < 0 OrElse discount > 100 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid discount value. Must be between 0 and 100.</div>"
            Return
        End If

        If Not Decimal.TryParse(txtFee.Text, fee) OrElse fee < 0 OrElse fee > 100 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid fee value. Must be between 0 and 100.</div>"
            Return
        End If

        ' Update database
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("
                    UPDATE DealerInventory 
                    SET Stock = @Stock, Price = @Price, DiscountPercent = @DiscountPercent, FeePercent = @FeePercent
                    WHERE DealerInventoryID = @DealerInventoryID", conn)
                cmd.Parameters.AddWithValue("@Stock", stock)
                cmd.Parameters.AddWithValue("@Price", price)
                cmd.Parameters.AddWithValue("@DiscountPercent", discount)
                cmd.Parameters.AddWithValue("@FeePercent", fee)
                cmd.Parameters.AddWithValue("@DealerInventoryID", dealerInventoryID)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    litMessage.Text = "<div class=""alert alert-success"">Inventory updated successfully.</div>"
                Else
                    litMessage.Text = "<div class=""alert alert-warning"">No changes were made to the inventory.</div>"
                End If
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error updating inventory: " & ex.Message & "</div>"
            Finally
                gvInventory.EditIndex = -1
                Dim dealerID As Integer
                If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
                    BindInventory(dealerID)
                End If
            End Try
        End Using
    End Sub

    Protected Sub gvInventory_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim dealerInventoryID As Integer = Convert.ToInt32(gvInventory.DataKeys(e.RowIndex).Value)

        ' Delete from database
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand("DELETE FROM DealerInventory WHERE DealerInventoryID = @DealerInventoryID", conn)
                cmd.Parameters.AddWithValue("@DealerInventoryID", dealerInventoryID)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    litMessage.Text = "<div class=""alert alert-success"">Car removed from inventory successfully.</div>"
                Else
                    litMessage.Text = "<div class=""alert alert-warning"">Failed to remove car from inventory.</div>"
                End If
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error removing car from inventory: " & ex.Message & "</div>"
            Finally
                Dim dealerID As Integer
                If Integer.TryParse(ddlDealers.SelectedValue, dealerID) AndAlso dealerID > 0 Then
                    BindInventory(dealerID)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnAddCar_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfDealerInventoryID.Value = "0"
        BindCars()
        
        ' Clear form fields
        ddlCars.SelectedIndex = 0
        txtStock.Text = ""
        txtPrice.Text = ""
        txtDiscount.Text = ""
        txtFee.Text = ""
        
        ' Show modal using JavaScript
        ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", _
            "setTimeout(function() {" & vbCrLf & _
            "    document.getElementById('carModalLabel').innerText = 'Add Car to Inventory';" & vbCrLf & _
            "    var myModal = new bootstrap.Modal(document.getElementById('carModal'));" & vbCrLf & _
            "    myModal.show();" & vbCrLf & _
            "}, 100);", True)
    End Sub

    Protected Sub btnSaveCar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dealerID As Integer
        If Not Integer.TryParse(ddlDealers.SelectedValue, dealerID) OrElse dealerID <= 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Please select a dealer first.</div>"
            Exit Sub
        End If

        Dim carID As Integer
        If Not Integer.TryParse(ddlCars.SelectedValue, carID) OrElse carID <= 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Please select a car model.</div>"
            Exit Sub
        End If

        Dim stock As Integer
        Dim price As Decimal
        Dim discount As Decimal
        Dim fee As Decimal

        If Not Integer.TryParse(txtStock.Text, stock) OrElse stock < 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid stock value.</div>"
            Exit Sub
        End If

        If Not Decimal.TryParse(txtPrice.Text, price) OrElse price < 0 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid price value.</div>"
            Exit Sub
        End If

        If Not Decimal.TryParse(txtDiscount.Text, discount) OrElse discount < 0 OrElse discount > 100 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid discount value. Must be between 0 and 100.</div>"
            Exit Sub
        End If

        If Not Decimal.TryParse(txtFee.Text, fee) OrElse fee < 0 OrElse fee > 100 Then
            litMessage.Text = "<div class=""alert alert-danger"">Invalid fee value. Must be between 0 and 100.</div>"
            Exit Sub
        End If

        ' Save to database
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                
                ' Check if this car already exists in this dealer's inventory
                Dim checkCmd As New SqlCommand("
                    SELECT COUNT(*) FROM DealerInventory 
                    WHERE DealerID = @DealerID AND CarID = @CarID", conn)
                checkCmd.Parameters.AddWithValue("@DealerID", dealerID)
                checkCmd.Parameters.AddWithValue("@CarID", carID)
                
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                
                If count > 0 Then
                    ' Update existing record
                    Dim updateCmd As New SqlCommand("
                        UPDATE DealerInventory 
                        SET Stock = @Stock, Price = @Price, DiscountPercent = @DiscountPercent, FeePercent = @FeePercent
                        WHERE DealerID = @DealerID AND CarID = @CarID", conn)
                    updateCmd.Parameters.AddWithValue("@DealerID", dealerID)
                    updateCmd.Parameters.AddWithValue("@CarID", carID)
                    updateCmd.Parameters.AddWithValue("@Stock", stock)
                    updateCmd.Parameters.AddWithValue("@Price", price)
                    updateCmd.Parameters.AddWithValue("@DiscountPercent", discount)
                    updateCmd.Parameters.AddWithValue("@FeePercent", fee)
                    
                    Dim result As Integer = updateCmd.ExecuteNonQuery()
                    If result > 0 Then
                        litMessage.Text = "<div class=""alert alert-success"">Car inventory updated successfully.</div>"
                    Else
                        litMessage.Text = "<div class=""alert alert-warning"">No changes were made to the inventory.</div>"
                    End If
                Else
                    ' Insert new record
                    Dim insertCmd As New SqlCommand("
                        INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent)
                        VALUES (@DealerID, @CarID, @Stock, @Price, @DiscountPercent, @FeePercent)", conn)
                    insertCmd.Parameters.AddWithValue("@DealerID", dealerID)
                    insertCmd.Parameters.AddWithValue("@CarID", carID)
                    insertCmd.Parameters.AddWithValue("@Stock", stock)
                    insertCmd.Parameters.AddWithValue("@Price", price)
                    insertCmd.Parameters.AddWithValue("@DiscountPercent", discount)
                    insertCmd.Parameters.AddWithValue("@FeePercent", fee)
                    
                    Dim result As Integer = insertCmd.ExecuteNonQuery()
                    If result > 0 Then
                        litMessage.Text = "<div class=""alert alert-success"">Car added to inventory successfully.</div>"
                    Else
                        litMessage.Text = "<div class=""alert alert-warning"">Failed to add car to inventory.</div>"
                    End If
                End If
            Catch ex As Exception
                litMessage.Text = "<div class=""alert alert-danger"">Error saving car to inventory: " & ex.Message & "</div>"
            Finally
                ' Refresh the inventory grid
                BindInventory(dealerID)
            End Try
        End Using
    End Sub
End Class