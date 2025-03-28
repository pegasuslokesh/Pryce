<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProjectMaster.aspx.cs" Inherits="ProjectManagement_ProjectMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-tasks"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Project Master%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Project Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>

            <asp:Button ID="Btn_Sales_Order" Style="display: none;" runat="server" OnClick="btnSalesOrder_Click" Text="Sales Order" />
            <asp:Button ID="Btn_Delete_Category" Style="display: none;" runat="server" data-toggle="modal" data-target="#Delete_Category" Text="Delete Category" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCustomerId" />

            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanBug" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField runat="server" ID="hdnCanTask" />
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
                    <li id="Li_Sales_Order"><a href="#Sales_Order" onclick="Li_Tab_Sales_Order()" data-toggle="tab">
                        <i class="fa fa-file-alt"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sales Order %>"></asp:Label></a></li>
                    <li id="Li_Close"><a href="#Close" data-toggle="tab">
                        <i class="fa fa-times"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></a></li>
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
                                <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnreset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvrProjectteam" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <div class="form-group">
                                                            <div class="col-lg-6">
                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Location  %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>

                                                            <div class="col-lg-12">
                                                                <br />
                                                            </div>

                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlProjectStatus" runat="server" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlProjectStatus_SelectedIndexChanged">
                                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                    <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                                    <asp:ListItem Text="Overdue" Value="Overdue"></asp:ListItem>
                                                                    <asp:ListItem Text="Upcoming" Value="Upcoming"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Project No." Value="Field7" />
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Project Name %>" Value="Project_Name"
                                                                        Selected="True" />
                                                                    <asp:ListItem Text="Project Manager" Value="ManagerName" />
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name" />
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Project Type %>" Value="Project_Type" />
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Project Title %>" Value="Project_Title" />
                                                                    <asp:ListItem Text="Sales Order No." Value="OrderNo" />

                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User" />
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
                                                            <div class="col-lg-4">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender_ProjectTypeSuggestion" runat="server"
                                                                        DelimiterCharacters="" Enabled="false" ServiceMethod="GetProjectType" ServicePath=""
                                                                        CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtValue"
                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>">  <span class="fa fa-search" style="font-size:25px;"></span></asp:LinkButton>
                                                                &nbsp;
                                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                                    OnClick="btnRefresh_Click"><span class="fa fa-repeat" style="font-size:25px;"></span></asp:LinkButton>
                                                                &nbsp;
                                                                <asp:LinkButton ID="btnGridView" runat="server" CausesValidation="False" Style="font-size: 25px;" CssClass="fa fa-list" Visible="true" OnClick="btnGridView_Click" ToolTip="<%$ Resources:Attendance, Tree View %>"></asp:LinkButton>
                                                                &nbsp;<asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                            </div>


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
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlLocation" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnbind" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnGridView" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnreset" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btncencel" EventName="Click" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjectteam" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                AllowPaging="True" PageSize="<%# PageControlCommon.GetPageSize() %>" OnPageIndexChanging="GvrProjectteam_PageIndexChanging"
                                                                AllowSorting="True" OnSorting="GvrProjectteam_Sorting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown" style="position: absolute;">
                                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu">
                                                                                    <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Project_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Project_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Project_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                                    </li>
                                                                                    <li <%= hdnCanTask.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnTask" runat="server" CommandName='<%# Eval("Project_Id") %>' CausesValidation="False" OnCommand="btnTask_command"><i class="fa fa-tasks"></i>Task</asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanBug.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnBugs" runat="server" CommandName='<%# Eval("Project_Id") %>' CausesValidation="False" OnCommand="btnBugs_command"><i class="fa fa-bug"></i>Bugs</asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Project_Id") %>' CommandName='<%# Eval("Field7") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>

                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Project No." SortExpression="Project_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprojectNo" runat="server" Text='<%# Eval("Field7") %>'></asp:Label>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Project_Title") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Project_Name">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="HiddeniD" runat="server" Value='<%# Eval("Project_Id") %>' />
                                                                            <asp:LinkButton ID="lnkProjectName" runat="server" Text='<%# Eval("Project_Name") %>' Font-Underline="true" Visible="false"></asp:LinkButton>
                                                                            <a onclick="projectNameLnkBtnClick(<%# Eval("Project_Id") %>);return false;" style="cursor: pointer">
                                                                                <%# Eval("Project_Name") %>
                                                                            </a>
                                                                            <%--<asp:Label ID="lblprojectIdList" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Project Manager" SortExpression="ManagerName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprojectManager" runat="server" Text='<%# Eval("ManagerName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcustname2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Type%>" SortExpression="Project_Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprojectype1" runat="server" Text='<%# Eval("Project_Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date %>" SortExpression="Start_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcustname1" runat="server" Text='<%#Formatdate( Eval("Start_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exp End Date %>" SortExpression="Exp_End_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Formatdate(Eval("Exp_End_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Completed %" SortExpression="ProjectCompleted">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcompletion" runat="server" Text='<%# Eval("ProjectCompleted") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SO No" SortExpression="OrderNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo") %>' ToolTip="Sales Order No."></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:TreeView ID="TreeViewCategory" runat="server" Visible="false" OnSelectedNodeChanged="TreeViewCategory_SelectedNodeChanged">
                                                    </asp:TreeView>
                                                    <asp:HiddenField ID="HiddeniD" runat="server" />
                                                    <asp:HiddenField ID="hidcustID1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-md">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                                    </h4>
                                </div>
                                <div class="modal-body">
                                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
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

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvrProjectteam" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="hdnPrjectId" runat="server" />
                                                        <asp:Label ID="Label17" runat="server" Text="Project No."></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProjectNo" ErrorMessage="<%$ Resources:Attendance,Enter Project No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtProjectNo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtprojecttype" ErrorMessage="<%$ Resources:Attendance,Enter Project Type%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtprojecttype" runat="server" CssClass="form-control" placeholder="Project Type" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetProjectType" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtprojecttype"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblrefferenceproject" runat="server" Text="Reference Project"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlprojectname" runat="server" CssClass="form-control" DropDownWidth="50%"
                                                            OnSelectedIndexChanged="ddlprojectname_OnSelectedIndexChanged"
                                                            DropDownStyle="DropDownList" ValueField="Project_Id" ValueType="System.String"
                                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            AutoPostBack="true" CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Project_Name" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="trImport" runat="server" visible="false">
                                                        <asp:CheckBox ID="chkImportRefProjectTeam" runat="server" ForeColor="Blue"
                                                            Text="Import Reference project team ?" />
                                                        <asp:CheckBox ID="chkImportRefProjectTask" Style="margin-left: 30px;" runat="server" ForeColor="Blue"
                                                            Text="Import Reference project task ?" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtprojectname" ErrorMessage="<%$ Resources:Attendance,Enter Project Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtprojectname" runat="server" CssClass="form-control" placeholder="Project Name"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:TextBox ID="txtprojectlocalname" runat="server" CssClass="form-control" placeholder="Project Name(Local)"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="ctlEditor1" runat="server">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Project Description %>"></asp:Label>
                                                        <cc2:Editor ID="Editor1" runat="server" CssClass="form-control" AutoFocus="False" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label15" runat="server" Text="Parent Project"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlparentProjectname" runat="server" CssClass="form-control" DropDownWidth="718px"
                                                            DropDownStyle="DropDownList" ValueField="Project_Id" ValueType="System.String"
                                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Project_Name" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlCustomerNamme" runat="server" CssClass="form-control" DropDownWidth="718px"
                                                            OnSelectedIndexChanged="txtcustomername_TextChanged" AutoPostBack="true"
                                                            DropDownStyle="DropDownList" ValueField="Customer_Id" ValueType="System.String"
                                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Name" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlOrderNo" runat="server" CssClass="form-control" DropDownWidth="100%"
                                                            ItemStyle-Wrap="True" OnSelectedIndexChanged="txtOrderNo_TextChanged"
                                                            DropDownStyle="DropDownList" ValueField="Trans_Id" ValueType="System.String"
                                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            AutoPostBack="true" CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="SalesOrderNo" Caption="Order No" />
                                                                <dx:ListBoxColumn FieldName="FormatedOrderDate" Caption="Order Date" />
                                                                <dx:ListBoxColumn FieldName="netamount" Caption="Order Amount" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlContactPerson" runat="server">
                                                        <br />
                                                        <asp:TextBox ID="ddlContactPerson" runat="server" Class="form-control" BackColor="#eeeeee" placeholder="Customer Representative" onchange="validateContactPerson(this)" />
                                                        <cc1:AutoCompleteExtender ID="ACContactPerson" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListContact" ServicePath=""
                                                            TargetControlID="ddlContactPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label23" runat="server" Text="Project Manager"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <dx:ASPxComboBox ID="ddlEmployeeName" runat="server" CssClass="form-control" DropDownWidth="550"
                                                            DropDownStyle="DropDownList" ValueField="Emp_Id" ValueType="System.String"
                                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Emp_Name" Caption="Employee Name" />
                                                                <dx:ListBoxColumn FieldName="Emp_Code" Caption="Employee Code" />
                                                                <dx:ListBoxColumn FieldName="Designation" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtstartdate" ErrorMessage="<%$ Resources:Attendance,Enter Start Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtstartdate" placeholder="Start Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtstartdate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtexpenddate" ErrorMessage="<%$ Resources:Attendance,Enter Exp End Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtexpenddate" runat="server" CssClass="form-control" placeholder="Exp End Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtexpenddate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lnkAddExp" runat="server" Text="Project Detail"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" id="pnlTabInformation" runat="server">
                                                                                <cc1:TabContainer ID="tabContainer" runat="server" CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="1">

                                                                                    <cc1:TabPanel ID="TabPanelproduct" runat="server" Width="100%" HeaderText="Order Detail">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPanelproduct" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvproduct" ShowHeader="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                                                                ShowFooter="true" OnRowDeleting="gvproduct_RowDeleting"
                                                                                                                OnRowCommand="gvproduct_RowCommand">
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblproductCode" runat="server" Text='<%#Eval("ProductCode") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                                                OnTextChanged="txtProductCode_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtendertxtProductCode" runat="server"
                                                                                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                                                                ServiceMethod="GetCompletionListRelatedProductCode" ServicePath="" TargetControlID="txtProductCode"
                                                                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblproductName" runat="server" Text='<%#Eval("EProductName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtERelatedProduct" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                                                OnTextChanged="txtERelatedProduct_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListRelatedProductName"
                                                                                                                                ServicePath="" TargetControlID="txtERelatedProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                            <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("Unit_Name") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:DropDownList ID="ddlunit" runat="server" CssClass="form-control">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblquantity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                                                                TargetControlID="txtquantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <EditItemTemplate>
                                                                                                                            <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                                                CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                                                            <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-primary" CommandName="Cancel" Text="Cancel" />
                                                                                                                        </EditItemTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-info" Visible="false" />
                                                                                                                            <asp:ImageButton ID="ButtonDelete" runat="server" CommandArgument='<%#Eval("Trans_Id") %>' CommandName="Delete" Width="14px" Height="14px" ImageUrl="~/Images/Delete1.png" />
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="ButtonAdd">

                                                                                                                                <%--  <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/Images/add.png" CommandName="AddNew" ToolTip="Add" />--%>

                                                                                                                                <asp:Button ID="ButtonAdd" runat="server" CssClass="btn btn-info" CommandName="AddNew" Text="Add Product" />
                                                                                                                                <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" />--%>
                                                                                                                            </asp:Panel>
                                                                                                                        </FooterTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>

                                                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_TabPanelproduct">
                                                                                                <ProgressTemplate>
                                                                                                    <div class="modal_Progress">
                                                                                                        <div class="center_Progress">
                                                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ProgressTemplate>
                                                                                            </asp:UpdateProgress>
                                                                                        </ContentTemplate>
                                                                                    </cc1:TabPanel>
                                                                                    <cc1:TabPanel ID="TabPaneltools" runat="server" Width="100%" HeaderText="Parts & Tools">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPaneltools" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTools" ShowHeader="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                                                                ShowFooter="true" OnRowDeleting="gvTools_RowDeleting"
                                                                                                                OnRowEditing="gvTools_RowEditing" OnRowCancelingEdit="gvTools_OnRowCancelingEdit"
                                                                                                                OnRowCommand="gvTools_RowCommand">

                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="Tools Id">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblproductCode" runat="server" Text='<%#Eval("ProductCode") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <EditItemTemplate>
                                                                                                                            <asp:TextBox ID="f" runat="server"></asp:TextBox>
                                                                                                                        </EditItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                                                OnTextChanged="txttoolsProductCode_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtendertxtProductCode" runat="server"
                                                                                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                                                                ServiceMethod="GetCompletionListRelatedProductCode" ServicePath="" TargetControlID="txtProductCode"
                                                                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Tools Name">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblproductName" runat="server" Text='<%#Eval("EProductName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtERelatedProduct" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                                                OnTextChanged="txtToolsERelatedProduct_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListRelatedProductName"
                                                                                                                                ServicePath="" TargetControlID="txtERelatedProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                            <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("Unit_Name") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:DropDownList ID="ddlunit" runat="server" CssClass="form-control">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblquantity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                                                                TargetControlID="txtquantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <EditItemTemplate>
                                                                                                                            <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                                                CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                                                            <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-primary" CommandName="Cancel" Text="Cancel" />
                                                                                                                        </EditItemTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Button ID="ButtonEdit" runat="server" CssClass="btn btn-info" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                                                            <asp:ImageButton ID="ButtonDelete" runat="server" CommandArgument='<%#Eval("Trans_Id") %>' Width="14px" Height="14px" ImageUrl="~/Images/Delete1.png" CommandName="Delete" />
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:Panel ID="pnlGridviewTools" runat="server" DefaultButton="ButtonAdd">
                                                                                                                                <asp:Button ID="ButtonAdd" CssClass="btn btn-info" runat="server" CommandName="AddNew" Text="Add Tools" />
                                                                                                                            </asp:Panel>
                                                                                                                        </FooterTemplate>
                                                                                                                        <%--  <FooterStyle Width="150px" />
                                                                                                             <ItemStyle Width="150px" />--%>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>

                                                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_TabPaneltools">
                                                                                                <ProgressTemplate>
                                                                                                    <div class="modal_Progress">
                                                                                                        <div class="center_Progress">
                                                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ProgressTemplate>
                                                                                            </asp:UpdateProgress>
                                                                                        </ContentTemplate>
                                                                                    </cc1:TabPanel>
                                                                                    <cc1:TabPanel ID="TabPanelExpenses" runat="server" Width="100%" HeaderText="Expenses Information">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPanelExpenses" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExpenses" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                                                ShowFooter="true" OnRowDeleting="gvExpenses_RowDeleting" Width="100%"
                                                                                                                OnRowCommand="gvExpenses_RowCommand">

                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="Expenses">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblExpName" runat="server" Text='<%#Eval("Exp_Name") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:DropDownList ID="ddlExpense" runat="server" CssClass="form-control">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency%>">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblcurrency" runat="server" Text='<%#Eval("CurrencyName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:DropDownList ID="ddlExpCurrency" runat="server" CssClass="form-control">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Expenses Amount">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# GetAmountDecimal(Eval("ExpCurrencyID").ToString(),Eval("FCExpAmount").ToString()) %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtexpAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                                                                TargetControlID="txtexpAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <EditItemTemplate>
                                                                                                                            <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                                                CommandArgument='<%#Eval("Expense_Id") %>' />
                                                                                                                            <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-primary" CommandName="Cancel" Text="Cancel" />
                                                                                                                        </EditItemTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Button ID="ButtonEdit" runat="server" CssClass="btn btn-info" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                                                            <asp:ImageButton ID="ButtonDelete" runat="server" Width="14px" Height="14px" ImageUrl="~/Images/Delete1.png" CommandArgument='<%#Eval("Expense_Id") %>'
                                                                                                                                CommandName="Delete" />
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:Panel ID="pnlGridviewTools" runat="server" DefaultButton="ButtonAdd">
                                                                                                                                <asp:Button ID="ButtonAdd" runat="server" CssClass="btn btn-info" CommandName="AddNew" Text="Add New Row" />
                                                                                                                            </asp:Panel>
                                                                                                                        </FooterTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>

                                                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_TabPanelExpenses">
                                                                                                <ProgressTemplate>
                                                                                                    <div class="modal_Progress">
                                                                                                        <div class="center_Progress">
                                                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ProgressTemplate>
                                                                                            </asp:UpdateProgress>
                                                                                        </ContentTemplate>
                                                                                    </cc1:TabPanel>
                                                                                    <cc1:TabPanel ID="TabPanelCost" Visible="false" runat="server" Width="100%" HeaderText="Project Cost">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPanelCost" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12">
                                                                                                            <asp:Label ID="lblExpectedCost" Font-Bold="true" runat="server" Text="Expected Cost"></asp:Label>
                                                                                                            &nbsp:&nbsp<asp:Label ID="lblValueExpectedCost" runat="server" Text="0"></asp:Label>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                        <div class="col-md-12">
                                                                                                            <br />
                                                                                                        </div>
                                                                                                        <div class="col-md-12">
                                                                                                            <asp:Label ID="lblActualCost" Font-Bold="true" runat="server" Text="Actual Cost"></asp:Label>
                                                                                                            &nbsp:&nbsp<asp:Label ID="lblValueActualCost" runat="server" Text="0"></asp:Label>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_TabPanelCost">
                                                                                                <ProgressTemplate>
                                                                                                    <div class="modal_Progress">
                                                                                                        <div class="center_Progress">
                                                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ProgressTemplate>
                                                                                            </asp:UpdateProgress>
                                                                                        </ContentTemplate>
                                                                                    </cc1:TabPanel>
                                                                                    <cc1:TabPanel ID="TabPanelPurchaseDetail" Visible="false" runat="server" Width="100%" HeaderText="Purchase Detail">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPanelPurchaseDetail" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseDetail" runat="server" Width="100%">

                                                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Update_TabPanelPurchaseDetail">
                                                                                                <ProgressTemplate>
                                                                                                    <div class="modal_Progress">
                                                                                                        <div class="center_Progress">
                                                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ProgressTemplate>
                                                                                            </asp:UpdateProgress>
                                                                                        </ContentTemplate>
                                                                                    </cc1:TabPanel>
                                                                                </cc1:TabContainer>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-12" style="text-align: left">

                                                        <asp:Button ID="btnsave" runat="server" Visible="false" CssClass="btn btn-success"
                                                            OnClick="btnsave_Click" Text="<%$ Resources:Attendance,Save%>" />&nbsp;&nbsp;
                                                        <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                                            Text="<%$ Resources:Attendance,Reset%>" />&nbsp;&nbsp;
                                                        <asp:Button ID="btncencel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            OnClick="btncencel_Click" CssClass="btn btn-danger" />

                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkMultipledemo" runat="server" Text="<%$ Resources:Attendance,Is Multiple Demo%>"
                                                            CssClass="form-control" Visible="false" />
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
                    <div class="tab-pane" id="Close">
                        <asp:UpdatePanel ID="Update_Close" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Project Name %>"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlCloseprojectName" runat="server" Enabled="true" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCloseprojectName_OnSelectedIndexChanged"
                                                            DropDownWidth="719" DropDownStyle="DropDownList" ValueField="Project_Id"
                                                            ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                            CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Project_Name" Caption="Project Name" />
                                                                <dx:ListBoxColumn FieldName="FormatedStartdate" Caption="Start date" />
                                                                <dx:ListBoxColumn FieldName="FormatedExpEnddate" Caption="Expected end date" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,End Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtenddate" ErrorMessage="<%$ Resources:Attendance,Enter End Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtenddate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtenddate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="trstatus" runat="server" visible="false">
                                                        <asp:Label ID="Label28" runat="server" Text="Status"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                            <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">

                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ErrorMessage="<%$ Resources:Attendance,Enter Close Remarks %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Close Remarks" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label46" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true"
                                                                                runat="server" Text="Customer Feedback"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12">
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTestimonial" ErrorMessage="<%$ Resources:Attendance,Enter Testimonial%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtTestimonial" runat="server" CssClass="form-control" placeholder="Testimonial"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">

                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAboutservices" ErrorMessage="<%$ Resources:Attendance,Enter About Services %>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtAboutservices" runat="server" CssClass="form-control" placeholder="About Services"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">

                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSysBenefit" ErrorMessage="<%$ Resources:Attendance,Enter System benefits%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtSysBenefit" runat="server" CssClass="form-control" placeholder="System benefits"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnProjectClose" runat="server" Visible="false" CssClass="btn btn-primary"
                                                            OnClick="btnProjectClose_Click" Text="<%$ Resources:Attendance,Close%>" />
                                                        <cc1:ConfirmButtonExtender ID="Confirm_btnProjectClose" runat="server" ConfirmText="Are you sure to close the project ?"
                                                            TargetControlID="btnProjectClose">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnProjectReset" runat="server" CssClass="btn btn-primary" OnClick="btnProjectReset_Click"
                                                            Text="<%$ Resources:Attendance,Reset%>" />

                                                        <asp:Button ID="btnCloseProjectCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCloseProjectCancel_Click"
                                                            Text="<%$ Resources:Attendance,Cancel%>" />
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
                    <div class="tab-pane" id="Sales_Order">
                        <asp:UpdatePanel ID="Update_Sales_Order" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Sales_Order" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblOrderTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="form-group">

                                                            <div class="col-lg-3">
                                                                <asp:DropDownList ID="ddlOrderFieldName" runat="server" CssClass="form-control"
                                                                    OnSelectedIndexChanged="ddlOrderFieldName_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Location Name %>" Value="Location_Name" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Reference Type %>" Value="TransType"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Ref No. %>" Value="QauotationNo"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Payment Mode %>" Value="PaymentModeName"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlOrderOption" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-5">
                                                                <asp:Panel ID="Panel" runat="server" DefaultButton="btnOrderbind">
                                                                    <asp:TextBox ID="txtOrderValue" runat="server" CssClass="form-control" placeholder="Search By Content"></asp:TextBox>
                                                                    <asp:TextBox ID="txtOrderValuedate" runat="server" CssClass="form-control" placeholder="Search By Date" Visible="false"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtOrderValuedate" />
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:LinkButton ID="btnOrderbind" runat="server" CausesValidation="False" OnClick="btnOrderbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"> <span class="fa fa-search" style="font-size:25px;"></span> </asp:LinkButton>&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnOrderRefresh" runat="server" CausesValidation="False" OnClick="btnOrderRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat" style="font-size:25px;"></span></asp:LinkButton>
                                                            </div>
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
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrder" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesOrder_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvSalesOrder_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                        CausesValidation="False" CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnproject_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>" SortExpression="Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSOLocationName" runat="server" Text='<%#Eval("Location_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reference Type%>" SortExpression="TransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("TransType") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref No.%>" SortExpression="QauotationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransNo" runat="server" Text='<%#Eval("QauotationNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode%>" SortExpression="PaymentModeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentname" runat="server" Text='<%# Eval("PaymentModeName").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblnetAmount" runat="server" Text='<%# GetCurrencySymbol(Eval("NetAmount").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
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
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Delete_Category" tabindex="-1" role="dialog" aria-labelledby="Delete_CategoryLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Delete_CategoryLabel">
                        <asp:Label ID="lblDeletePanelHeader" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance, Delete Category %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Delete_Category" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12" style="text-align: center">
                                                    <asp:Button ID="btnDeleteChild" runat="server" Text="<%$ Resources:Attendance, Delete Child %>"
                                                        CssClass="btn btn-primary" OnClick="btnDeleteChild_Click" />
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Attendance, Back %>"
                                                        CssClass="btn btn-primary" OnClick="btnBack_Click" />
                                                    <asp:Button ID="btnMoveChild" runat="server" Text="<%$ Resources:Attendance, Move Child %>"
                                                        CssClass="btn btn-primary" OnClick="btnMoveChild_Click" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Parent Category %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlMoveCategory" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Attendance, Move %>"
                                                        CssClass="btn btn-primary" OnClick="btnUpdateParent_Click" />
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Delete_Category_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Delete_Category">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Delete_Category_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Close">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Sales_Order">
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
        function resetPosition(object, args) {
            $(object._completionListElement.children).each(function () {
                var data = $(this)[0];
                if (data != null) {
                    data.style.paddingLeft = "10px";
                    data.style.cursor = "pointer";
                    data.style.borderBottom = "solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";

            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_Sales_Active() {
            $("#Li_Sales_Order").removeClass("active");
            $("#Sales_Order").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Tab_Sales_Order() {
            document.getElementById('<%= Btn_Sales_Order.ClientID %>').click();
        }

        function Delete_Category_Popup() {
            document.getElementById('<%= Btn_Delete_Category.ClientID %>').click();
        }
        function txtDDlProjectName_changed(ctrl) {
            PageMethods.txtDDlProjectName_changed(ctrl.value, successProject, errorProject);
        }
        function successProject(data) {
            <%--var hdnProjectNameValue = document.getElementById('<%= hdnProjectNameValue.ClientID %>');--%>
            if (data != "") {
                hdnProjectNameValue.value = data;
            }
            else {
                hdnProjectNameValue.value = "";
                showAlert("Please Select From Suggesstions", 'orange', 'white');
                return;
            }
        }
        function errorProject(data) {
            alert(data);
        }
    </script>
    <script type="text/javascript">

        function FUAll_UploadStarted(sender, args) {

        }
        function projectNameLnkBtnClick(projectId) {
            window.open('../Dashboard/Individual_Project_Dashboard.aspx?Project_Id=' + projectId + '');
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
    <script src="../Script/customer.js"></script>
    <script src="../Script/common.js"></script>
</asp:Content>
