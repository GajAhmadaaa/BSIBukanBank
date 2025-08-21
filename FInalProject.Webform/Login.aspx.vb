Imports System.Web.Security

Partial Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If User.Identity.IsAuthenticated Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Mock login logic
        If txtUsername.Text = "admin" AndAlso txtPassword.Text = "password" Then
            FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, chkRememberMe.Checked)
        Else
            litMessage.Text = "<div class='alert alert-danger mt-3'>Invalid username or password.</div>"
        End If
    End Sub

End Class
