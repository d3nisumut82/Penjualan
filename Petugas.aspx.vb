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
Imports System.IO

Imports System.Text
Imports System.Drawing

Imports System
Imports System.Collections.Generic
Imports System.Linq

Partial Class Petugas
    Inherits System.Web.UI.Page

#Region "Variables & Connect DB"

    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim SQL As String
    Dim DataLineItemAccount As DataTable
    Dim editLineItemAccount As Integer
    Private sConnstring As String = clsEnt.strKoneksi("connuq")
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl
    Private dtSelectMenu As DataTable
    Dim mySqlReader As Data.OleDb.OleDbDataReader
    Private MenuDataView As DataView

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

#End Region

#Region "Page Load"
    'Set Ambil Data Dari List SetDefaultBank
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim sUsr As String = Session("UserAuthentication")
        If sUsr = "" Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
        Else


            'txtModalNamaAccount.Attributes.Add("Readonly", "Readonly")

            'SET JAVASCRIPT POSTBACK DETECTION
            Page.ClientScript.RegisterClientScriptBlock([GetType](), "isPostBack", String.Format("var isPostback = {0};", IsPostBack.ToString().ToLower()), True)

            If Not IsPostBack Then
                txtidkaryawan.Text = HttpUtility.UrlDecode(Request.QueryString("idKaryawan"))
                If Not String.IsNullOrEmpty(txtidkaryawan.Text.Trim()) Then
                    QueryAccount(txtidkaryawan.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    txtidkaryawan.Attributes.Add("readonly", "readonly")
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If
                Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("select Nama as Nama from TblJabatan order by Kode")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        ddljabat.DataSource = cmd.ExecuteReader()
                        ddljabat.DataTextField = "Nama"
                        ddljabat.DataValueField = "Nama"
                        ddljabat.DataBind()
                        con.Close()
                    End Using
                End Using
                ddljabat.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Pilih Jabatan...", ""))

                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("select nama as nama from TblJenisKelamin order by IdPrimary")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        ddljeniskelamin.DataSource = cmd.ExecuteReader()
                        ddljeniskelamin.DataTextField = "nama"
                        ddljeniskelamin.DataValueField = "nama"
                        ddljeniskelamin.DataBind()
                        con.Close()
                    End Using
                End Using
                ddljeniskelamin.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Pilih Jenis Kelamin...", ""))
            End If
            txtidkaryawan.Attributes.Add("readonly", "readonly")
        End If
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS
        If String.IsNullOrEmpty(txtidkaryawan.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Kampus Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnama.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtalamat.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txttempat.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txthp.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddljeniskelamin.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Kelamin Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)

        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidkaryawan.Text.Trim(), "idKaryawan", "TblPetugas") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblPetugas (IDKaryawan,NAMA,ALAMAT,TEMPATLAHIR,TGLLAHIR,LOGINUSERNAME,LOGINPASSWORD,STATUSJABATAN,JENISKELAMIN,HP,TGLGABUNG,IDCABANG,IDUNIT,LastUpdate,UseridUpdate) VALUES ('" _
                    & txtidkaryawan.Text & "', '" & txtnama.Text & "', '" & txtalamat.Text & "','" & txttempat.Text & "','" & clsEnt.CDateME5(dtpTgllahir.Value) & "', '" & txtuser.Text & "', '" & txtpassword.Text & "', '" & ddljabat.Text & "', '" & ddljeniskelamin.Text & "', '" & txthp.Text & "', '" & clsEnt.CDateME5(txttglgabung.Value) & "','" & ascabang & "','" & asunit & "',Getdate(),'" & strUID & "'); "
                mySqlCmd.CommandText = mySqlCmd.CommandText & "insert into PetugasPic(IDKaryawan) Values ('" & txtidkaryawan.Text & "') ;"

                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                Response.Redirect("PetugasList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblPetugas SET Nama = '" & txtnama.Text & "',Alamat = '" & txtalamat.Text & "',TEMPATLAHIR = '" & txttempat.Text & "',TglLahir ='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',HP = '" & txthp.Text & "',LOGINUSERNAME = '" & txtuser.Text & "',LOGINPASSWORD = '" & txtpassword.Text & "', TGLGABUNG='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',IDCABANG='" & ddlcabang.Text & "', IDUNIT='" & ddlunit.Text & "' WHERE idKaryawan= '" & txtidkaryawan.Text.Trim() & "'; "
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                UploadPhoto()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                QueryAccount(txtidkaryawan.Text.Trim(), True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                ClearForm()
                Response.Redirect("PetugasList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("PetugasList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidkaryawan.Text = ""
        txtnama.Text = ""
        txtalamat.Text = ""
        txttempat.Text = ""
        txtuser.Text = ""
        txtpassword.Text = ""
        ddljabat.Text = ""
        ddljeniskelamin.Text = ""
        txthp.Text = ""
        ddlcabang.Text = ""
        ddlunit.Text = ""

    End Sub



    Private Function CheckAdaData(a_Str As String, str_Field As String, str_Tbl As String) As Boolean
        Return Convert.ToBoolean(clsEnt.ReturnFieldOne("SELECT COUNT(*) AS CNT FROM " & str_Tbl & " WHERE " & str_Field & " = '" & a_Str & "'; ", "", "", "CNT"))
    End Function


    Private Sub QueryAccount(ByVal a_Str As String, showmsg As Boolean)

        connectDB()

        Try
            mySqlCon.Open()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "setTimeout(function () {  callAlert2('error', 'Error Connecting to Database', '2000'); }, 10);", True)
            Exit Sub
        Finally
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand
            command = New System.Data.OleDb.OleDbCommand("SELECT IDKaryawan AS IDKaryawan, NAMA as NAMA, ALAMAT as ALAMAT,TEMPATLAHIR AS TEMPATLAHIR, CONVERT(VARCHAR(10),TGLLAHIR,103) AS TGLLAHIR, LOGINUSERNAME AS LOGINUSERNAME, LOGINPASSWORD AS LOGINPASSWORD, STATUSJABATAN AS STATUSJABATAN,JENISKELAMIN AS JENISKELAMIN, HP AS HP, CONVERT(VARCHAR(10),TGLGABUNG,103)  AS TGLGABUNG, IDCABANG AS IDCABANG, IDUNIT AS IDUNIT FROM TblPetugas WHERE idKaryawan = '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidkaryawan.Text = mySqlReader("IDKaryawan").ToString()
                txtnama.Text = mySqlReader("Nama").ToString()
                txtalamat.Text = mySqlReader("Alamat").ToString()
                txttempat.Text = mySqlReader("TEMPATLAHIR").ToString()
                dtpTgllahir.Value = mySqlReader("TGLLAHIR").ToString()
                txttglgabung.Value = mySqlReader("TGLGABUNG").ToString()
                txtuser.Text = mySqlReader("LOGINUSERNAME").ToString()
                txtpassword.Text = mySqlReader("Nama").ToString()
                ddljabat.Text = mySqlReader("STATUSJABATAN").ToString()
                ddljeniskelamin.Text = mySqlReader("JENISKELAMIN").ToString()
                txthp.Text = mySqlReader("HP").ToString()
                ddlcabang.Text = mySqlReader("IDCABANG").ToString()
                ddlunit.Text = mySqlReader("IDUNIT").ToString()
                'txtid.Attributes.Add("readonly", "readonly")
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidkaryawan.Text = ""
                txtnama.Text = ""
                txtalamat.Text = ""
                txttempat.Text = ""
                txtuser.Text = ""
                txtpassword.Text = ""
                ddljabat.Text = ""
                ddljeniskelamin.Text = ""
                txthp.Text = ""
                ddlcabang.Text = ""
                ddlunit.Text = ""
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()

                'txtid.Attributes.Remove("readonly")
            End If

        End Try
    End Sub



#Region "POP UP"
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#End Region

    Public Sub NOOTOMATIS()
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)Idkaryawan + 1 as nnn from TblPetugas order by Idkaryawan desc ", "", "", "nnn", "connuq")
        txtidkaryawan.Text = IIf(NO = "", "00001", NO)
    End Sub

    Protected Sub btnNew2_Click(sender As Object, e As System.EventArgs) Handles btnNew2.Click
        'txtid.Text = ""
        'txtlokasi.Text = ""
    End Sub

    Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs) Handles lnkDelete.Click
        ''CHECK REQUIRED FIELDS

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandText = "DELETE FROM TblPetugas WHERE idKaryawan = '" & txtidkaryawan.Text.Trim() & "'; "
            mySqlCmd.ExecuteNonQuery()

            Transaction.Commit()
            Session("Mode") = "Edit"
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Deleted', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            btnNew_Click(Nothing, Nothing)

        Catch ex As Exception
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error deleting data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            mySqlCmd.Parameters.Clear()
            Response.Redirect("PetugasList.aspx")
        End Try
    End Sub

    Public Sub UploadPhoto()
        Dim bytes As Byte()
        Using br As BinaryReader = New BinaryReader(FileUpload1.PostedFile.InputStream)
            bytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength)
        End Using

        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using conn As SqlConnection = New SqlConnection(constr)
            Dim Statuspic As String = "T"
            Dim query As String = "update PetugasPIC set PICPATHNAME=@Data,Data=@Data,Status=@Stattusket WHERE idkaryawan= '" & txtidkaryawan.Text & "' ; "
            Using cmd As SqlCommand = New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Data", bytes)
                cmd.Parameters.Add("@Stattusket", SqlDbType.VarChar).Value = Statuspic
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
    End Sub

    Protected Sub bntUpload_Click(sender As Object, e As System.EventArgs) Handles bntUpload.Click
        If FileUpload1.HasFile And FileUpload1.PostedFile.ContentLength > 256000 Then
            Dim CDTblWisuda As DataTable = clsEnt.GetDataTabel("select IDKaryawan as LoginUserName from Tblpetugas where IDKaryawan = '" & txtidkaryawan.Text & "' ")

            If CDTblWisuda.Rows.Count > 0 Then
                Dim FileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
                Dim Extension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
                Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")
                Dim FilePath As String = Server.MapPath(FolderPath & CDTblWisuda.Rows(0).Item("LoginUserName") & ".jpg")
                FileUpload1.SaveAs(FilePath)
                imgPasFoto.ImageUrl = "~/FilesFotoPetugas/" & CDTblWisuda.Rows(0).Item("LoginUserName") & ".jpg"
            Else
                Dim message As String = "MESSAGE : GAGAL UPLOAD ! Data belum disave terlebih dahulu"
                Dim script As String = "<script type='text/javascript' defer='defer'> alert('" + message + "');</script>"
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "AlertBox", script)

            End If
        ElseIf FileUpload1.HasFile And FileUpload1.PostedFile.ContentLength < 25600 Then
            Dim message As String = "MESSAGE : GAGAL UPLOAD ! FILE FOTO HARUS DI ATAS 256 KB"
            Dim script As String = "<script type='text/javascript' defer='defer'> alert('" + message + "');</script>"
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "AlertBox", script)
        End If
    End Sub

End Class
