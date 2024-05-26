Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System
Imports System.Text
Imports System.Security.Cryptography
Public Class ModGlobalClass

    Public Shared Function cmdEncrypt(ByVal aTxt As String, ByVal aPass As String) As String
        Dim pass As String
        Dim encstr As String
        Dim newstr As String
        Dim tmpstr As Integer
        Dim encrypted As String = ""
        Dim letter As String
        Dim i%

        pass = Len(aPass) 'the number you shift Each letter To encrypt
        tmpstr = Len(aTxt)


        If tmpstr = 0 Then
            'You can't encrypt nothing
            Return ""
        End If


        For i = 1 To tmpstr
            letter = Mid$(aTxt, i, 1) 'takes the ascii value and adds the length of the password To it
            encstr = Asc(letter) + pass
            newstr = Chr(encstr) 'changes ascii value To a character
            encrypted = encrypted & newstr 'puts all the encrypted characters together
        Next i
        Return encrypted 'puts the encrypted String in text box
    End Function
    Public Shared Function ConvertStr(ByVal aValue As String) As String
        aValue = Replace(aValue, "'", "`")
        aValue = Replace(aValue, "--", "")
        aValue = Replace(aValue, "[", "[[]")
        aValue = Replace(aValue, "%", "[%]")
        Return aValue
    End Function
    Public Shared Function ConvertDate(ByVal aValue As Date) As String
        Dim jDate As String = aValue.ToString("yyyy-MM-dd")
        Return jDate
    End Function
    Public Shared Function ConvertTime(ByVal aValue As Date) As String
        Dim jTime As String = aValue.ToString("hh:mm")
        Return jTime
    End Function
    Public Shared Function ToQ(ByVal aQ1 As Double, ByVal aQ2 As Double, ByVal aIsi As Double) As Double
        If aIsi = 0 Then
            aIsi = 1
        Else
            aIsi = aIsi
        End If
        ToQ = (aQ1 * aIsi) + aQ2
    End Function
    Public Shared Sub ExecSqlTransaction(ByVal connectionString As String, ByVal ParamArray cmdScript() As String)
        Dim strFile As String = "C:\ErrorLog_" & DateTime.Today.ToString("dd-MMM-yyyy") & ".txt"
        Dim sw As StreamWriter
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            Dim iRec As Integer
            ' Start a local transaction
            transaction = connection.BeginTransaction("EnterpriseTransaction")

            ' Must assign both transaction object and connection 
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For iRec = 0 To UBound(cmdScript)
                    command.CommandText = cmdScript(iRec)
                    command.ExecuteNonQuery()
                Next iRec
                ' Attempt to commit the transaction.
                transaction.Commit()

            Catch ex As Exception

                ' Attempt to roll back the transaction. 
                Try
                    transaction.Rollback()

                Catch ex2 As Exception
                    ' This catch block will handle any errors that may have occurred 
                    ' on the server that would cause the rollback to fail, such as 
                    ' a closed connection.


                    Try
                        If (Not File.Exists(strFile)) Then
                            sw = File.CreateText(strFile)
                            sw.WriteLine("Start Error Transaction Log for today")
                        Else
                            sw = File.AppendText(strFile)
                        End If
                        sw.WriteLine("RollBack Exception Type: {0}" & DateTime.Now)
                        sw.Close()
                    Catch ex3 As IOException
                        MsgBox("Error writing to log file.")
                    End Try

                End Try
            End Try
            connection.Close()
        End Using
    End Sub
    Public Shared Sub ExecSqlTransactionNonLog(ByVal connectionString As String, ByVal ParamArray cmdScript() As String)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            Dim iRec As Integer
            ' Start a local transaction
            transaction = connection.BeginTransaction("EnterpriseTransaction")

            ' Must assign both transaction object and connection 
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For iRec = 0 To UBound(cmdScript)
                    command.CommandText = cmdScript(iRec)
                    command.ExecuteNonQuery()
                Next iRec
                ' Attempt to commit the transaction.
                transaction.Commit()

            Catch ex As Exception

                ' Attempt to roll back the transaction. 
                Try
                    transaction.Rollback()
                Catch ex2 As Exception
                    ' This catch block will handle any errors that may have occurred 
                    ' on the server that would cause the rollback to fail, such as 
                    ' a closed connection.
                End Try
            Finally
                connection.Close()
            End Try
        End Using
    End Sub
    Public Shared Sub RunMultiSqlTransaction(ByVal connectionString As String, ByVal trxName As String, ByVal ParamArray multiQuery() As String)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction

            ' Start a local transaction.
            transaction = connection.BeginTransaction( _
              IsolationLevel.ReadCommitted, trxName)

            ' Must assign both transaction object and connection 
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For i As Integer = 0 To UBound(multiQuery, 1)    ' Use UBound to determine largest subscript of the array. 
                    'Debug.WriteLine("Score " & i & ": " & scores(i))
                    command.CommandText = multiQuery(i)
                    command.ExecuteNonQuery()
                Next i
                transaction.Commit()
                System.Diagnostics.Debug.WriteLine("Records are written to database.")
            Catch e As Exception
                Try
                    transaction.Rollback()
                Catch ex As SqlException
                    If Not transaction.Connection Is Nothing Then
                        System.Diagnostics.Debug.WriteLine("An exception of type " & ex.GetType().ToString() & _
                          " was encountered while attempting to roll back the transaction.")
                    End If
                End Try
                System.Diagnostics.Debug.WriteLine("An exception of type " & e.GetType().ToString() & _
                  "was encountered while writing the data.")
                System.Diagnostics.Debug.WriteLine("Neither record was written to database.")
            End Try
        End Using
    End Sub
    Public Shared Sub RunSqlScript(ByVal myConnString As String, ByVal aQuery As String, ByVal aTransName As String)
        Dim myConnection As New SqlConnection(myConnString)
        myConnection.Open()
        Dim myCommand As SqlCommand = myConnection.CreateCommand()
        Dim myTrans As SqlTransaction
        ' Start a local transaction
        myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted, aTransName)
        ' Must assign both transaction object and connection
        ' to Command object for a pending local transaction
        myCommand.Connection = myConnection
        myCommand.Transaction = myTrans
        Try
            myCommand.CommandText = aQuery
            myCommand.ExecuteNonQuery()
            myTrans.Commit()
            Console.WriteLine("Record(s) are written to database.")
        Catch e As Exception
            Try
                myTrans.Rollback(aTransName)
            Catch ex As SqlException
                If Not myTrans.Connection Is Nothing Then
                    Console.WriteLine("An exception of type " & ex.GetType().ToString() & _
                        " was encountered while attempting to roll back the transaction.")
                End If
            End Try
            Console.WriteLine("An exception of type " & e.GetType().ToString() & _
                 "was encountered while inserting the data.")
            Console.WriteLine("Record(s) Not Written Successfull")
        Finally
            myConnection.Close()
        End Try

    End Sub 'RunSqlTransaction

    Public Shared Function FillDataTable(table As String, fields As String) As DataTable
        Dim query As String = "SELECT " & fields & " FROM " & table
        Dim sqlConn As New SqlConnection(strKoneksi("Conn"))
        sqlConn.Open()
        Dim cmd As New SqlCommand(query, sqlConn)

        Dim dt As New DataTable()
        dt.Load(cmd.ExecuteReader())
        sqlConn.Close()
        Return dt
    End Function
    Public Shared Function ReturnOneField(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String) As String
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strKoneksi("Conn"))
        Dim dataReturn As String = ""
        If da.Fill(ds, "sdata") = 0 Then
            dataReturn = ""
        Else
            con = New SqlConnection(strKoneksi("Conn"))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
        End If
        da.Dispose()
        ds.Clear()
        ReturnOneField = dataReturn
        Return ReturnOneField
    End Function
    Public Shared Function ReturnFieldOne(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String) As Double
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strKoneksi("Conn"))
        Dim dataReturn As String = ""
        If da.Fill(ds, "sdata") = 0 Then
            dataReturn = 0
        Else
            con = New SqlConnection(strKoneksi("Conn"))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
        End If
        da.Dispose()
        ds.Clear()
        ReturnFieldOne = dataReturn
        Return ReturnFieldOne
    End Function


    Public Shared Function Terbilang(ByVal Angka As Double) As String
        Dim strAngka, strDiurai, Urai, Tbl1, Tbl2 As String
        Dim x, y, z As Short
        Dim arrBelasan() As String = {"SEPULUH ", "SEBELAS ", _
            "DUA BELAS ", "TIGA BELAS ", "EMPAT BELAS ", _
            "LIMA BELAS ", "ENAM BELAS ", "TUJUH BELAS ", _
            "DELAPAN BELAS ", "SEMBILAN BELAS "}
        Dim arrSatuan() As String = {"DUA ", "TIGA ", "EMPAT ", _
            "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN "}
        Urai = ""
        'Angka yang akan dibuat terbilang dibulatkan dulu Jika ada desimalnya

        Angka = Math.Round(Angka)

        'Angka tipe Double diubah menjadi string Dihilangkan spasi dikiri atau kanan angka jika ada
        strAngka = Trim(CStr(Angka))

        'Perulangan While ...End While akan mengevaluasi angka satu per satu dan dimulai dari angka paling kiri
        'x menunjukkan iterasi ke berapa dimulai dari 1

        While (x < Len(strAngka))
            x += 1
            strDiurai = Mid(strAngka, x, 1)

            'y menunjukkan angka yang sedang dievaluasi
            y += Val(strDiurai)

            'z menunjukkan posisi digit ke berapa
            z = Len(strAngka) - x + 1

            ' Jika yang dievaluasi angka 1
            If Val(strDiurai) = 1 Then
                If (z = 1 Or z = 7 Or z = 10 Or z = 13) Then
                    Tbl1 = "SATU "
                ElseIf (z = 4) Then
                    If (x = 1) Then
                        Tbl1 = "SE"
                    Else
                        Tbl1 = "SATU "
                    End If
                ElseIf (z = 2 Or z = 5 Or z = 8 Or z = 11 Or z = 14) Then
                    'Ditambahkan iterasi angka berikutnya
                    x += 1
                    strDiurai = Mid(strAngka, x, 1)
                    z = Len(strAngka) - x + 1
                    Tbl2 = ""
                    Tbl1 = arrBelasan(Val(strDiurai))
                Else
                    Tbl1 = "SE"
                End If
                'Yang dievaluasi angka 2 sampai 9
            ElseIf Val(strDiurai) > 1 And Val(strDiurai) < 10 Then
                Tbl1 = arrSatuan((Val(strDiurai)) - 2)
            Else
                Tbl1 = ""
            End If

            If (Val(strDiurai) > 0) Then
                If (z = 2 Or z = 5 Or z = 8 Or z = 11 Or _
                        z = 14) Then
                    Tbl2 = "PULUH "
                ElseIf (z = 3 Or z = 6 Or z = 9 Or z = 12 _
                        Or z = 15) Then
                    Tbl2 = "RATUS "
                Else
                    Tbl2 = ""
                End If
            Else
                Tbl2 = ""
            End If

            If (y > 0) Then
                Select Case z
                    Case 4
                        Tbl2 &= "RIBU "
                        y = 0
                    Case 7
                        Tbl2 &= "JUTA "
                        y = 0
                    Case 10
                        Tbl2 &= "MILYAR "
                        y = 0
                    Case 13
                        Tbl2 &= "TRILYUN "
                        y = 0
                End Select
            End If

            Urai = Urai & Tbl1 & Tbl2
        End While

        Terbilang = Urai & "RUPIAH"
        'Terbilang(CDbl(txtInput.Text))
    End Function
    Public Shared Function strKoneksi(ByVal aConnName As String) As String
        strKoneksi = WebConfigurationManager.ConnectionStrings("Conn").ConnectionString
    End Function

    Public Shared Function getNumeric(value As String) As String
        Dim output As StringBuilder = New StringBuilder
        Dim i As Integer
        For i = 0 To value.Length - 1
            If IsNumeric(value(i)) Then
                output.Append(value(i))
            End If
        Next
        Return output.ToString()
    End Function
    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function
    Public Shared Function logMhsLogin(aNpm As String, aDt As String, aAction As String, aModul As String, ByVal vIpAdd As String) As String
        logMhsLogin = "insert into logPortal (USERID,TIPE,TGL,WAKTU,STATUSACTION,MODUL,IPADD) values ('" & aNpm & "','Mahasiswa','" & aDt & "','" & Microsoft.VisualBasic.Right(aDt, 5) & "', '" & aAction & "','" & aModul & "','" & vIpAdd & "')"
    End Function

    Public Shared Function ReturnHari(ByVal aHari As String) As String
        ReturnHari = ""
        If aHari = "Monday" Then
            ReturnHari = "Senin"
        ElseIf aHari = "Tuesday" Then
            ReturnHari = "Selasa"
        ElseIf aHari = "Wednesday" Then
            ReturnHari = "Rabu"
        ElseIf aHari = "Thursday" Then
            ReturnHari = "Kamis"
        ElseIf aHari = "Friday" Then
            ReturnHari = "Jumat"
        ElseIf aHari = "Saturday" Then
            ReturnHari = "Sabtu"
        ElseIf aHari = "Sunday" Then
            ReturnHari = "Minggu"
        End If
        Return ReturnHari
    End Function
    Public Shared Function CDateME(ByVal aDate As String) As String
        CDateME = Microsoft.VisualBasic.Right(aDate, 4) & "-" & Microsoft.VisualBasic.Mid(aDate, 4, 2) & "-" & Left(aDate, 2)
    End Function
    Public Shared Function CToDate(ByVal aDate As String) As Date
        CToDate = DateSerial(Microsoft.VisualBasic.Right(aDate, 4), Microsoft.VisualBasic.Mid(aDate, 4, 2), Left(aDate, 2))
    End Function
    Public Shared Function StringCrystal() As String
        StringCrystal = "Server=localhost;UID=sa;PASSWORD=qdb1122334455;database=QualityDb;Max Pool Size=500;Connect Timeout=600;"
        Return StringCrystal
    End Function
    Public Shared Function StringOle() As String

        StringOle = "Provider=SQLOLEDB.1;Data Source=localhost;Initial Catalog=Qualitydb;User ID=sa;Password=qdb1122334455"
    End Function
    Public Shared Function GetDataTabel(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("Conn").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con

                sda.SelectCommand = cmd
                Using dt As New DataTable()
                    sda.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Public Shared Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public Shared Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
End Class
