﻿<%@ Page Title="Barang List | Quality" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RekondisiBarangList.aspx.vb" Inherits="RekondisiBarangList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
//            $('#<%=GridView1.ClientID%> thead tr').clone(true).appendTo('#<%=GridView1.ClientID%> thead');
            $('#<%=GridView1.ClientID%> thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
//                $(this).html('<input type="text" class="form-control" />');
                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            var table = $('#<%=GridView1.ClientID%>').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                into: true,
                autoWidth: false,
                dom: "<'form-group row'<'col-sm-6 pull-left'B><'col-sm-6 searchgrid pull-right'f>><'row'>tr<'row form-group'><'row'<'col-sm-6'l><'col-sm-6 pull-right'p>><'clear'><'row'<'col-sm-6'i><'col-sm-6 pull-right'>>",
                buttons: [
                    { extend: 'print', text: 'Export', className: 'btn btn-success', titleAttr: 'Export' },
                ]
            });

            $('.searchgrid label input').css('width', '300px');
            $('.searchgrid label input').removeClass('form-control-sm');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Rekondisi Barang List</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Rekondisi Barang List</li>
                        </ol>
                    </div>
                </div>
            </div>
        </section>

        <!-- Main content -->
        <section class="content">

            <!-- Default box -->
            <div class="card">
                <!-- Card Header -->
                <div class="card-header">
                    <asp:LinkButton ID="btnAddMataKuliah" CssClass="btn btn-primary" ForeColor="white" runat="server"><span class="fas fa-plus"></span><asp:label ID="Label4" text="Add Rekondisi" CssClass="d-none d-sm-inline-block" style="padding-left: 5px;" runat="server" /></asp:LinkButton><div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                       
                    </div>    
                </div>

                <div class="card-body">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" DataSourceID="GridDataSource">
                        <Columns>
                            <asp:TemplateField HeaderText="Kode">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hyperLink1" runat="server" NavigateUrl='<%#String.Format("~/RekondisiBarang.aspx?idTrans={0}", HttpUtility.UrlEncode(Eval("idTrans").ToString())) %>'
                                        Text='<%# Eval("idTrans") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="NamaPemilik" HeaderText="NAMA PEMILIK" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="NoMesin" HeaderText="NOMOR MESIN" NullDisplayText="-" />
                             <asp:BoundField DataField="NoRangka" HeaderText="NOMOR RANGKA" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaRekondisi" HeaderText="BIAYA REKONDISI" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaAdministrasi" HeaderText="BIAYA ADMINISTRASI" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaKomisi" HeaderText="BIAYA KOMISI" NullDisplayText="-" />
                             <asp:BoundField DataField="Tanggal" HeaderText="TANGGAL" NullDisplayText="-" />
                             <asp:BoundField DataField="Keterangan" HeaderText="KETERANGAN" NullDisplayText="-" />
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                    </asp:GridView>

                </div>
            </div>
        </section>
    </div>

    <asp:SqlDataSource ID="GridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, NamaPemilik as NamaPemilik, Merk as Merk, Jenis as Jenis, FORMAT(BiayaRekondisi,'#,###,##0') as BiayaRekondisi, FORMAT(BiayaAdministrasi,'#,###,##0')  as BiayaAdministrasi,FORMAT(BiayaKomisi,'#,###,##0')  as BiayaKomisi, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),Tanggal,103) as Tanggal,Keterangan AS Keterangan from TblRekondisi WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>