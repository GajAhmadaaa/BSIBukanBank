<%@ Page Title="Transfer Inventory" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferInventory.aspx.vb" Inherits="FinalProject.Webform.TransferInventory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Transfer Inventory</h2>
    <p>Transfer car inventory between dealers.</p>

    <div class="row">
        <div class="col-md-6">
            <h3>New Transfer</h3>
            <div class="form-group">
                <label for="ddlFromDealer">From Dealer:</label>
                <asp:DropDownList ID="ddlFromDealer" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="ddlToDealer">To Dealer:</label>
                <asp:DropDownList ID="ddlToDealer" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="ddlCar">Car Model:</label>
                <asp:DropDownList ID="ddlCar" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="txtQuantity">Quantity:</label>
                <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" CssClass="form-control" Text="1"></asp:TextBox>
            </div>
            <asp:Button ID="btnTransfer" runat="server" Text="Perform Transfer" CssClass="btn btn-primary" OnClick="btnTransfer_Click" />
            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
        </div>
        <div class="col-md-6">
            <h3>Transfer History</h3>
            <asp:GridView ID="gvTransferHistory" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover">
                <Columns>
                    <asp:BoundField DataField="InventoryTransferID" HeaderText="Transfer ID" />
                    <asp:BoundField DataField="FromDealerName" HeaderText="From Dealer" />
                    <asp:BoundField DataField="ToDealerName" HeaderText="To Dealer" />
                    <asp:BoundField DataField="CarModel" HeaderText="Car Model" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="MutationDate" HeaderText="Date" DataFormatString="{0:d}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
