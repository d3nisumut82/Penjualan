<%@ Page Title="Bayar Denda List | Quality" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="DendarealList.aspx.vb" Inherits="DendarealList" %>

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
                        <h1>Bayar Denda List</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Bayar Denda List</li>
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
                    <asp:LinkButton ID="btnAddMataKuliah" CssClass="btn btn-primary" ForeColor="white" runat="server"><span class="fas fa-plus"></span><asp:label ID="Label4" text="Add Denda" CssClass="d-none d-sm-inline-block" style="padding-left: 5px;" runat="server" /></asp:LinkButton><div class="card-tools">
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
                            <asp:TemplateField HeaderText="KODE">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hyperLink1" runat="server" NavigateUrl='<%#String.Format("~/Dendareal.aspx?IdTrans={0}", HttpUtility.UrlEncode(Eval("IdTrans").ToString())) %>'
                                        Text='<%# Eval("IdTrans") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="IdCabang" HeaderText="ID CABANG" NullDisplayText="-" />
                             <asp:BoundField DataField="IdUnit" HeaderText="ID UNIT" NullDisplayText="-" />
                             <asp:BoundField DataField="Wipem" HeaderText="WIPEM" NullDisplayText="-" />
                             <asp:BoundField DataField="NoPinjaman" HeaderText="NOOMOR PINJAMAN" NullDisplayText="-" />
                             <asp:BoundField DataField="idNasabah" HeaderText="ID NASABAH" NullDisplayText="-" />
                             <asp:BoundField DataField="nama" HeaderText="NAMA" NullDisplayText="-" />
                             <asp:BoundField DataField="Jumlah" HeaderText="JUMLAH" NullDisplayText="-" />
                             <asp:BoundField DataField="tanggal" HeaderText="TANGGAL" NullDisplayText="-" />
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
        SelectCommand="SELECT a.IdTrans as IdTrans, a.IdCabang as IdCabang, a.IdUnit as IdUnit, a.Wipem as Wipem, a.NoPinjaman as NoPinjaman, a.idNasabah as idNasabah, FORMAT(a.Jumlah,'#,###,##0') as Jumlah, CONVERT(VARCHAR(10),a.Tanggal,103) as Tanggal, b.nama as nama from TblDenda a inner join tblnasabah b on b.idnasabah=a.idnasabah WHERE a.idTrans <> '' ORDER BY a.idTrans"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>