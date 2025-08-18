Public Class TransferInventory
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
        End If
        
        If Not IsPostBack Then
            LoadDealers()
            LoadRiwayatTransfer()
        End If
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

    Private Sub LoadRiwayatTransfer()
        ' TODO: Implementasi pengambilan riwayat transfer dari database
        ' Contoh data dummy untuk demonstrasi
        Dim dt As New DataTable()
        dt.Columns.Add("TransferID")
        dt.Columns.Add("DealerAsal")
        dt.Columns.Add("DealerTujuan")
        dt.Columns.Add("ModelMobil")
        dt.Columns.Add("JumlahUnit")
        dt.Columns.Add("TanggalTransfer")
        dt.Columns.Add("Status")

        ' Menambahkan beberapa baris data dummy
        dt.Rows.Add(1, "Dealer Bandung", "Dealer Jakarta", "Toyota Avanza", 2, DateTime.Now.AddDays(-1), "Berhasil")
        dt.Rows.Add(2, "Dealer Surabaya", "Dealer Jakarta", "Honda Brio", 1, DateTime.Now.AddDays(-2), "Berhasil")

        gvRiwayatTransfer.DataSource = dt
        gvRiwayatTransfer.DataBind()
    End Sub

    Protected Sub btnProsesTransfer_Click(sender As Object, e As EventArgs) Handles btnProsesTransfer.Click
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
            Return
        End If
        
        ' TODO: Implementasi proses transfer inventory menggunakan stored procedure sp_TransferInventoryWithCheck
        ' Validasi input
        If String.IsNullOrEmpty(txtModelMobil.Text) OrElse String.IsNullOrEmpty(txtJumlahUnit.Text) Then
            lblMessage.Text = "Harap lengkapi semua field."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        If ddlDealerAsal.SelectedValue = ddlDealerTujuan.SelectedValue Then
            lblMessage.Text = "Dealer asal dan tujuan tidak boleh sama."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        ' Untuk sekarang, hanya menampilkan pesan
        lblMessage.Text = "Transfer inventory dari " & ddlDealerAsal.SelectedItem.Text & " ke " & ddlDealerTujuan.SelectedItem.Text & " untuk model " & txtModelMobil.Text & " sebanyak " & txtJumlahUnit.Text & " unit berhasil diproses."
        lblMessage.CssClass = "alert alert-success"

        ' Clear form setelah proses
        txtModelMobil.Text = ""
        txtJumlahUnit.Text = ""

        ' Reload riwayat transfer
        LoadRiwayatTransfer()
    End Sub
End Class