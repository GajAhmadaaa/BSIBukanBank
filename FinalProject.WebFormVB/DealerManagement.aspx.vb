Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DealerManagement
    Inherits System.Web.UI.Page

    Private ReadOnly _connStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindDealerGrid()
        End If
    End Sub

    Private Sub BindDealerGrid()
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("SELECT DealerID, DealerName, Address, Phone, Email FROM Dealer ORDER BY DealerName", conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    gvDealers.DataSource = dt
                    gvDealers.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub ClearForm()
        hfDealerID.Value = "0"
        txtDealerName.Text = String.Empty
        txtAddress.Text = String.Empty
        txtPhone.Text = String.Empty
        txtEmail.Text = String.Empty
        btnSave.Text = "Simpan"
        lblMessage.Text = String.Empty
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim dealerID As Integer = Integer.Parse(hfDealerID.Value)
        Dim dealerName As String = txtDealerName.Text.Trim()
        Dim address As String = txtAddress.Text.Trim()
        Dim phone As String = txtPhone.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()

        If String.IsNullOrEmpty(dealerName) Then
            lblMessage.Text = "Nama Dealer tidak boleh kosong."
            lblMessage.CssClass = "alert alert-danger"
            Return
        End If

        Using conn As New SqlConnection(_connStr)
            Dim query As String
            If dealerID > 0 Then
                ' Update
                query = "UPDATE Dealer SET DealerName = @DealerName, Address = @Address, Phone = @Phone, Email = @Email WHERE DealerID = @DealerID"
            Else
                ' Insert
                query = "INSERT INTO Dealer (DealerName, Address, Phone, Email) VALUES (@DealerName, @Address, @Phone, @Email)"
            End If

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@DealerName", dealerName)
                cmd.Parameters.AddWithValue("@Address", address)
                cmd.Parameters.AddWithValue("@Phone", phone)
                cmd.Parameters.AddWithValue("@Email", email)
                If dealerID > 0 Then
                    cmd.Parameters.AddWithValue("@DealerID", dealerID)
                End If

                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using

        lblMessage.Text = "Data dealer berhasil disimpan."
        lblMessage.CssClass = "alert alert-success"
        BindDealerGrid()
        ClearForm()
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    Protected Sub gvDealers_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim dealerID As Integer = Convert.ToInt32(e.CommandArgument)
        If e.CommandName = "EditDealer" Then
            PopulateFormForEdit(dealerID)
        ElseIf e.CommandName = "DeleteDealer" Then
            DeleteDealer(dealerID)
        End If
    End Sub

    Private Sub PopulateFormForEdit(dealerID As Integer)
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("SELECT DealerID, DealerName, Address, Phone, Email FROM Dealer WHERE DealerID = @DealerID", conn)
                cmd.Parameters.AddWithValue("@DealerID", dealerID)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        hfDealerID.Value = dt.Rows(0)("DealerID").ToString()
                        txtDealerName.Text = dt.Rows(0)("DealerName").ToString()
                        txtAddress.Text = dt.Rows(0)("Address").ToString()
                        txtPhone.Text = dt.Rows(0)("Phone").ToString()
                        txtEmail.Text = dt.Rows(0)("Email").ToString()
                        btnSave.Text = "Update"
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub DeleteDealer(dealerID As Integer)
        Using conn As New SqlConnection(_connStr)
            Using cmd As New SqlCommand("DELETE FROM Dealer WHERE DealerID = @DealerID", conn)
                cmd.Parameters.AddWithValue("@DealerID", dealerID)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        lblMessage.Text = "Data dealer berhasil dihapus."
        lblMessage.CssClass = "alert alert-warning"
        BindDealerGrid()
    End Sub

End Class