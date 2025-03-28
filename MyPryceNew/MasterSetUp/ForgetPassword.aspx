<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPassword.aspx.cs" Inherits="MasterSetUp_ForgetPassword" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Pryce | Reset Password</title>
    <link rel="shortcut icon" href="../Images/favicon.ico" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link href="../BootStrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../BootStrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="../BootStrap_For_Pryce_Master/font-awesome-4.7.0/css/font-awesome.min.css">
  <link rel="stylesheet" href="../BootStrap_For_Pryce_Master/ionicons-2.0.1/css/ionicons.min.css">  
  <link rel="stylesheet" href="../BootStrap_For_Pryce_Master/plugins/iCheck/square/blue.css">--%>
</head>
<body>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>

    <div class="container">
        <div id="login-box">
            <div class="logo">

                <div id="circle">
                    <img src="../Images/pryceLogo.png" class="img-responsive" width="200" style="padding-top: 45px;" /></div>
                <h3 class="logo-caption">Change Password</h3>
            </div>
            <!-- /.logo -->
            <div class="controls">



                <form id="Login_Form" runat="server">
                    <asp:ScriptManager ID="Login_Script_Manager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="Login_Update" runat="server">
                        <Triggers>
                           
                                   <asp:PostBackTrigger ControlID="btnSave" />
                                     <asp:PostBackTrigger ControlID="btnReset" />
                            
                        </Triggers>
                        <ContentTemplate>
                            <div class="form-group has-feedback">
                                <asp:TextBox ID="txtOldPassword" runat="server" class="form-control" TextMode="Password" placeholder="Enter Old Password"></asp:TextBox>
                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOldPassword" ErrorMessage="<%$ Resources:Attendance,Enter Old Password %>"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group has-feedback">
                                <asp:TextBox ID="txtNewPassword" runat="server" class="form-control" TextMode="Password" placeholder="Enter New Password"></asp:TextBox>
                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNewPassword" ErrorMessage="<%$ Resources:Attendance,Enter New Password %>"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group has-feedback">
                                <asp:TextBox ID="txtReEnterPass" runat="server" class="form-control" TextMode="Password" placeholder="Re-Enter New Password"></asp:TextBox>
                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReEnterPass" ErrorMessage="<%$ Resources:Attendance,Re-Enter New Password %>"></asp:RequiredFieldValidator>

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtReEnterPass" CssClass="ValidationError" ControlToCompare="txtNewPassword" ErrorMessage="New password and Re-Enter password does not match" ValidationGroup="Save" ToolTip="Password must be the same" />
                            </div>

                           
                            <div class="col-md-6">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" ValidationGroup="Save" CssClass="btn btn-success btn-block btn-flat" Text="Save" style="background-color: #088ab5;    color: white;" />
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-primary btn-block btn-flat" Text="Reset" style="background-color: #088ab5;    color: white;"/>
                            </div>
                            <br />
                             <div class="col-md-12" style="padding-top:10px;">
                                
                            </div>
                       
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Login_Update">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </form>



            </div>
            <!-- /.controls -->
        </div>
        <!-- /#login-box -->
    </div>
    <!-- /.container -->





    <!--================***=========== CSS START ============***================-->


    <style>
        @import url('https://fonts.googleapis.com/css?family=Nunito');
        @import url('https://fonts.googleapis.com/css?family=Poiret+One');

        body, html {
            background: #fff;
        }

        #login-box {
            position: absolute;
            top: 50px;
            left: 50%;
            transform: translateX(-50%);
            width: 350px;
            margin: 0 auto;
            border: 1px solid black;
            background: #595858;
            min-height: 250px;
            padding: 20px;
            z-index: 9999;
        }

            #login-box .logo .logo-caption {
                font-family: "Times New Roman", Times, serif;
                color: white;
                text-align: center;
                margin-bottom: 0px;
            }

            #login-box .logo .tweak {
                color: #ff5252;
            }

            #login-box .controls {
                padding-top: 30px;
            }

                #login-box .controls input {
                    border-radius: 0px;
                    background: #f0f0f0;
                    border: 0px;
                    color: #333;
                    font-family: 'Nunito', sans-serif;
                }

                    #login-box .controls input:focus {
                        box-shadow: none;
                    }

                    #login-box .controls input:first-child {
                        border-top-left-radius: 2px;
                        border-top-right-radius: 2px;
                    }

                    #login-box .controls input:last-child {
                        border-bottom-left-radius: 2px;
                        border-bottom-right-radius: 2px;
                    }

            #login-box button.btn-custom {
                border-radius: 2px;
                margin-top: 8px;
                background: #088ab5;
                border-color: rgba(48, 46, 45, 1);
                color: white;
                font-family: 'Nunito', sans-serif;
            }

                #login-box button.btn-custom:hover {
                    -webkit-transition: all 500ms ease;
                    -moz-transition: all 500ms ease;
                    -ms-transition: all 500ms ease;
                    -o-transition: all 500ms ease;
                    transition: all 500ms ease;
                    background: rgba(48, 46, 45, 1);
                    border-color: #088ab5;
                }


            #login-box button.btn-blue {
                margin-top: 8px;
                background: #111111;
                border-color: rgba(48, 46, 45, 1);
                color: white;
                font-family: 'Nunito', sans-serif;
            }

                #login-box button.btn-blue:hover {
                    -webkit-transition: all 500ms ease;
                    -moz-transition: all 500ms ease;
                    -ms-transition: all 500ms ease;
                    -o-transition: all 500ms ease;
                    transition: all 500ms ease;
                    background: rgba(48, 46, 45, 1);
                    border-color: #088ab5;
                }




        #circle {
            background: #FFFFFF;
            border-radius: 200px;
            height: 150px;
            width: 150px;
            margin-left: 80px;
        }
    </style>

    <%--<script src="../BootStrap_For_Pryce_Master/plugins/jQuery/jquery-2.2.3.min.js"></script>--%>
    <%--<script src="../BootStrap_For_Pryce_Master/bootstrap/js/bootstrap.min.js"></script>--%>
    <%--<script src="../BootStrap_For_Pryce_Master/plugins/iCheck/icheck.min.js"></script>--%>
    <%--<script>
    $(function () {
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });
    });
</script>--%>
    <script>
        function showAlert(data, bgcolor, txtcolor) {
       var x = document.getElementById("snackbar");
       x.innerHTML = data;
       x.style.backgroundColor = bgcolor;
       x.style.color = txtcolor;
       x.className = "show";
       setTimeout(function () { x.className = x.className.replace("show", ""); }, 10000);
   }
               </script>

</body>
</html>
