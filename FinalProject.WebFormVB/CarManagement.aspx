<%@ Page Title="Manajemen Mobil" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CarManagement.aspx.vb" Inherits="FinalProject.WebFormVB.CarManagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Page.Title %></h2>

        <div class="row">
            <div class="col-md-4">
                <h3>Form Mobil</h3>
                <div class="card">
                    <div class="card-body">
                        <asp:HiddenField ID="hfCarID" runat="server" Value="0" />
                        <div class="mb-3">
                            <label for="txtBrand" class="form-label">Merek:</label>
                            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtModel" class="form-label">Model:</label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtYear" class="form-label">Tahun:</label>
                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtColor" class="form-label">Warna:</label>
                            <asp:TextBox ID="txtColor" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtPrice" class="form-label">Harga:</label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSave" runat="server" Text="Simpan" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Batal" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
                        <br />
                        <asp:Label ID="lblMessage" runat="server" CssClass="mt-2"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <h3>Daftar Mobil</h3>
                <asp:GridView ID="gvCars" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" DataKeyNames="CarID" OnRowCommand="gvCars_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Brand" HeaderText="Merek" />
                        <asp:BoundField DataField="Model" HeaderText="Model" />
                        <asp:BoundField DataField="Year" HeaderText="Tahun" />
                        <asp:BoundField DataField="Color" HeaderText="Warna" />
                        <asp:BoundField DataField="Price" HeaderText="Harga" DataFormatString="{0:C}" />
                        <asp:TemplateField HeaderText="Aksi">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-warning" CommandName="EditCar" CommandArgument='<%# Eval("CarID") %>' />
                                <asp:Button ID="btnDelete" runat="server" Text="Hapus" CssClass="btn btn-sm btn-danger" CommandName="DeleteCar" CommandArgument='<%# Eval("CarID") %>' OnClientClick="return confirm('Apakah Anda yakin ingin menghapus mobil ini?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>