Public Class LOIMonitor
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindLOIs()
        End If
    End Sub

    Private Sub BindLOIs()
        ' This is a placeholder. In a real application, you would call your API to get LOIs with PendingStock status.
        ' For now, we'll use dummy data.
        Dim dummyLOIs As New List(Of Object) From {
            New With {.LOIID = 1, .CustomerName = "John Doe", .DealerName = "Dealer A", .LOIDate = #1/1/2025#, .Status = "PendingStock"},
            New With {.LOIID = 2, .CustomerName = "Jane Smith", .DealerName = "Dealer B", .LOIDate = #1/5/2025#, .Status = "PendingStock"},
            New With {.LOIID = 3, .CustomerName = "Peter Jones", .DealerName = "Dealer A", .LOIDate = #1/10/2025#, .Status = "PendingStock"}
        }
        gvLOIs.DataSource = dummyLOIs
        gvLOIs.DataBind()
    End Sub

    Protected Sub gvLOIs_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvLOIs.RowCommand
        Dim loiID As Integer = Convert.ToInt32(e.CommandArgument)

        If e.CommandName = "ConfirmStock" Then
            ' Handle Confirm Stock Ready button click
            Response.Write("<script>alert('Confirm Stock Ready for LOI ID: " & loiID.ToString() & "');</script>")
            ' In a real application, you would call your API to update LOI status to ReadyForAgreement
        ElseIf e.CommandName = "TransferInventory" Then
            ' Handle Transfer Inventory button click
            Response.Redirect("TransferInventory.aspx?loiId=" & loiID.ToString())
        End If
    End Sub
End Class
