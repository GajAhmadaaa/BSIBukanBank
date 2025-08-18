<%@ Page Title="Lihat Inventaris Dealer" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DealerInventoryView.aspx.vb" Inherits="FinalProject.WebFormVB.DealerInventoryView" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Page.Title %></h2>
        <div class="row">
            <div class="col-md-6">
                <div class="mb-3">
                    <label for="ddlDealer" class="form-label">Pilih Dealer:</label>
                    <asp:DropDownList ID="ddlDealer" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDealer_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <h3>Daftar Mobil & Jumlah per Dealer</h3>
                <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField DataField="ModelMobil" HeaderText="Model Mobil" />
                        <asp:BoundField DataField="JumlahUnit" HeaderText="Jumlah Unit" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>