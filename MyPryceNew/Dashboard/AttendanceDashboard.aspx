<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AttendanceDashboard.aspx.cs" Inherits="Dashboard_AttendanceDashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link rel="stylesheet" href="../Bootstrap_Files/plugins/iCheck/flat/blue.css">
    <link rel="stylesheet" href="../Bootstrap_Files/plugins/jvectormap/jquery-jvectormap-1.2.2.css">
    <link rel="stylesheet" href="../Bootstrap_Files/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css">
    <style>
        .alert-minimalist {
            background-color: rgb(241, 242, 240);
            border-color: rgba(149, 149, 149, 0.3);
            border-radius: 3px;
            color: rgb(149, 149, 149);
            padding: 10px;
        }

            .alert-minimalist > [data-notify="icon"] {
                height: 50px;
                margin-right: 12px;
            }

            .alert-minimalist > [data-notify="title"] {
                color: rgb(51, 51, 51);
                display: block;
                font-weight: bold;
                margin-bottom: 5px;
            }

            .alert-minimalist > [data-notify="message"] {
                font-size: 80%;
            }

        .flow {
            overflow: scroll;
            height: 300px;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Dashboard%>"></asp:Label>
        <small>
            <asp:Label ID="lbledit" runat="server" Text="Attendance" Visible="false"></asp:Label></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Home %>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label4" runat="server" Text="Attendance" Visible="false"></asp:Label></li>
    </ol>

    <div class="modal fade" id="modelViewDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <button type="button" id="" class="Close" data-dismiss="modal">
                    <span aria-hidden="true">&times;</span></button>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Leave_List" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					   <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Selected="True" Text="Emp_Code" Value="Emp_Code"></asp:ListItem>
                                                    <asp:ListItem Text="Emp_Name" Value="Emp_Name"></asp:ListItem>
                                                    <asp:ListItem Text="Dep_Name" Value="Dep_Name"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-lg-5">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box box-warning box-solid" <%= PresentView.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            Page Size :
                                                <asp:DropDownList ID="PresentPageNo" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>
                                            <br />
                                            <br />
                                            <div class="flow">

                                                <h3>Today Present</h3>
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" AutoPostBack="True" ID="PresentView" AllowPaging="true" OnPageIndexChanging="GvPresent_PageIndexChanging" PageSize="10" runat="server" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Emp_Code" SortExpression="Emp_Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label42" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp_Name" SortExpression="Emp_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label43" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dep_Name" SortExpression="Dep_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label44" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Designation" SortExpression="Designation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label45" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AttendanceLog(24 hrs)" SortExpression="AttendanceLog(24hrs)">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label46" runat="server" Text='<%# Eval("LastLog") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box box-warning box-solid" <%= AbsentData.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h3>Today Absent</h3>
                                            <br />
                                            Page Size :
                                                <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <br />
                                            <br />
                                            <div class="flow">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="AbsentData" OnPageIndexChanging="GvAbsent_PageIndexChanging" EnableSortingAndPagingCallbacks="false" runat="server" AllowPaging="true" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Emp_Code" SortExpression="Emp_Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label42" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp_Name" SortExpression="Emp_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label43" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dep_Name" SortExpression="Dep_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label44" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Designation" SortExpression="Designation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label45" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AttendanceLog(24 hrs)" SortExpression="AttendanceLog(24 hrs)">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label46" runat="server" Text='<%# Eval("LastLog") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>


                                                    </Columns>
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
    <div class="modal fade" id="modelViewDetail2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel ID="LeaveList2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div2" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                <asp:Label ID="Label47" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					   <asp:Label ID="TotalLeaves" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="Label48" runat="server" Visible="false"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I2" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName2" runat="server" CssClass="form-control">
                                                    <asp:ListItem Selected="True" Text="Emp_Code" Value="Emp_Code"></asp:ListItem>
                                                    <asp:ListItem Text="Emp_Name" Value="Emp_Name"></asp:ListItem>
                                                    <asp:ListItem Text="Dep_Name" Value="Dep_Name"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption2" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-lg-5">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue2" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="btnbind2" runat="server" CausesValidation="False" OnClick="btnbindLeave_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh2" runat="server" CausesValidation="False" OnClick="btnRefreshLeave_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box box-warning box-solid" <%= LeeaveGridView.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />

                                            Page Size :
                                                <asp:DropDownList ID="LeavePageNo" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged2" AutoPostBack="true"></asp:DropDownList>
                                            <br />
                                            <br />
                                            <div class="flow">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="LeeaveGridView" AllowPaging="true" OnPageIndexChanging="GvLeave_PageIndexChanging" PageSize="4" runat="server" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Emp_Code" SortExpression="Emp_Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Emp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp_Name" SortExpression="Emp_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Emp_Name" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dep_Name" SortExpression="Dep_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Dep_Name" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Designation" SortExpression="Designation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Designation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AttendanceLog(24 hrs)" SortExpression="AttendanceLog(24 hrs)">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LastLog" runat="server" Text='<%# Eval("LastLog") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
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


    <%--<div id="modelViewDetail" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">
                    &times;</button>
                <h4 class="modal-title">
                </h4>
            </div>
            <div class="modal-body">
            </div>
            <div class="modal-footer">
                <input type="button" id="btnClosePopup" value="Close" class="btn btn-danger" />
            </div>
        </div>
    </div>
</div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row" visible="false" runat="server">
        <div class="col-md-3 col-sm-6 col-xs-12">
            <div class="info-box">
                <span class="info-box-icon bg-aqua"><i class="fa fa-hand-o-up"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Present %>"></asp:Label></span>
                    <span class="info-box-number">
                        <asp:Literal ID="Ltr_Present" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12">
            <div class="info-box">
                <span class="info-box-icon bg-aqua"><i class="fa fa-thumbs-o-down"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Absent %>"></asp:Label></span>
                    <span class="info-box-number">
                        <asp:Literal ID="Ltr_Absent" runat="server"></asp:Literal>
                    </span>
                    <span class="info-box-text">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Leave %>"></asp:Label></span>
                    <span class="info-box-number">
                        <asp:Literal ID="Ltr_Leave" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12">
            <div class="info-box">
                <span class="info-box-icon bg-aqua"><i class="fa fa-calendar-times-o"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Week Off %>"></asp:Label></span>
                    <span class="info-box-number">
                        <asp:Literal ID="Ltr_Week_Off" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12">
            <div class="info-box">
                <span class="info-box-icon bg-aqua"><i class="fa  fa-calendar-minus-o"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Holiday %>"></asp:Label></span>
                    <span class="info-box-number">
                        <asp:Literal ID="Ltr_Holiday" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <div class="row">


        <div id="OT_Manipulation" runat="server" class="col-md-3">
            <a href="../attendance/ot_manipulation.aspx" class="btn btn-primary">
                <asp:Label ID="label28" runat="server" Text="<%$ resources:attendance,predefined shift & ot entry screen%> "></asp:Label>
            </a>
        </div>



        <div id="Short_Leave_Request" runat="server" class="col-md-3">
            <a href="../Attendance/Short_Leave_Request.aspx" class="btn btn-primary">
                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Short Leave Request and Approval%>"></asp:Label>
            </a>
        </div>

        <div id="AutoShortLeave" runat="server" class="col-md-3">
            <a href="../Attendance/AutoShortLeave.aspx" class="btn btn-primary">
                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Auto Short Leave%>"></asp:Label>
            </a>
        </div>

        <div id="TimeCard" runat="server" class="col-md-3">
            <a href="../Attendance_Report/TimeCard.aspx" class="btn btn-primary">
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Time Card Report%>"></asp:Label>
            </a>
        </div>


    </div>

    <br />

    <div class="row">

        <div id="OT_Request" runat="server" class="col-md-3">
            <a href="../Attendance/OT_Request.aspx" class="btn btn-primary">
                <asp:Label ID="LabelL1" runat="server" Text="<%$ Resources:Attendance,OT/Absent Request and Approval%>"></asp:Label>
            </a>
        </div>

    </div>

    <br />


    <div id="TL_Leave" visible="false" runat="server" class="row">
        <div class="col-md-6 col-sm-6 col-xs-12">
            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Literal ID="Ltr_Present_Team" runat="server"></asp:Literal>
                    </h3>
                    <p>
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Team Present%>"></asp:Label>
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-person-add"></i>

                </div>
                <div runat="server" id="PresentData" align="center">

                    <a href="#" class="small-box-footer" style="color: white;" onclick="Modal_ViewInfo_Open();">More info <i class="fa fa-arrow-circle-right"></i></a>

                </div>

            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-12">
            <div class="small-box bg-red">
                <div class="inner">
                    <h3>
                        <asp:Literal ID="Ltr_Leave_Team" runat="server"></asp:Literal>
                    </h3>
                    <p>
                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Team Leave %>"></asp:Label>
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-pie-graph"></i>

                </div>
                <div runat="server" id="LeaveData" align="center">

                    <a href="#" class="small-box-footer" style="color: white;" onclick="Modal_LeaveInfo_Open();">More info <i class="fa fa-arrow-circle-right"></i></a>
                </div>


            </div>
        </div>
    </div>
    <div class="row" id="TL_Dashbaord" visible="false" runat="server">
        <section class="col-lg-6 connectedSortable">


            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Leave Status %>"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12" style="overflow: auto; max-height: 250px">
                            <asp:Repeater ID="Rpt_Leave_Status" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="Ltr_Leave_Name" runat="server" Text='<%# Eval("Leave_Status") %>'></asp:Literal>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <%------------------------Task List-----------------------%>
            <div class="box box-primary" runat="server" id="Div_Duty_List">
                <div class="box-header with-border">
                    <i class="ion ion-clipboard"></i>
                    <h3 class="box-title">
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Duty List%>"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        <%--<ul class="pagination pagination-sm inline">
                            <li><a href="#">&laquo;</a></li>
                            <li><a href="#">1</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">3</a></li>
                            <li><a href="#">&raquo;</a></li>
                        </ul>--%>
                    </div>
                </div>
                <div class="box-body" style="overflow: auto; max-height: 300px;">
                    <ul class="todo-list">
                        <asp:Literal ID="Ltr_Duty" runat="server"></asp:Literal>
                    </ul>
                </div>
                <%--<div class="box-footer clearfix no-border">
                    <button type="button" class="btn btn-default pull-right"><i class="fa fa-plus"></i>Add item</button>
                </div>--%>
            </div>
            <%------------------------End Task List-----------------------%>
        </section>
        <section class="col-lg-6 connectedSortable">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Log Details%>"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body no-padding">
                    <div style="overflow: auto; max-height: 270px;">
                        <div class="table-responsive">
                            <asp:Repeater ID="Rpt_Log" runat="server">
                                <HeaderTemplate>
                                    <table class="table no-margin">
                                        <tr>
                                            <th>
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Log Time %>"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Date %>"></asp:Label></th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLog_Time" runat="server" Text='<%# Eval("Log_Time") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>


            <%------------------------Calendar-----------------------%>
            <%--<div style="display: none;" class="box box-primary">
                <div class="box-header">
                    <i class="fa fa-calendar"></i>
                    <h3 class="box-title">Calendar</h3>
                    <div class="pull-right box-tools">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body no-padding">
                    <div id="calendar" style="width: 100%"></div>
                </div>
                <div class="box-footer text-black">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="clearfix">
                                <span class="pull-left">Holiday</span>
                                <small class="pull-right">10%</small>
                            </div>
                            <div class="progress xs">
                                <div class="progress-bar progress-bar-red" style="width: 90%;"></div>
                            </div>
                            <div class="clearfix">
                                <span class="pull-left">Present</span>
                                <small class="pull-right">80%</small>
                            </div>
                            <div class="progress xs">
                                <div class="progress-bar progress-bar-green" style="width: 70%;"></div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="clearfix">
                                <span class="pull-left">Leave</span>
                                <small class="pull-right">20%</small>
                            </div>
                            <div class="progress xs">
                                <div class="progress-bar progress-bar-blue" style="width: 60%;"></div>
                            </div>
                            <div class="clearfix">
                                <span class="pull-left">Absent</span>
                                <small class="pull-right">40%</small>
                            </div>
                            <div class="progress xs">
                                <div class="progress-bar progress-bar-yellow" style="width: 40%;"></div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="clearfix">
                                <span class="pull-left">Weekday</span>
                                <small class="pull-right">20%</small>
                            </div>
                            <div class="progress xs">
                                <div class="progress-bar progress-bar-aqua" style="width: 60%;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <%------------------------End Calendar-----------------------%>

            <%------------------------Task List-----------------------%>
            <div class="box box-primary" runat="server" id="Div_Task_List">
                <div class="box-header with-border">
                    <i class="ion ion-clipboard"></i>
                    <h3 class="box-title">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Task List %>"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        <%--<ul class="pagination pagination-sm inline">
                            <li><a href="#">&laquo;</a></li>
                            <li><a href="#">1</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">3</a></li>
                            <li><a href="#">&raquo;</a></li>
                        </ul>--%>
                    </div>
                </div>
                <div class="box-body" style="overflow: auto; max-height: 300px;">
                    <ul class="todo-list">
                        <asp:Literal ID="Ltr_Task" runat="server"></asp:Literal>
                    </ul>
                </div>
                <%--<div class="box-footer clearfix no-border">
                    <button type="button" class="btn btn-default pull-right"><i class="fa fa-plus"></i>Add item</button>
                </div>--%>
            </div>
            <%------------------------End Task List-----------------------%>
        </section>



        <section class="col-lg-6 connectedSortable">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="Label32" runat="server" Text="Pending Short Leave Request"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body no-padding">
                    <div style="overflow: auto; max-height: 270px;">
                        <div class="table-responsive">
                            <asp:Repeater ID="rpt_short_leave" runat="server">
                                <HeaderTemplate>
                                    <table class="table no-margin">
                                        <tr>
                                            <th>
                                                <asp:Label ID="Label3" runat="server" Text="Date"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label15" runat="server" Text="From Time"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label16" runat="server" Text="To Time"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label40" runat="server" Text="Duration"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label17" runat="server" Text="Method"></asp:Label></th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSLDate" runat="server" Text='<%# Eval("Date") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSLInTime" runat="server" Text='<%# Eval("InTime") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSLOutTime" runat="server" Text='<%# Eval("OutTime") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label41" runat="server" Text='<%# Eval("Duration") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSLMethod" runat="server" Text='<%# Eval("Type") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>





        </section>

        <section class="col-lg-12 connectedSortable">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="Label33" runat="server" Text="Employee Short Leaves Status"></asp:Label></h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body no-padding">
                    <div style="overflow: auto; max-height: 270px;">
                        <div class="table-responsive">
                            <asp:Repeater ID="rpt_applied_short_leave" runat="server">
                                <HeaderTemplate>
                                    <table class="table no-margin">
                                        <tr>
                                            <th>
                                                <asp:Label ID="Label3" runat="server" Text="Document No"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label15" runat="server" Text="Request Date"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label16" runat="server" Text="Partial Leave Date"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label17" runat="server" Text="Method"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label34" runat="server" Text="Type"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label35" runat="server" Text="FromTime"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label36" runat="server" Text="ToTime"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label37" runat="server" Text="Duration"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label38" runat="server" Text="Trasn Type"></asp:Label></th>
                                            <th>
                                                <asp:Label ID="Label39" runat="server" Text="Status"></asp:Label></th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTrans_Id" runat="server" Text='<%# Eval("Trans_Id") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRequest_Date" runat="server" Text='<%# Convert.ToDateTime(Eval("Request_Date")).ToString("dd-MMM-yyyy") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPartial_Leave_Date" runat="server" Text='<%# Convert.ToDateTime(Eval("Partial_Leave_Date")).ToString("dd-MMM-yyyy")  %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMethod" runat="server" Text='<%# Eval("Method") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPartialType" runat="server" Text='<%# Eval("PartialType") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFrom_Time" runat="server" Text='<%# Eval("From_Time") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTo_Time" runat="server" Text='<%# Eval("To_Time") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblduration" runat="server" Text='<%# Eval("duration") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransType" runat="server" Text='<%# Eval("TransType") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIs_Confirmed" runat="server" Text='<%# Eval("Is_Confirmed") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>





        </section>

        <div class="col-md-12" runat="server" id="Div_Reminder_List">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-primary">
                        <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Reminder List%>"></asp:Label></h3>
                            <div class="box-tools">
                                <div class="input-group input-group-sm">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="box-body table-responsive no-padding">
                            <div style="overflow: auto; max-height: 270px;">
                                <ul class="todo-list">
                                    <asp:Literal ID="Lit_Reminder" runat="server"></asp:Literal>
                                </ul>
                                <%--<asp:ListView runat="server" ID="listReminder">

                                    <LayoutTemplate>
                                        <table runat="server" id="table1">
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>

                                        <tr runat="server" style="height: 20px">
                                            <td runat="server" style="padding-left: 10px; border-spacing: 5px;">
                                                <asp:LinkButton ForeColor="Black" runat="server" ID="lblReminderText" Text='<% #Eval("Reminder_text") %>' PostBackUrl='<% #Eval("Target_url") %>'></asp:LinkButton>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:ListView>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="col-md-12">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-primary">
                        <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Team Leave Request Status%>"></asp:Label></h3>
                            <div class="box-tools">
                                <div class="input-group input-group-sm">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="box-body table-responsive no-padding">
                            <div style="overflow: auto; max-height: 270px;">
                                <asp:Repeater ID="Rpt_Leave_Request_Status" runat="server">
                                    <HeaderTemplate>
                                        <table class="table no-margin">
                                            <tr>
                                                <th>
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Emp Code %>"></asp:Label></th>
                                                <th>
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label></th>
                                                <th>
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Leave Type %>"></asp:Label></th>
                                                <th>
                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label></th>
                                                <th>
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label></th>
                                                <th style="min-width: 100px; max-width: 100px;">
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Reason %>"></asp:Label></th>
                                                <th>
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label></th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEmpID" runat="server" Text='<%# Eval("EmpCode") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUser" runat="server" Text='<%# Eval("EmpName") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLeave_Type" runat="server" Text='<%# Eval("Leave_Type") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLSDate" runat="server" Text='<%# Eval("FromDate") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ToDate") %>' />
                                            </td>
                                            <td style="min-width: 100px; max-width: 250px;">
                                                <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>' />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hdnLeaveStatus" runat="server" Value='<%# Eval("StatusOriginal") %>' />
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false" />
                                                <asp:HyperLink ID="hypApprovalPage" Target="_blank" Visible="false"
                                                     runat="server" Text='<%# Eval("Status") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label27" runat="server" Text="Upcoming Birthdays"></asp:Label></h3>
                            <div class="box-tools pull-right">
                                <span class="label label-danger">
                                    <asp:Literal ID="Ltr_Bday_Count" runat="server"></asp:Literal>
                                </span>
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body no-padding">
                            <div style="overflow: auto; max-height: 270px;">
                                <ul class="users-list clearfix">
                                    <asp:Repeater ID="Rpt_Birthday" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="Ltr_Birthday" runat="server" Text='<%# Eval("Birthday") %>'></asp:Literal>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
	<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
	<span data-notify="icon"></span>
	<span data-notify="title">{1}</span>
	<span data-notify="message">{2}</span>
	<div class="progress" data-notify="progressbar">
		<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>
	</div>
	<a href="{3}" target="{4}" data-notify="url"></a>
</div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Bootstrap_Files/Additional/bootstrap-notify.js"></script>
    <script src="../Bootstrap_Files/Additional/bootstrap-notify.min.js"></script>
    <script>
        function Modal_ViewInfo_Open() {
            $('#modelViewDetail').modal('show');
            //$('#modelViewDetail').modal('show');
        }
        function Modal_LeaveInfo_Open() {

            $('#modelViewDetail2').modal('show');
        }
        //$(document).ready(function ()
        //{
        //    debugger;
        //    var TestVar = $('#ShowPopup').val();
        //    if (TestVar == "True") {
        //        $('#modelViewDetail').modal('show');
        //    }           

        //});
    </script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript">
    $(function () {       
     $('#<%= LeeaveGridView.ClientID%>').DataTable(
        {
            
            lengthMenu: [[5, 10, -1], [5, 10, "All"]],
           
        });
    });
</script>--%>
</asp:Content>
