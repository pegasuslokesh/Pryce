<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ApprovalMaster.aspx.cs" Inherits="MasterSetUp_ApprovalMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-thumbs-up"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Approval Setup %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Approval Master%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Approval Master%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Approval Setup%>"></asp:Label></li>
    </ol>
    <asp:HiddenField runat="server" ID="hdnCanEdit" />
    <asp:HiddenField runat="server" ID="hdnCanView" />
    <asp:HiddenField runat="server" ID="hdnCanDelete" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Form" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnNew" Text="New" runat="server" Style="display: none;" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />


            <div class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance, Approval Name %>" Value="Approval_Name" />
                                    <asp:ListItem Text="<%$ Resources:Attendance, Approval Name(Local) %>" Value="ApprovalName_L" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Approval Id %>" Value="Approval_Id"></asp:ListItem>
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
                                    <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="box box-warning box-solid" <%= GvApproval.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApproval" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                    AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvApproval_PageIndexChanging"
                                    AllowSorting="True" OnSorting="GvApproval_Sorting">

                                    <Columns>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                            <ItemTemplate>



                                                <div class="dropdown" style="position: absolute;">
                                                    <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                        <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                    </button>
                                                    <ul class="dropdown-menu">
                                                        <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                            <asp:LinkButton ID="btnView" runat="server" CommandName='<%# Eval("Approval_Name") %>' CommandArgument='<%# Eval("Approval_Id") %>' OnCommand="btnView_Command" CausesValidation="False"><i class="fa fa-eye"></i><%# Resources.Attendance.View%></asp:LinkButton>
                                                        </li>
                                                        <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName='<%# Eval("Approval_Name") %>' CommandArgument='<%# Eval("Approval_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                        </li>
                                                        <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                            <asp:LinkButton ID="lnkViewPendings" runat="server" CommandName='<%# Eval("Approval_Name") %>' CommandArgument='<%# Eval("Approval_Id") %>' OnCommand="lnkViewPendings_Command" CausesValidation="False"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i><%# Resources.Attendance.Pending %></asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approval Id %>" SortExpression="Approval_Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Approval_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Approval_Name" HeaderText="<%$ Resources:Attendance,Approval Name %>"
                                            SortExpression="Approval_Name" />
                                        <asp:BoundField DataField="ApprovalName_L" HeaderText="<%$ Resources:Attendance,Approval Name(Local) %>"
                                            SortExpression="ApprovalName_L" />
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

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="lbledit" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:UpdatePanel ID="Update_Modal_Title" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Lbl_Modal_Title" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,View_Approval_Setup %>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <Triggers>
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div style="text-align: center" class="col-md-12">
                                                    <asp:Label ID="Label3" runat="server" Style="margin-right: 15px;" Text="<%$ Resources:Attendance,Approval Name %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                        <asp:Label ID="lblHeaderApprovalName" runat="server" Style="margin-left: 15px;"></asp:Label>
                                                    <br />
                                                </div>
                                                <div style="text-align: center" class="col-md-12">
                                                    <br />
                                                    <asp:Label ID="lblBankCode" runat="server" Text="<%$ Resources:Attendance,Process Type%>"></asp:Label>

                                                    <asp:RadioButton ID="rdopriority" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Priority%>"
                                                        GroupName="Approval" OnCheckedChanged="rdoHierarchy_OnCheckedChanged" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rdoHierarchy" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Hierarchy%>"
                                                        GroupName="Approval" OnCheckedChanged="rdoHierarchy_OnCheckedChanged" AutoPostBack="true" />
                                                    <hr />
                                                </div>
                                                <div id="panelHerrachy" class="col-md-12" runat="server" visible="false">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Hierarchy Chain%>"></asp:Label>
                                                        <asp:CheckBox ID="chkTeamLeader" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Team Leader%>" />
                                                        <asp:CheckBox ID="chkDepartmentManager" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Department Manager%>" />
                                                        <asp:CheckBox ID="chkParentDepartmentManager" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Parent Department Manager%>" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Hierarchy Rules%>"></asp:Label>
                                                        <asp:RadioButton ID="rdoOpen" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Open%>" GroupName="Rules" />
                                                        <asp:RadioButton ID="rdorestricted" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Restricted%>" GroupName="Rules" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Authorized department%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtResponsibeDepartmentName" ErrorMessage="<%$ Resources:Attendance,Enter Authorized Department %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtResponsibeDepartmentName" BackColor="#eeeeee" runat="server"
                                                            CssClass="form-control" OnTextChanged="txtDepName_OnTextChanged" AutoPostBack="true" />

                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDepName" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsibeDepartmentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                        </cc1:AutoCompleteExtender>
                                                    </div>
                                                </div>
                                                <div id="panelPriority" class="col-md-12" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Emp_Add"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtResponsiblePerson" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtResponsiblePerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />

                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsiblePerson" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                        </cc1:AutoCompleteExtender>






                                                        <%-- <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsiblePerson"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:CheckBox ID="chkPriority" runat="server" Text="<%$ Resources:Attendance,Priority%>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnAddAppprovalEmployee" ValidationGroup="Emp_Add" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                            CssClass="btn btn-primary" OnClick="btnAddAppprovalEmployee_Click" />

                                                    </div>
                                                    <div style="overflow: auto" class="col-md-12">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApprovalEmployeeDetail" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" DataKeyNames="Emp_Id" ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="imgBtnEmployeeEdit" runat="server" CommandArgument='<%# Eval("Emp_id") %>' CommandName='<%# Eval("Emp_name") %>' OnCommand="imgBtnEmployeeEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Emp_id") %>' OnCommand="imgBtnEmpoloyeeDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvEmployeeCode" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%#Eval("Emp_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvEmployeeName" runat="server" Text='<%#Eval("Emp_name") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvempPriority" runat="server" Text='<%#Eval("Priority") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="editid" runat="server" />
                            <asp:Button ID="btnsaveConfig" runat="server" ValidationGroup="Save" CssClass="btn btn-success" OnClick="btnsaveConfig_Click" Text="<%$ Resources:Attendance,Save %>" Visible="false" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Form">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="panNewEdit" runat="server" defaultbutton="btnCSave" style="display: none">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-primary">
                    <div class="box-body">
                        <div class="form-group">
                            <div class="col-md-6">
                                <asp:Label ID="lblApprovalName" runat="server" Text="<%$ Resources:Attendance,Approval Name %>" />
                                <asp:TextBox ID="txtApprovalName" runat="server" Font-Names="Verdana" CssClass="form-control"
                                    BackColor="#eeeeee"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetCompletionListApproval" CompletionInterval="100"
                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtApprovalName"
                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblApprovalNameL" runat="server" Text="<%$ Resources:Attendance,Approval Name(Local) %>" />
                                <asp:TextBox ID="txtApprovalNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                <br />
                            </div>
                            <div class="col-md-12" style="text-align: center">
                                "
                                <asp:Button ID="btnCSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                    CssClass="btn btn-success" OnClick="btnCSave_Click" Visible="false" />

                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                    CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_pendings_detail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="up_pendings" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></span></button>
                            <h4 class="modal-title">
                                <asp:Label ID="lblApprovalType" runat="server" Font-Bold="true" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPendingApprovals" runat="server"
                                                            AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                            AllowSorting="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Approval Person">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvApprovalPerson" runat="server" Text='<%#Eval("approval_person") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" Width="80%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Pendings">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalPendings" runat="server" Text='<%#Eval("TotalPendings") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>

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
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="up_pendings">
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
        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }

        function Modal_Close() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }

        function View_Modal_Pendings() {
            $('#modal_pendings_detail').modal("show");
        }



    </script>
</asp:Content>
