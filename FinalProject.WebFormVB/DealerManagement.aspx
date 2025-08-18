<%@ Page Title="Manajemen Dealer" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DealerManagement.aspx.vb" Inherits="FinalProject.WebFormVB.DealerManagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Page.Title %></h2>

        <div class="row">
            <div class="col-md-4">
                <h3>Form Dealer</h3>
                <div class="card">
                    <div class="card-body">
                        <asp:HiddenField ID="hfDealerID" runat="server" Value="0" />
                        <div class="mb-3">
                            <label for="txtDealerName" class="form-label">Nama Dealer:</label>
                            <asp:TextBox ID="txtDealerName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtAddress" class="form-label">Alamat:</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtPhone" class="form-label">Telepon:</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">Email:</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSave" runat="server" Text="Simpan" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Batal" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
                        <br />
                        <asp:Label ID="lblMessage" runat="server" CssClass="mt-2"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <h3>Daftar Dealer</h3>
                <asp:GridView ID="gvDealers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" DataKeyNames="DealerID" OnRowCommand="gvDealers_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="DealerName" HeaderText="Nama Dealer" />
                        <asp:BoundField DataField="Address" HeaderText="Alamat" />
                        <asp:BoundField DataField="Phone" HeaderText="Telepon" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Aksi">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-warning" CommandName="EditDealer" CommandArgument='<%# Eval("DealerID") %>' />
                                <asp:Button ID="btnDelete" runat="server" Text="Hapus" CssClass="btn btn-sm btn-danger" CommandName="DeleteDealer" CommandArgument='<%# Eval("DealerID") %>' OnClientClick="return confirm('Apakah Anda yakin ingin menghapus dealer ini?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>