<%@ Page Title="Transfer Inventory" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferInventory.aspx.vb" Inherits="FinalProject.Webform.TransferInventory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h1 class="h3 mb-0">Inventory Transfer</h1>
            <p class="text-muted mb-0">Manage car transfers between dealers</p>
        </div>
        <div>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-outline-primary" OnClick="btnRefresh_Click" />
        </div>
    </div>

    <div class="row">
        <!-- Transfer Form Section -->
        <div class="col-lg-5 mb-4">
            <div class="card border-0 shadow-sm h-100">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">New Transfer</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="<%= ddlFromDealer.ClientID %>" class="form-label fw-medium">From Dealer</label>
                                <asp:DropDownList ID="ddlFromDealer" runat="server" CssClass="form-select" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            
                            <div class="mb-3">
                                <label for="<%= ddlToDealer.ClientID %>" class="form-label fw-medium">To Dealer</label>
                                <asp:DropDownList ID="ddlToDealer" runat="server" CssClass="form-select" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            
                            <div class="mb-3">
                                <label for="<%= ddlCar.ClientID %>" class="form-label fw-medium">Car Model</label>
                                <asp:DropDownList ID="ddlCar" runat="server" CssClass="form-select" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <!-- Stock Display -->
                            <div class="row mb-4">
                                <div class="col-6">
                                    <div class="card text-center bg-light">
                                        <div class="card-header small">Stock at Source</div>
                                        <div class="card-body py-2">
                                            <h4 class="card-title fw-bold mb-0"><asp:Label ID="lblFromDealerStock" runat="server" Text="-" /></h4>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="card text-center bg-light">
                                        <div class="card-header small">Stock at Destination</div>
                                        <div class="card-body py-2">
                                            <h4 class="card-title fw-bold mb-0"><asp:Label ID="lblToDealerStock" runat="server" Text="-" /></h4>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <div class="mb-4">
                        <label for="<%= txtQuantity.ClientID %>" class="form-label fw-medium">Quantity</label>
                        <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" CssClass="form-control" Text="1" min="1" required="required"></asp:TextBox>
                    </div>
                    
                    <div class="d-grid">
                        <asp:Button ID="btnTransfer" runat="server" Text="Perform Transfer" CssClass="btn btn-primary btn-lg" OnClick="btnTransfer_Click" />
                    </div>
                    
                    <div class="mt-3">
                        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>

        <!-- Transfer History Section -->
        <div class="col-lg-7">
            <div class="card border-0 shadow-sm h-100">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">Transfer History</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvTransferHistory" runat="server" 
                            AutoGenerateColumns="False" 
                            CssClass="table table-hover"
                            GridLines="None"
                            EmptyDataText="No transfer history found"
                            AllowPaging="true" 
                            PageSize="10"
                            OnPageIndexChanging="gvTransferHistory_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="InventoryTransferID" HeaderText="ID" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="FromDealerName" HeaderText="From Dealer" />
                                <asp:BoundField DataField="ToDealerName" HeaderText="To Dealer" />
                                <asp:BoundField DataField="CarModel" HeaderText="Car Model" />
                                <asp:BoundField DataField="Quantity" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MutationDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <PagerStyle CssClass="table-pager" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="table-primary" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>