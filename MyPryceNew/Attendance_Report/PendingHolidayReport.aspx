<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingHolidayReport.aspx.cs" Inherits="Attendance_Report_PendingHolidayReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Report</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="SHORTCUT ICON" href="Images/favicon.ico" />
    <link rel="stylesheet" href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css">
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css">
</head>
<body class="skin-blue" style="background-color: #ECF0F5">
  <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header class="main-header">
            <a class="logo">
                <span class="logo-lg">
                    <img style="width: 25px; margin-top: -5px;" src="../Images/product_icon.png" alt="" />
                    <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="Pending Holiday Report"></asp:Label>
                </span>
            </a>
            <nav class="navbar navbar-static-top">
            </nav>
        </header>
        <asp:UpdatePanel ID="Update1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnDownload" />
            </Triggers>
            <ContentTemplate>
                <div class="content">
                    <section class="content">


                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="col-md-6" style="text-align: left;">

                                            <br />
                                        </div>
                                        <div class="col-md-6" style="text-align: right;">

                                            <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Download%>" CssClass="btn btn-primary" OnClick="btnDownload_Click" />

                                            <%--<asp:ImageButton ID="btnExportPdf" runat="server" CommandArgument="1" CommandName="OP"
                                            Height="23px" ImageUrl="~/Images/pdfIcon.jpg" OnCommand="btnExportPdf_Command" />
                                        <asp:ImageButton ID="btnExportToExcel" runat="server" CommandArgument="2" CommandName="OP"
                                            Height="21px" ImageUrl="~/Images/excel-icon.gif" OnCommand="btnExportPdf_Command" />--%>
                                            <br style="text-align:center;vertical-align:middle;" />
                                        </div>
                                        <br />
                                        <div class="col-md-12" runat="server" id="divexport" >
                                            <br />
                                            <div class="col-md-12" style="width: 100%;overflow: auto;" runat="server" id="mDiv" >
                                            <%--<div class="col-md-12" style="overflow: auto; max-height: 400px; width: 100%;" runat="server" id="mDiv">--%>
                                                <%--  <br />


                                            <asp:Table ID="Table1" runat="server" BorderStyle="Solid" CellPadding="1" CellSpacing="1" Font-Size="12px"
                                                    GridLines="Both" Height="22px" Width="100%" >
                                            </asp:Table>
                                            <br />--%>
                                            </div>
                                           
                                           
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </section>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/bootstrap/js/bootstrap.min.js"></script>
    <script src="../Bootstrap_Files/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <script src="../Bootstrap_Files/plugins/fastclick/fastclick.js"></script>
    <script src="../Bootstrap_Files/dist/js/app.min.js"></script>
    <script src="../Bootstrap_Files/dist/js/demo.js"></script>
</body>
</html>
