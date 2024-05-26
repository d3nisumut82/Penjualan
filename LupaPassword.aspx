d<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LupaPassword.aspx.vb" Inherits="_LupaPassword" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <link rel="icon" href="assets/enterprise.ico">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>PORTAL DOSEN</title>

    <!-- Custom fonts for this template-->
    <!-- Custom styles for this template-->
    <link href="css/sb-admin-2.min.css" rel="stylesheet">
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">

 <%--   <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">--%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script type="text/javascript" src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="assets/css/bootstrap-dialog.css" rel="stylesheet" type="text/css" />
    <script src="assets/js/bootstrap-dialog.js" type="text/javascript"></script>

    <!-- Error Pop up -->
    <script type="text/javascript">
        function pesanError(psn) {
            var shortContent = '<p>' + psn + '</p>';
            BootstrapDialog.alert({
                message: shortContent,
                draggable: true
            });
        }
    </script>

    <style type="text/css">
        div.my-auto {
            position: fixed;
            top: 50%;
            left: 50%;
            -webkit-transform: translate(-50%, -50%);
            -webkit-box-shadow: 7px 7px 14px #d9d9d9, -7px -7px 14px #ffffff;
            min-width: 920px;
            min-height: 430px;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: 0.5s all;
        }

        .image-logo {
            padding: 0px;
            background-image: url('FilesFoto/Logo1.png');
            background-size: cover;
            width: 430px;
            height: 300px;
            transform: scale(0.9);
            -webkit-transform: scale(0.9);
            -moz-transform: scale(0.6);
        }

        div.card .small-image {
            display: none;
        }

        div.card:hover, div.card:focus {
            -webkit-box-shadow: 20px 20px 52px #d9d9d9, -26px -26px 52px #ffffff;
        }

        .text-center {
            margin-top: 12.5%;
        }

        .textbox {
            text-transform: capitalize;
        }

        @media screen and (min-width: 768px) and (max-width: 992px) {
            div.card .small-image {
                display: none;
            }

            div.my-auto {
                min-width: 300px;
                min-height: 300px;
            }

            .image-logo {
                background-image: url('FilesFoto/Logo1.png');
                background-size: cover;
                width: 300px;
                height: 300px;
                transform: scale(1.6);
                -webkit-transform: scale(1.6);
                -moz-transform: scale(1.6);
            }

            .text-center {
                margin-top: 10%;
            }

            .no-padding {
                font-size: medium;
                margin-left: 10%;
            }
        }

        @media screen and (max-width: 767px) {
            div.card {
                -webkit-border-radius: 8px;
                -webkit-box-shadow: 0px 0px 0px #b3b3b3;
                -webkit-transition: all 0.5s;
                position: fixed;
                top: 50%;
                left: 50%;
                -webkit-transform: translate(-50%, -50%);
                min-width: 100%;
                height: 100%;
            }

            div {
                overflow: hidden;
            }

                div.card .small-image {
                    display: block;
                    padding: 40px 0;
                    width: 100%;
                }

                div.card:hover, div.card:focus {
                    -webkit-border-radius: 12px;
                    -webkit-box-shadow: 0px 0px 0px #a6a6a6;
                }
        }
    </style>
</head>

<body>
    <div class="container">

        <!-- Outer Row --><div class="row">

            <div class="col-xl-10 col-lg-12 col-md-9">

                <div class="card o-hidden border-0 my-auto">
                    <div class="card-body p-5">
                        <!-- Nested Row within Card Body -->
                        <div class="row">
                            <div class="col-lg-6 d-none d-md-block image-logo"></div>
                            <div class="col-md-6">
                                <div class="p-5 no-padding">
                                    <img class="small-image" src="FilesFoto/Logo1.png" alt="UNIVERSITAS QUALITY" />
                                    <div class="text-center">
                                        <h1 class="h4 text-gray-900 mb-4">LUPA PASSWORD</h1>
                                    </div>

                                    <form id="userlogin" runat="server">
                                        <div class="form-group">
                                            <asp:TextBox ID="txtUserName" runat="server" class="textbox form-control form-control-user" placeholder="Ketik User ID"></asp:TextBox>
                                        </div>
                                                                           
                                        <asp:Button class="btn btn-primary btn-user btn-block" ID="btnSubmit" runat="server" Text="Kirim" OnClick="Login_Click" />
                                        <asp:Button class="btn btn-primary btn-user btn-block" ID="btnback" runat="server" Text="Kembali Login" />                
                                 </form>
                                 </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</body>
</html>