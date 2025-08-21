<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="FinalProject.Webform._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron bg-primary text-white rounded p-5 mb-4">
        <div class="container">
            <h1 class="display-4">Welcome to karolin</h1>
            <p class="lead">Your trusted partner for automotive solutions and inventory management.</p>
            <hr class="my-4 bg-light">
            <p>Manage your car inventory efficiently with our comprehensive platform.</p>
            <a class="btn btn-light btn-lg" href="TransferInventory.aspx" role="button">Get Started</a>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title"><i class="fas fa-exchange-alt text-primary me-2"></i>Inventory Transfer</h5>
                        <p class="card-text">Easily transfer vehicles between dealers with our streamlined process.</p>
                        <a href="TransferInventory.aspx" class="btn btn-outline-primary">Manage Transfers</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title"><i class="fas fa-clipboard-list text-success me-2"></i>LOI Monitoring</h5>
                        <p class="card-text">Track Letter of Intent statuses and manage customer orders effectively.</p>
                        <a href="LOIMonitor.aspx" class="btn btn-outline-success">View LOIs</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title"><i class="fas fa-chart-bar text-info me-2"></i>Reporting</h5>
                        <p class="card-text">Access detailed reports on inventory movements and sales performance.</p>
                        <a href="#" class="btn btn-outline-info">View Reports</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-4">
            <div class="col-12">
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white py-3">
                        <h5 class="card-title mb-0 fw-semibold">About Our Platform</h5>
                    </div>
                    <div class="card-body">
                        <p>karolin provides a comprehensive solution for automotive dealers to manage their inventory efficiently. Our platform offers:</p>
                        <ul>
                            <li>Real-time inventory tracking across multiple dealerships</li>
                            <li>Streamlined transfer processes between locations</li>
                            <li>Automated Letter of Intent management</li>
                            <li>Comprehensive reporting and analytics</li>
                            <li>Secure and scalable cloud-based infrastructure</li>
                        </ul>
                        <p class="mb-0">Our system helps dealers reduce costs, improve efficiency, and enhance customer satisfaction through better inventory management.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>