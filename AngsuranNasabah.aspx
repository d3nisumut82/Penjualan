<%@ Page Title="ANGSURAN NASABAH" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AngsuranNasabah.aspx.vb" Inherits="AngsuranNasabah"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                        <h1>ANGSURAN NASABAH</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.Aspx">Home</a></li>
                            <li class="breadcrumb-item active">ANGSURAN NASABAH</li>
                        </ol>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtpokok" runat="server" class="HiddenCol" ></asp:TextBox>
                        </div>
                    </div>
                   <div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtbunga" runat="server" class="HiddenCol" ></asp:TextBox>
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
                </div>
      
                    <div class="row">
                    <div class="col-sm-3">
                            <asp:TextBox ID="txtjumlah2" runat="server" class="HiddenCol" ></asp:TextBox>
                        </div>
                    </div>
                <div class="card-body"> 
                       <div class="row">
                        <div class="col-sm-12"> 
                         <div style="height:13px;"></div>  

                  <div class="row">        
                            <label class="col-sm-4">ID TRANS</label>
                             <asp:TextBox ID="txtidtrans" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div> 

                 <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">NOMOR PLAT</label>
                        <asp:TextBox ID="txtplat" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR PINJAMAN</label> 
                         <asp:TextBox ID="txtnopinjaman" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Nama..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                   
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
                        <label class="text-center">MERK </label>
                        <asp:TextBox ID="txtmerk" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group">
                        <label class="text-center">JENIS </label>
                        <asp:TextBox ID="txtjenis" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
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
                            <label class="col-sm-1">PEMBAYARAN </label>
                              <asp:RadioButton ID="rdBangsuran" runat="server" type="radio" Text="ANGSURAN  "  GroupName="search" AutoPostBack="False"/>  
                                <asp:RadioButton ID="rdbtunggakan" runat="server" type="radio" Text="TUNGGAKAN " GroupName="search" AutoPostBack="False" />
                                <asp:RadioButton ID="rdbpelunasan" runat="server" type="radio" Text="PELUNASAN" GroupName="search" AutoPostBack="False" />
                   </div> 
                    
                    <div class="row">        
                           <label >TANGGAL </label> 
                              <div class="input-group">
                                   <input id="dtglangsuran" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                            </div> 
                        </div> 
                <div style="height:7px;"></div>
                    <%-- <div class="row">
                         <div class="col-sm-1">
                            <label style="padding-left:30px;">PEMABAYARAN </label> </div>
                         <div class="col-sm-6">
                                <asp:RadioButton ID="rdBangsuran" runat="server" type="radio" Text="ANGSURAN  "  GroupName="search" AutoPostBack="False"/>  
                                <asp:RadioButton ID="rdbtunggakan" runat="server" type="radio" Text="TUNGGAKAN " GroupName="search" AutoPostBack="False" />
                                <asp:RadioButton ID="rdbpelunasan" runat="server" type="radio" Text="PELUNASAN" GroupName="search" AutoPostBack="False" />
                                 
                         </div>
                    </div>--%>

                                </div>
                       </div>                                        
                </div>

                

                <hr />
          </div>        
                    <div id="divDisplay" runat="server" class="panel panel-primary" style="margin-top:2px !important;margin-left:2px;margin-right:2px;margin-bottom:2px;display:none;" > 
                        <asp:Panel ID="lineItem" runat="server" ScrollBars="Vertical"  Height="435px" ><asp:PlaceHolder id="placeHolderWisuda" runat="server"></asp:PlaceHolder> </asp:Panel> 
                    </div>
   </section>

         <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                            <asp:LinkButton ID="Lngenerate" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label6" text="Generate" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>  
                    </div>
                   
                </div>
             </div>   
        </section>

   <div class="card">
          <div class="card-body"> 
                         <div class="row">        
                            <label class="col-sm-4">ANGSURAN KE</label>
                                         <asp:TextBox ID="txtangsuranke" runat="server" class="form-control input-sm" ></asp:TextBox>
                        </div> 
                      <div class="row">        
                            <label class="col-sm-4">TANGGAL JATUH TEMPO</label>
                                  <div class="input-group">
                                                <input id="dtgltempo" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                         </div> 
                     <div class="row">        
                            <label class="col-sm-4">TANGGAL JATUH TEMPO KONTRAK</label>
                                  <div class="input-group">
                                                <input id="dtgltempokontrak" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                         </div> 
                    

                <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JUMLAH ANGSURAN POKOK </label>
                              <asp:TextBox ID="txtpokok2" runat="server" class="form-control input-sm" placeholder="Ketikan Harga Pokok ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >JUMLAH ANGSURAN BUNGA </label> 
                              <asp:TextBox ID="txtbunga2" runat="server" class="form-control input-sm" placeholder="Ketikan Harga Pokok ..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 

                     <div class="row">        
                            <label class="col-sm-4">SISA ANGSURAN</label>
                                         <asp:TextBox ID="txtsisasaldo" runat="server" class="form-control input-sm" ></asp:TextBox>
                        </div> 
                    
                         <div class="row">        
                                <label class="text-center">TOTAL ANGSURAN </label>
                                    <asp:TextBox ID="txtjumlah" runat="server" class="form-control input-sm" ></asp:TextBox>
                        </div> 
               
              
              
              </div>
         </div>

</div>
                <!-- Card Body -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">



    <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtplat.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetAngsuranNasabah") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                nopinjamannya: item.nopinjamanGetAngsuran,
                                idcabangnya: item.idCabangGetAngsuran,
                                idunitnya: item.idUnitGetAngsuran,
                                idnasabahya: item.idnasabahGetAngsuran,
                                namanasabahya: item.namanasabahGetAngsuran,
                                wipemnya: item.WipemGetAngsuran,
                                idsalesnya: item.idsalesGetAngsuran,
                                idsurveynya: item.idsurveyGetAngsuran,
                                noplatnya: item.noplatGetAngsuran,
                                merknya: item.merkGetAngsuran,
                                rangkanya: item.rangkasGetAngsuran,
                                mesinnya: item.mesinGetAngsuran,
                                jenisnya: item.jenisGetAngsuran
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtnopinjaman.ClientID %>").val(ui.item.nopinjamannya);
                $("#<%=ddlcabang.ClientID %>").val(ui.item.idcabangnya);
                $("#<%=ddlunit.ClientID %>").val(ui.item.idunitnya);
                $("#<%=txtidnasabah.ClientID %>").val(ui.item.idnasabahya);
                $("#<%=txtnama.ClientID %>").val(ui.item.namanasabahya);
                $("#<%=txtwipem.ClientID %>").val(ui.item.wipemnya);
                $("#<%=txtsales.ClientID %>").val(ui.item.idsalesnya);
                $("#<%=txtsurvey.ClientID %>").val(ui.item.idsurveynya);
                $("#<%=txtplat.ClientID %>").val(ui.item.noplatnya);
                $("#<%=txtmerk.ClientID %>").val(ui.item.merknya);
                $("#<%=txtjenis.ClientID %>").val(ui.item.jenisnya);    
                $("#<%=txtnorangka.ClientID %>").val(ui.item.rangkanya);
                $("#<%=txtnomesin.ClientID %>").val(ui.item.mesinnya);   
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.noplatnya + " - " + item.namanasabahya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>


</asp:Content>

