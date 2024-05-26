Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl
    Private i_menu As HtmlGenericControl
    Private p_menu As HtmlGenericControl
    Private dtSelectMenu As DataTable
    Private MenuDataView As DataView
    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim SQL As String
    Dim mySqlReader As Data.OleDb.OleDbDataReader

    Public Sub connectDB()
        Dim sConnString As String
        sConnString = clsEnt.StringOle
        mySqlCon = New Data.OleDb.OleDbConnection(sConnString)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Session("UserAuthentication") = "Admin"
        'Dim dtSNAPS As DataTable = New DataTable()
        'dtSNAPS = clsEnt.GetDataTabel("SELECT MENU_ID, SM_OPEN, SM_MODIFY, SM_DELETE, ISNULL(SM_PRINT,'') AS SM_PRINT, LINKHREF, MENUWEB, GRUPMENU FROM SMUSR_PRIVS2 WHERE USERID = 'admin' ORDER BY GRUPMENU, MENU_ID;")

        'If dtSNAPS.Rows.Count > 0 Then
        '    Session("UserTable") = clsEnt.ConvertDataTabletoJSON(dtSNAPS)

        '    Response.Cookies("GMIUserID").Expires = DateTime.Now.AddDays(-1)
        '    Response.Cookies("GMIUserPassword").Expires = DateTime.Now.AddDays(-1)
        '    'ScriptManager.RegisterStartupScript(Me, [GetType](), "Popup", "showSweetAlert2('success', 'Successfully Logged In... Redirecting...', '1000');", True)
        '    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Redirect", "setTimeout(function () {  window.location.href ='EnterpriseHome.aspx' }, 1000);", True)
        'End If

        If Session("UserAuthentication") Is Nothing Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowStatus", "setTimeout(function () {  callAlert2('info', 'Session has Exppired, Please Re-Login', '2000'); }, 10);", True)
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='enterprisehome.aspx' }, 2000);", True)
        Else
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            ancUser.InnerText = Session("UserAuthentication").ToString()

            HideMenu()
        End If
    End Sub


#Region "POP UP"
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#End Region

    Protected Sub HideMenu()
        Dim sUsr As String = Session("UserAuthentication")
        Dim sBos As String = Session("tokenboss")
        Dim Akademik As String = "Biro Akademik"
        Dim dDataTable As DataTable = clsEnt.GetDataTabel("select * from SMUSR_PRIVS2Petugas WHERE USERID='" & sUsr & "'")
        '   lblRegisterName.Text = clsEnt.ReturnOneField("Select Nama from GLFSYS", "", "", "NAMA")
        Dim hide_Data As Boolean = True

        'Dim hide_Data As Boolean = True
        'Dim hide_Accounting As Boolean = True
        'Dim hide_Transaction As Boolean = True
        'Dim hide_Report As Boolean = True
        If dDataTable.Rows(0).Item("Divisi") = "Biro Petugas" Then

            'Master Tabe;
            dtSelectMenu = Nothing
            MenuDataView = Nothing
            MenuDataView = New DataView(dDataTable, "GRUPMENU = 'DataMaster'", "MENU_ID ASC", DataViewRowState.CurrentRows)
            dtSelectMenu = MenuDataView.ToTable()
            If dtSelectMenu.Rows.Count > 0 Then
                hide_Data = False

                For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                    If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                        li_menu = New HtmlGenericControl("li")
                        anchor_menu = New HtmlGenericControl("a")
                        i_menu = New HtmlGenericControl("i")
                        p_menu = New HtmlGenericControl("p")

                        li_menu.Attributes.Add("class", "nav-item")
                        ulMAster.Controls.Add(li_menu)
                        anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                        anchor_menu.Attributes.Add("class", "nav-link")
                        li_menu.Controls.Add(anchor_menu)
                        i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                        p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                        anchor_menu.Controls.Add(i_menu)
                        anchor_menu.Controls.Add(p_menu)
                    End If
                Next
            Else
                liMaster.Style.Add("display", "none")
            End If


            'Data Pembelian
            dtSelectMenu = Nothing
            MenuDataView = Nothing
            MenuDataView = New DataView(dDataTable, "GRUPMENU = 'DataBeli'", "MENU_ID ASC", DataViewRowState.CurrentRows)
            dtSelectMenu = MenuDataView.ToTable()
            If dtSelectMenu.Rows.Count > 0 Then
                hide_Data = False

                For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                    If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                        li_menu = New HtmlGenericControl("li")
                        anchor_menu = New HtmlGenericControl("a")
                        i_menu = New HtmlGenericControl("i")
                        p_menu = New HtmlGenericControl("p")

                        li_menu.Attributes.Add("class", "nav-item")
                        ulSysteminputPembelian.Controls.Add(li_menu)
                        anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                        anchor_menu.Attributes.Add("class", "nav-link")
                        li_menu.Controls.Add(anchor_menu)
                        i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                        p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                        anchor_menu.Controls.Add(i_menu)
                        anchor_menu.Controls.Add(p_menu)
                    End If
                Next
            Else
                liSysteminputPembelian.Style.Add("display", "none")
            End If

            'Data Penjualan
            dtSelectMenu = Nothing
            MenuDataView = Nothing
            MenuDataView = New DataView(dDataTable, "GRUPMENU = 'DataJual'", "MENU_ID ASC", DataViewRowState.CurrentRows)
            dtSelectMenu = MenuDataView.ToTable()
            If dtSelectMenu.Rows.Count > 0 Then
                hide_Data = False

                For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                    If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                        li_menu = New HtmlGenericControl("li")
                        anchor_menu = New HtmlGenericControl("a")
                        i_menu = New HtmlGenericControl("i")
                        p_menu = New HtmlGenericControl("p")

                        li_menu.Attributes.Add("class", "nav-item")
                        ulInputPenjualan.Controls.Add(li_menu)
                        anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                        anchor_menu.Attributes.Add("class", "nav-link")
                        li_menu.Controls.Add(anchor_menu)
                        i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                        p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                        anchor_menu.Controls.Add(i_menu)
                        anchor_menu.Controls.Add(p_menu)
                    End If
                Next
            Else
                liInputPenjualan.Style.Add("display", "none")
            End If

            'Angsuran
            dtSelectMenu = Nothing
            MenuDataView = Nothing
            MenuDataView = New DataView(dDataTable, "GRUPMENU = 'DataAngsuran'", "MENU_ID ASC", DataViewRowState.CurrentRows)
            dtSelectMenu = MenuDataView.ToTable()
            If dtSelectMenu.Rows.Count > 0 Then
                hide_Data = False

                For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                    If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                        li_menu = New HtmlGenericControl("li")
                        anchor_menu = New HtmlGenericControl("a")
                        i_menu = New HtmlGenericControl("i")
                        p_menu = New HtmlGenericControl("p")

                        li_menu.Attributes.Add("class", "nav-item")
                        ulAngsuran.Controls.Add(li_menu)
                        anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                        anchor_menu.Attributes.Add("class", "nav-link")
                        li_menu.Controls.Add(anchor_menu)
                        i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                        p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                        anchor_menu.Controls.Add(i_menu)
                        anchor_menu.Controls.Add(p_menu)
                    End If
                Next
            Else
                liAngsuran.Style.Add("display", "none")
            End If
        End If

        'Report
        dtSelectMenu = Nothing
        MenuDataView = Nothing
        MenuDataView = New DataView(dDataTable, "GRUPMENU = 'Report'", "MENU_ID ASC", DataViewRowState.CurrentRows)
        dtSelectMenu = MenuDataView.ToTable()
        If dtSelectMenu.Rows.Count > 0 Then
            hide_Data = False

            For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                    li_menu = New HtmlGenericControl("li")
                    anchor_menu = New HtmlGenericControl("a")
                    i_menu = New HtmlGenericControl("i")
                    p_menu = New HtmlGenericControl("p")

                    li_menu.Attributes.Add("class", "nav-item")
                    ulreport.Controls.Add(li_menu)
                    anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                    anchor_menu.Attributes.Add("class", "nav-link")
                    li_menu.Controls.Add(anchor_menu)
                    i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                    p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                    anchor_menu.Controls.Add(i_menu)
                    anchor_menu.Controls.Add(p_menu)
                End If
            Next
        Else
            lireport.Style.Add("display", "none")
        End If

        'Sign Out
        dtSelectMenu = Nothing
            MenuDataView = Nothing
            MenuDataView = New DataView(dDataTable, "GRUPMENU = 'SignOut'", "MENU_ID ASC", DataViewRowState.CurrentRows)
            dtSelectMenu = MenuDataView.ToTable()
        If dtSelectMenu.Rows.Count > 0 Then
            hide_Data = False

            For i As Integer = 0 To dtSelectMenu.Rows.Count - 1
                If dtSelectMenu.Rows(i).Field(Of String)("SM_OPEN") = "R" Then
                    li_menu = New HtmlGenericControl("li")
                    anchor_menu = New HtmlGenericControl("a")
                    i_menu = New HtmlGenericControl("i")
                    p_menu = New HtmlGenericControl("p")

                    li_menu.Attributes.Add("class", "nav-item")
                    ulOut.Controls.Add(li_menu)
                    anchor_menu.Attributes.Add("href", dtSelectMenu.Rows(i).Field(Of String)("LINKHREF"))
                    anchor_menu.Attributes.Add("class", "nav-link")
                    li_menu.Controls.Add(anchor_menu)
                    i_menu.Attributes.Add("class", "far fa-circle nav-icon")
                    p_menu.InnerText = dtSelectMenu.Rows(i).Field(Of String)("MENUWEB")
                    anchor_menu.Controls.Add(i_menu)
                    anchor_menu.Controls.Add(p_menu)
                End If
            Next
        Else
            liOut.Style.Add("display", "none")
        End If



    End Sub



End Class