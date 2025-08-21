<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.vb" Inherits="FinalProject.Webform.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <h1 class="h2 mb-4">About karolin</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-8">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-body">
                        <h5 class="card-title">Our Mission</h5>
                        <p class="card-text">At karolin, we are committed to revolutionizing automotive inventory management through innovative technology and exceptional service. Our mission is to empower automotive dealers with the tools they need to efficiently manage their vehicle inventory, streamline operations, and enhance customer satisfaction.</p>

                        <h5 class="card-title mt-4">Our Story</h5>
                        <p class="card-text">Founded in 2023, karolin emerged from a need to address the challenges faced by automotive dealers in managing multi-location inventory. Our team of industry experts and technology professionals came together to create a solution that simplifies complex inventory processes while providing real-time insights and analytics.</p>

                        <h5 class="card-title mt-4">Our Values</h5>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item"><i class="fas fa-check-circle text-success me-2"></i><strong>Innovation:</strong> Continuously improving our platform with cutting-edge technology</li>
                            <li class="list-group-item"><i class="fas fa-check-circle text-success me-2"></i><strong>Integrity:</strong> Maintaining the highest standards of honesty and transparency</li>
                            <li class="list-group-item"><i class="fas fa-check-circle text-success me-2"></i><strong>Customer Focus:</strong> Putting our customers' needs at the center of everything we do</li>
                            <li class="list-group-item"><i class="fas fa-check-circle text-success me-2"></i><strong>Excellence:</strong> Striving for excellence in every aspect of our business</li>
                            <li class="list-group-item"><i class="fas fa-check-circle text-success me-2"></i><strong>Collaboration:</strong> Working together to achieve shared success</li>
                        </ul>
                    </div>
                </div>
            </div>
            
            <div class="col-lg-4">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-primary text-white py-3">
                        <h5 class="card-title mb-0">Key Features</h5>
                    </div>
                    <div class="card-body">
                        <div class="d-flex align-items-center mb-3">
                            <div class="flex-shrink-0">
                                <i class="fas fa-sync-alt fa-2x text-primary"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-0">Real-time Sync</h6>
                                <small class="text-muted">Instant inventory updates across all locations</small>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-center mb-3">
                            <div class="flex-shrink-0">
                                <i class="fas fa-mobile-alt fa-2x text-success"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-0">Mobile Access</h6>
                                <small class="text-muted">Manage inventory on-the-go with our mobile app</small>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-center mb-3">
                            <div class="flex-shrink-0">
                                <i class="fas fa-chart-line fa-2x text-info"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-0">Advanced Analytics</h6>
                                <small class="text-muted">Gain insights with comprehensive reporting</small>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-center">
                            <div class="flex-shrink-0">
                                <i class="fas fa-shield-alt fa-2x text-warning"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-0">Enterprise Security</h6>
                                <small class="text-muted">Military-grade encryption for your data</small>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="card border-0 shadow-sm">
                    <div class="card-body text-center">
                        <h5 class="card-title">Ready to Get Started?</h5>
                        <p class="card-text">Join thousands of dealers who trust karolin for their inventory management needs.</p>
                        <a href="Contact.aspx" class="btn btn-primary">Contact Us</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>