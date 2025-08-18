Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class CarManagement
    Inherits System.Web.UI.Page

    Private ReadOnly _connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindCarGrid()
        End If
    End Sub

    Private Sub BindCarGrid()
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("SELECT CarID, Brand, Model, Year, Color, Price FROM Car ORDER BY Brand, Model", conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    gvCars.DataSource = dt
                    gvCars.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub ClearForm()
        hfCarID.Value = "0"
        txtBrand.Text = String.Empty
        txtModel.Text = String.Empty
        txtYear.Text = String.Empty
        txtColor.Text = String.Empty
        txtPrice.Text = String.Empty
        btnSave.Text = "Simpan"
        lblMessage.Text = String.Empty
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim carID As Integer = Integer.Parse(hfCarID.Value)
        Dim brand As String = txtBrand.Text.Trim()
        Dim model As String = txtModel.Text.Trim()
        Dim year As Integer = If(String.IsNullOrEmpty(txtYear.Text), 0, Integer.Parse(txtYear.Text))
        Dim color As String = txtColor.Text.Trim()
        Dim price As Decimal = If(String.IsNullOrEmpty(txtPrice.Text), 0, Decimal.Parse(txtPrice.Text))

        If String.IsNullOrEmpty(brand) OrElse String.IsNullOrEmpty(model) Then
            lblMessage.Text = "Merek dan Model tidak boleh kosong."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        Using conn As New SqlConnection(_connStr)
            Dim query As String
            If carID > 0 Then
                ' Update
                query = "UPDATE Car SET Brand = @Brand, Model = @Model, Year = @Year, Color = @Color, Price = @Price WHERE CarID = @CarID"
            Else
                ' Insert
                query = "INSERT INTO Car (Brand, Model, Year, Color, Price) VALUES (@Brand, @Model, @Year, @Color, @Price)"
            End If

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Brand", brand)
                cmd.Parameters.AddWithValue("@Model", model)
                cmd.Parameters.AddWithValue("@Year", year)
                cmd.Parameters.AddWithValue("@Color", color)
                cmd.Parameters.AddWithValue("@Price", price)
                If carID > 0 Then
                    cmd.Parameters.AddWithValue("@CarID", carID)
                End If

                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using

        lblMessage.Text = "Data mobil berhasil disimpan."
        lblMessage.CssClass = "alert alert-success"
        BindCarGrid()
        ClearForm()
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    Protected Sub gvCars_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim carID As Integer = Convert.ToInt32(e.CommandArgument)
        If e.CommandName = "EditCar" Then
            PopulateFormForEdit(carID)
        ElseIf e.CommandName = "DeleteCar" Then
            DeleteCar(carID)
        End If
    End Sub

    Private Sub PopulateFormForEdit(carID As Integer)
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("SELECT CarID, Brand, Model, Year, Color, Price FROM Car WHERE CarID = @CarID", conn)
                cmd.Parameters.AddWithValue("@CarID", carID)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        hfCarID.Value = dt.Rows(0)("CarID").ToString()
                        txtBrand.Text = dt.Rows(0)("Brand").ToString()
                        txtModel.Text = dt.Rows(0)("Model").ToString()
                        txtYear.Text = dt.Rows(0)("Year").ToString()
                        txtColor.Text = dt.Rows(0)("Color").ToString()
                        txtPrice.Text = dt.Rows(0)("Price").ToString()
                        btnSave.Text = "Update"
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub DeleteCar(carID As Integer)
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("DELETE FROM Car WHERE CarID = @CarID", conn)
                cmd.Parameters.AddWithValue("@CarID", carID)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        lblMessage.Text = "Data mobil berhasil dihapus."
        lblMessage.CssClass = "alert alert-warning"
        BindCarGrid()
    End Sub

End Class