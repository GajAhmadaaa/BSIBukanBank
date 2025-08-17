<%@ Page Title="Monitoring LOI" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOIMonitor.aspx.vb" Inherits="FinalProject.WebFormVB.LOIMonitor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Page.Title %></h2>
        
        <div class="row">
            <div class="col-md-12">
                <h3>Daftar LOI dengan Status Pending Stock</h3>
                <asp:GridView ID="gvLOIPendingStock" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" 
                    DataKeyNames="LOIID" OnRowCommand="gvLOIPendingStock_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="LOIID" HeaderText="ID LOI" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Nama Customer" />
                        <asp:BoundField DataField="DealerName" HeaderText="Dealer" />
                        <asp:BoundField DataField="TotalUnits" HeaderText="Jumlah Unit" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Tanggal Dibuat" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Aksi">
                            <ItemTemplate>
                                <asp:Button ID="btnKonfirmasiStok" runat="server" Text="Konfirmasi Stok Siap" 
                                    CommandName="KonfirmasiStok" CommandArgument='<%# Eval("LOIID") %>'
                                    CssClass="btn btn-primary btn-sm" />
                                <asp:Button ID="btnTransferInventory" runat="server" Text="Transfer Inventory" 
                                    CommandName="TransferInventory" CommandArgument='<%# Eval("LOIID") %>'
                                    CssClass="btn btn-secondary btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-md-12">
                <h3>Transfer Inventory</h3>
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
    </main>
</asp:Content>