<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Login.aspx.vb" Inherits="FinalProject.WebFormVB.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Sales</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-6 col-lg-4">
                    <div class="card">
                        <div class="card-header text-center">
                            <h4>Login Sales</h4>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label for="txtUsername" class="form-label">Username:</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Masukkan username"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPassword" class="form-label">Password:</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Masukkan password"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                            <asp:Label ID="lblMessage" runat="server" CssClass="mt-3" EnableViewState="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>