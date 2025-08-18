Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Periksa apakah pengguna sudah login
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            ' Jika belum login, arahkan ke halaman login
            Response.Redirect("~/Login.aspx")
        End If
    End Sub
End Class