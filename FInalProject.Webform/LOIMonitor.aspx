<%@ Page Title="LOI Monitor" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOIMonitor.aspx.vb" Inherits="FinalProject.Webform.LOIMonitor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>LOI Monitor</h2>
    <p>Monitor Letter of Intent with Pending status.</p>

    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="gvLOIs" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                DataKeyNames="LOIID" OnRowCommand="gvLOIs_RowCommand">
                <Columns>
                    <asp:BoundField DataField="LOIID" HeaderText="LOI ID" SortExpression="LOIID" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" />
                    <asp:BoundField DataField="DealerName" HeaderText="Dealer" SortExpression="DealerName" />
                    <asp:BoundField DataField="LOIDate" HeaderText="LOI Date" SortExpression="LOIDate" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnConfirmStock" runat="server" CommandName="ConfirmStock" CommandArgument='<%# Eval("LOIID") %>' Text="Confirm Stock Ready" CssClass="btn btn-success btn-sm" />
                            <asp:Button ID="btnTransferInventory" runat="server" CommandName="TransferInventory" CommandArgument='<%# Eval("LOIID") %>' Text="Transfer Inventory" CssClass="btn btn-info btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
