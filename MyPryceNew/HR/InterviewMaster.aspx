<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="InterviewMaster.aspx.cs" Inherits="HR_InterviewMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-comments"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Interview Master %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Interview Master%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Interview Master%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Interview Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List_Li" Style="display: none;" runat="server" OnClick="Btn_List_Li_Click" Text="List" />
            <asp:Button ID="Btn_New_Li" Style="display: none;" runat="server" OnClick="Btn_New_Li_Click" Text="New" />
            <asp:Button ID="Btn_Bin_Li" Style="display: none;" runat="server" OnClick="Btn_Bin_Li_Click" Text="Bin" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Lbl_TotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Candidate Name%>" Value="Candidate_Name"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Interview Id%>" Value="Interview_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Vacancy Name%>" Value="Profile_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:TextBox ID="txtValue" onkeypress="return Accept_Enter_Key_List(this);" runat="server" class="form-control" placeholder="Search from Content"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbind_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Interview_Master_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Interview_Id"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Interview_Master_List_PageIndexChanging" OnSorting="Gv_Interview_Master_List_Sorting">

                                                        <Columns>
                                                            <%--<asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_All" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Interview_Id") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IBtn_Delete" runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Interview_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interview Id%>" SortExpression="Interview_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Interview_Id_List" runat="server" Text='<%# Eval("Interview_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Candidate Name%>" SortExpression="Candidate_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_CandidateName_List" runat="server" Text='<%# Eval("Candidate_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Profile Name%>" SortExpression="Profile_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Profile_Name_List" runat="server" Text='<%# Eval("Profile_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interview Date%>" SortExpression="Interview_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_InterviewDate_List" runat="server" Text='<%# GetDate(Eval("Interview_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Salary %>" SortExpression="Expected_Salary">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Expected_Salary_List" runat="server" Text='<%# Eval("Expected_Salary") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Selected %>" SortExpression="Is_Selected">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Is_Selected_List" runat="server" Text='<%# Eval("Is_Selected") %>'></asp:Label>
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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div>
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                    <asp:HiddenField ID="Hdn_Candidate_ID" runat="server" />
                                    <asp:HiddenField ID="Hdn_Interview_ID" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_CandidateName" runat="server" Text="<%$ Resources:Attendance,Candidate Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_CandidateName" ErrorMessage="<%$ Resources:Attendance,Enter Candidate Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="Txt_CandidateName" BackColor="#eeeeee" AutoPostBack="true"
                                                            OnTextChanged="Txt_CandidateName_TextChanged" runat="server" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="txtSkillName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCandidateName" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_CandidateName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Vacancy" runat="server" Text="<%$ Resources:Attendance,Vacancy Name%>"></asp:Label>
                                                        <%--<a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="DDL_Profile" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Vacancy Name %>" />--%>
                                                        <asp:DropDownList ID="DDL_Profile" CssClass="form-control" OnSelectedIndexChanged="DDL_Profile_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_InterviewName" runat="server" Text="<%$ Resources:Attendance,Interviewer Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_InterviewerName" ErrorMessage="<%$ Resources:Attendance,Enter Interviewer Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_InterviewerName" BackColor="#eeeeee" AutoPostBack="true"
                                                            OnTextChanged="Txt_InterviewerName_TextChanged" runat="server" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetInterviewerName" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_InterviewerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_InterviewDate" runat="server" Text="<%$ Resources:Attendance,Interview Date%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_InterviewDate" ErrorMessage="<%$ Resources:Attendance,Enter Interview Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_InterviewDate" onPaste="return false" OnTextChanged="Txt_InterviewDate_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control" />
                                                        <asp:RegularExpressionValidator runat="server" onPaste="return false" ControlToValidate="Txt_InterviewDate" ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$" ErrorMessage="Please Enter a valid date in the format (dd-MMM-yyyy)." ValidationGroup="Save" />
                                                        <cc1:CalendarExtender ID="txtInterviewDate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="Txt_InterviewDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_ExpectedSalary" runat="server" Text="<%$ Resources:Attendance,Expected Salary%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_ExpectedSalary" ErrorMessage="<%$ Resources:Attendance,Enter Expected Salary%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_ExpectedSalary" onPaste="return false" onchange="Onchange_Salary(this.id)" onkeypress="return AllowOnlyAmountAndDot(this);" MaxLength="7" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_NoticePeriod" runat="server" Text="<%$ Resources:Attendance,Notice Period (Month)%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_NoticePeriod" runat="server" onPaste="return false" onkeypress="return AllowOnlyAmountAndDot(this);" MaxLength="3" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_Title" runat="server" Text="<%$ Resources:Attendance,Title%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Title" ErrorMessage="<%$ Resources:Attendance,Enter Title%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Title" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_Feedback" runat="server" Text="<%$ Resources:Attendance,Feedback%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Feedback" ErrorMessage="<%$ Resources:Attendance,Enter Feedback%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Feedback" runat="server" CssClass="form-control"
                                                            TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Communication" runat="server" Text="<%$ Resources:Attendance,Communication Skill%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="DDL_Communication" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Communication Skill (Marks)%>" />
                                                        <asp:DropDownList ID="DDL_Communication" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_SubjectKnowledge" runat="server" Text="<%$ Resources:Attendance,Subject Knowledge%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="DDL_SubjectKnowledge" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Subject Knowledge (Marks)%>" />
                                                        <asp:DropDownList ID="DDL_SubjectKnowledge" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_IsSelected" runat="server" Text="<%$ Resources:Attendance,Is Selected%>"></asp:Label>
                                                        <asp:CheckBox ID="Chk_IsSelect" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Lbl_IsEmployee" runat="server" Text="<%$ Resources:Attendance,Is Employee%>"></asp:Label>
                                                        <asp:CheckBox ID="Chk_IsEmployee" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" OnClick="Btn_Save_Click" Text="<%$ Resources:Attendance,Save %>" class="btn btn-success" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" class="btn btn-danger" OnClick="Btn_Cancel_Click" CausesValidation="False" />
                                                        <asp:Button ID="Btn_Reset" runat="server" Text="<%$ Resources:Attendance,Reset %>" class="btn btn-primary" OnClick="Btn_Reset_Click" CausesValidation="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Lbl_TotalRecords_Bin" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Candidate Name%>" Value="Candidate_Name"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Interview Id%>" Value="Interview_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Vacancy Name%>" Value="Profile_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:TextBox ID="txtValueBin" onkeypress="return Accept_Enter_Key_Bin(this);" runat="server" class="form-control" placeholder="Search from Content"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" 
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" CausesValidation="False" runat="server"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click" CausesValidation="true"
                                                        runat="server" ToolTip="<%$ Resources:Attendance, Active All %>"> <span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <cc1:ConfirmButtonExtender ID="Active_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                        TargetControlID="Img_Emp_List_Active">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= Gv_Interview_Master_Bin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Interview_Master_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Interview_Id" OnPageIndexChanging="Gv_Interview_Master_Bin_PageIndexChanging" OnSorting="Gv_Interview_Master_Bin_Sorting"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <%--<asp:CheckBox ID="Chk_Gv_Select_All_Bin" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_Bin_CheckedChanged" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_Bin" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Active %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IBtn_Active" Visible="false" runat="server" OnCommand="IBtn_Active_Command" CausesValidation="False" CommandArgument='<%# Eval("Interview_Id") %>' ImageUrl="~/Images/Active.png" Width="16px" ToolTip="<%$ Resources:Attendance,Active %>" />
                                                                    <cc1:ConfirmButtonExtender ID="Active_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                                        TargetControlID="IBtn_Active">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interview Id%>" SortExpression="Interview_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Interview_Id_Bin" runat="server" Text='<%# Eval("Interview_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Candidate Name%>" SortExpression="Candidate_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_CandidateName_Bin" runat="server" Text='<%# Eval("Candidate_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Profile Name%>" SortExpression="Profile_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Profile_Name_Bin" runat="server" Text='<%# Eval("Profile_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interview Date%>" SortExpression="Interview_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_InterviewDate_Bin" runat="server" Text='<%# GetDate(Eval("Interview_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Salary %>" SortExpression="Expected_Salary">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Expected_Salary_Bin" runat="server" Text='<%# Eval("Expected_Salary") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Selected %>" SortExpression="Is_Selected">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Is_Selected_Bin" runat="server" Text='<%# Eval("Is_Selected") %>'></asp:Label>
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List_Li.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New_Li.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin_Li.ClientID %>').click();
        }

        function AllowOnlyAmountAndDot(elementRef) {
            var specialKeys = new Array();
            specialKeys.push(8);
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {
                return true;
            }
            else if (keyCodeEntered == 46) {
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }
            return false;
        }

        function Onchange_Salary(id) {
            var salary = document.getElementById(id).value;
            var A_Salary = parseFloat(Math.round(salary * 100) / 100).toFixed(2);
            document.getElementById('<%= Txt_ExpectedSalary.ClientID %>').value = A_Salary;
        }
    </script>
</asp:Content>
