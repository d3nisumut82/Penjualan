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

Partial Class UnitKerjaList
    Inherits System.Web.UI.Page
    Private sConnstring As String = clsEnt.strKoneksi("connuq")
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl

    Private dtSelectMenu As DataTable
    Private MenuDataView As DataView
    Protected Sub GridView1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Right
        End If

    End Sub
    Protected Sub GridView1_PreRender(sender As Object, e As EventArgs) Handles GridView1.PreRender
        If GridView1.Rows.Count > 0 Then
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter
        End If

    End Sub

    Protected Sub btnAddMataKuliah_Click(sender As Object, e As System.EventArgs) Handles btnAddMataKuliah.Click
        Response.Redirect("UnitKerja.aspx")
    End Sub
End Class
