<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ExceptionCountReport.aspx.cs" Inherits="Attendance_Report_ExceptionCountReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

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
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
</head>
<body class="skin-blue" style="background-color: #ECF0F5">
    <form runat="server" id="form1">


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <asp:UpdatePanel ID="Update1" runat="server">
            <ContentTemplate>
                <header class="main-header">
                    <a class="logo">
                        <span class="logo-lg">
                            <img style="width: 25px; margin-top: -5px;" src="../Images/compare.png" alt="" />
                            <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="<%$ Resources:Attendance,Exception Report%>"></asp:Label>
                        </span>
                    </a>
                    <nav class="navbar navbar-static-top" style="text-align: right;">
                        <asp:Button ID="Btn_back" runat="server" Style="margin-top: 7px; margin-left: 10px; margin-right: 10px;" CssClass="btn btn-btn-warning" OnClick="lnkback_Click" Text="<%$ Resources:Attendance,Back %>" />
                    </nav>
                </header>

                <%--<asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#EmployeeListPopup" Text="EmployeeList" />
                <asp:HiddenField ID="hdnEmpList" runat="server" />
                <asp:Button ID="Btn_Department_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#DepartmentListPopup" Text="DepartmentList" />
                <asp:HiddenField ID="hdnDepartmentList" runat="server" />--%>


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
                                    <div class="col-md-12">
                                        <div id="Div1" style="margin-top: 15px; margin-bottom: 15px;" runat="server" class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"><%# Resources.Attendance.Exception_Count_Report%></h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblExceptionOptions" runat="server" Text="<%$ Resources:Attendance,Exception Options%>"></asp:Label>
                                                        <asp:RadioButton ID="radioAbsent" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Absent%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radioMissedCheckIn" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Missed Check-In%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radioMissedCheckOut" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Missed Check-Out%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radioLateIn" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Late In%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radioEarlyOut" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Early Out%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="radioLateInEalryOut" runat="server" GroupName="rptOption" Text="<%$ Resources:Attendance,Late-In & Ealry-Out%>" AutoPostBack="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">

                                                        <asp:Label ID="lblNoOfOccurance" runat="server" Text="<%$ Resources:Attendance,No Of Occurance%>"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text=">= "></asp:Label>
                                                        <asp:TextBox ID="txtNoOfOccurance" runat="server" MaxLength="6" Class="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtNoOfOccurance" FilterType="Numbers"></cc1:FilteredTextBoxExtender>


                                                        <br />
                                                    </div>

                                                    <div class="col-md-4" id="divFrom" runat="server" style="display: none">

                                                        <asp:Label ID="lblLateEarlyFrom" runat="server" Text="<%$ Resources:Attendance,Late In By/Early Out By From (In Minutes)%>"></asp:Label>
                                                        <asp:TextBox ID="txtLateEarlyFrom" runat="server" MaxLength="6" Class="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtLateEarlyFrom" FilterType="Numbers"></cc1:FilteredTextBoxExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="divTo" runat="server" style="display: none">
                                                        <asp:Label ID="lblLateEarlyTo" runat="server" Text="<%$ Resources:Attendance,To (In Minutes)%>"></asp:Label>
                                                        <asp:TextBox ID="txtlblLateEarlyTo" runat="server" MaxLength="6" Class="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtlblLateEarlyTo" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12"></div>

                                                    <%--                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                     <div class="col-md-4">
                                                        <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                                        <asp:DropDownList ID="ddlDepartment" runat="server" Class="form-control" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                                        <br />
                                                    </div>--%>

                                                    <%-- <div class="col-md-2">
                                                        <asp:Label ID="lbldept" runat="server" Text="Employee List"></asp:Label>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/employee.png" Height="30px" Width="30px" OnClientClick="visibleGrid()" />
                                                        <br />
                                                    </div>--%>

                                                    <%--                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblEmpName" runat="server" Text="Employee List"></asp:Label>
                                                        <asp:ImageButton ID="iBtnDDLEmpList" runat="server" ImageUrl="~/Images/employee.png" Height="30px" Width="30px" OnClientClick="visibleGrid()" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                                                        <asp:TextBox ID="txtFromDate" runat="server" Class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                                                        <asp:TextBox ID="txtToDate" runat="server" Class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>
                                                    --%>



                                                    <asp:UpdatePanel ID="upbtn" runat="server">
                                                        <ContentTemplate>
                                                            <div class="col-md-1">
                                                                <br />
                                                                <asp:Button ID="btnGetReport" runat="server" class="btn btn-primary" OnClick="btnGetReport_Click" Text="<%$ Resources:Attendance,Get Report%>" />&nbsp;&nbsp;&nbsp;
                                                                <br />
                                                            </div>
                                                            <div class="col-md-1">
                                                                <br />
                                                                <asp:Button ID="btnReset" runat="server" class="btn btn-primary" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset%>" />
                                                                <br />

                                                            </div>

                                                            <asp:UpdatePanel ID="export" runat="server">
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnExport" />
                                                                    <asp:PostBackTrigger ControlID="btnExportExcel" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <div class="col-md-1" style="margin-left: -10px">
                                                                        <br />
                                                                        <asp:Button ID="btnExport" Visible="false" runat="server" class="btn btn-primary" OnClick="btnExport_Click" Text="<%$ Resources:Attendance,PDF%>" />
                                                                        <br />
                                                                    </div>

                                                                    <div class="col-md-1" style="margin-left: -10px">
                                                                        <br />
                                                                        <asp:Button ID="btnExportExcel" Visible="false" runat="server" class="btn btn-primary" OnClick="btnExportExcel_Click" Text="<%$ Resources:Attendance,Excel%>" />
                                                                        <br />
                                                                        </div>

                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>


                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div id="Div_ExceptionData" runat="server" class="box box-primary collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"><%# Resources.Attendance.Exception_Data%></h3>
                                                <asp:Label ID="lblTotalDataRecords" runat="server"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Div_ExceptionData" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">


                                                    <div class="col-md-12">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvData" runat="server" PageSize="<%# PageControlCommon.GetPageSize() %>" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                            OnPageIndexChanging="gvData_PageIndexChanging" OnSorting="gvData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                        <columns>
                                                               

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Emp Code%>" HeaderStyle-Width="4%" SortExpression="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("Emp_Code")%>'></asp:Label>

                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="4%"></HeaderStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" HeaderStyle-Width="20%" SortExpression="Emp_Name">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnEmpCode" runat="server" ToolTip="Show All Records" OnCommand="lbtnEmpCode_Command" CommandName='<%#Eval("Emp_Name")+"/"+Eval("Emp_Code")+","+Eval("emp_id")%>' CommandArgument='<%#Eval("Location_Id")%>' Text='<%#Eval("Emp_Name")%>'></asp:LinkButton>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                                </asp:TemplateField>

                                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name%>" HeaderStyle-Width="6%" SortExpression="Location_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("Location_Name")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="6%"></HeaderStyle>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>" HeaderStyle-Width="15%" SortExpression="Dep_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("Dep_Name")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" HeaderStyle-Width="15%" SortExpression="Designation">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("Designation")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="<%$ Resources:Attendance,No Of Occurance%>" HeaderStyle-Width="5%" SortExpression="No_Of_Occurance">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lBtnNoOfOccurance" ToolTip="Show Occurance Records" runat="server" OnCommand="lBtnNoOfOccurance_Command" CommandName='<%#Eval("Emp_Name")+"/"+Eval("Emp_Code")+","+Eval("emp_id")%>' CommandArgument='<%#Eval("Location_Id")%>' Text='<%#Eval("No_Of_Occurance")%>'></asp:LinkButton>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="5%"></HeaderStyle>

                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </columns>
                                                                        <alternatingrowstyle cssclass="InvgridAltRow" backcolor="White"></alternatingrowstyle>
                                                                        <editrowstyle backcolor="#2461BF" />
                                                                        <footerstyle backcolor="#507CD1" font-bold="True" forecolor="Black" />
                                                                        <headerstyle cssclass="Invgridheader" backcolor="#507CD1" font-bold="True" forecolor="White" />
                                                                        <pagerstyle cssclass="pagination-ys" backcolor="#2461BF" forecolor="Black" horizontalalign="Center" />
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



                                                            <div class="modal fade" id="EmployeeListPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                                                <div class="modal-dialog modal-mg">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header">
                                                                            <button type="button" class="close" data-dismiss="modal">
                                                                                <span aria-hidden="true">&times;</span><span class="sr-only"><%# Resources.Attendance.Close%></span></button>

                                                                        </div>
                                                                        <div class="modal-body">

                                                                            <asp:UpdatePanel ID="up1" runat="server">
                                                                                <ContentTemplate>
                                                                                    <div id="divGV" style="height: 400px; min-height: 250px; max-height: 500px; overflow: scroll;">
                                                                                        <div class="col-md-12">

                                                                                            <asp:Label ID="lbltotalRecords" runat="server"></asp:Label>

                                                                                            <br />
                                                                                        </div>
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmployeeList" runat="server" AutoGenerateColumns="False" BorderStyle="None" AllowSorting="True" OnSorting="GvEmployeeList_Sorting">
                                                                                            <Columns>

                                                                                                <asp:TemplateField HeaderText="Select">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="selectAllCheckbox_Click(this)" />
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkSelect" runat="server" onclick="selectCheckBox_Click(this)" />
                                                                                                        <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%#Eval("Emp_Id") %>' Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code%>" SortExpression="Emp_Code">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEmp_Id" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name%>" SortExpression="Emp_Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Emp_Name") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="form-control" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>

                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                                            <%# Resources.Attendance.Close%></button>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="modal fade" id="DepartmentListPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                                                <div class="modal-dialog modal-mg">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header">
                                                                            <button type="button" class="close" data-dismiss="modal">
                                                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>

                                                                        </div>
                                                                        <div class="modal-body">

                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                <ContentTemplate>
                                                                                    <div id="divGVDepartment" style="height: 400px; min-height: 250px; max-height: 500px; overflow: scroll;">
                                                                                        <div class="col-md-12">

                                                                                            <asp:Label ID="lblDepartmentList" runat="server"></asp:Label>

                                                                                            <br />
                                                                                        </div>
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDetaprtmentData" runat="server" AutoGenerateColumns="False" BorderStyle="None" AllowSorting="True">
                                                                                            <Columns>

                                                                                                <asp:TemplateField HeaderText="Select">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:CheckBox ID="chkDeptSelectAll" runat="server" onclick="selectAllDepartmentCheckbox_Click(this)" />
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
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>

                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                                            <%# Resources.Attendance.Close%></button>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update1">
                                                                <ProgressTemplate>
                                                                    <div class="modal_Progress">
                                                                        <div class="center_Progress">
                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                        </div>
                                                                    </div>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upbtn">
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

      <%--  function visibleGrid() {
            document.getElementById('<%=Btn_Address_Modal.ClientID%>').click();
        }

        function visibleDepartmentGrid() {
            document.getElementById('<%=Btn_Department_Modal.ClientID%>').click();
        }--%>

                                                                function visibleContent(data) {

                                                                    if (data == 'yes') {
                                                                        document.getElementById('<%=divFrom.ClientID%>').style.display = 'block';
                document.getElementById('<%=divTo.ClientID%>').style.display = 'block';
            }
            else {
                document.getElementById('<%=divFrom.ClientID%>').style.display = 'none';
                document.getElementById('<%=divTo.ClientID%>').style.display = 'none';
            }

        }

        function selectAllCheckbox_Click(id) {

            var gridView = document.getElementById('<%=GvEmployeeList.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                cell.getElementsByTagName("INPUT")[0].checked = id.checked;
            }
            //SetEmpList();
            //SelectedEmpCount();
        }

        function selectAllDepartmentCheckbox_Click(id) {

            var gridView = document.getElementById('<%=gvDetaprtmentData.ClientID%>');
             for (var i = 1; i < gridView.rows.length; i++) {
                 var cell = gridView.rows[i].cells[0];
                 cell.getElementsByTagName("INPUT")[0].checked = id.checked;
             }
             //SetEmpList();
             //SelectedEmpCount();
         }

         function selectCheckBox_Click(id) {

             var gridView = document.getElementById('<%=GvEmployeeList.ClientID%>');
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

          function SelectedEmpCount() {
              var count = 0;
              var gridView = document.getElementById('<%=GvEmployeeList.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                if (cell.getElementsByTagName("INPUT")[0].checked == true) {
                    count++;
                }
            }
            <%--document.getElementById('<%=lblSelectedRecords.ClientID%>').innerText = 'Total Employees Selected :' + count;   --%>
        }


       <%-- function SetEmpList() {
            var list = "";
            var gridView = document.getElementById('<%=GvEmployeeList.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[1];
                if (cell.getElementsByTagName("INPUT")[1].checked == true) {
                    alert("in");
                    list = list + cell.getElementsByTagName("INPUT")[0].value + ",";
                }
            }
            alert(list);
            document.getElementById('<%=hdnEmpList.ClientID%>').innerText = list;

        }--%>

                                                                function Open_div_empData() {
                                                                    Div_ExceptionData.Attributes.Add("Class", "fa fa-minus");
                                                                    Btn_Div_ExceptionData.Attributes.Add("Class", "box box-primary");
                                                                }

                                                                function EmployeeTrackingReport() {
                                                                    debugger;
                                                                    window.open("../Attendance_Report/EmployeeTracking.aspx");
                                                                }


                                                            </script>
</body>
</html>
