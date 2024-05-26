Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.IO




Partial Class _LupaPassword
    Inherits System.Web.UI.Page
    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim SQL As String
    Private sConnstring As String = clsEnt.strKoneksi("connuq")

    Public Sub connectDB()
        Dim sConnString As String
        sConnString = clsEnt.StringOle
        mySqlCon = New Data.OleDb.OleDbConnection(sConnString)
    End Sub

    Private Function GetDataTbl(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            con.Open()
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                sda.SelectCommand = cmd
                Using dt As New DataTable()
                    sda.Fill(dt)
                    Return dt
                    sda.Dispose()
                End Using
            End Using
            con.Close()
        End Using
        cmd.Dispose()
    End Function

    Public Function IsValidEmail(email As String) As Boolean
        Dim emailRegex As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
        Return Regex.IsMatch(email, emailRegex)
    End Function

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim sTrue As DataTable
        Dim sTrue2 As DataTable
        Dim sTrue3 As DataTable

        Session("UserAuthentication") = Nothing

        If txtUserName.Text.Trim = "" Then
            Session("UserAuthentication") = Nothing
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Data Belum Di Validasi Keuangan...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim dtKoreksi As DataTable = GetDataTbl("select  Count(iddosen) as cnt  from dosen a  where  a.iddosen='" & Session("UserAuthentication") & "'")
        If CInt(dtKoreksi.Rows(0).Item("cnt")) > 0 Then

            Dim dtskswajibambil22 As DataTable = GetDataTbl("select count(emaildosen) as email from Dosen where LoginUserName='" & txtUserName.Text & "'")
            If CInt(dtskswajibambil22.Rows(0).Item("email")) > 0 Then
                sTrue3 = GetDataTbl("select emaildosen as email from Dosen where LoginUserName='" & txtUserName.Text & "'")
                Dim EmailDosen As String = sTrue3.Rows(0).Item("email")
                If EmailDosen = "" Then
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Email Belum Terisi Di Database...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    Exit Sub
                End If

            Else
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Email Tidak Terdaftar...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If



            If IsValidEmail("EmailDosen") Then
                sTrue = GetDataTbl("select emaildosen as email,LoginPassword as PasswordDosen, Universitas as Universitas from Dosen where LoginUserName='" & txtUserName.Text & "'")
                Dim EmailDosenKirim As String = sTrue.Rows(0).Item("email")
                Dim PasswordDosenKirim As String = sTrue.Rows(0).Item("PasswordDosen")
                Dim Universitas As String = sTrue.Rows(0).Item("Universitas")

                sTrue2 = GetDataTbl("select Nama as nama from KampusPMB where Universitas='" & Universitas & "'")
                Dim Dtitle As String = sTrue2.Rows(0).Item("nama")

                '    Dim Dtitle As String = clsEnt.ReturnOneField("select Nama from KampusPMB where KampusPMB='" & Universitas & "'", "", "", "Nama")
                Dim xGagal As String = ""
                Dim dmain2 As String = ""
                Dim suksesgagalemail As String = "Gagal"
                Dim suksesgagalemail2 As String = "Gagal"
                Dim prov As String = "gmail"
                Dim dmain As String = "smtp.gmail.com"
                Dim prtprov As Integer = 26
                Dim pssprov As String = ""
                Dim vDataEmail As DataTable = clsEnt.GetDataTabel("select providermail,passemail,emailppi,dmn,portprov,portprovgmail from KampusPMB where Universitas='" & Universitas & "'")

                If vDataEmail.Rows.Count > 0 Then
                    If vDataEmail.Rows(0).Item("providermail") = "xinergix" Then
                        dmain = vDataEmail.Rows(0).Item("dmn") ' clsEnt.ReturnOneField("select dmn from KampusPMB where KampusPMB=" & ConfigurationManager.AppSettings("UnivKey") & "", "", "", "dmn")
                        prtprov = vDataEmail.Rows(0).Item("portprov")
                        pssprov = "[XU;_]G9i@#]"
                        prov = vDataEmail.Rows(0).Item("providermail")
                    ElseIf vDataEmail.Rows(0).Item("providermail") = "gmail" Then
                        dmain = "smtp.gmail.com"
                        prtprov = vDataEmail.Rows(0).Item("portprovgmail")
                        pssprov = vDataEmail.Rows(0).Item("portprov")
                        prov = vDataEmail.Rows(0).Item("providermail")
                    End If
                End If


                If Universitas = "UQM" Then
                    dmain2 = "universitasquality.ac.id"
                Else
                    dmain2 = "uqb.ac.id"
                End If


                Dim angkas As String = Universitas
                Dim bodyemail As String = " Silahkan Login dengan " & vbNewLine & vbNewLine & " Password : " & PasswordDosenKirim & vbNewLine & " Password digenerate dengan otomatis "

                If prov = "xinergix" Then
                    suksesgagalemail = clsEnt.kirimemail(dmain, prtprov, bodyemail, "Password Dosen Quality " & Dtitle, "[XU;_]G9i@#]", "webadm@" & dmain, EmailDosenKirim)
                ElseIf prov = "gmail" Then
                    suksesgagalemail = clsEnt.kirimemail(dmain, prtprov, bodyemail, "Password Dosen Quality " & Dtitle, pssprov, vDataEmail.Rows(0).Item("emailppi"), EmailDosenKirim)
                End If
                'email kedua
                Dim mailbody As String = ""


                'Dim Univer As String = "UQB"
                '    Dim mailsubjek As String = "Universitas Quality " & IIf(angkas = Univer, "Berastagi", "") & " - MENUNGGU PEMBAYARAN UNTUK PENDAFTARAN MAHASISWA BARU - " & DateTime.Now.ToString("dd/MM/yyyy HH:mm")

                'If prov = "xinergix" Then
                '    suksesgagalemail2 = clsEnt.kirimemail(dmain, prtprov, mailbody, mailsubjek, "[XU;_]G9i@#]", "webadm@" & dmain, EmailDosenKirim)
                'ElseIf prov = "gmail" Then
                '    suksesgagalemail2 = clsEnt.kirimemail(dmain, prtprov, mailbody, mailsubjek, pssprov, vDataEmail.Rows(0).Item("emailppi"), EmailDosenKirim)
                'End If
                vDataEmail = Nothing
                '    Response.Redirect("Default.aspx")
            Else
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Data Sudah Terkirim Ke Email... Mohon Kembali Email', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If
        Else
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Data Belum Di Validasi Keuangan...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
        Exit Sub
        End If

    End Sub
    Public Sub notifikasi()
        Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Input Tanggal Tidak Boleh Kosong!', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

    End Sub
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        txtUserName.Focus()

        If Not IsPostBack Then

        End If

    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click
        Response.Redirect("Default.aspx")
    End Sub
End Class