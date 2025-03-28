<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkHourReport.aspx.cs" Inherits="Attendance_Report_WorkHourReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<!DOCTYPE html>
<html>
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
                    <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="Work Hour Details Report"></asp:Label>
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
                                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Size="XX-Large" />
                                            <br />
                                        </div>
                                        <div class="col-md-6" style="text-align: right;">

                                            <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Download%>" CssClass="btn btn-primary" OnClick="btnDownload_Click" />
                                            <br style="text-align: center; vertical-align: middle;" />
                                        </div>
                                        <br />
                                        <div class="col-md-12" runat="server" id="divexport">
                                            <br />
                                            <div class="col-md-12" style="width: 100%; overflow: auto;" runat="server" id="mDiv">
                                                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Employee Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Employee Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("Emp_Name") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                         <asp:TemplateField HeaderText="Day 1">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay1" runat="server" Text='<%#Eval("Day 1") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 2">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay2" runat="server" Text='<%#Eval("Day 2") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 3">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay3" runat="server" Text='<%#Eval("Day 3") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 4">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay4" runat="server" Text='<%#Eval("Day 4") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 5">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay5" runat="server" Text='<%#Eval("Day 5") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 6">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay6" runat="server" Text='<%#Eval("Day 6") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 7">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay7" runat="server" Text='<%#Eval("Day 7") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 8">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay8" runat="server" Text='<%#Eval("Day 8") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 9">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay9" runat="server" Text='<%#Eval("Day 9") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 10">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay10" runat="server" Text='<%#Eval("Day 10") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 11">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay11" runat="server" Text='<%#Eval("Day 11") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 12">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay12" runat="server" Text='<%#Eval("Day 12") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 13">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay13" runat="server" Text='<%#Eval("Day 13") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 14">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay14" runat="server" Text='<%#Eval("Day 14") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 15">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay15" runat="server" Text='<%#Eval("Day 15") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 16">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay16" runat="server" Text='<%#Eval("Day 16") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 17">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay17" runat="server" Text='<%#Eval("Day 17") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 18">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay18" runat="server" Text='<%#Eval("Day 18") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 19">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay19" runat="server" Text='<%#Eval("Day 19") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 20">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay20" runat="server" Text='<%#Eval("Day 20") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 21">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay21" runat="server" Text='<%#Eval("Day 21") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 22">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay22" runat="server" Text='<%#Eval("Day 22") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 23">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay23" runat="server" Text='<%#Eval("Day 23") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 24">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay24" runat="server" Text='<%#Eval("Day 24") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 25">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay25" runat="server" Text='<%#Eval("Day 25") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 26">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay26" runat="server" Text='<%#Eval("Day 26") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 27">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay27" runat="server" Text='<%#Eval("Day 27") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 28">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay28" runat="server" Text='<%#Eval("Day 28") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 29">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay29" runat="server" Text='<%#Eval("Day 29") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 30">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay30" runat="server" Text='<%#Eval("Day 30") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Day 31">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay31" runat="server" Text='<%#Eval("Day 31") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>        
                                                           <asp:TemplateField HeaderText="Total Hour">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalHour" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                             
                                                    </Columns>
                                                </asp:GridView>
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

