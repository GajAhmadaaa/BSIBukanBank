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
        ' Placeholder for fetching dealers from API
        Dim dummyDealers As New List(Of Object) From {
            New With {.DealerID = 1, .Name = "Dealer A"},
            New With {.DealerID = 2, .Name = "Dealer B"}
        }
        ddlFromDealer.DataSource = dummyDealers
        ddlFromDealer.DataTextField = "Name"
        ddlFromDealer.DataValueField = "DealerID"
        ddlFromDealer.DataBind()

        ddlToDealer.DataSource = dummyDealers
        ddlToDealer.DataTextField = "Name"
        ddlToDealer.DataValueField = "DealerID"
        ddlToDealer.DataBind()
    End Sub

    Private Sub BindCars()
        ' Placeholder for fetching cars from API
        Dim dummyCars As New List(Of Object) From {
            New With {.CarID = 101, .Model = "Sedan X"},
            New With {.CarID = 102, .Model = "SUV Y"}
        }
        ddlCar.DataSource = dummyCars
        ddlCar.DataTextField = "Model"
        ddlCar.DataValueField = "CarID"
        ddlCar.DataBind()
    End Sub

    Private Sub BindTransferHistory()
        ' Placeholder for fetching transfer history from API
        Dim dummyHistory As New List(Of Object) From {
            New With {.InventoryTransferID = 1, .FromDealerName = "Dealer A", .ToDealerName = "Dealer B", .CarModel = "Sedan X", .Quantity = 1, .MutationDate = #2/1/2025#},
            New With {.InventoryTransferID = 2, .FromDealerName = "Dealer B", .ToDealerName = "Dealer A", .CarModel = "SUV Y", .Quantity = 2, .MutationDate = #2/5/2025#}
        }
        gvTransferHistory.DataSource = dummyHistory
        gvTransferHistory.DataBind()
    End Sub

    Protected Sub btnTransfer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransfer.Click
        Dim fromDealerID As Integer = Convert.ToInt32(ddlFromDealer.SelectedValue)
        Dim toDealerID As Integer = Convert.ToInt32(ddlToDealer.SelectedValue)
        Dim carID As Integer = Convert.ToInt32(ddlCar.SelectedValue)
        Dim quantity As Integer = Convert.ToInt32(txtQuantity.Text)

        ' Placeholder for calling API to perform transfer
        litMessage.Text = "<div class=""alert alert-success"">Transfer from " & ddlFromDealer.SelectedItem.Text & " to " & ddlToDealer.SelectedItem.Text & " for " & quantity.ToString() & " of " & ddlCar.SelectedItem.Text & " initiated.</div>"
        BindTransferHistory()
    End Sub
End Class
