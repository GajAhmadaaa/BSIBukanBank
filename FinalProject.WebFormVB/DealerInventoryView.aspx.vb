Imports System.Data
Imports System.Data.SqlClient

Public Class DealerInventoryView
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadDealers()
            LoadInventory()
        End If
    End Sub

    Private Sub LoadDealers()
        ddlDealer.Items.Clear()
        ' Ganti connection string sesuai konfigurasi Anda
        Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT DealerID, DealerName FROM Dealer", conn)
            Dim rdr As SqlDataReader = cmd.ExecuteReader()
            While rdr.Read()
                ddlDealer.Items.Add(New ListItem(rdr("DealerName").ToString(), rdr("DealerID").ToString()))
            End While
        End Using
    End Sub

    Private Sub LoadInventory()
        gvInventory.DataSource = Nothing
        gvInventory.DataBind()
        If ddlDealer.SelectedValue = "" Then Return

        Dim dt As New DataTable()
        ' Ganti connection string sesuai konfigurasi Anda
        Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT c.Brand + ' ' + c.Model AS ModelMobil, di.Stock AS JumlahUnit FROM DealerInventory di INNER JOIN Car c ON di.CarID = c.CarID WHERE di.DealerID = @DealerID", conn)
            cmd.Parameters.AddWithValue("@DealerID", ddlDealer.SelectedValue)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using

        gvInventory.DataSource = dt
        gvInventory.DataBind()
    End Sub

    Protected Sub ddlDealer_SelectedIndexChanged(sender As Object, e As EventArgs)
        LoadInventory()
    End Sub
End Class