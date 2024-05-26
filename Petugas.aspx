<%@ Page Title="Petugas" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Petugas.aspx.vb" Inherits="Petugas"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>DATA PETUGAS</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.Aspx">Home</a></li>
                            <li class="breadcrumb-item active">DATA PETUGAS</li>
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
                    <asp:LinkButton ID="btnNew2" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label2" text="New" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="btnNew" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label1" text="Back" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="lnkSave" CssClass="btn btn-primary" ForeColor="white" runat="server" OnClick="lnkSave_Click" OnClientClick="return ShowSaveConfirm(this);"><span class="fas fa-save"></span><asp:label ID="Label3" text="Save" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" CssClass="btn btn-danger" ForeColor="white" runat="server" OnClick="lnkDelete_Click" OnClientClick="return ShowDeleteConfirm(this);"><span class="fas fa-trash"></span><asp:label ID="Label4" text="Delete" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                   <%-- <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>--%>
                </div>
                <!-- Card Header -->
                <!-- Card Body -->
                <div class="card-body"> 
                
                       <div class="row">
                        <div class="col-sm-12"> 
                         <div style="height:13px;"></div>  
                 <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">ID KARYAWAN</label>
                        <asp:TextBox ID="txtidkaryawan" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NAMA</label> 
                         <asp:TextBox ID="txtnama" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Nama..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                        <div class="row">        
                            <label class="col-sm-4">  ALAMAT</label>
                             <asp:TextBox ID="txtalamat" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div> 

                      <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">TEMPAT LAHIR </label>
                        <asp:TextBox ID="txttempat" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >TANGGAL LAHIR </label> 
                           <div class="input-group">
                                                <input id="dtpTgllahir" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
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
                        <label class="text-center">USER NAME </label>
                        <asp:TextBox ID="txtuser" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >PASSWORD </label> 
                         <asp:TextBox ID="txtpassword" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Kuliah ..."></asp:TextBox>
                      </div>
                    </div>
                   </div>
                
                    <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JABATAN </label>
                              <asp:DropDownList ID="ddljabat" CssClass="form-control" EnableViewState="true" runat="server"  />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >JENIS KELAMIN </label> 
                              <asp:DropDownList ID="ddljeniskelamin" CssClass="form-control" EnableViewState="true" runat="server"  />
                      </div>
                    </div>
                   </div>
                    

                    <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">HP </label>
                        <asp:TextBox ID="txthp" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >TANGGAL GABUNG </label> 
                                  <div class="input-group">
                                                <input id="txttglgabung" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
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
                                <label class="text-center">ID CABANG </label>
                                 <%--  <asp:DropDownList ID="ddlcabang" CssClass="form-control" EnableViewState="true" runat="server"  />  --%>   
                                <asp:TextBox ID="ddlcabang" runat="server" class="form-control input-sm" placeholder="Ketikan Id Cabang ..."></asp:TextBox>                   
                             </div>
                        </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID UNIT </label> 
                               <asp:TextBox ID="ddlunit" runat="server" class="form-control input-sm" placeholder="Ketikan Id Unit ..."></asp:TextBox>    
                          <%-- <asp:DropDownList ID="ddlunit" CssClass="form-control" EnableViewState="true" runat="server"  />
  --%>                    </div>
                    </div>
                   </div> 

                                </div></div>
                <h2>
                B. PHOTO</h2>
          
                   <div style="margin-left: 30px">
         <%--   <asp:Button ID="btnUpdate" runat="server" Text="Update Nama/TglLahir/TempatLahir" />
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Simpan" />--%>
            <br />
            <br />
            <asp:Image ID="imgPasFoto" runat="server" Height="201px" 
                ImageUrl="~/FilesFotoPetugas/NoImage.png" Width="151px" 
                BorderStyle="Double" />
            <br />
        <ul>
            Catatan :
            <li>Harap ketik dengan huruf KAPITAL</li>
            <li>Besar File Foto minimal 1 MB dan maksimal 1.5 MB</li>
            
        </ul>

        </div>


        <br />
        
        <br />
        <div style="margin-left: 30px">
        Pilih File Foto Anda : 
        <asp:FileUpload ID="FileUpload1" runat="server" onChange="validfoto();"/>
             
      
        <asp:Button ID="bntUpload" runat="server" Text="Upload File Foto Anda" onclick="bntUpload_Click"/>
        
        </div>

        <div style="margin-left: 30px">
            <br />
            <br />
            <hr />
            <b>Preview dari Foto Anda </b>
            <asp:Label ID="lblExtension" runat="server"></asp:Label>
            <br />
            <br />
            <div id="dvPreview">
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

</asp:Content>

