<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EmployeeTracking.aspx.cs" Inherits="Attendance_Report_EmployeeTracking" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title><%# Resources.Attendance.Report%></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="SHORTCUT ICON" href="Images/favicon.ico" />
    <link rel="stylesheet" href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css">
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css">
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
</head>
<body class="skin-blue" style="background-color: #ECF0F5">
    <form runat="server" id="form1">


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:HiddenField ID="hdnExceptionOption" runat="server" />
        <asp:HiddenField ID="hdn_l_value" runat="server" />
        <asp:HiddenField ID="hdn_u_value" runat="server" />
        <asp:HiddenField ID="hdn_isNoOfOcc" runat="server" Value="no" />



        <asp:UpdatePanel ID="Update1" runat="server">
            <ContentTemplate>
                <header class="main-header">
                    <a class="logo">
                        <span class="logo-lg">
                            <img style="width: 25px; margin-top: -5px;" src="../Images/compare.png" alt="" />
                            <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="<%$ Resources:Attendance,Tracking Report%>"></asp:Label>
                        </span>

                    </a>
                    <nav class="navbar navbar-static-top" style="text-align: right;">
                        <asp:Button ID="Btn_back" runat="server" Style="margin-top: 7px; margin-left: 10px; margin-right: 10px;" CssClass="btn btn-btn-warning" OnClick="lnkback_Click" Text="<%$ Resources:Attendance,Back %>" />
                    </nav>
                </header>

                <div class="content">
                    <section class="content">
                        <div class="box box-primary">
                            <div class="box-header with-border">

                                <div class="box-tools pull-right">
                                    <%--<button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                                <i class="fa fa-minus"></i>
                            </button>--%>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">

                                    <asp:TextBox ID="txtFromDate" runat="server" Class="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtToDate" runat="server" Class="form-control" Visible="false"></asp:TextBox>

                                    <div class="col-md-12">
                                        <div id="Div_ExceptionData" runat="server" class="box box-primary">
                                            <div class="box-header with-border">

                                                <asp:UpdatePanel ID="update_save" runat="server">
                                                    <ContentTemplate>

                                                        <h3 class="box-title">
                                                            <asp:Label ID="lbltrackrptValue" runat="server" Text="<%$ Resources:Attendance,Tracked Data For%>"></asp:Label>:
                                                        </h3>
                                                        &nbsp;&nbsp;

                                                        <asp:Label ID="lblTotalDataRecords" runat="server"></asp:Label>

                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Div_ExceptionData" runat="server" class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="up22" runat="server">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnExport" />
                                                                <asp:PostBackTrigger ControlID="btnExportExcel" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <div class="col-md-2">
                                                                    <br />
                                                                    <asp:Button ID="btnExport" Visible="false" runat="server" class="btn btn-primary" OnClick="btnExport_Click" Text="<%$ Resources:Attendance,PDF%>" />
                                                                    <br />

                                                                </div>
                                                                <div class="col-md-2" style="margin-left: 15px">
                                                                    <br />
                                                                    <asp:Button ID="btnExportExcel" Visible="false" runat="server" class="btn btn-primary" OnClick="btnExportExcel_Click" Text="<%$ Resources:Attendance,Excel%>" />
                                                                    <br />

                                                                    </div>
                                                                <br />

                                                                    </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <div style="overflow: auto">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvData" class="table table-bordered" runat="server" PageSize="<%# PageControlCommon.GetPageSize() %>" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                                                                OnPageIndexChanging="gvData_PageIndexChanging" OnSorting="gvData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                    <columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("emp_code")%>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("emp_name")%>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("location_name")%>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("department_name")%>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" HeaderStyle-Width="500px">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("designation")%>' Width="100px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#GetDate(Eval("att_date").ToString())%>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Day%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("att_day")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("shift_name")%>' Width="70px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Location%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("Assign_Location")%>' Width="70px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Week Off%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("is_weekOff")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time In%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("time_in_1")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("in_by_device_1")%>' Width="200px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Out%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("time_out_1")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("out_by_device_1")%>' Width="200px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Work Hour%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("work_hours_HhMM_1")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Prev ?%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time In%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("time_in_2")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("in_by_device_2")%>' Width="200px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Out%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("time_out_2")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("in_by_device_2")%>' Width="200px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Work Hour%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("work_hours_HhMM_2")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Next ?%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,WH Total%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("total_work_hours_HhMM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,In Missed%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" Checked='<%#Eval("is_in_missed")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Out Missed%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" Checked='<%#Eval("is_out_missed")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Late In%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("total_late_in_HhMM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Early Out%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("total_early_out_HhMM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,OT%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" Checked='<%#Eval("is_ot")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,OT Hrs%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("total_ot_hours_HhMM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave ?%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" Checked='<%#Eval("is_leave")%>' runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("leave_type")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,PH%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" Checked='<%#Eval("is_holiday")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Absent ?%>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" Checked='<%#Eval("is_absent")%>'></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Log Detail">
                                                                        <ItemTemplate>
                                                                            <asp:Label Enabled="false" runat="server" text='<%#Eval("log_detail")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </columns>
                                                                    <alternatingrowstyle cssclass="InvgridAltRow" backcolor="White"></alternatingrowstyle>
                                                                    <editrowstyle backcolor="#2461BF" />
                                                                    <footerstyle backcolor="#507CD1" font-bold="True" forecolor="black" />
                                                                    <headerstyle cssclass="Invgridheader" backcolor="#507CD1" font-bold="True" forecolor="White" />
                                                                    <pagerstyle cssclass="pagination-ys" backcolor="#2461BF" forecolor="black" horizontalalign="Center" />
                                                                    <rowstyle cssclass="Invgridrow" backcolor="#EFF3FB"></rowstyle>
                                                                    <selectedrowstyle backcolor="#D1DDF1" font-bold="True" forecolor="#333333" />
                                                                    <sortedascendingcellstyle backcolor="#F5F7FB" />
                                                                    <sortedascendingheaderstyle backcolor="#6D95E1" />
                                                                    <sorteddescendingcellstyle backcolor="#E9EBEF" />
                                                                    <sorteddescendingheaderstyle backcolor="#4870BE" />
                                                                    </asp:GridView>
                                                                </div>
                                                                </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <%--  <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
                                                Width="100%" AccessibilityCompliant="True">
                                                <Items>
                                                    <dx:ReportToolbarButton ItemKind="Search" />
                                                    <dx:ReportToolbarSeparator />
                                                    <dx:ReportToolbarButton ItemKind="PrintReport" />
                                                    <dx:ReportToolbarButton ItemKind="PrintPage" />
                                                    <dx:ReportToolbarSeparator />
                                                    <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                                                    <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                                                    <dx:ReportToolbarLabel ItemKind="PageLabel" />
                                                    <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                                                    </dx:ReportToolbarComboBox>
                                                    <dx:ReportToolbarLabel ItemKind="OfLabel" />
                                                    <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                                                    <dx:ReportToolbarButton ItemKind="NextPage" />
                                                    <dx:ReportToolbarButton ItemKind="LastPage" />
                                                    <dx:ReportToolbarSeparator />
                                                    <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                                                    <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                                                    <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                                        <Elements>
                                                            <dx:ListElement Value="pdf" />
                                                            <dx:ListElement Value="xls" />
                                                            <dx:ListElement Value="xlsx" />
                                                            <dx:ListElement Value="rtf" />
                                                            <dx:ListElement Value="mht" />
                                                            <dx:ListElement Value="html" />
                                                            <dx:ListElement Value="txt" />
                                                            <dx:ListElement Value="csv" />
                                                            <dx:ListElement Value="png" />
                                                        </Elements>
                                                    </dx:ReportToolbarComboBox>
                                                </Items>
                                                <Styles>
                                                    <LabelStyle>
                                                        <Margins MarginLeft="3px" MarginRight="3px" />
                                                    </LabelStyle>
                                                </Styles>
                                            </dx:ReportToolbar>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="Update_Report" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <dx:ReportViewer ID="rptViewer" Width="100%" runat="server">
                                                    </dx:ReportViewer>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>--%>
                            </div>
                        </div>
                    </section>
                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                        <div class="modal fade" id="LocationListPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel11" aria-hidden="true">
                                                            <div class="modal-dialog modal-mg">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal">
                                                                            <span aria-hidden="true">&times;</span><span class="sr-only"><%# Resources.Attendance.Close%></span></button>
                                                                        <h4 class="modal-title" id="myModalLabel11"><%# Resources.Attendance.Location%>
                                                                        </h4>
                                                                    </div>
                                                                    <div class="modal-body">

                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                            <ContentTemplate>
                                                                                <div id="divGVLocation" style="height: 400px; min-height: 250px; max-height: 500px; overflow: scroll;">

                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLocation" runat="server" AutoGenerateColumns="False" BorderStyle="None" AllowSorting="True">
                                                                                        <Columns>

                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkLocationSelectAll" runat="server" onclick="selectLocationAllCheckbox_Click(this)" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkLocationSelect" runat="server" onclick="selectLocationCheckBox_Click(this)" />
                                                                                                    <asp:HiddenField ID="hdnLocationtId" runat="server" Value='<%#Eval("Location_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>

                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name%>" SortExpression="Department_name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblLocation_name" runat="server" Text='<%#Eval("Location_Name") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="form-control" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                                            <%# Resources.Attendance.Close%></button>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div class="modal fade" id="DepartmentListPopup" tabindex="-1" role="dialog" aria-labelledby="DepartmentListPopup_Label" aria-hidden="true">
                                                            <div class="modal-dialog modal-mg">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal">
                                                                            <span aria-hidden="true">&times;</span><span class="sr-only"><%# Resources.Attendance.Close%></span></button>
                                                                        <h4 class="modal-title" id="DepartmentListPopup_Label"><%# Resources.Attendance.Department%>
                                                                        </h4>
                                                                    </div>
                                                                    <div class="modal-body">

                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                            <ContentTemplate>
                                                                                <div id="divGVDepartment" style="height: 400px; min-height: 250px; max-height: 500px; overflow: scroll;">

                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDetaprtmentData" runat="server" AutoGenerateColumns="False" BorderStyle="None" AllowSorting="True">
                                                                                        <Columns>

                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkDeptSelectAll" runat="server" onclick="selectDepartmentAllCheckbox_Click(this)" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkDeptSelect" runat="server" onclick="selectDepartmentCheckBox_Click(this)" />
                                                                                                    <asp:HiddenField ID="hdnDeptId" runat="server" Value='<%#Eval("Department_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>

                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name%>" SortExpression="Department_name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDepartment_name" runat="server" Text='<%#Eval("Department_name") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="form-control" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                                            Close</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="modal fade" id="EmployeeListPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true">
                                                            <div class="modal-dialog modal-mg">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal">
                                                                            <span aria-hidden="true">&times;</span><span class="sr-only"><%# Resources.Attendance.Close%></span></button>
                                                                        <h4 class="modal-title" id="myModalLabel2"><%# Resources.Attendance.Employee%>
                                                                        </h4>
                                                                    </div>
                                                                    <div class="modal-body">

                                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                            <ContentTemplate>
                                                                                <div id="divGVEmployee" style="height: 400px; min-height: 250px; max-height: 500px; overflow: scroll;">

                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AutoGenerateColumns="False" BorderStyle="None" AllowSorting="True">
                                                                                        <Columns>

                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkEmployeeSelectAll" runat="server" onclick="selectEmployeeAllCheckbox_Click(this)" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkEmployeeSelect" runat="server" onclick="selectEmployeeCheckBox_Click(this)" />
                                                                                                    <asp:HiddenField ID="hdnEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>

                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code%>" SortExpression="Emp_Code">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblEmp_Code" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="form-control" />
                                                                                            </asp:TemplateField>

                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblEmployee_name" runat="server" Text='<%#Eval("Emp_name") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="form-control" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                                            <%# Resources.Attendance.Close%></button>
                                                                    </div>

                                                                </div>

                                                            </div>
                                                        </div>

                                                        <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Save">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>

                                                        <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update1">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>

                                                        </form>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
                                                        <script src="../Bootstrap_Files/bootstrap/js/bootstrap.min.js"></script>
                                                        <script src="../Bootstrap_Files/plugins/slimScroll/jquery.slimscroll.min.js"></script>
                                                        <script src="../Bootstrap_Files/plugins/fastclick/fastclick.js"></script>
                                                        <script src="../Bootstrap_Files/dist/js/app.min.js"></script>
                                                        <script src="../Bootstrap_Files/dist/js/demo.js"></script>


                                                        <script type="text/javascript">

                                                            function findPositionWithScrolling(oElement) {
                                                                if (typeof (oElement.offsetParent) != 'undefined') {
                                                                    var originalElement = oElement;
                                                                    for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                                                                        posX += oElement.offsetLeft;
                                                                        posY += oElement.offsetTop;
                                                                        if (oElement != originalElement && oElement != document.body && oElement != document.documentElement) {
                                                                            posX -= oElement.scrollLeft;
                                                                            posY -= oElement.scrollTop;
                                                                        }
                                                                    }
                                                                    return [posX, posY];
                                                                } else {
                                                                    return [oElement.x, oElement.y];
                                                                }
                                                            }
                                                            function showCalendar(sender, args) {

                                                                var ctlName = sender._textbox._element.name;

                                                                ctlName = ctlName.replace('$', '_');
                                                                ctlName = ctlName.replace('$', '_');

                                                                var processingControl = $get(ctlName);
                                                                //var targetCtlHeight = processingControl.clientHeight;
                                                                sender._popupDiv.parentElement.style.top = processingControl.offsetTop + processingControl.clientHeight + 'px';
                                                                sender._popupDiv.parentElement.style.left = processingControl.offsetLeft + 'px';

                                                                var positionTop = processingControl.clientHeight + processingControl.offsetTop;
                                                                var positionLeft = processingControl.offsetLeft;
                                                                var processingParent;
                                                                var continueLoop = false;

                                                                do {
                                                                    // If the control has parents continue loop.
                                                                    if (processingControl.offsetParent != null) {
                                                                        processingParent = processingControl.offsetParent;
                                                                        positionTop += processingParent.offsetTop;
                                                                        positionLeft += processingParent.offsetLeft;
                                                                        processingControl = processingParent;
                                                                        continueLoop = true;
                                                                    }
                                                                    else {
                                                                        continueLoop = false;
                                                                    }
                                                                } while (continueLoop);

                                                                sender._popupDiv.parentElement.style.top = positionTop + 2 + 'px';
                                                                sender._popupDiv.parentElement.style.left = positionLeft + 'px';
                                                                sender._popupBehavior._element.style.zIndex = 10005;
                                                            }

      <%--  function resetEmpList() {
            document.getElementById('<%=txtEmpList.ClientID%>').value = "";
        }--%>


                                                            function selectLocationCheckBox_Click(id) {

                                                                var gridView = document.getElementById('<%=gvLocation.ClientID%>');
            var AtLeastOneCheck = false;
            var cell;
            for (var i = 1; i < gridView.rows.length; i++) {
                cell = gridView.rows[i].cells[0];
                if (cell.getElementsByTagName("INPUT")[0].checked == false) {
                    AtLeastOneCheck = true;
                    break;
                }
            }
            gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = !AtLeastOneCheck;

        }

        function selectDepartmentCheckBox_Click(id) {

            var gridView = document.getElementById('<%=gvDetaprtmentData.ClientID%>');
            var AtLeastOneCheck = false;
            var cell;
            for (var i = 1; i < gridView.rows.length; i++) {
                cell = gridView.rows[i].cells[0];
                if (cell.getElementsByTagName("INPUT")[0].checked == false) {
                    AtLeastOneCheck = true;
                    break;
                }
            }
            gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = !AtLeastOneCheck;

            //SelectedEmpCount();
        }


        function selectEmployeeCheckBox_Click(id) {

            var gridView = document.getElementById('<%=gvEmployee.ClientID%>');
                    var AtLeastOneCheck = false;
                    var cell;
                    for (var i = 1; i < gridView.rows.length; i++) {
                        cell = gridView.rows[i].cells[0];
                        if (cell.getElementsByTagName("INPUT")[0].checked == false) {
                            AtLeastOneCheck = true;
                            break;
                        }
                    }
                    gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = !AtLeastOneCheck;

                    //SelectedEmpCount();
                }

                function selectLocationAllCheckbox_Click(id) {

                    var gridView = document.getElementById('<%=gvLocation.ClientID%>');
                    for (var i = 1; i < gridView.rows.length; i++) {
                        var cell = gridView.rows[i].cells[0];
                        cell.getElementsByTagName("INPUT")[0].checked = id.checked;
                    }
                    //SetEmpList();
                    //SelectedEmpCount();
                }

                function selectDepartmentAllCheckbox_Click(id) {

                    var gridView = document.getElementById('<%=gvDetaprtmentData.ClientID%>');
                    for (var i = 1; i < gridView.rows.length; i++) {
                        var cell = gridView.rows[i].cells[0];
                        cell.getElementsByTagName("INPUT")[0].checked = id.checked;
                    }
                    //SetEmpList();
                    //SelectedEmpCount();
                }

                function selectEmployeeAllCheckbox_Click(id) {

                    var gridView = document.getElementById('<%=gvEmployee.ClientID%>');
             for (var i = 1; i < gridView.rows.length; i++) {
                 var cell = gridView.rows[i].cells[0];
                 cell.getElementsByTagName("INPUT")[0].checked = id.checked;
             }
             //SetEmpList();
             //SelectedEmpCount();
         }

                                                        </script>
</body>
</html>
