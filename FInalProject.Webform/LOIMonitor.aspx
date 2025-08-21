<%@ Page Title="LOI Monitor" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOIMonitor.aspx.vb" Inherits="FinalProject.Webform.LOIMonitor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h1 class="h3 mb-0">LOI Monitor</h1>
            <p class="text-muted mb-0">Monitor Letter of Intent with Pending and Ready for Agreement status</p>
        </div>
        <div>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-outline-primary" OnClick="btnRefresh_Click" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">Pending & Ready for Agreement LOIs</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvLOIs" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover" GridLines="None" EmptyDataText="No pending or ready for agreement LOIs found"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLOIs_PageIndexChanging"
                            OnRowCommand="gvLOIs_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="LOIID" HeaderText="LOI ID" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                <asp:BoundField DataField="DealerName" HeaderText="Dealer" />
                                <asp:BoundField DataField="LOIDate" HeaderText="LOI Date" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Stock Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockStatus" runat="server" 
                                            Text='<%# Eval("StockStatus") %>' 
                                            ForeColor='<%# If(Eval("StockStatus").ToString().StartsWith("Shortage"), System.Drawing.Color.Red, If(Eval("StockStatus").ToString() = "In Stock", System.Drawing.Color.Green, System.Drawing.Color.Black)) %>'
                                            Font-Bold='<%# Eval("StockStatus").ToString().StartsWith("Shortage") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnViewDetail" runat="server" CommandName="ViewDetail" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="View Detail" 
                                            CssClass="btn btn-outline-primary btn-sm me-1" />
                                        <asp:Button ID="btnConfirmStock" runat="server" CommandName="ConfirmStock" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="Confirm Stock" 
                                            CssClass="btn btn-success btn-sm me-1" 
                                            Visible='<%# Eval("Status").ToString() = "Pending" %>' />
                                        <asp:Button ID="btnTransferInventory" runat="server" CommandName="TransferInventory" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="Transfer Inventory" 
                                            CssClass="btn btn-info btn-sm" 
                                            Visible='<%# Eval("Status").ToString() = "Pending" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="table-pager" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="table-primary" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- LOI Detail Modal -->
    <div class="modal fade" id="loiDetailModal" tabindex="-1" aria-labelledby="loiDetailModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="loiDetailModalLabel">LOI Details</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12">
                            <div class="card border-0 shadow-sm mb-4">
                                <div class="card-header bg-white py-3">
                                    <h5 class="card-title mb-0 fw-semibold">Order #<asp:Label ID="lblLOIID" runat="server" /></h5>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <dl class="row">
                                                <dt class="col-sm-4">Customer:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblCustomerName" runat="server" /></dd>
                                                
                                                <dt class="col-sm-4">Dealer:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblDealerName" runat="server" /></dd>

                                                <dt class="col-sm-4">Sales Person:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblSalesPersonName" runat="server" /></dd>
                                                
                                                <dt class="col-sm-4">LOI Date:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblLOIDate" runat="server" /></dd>
                                            </dl>
                                        </div>
                                        <div class="col-md-6">
                                            <dl class="row">
                                                <dt class="col-sm-4">Status:</dt>
                                                <dd class="col-sm-8">
                                                    <span class="badge bg-<%= GetStatusClass(lblStatus.Text) %>">
                                                        <asp:Label ID="lblStatus" runat="server" />
                                                    </span>
                                                </dd>
                                                
                                                <dt class="col-sm-4">Ordered Car:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblSpecialRequests" runat="server" Text="0" /></dd>
                                                
                                                <dt class="col-sm-4">Notes:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblNotes" runat="server" Text="No notes" /></dd>
                                            </dl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-12">
                            <h5 class="fw-semibold mb-3">Items</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvLOIDetails" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-hover" GridLines="None" EmptyDataText="No items found">
                                    <Columns>
                                        <asp:BoundField DataField="CarModel" HeaderText="Car Model" ItemStyle-CssClass="fw-bold" />
                                        <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("AgreedPrice") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("Discount") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="fw-bold">
                                            <ItemTemplate>
                                                <%# Eval("TotalAmount") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="table-pager" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="table-primary" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row mt-3">
                        <div class="col-12 text-end">
                            <h5 class="fw-semibold">Total Amount: <asp:Label ID="lblDetailTotalAmount" runat="server" CssClass="fw-bold" /></h5>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Hidden field to store LOI ID for modal -->
    <asp:HiddenField ID="hfLOIID" runat="server" />
    
    <!-- Literal for messages -->
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
</asp:Content>