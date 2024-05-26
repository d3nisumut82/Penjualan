<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LogOut.aspx.vb" Inherits="LogOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="assets/enterprise.ico">
    <title>Enterprise System</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"> 
    <link href="dist/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/bootstrap-modalgrid.css" rel="stylesheet" type="text/css">
    <link href="assets/css/navbar.css" rel="Stylesheet" type="text/css">
    <link href="assets/css/tabstrip.css" rel="Stylesheet" type="text/css">
    <link href="assets/css/bootstrap-dialog.css" rel="stylesheet" type="text/css">  
    <script type="text/javascript" src="dist/js/jquery.min.js" ></script>
    <script type="text/javascript" src="dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap-dialog.js" ></script>
    <script type="text/javascript" src="assets/js/moment.min.js"></script>   
    <link href="assets/css/responsive.bootstrap.min.css" rel="stylesheet" type="text/css" />  
    <script type="text/javascript" src="assets/view/jquery-1.12.4.js"></script> 
    <style>
        body {background: lightblue url("assets/banner.jpg") no-repeat fixed center;}
        jumbotron{}
    </style>
    <script src="assets/js/modernizr.custom.80028.js" type="text/javascript"></script>
    <style>
    #note {
        position: absolute;
        z-index: 6001;
        top: 0;
        left: 0;
        right: 0;
        background: #fde073;
        text-align: center;
        line-height: 2.5;
        overflow: hidden; 
        -webkit-box-shadow: 0 0 5px black;
        -moz-box-shadow:    0 0 5px black;
        box-shadow:         0 0 5px black;
    }
    .cssanimations.csstransforms #note {
        -webkit-transform: translateY(-50px);
        -webkit-animation: slideDown 10.5s 1.0s 1 ease forwards;
        -moz-transform:    translateY(-50px);
        -moz-animation:    slideDown 10.5s 1.0s 1 ease forwards;
    }

    #close {
      position: absolute;
      right: 10px;
      top: 9px;
      text-indent: -9999px;
      background: url(images/close.png);
      height: 16px;
      width: 16px;
      cursor: pointer;
    }
    .cssanimations.csstransforms #close {
      display: none;
    }
    
    @-webkit-keyframes slideDown {
        0%, 100% { -webkit-transform: translateY(-50px); }
        10%, 90% { -webkit-transform: translateY(0px); }
    }
    @-moz-keyframes slideDown {
        0%, 100% { -moz-transform: translateY(-50px); }
        10%, 90% { -moz-transform: translateY(0px); }
    }
	</style>
	<script type="text/javascript">
	    window.setTimeout("location=('Default.aspx');", 10000);
    </script>
</head>
<body style="min-width:330px;width: auto !important;width:330px; " >
     
    <form id="formLogOut" runat="server">e<asp:ScriptManager ID="smLogout" runat="server"></asp:ScriptManager> 
        <div id="note">Logout <asp:Label ID="LogOffTime" runat="server" Text="Label"></asp:Label><a id="close">[close]</a>
        </div> 
    </form> 
</body>
</html>
