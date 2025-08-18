Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Logout
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Hapus sesi dan autentikasi
        Session.Clear()
        Session.Abandon()
        FormsAuthentication.SignOut()
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Arahkan ke halaman login
        Response.Redirect("~/Login.aspx")
    End Sub
End Class