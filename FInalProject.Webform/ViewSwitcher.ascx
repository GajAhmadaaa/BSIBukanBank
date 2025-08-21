<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewSwitcher.ascx.vb" Inherits="FinalProject.Webform.ViewSwitcher" %>
<div id="viewSwitcher" class="alert alert-info alert-dismissible fade show" role="alert">
    <i class="fas fa-mobile-alt me-2"></i>
    <%: CurrentView %> view | <a href="<%: SwitchUrl %>" class="alert-link">Switch to <%: AlternateView %></a>
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
</div>