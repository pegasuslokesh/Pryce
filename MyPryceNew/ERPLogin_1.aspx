<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeFile="ERPLogin.aspx.cs" EnableSessionState="True" Inherits="ERPLogin" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Pryce | Log in</title>
    <link rel="shortcut icon" href="Images/favicon.ico" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <link href="Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
    <link href="BootStrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="BootStrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" type="text/css" />
    <script src="Script/common.js"></script>
    <link href="CSS/controlsCss.css" rel="stylesheet" />
</head>
<body>
    <div class="container-fluid" style="padding-left: 0px;">
        <div class="row">
            <div class="col-md-8" style="padding-left: 0px;" id="big-img">
                <div class="carousel-inner">
                    <div id="myCarousel" class="carousel slide" data-ride="carousel">
                        <!-- Indicators -->
                        <ol class="carousel-indicators">
                            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                            <li data-target="#myCarousel" data-slide-to="1"></li>
                            <li data-target="#myCarousel" data-slide-to="2"></li>
                            <br />
                            <br />
                            <div style="border-bottom: solid #aeaeae 4px; opacity: 0.5"></div>
                            <br />
                            <p style="color: #FFFFFF;">© 2019 <a href="http://www.pegasustech.net" target="_blank">Pegasus</a>. All right reserved.</p>

                        </ol>

                        <!-- Wrapper for slides -->
                        <div class="carousel-inner">

                            <div class="item active">
                                <img src="../Pryce/Images/Price-Slide-2.JPG" alt="Pryce-ERP">
                                <div class="carousel-caption">
                                   
                                </div>
                            </div>

                            <div class="item">
                                <img src="../Pryce/Images/Price-Slide-3.JPG" alt="Pryce-ERP">
                                <div class="carousel-caption">
                                   
                                   

                                </div>

                            </div>

                            <div class="item">
                                <img src="../Pryce/Images/Price-Slide-1.JPG" alt="Pryce-ERP">
                                <div class="carousel-caption">
                                  
                                </div>
                            </div>
                        </div>



                    </div>
                </div>

            </div>


            <div class="col-md-4" align="center" id="login-part">
                <%--<img src="/Images/PegasusLogo.png" width="220px" class="img-responsive"/>--%>
                <img id="imgLogo" runat="server" src="Images/PegasusLogo.png" width="220" class="img-responsive" />
                <h4>Pryce Enterprise Resource Planning</h4>
                <br />
                <form class="login-form" id="Login_Form" runat="server">
                    <asp:ScriptManager ID="Login_Script_Manager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="Login_Update" runat="server">
                        <ContentTemplate>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="Username"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <i class="glyphicon glyphicon-user"></i>
                                    </span>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="txtPassWord" runat="server" class="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <i class="glyphicon glyphicon-lock"></i>
                                    </span>
                                </div>
                            </div>

                            <div class="form-group" runat="server" id="Div_Company">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlCompany" class="form-control" placeholder="Company" runat="server"></asp:DropDownList>
                                    <span class="input-group-addon">
                                        <i class="glyphicon glyphicon-lock"></i>
                                    </span>
                                </div>
                            </div>
                            <p>
                                <asp:LinkButton ID="lnkFogetPassword" runat="server" Text="Forgot Password ?" OnClick="lnkFogetPassword_OnClick"></asp:LinkButton>
                            </p>

                            <div class="form-group sign-btn">
                                <asp:Button ID="btnLogin" runat="server" OnClick="btnlogin_Click" class="btn btn-lg btn-primary btn-block" Text="Sign In" />
                                <asp:Label ID="lblVersion" runat="server" ></asp:Label><br />
                                <asp:Label ID="lblReleaseDate" runat="server" ></asp:Label>
                            
                            </div>
                            <p class="float">
                                <blink> <asp:Label ID="lblErrormessage" Width="150px" Font-Bold="true" Font-Size="12px" runat="server" ForeColor="Red"></asp:Label></blink>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Login_Update">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </form>
                <div id="snackbar" align="center">Saved Successfully</div>
            </div>
            <asp:Button ID="btnredirecttocloud" runat="server" OnClick="btnredirecttocloud_Click" Visible="false" />
        </div>

        <style>
            p {
                text-align: center;
                font-size: 14px;
            }

            .title {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 30px;
                color: #FFFFFF;
                margin-bottom: 0px;
            }

            #title-div {
                left-align: 10px;
            }

            .carousel-caption {
                position: absolute;
                top: 0px;
                text-align: left;
                z-index: 10;
                color: #fff;
                left: 165px;
            }


            #login-part {
                margin-top: 100px;
            }

            .input-group .form-control {
                background: none;
                height: 42px;
            }

            @media only screen and (max-width: 768px) {
                .title {
                    font-size: 15px;
                }

                #big-img {
                    padding-right: 0px;
                }

                #pega-logo {
                    text-align: center;
                }

                #login-part {
                    margin-left: 15px;
                }


                h1 {
                    font-size: 15px;
                }

                .copy {
                    margin-top: 15px;
                }


                .carousel-caption {
                    left: 110px;
                }
            }

            @media only screen and (max-width: 500px) {
                .title {
                    font-size: 15px;
                }

                .copy {
                    margin-top: 15px;
                }

                #pega-logo {
                    text-align: center;
                }

                #big-img {
                    padding-right: 0px;
                }

                #login-part {
                    margin-left: 15px;
                    margin-top: 20px;
                }

                h1 {
                    font-size: 15px;
                }


                .carousel-caption {
                    left: 110px;
                }
            }
        </style>

        <script src="../pryceHome/js/select2.min.js"></script>
        <script src="../pryceHome/js/global.js"></script>


    </div>
</body>
</html>
