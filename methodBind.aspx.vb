Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class methodBind
    Inherits System.Web.UI.Page

#Region "Auto Complete"
    <WebMethod()>
    Public Function GetdtTa(ByVal input As String) As List(Of rekdtTa)
        '   Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select kurikulum,TAHUNAJARAN1 from kurikulum order by kurikulum"

        Using cnn As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("kurikulum+TAHUNAJARAN1 like '%{0}%'", input))
        Dim resultdtTa As New List(Of rekdtTa)
        For Each row In rows
            Dim r As New rekdtTa
            r.kodedtTa = row("kurikulum")
            r.namadtTa = row("TAHUNAJARAN1")
            resultdtTa.Add(r)
        Next
        Return resultdtTa
    End Function
    Public Class rekdtTa
        Public kodedtTa As String
        Public namadtTa As String
    End Class
#End Region
    <WebMethod()>
    Public Shared Function BindDataWisuda() As WisudaDetails()
        Dim dt As New DataTable()

        Dim detailsWisuda As New List(Of WisudaDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand(" select MAHASISWA.NPM, MAHASISWA.NAMA, MAHASISWA.PRODI, MAHASISWA.TA, MAHASISWA.SEMESTER, MAHASISWA.IDKAMPUS, FAKULTAS.IDFAKULTAS, FAKULTAS.FAKULTAS from MAHASISWA INNER JOIN FAKULTAS ON MAHASISWA.IDFAKULTAS = FAKULTAS.IDFAKULTAS order by MAHASISWA.NPM ASC ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataWisuda As New WisudaDetails()
                    dataWisuda.Wisuda = dtrow("NPM").ToString()
                    dataWisuda.NamaWisuda = dtrow("NAMA").ToString()
                    dataWisuda.ProdiWisuda = dtrow("PRODI").ToString()
                    dataWisuda.TaWisuda = dtrow("TA").ToString()
                    dataWisuda.SemesterWisuda = dtrow("SEMESTER").ToString()
                    dataWisuda.IdFakultasWisuda = dtrow("IDFAKULTAS").ToString()
                    dataWisuda.FakultasWisuda = dtrow("FAKULTAS").ToString()
                    dataWisuda.IdKampusWisuda = dtrow("IDKAMPUS").ToString()
                    detailsWisuda.Add(dataWisuda)
                Next
            End Using
        End Using
        Return detailsWisuda.ToArray()
    End Function

    Public Class WisudaDetails

        Public Property Wisuda() As String
            Get
                Return m_Wisuda
            End Get
            Set(ByVal value As String)
                m_Wisuda = value
            End Set
        End Property

        Private m_Wisuda As String

        Public Property NamaWisuda() As String
            Get
                Return m_NamaWisuda
            End Get
            Set(ByVal value As String)
                m_NamaWisuda = value
            End Set
        End Property

        Private m_NamaWisuda As String

        Public Property ProdiWisuda() As String
            Get
                Return m_ProdiWisuda
            End Get
            Set(ByVal value As String)
                m_ProdiWisuda = value
            End Set
        End Property

        Private m_ProdiWisuda As String

        Public Property TaWisuda() As String
            Get
                Return m_TaWisuda
            End Get
            Set(ByVal value As String)
                m_TaWisuda = value
            End Set
        End Property

        Private m_TaWisuda As String

        Public Property SemesterWisuda() As String
            Get
                Return m_SemesterWisuda
            End Get
            Set(ByVal value As String)
                m_SemesterWisuda = value
            End Set
        End Property

        Private m_SemesterWisuda As String

        Public Property IdFakultasWisuda() As String
            Get
                Return m_IdFakultasWisuda
            End Get
            Set(ByVal value As String)
                m_IdFakultasWisuda = value
            End Set
        End Property

        Private m_IdFakultasWisuda As String

        Public Property FakultasWisuda() As String
            Get
                Return m_FakultasWisuda
            End Get
            Set(ByVal value As String)
                m_FakultasWisuda = value
            End Set
        End Property

        Private m_FakultasWisuda As String

        Public Property IdKampusWisuda() As String
            Get
                Return m_IdKampusWisuda
            End Get
            Set(ByVal value As String)
                m_IdKampusWisuda = value
            End Set
        End Property

        Private m_IdKampusWisuda As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataAccPiutang() As AccPiutangDetails()
        Dim dt As New DataTable()

        Dim detailsAccPiutang As New List(Of AccPiutangDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAccPiutang As New AccPiutangDetails()
                    dataAccPiutang.KodeAccPiutang = dtrow("Kode").ToString()
                    dataAccPiutang.NamaAccPiutang = dtrow("Nama").ToString()
                    dataAccPiutang.JrAccPiutang = dtrow("Jr").ToString()
                    dataAccPiutang.KodeTipeAccPiutang = dtrow("KodeTipe").ToString()
                    dataAccPiutang.KomponenAccPiutang = dtrow("Komponen").ToString()
                    detailsAccPiutang.Add(dataAccPiutang)
                Next
            End Using
        End Using
        Return detailsAccPiutang.ToArray()
    End Function

    Public Class AccPiutangDetails

        Public Property KodeAccPiutang() As String
            Get
                Return m_KodeAccPiutang
            End Get
            Set(ByVal value As String)
                m_KodeAccPiutang = value
            End Set
        End Property

        Private m_KodeAccPiutang As String

        Public Property NamaAccPiutang() As String
            Get
                Return m_NamaAccPiutang
            End Get
            Set(ByVal value As String)
                m_NamaAccPiutang = value
            End Set
        End Property

        Private m_NamaAccPiutang As String

        Public Property JrAccPiutang() As String
            Get
                Return m_JrAccPiutang
            End Get
            Set(ByVal value As String)
                m_JrAccPiutang = value
            End Set
        End Property

        Private m_JrAccPiutang As String

        Public Property KodeTipeAccPiutang() As String
            Get
                Return m_KodeTipeAccPiutang
            End Get
            Set(ByVal value As String)
                m_KodeTipeAccPiutang = value
            End Set
        End Property

        Private m_KodeTipeAccPiutang As String

        Public Property KomponenAccPiutang() As String
            Get
                Return m_KomponenAccPiutang
            End Get
            Set(ByVal value As String)
                m_KomponenAccPiutang = value
            End Set
        End Property

        Private m_KomponenAccPiutang As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataModalAccount() As ModalAccountDetails()
        Dim dt As New DataTable()

        Dim detailsModalAccount As New List(Of ModalAccountDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT KODE,NAMA from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataModalAccount As New ModalAccountDetails()
                    dataModalAccount.KodeAccount = dtrow("Kode").ToString()
                    dataModalAccount.NamaAccount = dtrow("Nama").ToString()
                    detailsModalAccount.Add(dataModalAccount)
                Next
            End Using
        End Using
        Return detailsModalAccount.ToArray()
    End Function

    Public Class ModalAccountDetails

        Public Property KodeAccount() As String
            Get
                Return m_KodeAccount
            End Get
            Set(ByVal value As String)
                m_KodeAccount = value
            End Set
        End Property

        Private m_KodeAccount As String

        Public Property NamaAccount() As String
            Get
                Return m_NamaAccount
            End Get
            Set(ByVal value As String)
                m_NamaAccount = value
            End Set
        End Property

        Private m_NamaAccount As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataModalJenis() As ModalJenisDetails()
        Dim dt As New DataTable()

        Dim detailsModalJenis As New List(Of ModalJenisDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select * from JENISBIAYAKULIAH", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataModalJenis As New ModalJenisDetails()
                    dataModalJenis.Jenis = dtrow("Jenis").ToString()
                    dataModalJenis.Grup = dtrow("Grup").ToString()
                    detailsModalJenis.Add(dataModalJenis)
                Next
            End Using
        End Using
        Return detailsModalJenis.ToArray()
    End Function

    Public Class ModalJenisDetails

        Public Property Jenis() As String
            Get
                Return m_Jenis
            End Get
            Set(ByVal value As String)
                m_Jenis = value
            End Set
        End Property

        Private m_Jenis As String

        Public Property Grup() As String
            Get
                Return m_Grup
            End Get
            Set(ByVal value As String)
                m_Grup = value
            End Set
        End Property

        Private m_Grup As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataProdi() As ProdiDetails()
        Dim dt As New DataTable()

        Dim detailsProdi As New List(Of ProdiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connuq"))
            Using cmd As New SqlCommand("select Distinct(prodi) from PRODI", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataModalProdi As New ProdiDetails()
                    dataModalProdi.Prodi = dtrow("Prodi").ToString()
                    detailsProdi.Add(dataModalProdi)
                Next
            End Using
        End Using
        Return detailsProdi.ToArray()
    End Function

    Public Class ProdiDetails

        Public Property Prodi() As String
            Get
                Return m_Prodi
            End Get
            Set(ByVal value As String)
                m_Prodi = value
            End Set
        End Property

        Private m_Prodi As String

        Public Property Jurusan() As String
            Get
                Return m_Jurusan
            End Get
            Set(ByVal value As String)
                m_Jurusan = value
            End Set
        End Property

        Private m_Jurusan As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataAccKasBank() As AccKasBankDetails()
        Dim dt As New DataTable()

        Dim detailsAccKasBank As New List(Of AccKasBankDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAccKasBank As New AccKasBankDetails()
                    dataAccKasBank.KodeAccKasBank = dtrow("Kode").ToString()
                    dataAccKasBank.NamaAccKasBank = dtrow("Nama").ToString()
                    dataAccKasBank.JrAccKasBank = dtrow("Jr").ToString()
                    dataAccKasBank.KodeTipeAccKasBank = dtrow("KodeTipe").ToString()
                    dataAccKasBank.KomponenAccKasBank = dtrow("Komponen").ToString()
                    detailsAccKasBank.Add(dataAccKasBank)
                Next
            End Using
        End Using
        Return detailsAccKasBank.ToArray()
    End Function

    Public Class AccKasBankDetails

        Public Property KodeAccKasBank() As String
            Get
                Return m_KodeAccKasBank
            End Get
            Set(ByVal value As String)
                m_KodeAccKasBank = value
            End Set
        End Property

        Private m_KodeAccKasBank As String

        Public Property NamaAccKasBank() As String
            Get
                Return m_NamaAccKasBank
            End Get
            Set(ByVal value As String)
                m_NamaAccKasBank = value
            End Set
        End Property

        Private m_NamaAccKasBank As String

        Public Property JrAccKasBank() As String
            Get
                Return m_JrAccKasBank
            End Get
            Set(ByVal value As String)
                m_JrAccKasBank = value
            End Set
        End Property

        Private m_JrAccKasBank As String

        Public Property KodeTipeAccKasBank() As String
            Get
                Return m_KodeTipeAccKasBank
            End Get
            Set(ByVal value As String)
                m_KodeTipeAccKasBank = value
            End Set
        End Property

        Private m_KodeTipeAccKasBank As String

        Public Property KomponenAccKasBank() As String
            Get
                Return m_KomponenAccKasBank
            End Get
            Set(ByVal value As String)
                m_KomponenAccKasBank = value
            End Set
        End Property

        Private m_KomponenAccKasBank As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataDosenPegawai() As DosenPegawaiDetails()
        Dim dt As New DataTable()

        Dim detailsDosenPegawai As New List(Of DosenPegawaiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataDosenPegawai As New DosenPegawaiDetails()
                    dataDosenPegawai.KodeDosenPegawai = dtrow("Kode").ToString()
                    dataDosenPegawai.NamaDosenPegawai = dtrow("Nama").ToString()
                    dataDosenPegawai.JrDosenPegawai = dtrow("Jr").ToString()
                    dataDosenPegawai.KodeTipeDosenPegawai = dtrow("KodeTipe").ToString()
                    dataDosenPegawai.KomponenDosenPegawai = dtrow("Komponen").ToString()
                    detailsDosenPegawai.Add(dataDosenPegawai)
                Next
            End Using
        End Using
        Return detailsDosenPegawai.ToArray()
    End Function

    Public Class DosenPegawaiDetails

        Public Property KodeDosenPegawai() As String
            Get
                Return m_KodeDosenPegawai
            End Get
            Set(ByVal value As String)
                m_KodeDosenPegawai = value
            End Set
        End Property

        Private m_KodeDosenPegawai As String

        Public Property NamaDosenPegawai() As String
            Get
                Return m_NamaDosenPegawai
            End Get
            Set(ByVal value As String)
                m_NamaDosenPegawai = value
            End Set
        End Property

        Private m_NamaDosenPegawai As String

        Public Property JrDosenPegawai() As String
            Get
                Return m_JrDosenPegawai
            End Get
            Set(ByVal value As String)
                m_JrDosenPegawai = value
            End Set
        End Property

        Private m_JrDosenPegawai As String

        Public Property KodeTipeDosenPegawai() As String
            Get
                Return m_KodeTipeDosenPegawai
            End Get
            Set(ByVal value As String)
                m_KodeTipeDosenPegawai = value
            End Set
        End Property

        Private m_KodeTipeDosenPegawai As String

        Public Property KomponenDosenPegawai() As String
            Get
                Return m_KomponenDosenPegawai
            End Get
            Set(ByVal value As String)
                m_KomponenDosenPegawai = value
            End Set
        End Property

        Private m_KomponenDosenPegawai As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataVendor() As Vendor1Details()
        Dim dt As New DataTable()

        Dim detailsVendor1 As New List(Of Vendor1Details)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select SUPP,NAMA,AL from VENDOR", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataVendor1 As New Vendor1Details()
                    dataVendor1.SuppVendor = dtrow("SUPP").ToString()
                    dataVendor1.NamaVendor = dtrow("NAMA").ToString()
                    dataVendor1.AlamatVendor = dtrow("AL").ToString()
                    detailsVendor1.Add(dataVendor1)
                Next
            End Using
        End Using
        Return detailsVendor1.ToArray()
    End Function

    Public Class Vendor1Details

        Public Property SuppVendor() As String
            Get
                Return m_SuppVendor
            End Get
            Set(ByVal value As String)
                m_SuppVendor = value
            End Set
        End Property

        Private m_SuppVendor As String

        Public Property NamaVendor() As String
            Get
                Return m_NamaVendor
            End Get
            Set(ByVal value As String)
                m_NamaVendor = value
            End Set
        End Property

        Private m_NamaVendor As String

        Public Property AlamatVendor() As String
            Get
                Return m_AlamatVendor
            End Get
            Set(ByVal value As String)
                m_AlamatVendor = value
            End Set
        End Property

        Private m_AlamatVendor As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataMahasiswa() As MahasiswaDetails()
        Dim dt As New DataTable()

        Dim detailsMahasiswa As New List(Of MahasiswaDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataMahasiswa As New MahasiswaDetails()
                    dataMahasiswa.KodeMahasiswa = dtrow("Kode").ToString()
                    dataMahasiswa.NamaMahasiswa = dtrow("Nama").ToString()
                    dataMahasiswa.JrMahasiswa = dtrow("Jr").ToString()
                    dataMahasiswa.KodeTipeMahasiswa = dtrow("KodeTipe").ToString()
                    dataMahasiswa.KomponenMahasiswa = dtrow("Komponen").ToString()
                    detailsMahasiswa.Add(dataMahasiswa)
                Next
            End Using
        End Using
        Return detailsMahasiswa.ToArray()
    End Function

    Public Class MahasiswaDetails

        Public Property KodeMahasiswa() As String
            Get
                Return m_KodeMahasiswa
            End Get
            Set(ByVal value As String)
                m_KodeMahasiswa = value
            End Set
        End Property

        Private m_KodeMahasiswa As String

        Public Property NamaMahasiswa() As String
            Get
                Return m_NamaMahasiswa
            End Get
            Set(ByVal value As String)
                m_NamaMahasiswa = value
            End Set
        End Property

        Private m_NamaMahasiswa As String

        Public Property JrMahasiswa() As String
            Get
                Return m_JrMahasiswa
            End Get
            Set(ByVal value As String)
                m_JrMahasiswa = value
            End Set
        End Property

        Private m_JrMahasiswa As String

        Public Property KodeTipeMahasiswa() As String
            Get
                Return m_KodeTipeMahasiswa
            End Get
            Set(ByVal value As String)
                m_KodeTipeMahasiswa = value
            End Set
        End Property

        Private m_KodeTipeMahasiswa As String

        Public Property KomponenMahasiswa() As String
            Get
                Return m_KomponenMahasiswa
            End Get
            Set(ByVal value As String)
                m_KomponenMahasiswa = value
            End Set
        End Property

        Private m_KomponenMahasiswa As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataEc() As EcDetails()
        Dim dt As New DataTable()

        Dim detailsEc As New List(Of EcDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataEc As New EcDetails()
                    dataEc.KodeEc = dtrow("Kode").ToString()
                    dataEc.NamaEc = dtrow("Nama").ToString()
                    dataEc.JrEc = dtrow("Jr").ToString()
                    dataEc.KodeTipeEc = dtrow("KodeTipe").ToString()
                    dataEc.KomponenEc = dtrow("Komponen").ToString()
                    detailsEc.Add(dataEc)
                Next
            End Using
        End Using
        Return detailsEc.ToArray()
    End Function

    Public Class EcDetails

        Public Property KodeEc() As String
            Get
                Return m_KodeEc
            End Get
            Set(ByVal value As String)
                m_KodeEc = value
            End Set
        End Property

        Private m_KodeEc As String

        Public Property NamaEc() As String
            Get
                Return m_NamaEc
            End Get
            Set(ByVal value As String)
                m_NamaEc = value
            End Set
        End Property

        Private m_NamaEc As String

        Public Property JrEc() As String
            Get
                Return m_JrEc
            End Get
            Set(ByVal value As String)
                m_JrEc = value
            End Set
        End Property

        Private m_JrEc As String

        Public Property KodeTipeEc() As String
            Get
                Return m_KodeTipeEc
            End Get
            Set(ByVal value As String)
                m_KodeTipeEc = value
            End Set
        End Property

        Private m_KodeTipeEc As String

        Public Property KomponenEc() As String
            Get
                Return m_KomponenEc
            End Get
            Set(ByVal value As String)
                m_KomponenEc = value
            End Set
        End Property

        Private m_KomponenEc As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKepsek() As KepsekDetails()
        Dim dt As New DataTable()

        Dim detailsKepsek As New List(Of KepsekDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKepsek As New KepsekDetails()
                    dataKepsek.KodeKepsek = dtrow("Kode").ToString()
                    dataKepsek.NamaKepsek = dtrow("Nama").ToString()
                    dataKepsek.JrKepsek = dtrow("Jr").ToString()
                    dataKepsek.KodeTipeKepsek = dtrow("KodeTipe").ToString()
                    dataKepsek.KomponenKepsek = dtrow("Komponen").ToString()
                    detailsKepsek.Add(dataKepsek)
                Next
            End Using
        End Using
        Return detailsKepsek.ToArray()
    End Function

    Public Class KepsekDetails

        Public Property KodeKepsek() As String
            Get
                Return m_KodeKepsek
            End Get
            Set(ByVal value As String)
                m_KodeKepsek = value
            End Set
        End Property

        Private m_KodeKepsek As String

        Public Property NamaKepsek() As String
            Get
                Return m_NamaKepsek
            End Get
            Set(ByVal value As String)
                m_NamaKepsek = value
            End Set
        End Property

        Private m_NamaKepsek As String

        Public Property JrKepsek() As String
            Get
                Return m_JrKepsek
            End Get
            Set(ByVal value As String)
                m_JrKepsek = value
            End Set
        End Property

        Private m_JrKepsek As String

        Public Property KodeTipeKepsek() As String
            Get
                Return m_KodeTipeKepsek
            End Get
            Set(ByVal value As String)
                m_KodeTipeKepsek = value
            End Set
        End Property

        Private m_KodeTipeKepsek As String

        Public Property KomponenKepsek() As String
            Get
                Return m_KomponenKepsek
            End Get
            Set(ByVal value As String)
                m_KomponenKepsek = value
            End Set
        End Property

        Private m_KomponenKepsek As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataAccBankSpp() As AccBankSppDetails()
        Dim dt As New DataTable()

        Dim detailsAccBankSpp As New List(Of AccBankSppDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select * from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAccBankSpp As New AccBankSppDetails()
                    dataAccBankSpp.KodeAccBankSpp = dtrow("Kode").ToString()
                    dataAccBankSpp.NamaAccBankSpp = dtrow("Nama").ToString()
                    detailsAccBankSpp.Add(dataAccBankSpp)
                Next
            End Using
        End Using
        Return detailsAccBankSpp.ToArray()
    End Function

    Public Class AccBankSppDetails

        Public Property KodeAccBankSpp() As String
            Get
                Return m_KodeAccBankSpp
            End Get
            Set(ByVal value As String)
                m_KodeAccBankSpp = value
            End Set
        End Property

        Private m_KodeAccBankSpp As String

        Public Property NamaAccBankSpp() As String
            Get
                Return m_NamaAccBankSpp
            End Get
            Set(ByVal value As String)
                m_NamaAccBankSpp = value
            End Set
        End Property

        Private m_NamaAccBankSpp As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataHutMarketingFee() As HutMarketingFeeDetails()
        Dim dt As New DataTable()

        Dim detailsHutMarketingFee As New List(Of HutMarketingFeeDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataHutMarketingFee As New HutMarketingFeeDetails()
                    dataHutMarketingFee.KodeHutMarketingFee = dtrow("Kode").ToString()
                    dataHutMarketingFee.NamaHutMarketingFee = dtrow("Nama").ToString()
                    dataHutMarketingFee.JrHutMarketingFee = dtrow("Jr").ToString()
                    dataHutMarketingFee.KodeTipeHutMarketingFee = dtrow("KodeTipe").ToString()
                    dataHutMarketingFee.KomponenHutMarketingFee = dtrow("Komponen").ToString()
                    detailsHutMarketingFee.Add(dataHutMarketingFee)
                Next
            End Using
        End Using
        Return detailsHutMarketingFee.ToArray()
    End Function

    Public Class HutMarketingFeeDetails

        Public Property KodeHutMarketingFee() As String
            Get
                Return m_KodeHutMarketingFee
            End Get
            Set(ByVal value As String)
                m_KodeHutMarketingFee = value
            End Set
        End Property

        Private m_KodeHutMarketingFee As String

        Public Property NamaHutMarketingFee() As String
            Get
                Return m_NamaHutMarketingFee
            End Get
            Set(ByVal value As String)
                m_NamaHutMarketingFee = value
            End Set
        End Property

        Private m_NamaHutMarketingFee As String

        Public Property JrHutMarketingFee() As String
            Get
                Return m_JrHutMarketingFee
            End Get
            Set(ByVal value As String)
                m_JrHutMarketingFee = value
            End Set
        End Property

        Private m_JrHutMarketingFee As String

        Public Property KodeTipeHutMarketingFee() As String
            Get
                Return m_KodeTipeHutMarketingFee
            End Get
            Set(ByVal value As String)
                m_KodeTipeHutMarketingFee = value
            End Set
        End Property

        Private m_KodeTipeHutMarketingFee As String

        Public Property KomponenHutMarketingFee() As String
            Get
                Return m_KomponenHutMarketingFee
            End Get
            Set(ByVal value As String)
                m_KomponenHutMarketingFee = value
            End Set
        End Property

        Private m_KomponenHutMarketingFee As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataAccPendapatan() As AccPendapatanDetails()
        Dim dt As New DataTable()

        Dim detailsAccPendapatan As New List(Of AccPendapatanDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA,JR,KODETIPE,KOMPONEN from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAccPendapatan As New AccPendapatanDetails()
                    dataAccPendapatan.KodeAccPendapatan = dtrow("Kode").ToString()
                    dataAccPendapatan.NamaAccPendapatan = dtrow("Nama").ToString()
                    dataAccPendapatan.JrAccPendapatan = dtrow("Jr").ToString()
                    dataAccPendapatan.KodeTipeAccPendapatan = dtrow("KodeTipe").ToString()
                    dataAccPendapatan.KomponenAccPendapatan = dtrow("Komponen").ToString()
                    detailsAccPendapatan.Add(dataAccPendapatan)
                Next
            End Using
        End Using
        Return detailsAccPendapatan.ToArray()
    End Function

    Public Class AccPendapatanDetails

        Public Property KodeAccPendapatan() As String
            Get
                Return m_KodeAccPendapatan
            End Get
            Set(ByVal value As String)
                m_KodeAccPendapatan = value
            End Set
        End Property

        Private m_KodeAccPendapatan As String

        Public Property NamaAccPendapatan() As String
            Get
                Return m_NamaAccPendapatan
            End Get
            Set(ByVal value As String)
                m_NamaAccPendapatan = value
            End Set
        End Property

        Private m_NamaAccPendapatan As String

        Public Property JrAccPendapatan() As String
            Get
                Return m_JrAccPendapatan
            End Get
            Set(ByVal value As String)
                m_JrAccPendapatan = value
            End Set
        End Property

        Private m_JrAccPendapatan As String

        Public Property KodeTipeAccPendapatan() As String
            Get
                Return m_KodeTipeAccPendapatan
            End Get
            Set(ByVal value As String)
                m_KodeTipeAccPendapatan = value
            End Set
        End Property

        Private m_KodeTipeAccPendapatan As String

        Public Property KomponenAccPendapatan() As String
            Get
                Return m_KomponenAccPendapatan
            End Get
            Set(ByVal value As String)
                m_KomponenAccPendapatan = value
            End Set
        End Property

        Private m_KomponenAccPendapatan As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataJurnal() As JurnalDetails()
        Dim dt As New DataTable()

        Dim detailsJurnal As New List(Of JurnalDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Ref, Debet, Kredit, NoBukti, Catatan FROM REFJURNAL", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataJurnal As New JurnalDetails()
                    dataJurnal.RefJurnal = dtrow("Ref").ToString()
                    dataJurnal.DebetJurnal = dtrow("Debet").ToString()
                    dataJurnal.KreditJurnal = dtrow("Kredit").ToString()
                    dataJurnal.NoBuktiJurnal = dtrow("NoBukti").ToString()
                    dataJurnal.CatatanJurnal = dtrow("Catatan").ToString()
                    detailsJurnal.Add(dataJurnal)
                Next
            End Using
        End Using
        Return detailsJurnal.ToArray()
    End Function

    Public Class JurnalDetails

        Public Property RefJurnal() As String
            Get
                Return m_RefJurnal
            End Get
            Set(ByVal value As String)
                m_RefJurnal = value
            End Set
        End Property

        Private m_RefJurnal As String

        Public Property DebetJurnal() As String
            Get
                Return m_DebetJurnal
            End Get
            Set(ByVal value As String)
                m_DebetJurnal = value
            End Set
        End Property

        Private m_DebetJurnal As String

        Public Property KreditJurnal() As String
            Get
                Return m_KreditJurnal
            End Get
            Set(ByVal value As String)
                m_KreditJurnal = value
            End Set
        End Property

        Private m_KreditJurnal As String

        Public Property NoBuktiJurnal() As String
            Get
                Return m_NoBuktiJurnal
            End Get
            Set(ByVal value As String)
                m_NoBuktiJurnal = value
            End Set
        End Property

        Private m_NoBuktiJurnal As String

        Public Property CatatanJurnal() As String
            Get
                Return m_CatatanJurnal
            End Get
            Set(ByVal value As String)
                m_CatatanJurnal = value
            End Set
        End Property

        Private m_CatatanJurnal As String
    End Class

      
    'DiBawah Untuk Modal Isi Dari Via From PenerimaanKasGiro
    <WebMethod()>
    Public Shared Function BindDataVia() As ViaDetails()
        Dim dt As New DataTable()

        Dim detailsVia As New List(Of ViaDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT ID,NAMA FROM KASOUTVIA ORDER BY ID", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataVia As New ViaDetails()
                    dataVia.IDVia = dtrow("ID").ToString()
                    dataVia.NAMAVia = dtrow("NAMA").ToString()
                    detailsVia.Add(dataVia)
                Next
            End Using
        End Using
        Return detailsVia.ToArray()
    End Function

    Public Class ViaDetails

        Public Property IDVia() As String
            Get
                Return m_IDVia
            End Get
            Set(ByVal value As String)
                m_IDVia = value
            End Set
        End Property

        Private m_IDVia As String
        Public Property NAMAVia() As String
            Get
                Return m_NAMAVia
            End Get
            Set(ByVal value As String)
                m_NAMAVia = value
            End Set
        End Property

        Private m_NAMAVia As String
    End Class

#Region "Pengeluaran"
    'DiBawah Untuk Modal Isi Dari Via From PenerimaanKasGiro
    <WebMethod()>
    Public Shared Function BindDataViaPengeluaran() As ViaDetailsPengeluaran()
        Dim dt As New DataTable()

        Dim detailsViaPengeluaran As New List(Of ViaDetailsPengeluaran)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT ID,NAMA FROM KASOUTVIA ORDER BY ID", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataViaPengeluaran As New ViaDetailsPengeluaran()
                    dataViaPengeluaran.IDViaPengeluaran = dtrow("ID").ToString()
                    dataViaPengeluaran.NAMAViaPengeluaran = dtrow("NAMA").ToString()
                    detailsViaPengeluaran.Add(dataViaPengeluaran)
                Next
            End Using
        End Using
        Return detailsViaPengeluaran.ToArray()
    End Function

    Public Class ViaDetailsPengeluaran

        Public Property IDViaPengeluaran() As String
            Get
                Return m_IDViaPengeluaran
            End Get
            Set(ByVal value As String)
                m_IDViaPengeluaran = value
            End Set
        End Property

        Private m_IDViaPengeluaran As String
        Public Property NAMAViaPengeluaran() As String
            Get
                Return m_NAMAViaPengeluaran
            End Get
            Set(ByVal value As String)
                m_NAMAViaPengeluaran = value
            End Set
        End Property

        Private m_NAMAViaPengeluaran As String
    End Class

    'DiBawah Untuk Modal Isi Dari Setuju From Pengeluaran KasGiro
    <WebMethod()>
    Public Shared Function BindDataSetujuPengeluaran() As SetujuDetailsPengeluaran()
        Dim dt As New DataTable()

        Dim detailsSetujuPengeluaran As New List(Of SetujuDetailsPengeluaran)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT ID,NAMA FROM KASOUTVIA ORDER BY ID", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataSetujuPengeluaran As New SetujuDetailsPengeluaran()
                    dataSetujuPengeluaran.IDSetujuPengeluaran = dtrow("ID").ToString()
                    dataSetujuPengeluaran.NAMASetujuPengeluaran = dtrow("NAMA").ToString()
                    detailsSetujuPengeluaran.Add(dataSetujuPengeluaran)
                Next
            End Using
        End Using
        Return detailsSetujuPengeluaran.ToArray()
    End Function

    Public Class SetujuDetailsPengeluaran

        Public Property IDSetujuPengeluaran() As String
            Get
                Return m_IDSetujuPengeluaran
            End Get
            Set(ByVal value As String)
                m_IDSetujuPengeluaran = value
            End Set
        End Property

        Private m_IDSetujuPengeluaran As String
        Public Property NAMASetujuPengeluaran() As String
            Get
                Return m_NAMASetujuPengeluaran
            End Get
            Set(ByVal value As String)
                m_NAMASetujuPengeluaran = value
            End Set
        End Property

        Private m_NAMASetujuPengeluaran As String
    End Class

    'DiBawah Untuk Modal Isi Dari No Bukti Ap From Penerimaan KasGiro
    <WebMethod()>
    Public Shared Function BindDataNoBuktiApPengeluaran() As NoBuktiApDetailsPengeluaran()
        Dim dt As New DataTable()

        Dim detailsNoBuktiApPengeluaran As New List(Of NoBuktiApDetailsPengeluaran)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Ref, TotalByr FROM HutangAp ORDER BY Ref", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataNoBuktiApPengeluaran As New NoBuktiApDetailsPengeluaran()
                    dataNoBuktiApPengeluaran.RefNoBuktiAp = dtrow("Ref").ToString()
                    dataNoBuktiApPengeluaran.TotalNoBuktiAp = dtrow("TotalByr").ToString()
                    detailsNoBuktiApPengeluaran.Add(dataNoBuktiApPengeluaran)
                Next
            End Using
        End Using
        Return detailsNoBuktiApPengeluaran.ToArray()
    End Function

    Public Class NoBuktiApDetailsPengeluaran

        Public Property RefNoBuktiAp() As String
            Get
                Return m_RefNoBuktiAp
            End Get
            Set(ByVal value As String)
                m_RefNoBuktiAp = value
            End Set
        End Property

        Private m_RefNoBuktiAp As String
        Public Property TotalNoBuktiAp() As String
            Get
                Return m_TotalNoBuktiAp
            End Get
            Set(ByVal value As String)
                m_TotalNoBuktiAp = value
            End Set
        End Property

        Private m_TotalNoBuktiAp As String
    End Class
#End Region


    <WebMethod()>
    Public Shared Function BindDataSubAccount() As SubAccountDetails()
        Dim dt As New DataTable()

        Dim detailsSubAccount As New List(Of SubAccountDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT kodetipe, LevelKode, GrupLevel, Jr, Komponen FROM GLTYPEACC ORDER BY kodetipe", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataSubAccount As New SubAccountDetails()
                    dataSubAccount.KodeSubAccount = dtrow("kodetipe").ToString()
                    dataSubAccount.LevelKodeSubAccount = dtrow("LevelKode").ToString()
                    dataSubAccount.GrupLevelSubAccount = dtrow("GrupLevel").ToString()
                    dataSubAccount.JrSubAccount = dtrow("Jr").ToString()
                    dataSubAccount.KomponenSubAccount = dtrow("Komponen").ToString()
                    detailsSubAccount.Add(dataSubAccount)
                Next
            End Using
        End Using
        Return detailsSubAccount.ToArray()
    End Function

    Public Class SubAccountDetails

        Public Property KodeSubAccount() As String
            Get
                Return m_KodeSubAccount
            End Get
            Set(ByVal value As String)
                m_KodeSubAccount = value
            End Set
        End Property

        Private m_KodeSubAccount As String

        Public Property LevelKodeSubAccount() As String
            Get
                Return m_LevelKodeSubAccount
            End Get
            Set(ByVal value As String)
                m_LevelKodeSubAccount = value
            End Set
        End Property

        Private m_LevelKodeSubAccount As String

        Public Property GrupLevelSubAccount() As String
            Get
                Return m_GrupLevelSubAccount
            End Get
            Set(ByVal value As String)
                m_GrupLevelSubAccount = value
            End Set
        End Property

        Private m_GrupLevelSubAccount As String

        Public Property JrSubAccount() As String
            Get
                Return m_JrSubAccount
            End Get
            Set(ByVal value As String)
                m_JrSubAccount = value
            End Set
        End Property

        Private m_JrSubAccount As String

        Public Property KomponenSubAccount() As String
            Get
                Return m_KomponenSubAccount
            End Get
            Set(ByVal value As String)
                m_KomponenSubAccount = value
            End Set
        End Property

        Private m_KomponenSubAccount As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataAccount() As AccountDetails()
        Dim dt As New DataTable()

        Dim detailsAccount As New List(Of AccountDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select KODE,NAMA from GLFREK", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAccount As New AccountDetails()
                    dataAccount.KodeAccount = dtrow("Kode").ToString()
                    dataAccount.NamaAccount = dtrow("Nama").ToString()
                    detailsAccount.Add(dataAccount)
                Next
            End Using
        End Using
        Return detailsAccount.ToArray()
    End Function

    Public Class AccountDetails

        Public Property KodeAccount() As String
            Get
                Return m_KodeAccount
            End Get
            Set(ByVal value As String)
                m_KodeAccount = value
            End Set
        End Property

        Private m_KodeAccount As String

        Public Property NamaAccount() As String
            Get
                Return m_NamaAccount
            End Get
            Set(ByVal value As String)
                m_NamaAccount = value
            End Set
        End Property

        Private m_NamaAccount As String
    End Class


    'DiBawah Untuk Modal Isi Dari Setuju From PenerimaanKasGiro
    <WebMethod()>
    Public Shared Function BindDataSetuju() As SetujuDetails()
        Dim dt As New DataTable()

        Dim detailsSetuju As New List(Of SetujuDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT ID,NAMA FROM KASOUTVIA ORDER BY ID", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataSetuju As New SetujuDetails()
                    dataSetuju.IDSetuju = dtrow("ID").ToString()
                    dataSetuju.NAMASetuju = dtrow("NAMA").ToString()
                    detailsSetuju.Add(dataSetuju)
                Next
            End Using
        End Using
        Return detailsSetuju.ToArray()
    End Function

    Public Class SetujuDetails

        Public Property IDSetuju() As String
            Get
                Return m_IDSetuju
            End Get
            Set(ByVal value As String)
                m_IDSetuju = value
            End Set
        End Property

        Private m_IDSetuju As String
        Public Property NAMASetuju() As String
            Get
                Return m_NAMASetuju
            End Get
            Set(ByVal value As String)
                m_NAMASetuju = value
            End Set
        End Property

        Private m_NAMASetuju As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataCabang() As CabangDetails()
        Dim dt As New DataTable()

        Dim detailsCabang As New List(Of CabangDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDCabang,LOKASI FROM TblCabang ORDER BY IDCabang", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataCabang As New CabangDetails()
                    dataCabang.IdCabang = dtrow("IDCabang").ToString()
                    dataCabang.lokasiCabang = dtrow("LOKASI").ToString()
                    detailsCabang.Add(dataCabang)
                Next
            End Using
        End Using
        Return detailsCabang.ToArray()
    End Function
    Public Class CabangDetails

        Public Property IdCabang() As String
            Get
                Return m_IdCabang
            End Get
            Set(ByVal value As String)
                m_IdCabang = value
            End Set
        End Property

        Private m_IdCabang As String

        Public Property lokasiCabang() As String
            Get
                Return m_lokasiCabang
            End Get
            Set(ByVal value As String)
                m_lokasiCabang = value
            End Set
        End Property

        Private m_lokasiCabang As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataUnit() As UnitDetails()
        Dim dt As New DataTable()

        Dim detailsUnit As New List(Of UnitDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDUnit,LOKASI FROM TblUnitKerja ORDER BY IDUnit", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataUnit As New UnitDetails()
                    dataUnit.IdUnit = dtrow("IDUnit").ToString()
                    dataUnit.lokasiUnit = dtrow("LOKASI").ToString()
                    detailsUnit.Add(dataUnit)
                Next
            End Using
        End Using
        Return detailsUnit.ToArray()
    End Function
    Public Class UnitDetails

        Public Property IdUnit() As String
            Get
                Return m_IdUnit
            End Get
            Set(ByVal value As String)
                m_IdUnit = value
            End Set
        End Property

        Private m_IdUnit As String

        Public Property lokasiUnit() As String
            Get
                Return m_lokasiUnit
            End Get
            Set(ByVal value As String)
                m_lokasiUnit = value
            End Set
        End Property

        Private m_lokasiUnit As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataWipem() As WipemDetails()
        Dim dt As New DataTable()

        Dim detailsWipem As New List(Of WipemDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDWipem,LOKASI FROM TblWipem ORDER BY IDWipem", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataWipem As New WipemDetails()
                    dataWipem.IdWipem = dtrow("IDWipem").ToString()
                    dataWipem.lokasiWipem = dtrow("LOKASI").ToString()
                    detailsWipem.Add(dataWipem)
                Next
            End Using
        End Using
        Return detailsWipem.ToArray()
    End Function
    Public Class WipemDetails

        Public Property IdWipem() As String
            Get
                Return m_IdWipem
            End Get
            Set(ByVal value As String)
                m_IdWipem = value
            End Set
        End Property

        Private m_IdWipem As String

        Public Property lokasiWipem() As String
            Get
                Return m_lokasiWipem
            End Get
            Set(ByVal value As String)
                m_lokasiWipem = value
            End Set
        End Property

        Private m_lokasiWipem As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataGedungKampus() As KampusDetails2()
        Dim dt As New DataTable()

        Dim detailsKampus2 As New List(Of KampusDetails2)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDKAMPUS,LOKASI FROM KAMPUS ORDER BY IDKAMPUS", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKampus As New KampusDetails2()
                    dataKampus.Idkampus2 = dtrow("IDKAMPUS").ToString()
                    dataKampus.lokasikampus2 = dtrow("LOKASI").ToString()
                    detailsKampus2.Add(dataKampus)
                Next
            End Using
        End Using
        Return detailsKampus2.ToArray()
    End Function

    Public Class KampusDetails2

        Public Property Idkampus2() As String
            Get
                Return m_Idkampus
            End Get
            Set(ByVal value As String)
                m_Idkampus = value
            End Set
        End Property

        Private m_Idkampus As String

        Public Property lokasikampus2() As String
            Get
                Return m_lokasikampus
            End Get
            Set(ByVal value As String)
                m_lokasikampus = value
            End Set
        End Property

        Private m_lokasikampus As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataGedungAsal() As KampusAsalDetails()
        Dim dt As New DataTable()

        Dim detailsKampusAsal As New List(Of KampusAsalDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT kode_perguruan_tinggi as IDASALPTI,nama_perguruan_tinggi as Nama, KET FROM feederptasal ORDER BY kode_perguruan_tinggi Asc", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKampusAsal As New KampusAsalDetails()
                    dataKampusAsal.IdKampusAsal = dtrow("IDASALPTI").ToString()
                    dataKampusAsal.lokasiKampusAsal = dtrow("Nama").ToString()
                    dataKampusAsal.KETKampusAsal = dtrow("KET").ToString()
                    detailsKampusAsal.Add(dataKampusAsal)
                Next
            End Using
        End Using
        Return detailsKampusAsal.ToArray()
    End Function

    Public Class KampusAsalDetails

        Public Property IdKampusAsal() As String
            Get
                Return m_IdKampusAsal
            End Get
            Set(ByVal value As String)
                m_IdKampusAsal = value
            End Set
        End Property

        Private m_IdKampusAsal As String

        Public Property lokasiKampusAsal() As String
            Get
                Return m_lokasiKampusAsal
            End Get
            Set(ByVal value As String)
                m_lokasiKampusAsal = value
            End Set
        End Property

        Private m_lokasiKampusAsal As String

        Public Property KETKampusAsal() As String
            Get
                Return m_KETKampusAsal
            End Get
            Set(ByVal value As String)
                m_KETKampusAsal = value
            End Set
        End Property

        Private m_KETKampusAsal As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataGedungAsalll() As KampusssAsalDetails()
        Dim dt As New DataTable()
        Dim awal As String = "011040"
        Dim akhir As String = "011041"
        Dim ket As String = "Universitas Quality"

        Dim detailsKampusssAsal As New List(Of KampusssAsalDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDASALPTI,Nama,KET FROM ASALPTI where nama like '%" & ket & "%'   ORDER BY IDASALPTI", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKampusssAsal As New KampusssAsalDetails()
                    dataKampusssAsal.IdKampusssAsal = dtrow("IDASALPTI").ToString()
                    dataKampusssAsal.lokasiKampusssAsal = dtrow("Nama").ToString()
                    dataKampusssAsal.KetKampusssAsal = dtrow("KET").ToString()
                    detailsKampusssAsal.Add(dataKampusssAsal)
                Next
            End Using
        End Using
        Return detailsKampusssAsal.ToArray()
    End Function

    Public Class KampusssAsalDetails

        Public Property IdKampusssAsal() As String
            Get
                Return m_IdKampusssAsal
            End Get
            Set(ByVal value As String)
                m_IdKampusssAsal = value
            End Set
        End Property

        Private m_IdKampusssAsal As String

        Public Property lokasiKampusssAsal() As String
            Get
                Return m_lokasiKampusssAsal
            End Get
            Set(ByVal value As String)
                m_lokasiKampusssAsal = value
            End Set
        End Property

        Private m_lokasiKampusssAsal As String

        Public Property KetKampusssAsal() As String
            Get
                Return m_KetKampusssAsal
            End Get
            Set(ByVal value As String)
                m_KetKampusssAsal = value
            End Set
        End Property

        Private m_KetKampusssAsal As String

    End Class

    <WebMethod()>
    Public Shared Function BindDataGedungProdiAsal(Ptasal As String) As ProdiAsalDetails()
        Dim dt As New DataTable()

        Dim detailsProdiAsal As New List(Of ProdiAsalDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDASALPRODI, Nama FROM ProdiAsalFeeder where IDASALPTI='" & Ptasal & "'  ORDER BY IDASALPRODI", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataProdiAsal As New ProdiAsalDetails()
                    dataProdiAsal.IdProdiAsal = dtrow("IDASALPRODI").ToString()
                    dataProdiAsal.lokasiProdiAsal = dtrow("Nama").ToString()
                    detailsProdiAsal.Add(dataProdiAsal)
                Next
            End Using
        End Using
        Return detailsProdiAsal.ToArray()
    End Function

    Public Class ProdiAsalDetails

        Public Property IdProdiAsal() As String
            Get
                Return m_IdProdiAsal
            End Get
            Set(ByVal value As String)
                m_IdProdiAsal = value
            End Set
        End Property

        Private m_IdProdiAsal As String

        Public Property lokasiProdiAsal() As String
            Get
                Return m_lokasiProdiAsal
            End Get
            Set(ByVal value As String)
                m_lokasiProdiAsal = value
            End Set
        End Property

        Private m_lokasiProdiAsal As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataGedungProdiAsal2() As ProdiAsalDetails2()
        Dim dt As New DataTable()

        Dim detailsProdiAsal As New List(Of ProdiAsalDetails2)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDASALPRODI, Nama FROM ProdiAsalFeeder ORDER BY IDASALPRODI Asc", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataProdiAsal As New ProdiAsalDetails2()
                    dataProdiAsal.IdProdiAsal = dtrow("IDASALPRODI").ToString()
                    dataProdiAsal.lokasiProdiAsal = dtrow("Nama").ToString()
                    detailsProdiAsal.Add(dataProdiAsal)
                Next
            End Using
        End Using
        Return detailsProdiAsal.ToArray()
    End Function

    Public Class ProdiAsalDetails2

        Public Property IdProdiAsal() As String
            Get
                Return m_IdProdiAsal
            End Get
            Set(ByVal value As String)
                m_IdProdiAsal = value
            End Set
        End Property

        Private m_IdProdiAsal As String

        Public Property lokasiProdiAsal() As String
            Get
                Return m_lokasiProdiAsal
            End Get
            Set(ByVal value As String)
                m_lokasiProdiAsal = value
            End Set
        End Property

        Private m_lokasiProdiAsal As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMK2() As IDMK2Details()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK2 As New List(Of IDMK2Details)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,SEMESTER from MATAKULIAH ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK2 As New IDMK2Details()
                    dataIDMK2.IDMK = dtrow("IDMK").ToString()
                    dataIDMK2.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK2.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK2.SEMESTERIDMK = dtrow("SEMESTER").ToString()
                    IDMK2.Add(dataIDMK2)
                Next
            End Using
        End Using
        Return IDMK2.ToArray()
    End Function
    Public Class IDMK2Details
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property SEMESTERIDMK() As String
            Get
                Return m_SEMESTERIDMK
            End Get
            Set(ByVal value As String)
                m_SEMESTERIDMK = value
            End Set
        End Property
        Private m_SEMESTERIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMKKurikulum(Prodi As String) As IDMK2Details()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK2 As New List(Of IDMK2Details)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            '   Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,SEMESTER from ViewJadwalMKPengampu where PRODIMATAKULIAH = '" & Prodi & "' AND TA='" & TA & "'", con)
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,SEMESTER from matakuliah where PRODIMATAKULIAH = '" & Prodi & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK2 As New IDMK2Details()
                    dataIDMK2.IDMK = dtrow("IDMK").ToString()
                    dataIDMK2.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK2.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK2.SEMESTERIDMK = dtrow("SEMESTER").ToString()
                    IDMK2.Add(dataIDMK2)
                Next
            End Using
        End Using
        Return IDMK2.ToArray()
    End Function
    Public Class IDMKKurikulumDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property SEMESTERIDMK() As String
            Get
                Return m_SEMESTERIDMK
            End Get
            Set(ByVal value As String)
                m_SEMESTERIDMK = value
            End Set
        End Property
        Private m_SEMESTERIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdmk3() As Idmk3Details()
        Dim dt As New DataTable()

        Dim detailsIdmk3 As New List(Of Idmk3Details)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS from MATAKULIAH  ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdmk3 As New Idmk3Details()
                    dataIdmk3.IDMK = dtrow("IDMK").ToString()
                    dataIdmk3.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIdmk3.SKSIDMK = dtrow("SKS").ToString()
                    detailsIdmk3.Add(dataIdmk3)
                Next
            End Using
        End Using
        Return detailsIdmk3.ToArray()
    End Function

    Public Class Idmk3Details
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String

    End Class

    <WebMethod()>
    Public Shared Function BindDataNPM() As NPMDetails()
        Dim dt As New DataTable()

        Dim detailsNPM As New List(Of NPMDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select MAHASISWA.NPM AS NPM,MAHASISWA.NAMA AS NAMA,MAHASISWA.PRODI AS PRODI,MAHASISWA.IDKAMPUS AS IDKAMPUS,KAMPUS.LOKASI AS LOKASI,MAHASISWA.IDFAKULTAS AS IDFAKULTAS,FAKULTAS.FAKULTAS AS FAKULTAS,MAHASISWA.STAMBUK AS STAMBUK from MAHASISWA INNER JOIN KAMPUS ON MAHASISWA.IDKAMPUS=KAMPUS.IDKAMPUS INNER JOIN FAKULTAS ON MAHASISWA.IDFAKULTAS=FAKULTAS.IDFAKULTAS", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataNPM As New NPMDetails()
                    dataNPM.IDNPM = dtrow("NPM").ToString()
                    dataNPM.NAMANPM = dtrow("NAMA").ToString()
                    dataNPM.PRODINPM = dtrow("PRODI").ToString()
                    dataNPM.IDKAMPUSNPM = dtrow("IDKAMPUS").ToString()
                    dataNPM.LOKASINPM = dtrow("LOKASI").ToString()
                    dataNPM.IDFAKULTASNPM = dtrow("IDFAKULTAS").ToString()
                    dataNPM.FAKULTASNPM = dtrow("FAKULTAS").ToString()
                    dataNPM.STAMBUKNPM = dtrow("STAMBUK").ToString()
                    detailsNPM.Add(dataNPM)
                Next
            End Using
        End Using
        Return detailsNPM.ToArray()
    End Function

    Public Class NPMDetails

        Public Property IDNPM() As String
            Get
                Return m_IDNPM
            End Get
            Set(ByVal value As String)
                m_IDNPM = value
            End Set
        End Property

        Private m_IDNPM As String
        Public Property NAMANPM() As String
            Get
                Return m_NAMANPM
            End Get
            Set(ByVal value As String)
                m_NAMANPM = value
            End Set
        End Property

        Private m_NAMANPM As String
        Public Property PRODINPM() As String
            Get
                Return m_PRODINPM
            End Get
            Set(ByVal value As String)
                m_PRODINPM = value
            End Set
        End Property

        Private m_PRODINPM As String
        Public Property IDKAMPUSNPM() As String
            Get
                Return m_IDKAMPUSNPM
            End Get
            Set(ByVal value As String)
                m_IDKAMPUSNPM = value
            End Set
        End Property

        Private m_IDKAMPUSNPM As String
        Public Property LOKASINPM() As String
            Get
                Return m_LOKASINPM
            End Get
            Set(ByVal value As String)
                m_LOKASINPM = value
            End Set
        End Property

        Private m_LOKASINPM As String
        Public Property IDFAKULTASNPM() As String
            Get
                Return m_IDFAKULTASNPM
            End Get
            Set(ByVal value As String)
                m_IDFAKULTASNPM = value
            End Set
        End Property

        Private m_IDFAKULTASNPM As String
        Public Property FAKULTASNPM() As String
            Get
                Return m_FAKULTASNPM
            End Get
            Set(ByVal value As String)
                m_FAKULTASNPM = value
            End Set
        End Property

        Private m_FAKULTASNPM As String
        Public Property STAMBUKNPM() As String
            Get
                Return m_STAMBUKNPM
            End Get
            Set(ByVal value As String)
                m_STAMBUKNPM = value
            End Set
        End Property

        Private m_STAMBUKNPM As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataFakultas() As FakultasDetails()
        Dim dt As New DataTable()

        Dim detailsFakultas As New List(Of FakultasDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT IDFakultas,Fakultas FROM Fakultas ORDER BY IDFakultas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataFakultas As New FakultasDetails()
                    dataFakultas.IdFakultas = dtrow("IDFakultas").ToString()
                    dataFakultas.lokasiFakultas = dtrow("Fakultas").ToString()
                    detailsFakultas.Add(dataFakultas)
                Next
            End Using
        End Using
        Return detailsFakultas.ToArray()
    End Function

    Public Class FakultasDetails

        Public Property IdFakultas() As String
            Get
                Return m_IdFakultas
            End Get
            Set(ByVal value As String)
                m_IdFakultas = value
            End Set
        End Property

        Private m_IdFakultas As String

        Public Property lokasiFakultas() As String
            Get
                Return m_lokasiFakultas
            End Get
            Set(ByVal value As String)
                m_lokasiFakultas = value
            End Set
        End Property

        Private m_lokasiFakultas As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataNpmNilai() As NpmNilaiDetails()
        Dim dt As New DataTable()

        Dim detailsNpmNilai As New List(Of NpmNilaiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select TOP 1 PERCENT mahasiswa.npm, mahasiswa.nama, ProdiFakultas.prodi, FAKULTAS.IDFAKULTAS, FAKULTAS.FAKULTAS, mahasiswa.kurikulum, mahasiswa.stambuk from ((Mahasiswa inner join ProdiFakultas on  mahasiswa.IDFAKULTAS = ProdiFakultas.IDFAKULTAS) inner join FAKULTAS on mahasiswa.IDFAKULTAS = FAKULTAS.IDFAKULTAS) order by MAHASISWA.Npm ASC", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataNpmNilai As New NpmNilaiDetails()
                    dataNpmNilai.NpmNilai = dtrow("Npm").ToString()
                    dataNpmNilai.NamaNpmNilai = dtrow("Nama").ToString()
                    dataNpmNilai.ProdiNpmNilai = dtrow("PRODI").ToString()
                    dataNpmNilai.IdFakultasNpmNilai = dtrow("IdFakultas").ToString()
                    dataNpmNilai.FakultasNpmNilai = dtrow("Fakultas").ToString()
                    dataNpmNilai.KurikulumNpmNilai = dtrow("Kurikulum").ToString()
                    dataNpmNilai.StambukNpmNilai = dtrow("Stambuk").ToString()
                    detailsNpmNilai.Add(dataNpmNilai)
                Next
            End Using
        End Using
        Return detailsNpmNilai.ToArray()
    End Function

    Public Class NpmNilaiDetails

        Public Property NpmNilai() As String
            Get
                Return m_NpmNilai
            End Get
            Set(ByVal value As String)
                m_NpmNilai = value
            End Set
        End Property

        Private m_NpmNilai As String

        Public Property NamaNpmNilai() As String
            Get
                Return m_NamaNpmNilai
            End Get
            Set(ByVal value As String)
                m_NamaNpmNilai = value
            End Set
        End Property

        Private m_NamaNpmNilai As String

        Public Property ProdiNpmNilai() As String
            Get
                Return m_ProdiNpmNilai
            End Get
            Set(ByVal value As String)
                m_ProdiNpmNilai = value
            End Set
        End Property

        Private m_ProdiNpmNilai As String

        Public Property IdFakultasNpmNilai() As String
            Get
                Return m_IdFakultasNpmNilai
            End Get
            Set(ByVal value As String)
                m_IdFakultasNpmNilai = value
            End Set
        End Property

        Private m_IdFakultasNpmNilai As String

        Public Property FakultasNpmNilai() As String
            Get
                Return m_FakultasNpmNilai
            End Get
            Set(ByVal value As String)
                m_FakultasNpmNilai = value
            End Set
        End Property

        Private m_FakultasNpmNilai As String

        Public Property KurikulumNpmNilai() As String
            Get
                Return m_KurikulumNpmNilai
            End Get
            Set(ByVal value As String)
                m_KurikulumNpmNilai = value
            End Set
        End Property

        Private m_KurikulumNpmNilai As String

        Public Property StambukNpmNilai() As String
            Get
                Return m_StambukNpmNilai
            End Get
            Set(ByVal value As String)
                m_StambukNpmNilai = value
            End Set
        End Property

        Private m_StambukNpmNilai As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataProdiALL() As ProdiALLDetails()
        Dim dt As New DataTable()

        Dim detailsProdiALL As New List(Of ProdiALLDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Prodi FROM ProdiFakultas ORDER BY Prodi", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataProdiALL As New ProdiALLDetails()
                    dataProdiALL.IdProdiALL = dtrow("Prodi").ToString()
                    detailsProdiALL.Add(dataProdiALL)
                Next
            End Using
        End Using
        Return detailsProdiALL.ToArray()
    End Function

    Public Class ProdiALLDetails

        Public Property IdProdiALL() As String
            Get
                Return m_IdProdiALL
            End Get
            Set(ByVal value As String)
                m_IdProdiALL = value
            End Set
        End Property

        Private m_IdProdiALL As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataMenu() As MenuDetails()
        Dim dt As New DataTable()
        Dim statusketuser As String = "Super Admin"
        Dim detailsMenu As New List(Of MenuDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT a.Menu_id as menu, b.menuweb as namamenu FROM TblSementaraHakAkses a Left Outer Join SM_SnapsWeb b on b.menu_id=a.menu_id where a.StatusUser='" & statusketuser & "' ORDER BY Menu", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataMenu As New MenuDetails()
                    dataMenu.IdMenu = dtrow("Menu").ToString()
                    dataMenu.NamaMenu = dtrow("namamenu").ToString()
                    detailsMenu.Add(dataMenu)
                Next
            End Using
        End Using
        Return detailsMenu.ToArray()
    End Function

    Public Class MenuDetails

        Public Property IdMenu() As String
            Get
                Return m_IdMenu
            End Get
            Set(ByVal value As String)
                m_IdMenu = value
            End Set
        End Property

        Private m_IdMenu As String
        Public Property NamaMenu() As String
            Get
                Return m_NamaMenu
            End Get
            Set(ByVal value As String)
                m_NamaMenu = value
            End Set
        End Property

        Private m_NamaMenu As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelas() As KelasDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Kelas FROM Kelas ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function

    Public Class KelasDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataAkademik() As AkademikDetails()
        Dim dt As New DataTable()

        Dim detailsAkademik As New List(Of AkademikDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT JabatanAkademik FROM TblAkademik ORDER BY idPrimary", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAkademik As New AkademikDetails()
                    dataAkademik.IdAkademik = dtrow("JabatanAkademik").ToString()
                    detailsAkademik.Add(dataAkademik)
                Next
            End Using
        End Using
        Return detailsAkademik.ToArray()
    End Function

    Public Class AkademikDetails

        Public Property IdAkademik() As String
            Get
                Return m_IdAkademik
            End Get
            Set(ByVal value As String)
                m_IdAkademik = value
            End Set
        End Property

        Private m_IdAkademik As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKum() As KumDetails()
        Dim dt As New DataTable()

        Dim detailsKum As New List(Of KumDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT KUM FROM KUM ORDER BY idPrimary", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKum As New KumDetails()
                    dataKum.IdKum = dtrow("Kum").ToString()
                    detailsKum.Add(dataKum)
                Next
            End Using
        End Using
        Return detailsKum.ToArray()
    End Function

    Public Class KumDetails

        Public Property IdKum() As String
            Get
                Return m_IdKum
            End Get
            Set(ByVal value As String)
                m_IdKum = value
            End Set
        End Property

        Private m_IdKum As String
    End Class

    <WebMethod()>
    Public Shared Function BindPangkatDosen() As PangkatDetails()
        Dim dt As New DataTable()

        Dim detailsPangkat As New List(Of PangkatDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT golongan as golongan, Keterangan as Keterangan FROM GolonganPegawai ORDER BY idPrimary", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataPangkat As New PangkatDetails()
                    dataPangkat.IdPangkat = dtrow("keterangan").ToString()
                    dataPangkat.Namagol = dtrow("golongan").ToString()
                    detailsPangkat.Add(dataPangkat)
                Next
            End Using
        End Using
        Return detailsPangkat.ToArray()
    End Function

    Public Class PangkatDetails

        Public Property IdPangkat() As String
            Get
                Return m_IdPangkat
            End Get
            Set(ByVal value As String)
                m_IdPangkat = value
            End Set
        End Property

        Private m_IdPangkat As String
        Public Property Namagol() As String
            Get
                Return m_Namagol
            End Get
            Set(ByVal value As String)
                m_Namagol = value
            End Set
        End Property

        Private m_Namagol As String
    End Class


    <WebMethod()>
    Public Shared Function BinProfiledDosen() As DosenDetails()
        Dim dt As New DataTable()

        Dim detailsDosen As New List(Of DosenDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT iddosen as iddosen, nama as nama,NIDNNTBDOS AS NIDNNTBDOS FROM Dosen ORDER BY iddosen", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataDosen As New DosenDetails()
                    dataDosen.IdDosen = dtrow("iddosen").ToString()
                    dataDosen.Nama = dtrow("nama").ToString()
                    dataDosen.nidnntbdos = dtrow("nidnntbdos").ToString()
                    detailsDosen.Add(dataDosen)
                Next
            End Using
        End Using
        Return detailsDosen.ToArray()
    End Function

    Public Class DosenDetails

        Public Property iddosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_iddosen As String
        Public Property Nama() As String
            Get
                Return m_Nama
            End Get
            Set(ByVal value As String)
                m_Nama = value
            End Set
        End Property

        Private m_Nama As String
        Public Property nidnntbdos() As String
            Get
                Return m_nidnntbdos
            End Get
            Set(ByVal value As String)
                m_nidnntbdos = value
            End Set
        End Property

        Private m_nidnntbdos As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataJabatDosen() As JabatDosenDetails()
        Dim dt As New DataTable()

        Dim detailsJabatDosen As New List(Of JabatDosenDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Jabatan FROM Jabatan ORDER BY idPrimary", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataJabatDosen As New JabatDosenDetails()
                    dataJabatDosen.IdJabatDosen = dtrow("Jabatan").ToString()
                    detailsJabatDosen.Add(dataJabatDosen)
                Next
            End Using
        End Using
        Return detailsJabatDosen.ToArray()
    End Function

    Public Class JabatDosenDetails

        Public Property IdJabatDosen() As String
            Get
                Return m_IdJabatDosen
            End Get
            Set(ByVal value As String)
                m_IdJabatDosen = value
            End Set
        End Property

        Private m_IdJabatDosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataStatusDosen() As StatusDosenDetails()
        Dim dt As New DataTable()

        Dim detailsStatusDosen As New List(Of StatusDosenDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT StatusDosen FROM StatusDosenAja ORDER BY idPrimary", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataStatusDosen As New StatusDosenDetails()
                    dataStatusDosen.IdStatusDosen = dtrow("StatusDosen").ToString()
                    detailsStatusDosen.Add(dataStatusDosen)
                Next
            End Using
        End Using
        Return detailsStatusDosen.ToArray()
    End Function

    Public Class StatusDosenDetails

        Public Property IdStatusDosen() As String
            Get
                Return m_IdStatusDosen
            End Get
            Set(ByVal value As String)
                m_IdStatusDosen = value
            End Set
        End Property

        Private m_IdStatusDosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataProdiGabungan() As ProdiGabunganDetails()
        Dim dt As New DataTable()

        Dim detailsProdi As New List(Of ProdiGabunganDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(Prodi) FROM prodi ORDER BY prodi", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataProdi As New ProdiGabunganDetails()
                    dataProdi.Idprodi = dtrow("prodi").ToString()
                    detailsProdi.Add(dataProdi)
                Next
            End Using
        End Using
        Return detailsProdi.ToArray()
    End Function

    Public Class ProdiGabunganDetails

        Public Property Idprodi() As String
            Get
                Return m_IdProdi
            End Get
            Set(ByVal value As String)
                m_IdProdi = value
            End Set
        End Property

        Private m_IdProdi As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasValidasi(TA As String, Prodi As String, IdMK As String) As KelasValidasiDetails()
        Dim dt As New DataTable()
        Dim Valid As String = "T"

        Dim detailsKelas As New List(Of KelasValidasiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where PRODI = '" & Prodi & "' AND IdMK='" & IdMK & "' AND Validasi='" & Valid & "' AND TA='" & TA & "' ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function

   
    Public Class KelasValidasiDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasUjian(ByVal TA As String, ByVal Prodi As String, ByVal Semester As String) As KelasUjianDetails()
        Dim dt As New DataTable()
        Dim Valid As String = "T"

        Dim detailsUjianKelas As New List(Of KelasUjianDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) as kelas FROM JadwalPrimary where PRODI = '" & Prodi & "' AND Semester='" & Semester & "' AND Validasi='" & Valid & "' AND TA='" & TA & "' ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelasUjian As New KelasUjianDetails()
                    dataKelasUjian.IdKelas = dtrow("Kelas").ToString()
                    detailsUjianKelas.Add(dataKelasUjian)
                Next
            End Using
        End Using
        Return detailsUjianKelas.ToArray()
    End Function


    Public Class KelasUjianDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class
    <WebMethod()>
    Public Shared Function BindDataKelasGabungan(TA As String, harijadwal As String) As KelassValidasiDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelassValidasiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' and harijadwal='" & harijadwal & "' ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelassValidasiDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelassValidasiDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class
    <WebMethod()>
    Public Shared Function BindDataKelasGabunganSenin(TA As String) As KelasValidasiSeninDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiSeninDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 1 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiSeninDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiSeninDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasGabunganSelasa(TA As String) As KelasValidasiSelasaDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiSelasaDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 2 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiSelasaDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiSelasaDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasGabunganRabu(TA As String) As KelasValidasiRabuDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiRabuDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 3 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiRabuDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiRabuDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasGabunganKamis(TA As String) As KelasValidasiKamisDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiKamisDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 4 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiKamisDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiKamisDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasGabunganJumat(TA As String) As KelasValidasiJumatDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiJumatDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 5 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiJumatDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiJumatDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKelasGabunganSabtu(TA As String) As KelasValidasiSabtuDetails()
        Dim dt As New DataTable()

        Dim detailsKelas As New List(Of KelasValidasiSabtuDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT Distinct(kelas) FROM JadwalPrimary where TA='" & TA & "' AND harijadwal= 6 ORDER BY Kelas", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKelas As New KelasValidasiSabtuDetails()
                    dataKelas.IdKelas = dtrow("Kelas").ToString()
                    detailsKelas.Add(dataKelas)
                Next
            End Using
        End Using
        Return detailsKelas.ToArray()
    End Function
    Public Class KelasValidasiSabtuDetails

        Public Property IdKelas() As String
            Get
                Return m_IdKelas
            End Get
            Set(ByVal value As String)
                m_IdKelas = value
            End Set
        End Property

        Private m_IdKelas As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataKurikulum() As KurikulumDetails()
        Dim dt As New DataTable()

        Dim detailsKurikulum As New List(Of KurikulumDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("SELECT kurikulum,TAHUNAJARAN1,TAHUNAJARAN2,DEFAULTKURIKULUM FROM Kurikulum ORDER BY Kurikulum", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataKurikulum As New KurikulumDetails()
                    dataKurikulum.IdKurikulum = dtrow("kurikulum").ToString()
                    dataKurikulum.TA1Kurikulum = dtrow("TAHUNAJARAN1").ToString()
                    dataKurikulum.TA2Kurikulum = dtrow("TAHUNAJARAN2").ToString()
                    dataKurikulum.DefKurikulum = dtrow("DEFAULTKURIKULUM").ToString()
                    detailsKurikulum.Add(dataKurikulum)
                Next
            End Using
        End Using
        Return detailsKurikulum.ToArray()
    End Function

    Public Class KurikulumDetails

        Public Property IdKurikulum() As String
            Get
                Return m_IdKurikulum
            End Get
            Set(ByVal value As String)
                m_IdKurikulum = value
            End Set
        End Property

        Private m_IdKurikulum As String
        Public Property TA1Kurikulum() As String
            Get
                Return m_TA1Kurikulum
            End Get
            Set(ByVal value As String)
                m_TA1Kurikulum = value
            End Set
        End Property

        Private m_TA1Kurikulum As String
        Public Property TA2Kurikulum() As String
            Get
                Return m_TA2Kurikulum
            End Get
            Set(ByVal value As String)
                m_TA2Kurikulum = value
            End Set
        End Property

        Private m_TA2Kurikulum As String
        Public Property DefKurikulum() As String
            Get
                Return m_DefKurikulum
            End Get
            Set(ByVal value As String)
                m_DefKurikulum = value
            End Set
        End Property

        Private m_DefKurikulum As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdDosenPengampu(Dosen As String) As IdDosenDetailsPengampu()
        Dim dt As New DataTable()

        Dim detailsIdDosen As New List(Of IdDosenDetailsPengampu)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select Distinct idDosenPengampu,NAMADOSEN from MKTASEMESTER2 where idmk ='" & Dosen & "' order by idDosenPengampu", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen As New IdDosenDetailsPengampu()
                    dataIdDosen.IdDosen = dtrow("idDosenPengampu").ToString()
                    dataIdDosen.NamaDosen = dtrow("NAMADOSEN").ToString()
                    detailsIdDosen.Add(dataIdDosen)
                Next
            End Using
        End Using
        Return detailsIdDosen.ToArray()
    End Function

    Public Class IdDosenDetailsPengampu

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property NamaDosen() As String
            Get
                Return m_NamaDosen
            End Get
            Set(ByVal value As String)
                m_NamaDosen = value
            End Set
        End Property

        Private m_NamaDosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdDosen(Dosen As String) As IdDosenDetails()
        Dim dt As New DataTable()

        Dim detailsIdDosen As New List(Of IdDosenDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IdDosen,Nama,FORMAT(HONORSKS,'#,###,##0') as HONORSKS from MKDOSEN2 where idmk='" & Dosen & "'  order by IdDosen ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen As New IdDosenDetails()
                    dataIdDosen.IdDosen = dtrow("IdDosen").ToString()
                    dataIdDosen.NamaDosen = dtrow("Nama").ToString()
                    dataIdDosen.HonorDosen = dtrow("HONORSKS").ToString()
                    detailsIdDosen.Add(dataIdDosen)
                Next
            End Using
        End Using
        Return detailsIdDosen.ToArray()
    End Function

    Public Class IdDosenDetails

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property NamaDosen() As String
            Get
                Return m_NamaDosen
            End Get
            Set(ByVal value As String)
                m_NamaDosen = value
            End Set
        End Property

        Private m_NamaDosen As String
        Public Property HonorDosen() As String
            Get
                Return m_HonorDosen
            End Get
            Set(ByVal value As String)
                m_HonorDosen = value
            End Set
        End Property

        Private m_HonorDosen As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataIdDosenGlobal() As IdDosenGlobalDetails()
        Dim dt As New DataTable()

        Dim detailsIdDosen As New List(Of IdDosenGlobalDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IdDosen,Nama,FORMAT(HONORSKS,'#,###,##0') as HONORSKS from DOSEN  order by IdDosen ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen As New IdDosenGlobalDetails()
                    dataIdDosen.IdDosen = dtrow("IdDosen").ToString()
                    dataIdDosen.NamaDosen = dtrow("Nama").ToString()
                    dataIdDosen.HonorDosen = dtrow("HONORSKS").ToString()
                    detailsIdDosen.Add(dataIdDosen)
                Next
            End Using
        End Using
        Return detailsIdDosen.ToArray()
    End Function

    Public Class IdDosenGlobalDetails

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property NamaDosen() As String
            Get
                Return m_NamaDosen
            End Get
            Set(ByVal value As String)
                m_NamaDosen = value
            End Set
        End Property

        Private m_NamaDosen As String
        Public Property HonorDosen() As String
            Get
                Return m_HonorDosen
            End Get
            Set(ByVal value As String)
                m_HonorDosen = value
            End Set
        End Property

        Private m_HonorDosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdDosen3() As IdDosenDetails3()
        Dim dt As New DataTable()

        Dim detailsIdDosen As New List(Of IdDosenDetails3)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IdDosen,Nama from dosen  order by IdDosen ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen As New IdDosenDetails3()
                    dataIdDosen.IdDosen = dtrow("IdDosen").ToString()
                    dataIdDosen.NamaDosen = dtrow("Nama").ToString()
                    detailsIdDosen.Add(dataIdDosen)
                Next
            End Using
        End Using
        Return detailsIdDosen.ToArray()
    End Function

    Public Class IdDosenDetails3

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property NamaDosen() As String
            Get
                Return m_NamaDosen
            End Get
            Set(ByVal value As String)
                m_NamaDosen = value
            End Set
        End Property

        Private m_NamaDosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdRuang(SKS As String) As IdRuangDetails()
        Dim dt As New DataTable()

        Dim detailsIdRuang As New List(Of IdRuangDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IDRUANG,JAMMASUK,JAMKELUAR,LANTAI,IDRUANGINDEX from RuangJam2 WHERE SKS =" & SKS & " order by IDRUANG", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdRuang As New IdRuangDetails()
                    dataIdRuang.IdRuang = dtrow("IDRUANG").ToString()
                    dataIdRuang.JamMasuk = dtrow("JAMMASUK").ToString()
                    dataIdRuang.JamKeluar = dtrow("JAMKELUAR").ToString()
                    dataIdRuang.Lantai = dtrow("LANTAI").ToString()
                    dataIdRuang.IdRuangIndex = dtrow("IDRUANGINDEX").ToString()
                    detailsIdRuang.Add(dataIdRuang)
                Next
            End Using
        End Using
        Return detailsIdRuang.ToArray()
    End Function
    Public Class IdRuangDetails

        Public Property IdRuang() As String
            Get
                Return m_IdRuang
            End Get
            Set(ByVal value As String)
                m_IdRuang = value
            End Set
        End Property

        Private m_IdRuang As String
        Public Property JamMasuk() As String
            Get
                Return m_JamMasuk
            End Get
            Set(ByVal value As String)
                m_JamMasuk = value
            End Set
        End Property

        Private m_JamMasuk As String
        Public Property JamKeluar() As String
            Get
                Return m_JamKeluar
            End Get
            Set(ByVal value As String)
                m_JamKeluar = value
            End Set
        End Property

        Private m_JamKeluar As String
        Public Property Lantai() As String
            Get
                Return m_Lantai
            End Get
            Set(ByVal value As String)
                m_Lantai = value
            End Set
        End Property

        Private m_Lantai As String
        Public Property IdRuangIndex() As String
            Get
                Return m_IdRuangIndex
            End Get
            Set(ByVal value As String)
                m_IdRuangIndex = value
            End Set
        End Property

        Private m_IdRuangIndex As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdRuang2() As IdRuangDetails2()
        Dim dt As New DataTable()

        Dim detailsIdRuang As New List(Of IdRuangDetails2)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IDRUANG,JAMMASUK,JAMKELUAR,LANTAI,IDRUANGINDEX from RuangJam2 where lantai <> '' order by IDRUANG asc", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdRuang As New IdRuangDetails2()
                    dataIdRuang.IdRuang = dtrow("IDRUANG").ToString()
                    dataIdRuang.JamMasuk = dtrow("JAMMASUK").ToString()
                    dataIdRuang.JamKeluar = dtrow("JAMKELUAR").ToString()
                    dataIdRuang.Lantai = dtrow("LANTAI").ToString()
                    dataIdRuang.IdRuangIndex = dtrow("IDRUANGINDEX").ToString()
                    detailsIdRuang.Add(dataIdRuang)
                Next
            End Using
        End Using
        Return detailsIdRuang.ToArray()
    End Function
    Public Class IdRuangDetails2

        Public Property IdRuang() As String
            Get
                Return m_IdRuang
            End Get
            Set(ByVal value As String)
                m_IdRuang = value
            End Set
        End Property

        Private m_IdRuang As String
        Public Property JamMasuk() As String
            Get
                Return m_JamMasuk
            End Get
            Set(ByVal value As String)
                m_JamMasuk = value
            End Set
        End Property

        Private m_JamMasuk As String
        Public Property JamKeluar() As String
            Get
                Return m_JamKeluar
            End Get
            Set(ByVal value As String)
                m_JamKeluar = value
            End Set
        End Property

        Private m_JamKeluar As String
        Public Property Lantai() As String
            Get
                Return m_Lantai
            End Get
            Set(ByVal value As String)
                m_Lantai = value
            End Set
        End Property

        Private m_Lantai As String
        Public Property IdRuangIndex() As String
            Get
                Return m_IdRuangIndex
            End Get
            Set(ByVal value As String)
                m_IdRuangIndex = value
            End Set
        End Property

        Private m_IdRuangIndex As String
    End Class










    <WebMethod()>
    Public Shared Function BindDataIDMK(IdKampus As String, Kurikulum As String, Prodi As String, Semester As String, TA As String) As IDMKDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,NAMADOSEN,IdDosenPengampu from ViewJadwalMKPengampu WHERE  IDMK IN (Select idmk from prodimk where PRODI = '" & Prodi & "' AND kurikulum = '" & Kurikulum & "' AND IdKampus = '" & IdKampus & "') AND Semester = '" & Semester & "' AND TA = '" & TA & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.DOSENIDMK = dtrow("NAMADOSEN").ToString()
                    dataIDMK.IDDOSENIDMK = dtrow("IdDosenPengampu").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function

    <WebMethod()>
    Public Shared Function BindDataIDMKKonversi(IdKampus As String, Kurikulum As String, Prodi As String, Semester As String, TA As String) As IDMKKonversiDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKKonversiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,SemesterKul from ViewJadwalMKPengampu WHERE  IDMK IN (Select idmk from prodimk where PRODI = '" & Prodi & "' AND kurikulum = '" & Kurikulum & "' AND IdKampus = '" & IdKampus & "') AND Semester = '" & Semester & "' AND TA = '" & TA & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKKonversiDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.SEMESTERIDMK = dtrow("SemesterKul").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKKonversiDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property SEMESTERIDMK() As String
            Get
                Return m_SEMESTERIDMK
            End Get
            Set(ByVal value As String)
                m_SEMESTERIDMK = value
            End Set
        End Property
        Private m_SEMESTERIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataMataKuliahKonversi(Kurikulum As String, Prodi As String, Kampus As String) As MataKuliahKonversiDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of MataKuliahKonversiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,Semester from KonversiMataKuliah WHERE kurikulum='" & Kurikulum & "' and prodi='" & Prodi & "' and idkampus='" & Kampus & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New MataKuliahKonversiDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.SEMESTERIDMK = dtrow("Semester").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class MataKuliahKonversiDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property SEMESTERIDMK() As String
            Get
                Return m_SEMESTERIDMK
            End Get
            Set(ByVal value As String)
                m_SEMESTERIDMK = value
            End Set
        End Property
        Private m_SEMESTERIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMKAsal(prodi As String, idkampus As String) As IDMKAsallDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKAsallDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select a.IDMK as IDMK,A.MATAKULIAH AS MATAKULIAH,a.SKS as SKS,IDMK2,a.MATAKULIAH2 as MATAKULIAH2,a.SKS2 as SKS2,a.statusMK as statusMK,a.HasilPengakuan as HasilPengakuan ,a.kurikulum as kurikulum From matakuliahAsal a left outer join feederptasal b on b.kode_perguruan_tinggi=a.idkampus Left outer join prodiasalfeeder c ON c.IDASALPRODI = a.PRODIMATAKULIAH AND c.IDASALPTI = a.IDKAMPUS LEFT outer join AsalPTIFeeder d on d.kode_perguruan_tinggi=a.idkampus WHERE a.prodimatakuliah = '" & prodi & "' and a.idkampus='" & idkampus & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKAsallDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.IDMK2 = dtrow("IDMK2").ToString()
                    dataIDMK.MKIDMK2 = dtrow("MATAKULIAH2").ToString()
                    dataIDMK.SKSIDMK2 = dtrow("SKS2").ToString()
                    dataIDMK.HasilPengakuan = dtrow("HasilPengakuan").ToString()
                    dataIDMK.statusMK2 = dtrow("statusMK").ToString()
                    dataIDMK.Kurikulum2 = dtrow("Kurikulum").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKAsallDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property HasilPengakuan() As String
            Get
                Return m_HasilPengakuan
            End Get
            Set(ByVal value As String)
                m_HasilPengakuan = value
            End Set
        End Property
        Private m_HasilPengakuan As String
        Public Property IDMK2() As String
            Get
                Return m_IDMK2
            End Get
            Set(ByVal value As String)
                m_IDMK2 = value
            End Set
        End Property
        Private m_IDMK2 As String
        Public Property MKIDMK2() As String
            Get
                Return m_MKIDMK2
            End Get
            Set(ByVal value As String)
                m_MKIDMK2 = value
            End Set
        End Property
        Private m_MKIDMK2 As String
        Public Property SKSIDMK2() As String
            Get
                Return m_SKSIDMK2
            End Get
            Set(ByVal value As String)
                m_SKSIDMK2 = value
            End Set
        End Property
        Private m_SKSIDMK2 As String
        Public Property statusMK2() As String
            Get
                Return m_statusMK2
            End Get
            Set(ByVal value As String)
                m_statusMK2 = value
            End Set
        End Property
        Private m_statusMK2 As String
        Public Property Kurikulum2() As String
            Get
                Return m_Kurikulum2
            End Get
            Set(ByVal value As String)
                m_Kurikulum2 = value
            End Set
        End Property
        Private m_Kurikulum2 As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMKSelasa(Kurikulum As String, Prodi As String, IdKampus As String, Semester As String) As IDMKSelasaDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKSelasaDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,honor,NAMADOSEN,IdDosenPengampu from ViewJadwalMKPengampu WHERE IDMK in (SELECT IDMK FROM PRODIMK where PRODI = '" & Prodi & "' AND IDKAMPUS = '" & IdKampus & "' AND kurikulum = '" & Kurikulum & "') AND TA='" & Kurikulum & "' AND Semester='" & Semester & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKSelasaDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.HONORIDMK = dtrow("honor").ToString()
                    dataIDMK.DOSENIDMK = dtrow("NAMADOSEN").ToString()
                    dataIDMK.IDDOSENIDMK = dtrow("IdDosenPengampu").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKSelasaDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property HONORIDMK() As String
            Get
                Return m_HONORIDMK
            End Get
            Set(ByVal value As String)
                m_HONORIDMK = value
            End Set
        End Property
        Private m_HONORIDMK As String
        Public Property DOSENIDMK() As String
            Get
                Return m_DOSENIDMK
            End Get
            Set(ByVal value As String)
                m_DOSENIDMK = value
            End Set
        End Property
        Private m_DOSENIDMK As String
        Public Property IDDOSENIDMK() As String
            Get
                Return m_IDDOSENIDMK
            End Get
            Set(ByVal value As String)
                m_IDDOSENIDMK = value
            End Set
        End Property
        Private m_IDDOSENIDMK As String
    End Class
    <WebMethod()>
    Public Shared Function BindDataIDMKValidasi(ByVal TA As String, ByVal Prodi As String, ByVal Semester As String) As IDMKValidasiDetails()
        Dim dt As New DataTable()
        Dim valid As String = "T"
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKValidasiDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select Kelas, IDMK,MATAKULIAH,SKS,Validasi from matakuliahvalidasi WHERE PRODI = '" & Prodi & "' AND Semester='" & Semester & "' AND Validasi='" & valid & "' AND TA='" & TA & "' Group by Kelas, IDMK,MATAKULIAH,SKS,Validasi", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKValidasiDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKValidasiDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
    End Class


    <WebMethod()>
    Public Shared Function BindDataIDMKUjian(ByVal TA As String, ByVal Prodi As String, ByVal Semester As String) As IDMKUjianDetails()
        Dim dt As New DataTable()
        Dim valid As String = "T"
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKUjianDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS from Matakuliah where idmk in (Select idmk from jadwalprimary WHERE PRODI = '" & Prodi & "' AND Semester='" & Semester & "' AND TA='" & TA & "')", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKUjianDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKUjianDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMK3(Kurikulum As String, Prodi As String, IdKampus As String) As IDMKDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,honor from MATAKULIAH WHERE IDMK in (SELECT IDMK FROM PRODIMK where PRODI = '" & Prodi & "' AND IDKAMPUS = '" & IdKampus & "')", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function

    Public Class IDMKDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property DOSENIDMK() As String
            Get
                Return m_DOSENIDMK
            End Get
            Set(ByVal value As String)
                m_DOSENIDMK = value
            End Set
        End Property
        Private m_DOSENIDMK As String
        Public Property IDDOSENIDMK() As String
            Get
                Return m_IDDOSENIDMK
            End Get
            Set(ByVal value As String)
                m_IDDOSENIDMK = value
            End Set
        End Property
        Private m_IDDOSENIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMKDosen(IdKampus As String, Kurikulum As String, Prodi As String) As IDMKDetailsDosen()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKDetailsDosen)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH from Matakuliah WHERE  IDMK IN (Select idmk from prodimk where PRODI = '" & Prodi & "' AND kurikulum = '" & Kurikulum & "' AND IdKampus = '" & IdKampus & "') And Prodimatakuliah= '" & Prodi & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKDetailsDosen()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function

    Public Class IDMKDetailsDosen
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String

    End Class

    <WebMethod()>
    Public Shared Function BindDataIDMKMBKM(Kurikulum As String, Prodi As String, IdKampus As String) As IDMKMBKMDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKMBKMDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS from MATAKULIAH WHERE IDMK in (SELECT IDMK FROM PRODIMK where PRODI = '" & Prodi & "' AND IDKAMPUS = '" & IdKampus & "' AND KURIKULUM ='" & Kurikulum & "')", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKMBKMDetails()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function

    Public Class IDMKMBKMDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataMK(ProdiMK As String) As IDMKDetails3()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim IDMK As New List(Of IDMKDetails3)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS,SEMESTER from MATAKULIAH where prodimatakuliah ='" & ProdiMK & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIDMK As New IDMKDetails3()
                    dataIDMK.IDMK = dtrow("IDMK").ToString()
                    dataIDMK.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataIDMK.SKSIDMK = dtrow("SKS").ToString()
                    dataIDMK.SEMESTERIDMK = dtrow("semester").ToString()
                    IDMK.Add(dataIDMK)
                Next
            End Using
        End Using
        Return IDMK.ToArray()
    End Function
    Public Class IDMKDetails3
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
        Public Property SEMESTERIDMK() As String
            Get
                Return m_SEMESTERIDMK
            End Get
            Set(ByVal value As String)
                m_SEMESTERIDMK = value
            End Set
        End Property
        Private m_SEMESTERIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataAktivitas() As AktivitasDetails()
        Dim dt As New DataTable()
        'LEFT(Alamat,30)
        Dim Aktivitas As New List(Of AktivitasDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select IDMK,MATAKULIAH,SKS from MATAKULIAH", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAktivitas As New AktivitasDetails()
                    dataAktivitas.IDMK = dtrow("IDMK").ToString()
                    dataAktivitas.MKIDMK = dtrow("MATAKULIAH").ToString()
                    dataAktivitas.SKSIDMK = dtrow("SKS").ToString()
                    Aktivitas.Add(dataAktivitas)
                Next
            End Using
        End Using
        Return Aktivitas.ToArray()
    End Function
    Public Class AktivitasDetails
        Public Property IDMK() As String
            Get
                Return m_IDMK
            End Get
            Set(ByVal value As String)
                m_IDMK = value
            End Set
        End Property
        Private m_IDMK As String
        Public Property MKIDMK() As String
            Get
                Return m_MKIDMK
            End Get
            Set(ByVal value As String)
                m_MKIDMK = value
            End Set
        End Property
        Private m_MKIDMK As String
        Public Property SKSIDMK() As String
            Get
                Return m_SKSIDMK
            End Get
            Set(ByVal value As String)
                m_SKSIDMK = value
            End Set
        End Property
        Private m_SKSIDMK As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataAbsenAkademik(tgl1 As String, tgl2 As String) As AbsenAkademikDetails()
        Dim dt As New DataTable()

        Dim detailsAbsenAkademik As New List(Of AbsenAkademikDetails)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("Select KELAS,MATAKULIAH,IDDOSEN,NAMA,TGL,TGLGANTI,TGLIN,TGLABSENIDDOSEN from ATTDOSENRUANG2 where TGL = '" & clsEnt.CDateME(tgl1) & "' AND TGLGANTI = '" & clsEnt.CDateME(tgl2) & "'", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataAbsenAkademik As New AbsenAkademikDetails()
                    dataAbsenAkademik.KelasAkademik = dtrow("KELAS").ToString()
                    dataAbsenAkademik.MataKuliahAkademik = dtrow("MATAKULIAH").ToString()
                    dataAbsenAkademik.DosenAkademik = dtrow("IDDOSEN").ToString()
                    dataAbsenAkademik.NamaAkademik = dtrow("NAMA").ToString()
                    dataAbsenAkademik.TglAkademik = dtrow("TGL").ToString()
                    dataAbsenAkademik.TglGantiAkademik = dtrow("TGLGANTI").ToString()
                    dataAbsenAkademik.TglInAkademik = dtrow("TGLIN").ToString()
                    dataAbsenAkademik.TglAbsenAkademik = dtrow("TGLABSENIDDOSEN").ToString()
                    detailsAbsenAkademik.Add(dataAbsenAkademik)
                Next
            End Using
        End Using
        Return detailsAbsenAkademik.ToArray()
    End Function

    Public Class AbsenAkademikDetails

        Public Property KelasAkademik() As String
            Get
                Return m_KelasAkademik
            End Get
            Set(ByVal value As String)
                m_KelasAkademik = value
            End Set
        End Property

        Private m_KelasAkademik As String

        Public Property MataKuliahAkademik() As String
            Get
                Return m_MataKuliahAkademik
            End Get
            Set(ByVal value As String)
                m_MataKuliahAkademik = value
            End Set
        End Property

        Private m_MataKuliahAkademik As String

        Public Property DosenAkademik() As String
            Get
                Return m_DosenAkademik
            End Get
            Set(ByVal value As String)
                m_DosenAkademik = value
            End Set
        End Property

        Private m_DosenAkademik As String

        Public Property NamaAkademik() As String
            Get
                Return m_NamaAkademik
            End Get
            Set(ByVal value As String)
                m_NamaAkademik = value
            End Set
        End Property

        Private m_NamaAkademik As String

        Public Property TglAkademik() As String
            Get
                Return m_TglAkademik
            End Get
            Set(ByVal value As String)
                m_TglAkademik = value
            End Set
        End Property

        Private m_TglAkademik As String

        Public Property TglGantiAkademik() As String
            Get
                Return m_TglGantiAkademik
            End Get
            Set(ByVal value As String)
                m_TglGantiAkademik = value
            End Set
        End Property

        Private m_TglGantiAkademik As String

        Public Property TglInAkademik() As String
            Get
                Return m_TglInAkademik
            End Get
            Set(ByVal value As String)
                m_TglInAkademik = value
            End Set
        End Property

        Private m_TglInAkademik As String

        Public Property TglAbsenAkademik() As String
            Get
                Return m_TglAbsenAkademik
            End Get
            Set(ByVal value As String)
                m_TglAbsenAkademik = value
            End Set
        End Property

        Private m_TglAbsenAkademik As String
    End Class
#Region "methodbind Billy"

    <WebMethod()>
    Public Shared Function BindDataIdDosen2() As IdDosenDetails2()
        Dim dt As New DataTable()

        Dim detailsIdDosen2 As New List(Of IdDosenDetails2)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IdDosen,nama,HONORSKS from Dosen  order by IDDosen", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen2 As New IdDosenDetails2()
                    dataIdDosen2.IdDosen = dtrow("IdDosen").ToString()
                    dataIdDosen2.namadosen = dtrow("nama").ToString()
                    dataIdDosen2.honorsks = dtrow("HONORSKS").ToString()
                    detailsIdDosen2.Add(dataIdDosen2)
                Next
            End Using
        End Using
        Return detailsIdDosen2.ToArray()
    End Function

    Public Class IdDosenDetails2

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property honorsks() As String
            Get
                Return m_honorsks
            End Get
            Set(ByVal value As String)
                m_honorsks = value
            End Set
        End Property

        Private m_honorsks As String
        Public Property namadosen() As String
            Get
                Return m_namadosen
            End Get
            Set(ByVal value As String)
                m_namadosen = value
            End Set
        End Property

        Private m_namadosen As String
    End Class

    <WebMethod()>
    Public Shared Function BindDataIdDosen22(idmk As String) As IdDosenDetails22()
        Dim dt As New DataTable()

        Dim detailsIdDosen22 As New List(Of IdDosenDetails22)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select IdDosen,nama from Dosen where iddosen in (Select iddosen from mkdosen where idmk='" & idmk & "') order by IDDosen", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdDosen22 As New IdDosenDetails22()
                    dataIdDosen22.IdDosen = dtrow("IdDosen").ToString()
                    dataIdDosen22.namadosen = dtrow("nama").ToString()
                    detailsIdDosen22.Add(dataIdDosen22)
                Next
            End Using
        End Using
        Return detailsIdDosen22.ToArray()
    End Function

    Public Class IdDosenDetails22

        Public Property IdDosen() As String
            Get
                Return m_IdDosen
            End Get
            Set(ByVal value As String)
                m_IdDosen = value
            End Set
        End Property

        Private m_IdDosen As String
        Public Property namadosen() As String
            Get
                Return m_namadosen
            End Get
            Set(ByVal value As String)
                m_namadosen = value
            End Set
        End Property

        Private m_namadosen As String
    End Class
#End Region


    <WebMethod()>
    Public Shared Function BindDataIdProdi() As IdProdiDetails3()
        Dim dt As New DataTable()

        Dim detailsIdProdi As New List(Of IdProdiDetails3)()
        Using con As New SqlConnection(clsEnt.strKoneksi("connQuality"))
            Using cmd As New SqlCommand("select prodi from prodifakultas  order by prodi ", con)
                con.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                For Each dtrow As DataRow In dt.Rows
                    Dim dataIdProdi As New IdProdiDetails3()
                    dataIdProdi.IdProdi = dtrow("Prodi").ToString()
                    detailsIdProdi.Add(dataIdProdi)
                Next
            End Using
        End Using
        Return detailsIdProdi.ToArray()
    End Function

    Public Class IdProdiDetails3

        Public Property IdProdi() As String
            Get
                Return m_IdProdi
            End Get
            Set(ByVal value As String)
                m_IdProdi = value
            End Set
        End Property

        Private m_IdProdi As String
        
    End Class


End Class