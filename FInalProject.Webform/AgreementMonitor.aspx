<%@ Page Title="Agreement Monitor" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgreementMonitor.aspx.vb" Inherits="FinalProject.Webform.AgreementMonitor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h1 class="h3 mb-0">Agreement Monitor</h1>
            <p class="text-muted mb-0">Monitor Sales Agreements</p>
        </div>
        <div>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-outline-primary" OnClick="btnRefresh_Click" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">Sales Agreements</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvAgreements" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover" GridLines="None" EmptyDataText="No sales agreements found"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gvAgreements_PageIndexChanging"
                            OnRowCommand="gvAgreements_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="SalesAgreementID" HeaderText="Agreement ID" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                <asp:BoundField DataField="DealerName" HeaderText="Dealer" />
                                <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person" />
                                <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnViewDetail" runat="server" CommandName="ViewDetail" 
                                            CommandArgument='<%# Eval("SalesAgreementID") %>' Text="View Detail" 
                                            CssClass="btn btn-outline-primary btn-sm" />
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

    <!-- Agreement Detail Modal -->
    <div class="modal fade" id="agreementDetailModal" tabindex="-1" aria-labelledby="agreementDetailModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="agreementDetailModalLabel">Agreement Details</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12">
                            <div class="card border-0 shadow-sm mb-4">
                                <div class="card-header bg-white py-3">
                                    <h5 class="card-title mb-0 fw-semibold">Agreement #<asp:Label ID="lblAgreementID" runat="server" /></h5>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <dl class="row">
                                                <dt class="col-sm-4">Customer:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblCustomerName" runat="server" />
                                                </dd>
                                                
                                                <dt class="col-sm-4">Dealer:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblDealerName" runat="server" />
                                                </dd>

                                                <dt class="col-sm-4">Sales Person:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblSalesPersonName" runat="server" />
                                                </dd>
                                                
                                                <dt class="col-sm-4">Transaction Date:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblTransactionDate" runat="server" />
                                                </dd>
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
                                                
                                                <dt class="col-sm-4">Total Amount:</dt>
                                                <dd class="col-sm-8"><asp:Label ID="lblTotalAmount" runat="server" CssClass="fw-bold" />
                                                </dd>
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
                                <asp:GridView ID="gvAgreementDetails" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-hover" GridLines="None" EmptyDataText="No items found">
                                    <Columns>
                                        <asp:BoundField DataField="CarModel" HeaderText="Car Model" ItemStyle-CssClass="fw-bold" />
                                        <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("Price") %>
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
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Literal for messages -->
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
</asp:Content>