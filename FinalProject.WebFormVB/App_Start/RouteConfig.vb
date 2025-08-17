Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Public Module RouteConfig
    Sub RegisterRoutes(ByVal routes As RouteCollection)
        ' Register custom routes before enabling friendly URLs
        routes.MapPageRoute("DefaultRoute", "", "~/Default.aspx")
        routes.MapPageRoute("LOIMonitorRoute", "LOIMonitor", "~/LOIMonitor.aspx")
        routes.MapPageRoute("TransferInventoryRoute", "TransferInventory", "~/TransferInventory.aspx")
        routes.MapPageRoute("AboutRoute", "About", "~/About.aspx")
        routes.MapPageRoute("ContactRoute", "Contact", "~/Contact.aspx")
        
        Dim settings As FriendlyUrlSettings = New FriendlyUrlSettings() With {
            .AutoRedirectMode = RedirectMode.Permanent
        }
        routes.EnableFriendlyUrls(settings)
    End Sub
End Module
