<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeFile="CloudLogin.aspx.cs" Inherits="CloudLogin" %>

<%@ Register Src="~/WebUserControl/TimeManLicense.ascx" TagPrefix="uc1" TagName="UpdateLicense" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">


    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>TimeMan-Cloud-Login</title>
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
                                <img src="images/TimeMan-Slide-1.jpg" alt="Phomello-LitePOS">
                                <div class="carousel-caption">
                                    <div style="border-bottom: solid #fff 4px; width: 100px; float: left;"></div>

                                    <h1 style="text-transform: uppercase;">Optimize Your Time 
          <br>
                                        Don't Manage it.. </h1>
                                    <a href="http://104.238.86.46:85/pegasus_license/" target="_blank">
                                        <button type="button" class="btn btn-danger">Request For Demo</button></a>
                                    <a href="http://104.238.86.46/timeman/cloudlogin.aspx" target="_blank">
                                        <button type="button" class="btn btn-primary">Cloud Login</button></a>

                                </div>
                            </div>

                            <div class="item">
                                <img src="images/TimeMan-Slide-2.jpg" alt="Phomello-Windows">
                                <div class="carousel-caption">
                                    <div style="border-bottom: solid #fff 4px; width: 200px; float: left;"></div>

                                    <h1 style="text-transform: uppercase;">Times have changed Your 
           <br>
                                        HR and Payroll</h1>
                                    <a href="http://104.238.86.46:85/pegasus_license/" target="_blank">
                                        <button type="button" class="btn btn-danger">Request For Demo</button></a>
                                    <a href="http://104.238.86.46/timeman/cloudlogin.aspx" target="_blank">
                                        <button type="button" class="btn btn-primary">Cloud Login</button></a>

                                </div>

                            </div>

                            <div class="item">
                                <img src="images/TimeMan-Slide-3.jpg" alt="Phomello-Android">
                                <div class="carousel-caption">
                                    <div style="border-bottom: solid #fff 4px; width: 300px; float: left;"></div>

                                    <h1 style="text-transform: uppercase;">Work with 
           <br>
                                        More Effectively</h1>
                                    <a href="http://104.238.86.46:85/pegasus_license/" target="_blank">
                                        <button type="button" class="btn btn-danger">Request For Demo</button></a>
                                    <a href="http://104.238.86.46/timeman/cloudlogin.aspx" target="_blank">
                                        <button type="button" class="btn btn-primary">Cloud Login</button></a>
                                    <br />
                                </div>
                            </div>

                        </div>


                        <!-- Left and right controls 
    <a class="left carousel-control" href="#myCarousel" data-slide="prev">
      <span class="glyphicon glyphicon-chevron-left"></span>
      <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#myCarousel" data-slide="next">
      <span class="glyphicon glyphicon-chevron-right"></span>
      <span class="sr-only">Next</span>
    </a>-->
                    </div>
                </div>

            </div>

            <div class="col-md-4" align="center" id="login-part">

                <img src="images/TimeMan-Logo.png" class="img-responsive" />
                <h4>TimeMan : Enterprise Time & Attendance Solution</h4>
                <br />
                <br />

                <form class="login-form" runat="server">
                    <asp:ScriptManager ID="Login_Script_Manager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="Login_Update" runat="server">
                        <ContentTemplate>
                             <asp:HiddenField ID="hdnConnection" runat="server" />
                            <div class="group">
                                <asp:TextBox ID="txtRegistratioCode" runat="server" Placeholder="Registration Code"></asp:TextBox>
                                <span class="highlight"></span><span class="bar"></span>

                            </div>
                            <br />


                            <div class="group">
                                <asp:TextBox ID="txtEmail" runat="server" Placeholder="UserName"></asp:TextBox>
                                <span class="highlight"></span><span class="bar"></span>

                            </div>
                            <br />


                            <div class="group">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Password"></asp:TextBox>
                                <span class="highlight"></span><span class="bar"></span>

                            </div>
                            <br />


                            <asp:LinkButton ID="lnkForgetpassword" runat="server" Text="Forgot Password?" Font-Bold="true" Font-Underline="true" ForeColor="#3c8dbc" OnClick="lnkForgetpassword_Click"></asp:LinkButton>


                            <br />
                            <div class="form-group sign-btn">
                                <br />
                                <asp:Button ID="btnLogin" runat="server" OnClick="btnlogin_Click" CssClass="btn btn-primary btn-block btn-flat" Text="Sign In" />
                                   <asp:Label ID="lblVersion" runat="server" ></asp:Label><br />
                                <asp:Label ID="lblReleaseDate" runat="server" ></asp:Label>
                            </div>

                            <p class="float">
                                <blink> <asp:Label ID="lblErrormessage" Width="150px" Font-Bold="true" Font-Size="12px" runat="server" ForeColor="Red"></asp:Label></blink>
                            </p>



                            <div class="modal fade" id="myModal" role="dialog">
                                <div class="modal-dialog modal-lg">

                                    <!-- Modal content-->
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            <div class="modal-title" style="text-align: left;">
                                                <img src="https://www.pegasustech.net/image/catalog/logo.png" class="img-responsive" width="120px" /></div>
                                        </div>

                                        <div>
                                            <uc1:UpdateLicense ID="UC_LicenseInfo" runat="server" />
                                        </div>
                                        <div class="modal-footer">
                                        </div>
                                    </div>

                                </div>
                            </div>
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

        </div>




        <!--<div class="row" align="left" style="margin-left:15px;" id="pega-logo">

	<div class="col-md-8"><a href="http://www.pegasustech.net" target="_blank">
    <img src="https://www.pegasustech.net/image/catalog/logo.png" width="120"/></a></div>
    <div class="col-md-4"><p class="copy">© 2019 <a href="http://www.pegasustech.net" target="_blank">Pegasus</a>. All right reserved.</p></div>

</div>-->


    </div>
    <!----close container-fluid ---->


</body>
<script type="text/javascript">

    function OpenLicenseModel() {
        $('#myModal').modal('show');
    }
</script>

<script type="text/javascript">

    function CloseLicenseModel() {
        $('#myModal').modal('hide');
        window.location.reload();
    }
</script>


<style>
    p {
        font-size: 12px;
        letter-spacing: 1px;
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
        margin-top: 120px;
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
        border-bottom: 1px solid #ccc;
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
        left: 160px;
        top: 10px;
        transition: 0.2s ease all;
        -moz-transition: 0.2s ease all;
        -webkit-transition: 0.2s ease all;
    }

    /* active state */
    input:focus ~ label, input:valid ~ label {
        top: -20px;
        font-size: 14px;
        color: #1e73be;
    }

    /* BOTTOM BARS ================================= */
    .bar {
        position: relative;
        display: block;
        width: 300px;
    }

        .bar:before, .bar:after {
            content: '';
            height: 2px;
            width: 0;
            bottom: 1px;
            position: absolute;
            background: #1e73be;
            transition: 0.2s ease all;
            -moz-transition: 0.2s ease all;
            -webkit-transition: 0.2s ease all;
        }

        .bar:before {
            left: 50%;
        }

        .bar:after {
            right: 50%;
        }

    /* active state */
    input:focus ~ .bar:before, input:focus ~ .bar:after {
        width: 50%;
    }

    /* HIGHLIGHTER ================================== */
    .highlight {
        position: absolute;
        height: 60%;
        width: 100px;
        top: 25%;
        left: 0;
        pointer-events: none;
        opacity: 0.5;
    }




    /* active state */
    input:focus ~ .highlight {
        -webkit-animation: inputHighlighter 0.3s ease;
        -moz-animation: inputHighlighter 0.3s ease;
        animation: inputHighlighter 0.3s ease;
    }

    /* ANIMATIONS ================ */
    @-webkit-keyframes inputHighlighter {
        from {
            background: #1e73be;
        }

        to {
            width: 0;
            background: transparent;
        }
    }

    @-moz-keyframes inputHighlighter {
        from {
            background: #1e73be;
        }

        to {
            width: 0;
            background: transparent;
        }
    }

    @keyframes inputHighlighter {
        from {
            background: #1e73be;
        }

        to {
            width: 0;
            background: transparent;
        }
    }
</style>
      <script src="../pryceHome/js/select2.min.js"></script>
        <script src="../pryceHome/js/global.js"></script>
    
  

</html>
