<%@ Page Title="LOI Monitor" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOIMonitor.aspx.vb" Inherits="FinalProject.Webform.LOIMonitor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h1 class="h3 mb-0">LOI Monitor</h1>
            <p class="text-muted mb-0">Monitor Letter of Intent with Pending status</p>
        </div>
        <div>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-outline-primary" OnClick="btnRefresh_Click" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">Pending LOIs</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvLOIs" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover" GridLines="None" EmptyDataText="No pending LOIs found"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLOIs_PageIndexChanging"
                            OnRowCommand="gvLOIs_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="LOIID" HeaderText="LOI ID" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                <asp:BoundField DataField="DealerName" HeaderText="Dealer" />
                                <asp:BoundField DataField="LOIDate" HeaderText="LOI Date" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnViewDetail" runat="server" CommandName="ViewDetail" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="View Detail" 
                                            CssClass="btn btn-outline-primary btn-sm me-1" />
                                        <asp:Button ID="btnConfirmStock" runat="server" CommandName="ConfirmStock" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="Confirm Stock" 
                                            CssClass="btn btn-success btn-sm me-1" />
                                        <asp:Button ID="btnTransferInventory" runat="server" CommandName="TransferInventory" 
                                            CommandArgument='<%# Eval("LOIID") %>' Text="Transfer Inventory" 
                                            CssClass="btn btn-info btn-sm" />
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
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="loiDetailModalLabel">LOI Details</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <h6 class="fw-semibold">Basic Information</h6>
                            <hr />
                            <dl class="row">
                                <dt class="col-sm-4">LOI ID:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblLOIID" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Customer:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblCustomerName" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Dealer:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblDealerName" runat="server" /></dd>
                                
                                <dt class="col-sm-4">LOI Date:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblLOIDate" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Status:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblStatus" runat="server" /></dd>
                            </dl>
                        </div>
                        <div class="col-md-6">
                            <h6 class="fw-semibold">Car Details</h6>
                            <hr />
                            <dl class="row">
                                <dt class="col-sm-4">Car Model:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblCarModel" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Quantity:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblQuantity" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Agreed Price:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblAgreedPrice" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Discount:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblDiscount" runat="server" /></dd>
                                
                                <dt class="col-sm-4">Total Amount:</dt>
                                <dd class="col-sm-8"><asp:Label ID="lblTotalAmount" runat="server" /></dd>
                            </dl>
                        </div>
                    </div>
                    
                    <div class="row mt-3">
                        <div class="col-12">
                            <h6 class="fw-semibold">Additional Information</h6>
                            <hr />
                            <dl class="row">
                                <dt class="col-sm-3">Special Requests:</dt>
                                <dd class="col-sm-9"><asp:Label ID="lblSpecialRequests" runat="server" Text="No special requests" /></dd>
                                
                                <dt class="col-sm-3">Notes:</dt>
                                <dd class="col-sm-9"><asp:Label ID="lblNotes" runat="server" Text="No notes" /></dd>
                            </dl>
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