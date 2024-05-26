<%@ Page Title="Jual Barang" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="JualBarang.aspx.vb" Inherits="JualBarang"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
   .HiddenCol{display:none;}    
</style>

    <style type="text/css">
        .divblock {
     //other styles

     display: table;
     text-align: center;
 }

 .linkbutton {
     //other styles

     display: table-cell;
     vertical-align: middle;
}
     </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>JUAL BARANG</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.Aspx">Home</a></li>
                            <li class="breadcrumb-item active">JUAL BARANG</li>
                        </ol>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txttotal2" runat="server" class="HiddenCol" ></asp:TextBox></div></div><div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtsaldo2" runat="server" class="HiddenCol" ></asp:TextBox></div></div><div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtangsuran2" runat="server" class="HiddenCol" ></asp:TextBox></div></div><div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtbunga2" runat="server" class="HiddenCol" ></asp:TextBox></div>
                     <div class="col-sm-3">
                            <asp:TextBox ID="txtjual2" runat="server" class="HiddenCol" ></asp:TextBox></div>
                     <div class="col-sm-3">
                            <asp:TextBox ID="txtangsuranpokok2" runat="server" class="HiddenCol" ></asp:TextBox></div>
                     <div class="col-sm-3">
                            <asp:TextBox ID="txtangsuranbunga2" runat="server" class="HiddenCol" ></asp:TextBox></div>
                      <div class="col-sm-3">
                            <asp:TextBox ID="txtlaba2" runat="server" class="HiddenCol" ></asp:TextBox></div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtnojurnal" runat="server" class="HiddenCol" ></asp:TextBox></div>
                          
                     </div> 
                 
            </div>
        </section>
         <section class="content"> 
            <!-- Default box -->
            <div class="card">
               
                <div class="card-header">
                     <asp:LinkButton ID="btnNew2" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label2" text="New" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="btnNew" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label1" text="Back" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="lnkSave" CssClass="btn btn-primary" ForeColor="white" runat="server" OnClick="lnkSave_Click" OnClientClick="return ShowSaveConfirm(this);"><span class="fas fa-save"></span><asp:label ID="Label3" text="Save" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" CssClass="btn btn-danger" ForeColor="white" runat="server" OnClick="lnkDelete_Click" OnClientClick="return ShowDeleteConfirm(this);"><span class="fas fa-trash"></span><asp:label ID="Label4" text="Delete" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                </div>
             </div>      
       </section>
        <!-- Main content -->
        <section class="content"> 
            <!-- Default box -->
            <div class="card">
                <!-- Card Header -->
        <div class="card-body"> 
               
        </div>
                <div class="card-body"> 
                       <div class="row">
                        <div class="col-sm-12"> 
                  <div class="row">        
                            <label class="col-sm-4">ID TRANSAKSI </label>
                                 <asp:TextBox ID="txtidtrans" runat="server" class="form-control input-sm" autocomplete="off" />
                         </div> 
                         <div style="height:13px;"></div>  
               
                   
                 <div class="row">        
                        <div class="col-sm-6">
                            <!-- text input -->
                            <div class="form-group">
                                <label class="text-center">ID CABANG </label>
                                <asp:TextBox ID="ddlcabang" runat="server" class="form-control input-sm" placeholder="Ketikan Id Cabang ..."></asp:TextBox>                   
                             </div>
                        </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID UNIT </label> 
                               <asp:TextBox ID="ddlunit" runat="server" class="form-control input-sm" placeholder="Ketikan Id Unit ..."></asp:TextBox>    
                       </div>
                    </div>
                   </div> 
                    
                 <div class="row">        
                        <div class="col-sm-6">
                            <!-- text input -->
                            <div class="form-group">
                                <label class="text-center">ID SALES </label>
                                <asp:TextBox ID="txtsales" runat="server" class="form-control input-sm" placeholder="Ketikan Id Cabang ..."></asp:TextBox>                   
                             </div>
                        </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID SURVEYOR </label> 
                               <asp:TextBox ID="txtsurvey" runat="server" class="form-control input-sm" placeholder="Ketikan Id Unit ..."></asp:TextBox>    
                       </div>
                    </div>
                   </div> 
                      <div class="row">        
                            <label class="col-sm-4">WIPEM </label>
                             <asp:TextBox ID="txtwipem" runat="server" class="form-control input-sm" placeholder="Ketikan WIPEN..."></asp:TextBox>
                         </div> 
                     <div class="row">        
                            <label class="col-sm-4">ID NASABAH</label>
                             <asp:TextBox ID="txtidnasabah" runat="server" class="form-control input-sm" placeholder="Ketikan ID Nasabah..."></asp:TextBox>
                         </div> 
                        <div class="row">        
                            <label class="col-sm-4">NAMA NASABAH</label>
                             <asp:TextBox ID="txtnama" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 

                      <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JENIS </label>
                                    <asp:TextBox ID="txtjenis" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR PLAT</label> 
                         <asp:TextBox ID="txtplat" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Nama..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 

                          <div class="row">        
                            <label class="col-sm-4">MERK </label>
                                  <asp:TextBox ID="txtmerk" runat="server" class="form-control input-sm" autocomplete="off" />
                         </div> 

  
                    <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">NOMOR MESIN </label>
                               <asp:TextBox ID="txtnomesin" runat="server" class="form-control input-sm" placeholder="Ketikan Nomor Mesin ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR RANGKA </label> 
                         <asp:TextBox ID="txtnorangka" runat="server" class="form-control input-sm" placeholder="Ketikan Nomor Rangka ..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                
                   <div class="row">        
                            <label class="col-sm-4">JENIS PENJUALAN</label>
                                   <asp:DropDownList ID="ddjenis" runat="server" class="form-control input-sm">
                                  </asp:DropDownList>
                         </div> 
                      <div class="row">        
                            <label class="col-sm-4">TANGGAL</label>
                                  <div class="input-group">
                                                <input id="dtgl" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                         </div> 

                     
                <div class="row">        
                            <label class="col-sm-4">LAMA ANGSURAN</label>
                                  <asp:DropDownList ID="ddllama" runat="server" class="form-control input-sm">
                                  </asp:DropDownList>
                         </div> 
                 
                   <div class="row">        
                            <label class="col-sm-4">HARGA POKOK PENJUALAN (HPP)</label>
                                     <asp:TextBox ID="txtpokok" runat="server" class="form-control input-sm" placeholder="Ketikan Harga Pokok ..."></asp:TextBox>
                         </div> 
                                         
                <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">HARGA JUAL </label>
                              <asp:TextBox ID="txtjual" runat="server" class="form-control input-sm" placeholder="Ketikan Harga Pokok ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >UANG MUKA(DP) </label> 
                         <asp:TextBox ID="txtdp" runat="server" class="form-control input-sm" onkeypress="return IsNumericHPOS(event);" ondrop="return false;" onpaste="return false;" placeholder="KETIKAN Harga Pembelian ..."></asp:TextBox>
                               <span id="errorHPOS3" style="color: Red; display: none">* Input digits (0 - 9)</span>
                        <script type="text/javascript">
                            var specialKeys = new Array();
                            specialKeys.push(8); //Backspace
                            function IsNumericHPOS(e) {
                                var keyCode = e.which ? e.which : e.keyCode
                                var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                                document.getElementById("errorHPOS3").style.display = ret ? "none" : "inline";
                                return ret;
                            }
                        </script>   
                      </div>
                    </div>
                   </div> 


                                </div></div>
                
                   <div class="card-header">
                    <asp:LinkButton ID="Lbtnhitung" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label5" text="Generate" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                </div>

                 <div class="row">
                     <div class="col-sm-12"> 
                         <div style="height:13px;"></div> 
                       <div class="row">        
                            <label class="col-sm-4">MARK-UP</label>
                             <asp:TextBox ID="txtlaba" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 
                     <div class="row">        
                            <label class="col-sm-4">SALDO AWAL POKOK</label>
                             <asp:TextBox ID="txttotal" runat="server" class="form-control input-sm" placeholder="Ketikan Nama Supplier..."></asp:TextBox>
                         </div> 
                 <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">SALDO AWAL POKOK + BUNGA</label>
                              <asp:TextBox ID="txtsaldo" runat="server" class="form-control input-sm" placeholder="Ketikan Harga Saldo Awal Pokok Dan Bunga ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ANGSURAN </label> 
                         <asp:TextBox ID="txtangsuran" runat="server" class="form-control input-sm" onkeypress="return IsNumericHPOS(event);" ondrop="return false;" onpaste="return false;" placeholder="KETIKAN Harga Pembelian ..."></asp:TextBox>
                               <span id="errorHPOS4" style="color: Red; display: none">* Input digits (0 - 9)</span>
                        <script type="text/javascript">
                            var specialKeys = new Array();
                            specialKeys.push(8); //Backspace
                            function IsNumericHPOS(e) {
                                var keyCode = e.which ? e.which : e.keyCode
                                var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                                document.getElementById("errorHPOS4").style.display = ret ? "none" : "inline";
                                return ret;
                            }
                        </script>   
                      </div>
                    </div>
                   </div> 
                    
                      <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">ANGSURAN POKOK</label>
                              <asp:TextBox ID="txtangsuranpokok" runat="server" class="form-control input-sm" placeholder="Ketikan Angsuran Pokok ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ANGSURAN BUNGA</label> 
                         <asp:TextBox ID="txtangsuranbunga" runat="server" class="form-control input-sm" onkeypress="return IsNumericHPOS(event);" ondrop="return false;" onpaste="return false;" placeholder="KETIKAN ANGSURAN BUNGA ..."></asp:TextBox>
                               <span id="errorHPOS5" style="color: Red; display: none">* Input digits (0 - 9)</span>
                        <script type="text/javascript">
                            var specialKeys = new Array();
                            specialKeys.push(8); //Backspace
                            function IsNumericHPOS(e) {
                                var keyCode = e.which ? e.which : e.keyCode
                                var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                                document.getElementById("errorHPOS5").style.display = ret ? "none" : "inline";
                                return ret;
                            }
                        </script>   
                      </div>
                    </div>
                   </div> 

                 <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">BUNGA </label>
                             <asp:TextBox ID="txtbunga" runat="server" class="form-control input-sm" placeholder="..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label > TANGGAL JATUH TEMPO </label> 
                                  <div class="input-group">
                                                <input id="dtgltempo" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                  </div>
                       </div>
                    </div>
              </div>   
                         
               <div class="row">     
                <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                         <label> TANGGAL CALON MACET </label> 
                                  <div class="input-group">
                                                <input id="dtglcalonmacet" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                  </div>
                      </div>
                 </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label> TANGGAL MACET BARU</label> 
                                  <div class="input-group">
                                                <input id="dtglmacetbaru" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                  </div>
                       </div>
                    </div>
              </div>   
                      
                        <div class="row">        
                            <label class="col-sm-4">TANGGAL MACET LAMA</label>
                                      <div class="input-group">
                                                <input id="dtglmacetlama" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                  </div>
                         </div>              
                                             
        </div>
    </div>        
    
                <hr />

                    <%--<button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addmodalBank"><span data-toggle="tooltip" title="Add" class="glyphicon glyphicon-plus"></span>Tambah Sub Account</button>
                    
                    <br /><br />--%>
     <!-- Search Button Kampus --><div class="modal fade" id="myKampus">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Kampus</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchKampus" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="tblKampus" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.UNIT</td><td class="bg-primary">Lokasi Kampus</td></tr></thead></table></div></div></div></div>



                    
                    <div id="divDisplay" runat="server" class="panel panel-primary" style="margin-top:2px !important;margin-left:2px;margin-right:2px;margin-bottom:2px;display:none;" > 
                        <asp:Panel ID="lineItem" runat="server" ScrollBars="Vertical"  Height="435px" ><asp:PlaceHolder id="placeHolderWisuda" runat="server"></asp:PlaceHolder> </asp:Panel> 
                    </div>

                </div>
                <!-- Card Body -->
            </div>
        </section>
        
    </div>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlcabang.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetMkKonversi2") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetMkKonversi2,
                                namanya: item.namaGetMkKonversi2,
                                sksnya: item.sksGetMkKonversi2
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=ddlcabang.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlunit.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetMkKonversiunit") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetMkKonversi3,
                                namanya: item.namaGetMkKonversi3,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=ddlunit.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

    <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtplat.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetPenjualanBarang") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                noplatnya: item.noplatGetPenjualan,
                                merknya: item.merkGetPenjualan,
                                jenisnya: item.jenisGetPenjualan,
                                nomesinnya: item.nomesinGetPenjualan,
                                totalnya: item.nototalGetPenjualan,
                                norangkanya: item.norangkaGetPenjualan
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtplat.ClientID %>").val(ui.item.noplatnya);
                $("#<%=txtmerk.ClientID %>").val(ui.item.merknya);
                $("#<%=txtjenis.ClientID %>").val(ui.item.jenisnya);
                $("#<%=txtnomesin.ClientID %>").val(ui.item.nomesinnya);
                $("#<%=txtnorangka.ClientID %>").val(ui.item.norangkanya);
                $("#<%=txtpokok.ClientID %>").val(ui.item.totalnya);
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.noplatnya + " - " + item.merknya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtsales.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetKaryawan") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetKaryawan,
                                namanya: item.namaGetKaryawan,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtsales.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtsurvey.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetKaryawan") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetKaryawan,
                                namanya: item.namaGetKaryawan,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtsurvey.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtwipem.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetWipem") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetWipem,
                                namanya: item.namaGetWipem,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtwipem.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

    <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtidnasabah.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetNasabah") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetNasabah,
                                namanya: item.namaGetNasabah,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtidnasabah.ClientID %>").val(ui.item.kodenya);
                $("#<%=txtnama.ClientID %>").val(ui.item.namanya);
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

   <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtjenis.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetPenjualanBarang") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                noplatnya: item.noplatGetPenjualan,
                                merknya: item.merkGetPenjualan,
                                jenisnya: item.jenisGetPenjualan,
                                nomesinnya: item.nomesinGetPenjualan,
                                totalnya: item.nototalGetPenjualan,
                                norangkanya: item.norangkaGetPenjualan
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtplat.ClientID %>").val(ui.item.noplatnya);
                $("#<%=txtmerk.ClientID %>").val(ui.item.merknya);
                $("#<%=txtjenis.ClientID %>").val(ui.item.jenisnya);
                $("#<%=txtnomesin.ClientID %>").val(ui.item.nomesinnya);
                $("#<%=txtnorangka.ClientID %>").val(ui.item.norangkanya);
                $("#<%=txtpokok.ClientID %>").val(ui.item.totalnya);
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.noplatnya + " - " + item.jenisnya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

</asp:Content>

