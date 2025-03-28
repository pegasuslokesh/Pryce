<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true" CodeFile="ProjectTaskFeedback.aspx.cs" Inherits="ProjectManagement_ProjectTaskFeedback" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/TaskFeedback.ascx" TagPrefix="TF1" TagName="TaskFeedback1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/ProjectBugs.ascx" TagPrefix="Bug" TagName="ProjectBugs1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .GridStyle {
            border: 1px solid rgb(217, 231, 255);
            background-color: White;
            font-family: arial;
            font-size: 12px;
            border-collapse: collapse;
            margin-bottom: 0px;
        }

            .GridStyle tr {
                border: 1px solid rgb(217, 231, 255);
                color: Black;
                height: 25px;
            }
            /* Your grid header column style */
            .GridStyle th {
                background-color: rgb(217, 231, 255);
                border: none;
                text-align: left;
                font-weight: bold;
                font-size: 15px;
                padding: 4px;
                color: Black;
                text-align: center;
            }
            /* Your grid header link style */
            .GridStyle tr th a, .GridStyle tr th a:visited {
                color: Black;
            }

            .GridStyle tr th, .GridStyle tr td table tr td {
                border: none;
            }

            .GridStyle td {
                border-bottom: 1px solid rgb(217, 231, 255);
                padding: 2px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-thumbs-up"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Project Task Feedback"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Project task feedback%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnProjectId" runat="server" />
            <asp:HiddenField ID="hdnUserId" runat="server" />
            <asp:HiddenField runat="server" ID="hdnTaskStarted" />
            <asp:HiddenField ID="hdnRPTClick" runat="server" />
            <asp:HiddenField ID="hdnCurrentPageIndex" runat="server" />
            <asp:HiddenField ID="hdnReportTaskId" runat="server" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="btn_modal_allWorkingEmployee" Style="display: none;" runat="server" data-toggle="modal" data-target="#AllWorkingTask" />
            <asp:Button ID="Btn_TaskContractReport" Style="display: none;" runat="server" data-toggle="modal" data-target="#TaskContractReport" Text="ContractReport" />
            <asp:Button ID="Btn_TaskFeedback" Style="display: none;" runat="server" data-toggle="modal" data-target="#TaskFeedback" />
            <asp:Button ID="btn_taskHistory" Style="display: none;" runat="server" data-toggle="modal" data-target="#EmpHistory" />
            <asp:HiddenField ID="hdnCanEdit" runat="server" Value="false" />
            <asp:HiddenField ID="hdnCanPrint" runat="server" Value="false" />
            <asp:HiddenField ID="hdnCanUpload" runat="server" Value="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:FileUpload1 runat="server" ID="FUpload1" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="TaskFeedback" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-6">
                        <h4>Stop Task</h4>
                    </div>
                    <div class="col-md-6">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    </div>
                </div>
                <div class="modal-body">
                    <TF1:TaskFeedback1 runat="server" ID="taskFeedback" />
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="BugsData" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-6">
                        <h4>New Bug</h4>
                    </div>
                    <div class="col-md-6">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    </div>
                </div>
                <div class="modal-body">
                    <Bug:ProjectBugs1 runat="server" ID="bugData" />
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="AllWorkingTask" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <h3>All Working Employee Tasks</h3>
                    <br />

                    <table id="tblCurrentActivities" class="table-striped table-bordered table table-hover" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Project Name</th>
                                <th>Task Name</th>
                                <th>Employee</th>
                                <th>Start Time</th>
                                <th>Working Hrs</th>
                                <th>Actual Cost/Expected Cost</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>


                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="EmpHistory" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-6">
                        <h3>Employee Task History</h3>
                    </div>
                    <div class="col-md-6">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    </div>
                </div>

                <div class="col-md-4">
                    <asp:Label ID="lblDate" runat="server" Text="Task Date"></asp:Label>
                    <asp:TextBox ID="txtTaskDate" runat="server" onkeydown='return (event.keyCode!=13);' CssClass="form-control"></asp:TextBox>
                    <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtTaskDate">
                    </cc1:CalendarExtender>
                    <br />
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label15" runat="server" Text="Assign To"></asp:Label>
                    <asp:TextBox runat="server" ID="txtAssignTo" CssClass="form-control" BackColor="#eeeeee" onchange="validateEmployee(this)"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetCompletionListAssignBy" ServicePath="" CompletionInterval="100"
                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAssignTo"
                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition2">
                    </cc1:AutoCompleteExtender>
                    <br />
                </div>
                <div class="col-md-4">
                    <br />
                    <asp:Button runat="server" ID="Button1" Text="Get Task Details" CssClass="btn btn-primary" OnClientClick="btnTaskDetails_Click(this);return false;" />
                    <br />
                </div>
                <div class="col-md-12" style="overflow: auto; max-height: 300px">
                    <table id="tblEmpTaskHistory" class="table-striped  table table-hover" style="width: 100%">
                        <thead>
                            <tr>
                                <td></td>
                                <td>Project Name</td>
                                <td>Task Name</td>
                                <td>Employee</td>
                                <td>Start Time</td>
                                <td>Stop Time</td>
                                <td>Working Hrs</td>
                                <td>Actual Cost/Expected Cost</td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>

                <div class="col-md-6">
                    <br />
                    <asp:Label runat="server" ID="lblProjectName"></asp:Label>
                    <br />
                </div>
                <div class="col-md-6">
                    <br />
                    <asp:Label runat="server" ID="lblTaskName"></asp:Label>
                    <br />
                </div>

                <div class="col-md-12">
                    <table id="tblEmpLogHistory" class="table-striped table-bordered table table-hover" style="width: 100%">
                        <thead>
                            <tr>
                                <td>Feedback</td>
                                <td>Start Date & Time</td>
                                <td>Stop Date & Time</td>
                                <td>Working Hrs</td>
                                <td>Commented By</td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>


                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="gvrstatus" />
        </Triggers>
        <ContentTemplate>
            <div id="pnlfield" runat="server" class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6">

                                        <asp:TextBox ID="txtprojecttype" runat="server" placeholder="<%$ Resources:Attendance,Project Type %>" CssClass="form-control" OnTextChanged="txtprojecttype_TextChanged"
                                            BackColor="#eeeeee" AutoPostBack="true"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetProjectType" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtprojecttype"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtProjectName" runat="server" placeholder="<%$ Resources:Attendance,Project Name %>" Class="form-control" BackColor="#eeeeee" onkeydown='return (event.keyCode!=13);' onchange="txtProjectName_TextChanged(this)" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListProject" ServicePath=""
                                            TargetControlID="txtProjectName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEndFromDate" runat="server" placeholder="Expected End From date" onkeydown='return (event.keyCode!=13);' CssClass="form-control"></asp:TextBox>
                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtEndFromDate">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEndToDate" runat="server" onkeydown='return (event.keyCode!=13);' placeholder="Expected End To date" CssClass="form-control"></asp:TextBox>
                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtEndToDate">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>

                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Both" Value="All"></asp:ListItem>
                                            <asp:ListItem Text="Bugs" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="Task" Value="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>

                                    <div class="col-lg-2">
                                        <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                            <asp:ListItem Text="Assigned" Value="Assigned" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Extended" Value="Extended"></asp:ListItem>
                                            <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                            <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                            <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Project" Value="Project_Name" />
                                            <asp:ListItem Text="Task" Value="Subject" Selected="True" />
                                            <asp:ListItem Text="Assigned To" Value="AssignTo" />
                                            <asp:ListItem Text="Assigned By" Value="Assignbyname" />
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-lg-6">
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnRefresh">
                                            <asp:TextBox ID="txtValue" runat="server" placeholder="Search from Content" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </asp:Panel>
                                    </div>

                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" Visible="true" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                        <asp:Button ID="btnRefresh" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Refresh %>" Visible="true" CssClass="btn btn-primary" OnClick="btnRefresh_Click" />
                                        <asp:Button ID="btnEmployeeWorking" runat="server" CausesValidation="False" Text="Current Activities" CssClass="btn btn-primary" OnClientClick="currentActivitiesClick();return false;" />
                                        <asp:Button ID="btnTaskHistory" runat="server" CausesValidation="False" Text="Employees Task History" CssClass="btn btn-primary" OnClientClick="openTaskHistory();return false;" />
                                        <asp:LinkButton runat="server" ID="lnkCurrentWorkingTask" OnCommand="lnkCurrentWorkingTask_Command" CssClass="btn btn-primary"></asp:LinkButton>
                                        <br />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btngo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkCurrentWorkingTask" />

        </Triggers>
        <ContentTemplate>
            <div id="pnllist" runat="server">
                <div class="box box-warning box-solid" <%= gvrstatus.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                    <div class="box-body">
                        <div class="row">



                            <div class="col-md-12">
                                <div style="overflow: auto">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvrstatus" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <RowStyle Font-Size="Small" />
                                        <HeaderStyle Font-Size="Small" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>

                                                    <div class="dropdown" onclick="validateControls(this)" style="position: absolute;">
                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                        </button>
                                                        <ul class="dropdown-menu">
                                                            <li>
                                                                <asp:LinkButton ID="btnPrint" runat="server" CommandArgument='<%# Eval("Task_Id") %>' CommandName='<%# Eval("Project_Id") %>' OnCommand="btnPrint_Command" CausesValidation="False"><i class="fa fa-print"></i> Print</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Task_Id")+"/"+Eval("assignbyId") %>' CommandName='<%# Eval("feedbackTrans_id")+"/"+Eval("workStatus")  %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-comment"></i> Comment</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Project_Id")+"/"+Eval("field7") %>' CommandName='<%# Eval("Subject") %>' CausesValidation="False" OnCommand="btnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <a style="cursor: pointer;" ondblclick="openTaskDetails('<%# Formatdate(Eval("Assign_Date")) %>','<%# Formatdate(Eval("Emp_Close_Date")) %>','<%# Eval("AssignTo") %>')"><i class="fa fa-desktop"></i>Show Details</a>
                                                            </li>
                                                        </ul>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a id="aStartNStop" style="cursor: pointer" title='<%# Eval("workStatus").ToString()=="Stop"?"Stop":"Start" %>' onclick="startStopTask('<%# Container.DataItemIndex + 1 %>','<%# Eval("project_id") %>','<%#Eval("feedbackTrans_id")%>','<%#Eval("Task_Id")%>','<%#Eval("AssignToId")%>','<%#Eval("Status")%>')"><i style="font-size: 18px" id="imgToggle" class='<%# Eval("workStatus").ToString()=="Stop"?"fa fa-toggle-on":"fa fa-toggle-off" %>'></i></a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblproject" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="gvHdnProjectID" Value='<%# Eval("Project_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Task">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprojectIdList41" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                    <asp:Label ID="lbltskId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Working Hrs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalHr" ClientIDMode="Static" runat="server" Text='<%# Eval("totalWorkingHr")+"/"+Eval("requiredhours") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="88px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprojectIdassignedBy" runat="server" Text='<%# Eval("Assignbyname") %>'></asp:Label>
                                                    <asp:Label ID="lblAssignDate" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblExpectedEndDate" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Overdue Days">
                                                <ItemTemplate>
                                                    <asp:Label ID="lvloverdue" runat="server" Text='<%# Eval("Overduedays") %>'></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assign To">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnAssignToId" runat="server" Value='<%# Eval("AssignToId") %>'></asp:HiddenField>
                                                    <asp:Label ID="lblprojectIdList2" runat="server" Text='<%# Eval("AssignTo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Completed%">
                                                <ItemTemplate>
                                                    <span id="lblPercentage"><%# Eval("Field51") %></span>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                    <asp:HiddenField ID="hdntaskid" runat="server" />
                                    <asp:HiddenField ID="HiddeniD" runat="server" />
                                    <asp:HiddenField ID="hidcustID1" runat="server" />
                                    <asp:Repeater ID="rptPager" runat="server">
                                        <ItemTemplate>
                                            <ul class="pagination">
                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' CssClass="page-link" OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvrstatus" />
            <asp:AsyncPostBackTrigger ControlID="grvcomment" />
            <asp:AsyncPostBackTrigger ControlID="txtProjectName" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />

            <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div id="pnlstustrecord" runat="server" class="row">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <div class="box box-primary collapsed-box">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <asp:Label ID="Label18" runat="server" Font-Bold="true" Text="Project"></asp:Label>
                                        &nbsp : &nbsp
                                            <asp:Label ID="lblidisproname" runat="server"></asp:Label></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Project Manager"></asp:Label>
                                            &nbsp : &nbsp
                                                    <asp:Label ID="lblProjectManagerValue" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Start Date"></asp:Label>
                                            &nbsp : &nbsp
                                                    <asp:Label ID="lblPrjStartDate" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="Due Date"></asp:Label>
                                            &nbsp : &nbsp
                                                    <asp:Label ID="lblPrjExpEndate" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnIsTaskStart" />

                        <div class="col-md-12">
                            <div class="box box-primary collapsed-box">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <asp:Label ID="Label17" runat="server" Font-Bold="true" Text="Task"></asp:Label>
                                        &nbsp : &nbsp
                                            <asp:Label ID="lbldissubject" runat="server"></asp:Label></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; overflow: auto; max-height: 100px;">
                                                <asp:Literal ID="lbldisdescription" runat="server"></asp:Literal>
                                            </asp:Panel>
                                            <asp:HiddenField ID="HidCustId" runat="server" />
                                            <asp:Label ID="lblTaskId" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                                            <hr />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Assign By %>"></asp:Label>
                                            &nbsp :&nbsp
                                            <asp:Label ID="lblAssignBy" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblAssigndt" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Assign Date %>"></asp:Label>
                                            &nbsp :&nbsp
                                            <asp:Label ID="lbldisassigndate" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Expected Date"></asp:Label>
                                            &nbsp :&nbsp
                                            <asp:Label ID="lblEmpCloseDate" runat="server"></asp:Label>
                                        </div>

                                        <div class="col-md-4">
                                            <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="Required Time"></asp:Label>
                                            &nbsp :&nbsp
                                            <asp:Label ID="lblRequiredTime" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label1ww" runat="server" Font-Bold="true" Text="Total Time"></asp:Label>
                                            &nbsp :&nbsp
                                            <asp:Label ID="lblTaskDuration" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div id="Div_General_Info" runat="server" class="box box-primary collapsed-box">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Previous Conversation</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i id="Btn_Div_General_Info" runat="server" class="fa fa-plus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto; max-height: 500px;">
                                                <asp:UpdatePanel runat="server" ID="upComments" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvrstatus" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="grvcomment" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="grvcomment_PageIndexChanging" OnSorting="grvcomment_Sorting">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnEditcomment" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEditcomment_Command" CausesValidation="False" Visible='<%# Eval("CommentBy").ToString()==Session["EmpId"].ToString()?true:false %>' ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="IbtnDeletecomment" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDeletecomment_Command" Visible='<%# Eval("CommentBy").ToString()==Session["EmpId"].ToString()?true:false %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                            TargetControlID="IbtnDeletecomment">
                                                                        </cc1:ConfirmButtonExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblprojectIdList2" runat="server" Text='<%# GetEmployeeName(Eval("CommentBy")) %>'></asp:Label>
                                                                        <asp:Label ID="lblCommentsby" runat="server" Text='<%# Eval("CommentBy") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblprojectIdList3" runat="server" Text='<%# Eval("DateTime").ToString()==""?"": Convert.ToDateTime(Eval("DateTime").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Start Time">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("field2").ToString().Trim() == "" ? "" :  getCountryTimeFormatStatic(Eval("field2").ToString(),Eval("timeZoneID").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Stop Time">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStopTime" runat="server" Text='<%# Eval("field3").ToString().Trim() == "" ? "" : getCountryTimeFormatStatic(Eval("field3").ToString(),Eval("timeZoneID").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdntask" runat="server" Value='<%# Eval("Task_Id") %>' />
                                                                        <asp:HiddenField ID="HiddeniD" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                        <asp:Label ID="lblprojectIdList1" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Feedback</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <%--<cc2:Editor ID="Editor1" runat="server" CssClass="form-control" />--%>
                                            <asp:TextBox runat="server" ID="Editor1" TextMode="MultiLine" Width="100%" Style="max-height: 300px; min-height: 80px"></asp:TextBox>
                                            <asp:Label ID="lblNA" Visible="false" runat="server" Text="NA"></asp:Label>
                                            <asp:HiddenField ID="hdnEditId" runat="server" />
                                            <br />
                                        </div>
                                        <div class="col-md-6" style="margin-top: 20px;">
                                            <asp:CheckBox ID="chkCloseTask" Visible="false" CssClass="form-control" Font-Size="16px" ForeColor="Blue" Text="Closed Task" onchange="ShowClosedExtraFields()" runat="server" />
                                            <br />
                                        </div>

                                        <div class="col-md-6" id="ddlTaskCompletion_div" runat="server" contenteditable="false" enableviewstate="true">
                                            <div class="col-md-6">


                                                <asp:Label ID="lblTaskFeedbackDate" runat="server" Text="Task Feedback Date"></asp:Label>
                                                <asp:TextBox ID="txtTaskFeedbackDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server"
                                                    Enabled="True" TargetControlID="txtTaskFeedbackDate">
                                                </cc1:CalendarExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">

                                                <asp:Label ID="lblTaskCompletion" runat="server" Text="Task Completion (%)"></asp:Label>
                                                <asp:DropDownList ID="ddlTaskCompletion" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                    <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                                    <asp:ListItem Text="70" Value="70"></asp:ListItem>
                                                    <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                                    <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                        </div>

                                        <div class="col-md-12"></div>
                                        <div id="trTeamLeader" style="display: none;" runat="server" contenteditable="false" enableviewstate="true">
                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="lblResult" Text="Result"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlResult" CssClass="form-control">
                                                    <asp:ListItem Value="Closed" Text="Pass"></asp:ListItem>
                                                    <asp:ListItem Value="Failed" Text="Failed"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblEndDate" runat="server" Text="Task End Date"></asp:Label>
                                                <asp:TextBox ID="txttlenddate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                                    Enabled="True" TargetControlID="txttlenddate">
                                                </cc1:CalendarExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblEndTime" runat="server" Text="Task End Time"></asp:Label>
                                                <asp:TextBox ID="txttlendtime" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                    Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txttlendtime">
                                                </cc1:MaskedEditExtender>
                                                <br />
                                            </div>

                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="lblFinalCost" Text="Final Cost"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFinalCost" Enabled="false" CssClass="form-control" onchange="validateAmt(this)"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="lblLoss" Text="Loss"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtLoss" CssClass="form-control" onchange="validateAmt(this)"></asp:TextBox>
                                                <br />
                                            </div>

                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lblRemarks" Text="Remarks"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRemark" CssClass="form-control" TextMode="MultiLine" Style="resize: vertical; min-height: 60px; max-height: 150px; height: 60px;"></asp:TextBox>
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Button ID="btnsave" runat="server" Visible="false" CssClass="btn btn-success" OnClick="btnsave_Click" Text="Save" />
                                            <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click" Text="Reset" />
                                            <asp:Button ID="btncencel" runat="server" Text="Cancel" OnClick="btncencel_Click" CssClass="btn btn-danger" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="TaskContractReport" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="resetReportField()">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="uptaskContrcat" runat="server">
                        <ContentTemplate>
                            <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal" onclick="resetReportField()">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="TaskDetails" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>Task Details : 
                    </h3>
                </div>
                <div class="modal-body">
                    <div class="col-md-6">
                        <b>Asign Date </b>
                        <br />
                        <span id="AssignDate"></span>
                    </div>
                    <div class="col-md-6">
                        <b>Expected End Date</b><br />
                        <span id="ExpEndDate"></span>
                    </div>
                    <div class="col-md-12">
                        <br />
                    </div>
                    <div class="col-md-12">
                        <b>Assign To</b>
                        <br />
                        <span id="AssignTo"></span>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal" onclick="resetReportField()">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upComments">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
       
        function resetPosition2(object, args) {
            $(object._completionListElement.children).each(function(){                
                var data = $(this)[0];
                if(data!=null)
                {
                    data.style.paddingLeft="10px";
                    data.style.cursor="pointer";
                    data.style.borderBottom="solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className="autoCompleteList scrollbar scrollbar-primary force-overflow";
            var tbposition = findPositionWithScrolling(100004);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;

            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
      
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100004;
        }

        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function Modal_Open_WorkingEmployee() {
            document.getElementById('<%= btn_modal_allWorkingEmployee.ClientID %>').click();
        }


        function validateAmt(id) {
            if (isNaN(id.value)) {
                showAlert('Please Enter A Valid Number','orange','white');
                id.value = "";
            }
        }

        function ShowClosedExtraFields() {
        
            if (document.getElementById('<%=chkCloseTask.ClientID%>').checked) {
                $('#<%= trTeamLeader.ClientID%>').show();
                $('#<%= ddlTaskCompletion_div.ClientID%>').hide();
                var endDt = document.getElementById('<%= txttlenddate.ClientID%>');

                var today = new Date();

                var dd = today.getDate();
                var mm = today.getMonth() + 1;
                var yyyy = today.getFullYear();
                var hr = today.getHours();
                var min = today.getMinutes();

                if (min == '1' || min == '2' || min == '3' || min == '4' || min == '5' || min == '6' || min == '7' || min == '8' || min == '9') {
                    min = '0' + min;
                }

                if (dd < 10) {
                    dd = '0' + dd;
                }

                var mmm;
                if (mm == '1') {
                    mmm = 'Jan';
                } else if (mm == '2') {
                    mmm = 'Feb';
                } else if (mm == '3') {
                    mmm = 'Mar';
                } else if (mm == '4') {
                    mmm = 'Apr';
                } else if (mm == '5') {
                    mmm = 'May';
                } else if (mm == '6') {
                    mmm = 'Jun';
                } else if (mm == '7') {
                    mmm = 'Jul';
                } else if (mm == '8') {
                    mmm = 'Aug';
                } else if (mm == '9') {
                    mmm = 'Sep';
                } else if (mm == '10') {
                    mmm = 'Oct';
                } else if (mm == '11') {
                    mmm = 'Nov';
                } else if (mm == '12') {
                    mmm = 'Dec';
                }
                var today = dd + '-' + mmm + '-' + yyyy;



                var time = hr + ':' + min;

                document.getElementById('<%= txttlenddate.ClientID%>').value = today;
                document.getElementById('<%= txttlendtime.ClientID%>').value = time;

            }
            else {
                $('#<%= trTeamLeader.ClientID%>').hide();
                $('#<%= ddlTaskCompletion_div.ClientID%>').show();
                //document.getElementById('trTeamLeader').style = "display:none";
                //document.getElementById('ddlTaskCompletion_div').style = "display:block";
            }

            <%--if (document.getElementById('<%=chkCloseTask.ClientID%>').checked) {
                var stop = document.getElementById('<%= div_stop.ClientID%>');
                if (stop == null) {
                    return true;
                } else {
                    return false;
                }
            }
            else {
                return false;
            }--%>
        }

        function ShowClosedExtraFields_onEdit() {
            if (document.getElementById('<%=chkCloseTask.ClientID%>').checked) {
                $('#<%= trTeamLeader.ClientID%>').show();
                $('#<%= ddlTaskCompletion_div.ClientID%>').hide();
            }
            else {
                $('#<%= trTeamLeader.ClientID%>').hide();
                $('#<%= ddlTaskCompletion_div.ClientID%>').show();
            }

            //document.getElementById('trTeamLeader').style = "display:block";
            //document.getElementById('ddlTaskCompletion_div').style = "display:none";
        }

        function setTaskStart() {
            var isTaskStart = document.getElementById('<%= hdnIsTaskStart.ClientID%>');
            var asd = isTaskStart.value;

            if (isTaskStart.value == "false") {
                isTaskStart.value = "true";
                showAlert('Task Started','green','white')
                return true;
            }
            else {
                showAlert('Please stop the started task','orange','white')
                
                return false;
            }
        }
        function getProjectId(ctrl) {
            $("#prgBar").css("display", "block");
            PageMethods.getProjectId(ctrl.value, onsuccessProjectId, onerror);
        }

        function onsuccessProjectId(data) {
            var projectId = document.getElementById('<%= hdnProjectId.ClientID%>');
            if (data == "") {
                showAlert('Please Select from suggestions','orange','white');
                
                projectId.value = "";
                document.getElementById('<%= txtProjectName.ClientID%>').value = "";
            }
            else {
                projectId.value = data;
            }
            $("#prgBar").css("display", "none");
        }
        function onerror(data) {
            showAlert(data,'red','white')
            $("#prgBar").css("display", "none");
        }
        function txtProjectName_TextChanged(ctrl) {
            if(ctrl.value=="")
            {
                document.getElementById('<%= hdnProjectId.ClientID%>').value = "";
                return;
            }
            $("#prgBar").css("display", "block");
            PageMethods.txtProjectName_TextChanged(ctrl.value, projectExist, projectNotExist);
        }
        function projectExist(data) {
            //var hdnProjectId = document.getElementById('<%= hdnProjectId.ClientID%>');
            if (data == "0") {
                showAlert('Please Select from suggestions','orange','white');
                document.getElementById('<%= txtProjectName.ClientID%>').value = "";
            }
            $("#prgBar").css("display", "none");
        }
        function projectNotExist(data) {
            showAlert(data,'orange','white');
            document.getElementById('<%= txtProjectName.ClientID%>').value = "";
            $("#prgBar").css("display", "none");
        }
        function OpenTaskContractReportPopup() {
            document.getElementById('<%= Btn_TaskContractReport.ClientID %>').click();
        }
        function resetReportField() {
            document.getElementById('<%= hdnRPTClick.ClientID%>').value = "0";
            var projectName = document.getElementById('<%= txtProjectName.ClientID%>').value;
            if(projectName =="")
            {
                document.getElementById('<%= hdnProjectId.ClientID%>').value="";
            }
        }
        function OpenTaskFeedback() {
            $('#TaskFeedback').modal('show');
        }
        function CloseTaskFeedback() {
            $('#TaskFeedback').modal('hide');
        }
        function startStopTask(row, project_id, feedbackId, taskId, assignTo, taskStatus) {
            $("#prgBar").css("display", "block");
            if (taskStatus != "Assigned") {
                showAlert('Cant start ' + taskStatus + ' task','orange','white')                
                $("#prgBar").css("display", "none");
                return;
            }
            var userExist = false;
            if (assignTo == "") {
                showAlert('Task Not Assigned To You','red','white');                     
                $("#prgBar").css("display", "none");
                return;
            }
            try {
                var empList = assignTo.split(',');
                for (var i = 0; i < empList.length; i++) {
                    if (empList[i] == '<%= Session["EmpId"].ToString()%>') {
                        userExist = true;
                        break;
                    }
                }
            }
            catch (data) {
                if (userExist == '<%= Session["EmpId"].ToString()%>') {
                    userExist = true;
                }
                userExist = false;
            }
            if (!userExist) {
                showAlert('Task Not Assigned To You','red','white');     
                $("#prgBar").css("display", "none");
                return;
            }

            document.getElementById('<%= hdnProjectId.ClientID %>').value = project_id;
            var userId = '<%= Session["UserId"].ToString() %>';

            if (getTaskStatus(row) == "Start") {
                PageMethods.btnStart_Click(taskId, row, userId, startSuccess, Failure);
            }
            else {
                OpenTaskFeedback();
                $('#<%= taskFeedback.FindControl("Editor1").ClientID %>')[0].value = "";
                document.getElementById('<%= taskFeedback.FindControl("hdnFeedbackTaskId").ClientID %>').value = taskId;
                document.getElementById('<%= taskFeedback.FindControl("hdnFeedbackId").ClientID %>').value = feedbackId;
                document.getElementById('<%= taskFeedback.FindControl("hdnRowNo").ClientID %>').value = row;
                $('#<%= taskFeedback.FindControl("ddlCompleted").ClientID %>').val(getPercentage(row));
                $("#prgBar").css("display", "none");
            }
        }
        function startSuccess(data) {
            if (data[1] != "") {
                showAlert('Task started successfully','green','white');     
                setToggleOnOff(data[1], 'fa fa-toggle-on', 'Stop');
                $('#<%= lnkCurrentWorkingTask.ClientID %>').addClass("btn btn-primary");
                document.getElementById('<%= lnkCurrentWorkingTask.ClientID %>').innerText = "Show Current Working Task";
                //document.getElementById('<%= btngo.ClientID %>').click();
            }
            else {
                showAlert(data[0],'red','white');                   
            }
            $("#prgBar").css("display", "none");
        }
        function Failure(data) {
            showAlert(data,'red','white');   
            $("#prgBar").css("display", "none");
        }

        function fillGrid() {
            document.getElementById('<%= btngo.ClientID %>').click();
        }
        function openTaskHistory() {
            $('#EmpHistory').modal('show');
        }

        function btnStop_Click(row, feedbackId, taskid, feedback,percentage) {
            $("#prgBar").css("display", "block");
            PageMethods.btnStop_Click(row, feedbackId, taskid, feedback,percentage, successStop, Failure);
        }
        function successStop(data) {
            if(data[2]=="false")
            {
                showAlert(data[3],'red','white');   
                $("#prgBar").css("display", "none");    
                CloseTaskFeedback();
            }
            else
            {
                document.getElementById('<%= taskFeedback.FindControl("hdnFeedbackTaskId").ClientID %>').value = "";
                document.getElementById('<%= taskFeedback.FindControl("hdnFeedbackId").ClientID %>').value = "";
                document.getElementById('<%= taskFeedback.FindControl("hdnRowNo").ClientID %>').value = "";
                CloseTaskFeedback();
                debugger;
                setActualTime(data[0],data[4]);
                showAlert('Task Stopped Successfully','green','white');   
                $('#<%= lnkCurrentWorkingTask.ClientID %>').removeClass("btn btn-primary");
                document.getElementById('<%= lnkCurrentWorkingTask.ClientID %>').innerText = "";
                setToggleOnOff(data[0], 'fa fa-toggle-off', 'Start');
                updatePercentage(data[0],data[1]);
                $("#prgBar").css("display", "none");
            }
        }

        function setToggleOnOff(row, classname, title) {

            var paging = $('#<%= gvrstatus.ClientID %> tbody tr.pagination-ys span')[0];
            var index = row;
            if (paging != null) {
                if (parseInt(paging.innerText) > 1) {
                    index = index - (10 * parseInt(paging.innerText));
                }
            }


            var grid = $('#<%= gvrstatus.ClientID %> tbody tr')[index];
            $(grid).each(function () {
                var gRow = $(this);
                $(gRow).find("td i[id='imgToggle']")[0].className = classname;
                $(gRow).find("td a[id='aStartNStop']")[0].title = title;
            });
        }

          function setActualTime(row, time) {
              debugger;
            var paging = $('#<%= gvrstatus.ClientID %> tbody tr.pagination-ys span')[0];
            var index = row;
            if (paging != null) {
                if (parseInt(paging.innerText) > 1) {
                    index = index - (10 * parseInt(paging.innerText));
                }
            }


            var grid = $('#<%= gvrstatus.ClientID %> tbody tr')[index];
            $(grid).each(function () {
                var gRow = $(this);
                debugger;
                var dataTime =  gRow[0].cells[4].innerText.split('/')[1];
                gRow[0].cells[4].innerText = time+'/'+dataTime;
                //var dataasas =  $(gRow).find("td i[id='lblTotalHr']")[0];
            });
          }

        function updatePercentage(row,precentage)
        {
            var index = row;
            var grid = $('#<%= gvrstatus.ClientID %> tbody tr')[index];
            $(grid).each(function () {
                var gRow = $(this);
                $(gRow).find("td span[id='lblPercentage']")[0].innerText = precentage;
            });
        }
        function getPercentage(row)
        {
            var grid = $('#<%= gvrstatus.ClientID %> tbody tr')[row];
            var per ='';
            $(grid).each(function () {
                var gRow = $(this);
                per = $(gRow).find("td span[id='lblPercentage']")[0].innerText;
            });
            return per;
        }

        function getTaskStatus(row) {
            var gridRows = <%= PageControlCommon.GetPageSize() %>;
            var paging = $('#<%= gvrstatus.ClientID %> tbody tr.pagination-ys span')[0];
            var index = row;
            if (paging != null) {
                if (parseInt(paging.innerText) > 1) {
                    //index = index - (10 * parseInt(paging.innerText));
                    index = index - gridRows;
                }
            }
            var grid = $('#<%= gvrstatus.ClientID %> tbody tr')[index];
            var status = '';
            $(grid).each(function () {
                var gRow = $(this);
                status = $(gRow).find("td a[id='aStartNStop']")[0].title;
            });
            return status;
        }
        function btnTaskDetails_Click(ctrl)
        { 
            var txtTaskDate = $('#<%=txtTaskDate.ClientID%>').val();
            var txtAssignTo = $('#<%=txtAssignTo.ClientID%>').val();
            document.getElementById('<%=lblTaskName.ClientID%>').innerText="";
            document.getElementById('<%=lblProjectName.ClientID%>').innerText="";


            ctrl.disable = true;
            $("#prgBar").css("display", "block");
            if(txtTaskDate.trim()=="")
            {
                showAlert('Please Enter Date','orange','white');   
                
                ctrl.disable = false;
                $("#prgBar").css("display", "none");
                return;
            }
            var projectList =  '<%= Session["ProjectIdListByEmpId"].ToString() %>'; 
            if (projectList == "")
            {
                showAlert('You Dont Have Permission Of Any Project','orange','white');   
                ctrl.disable = false;
                $("#prgBar").css("display", "none");
                return;
            }
                        
            $('#tblEmpTaskHistory tbody tr').remove();
            $('#tblEmpLogHistory tbody tr').remove();

         
            PageMethods.btnTaskDetails_Click(txtTaskDate,txtAssignTo,function(json){                
                var data = JSON.parse(json);       
                var index=0;
                $(data).each(function(){                    
                    var row = $(this)[0];
                    var htmlRow="<tr>";
                    htmlRow=htmlRow+"<td><a onclick='btnShowFeedbacks_Command("+index+","+row.taskId+",&quot;"+row.projectName+"&quot;,&quot;"+row.subject+"&quot;)' title='Show Feedbacks' style='cursor:pointer'><i class='fa fa-comments' style='color:#333333'></i></a></td>";
                    htmlRow=htmlRow+"<td>"+row.projectName+"</td>";
                    htmlRow=htmlRow+"<td>"+row.subject+"</td>";
                    htmlRow=htmlRow+"<td>"+row.empName+"</td>";
                    htmlRow=htmlRow+"<td>"+row.startTime+"</td>";
                    htmlRow=htmlRow+"<td>"+row.stopTime+"</td>";
                    htmlRow=htmlRow+"<td>"+row.actualTime+"/"+row.requiredTime+"</td>";
                    htmlRow=htmlRow+"<td>"+ GetAmountWithDecimal(row.actualCost,0)  +"/"+ GetAmountWithDecimal(row.expectedCost,0) +"</td></tr>";
                    $('#tblEmpTaskHistory > tbody:last-child').append(htmlRow);
                    index=index+1;
                });
                ctrl.disable = false;
                $("#prgBar").css("display", "none");
            });
        }


        function btnShowFeedbacks_Command(index,taskId,project,taskName)
        {
            $("#prgBar").css("display", "block");
            $('#tblEmpTaskHistory tbody tr').each(function(){
                var row = $(this)[0];
                if(row!=null )
                {
                    row.style.backgroundColor="";
                }
            });



            $('#tblEmpTaskHistory tbody tr')[index].style.backgroundColor= '#cccccc';

            var txtTaskDate = $('#<%=txtTaskDate.ClientID%>').val();
            var txtAssignTo = $('#<%=txtAssignTo.ClientID%>').val();

            if(txtTaskDate.trim()=="")
            {
                showAlert('Please Enter Date','orange','white');   
                $("#prgBar").css("display", "none");
                return;
            }
            var projectList =  '<%= Session["ProjectIdListByEmpId"].ToString() %>'; 
            if (projectList == "")
            {
                DisplayMessage("You Dont Have Permission Of Any Project");
                $("#prgBar").css("display", "none");
                return;
            }
            
            document.getElementById('<%=lblTaskName.ClientID%>').innerText="Task Name: "+taskName;
            document.getElementById('<%=lblProjectName.ClientID%>').innerText="Project Name: "+project;

            $('#tblEmpLogHistory tbody tr').remove();

            PageMethods.btnShowFeedbacks_Command(taskId,txtAssignTo,txtTaskDate,function(json){
                var data = JSON.parse(json);                
                $(data).each(function(){
                    var row = $(this)[0];
                    var htmlRow="<tr>";                    
                    htmlRow=htmlRow+"<td>"+row.Description+"</td>";
                    htmlRow=htmlRow+"<td>"+row.Field2+"</td>";
                    htmlRow=htmlRow+"<td>"+row.field3+"</td>";
                    htmlRow=htmlRow+"<td>"+row.timediff+"</td>";
                    htmlRow=htmlRow+"<td>"+row.emp_name+"</td>";
                    htmlRow=htmlRow+"</tr>";
                    $('#tblEmpLogHistory > tbody:last-child').append(htmlRow);
                });
                $("#prgBar").css("display", "none");
            });
        }
        function currentActivitiesClick()
        {
            $('#tblCurrentActivities tbody tr').remove();            
            $('#AllWorkingTask').modal('show');
            PageMethods.btnEmployeeWorking_Click("",function(json){     
                var data = JSON.parse(json);       
                $(data).each(function(){   
                    var row = $(this)[0];
                    var htmlRow="<tr>";       
                    var ac = row.ActualCost==null?"0":row.actualCost;
                    var ec = row.Expected_Cost==null?"0":row.expectedCost;
                    htmlRow=htmlRow+"<td>"+row.projectName+"</td>";
                    htmlRow=htmlRow+"<td>"+row.subject+"</td>";
                    htmlRow=htmlRow+"<td>"+row.empName+"</td>";
                    htmlRow=htmlRow+"<td>"+row.startTime+"</td>";
                    htmlRow=htmlRow+"<td>"+row.actualTime+"/"+row.requiredTime+"</td>";
                    htmlRow=htmlRow+"<td>"+ GetAmountWithDecimal(ac,0)  +"/"+ GetAmountWithDecimal(ec,0) +"</td>";                    
                    htmlRow=htmlRow+"</tr>";
                    $('#tblCurrentActivities > tbody:last-child').append(htmlRow);
                    ac='0';
                    ec='0';
                });
            });
        }
        function validateControls(ctrl) {
            debugger;
            if (document.getElementById('<%= hdnCanPrint.ClientID %>').value != "true") {
                ctrl.children[1].children[0].hidden = true;
            }
            if (document.getElementById('<%= hdnCanEdit.ClientID %>').value != "true") {
                ctrl.children[1].children[1].hidden = true;
            }
            if (document.getElementById('<%= hdnCanUpload.ClientID %>').value != "true") {
                ctrl.children[1].children[2].hidden = true;
            }
        }
        function openTaskDetails(assignDate,expEndDate,Assignto)
        {
            document.getElementById("AssignDate").innerText=assignDate;
            document.getElementById("ExpEndDate").innerText=expEndDate;
            document.getElementById("AssignTo").innerText=Assignto;
            $('#TaskDetails').modal('show');
        }
    </script>
    <script src="../Script/employee.js"></script>
    <script src="../Script/common.js"></script>
</asp:Content>
