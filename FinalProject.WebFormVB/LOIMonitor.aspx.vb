Public Class LOIMonitor
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
        End If
        
        If Not IsPostBack Then
            LoadLOIPendingStock()
            LoadDealers()
        End If
    End Sub

    Private Sub LoadLOIPendingStock()
        ' TODO: Implementasi pengambilan data LOI dengan status PendingStock dari database
        ' Contoh data dummy untuk demonstrasi
        Dim dt As New DataTable()
        dt.Columns.Add("LOIID")
        dt.Columns.Add("CustomerName")
        dt.Columns.Add("DealerName")
        dt.Columns.Add("TotalUnits")
        dt.Columns.Add("CreatedDate")

        ' Menambahkan beberapa baris data dummy
        dt.Rows.Add(1, "Budi Santoso", "Dealer Jakarta", 3, DateTime.Now.AddDays(-2))
        dt.Rows.Add(2, "Ahmad Rifai", "Dealer Surabaya", 2, DateTime.Now.AddDays(-1))

        gvLOIPendingStock.DataSource = dt
        gvLOIPendingStock.DataBind()
    End Sub

    Private Sub LoadDealers()
        ' TODO: Implementasi pengambilan data dealer dari database
        ' Contoh data dummy untuk demonstrasi
        ddlDealerAsal.Items.Add(New ListItem("Dealer Jakarta", "1"))
        ddlDealerAsal.Items.Add(New ListItem("Dealer Bandung", "2"))
        ddlDealerAsal.Items.Add(New ListItem("Dealer Surabaya", "3"))

        ddlDealerTujuan.Items.Add(New ListItem("Dealer Jakarta", "1"))
        ddlDealerTujuan.Items.Add(New ListItem("Dealer Bandung", "2"))
        ddlDealerTujuan.Items.Add(New ListItem("Dealer Surabaya", "3"))
    End Sub

    Protected Sub gvLOIPendingStock_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
            Return
        End If
        
        If e.CommandName = "KonfirmasiStok" Or e.CommandName = "TransferInventory" Then
            Dim loiid As Integer = Convert.ToInt32(e.CommandArgument)
            
            ' TODO: Implementasi logika untuk konfirmasi stok atau transfer inventory
            ' Untuk sekarang, hanya menampilkan pesan
            If e.CommandName = "KonfirmasiStok" Then
                ' Update status LOI menjadi ReadyForAgreement
                lblMessage.Text = "Status LOI ID " & loiid & " telah diubah menjadi ReadyForAgreement"
                lblMessage.CssClass = "alert alert-success"
            ElseIf e.CommandName = "TransferInventory" Then
                ' Lakukan transfer inventory
                lblMessage.Text = "Proses transfer inventory untuk LOI ID " & loiid & " dimulai"
                lblMessage.CssClass = "alert alert-info"
            End If
            
            ' Reload data setelah proses
            LoadLOIPendingStock()
        End If
    End Sub

    Protected Sub btnProsesTransfer_Click(sender As Object, e As EventArgs) Handles btnProsesTransfer.Click
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
            Return
        End If
        
        ' TODO: Implementasi proses transfer inventory menggunakan stored procedure sp_TransferInventoryWithCheck
        ' Untuk sekarang, hanya menampilkan pesan
        lblMessage.Text = "Transfer inventory dari " & ddlDealerAsal.SelectedItem.Text & " ke " & ddlDealerTujuan.SelectedItem.Text & " untuk model " & txtModelMobil.Text & " sebanyak " & txtJumlahUnit.Text & " unit berhasil diproses."
        lblMessage.CssClass = "alert alert-success"
        
        ' Clear form setelah proses
        txtModelMobil.Text = ""
        txtJumlahUnit.Text = ""
    End Sub
End Class