<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProjectTask.aspx.cs" Inherits="ProjectManagement_ProjectTask" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-tasks"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Project Task%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Project Task%>"></asp:Label></li>
    </ol>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_EmployeeList" Style="display: none;" runat="server" data-toggle="modal" data-target="#EmployeeListPopup" Text="EmployeeList" />
            <asp:Button ID="Btn_TaskContractReport" Style="display: none;" runat="server" data-toggle="modal" data-target="#TaskContractReport" Text="ContractReport" />
            <asp:Button ID="Btn_FillBugGrid" Style="display: none;" runat="server" OnClick="Btn_FillBugGrid_Click" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanAssignTask" />
            <asp:HiddenField runat="server" ID="canModifyDate" />
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
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtProjectName" />
                                <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnreset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label13" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <div class="col-md-11">
                                                            <asp:HiddenField ID="hdnDllProjectChange" runat="server" />
                                                            <asp:HiddenField ID="HidCustId" runat="server" />
                                                            <asp:HiddenField ID="hdnfileid" runat="server" />
                                                            <asp:HiddenField ID="hdnTaskid" runat="server" />
                                                            <asp:HiddenField ID="hdnProjId" runat="server" />
                                                            <asp:HiddenField ID="hdnfileidupdate" runat="server" />
                                                            <asp:TextBox ID="txtSearchprojectName" placeholder="Project Name" runat="server" Class="form-control" BackColor="#eeeeee" OnTextChanged="txtSearchprojectName_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListProject" ServicePath=""
                                                                TargetControlID="txtSearchprojectName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-1">
                                                            <asp:HiddenField runat="server" ID="hdnTreeNGrid" Value="1" />
                                                            <asp:LinkButton runat="server" ID="imgBtnTreeNGrid" CssClass="fa fa-list" Style="font-size: 30px" ToolTip="Grid View" OnClick="imgBtnTreeNGrid_Click"></asp:LinkButton>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Assigned" Value="Assigned"></asp:ListItem>
                                                                <asp:ListItem Text="Extended" Value="Extended"></asp:ListItem>
                                                                <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                                                <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                                <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="Task" Value="Subject" Selected="True" />
                                                                <asp:ListItem Text="Assigned To" Value="AssignTo" />
                                                                <asp:ListItem Text="Assigned By" Value="Assignbyname" />
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="txtValue" onkeydown='return (event.keyCode!=13);' runat="server" placeholder="Search from Content" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"> <span class="fa fa-search" style="font-size:25px;"></span></asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefresh_Click"> <span class="fa fa-repeat" style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-2" style="display: none" runat="server" id="div_assignTask">
                                                <asp:Button ID="btnAssignTaskToEmp" runat="server" CssClass="btn btn-primary" OnClick="btnAssignTaskToEmp_Click" Text="Assign Task to Employee" />
                                                <asp:HiddenField ID="hdnRPTClick" runat="server" />
                                                <br />
                                            </div>
                                            <div class="col-md-2" style="display: none" runat="server" id="div_printReport">
                                                <asp:Button ID="btnPrintTask" runat="server" Text="Print Task Contract" CssClass="btn btn-primary" OnClick="btnPrintTask_Click" />
                                                <br />
                                            </div>
                                            <div class="col-md-8">
                                            </div>
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="hdnCanEdit" runat="server" />
                                                <asp:HiddenField ID="hdnCanView" runat="server" />
                                                <asp:HiddenField ID="hdnCanComment" runat="server" />
                                                <asp:HiddenField ID="hdnCanUpload" runat="server" />
                                                <asp:HiddenField ID="hdnCanBug" runat="server" />
                                                <br />
                                            </div>
                                            <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                            <div class="col-md-12">
                                                <div class="col-md-12" style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjecttask" runat="server" PageSize="<%# PageControlCommon.GetPageSize() %>" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvrProjecttask_PageIndexChanging" AllowSorting="True" OnSorting="GvrProjecttask_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Project_Id")+"/"+Eval("Field7") %>' CommandName='<%# Eval("Subject") %>' CausesValidation="False" OnCommand="btnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName='<%# Eval("Project_Id") %>' CommandArgument='<%# Eval("Task_Id") %>' CausesValidation="False" OnCommand="btnEdit_command"><i class="fa fa-pencil"></i>Edit</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanComment.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnComment" runat="server" CommandName='<%# Eval("Project_Id") %>' CommandArgument='<%# Eval("Task_Id") %>' CausesValidation="False" OnCommand="btnComment_command"> <i class="fa fa-comment"></i>Comment </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanBug.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnBugs" runat="server" CommandName='<%# Eval("Project_Id") %>' CommandArgument='<%# Eval("Task_Id") %>' CausesValidation="False" OnCommand="btnBugs_command"> <i class="fa fa-bug"></i>Bugs
                                                                                </asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Project No." SortExpression="Field7">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProjectNo" runat="server" Text='<%# Eval("Field7") %>'></asp:Label>
                                                                    <asp:Label ID="lblProjectId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblprojectName" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Project Manager" SortExpression="ManagerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblprojectManager" runat="server" Text='<%# Eval("ManagerName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcustname2" runat="server" Text='<%# Eval("Customername") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Task" SortExpression="Subject">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Associated Task" SortExpression="parenttaskSubject" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="parenttaskSubject" runat="server" Text='<%# Eval("parenttaskSubject") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblprojectIdList12" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="Assign_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList2" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Expected Date" SortExpression="Emp_Close_Date" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Expected Hour" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaskDuration" runat="server" Text='<%#Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="End Date" SortExpression="TL_Close_Date" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTldate" runat="server" Text='<%# Formatdate(Eval("TL_Close_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Overdue Days" SortExpression="Overduedays">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lvloverdue" runat="server" Text='<%# Eval("Overduedays") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList4" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpriority" runat="server" Text='<%# Eval("Field41") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Completed %" SortExpression="Field5">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcompletion" runat="server" Text='<%# Eval("Field51") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>

                                                    <dx:ASPxTreeList ID="TaskTreeList" CssClass="table-striped table-bordered table table-hover" runat="server" AutoGenerateColumns="False" OnCustomCallback="treeList_CustomCallback" EnableCallbacks="false"
                                                        Width="100%" KeyFieldName="Task_Id" ParentFieldName="ParentTaskId">
                                                        <SettingsBehavior AllowDragDrop="true" />
                                                        <SettingsSelection Enabled="true" />
                                                        <Columns>

                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListGetparenttask1">                                                                        
                                                                        <Image Url="~/Images/Add.png" Height="16px" Width="16px" ToolTip="Add Child">
                                                                        </Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>
                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListExtended">
                                                                        <Image Url="~/Images/contact_renewal.png" Height="16px" Width="16px" ToolTip="Extend Task">
                                                                        </Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>
                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListFileUpload1">
                                                                        <Image Url="~/Images/ModuleIcons/archiving.png" Height="16px" Width="16px" ToolTip="File Upload">
                                                                        </Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>
                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px" CellStyle-Border-BorderStyle="None">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListEditTask">
                                                                        <Image Url="~/Images/edit.png" Height="16px" Width="16px" ToolTip="Edit">
                                                                        </Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>
                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListEditComments">
                                                                        <Image Url="~/Images/comment.png" Width="20px" ToolTip="Comments"></Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>
                                                            <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image" Caption=" " Width="25px">
                                                                <CustomButtons>
                                                                    <dx:TreeListCommandColumnCustomButton ID="TreeListEditBugs">
                                                                        <Image Url="~/Images/bug.png" Width="20px" ToolTip="Bug">
                                                                        </Image>
                                                                    </dx:TreeListCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:TreeListCommandColumn>

                                                            <dx:TreeListDataColumn FieldName="Subject" VisibleIndex="0" CellStyle-Wrap="True" Width="50px" />
                                                            <dx:TreeListDataColumn FieldName="AssignBy" VisibleIndex="1" CellStyle-Wrap="True" Width="30px" />
                                                            <dx:TreeListDataColumn FieldName="AssignTo" VisibleIndex="1" CellStyle-Wrap="True" Width="30px" />
                                                            <dx:TreeListDataColumn FieldName="Field51" Caption="Completed %" VisibleIndex="4" Width="100px" CellStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                        <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                                                        <ClientSideEvents CustomButtonClick="function(s, e) {s.PerformCallback(e.buttonID + '|' + e.nodeKey);}" />
                                                    </dx:ASPxTreeList>

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaskData" runat="server" PageSize="<%# PageControlCommon.GetPageSize() %>" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="gvTaskData_PageIndexChanging" AllowSorting="True" OnSorting="gvTaskData_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <div class="dropdown" onclick="validateControls(this)">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li >
                                                                                <asp:LinkButton ID="imgBtnRenewal" runat="server" CommandName='<%# Eval("Project_Id") %>' CommandArgument='<%# Eval("Task_Id") %>' CausesValidation="False" OnCommand="imgBtnRenewal_Command"><i class="fa fa-repeat"></i>Renewal</asp:LinkButton>
                                                                            </li>
                                                                            <li  <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnPrint" runat="server" CommandArgument='<%# Eval("Task_Id") %>' CommandName='<%# Eval("Project_Id") %>' CausesValidation="False" OnCommand="btnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>
                                                                            <li  <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Project_Id")+"/"+Eval("Field7") %>' CommandName='<%# Eval("Subject") %>' CausesValidation="False" OnCommand="btnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                            <li  <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName='<%# Eval("Project_Id") %>' CommandArgument='<%# Eval("Task_Id") %>' CausesValidation="False" OnCommand="btnEdit_command"><i class="fa fa-pencil"></i>Edit</asp:LinkButton>
                                                                            </li>
                                                                            <li  <%= hdnCanComment.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="btnComment_command('<%# Eval("Task_Id") %>');" style="cursor: pointer"><i class="fa fa-comment"></i>Comments</a>
                                                                            </li>
                                                                            <li  <%= hdnCanBug.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="btnBug_click('<%# Eval("Task_Id") %>','<%# Eval("project_Id") %>');" style="cursor: pointer"><i class="fa fa-bug"></i>Bugs</a>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Task">
                                                                <ItemTemplate>
                                                                    <%# Eval("Subject") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="AssignBy">
                                                                <ItemTemplate>
                                                                    <%# Eval("AssignBy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="AssignDate1">
                                                                <ItemTemplate>
                                                                    <%# Eval("AssignDate1") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField HeaderText="Expected Date" SortExpression="Emp_Close_Date" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="Expected Hour" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaskDuration" runat="server" Text='<%#Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Actual Hour" SortExpression="Actual_Hours">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaskActualhours" runat="server" Text='<%#Eval("Actual_Hours") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <%# Eval("Status") %>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpriority" runat="server" Text='<%# Eval("Field41") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Completed %" SortExpression="Field51">
                                                                <ItemTemplate>
                                                                    <%# Eval("Field51") %>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>

                                                </div>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:HiddenField ID="HiddeniD" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvrProjecttask" />
                                <asp:AsyncPostBackTrigger ControlID="TaskTreeList" />
                                <asp:AsyncPostBackTrigger ControlID="txtSearchprojectName" />
                                <asp:AsyncPostBackTrigger ControlID="gvTaskData" />

                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdnProjectId" />
                                                        <br />
                                                        <asp:TextBox ID="txtProjectName" runat="server" Class="form-control" BackColor="#eeeeee" placeholder="Project Name" OnTextChanged="ddlprojectname_SelectedIndexChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListProject" ServicePath=""
                                                            TargetControlID="txtProjectName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="Task Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlAssigningTasktype" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div runat="server" id="tdprojectSchedule" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblProjectStartdate" runat="server" Text="Project Start Date" Font-Bold="true"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblProjectStartdateValue" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblProjectExpenddate" runat="server" Text="Expected End Date" Font-Bold="true"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblProjectExpenddateValue" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTaskcaption" Visible="false" runat="server" Text="Task"></asp:Label>
                                                        <asp:TextBox ID="txtsubject" placeholder="Task" onkeydown="return (event.keyCode!=13);" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label9" runat="server" Text="Task Description" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="Editor1" runat="server" placeholder="Task Description" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label21" runat="server" Text="Nature Of Task"></asp:Label>
                                                        <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlTaskType_SelectedIndexChanged">
                                                            <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                            <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label19" runat="server" Text="Priority"></asp:Label>
                                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                                            <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                                                            <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                                                            <asp:ListItem Text="High" Value="High"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:TextBox ID="txtExtendedId" runat="server" placeholder="Extended By" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" id="trparenttask" runat="server">
                                                        <asp:Label ID="Label18" runat="server" Text="Parent Task"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlParentTask" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="trParentttaskdetail" runat="server" visible="false">
                                                        <asp:Label ID="Label4" runat="server" Text="Parent Task Detail"></asp:Label>
                                                        <asp:Label ID="lblParentttaskDetailStart" Style="margin-left: 50px;" Font-Italic="true" runat="server"></asp:Label>
                                                        <asp:Label ID="lblParentttaskDetailEnd" Style="margin-left: 50px;" runat="server" Font-Italic="true"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trAssigneddateInfo" runat="server">
                                                        <div class="col-md-4" id="div_assign" runat="server">
                                                            <asp:TextBox ID="txtassigndate" runat="server" placeholder="Assign Date" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtassigndate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4" id="div_end" runat="server">

                                                            <asp:TextBox ID="txtempenddate" runat="server" placeholder="Exp. End Date" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtempenddate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4" id="div2" runat="server">
                                                            <asp:TextBox ID="txtRequiredHrs" runat="server" placeholder="Required Hours" CssClass="form-control"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtRequiredHrs">
                                                            </cc1:MaskedEditExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-4" style="display: none;" id="div_extended" runat="server">
                                                            <asp:Label ID="lblExtendedTime" runat="server" Text="Extended Date & Time"></asp:Label>
                                                            <a style="color: Red">*</a>

                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtExtendedDate" runat="server" CssClass="form-control" onchange="checkDateDiff_EndDate_ExtendedDate()"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server"
                                                                    Enabled="True" TargetControlID="txtExtendedDate">
                                                                </cc1:CalendarExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:TextBox ID="txtExtendedTime" Width="120px" runat="server" CssClass="form-control" onchange="checkExtendedTime()"></asp:TextBox>
                                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" CultureAMPMPlaceholder=""
                                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                        Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtExtendedTime">
                                                                    </cc1:MaskedEditExtender>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="tlEnddate" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,TL End Date %>"></asp:Label>
                                                            <asp:TextBox ID="txttlenddate" onkeydown="return (event.keyCode!=13);" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txttlenddate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,TL End Time %>"></asp:Label>
                                                            <asp:TextBox ID="txttlendtime" onkeydown="return (event.keyCode!=13);" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txttlendtime">
                                                            </cc1:MaskedEditExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <%--   <div id="trteamavailable" runat="server" class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnCheckTeamAvailable" runat="server" Text="Check Team Availability"
                                                            Visible="false" OnClick="btnCheckTeamAvailable_Click" CssClass="btn btn-primary" />
                                                        <br />
                                                    </div>--%>
                                                    <div class="col-md-12" id="trassignee" runat="server">
                                                        <asp:Label ID="Label16" runat="server" Text="Assign To"></asp:Label>
                                                        <asp:Panel ID="pnlBrand" runat="server" Style="max-height: 500px;" Width="100%" BorderStyle="Solid"
                                                            BorderWidth="1px" BorderColor="#eeeeee" BackColor="White" ScrollBars="Auto">
                                                            <asp:CheckBoxList ID="listtaskEmployee" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS"
                                                                CssClass="checkboxList" Font-Size="Small" ForeColor="Gray" />
                                                        </asp:Panel>

                                                        <style>
                                                            .checkboxList {
                                                                margin: 10px;
                                                            }
                                                        </style>

                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" id="taskduration" runat="server" visible="false">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Task Duration(In Hours) %>"></asp:Label>
                                                        <asp:TextBox ID="txtDuration" onkeydown="return (event.keyCode!=13);" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtDuration">
                                                        </cc1:MaskedEditExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trAddress" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="Customer Site Address" />
                                                        <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                        <asp:TextBox ID="txtSiteAddress" onkeydown="return (event.keyCode!=13);" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtShipingAddress_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_shipaddress" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSiteAddress"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trIsAddNewCustomer" runat="server" visible="false" class="col-md-6" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnaddContact" runat="server" Text="Add New Contact Person"
                                                            CssClass="btn btn-primary" OnClick="btnaddContact_Click" />
                                                        <asp:Button ID="btnRefreshContactList" runat="server" Text="Refresh" CssClass="btn btn-primary"
                                                            OnClick="btnRefreshContactList_Click" />
                                                        <br />
                                                    </div>
                                                    <div id="trContactperson" runat="server" visible="false" class="col-md-12">
                                                        <asp:Label ID="lbl" runat="server" Text="Customer Contact Person"></asp:Label>
                                                        <asp:Panel ID="pnlContactPerson" runat="server" Height="100px" Width="100%" BorderStyle="Solid"
                                                            BorderWidth="1px" BorderColor="#abadb3" BackColor="White" ScrollBars="Auto">
                                                            <asp:CheckBoxList ID="ChkContactPerson" runat="server" RepeatColumns="4" CellPadding="5"
                                                                Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                                        </asp:Panel>
                                                        <br />
                                                    </div>
                                                    <div runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Task Completion (%)"></asp:Label>
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

                                                    <div class="col-md-4">
                                                        <asp:TextBox runat="server" ID="txtAssignBy" placeholder="Assign By" CssClass="form-control" BackColor="#eeeeee" onchange="validateEmployee(this)"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAssignBy" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAssignBy"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkCancel" onkeydown="return (event.keyCode!=13);" CssClass="form-control" runat="server" Text="Is cancel" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:TextBox runat="server" ID="txtExpectedCost" placeholder="Expected Cost" CssClass="form-control" onchange="validateAmt(this)"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12"></div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnsave" Visible="false" runat="server" CssClass="btn btn-success"
                                                            OnClick="btnsave_Click" Text="<%$ Resources:Attendance,Save%>" />
                                                        <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                                            Text="<%$ Resources:Attendance,Reset%>" />
                                                        <asp:Button ID="btncencel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            OnClick="btncencel_Click" CssClass="btn btn-danger" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="EmployeeListPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-mg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel2">Assign Task Employees
                    </h4>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnSelectedTask" runat="server" />

                            <div class="col-md-4">
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                                <asp:TextBox runat="server" ID="txtFrDt" CssClass="form-control" onchange="checkDateDiffForAssignTaskEmpPopup()"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender4" runat="server" TargetControlID="txtFrDt" Format="dd-MMM-yyyy" />
                                <br />
                            </div>
                            <%--<div class="col-md-3">
                                <asp:Label ID="lblAssignTime" runat="server" Text="Assign Time"></asp:Label>
                                <asp:TextBox ID="txtAssignTime1" Width="120px" runat="server" CssClass="form-control" onchange="checkAssignTimeEmpPopup()"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender7" runat="server" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                    Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtAssignTime1" UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                </cc1:MaskedEditExtender>
                            </div>--%>
                            <div class="col-md-4">
                                <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                                <asp:TextBox runat="server" ID="txtToDt" CssClass="form-control" onchange="checkDateDiffForAssignTaskEmpPopup()"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender6" runat="server" TargetControlID="txtToDt" Format="dd-MMM-yyyy" />
                                <br />
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblEndTime" runat="server" Text="Required Hours"></asp:Label>
                                <asp:TextBox ID="txtRequiredHr" Width="120px" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender8" runat="server" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                    Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtRequiredHr">
                                </cc1:MaskedEditExtender>
                            </div>
                            <div class="col-md-12" id="Div1" runat="server" style="overflow: scroll">
                                <br />
                                <asp:Label ID="Label25" runat="server" Text="Assing To"></asp:Label>
                                <asp:Panel ID="Panel1" runat="server" Style="max-height: 500px;" Width="100%" BorderStyle="Solid"
                                    BorderWidth="1px" BorderColor="#eeeeee" BackColor="White" ScrollBars="Auto">
                                    <asp:CheckBoxList ID="CBLAssignTo" runat="server" RepeatColumns="3" CellPadding="5" CellSpacing="10" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                </asp:Panel>
                                <br />

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-12">
                    <br />
                    <asp:Button runat="server" ID="btnUpdateTask" CssClass="btn btn-primary" Text="Add Selected Employees To Task" OnClick="btnUpdateTask_Click" />
                    <br />
                </div>

                <div class="modal-footer">
                </div>

            </div>

        </div>
    </div>


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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
    <script type="text/javascript">
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

        function LI_NewBug() {

            document.getElementById('Li_List').style.display = "none";
            document.getElementById('List').style.display = "none";

            document.getElementById('Li_New').classList.add("active");
            document.getElementById('New').classList.add("active");

        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100004;
        }
        function validateAmt(id) {
            if (isNaN(id.value)) {
                showAlert("Please Enter A Valid Number", 'orange', 'white');
                id.value = "";
            }
        }

      <%--  function checkAssignTime() {
            var assigntime = document.getElementById('<%= txtassigntime.ClientID%>');
            var hr = assigntime.value.split(':')[0];
            var min = assigntime.value.split(':')[1];
            if (parseInt(hr) > 23) {
                showAlert("Not a valid Time");
                assigntime.value = "";
            }
            else {
                if (parseInt(min) > 59) {
                    showAlert("Not a valid Time");
                    assigntime.value = "";
                }
            }
        }--%>

        function checkEndTime() {
            var endtime = document.getElementById('<%= txtRequiredHrs.ClientID%>');
            var hr = endtime.value.split(':')[0];
            var min = endtime.value.split(':')[1];
            if (parseInt(hr) > 23) {
                showAlert("Not a valid Time", 'orange', 'white');
                endtime.value = "";
            }
            else {
                if (parseInt(min) > 59) {
                    showAlert("Not a valid Time", 'orange', 'white');
                    endtime.value = "";
                }
            }
        }

        function checkExtendedTime() {
            var endtime = document.getElementById('<%= txtExtendedTime.ClientID%>');
            var hr = endtime.value.split(':')[0];
            var min = endtime.value.split(':')[1];
            if (parseInt(hr) > 23) {
                showAlert("Not a valid Time", 'orange', 'white');
                endtime.value = "";
            }
            else {
                if (parseInt(min) > 59) {
                    showAlert("Not a valid Time", 'orange', 'white');
                    endtime.value = "";
                }
            }
        }

        function checkDateDiff_StartDate_EndDate() {
            var fromdt = document.getElementById('<%= txtassigndate.ClientID%>');
            if (fromdt.value == "") {
                showAlert("Please Enter Assign Date", 'orange', 'white');
                return;
            }
            var todt = document.getElementById('<%= txtempenddate.ClientID%>');
            if (fromdt.value != "" && todt.value) {
                if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                    showAlert("Expected End Date Should Be Greater Than Assign Date", 'orange', 'white');
                    todt.value = "";
                }
            }
        }

        function checkDateDiff_EndDate_ExtendedDate() {
            var fromdt = document.getElementById('<%= txtempenddate.ClientID%>');
            if (fromdt.value == "") {
                showAlert("Please Enter Expected End Date", 'orange', 'white');
                return;
            }
            var todt = document.getElementById('<%= txtExtendedDate.ClientID%>');
            if (fromdt.value != "" && todt.value) {
                if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                    showAlert("Extended Date Should Be Greater Than Expected End Date", 'orange', 'white');
                    todt.value = "";
                }
            }
        }

        function OpenEmployeeListPopup() {
            document.getElementById('<%= Btn_Modal_EmployeeList.ClientID %>').click();

        }


        function OpenTaskContractReportPopup() {
            document.getElementById('<%= Btn_TaskContractReport.ClientID %>').click();
        }

        function checkDateDiffForAssignTaskEmpPopup() {
            var fromdt = document.getElementById('<%= txtFrDt.ClientID%>');
            var todt = document.getElementById('<%= txtToDt.ClientID%>');
            if (fromdt.value != "" && todt.value) {
                if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                    showAlert("To Date Should Be Greater Than From Date", 'orange', 'white');
                    todt.value = "";
                }
            }
        }
        function resetReportField() {
            document.getElementById('<%= hdnRPTClick.ClientID%>').value = "0";
        }
        function ddlSearchprojectName_IndexChanged(s, e) {
            var urlParams = new URLSearchParams(window.location.search);
            var taskId = "";
            if (urlParams.has('Task')) {
                taskId = urlParams.get('Task');
            }
            var ddlStatus = document.getElementById('<%= ddlStatus.ClientID%>');
            var status = "All";
            if (ddlStatus != null) {
                status = ddlStatus.value;
            }
            if (s.lastSuccessValue == null) {
                return;
            }
            PageMethods.FillTaskListAjax(s.lastSuccessValue, status, taskId, onSuccessUpdate, onFailUpdate);
        }

        function onSuccessUpdate(isTask) {
            document.getElementById('<%= hdnDllProjectChange.ClientID%>').value = "0";

            if (isTask == true) {
                document.getElementById('<%= Btn_FillBugGrid.ClientID%>').click();
            }

        }
        function onFailUpdate(error) {
            showAlert(error, 'orange', 'white');
        }
        function getProjectId(ctrl) {
            var projectId = document.getElementById('<%= hdnProjectId.ClientID%>');
            PageMethods.getProjectId(ctrl.value, function (data) {
                if (data == "") {
                    showAlert("Please Select From Suggesstions", 'orange', 'white');
                    projectId.value = "";
                }
                else {
                    projectId.value = data;
                }
            });
        }
        function btnComment_command(taskId) {
            window.open('../ProjectManagement/Projecttaskfeedback.aspx?Task_Id=' + taskId + '', 'window', 'width=1024, ');
        }
        function btnBug_click(taskId, projectId) {
            window.open('../ProjectManagement/Projecttask.aspx?Task=' + taskId + '&Project_Id=' + projectId + '', 'window', 'width=1024, ');
        }
        function validateControls(ctrl) {
<%--            if (document.getElementById('<%= hdnCanPrint.ClientID %>').value != "true") {
                ctrl.children[1].children[0].hidden = true;
            }
            if (document.getElementById('<%= hdnCanEdit.ClientID %>').value != "true") {
                ctrl.children[1].children[1].hidden = true;
            }
            if (document.getElementById('<%= hdnCanUpload.ClientID %>').value != "true") {
                ctrl.children[1].children[2].hidden = true;
            }--%>
        }
    </script>
    <script src="../Script/employee.js"></script>
    <script src="../Script/common.js"></script>
</asp:Content>
