<%@ Page Title="Dealer Inventory Management" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DealerInventory.aspx.vb" Inherits="FinalProject.Webform.DealerInventory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
            <h1 class="h3 mb-0">Dealer Inventory Management</h1>
            <p class="text-muted mb-0">Manage car inventory for each dealership</p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white py-3">
                    <h5 class="card-title mb-0 fw-semibold">Select Dealer</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label for="ddlDealers" class="form-label">Dealer:</label>
                                <asp:DropDownList ID="ddlDealers" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDealers_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlInventory" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0 fw-semibold">Inventory for <asp:Label ID="lblDealerName" runat="server" CssClass="text-primary"></asp:Label></h5>
                        <asp:Button ID="btnAddCar" runat="server" Text="Add Car to Inventory" CssClass="btn btn-primary" OnClick="btnAddCar_Click" />
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-hover" GridLines="None" EmptyDataText="No inventory found for this dealer"
                                AllowPaging="true" PageSize="10" OnPageIndexChanging="gvInventory_PageIndexChanging"
                                OnRowEditing="gvInventory_RowEditing" OnRowCancelingEdit="gvInventory_RowCancelingEdit"
                                OnRowUpdating="gvInventory_RowUpdating" OnRowDeleting="gvInventory_RowDeleting"
                                DataKeyNames="DealerInventoryID">
                                <Columns>
                                    <asp:BoundField DataField="Model" HeaderText="Car Model" ItemStyle-CssClass="fw-bold" />
                                    <asp:TemplateField HeaderText="Stock">
                                        <ItemTemplate>
                                            <%# Eval("Stock") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtStock" runat="server" Text='<%# Bind("Stock") %>' CssClass="form-control" Width="80px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <%# Convert.ToDecimal(Eval("Price")).ToString("C", New System.Globalization.CultureInfo("id-ID")) %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Price") %>' CssClass="form-control" Width="120px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount %">
                                        <ItemTemplate>
                                            <%# Eval("DiscountPercent") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDiscount" runat="server" Text='<%# Bind("DiscountPercent") %>' CssClass="form-control" Width="80px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fee %">
                                        <ItemTemplate>
                                            <%# Eval("FeePercent") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFee" runat="server" Text='<%# Bind("FeePercent") %>' CssClass="form-control" Width="80px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-outline-primary btn-sm me-1" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-outline-danger btn-sm" 
                                                OnClientClick="return confirm('Are you sure you want to remove this car from the inventory?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-success btn-sm me-1" />
                                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-secondary btn-sm" />
                                        </EditItemTemplate>
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
    </asp:Panel>

    <!-- Add/Edit Car Modal -->
    <div class="modal fade" id="carModal" tabindex="-1" aria-labelledby="carModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="carModalLabel">Add Car to Inventory</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfDealerInventoryID" runat="server" Value="0" />
                    <div class="mb-3">
                        <label for="ddlCars" class="form-label">Car Model:</label>
                        <asp:DropDownList ID="ddlCars" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label for="txtStock" class="form-label">Stock:</label>
                                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" min="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label for="txtPrice" class="form-label">Price (Rp):</label>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" min="0" step="1000"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label for="txtDiscount" class="form-label">Discount (%):</label>
                                <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" TextMode="Number" min="0" max="100" step="0.1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label for="txtFee" class="form-label">Fee (%):</label>
                                <asp:TextBox ID="txtFee" runat="server" CssClass="form-control" TextMode="Number" min="0" max="100" step="0.1"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveCar" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveCar_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Literal for messages -->
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
</asp:Content>