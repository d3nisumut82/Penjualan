
Partial Class LogOut
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'lastuser.Text = Session("UserAuthentication")

        'xxxx Response.Cache.SetNoStore()
        'xxxx Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'xxxx Response.Write(Session("UserAuthentication"))
        'HttpContext.Current.Session.Clear()
        'HttpContext.Current.Session.Abandon()
        'HttpContext.Current.Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))

        'HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'HttpContext.Current.Response.Cache.SetNoServerCaching()
        'HttpContext.Current.Response.Cache.SetNoStore()

        'Session("UserAuthentication") = Nothing
        'Session("gCabang") = Nothing
        'Session("tokenboss") = Nothing
        'Session.Abandon()


        Session.Abandon()
        Session.Clear()
        Session("UserAuthentication") = Nothing
        Session("UserPrivs") = Nothing
        'Session.Clear()
        LogOffTime.Text = Format(DateTime.Now, "dd MMM yyyy HH:mm")
        'System.Threading.Thread.Sleep(10000)
        'Response.Redirect("default.aspx")
         
    End Sub
End Class
