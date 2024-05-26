<%@ Page Title="Report Kartu Piutang" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RptKartuPiutang.aspx.vb" Inherits="RptKartuPiutang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
</script>



    <script type="text/javascript">
        //Kampus
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataCabang",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=tblCabang.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueCabang1(this);'>" + data.d[i].IdCabang + "</td><td onclick='javascript:putvalueCabang2(this);'>" + data.d[i].lokasiCabang + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>
 
     
<script type="text/javascript">
        //Unit
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataUnit",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=Tblunit.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueUnit1(this);'>" + data.d[i].IdUnit + "</td><td onclick='javascript:putvalueUnit2(this);'>" + data.d[i].lokasiUnit + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>

   <%-- <script type="text/javascript">
        //Wipem
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataWipem",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=TblWipem.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueWipem1(this);'>" + data.d[i].IdWipem + "</td><td onclick='javascript:putvalueUnit2(this);'>" + data.d[i].lokasiWipem + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>--%>


<style type="text/css">
   .HiddenCol{display:none;}                
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>REPORT KARTU PIUTANG NASABAH</h1>
                    </div>
                </div>
            </div>
        </section>
        <!-- Main content -->
        <section class="content"> 
        <div class="card">
                <!-- Card Header -->
            <div class="card-header">
                <asp:LinkButton ID="lnkPrintJadwal" CssClass="btn btn-info" ForeColor="white" runat="server" ><span class="fas fa-print"></span><asp:Label ID="Label4" Text="PRINT" CssClass="d-none d-sm-inline-block" Style="padding-left: 8px;" runat="server" /></asp:LinkButton>
           </div>
       </div>
           <div class="card">
                <!-- Card Header -->
                    <div class="card-header">
            <!-- Default box -->
                <div class="card-body"> 
                      <div class="row">        
                            <label class="col-sm-4">ID NASABAH</label>
                             <asp:TextBox ID="txtidnasabah" runat="server" class="form-control input-sm" placeholder="Ketikan ID Nasabah..."></asp:TextBox>
                         </div> 
                        <div class="row">        
                            <label class="col-sm-4">NAMA NASABAH</label>
                             <asp:TextBox ID="txtnama" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 
                          <div class="row">        
                            <label class="col-sm-4">NOMOR KONTRAK</label>
                             <asp:TextBox ID="txtnopinjam" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 
                          <div class="row">        
                            <label class="col-sm-4">ALAMAT</label>
                             <asp:TextBox ID="txtalamat" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 

                        </div>                                      
                    </div>
                </div>  
                
     
      <!-- Search Button Cabang --><div class="modal fade" id="myCabang">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Cabang</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchCabang" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="tblCabang" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.Cabang</td><td class="bg-primary">Lokasi Cabang</td></tr></thead></table></div></div></div></div>

 <!-- Search Button Unit --><div class="modal fade" id="myUnit">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Unit</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchUnit" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="Tblunit" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.Unit</td><td class="bg-primary">Lokasi Unit</td></tr></thead></table></div></div></div></div>

  </section>

     <div style="height:20px;"></div>
     <section class="content"> 
        <div class="card">
                <!-- Card Header -->
            <div class="card-header">
                 <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JENIS BARANG </label>
                        <asp:TextBox ID="txtjenis" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">MERK </label>
                        <asp:TextBox ID="txtmerk" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 

                   <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">TYPE </label>
                        <asp:TextBox ID="txttype" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">NO PLAT </label>
                        <asp:TextBox ID="txtplat" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 
        
                   <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">WARNA </label>
                        <asp:TextBox ID="txtwarna" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">KONDISI </label>
                        <asp:TextBox ID="txtkondisi" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 

                   <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">HARGA NOMINAL </label>
                        <asp:TextBox ID="txtharga" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">DP </label>
                        <asp:TextBox ID="txtdp" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 

                   <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">POKOK KREDIT </label>
                        <asp:TextBox ID="txtpokok" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">ANGSURAN / BULAN </label>
                        <asp:TextBox ID="txtangsuran" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 

                
                   <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JANGKA </label>
                        <asp:TextBox ID="txtjangka" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">TANGGAL PEMBAYARAN </label>
                          <div class="input-group">
                                                <input id="dtgl" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                      </div>
                    </div>
                   </div> 

           </div>
       </div>
     </section>
         <asp:Panel ID="lineItem" runat="server" ScrollBars="Vertical" Height="435px">
                            <div id="divDisplay" runat="server" class="panel panel-primary" style="margin-top: 2px !important; margin-left: 2px; margin-right: 2px; margin-bottom: 2px; display: none;"> 
                                <asp:PlaceHolder ID="placeHolderJadwal" runat="server"></asp:PlaceHolder>
                            </div>
                        </asp:Panel>                    
         <hr />
  </div>
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
       <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtidnasabah.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetNasabahNasabah") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                nopinjamnasabahya: item.nopinjamanGetNasabah,
                                idnasabahya: item.idnasabahGetNasabah,
                                namanasabahya: item.namanasabahGetNasabah,
                                alamatnya: item.alamatGetNasabah,
                                noplatnya: item.noplatGetNasabah,
                                jenisnya: item.jenisGetNasabah,
                                merknya: item.merkGetNasabah,
                                typenya: item.typeGetNasabah,
                                warnanya: item.warnaGetNasabah,
                                dpnya: item.dpGetNasabah,
                                saldoawalnya: item.saldoawalGetNasabah,
                                hargapokoknya: item.hargapokokGetNasabah,
                                angsurannya: item.angsuranGetNasabah,
                                lamanya: item.LamaangsuranGetNasabah,
                                kondisinya: item.kondisiGetNasabah,
                                tanggalnya: item.tanggalGetNasabah
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtnopinjam.ClientID %>").val(ui.item.nopinjamnasabahya);
                $("#<%=txtidnasabah.ClientID %>").val(ui.item.idnasabahya);
                $("#<%=txtnama.ClientID %>").val(ui.item.namanasabahya);
                $("#<%=txtalamat.ClientID %>").val(ui.item.alamatnya);
                $("#<%=txtplat.ClientID %>").val(ui.item.noplatnya);
                $("#<%=txtjenis.ClientID %>").val(ui.item.jenisnya);
                $("#<%=txtmerk.ClientID %>").val(ui.item.merknya);
                $("#<%=txttype.ClientID %>").val(ui.item.typenya);
                $("#<%=txtwarna.ClientID %>").val(ui.item.warnanya);
                $("#<%=txtharga.ClientID %>").val(ui.item.saldoawalnya);
                $("#<%=txtdp.ClientID %>").val(ui.item.dpnya);
                $("#<%=txtpokok.ClientID %>").val(ui.item.hargapokoknya);
                $("#<%=txtangsuran.ClientID %>").val(ui.item.angsurannya);
                $("#<%=txtjangka.ClientID %>").val(ui.item.lamanya);
                $("#<%=txtkondisi.ClientID %>").val(ui.item.kondisinya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.namanasabahya + " - " + item.noplatnasabahya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

</asp:Content>

