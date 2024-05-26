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


Partial Class _Default
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

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim username As String = txtUserName.Text.Trim
        Dim pwd As String = txtPassword.Text.Trim
        Dim strConn As String = clsEnt.strKoneksi("connuq")
        Dim Conn As New SqlConnection(strConn)

        Dim dtKoreksi As DataTable = GetDataTbl("select  Count(IDKaryawan) as cnt  from TblPetugas a  where  a.loginusername='" & username & "' and a.loginpassword='" & txtPassword.Text & "'")
        If CInt(dtKoreksi.Rows(0).Item("cnt")) > 0 Then
            Try
                Conn.Open()
                'ini part dari query setelah open() strconnection nya
                Dim sqlUserName As String
                sqlUserName = "SELECT LOGINUSERNAME as username,LOGINPASSWORD as pwd FROM Tblpetugas WHERE (LOGINUSERNAME = @UserName AND LOGINPASSWORD = @Password )" 'tentukan mau dari tabel mana dia cek userid sama passwordnya sesuai variabel diatas
                Dim com As New SqlCommand(sqlUserName, Conn)
                com.Parameters.AddWithValue("@UserName", username)
                com.Parameters.AddWithValue("@Password", pwd)

                Dim CurrentName As String

                CurrentName = CStr(com.ExecuteScalar)

                If CurrentName <> "" Then
                    Dim dtM As DataTable = clsEnt.GetDataTabel("SELECT * FROM SMUSR_PRIVS2Petugas WHERE USERID = '" & username & "' order by GRUPMENU,MENU_ID")
                    If dtM.Rows.Count > 0 Then
                        InsertMenu(Nothing, Nothing)
                        InsertHeaderMenu(Nothing, Nothing)
                        Session("UserAuthentication") = username
                        dtM.Dispose()
                        Response.Redirect("EnterpriseHome.aspx")

                    Else
                        InsertHeaderMenu(Nothing, Nothing)
                        InsertMenu(Nothing, Nothing)
                        Session("UserAuthentication") = username
                        dtM.Dispose()
                        Response.Redirect("EnterpriseHome.aspx")
                    End If
                Else 'failed password salah 
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), UniqueID, "javascript:pesanError('Incorrect Username or Password combination ! ');", True)
                End If
            Finally
                Conn.Close()
                Conn.Dispose()
                SqlConnection.ClearPool(Conn)
            End Try
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
            If Request.Cookies("UqUserName") IsNot Nothing AndAlso Request.Cookies("UqPassword") IsNot Nothing Then
                txtUserName.Text = Request.Cookies("UqUserName").Value
                txtPassword.Attributes("value") = Request.Cookies("UqPassword").Value
                'customCheck.Checked = True
                '      btnSubmit.Focus()
            End If
        End If

    End Sub

    Public Sub InsertMenu(sender As Object, e As System.EventArgs)
        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction
        mySqlCmd.CommandTimeout = 300
        Try

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "USP_InsertMenuPetugas"
            mySqlCmd.Parameters.AddWithValue("@Userid", SqlDbType.VarChar).Value = txtUserName.Text

            mySqlCmd.ExecuteNonQuery()
            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            '   mySqlCon.Close()
        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            mySqlCmd.Parameters.Clear()

        End Try
    End Sub

    Public Sub InsertHeaderMenu(sender As Object, e As System.EventArgs)
        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction
        mySqlCmd.CommandTimeout = 300
        Try

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "USP_InsertHeaderMenuPetugas"

            mySqlCmd.ExecuteNonQuery()
            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            '   mySqlCon.Close()
        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            mySqlCmd.Parameters.Clear()

        End Try
    End Sub
    Protected Sub DeleteMyFile(sender As Object, e As EventArgs)


        Dim ketstatus As String = "T"
        Dim ketstatus2 As String = "F"

        If txtUserName.Text = "" Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Ketikan Dulu User Id ...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
        End If
    End Sub

End Class