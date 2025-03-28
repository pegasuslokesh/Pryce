<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="MasterSetUp_Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pryce Location Setup</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
</head>



<body id="body1" runat="server">
    <div class="container-fluid" style="padding-left: 0px;">
        <div class="row">
            <div class="col-md-8" style="padding-left: 0px;" id="big-img">

                <div class="carousel-inner">
                    <div id="myCarousel" class="carousel slide" data-ride="carousel">


                        <div class="carousel-inner">

                            <div class="item active">
                                <img src="../Images/Pryce-Cloud-System-4.png" alt="Pegasus" runat="server" id="imgAppBanner" />
                                <div class="carousel-caption">

                                   <h1 style="text-transform: uppercase; margin-bottom: 15px;">Enhancing Creativity





          <br>
                                       for Business Success</h1>
<%--                                    <p>TimeMan Cloud is a convenient cloud-based time & attendance solution like no other. Designed to help organizations reduce administrative overheads by eliminating manual timecard calculations,Timeman Cloud also helps reduce support and labour costs, as storage of all data from your terminals is managed, maintained, and monitored by Timeman Cloud servers.</p>--%>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>


            <!--------------------------------------- Login Part --------------------------------------->


            <div class="col-md-4" id="login-part">
                <form id="Login_Form" class="login-form" runat="server">
                    <asp:ScriptManager ID="Home_Script" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="Home_Udpate" runat="server">
                        <ContentTemplate>

                            <div class="box-body">
                                <div class="row">


                                    <div align="center">

                                        <a href="http://timeman.pegasustech.net/" target="_blank">
                                            <img src="../Images/Pryce-Logo.png" class="img-responsive" runat="server" id="imgAppLogo"></a>
                                        <h4 runat="server" id="hTitle">Pryce Enterprise Resource Planning</h4>

                                    </div>

                                    <div class="form-group" align="left">
                                        <asp:Label ID="Label1" Text="<%$ Resources:Attendance,Company %>" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlCompany" class="form-control select2" Style="width: 100%;" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <div class="form-group" align="left">
                                        <asp:Label ID="Label2" Text="<%$ Resources:Attendance,Brand/Division%>" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlBrand" class="form-control select2" Style="width: 100%;" runat="server" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <div class="form-group" align="left">
                                        <asp:Label ID="Label3" Text="<%$ Resources:Attendance,Location %>" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlLocation" class="form-control select2" Style="width: 100%;" runat="server" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <div class="form-group" runat="server" id="Div_Fyc" visible="false" align="left">
                                        <asp:Label ID="Label4" Text="<%$ Resources:Attendance,Year %>" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlFinanceyear" class="form-control select2" Style="width: 100%;" runat="server" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <div class="form-group" runat="server" align="left">
                                        <asp:Label ID="Label5" Text="<%$ Resources:Attendance,Language %>" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlLanguage" class="form-control select2" Style="width: 100%;" runat="server" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="<%$ Resources:Attendance,US English %>"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="<%$ Resources:Attendance,Arabic %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group sign-btn">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-lg btn-primary btn-block" Text="<%$ Resources:Attendance,Save %>" />
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                    </div>
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>                   
                </form>
                 <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Home_Udpate">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
            </div>
        </div>
    </div>
    <!--------------------------------------- CSS Start --------------------------------------->
    <style>
        p {
            font-size: 12px;
            letter-spacing: 1px;
            text-align: justify;
        }

        h1 {
            font-size: 30px;
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
            margin-top: 40px;
        }

        .input-group .form-control {
            background: none;
            height: 42px;
        }

        a {
            color: #FF0000;
        }

            a:hover {
                text-decoration: underline;
                color: #FF0000;
            }


        @media only screen and (max-width: 768px) {
            .title {
                font-size: 15px;
            }

            p {
                font-size: 11px;
                letter-spacing: 1px;
                text-align: justify;
            }

            h1 {
                font-size: 18px;
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

            .copy {
                margin-top: 15px;
            }


            .carousel-caption {
                position: absolute;
                top: 0px;
                text-align: justify;
                color: #fff;
                left: 60px;
            }
        }






        @media only screen and (max-width: 500px) {
            .title {
                font-size: 15px;
            }

            p {
                font-size: 11px;
                letter-spacing: 1px;
                text-align: justify;
            }

            h1 {
                font-size: 18px;
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
                margin: 25px;
                
            }

            .carousel-caption {
                position: absolute;
                top: 0px;
                text-align: justify;
                color: #fff;
                left: 60px;
            }
        }





        /* Animate Input ------------------------------------------ */

        * {
            box-sizing: border-box;
        }


        /* form starting stylings ------------------------------- */
        .group {
            position: relative;
        }

        input {
            font-size: 16px;
            padding: 10px 10px 10px 5px;
            display: block;
            width: 300px;
            border: none;
            border-bottom: 1px solid #757575;
        }

            input:focus {
                outline: none;
            }

        /* LABEL ======================================= */
        label {
            color: #999;
            font-size: 16px;
            font-weight: normal;
            position: absolute;
            pointer-events: none;
            left: 170px;
            top: 10px;
            transition: 0.2s ease all;
            -moz-transition: 0.2s ease all;
            -webkit-transition: 0.2s ease all;
        }

        /* active state */
        input:focus ~ label, input:valid ~ label {
            top: -20px;
            font-size: 14px;
            color: #da0611;
        }
    </style>


    <!-- Jquery JS-->

    <!-- Vendor JS-->
    <script src="../pryceHome/js/select2.min.js"></script>
    <!-- Main JS-->
    <script src="../pryceHome/js/global.js"></script>



</body>
</html>
