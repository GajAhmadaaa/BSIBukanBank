<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Logout.aspx.vb" Inherits="FinalProject.WebFormVB.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h2>Logout</h2>
                    <p>Anda telah berhasil logout dari sistem.</p>
                    <asp:Button ID="btnLogin" runat="server" Text="Login Kembali" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>