<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.vb" Inherits="FinalProject.Webform.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <h1 class="h2 mb-4">Contact Us</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-8">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-white py-3">
                        <h5 class="card-title mb-0 fw-semibold">Get in Touch</h5>
                    </div>
                    <div class="card-body">
                        <p class="mb-4">Have questions about our services or need assistance? Reach out to our team and we'll get back to you as soon as possible.</p>
                        
                        <form>
                            <div class="mb-3">
                                <label for="name" class="form-label fw-medium">Name</label>
                                <input type="text" class="form-control" id="name" placeholder="Enter your name">
                            </div>
                            
                            <div class="mb-3">
                                <label for="email" class="form-label fw-medium">Email Address</label>
                                <input type="email" class="form-control" id="email" placeholder="Enter your email">
                            </div>
                            
                            <div class="mb-3">
                                <label for="subject" class="form-label fw-medium">Subject</label>
                                <input type="text" class="form-control" id="subject" placeholder="Enter subject">
                            </div>
                            
                            <div class="mb-4">
                                <label for="message" class="form-label fw-medium">Message</label>
                                <textarea class="form-control" id="message" rows="5" placeholder="Enter your message"></textarea>
                            </div>
                            
                            <div class="d-grid">
                                <button type="submit" class="btn btn-primary btn-lg">Send Message</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            
            <div class="col-lg-4">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-primary text-white py-3">
                        <h5 class="card-title mb-0">Contact Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="d-flex align-items-start mb-4">
                            <div class="flex-shrink-0">
                                <i class="fas fa-map-marker-alt fa-lg text-primary"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-1">Our Location</h6>
                                <p class="mb-0">One Microsoft Way</p>
                                <p class="mb-0">Redmond, WA 98052-6399</p>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-start mb-4">
                            <div class="flex-shrink-0">
                                <i class="fas fa-phone fa-lg text-success"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-1">Phone Numbers</h6>
                                <p class="mb-1"><strong>Support:</strong> 425.555.0100</p>
                                <p class="mb-0"><strong>Sales:</strong> 425.555.0101</p>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-start mb-4">
                            <div class="flex-shrink-0">
                                <i class="fas fa-envelope fa-lg text-info"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-1">Email Addresses</h6>
                                <p class="mb-1"><strong>General:</strong> <a href="mailto:info@example.com">info@example.com</a></p>
                                <p class="mb-1"><strong>Support:</strong> <a href="mailto:support@example.com">support@example.com</a></p>
                                <p class="mb-0"><strong>Sales:</strong> <a href="mailto:sales@example.com">sales@example.com</a></p>
                            </div>
                        </div>
                        
                        <div class="d-flex align-items-start">
                            <div class="flex-shrink-0">
                                <i class="fas fa-clock fa-lg text-warning"></i>
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h6 class="mb-1">Business Hours</h6>
                                <p class="mb-1"><strong>Monday - Friday:</strong> 9:00 AM - 6:00 PM</p>
                                <p class="mb-0"><strong>Saturday - Sunday:</strong> Closed</p>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white py-3">
                        <h5 class="card-title mb-0 fw-semibold">Follow Us</h5>
                    </div>
                    <div class="card-body">
                        <div class="d-flex justify-content-around">
                            <a href="#" class="text-decoration-none">
                                <i class="fab fa-facebook-f fa-2x text-primary"></i>
                            </a>
                            <a href="#" class="text-decoration-none">
                                <i class="fab fa-twitter fa-2x text-info"></i>
                            </a>
                            <a href="#" class="text-decoration-none">
                                <i class="fab fa-linkedin-in fa-2x text-primary"></i>
                            </a>
                            <a href="#" class="text-decoration-none">
                                <i class="fab fa-instagram fa-2x text-danger"></i>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-12">
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white py-3">
                        <h5 class="card-title mb-0 fw-semibold">Our Office</h5>
                    </div>
                    <div class="card-body p-0">
                        <!-- Placeholder for map -->
                        <div class="bg-light" style="height: 300px; display: flex; align-items: center; justify-content: center;">
                            <p class="text-muted mb-0">Interactive Map Placeholder</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>