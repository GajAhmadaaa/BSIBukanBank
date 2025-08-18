Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Tampilkan nama pengguna
            lblUserInfo.Text = "Hello, " & HttpContext.Current.User.Identity.Name
            LoginStatus.Visible = True
        Else
            ' Sembunyikan status login jika tidak login
            LoginStatus.Visible = False
        End If
    End Sub
End Class