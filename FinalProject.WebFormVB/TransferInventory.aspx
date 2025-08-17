<%@ Page Title="Transfer Inventory" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferInventory.aspx.vb" Inherits="FinalProject.WebFormVB.TransferInventory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Page.Title %></h2>
        
        <div class="row">
            <div class="col-md-12">
                <h3>Transfer Inventory Antara Dealer</h3>
                <div class="card">
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="ddlDealerAsal" class="form-label">Dealer Asal:</label>
                            <asp:DropDownList ID="ddlDealerAsal" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label for="ddlDealerTujuan" class="form-label">Dealer Tujuan:</label>
                            <asp:DropDownList ID="ddlDealerTujuan" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label for="txtModelMobil" class="form-label">Model Mobil:</label>
                            <asp:TextBox ID="txtModelMobil" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtJumlahUnit" class="form-label">Jumlah Unit:</label>
                            <asp:TextBox ID="txtJumlahUnit" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnProsesTransfer" runat="server" Text="Proses Transfer" 
                            CssClass="btn btn-success" OnClick="btnProsesTransfer_Click" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="mt-3"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-md-12">
                <h3>Riwayat Transfer Inventory</h3>
                <asp:GridView ID="gvRiwayatTransfer" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" 
                    DataKeyNames="TransferID">
                    <Columns>
                        <asp:BoundField DataField="TransferID" HeaderText="ID Transfer" />
                        <asp:BoundField DataField="DealerAsal" HeaderText="Dealer Asal" />
                        <asp:BoundField DataField="DealerTujuan" HeaderText="Dealer Tujuan" />
                        <asp:BoundField DataField="ModelMobil" HeaderText="Model Mobil" />
                        <asp:BoundField DataField="JumlahUnit" HeaderText="Jumlah Unit" />
                        <asp:BoundField DataField="TanggalTransfer" HeaderText="Tanggal Transfer" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </main>
</asp:Content>