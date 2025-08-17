Imports System.Web.Optimization
Imports System.Web.Routing

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Private Sub RegisterRoutes(routes As RouteCollection)
        ' Mendaftarkan route untuk halaman LOI Monitor
        routes.MapPageRoute("LOIMonitorRoute", "LOIMonitor", "~/LOIMonitor.aspx")
        
        ' Mendaftarkan route untuk halaman Transfer Inventory
        routes.MapPageRoute("TransferInventoryRoute", "TransferInventory", "~/TransferInventory.aspx")
        
        ' Memanggil konfigurasi route default
        RouteConfig.RegisterRoutes(routes)
    End Sub
End Class