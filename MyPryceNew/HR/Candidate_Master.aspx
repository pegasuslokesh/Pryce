<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Candidate_Master.aspx.cs" Inherits="HR_Candidate_Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-shield"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Candidate Master %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Candidate Master%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Candidate Master%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Candidate Master%>"></asp:Label></li>
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
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Candidate Name %>" Value="Candidate_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Mobile_No1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id1"></asp:ListItem>
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
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbind_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= Gv_Candi_Master_List.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Candi_Master_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Candidate_Id"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Candi_Master_List_PageIndexChanging" OnSorting="Gv_Candi_Master_List_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_All" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Candidate_Id") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IBtn_Delete" runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Candidate_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Candidate Name%>" SortExpression="Candidate_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Candidate_Name_List" runat="server" Text='<%# Eval("Candidate_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Birth%>" SortExpression="Dob">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_DateofBirth_List" runat="server" Text='<%#GetDate(Eval("Dob"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mobile No%>" SortExpression="Mobile_No1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Mobile_No1_List" runat="server" Text='<%# Eval("Mobile_No1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id%>" SortExpression="Email_Id1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Email_Id1_List" runat="server" Text='<%# Eval("Email_Id1") %>'></asp:Label>
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
                                    <asp:HiddenField ID="ServerDateTime" runat="server" />
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_CandidateName" runat="server" Text="<%$ Resources:Attendance,Candidate Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_CandidateName" ErrorMessage="<%$ Resources:Attendance,Enter Candidate Name%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_CandidateName" runat="server" MaxLength="200" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Candidate_Local" runat="server" Text="<%$ Resources:Attendance,Candidate Name (Local)%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_Candidate_Local" runat="server" MaxLength="200" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Father" runat="server" Text="<%$ Resources:Attendance,Father/Husband Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Father" ErrorMessage="<%$ Resources:Attendance,Enter Father/Husband Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Father" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Gender" runat="server" Text="<%$ Resources:Attendance,Gender%>"></asp:Label>
                                                        <asp:DropDownList ID="DDL_Gender" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="Male" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Dob" runat="server" Text="<%$ Resources:Attendance,Date of Birth%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Dob" ErrorMessage="<%$ Resources:Attendance,Enter Date of Birth %>"></asp:RequiredFieldValidator>
                                                        <%--<div class="input-group">--%>
                                                        <asp:TextBox ID="Txt_Dob" runat="server" onChange="Check_DOB()" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="Txt_Dob" ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$" ErrorMessage="Please Enter a valid date in the format (dd-MMM-yyyy)." ValidationGroup="Save" />
                                                        <cc1:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" Enabled="True" TargetControlID="Txt_Dob"></cc1:CalendarExtender>
                                                        <%--<div class="input-group-btn">
                                                                <asp:Button ID="Btn_Clear_Date" OnClientClick="Clear_DOB()" Cssclass="btn btn-danger" runat="server" Style="background-image: url(../Images/Erase.png); background-size: 20px; background-position-x: center; background-position-y: center; background-repeat: no-repeat" ToolTip="<%$ Resources:Attendance,Clear %>" />
                                                            </div>--%>
                                                        <%--</div>--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_PermanentAddress" runat="server" Text="<%$ Resources:Attendance,Permanent Address%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_PermanentAddress" ErrorMessage="<%$ Resources:Attendance,Enter Permanent Address %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_PermanentAddress" Style="resize: none; height: 60px;" MaxLength="1000" OnChange="Chk_Address_Checked()" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="Chk_Address_Check" runat="server" Text="<%$ Resources:Attendance,Same As Permanent Address%>" OnChange="Chk_Address_Checked()" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_CorrespondenceAddress" runat="server" Text="<%$ Resources:Attendance,Correspondence Address%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_CorrespondenceAddress" ErrorMessage="<%$ Resources:Attendance,Enter Correspondence Address %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_CorrespondenceAddress" Style="resize: none; height: 60px;" MaxLength="1000" OnChange="Chk_Address_Checked()" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Mobile" runat="server" Text="<%$ Resources:Attendance,Mobile No%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_MobileNo_1" ErrorMessage="<%$ Resources:Attendance,Enter Mobile No %>"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="DDL_CountryId_1" InitialValue="--Country--" ErrorMessage="<%$ Resources:Attendance,Select Country %>" />
                                                        <div class="input-group">
                                                            <div class="input-group-btn">
                                                                <asp:DropDownList ID="DDL_CountryId_1" runat="server" onChange="Country_Change()" Width="120" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                            <asp:TextBox ID="Txt_MobileNo_1" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_MobileNo_1" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_MobileNoAlternate" runat="server" Text="<%$ Resources:Attendance,Alternate Mobile No%>"></asp:Label>
                                                        <div class="input-group">
                                                            <div class="input-group-btn">
                                                                <asp:DropDownList ID="DDL_CountryId_2" Width="120" Enabled="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                            <asp:TextBox ID="Txt_MobileNo_2" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_MobileNo_2" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Email" runat="server" Text="<%$ Resources:Attendance,Email Id%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Email_1" ErrorMessage="<%$ Resources:Attendance,Enter Email Id %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Email_1" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please Enter Valid Email ID" ValidationGroup="Save" ControlToValidate="Txt_Email_1" CssClass="requiredFieldValidateStyle" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"> </asp:RegularExpressionValidator>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_AlternateEmail" runat="server" Text="<%$ Resources:Attendance,Alternate Email Id%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_Email_2" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter Valid Email ID" ValidationGroup="Save" ControlToValidate="Txt_Email_2" CssClass="requiredFieldValidateStyle" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"> </asp:RegularExpressionValidator>
                                                        <asp:CompareValidator runat="server" ID="cmpNumbers" ControlToValidate="Txt_Email_2" ControlToCompare="Txt_Email_1" Operator="NotEqual" ErrorMessage="Alternate Email Id must be different from Email Id!" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_Key_Skill" runat="server" Text="<%$ Resources:Attendance, Key Skill%>"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="Txt_Key_Skill" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters="," ShowOnlyCurrentWordInCompletionListItem="true"
                                                                Enabled="True" ServiceMethod="Get_Skill" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Key_Skill" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="Btn_Add_Qualification" runat="server" data-toggle="modal" data-target="#QualificationModal" Text="Add Qualification" CssClass="btn btn-info" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQualification" runat="server" Width="100%" AutoGenerateColumns="False">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Qualification_Edit" runat="server" CommandArgument='<%# Eval("QualficationId") %>'
                                                                            ImageUrl="~/Images/edit.png" Width="16px" Visible="true" OnCommand="Btn_Qualification_Edit_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Qualification_Delete" runat="server" CommandArgument='<%# Eval("QualficationId") %>'
                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" Visible="true" OnCommand="Btn_Qualification_Delete_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qualification">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_Qualification_Name" runat="server" Text='<%# Eval("QualficationName") %>' />
                                                                        <asp:HiddenField ID="Hdn_Lbl_Qualification_Name" runat="server" Value='<%# Eval("QualficationId") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="School/College Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_College_Name" runat="server" Text='<%# Eval("CollegeName") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Board/University Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_University_Name" runat="server" Text='<%# Eval("BoardName") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Grade">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_Grade" runat="server" Text='<%# Eval("Grade") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            

                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_MaritalStatus" runat="server" Text="<%$ Resources:Attendance,Marital Status%>"></asp:Label>
                                                        <asp:DropDownList ID="DDL_Marrital_Status" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="Un Married" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Married" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_IsFresher" runat="server" Text="<%$ Resources:Attendance,Candidate Type%>"></asp:Label>
                                                        <asp:DropDownList ID="DDL_IsFresher" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_IsFresher_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Text="Fresher" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Experience" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="Div_Fresher" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Lbl_CurrentCompany" runat="server" Text="<%$ Resources:Attendance,Current Company%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="Req_CurrentCompany" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_CurrentCompany" ErrorMessage="<%$ Resources:Attendance,Enter Current Company%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="Txt_CurrentCompany" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbl_TotalExperince" runat="server" Text="<%$ Resources:Attendance,Total Experience%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="Req_TotalExperince" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_TotalExperince" ErrorMessage="<%$ Resources:Attendance,Enter Total Experience%>"></asp:RequiredFieldValidator>

                                                            <div class="input-group">
                                                                <asp:TextBox ID="Txt_TotalExperince" runat="server" MaxLength="2" CssClass="form-control" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="Txt_TotalExperince" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,In Year%>" CssClass="form-control"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbl_CTC" runat="server" Text="<%$ Resources:Attendance,Current Salary%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="Req_CurrentCTC" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_CurrentCTC" ErrorMessage="<%$ Resources:Attendance,Enter Current Salary%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="Txt_CurrentCTC" onchange="Onchange_Salary(this.id)" onkeypress="return AllowOnlyAmountAndDot(this);" MaxLength="7" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Source" runat="server" Text="<%$ Resources:Attendance,Source%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="DDL_Source" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Source %>" />
                                                        <asp:DropDownList ID="DDL_Source" AutoPostBack="true" OnSelectedIndexChanged="DDL_Source_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Direct" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="News-Paper" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Employee Reference" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Third Party" Value="4"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="Div_Ref_Third_Party" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Lbl_thirdParty" runat="server" Text="<%$ Resources:Attendance,Third Party Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="Req_Third_Party" ValidationGroup="Not_Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Third_Party" ErrorMessage="<%$ Resources:Attendance,Enter Third Party Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Third_Party" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div id="Div_Ref_Employee" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Lbl_CompRefEmp" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="Req_CompRefEmp" ValidationGroup="Not_Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Reference_Emp" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Reference_Emp" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="Txt_Reference_Emp_TextChanged" BackColor="#c2c2c2" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Get_Emp_Name_List" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Reference_Emp"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Reference_1" runat="server" Text="<%$ Resources:Attendance,Reference 1%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_Reference_1" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Reference_2" runat="server" Text="<%$ Resources:Attendance,Reference 2%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_Reference_2" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_IsEmpEmail" runat="server" Text="<%$ Resources:Attendance,Email Alert%>"></asp:Label>
                                                        <asp:CheckBox ID="Chk_Email" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_IsSms" runat="server" Text="<%$ Resources:Attendance,Sms Alert%>"></asp:Label>
                                                        <asp:CheckBox ID="Chk_Sms" runat="server" />
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
                                                    <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Candidate Name %>" Value="Candidate_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Mobile_No1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id1"></asp:ListItem>
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
                                                    <asp:TextBox ID="txtValueBin" onkeypress="return Accept_Enter_Key_Bin(this);" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" 
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click" CausesValidation="true"
                                                        runat="server" ToolTip="<%$ Resources:Attendance, Active All %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <cc1:ConfirmButtonExtender ID="Active_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                        TargetControlID="Img_Emp_List_Active">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= Gv_Candi_Master_Bin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Candi_Master_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Candidate_Id" OnPageIndexChanging="Gv_Candi_Master_Bin_PageIndexChanging" OnSorting="Gv_Candi_Master_Bin_Sorting"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_All_Bin" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_Bin_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_Bin" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Active %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IBtn_Active" Visible="false" runat="server" OnCommand="IBtn_Active_Command" CausesValidation="False" CommandArgument='<%# Eval("Candidate_Id") %>' ImageUrl="~/Images/Active.png" Width="16px" ToolTip="<%$ Resources:Attendance,Active %>" />
                                                                    <cc1:ConfirmButtonExtender ID="Active_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                                        TargetControlID="IBtn_Active">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Candidate Name%>" SortExpression="Candidate_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Candidate_Name_Bin" runat="server" Text='<%# Eval("Candidate_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Birth%>" SortExpression="Dob">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_DateofBirth_Bin" runat="server" Text='<%#GetDate(Eval("Dob"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mobile No%>" SortExpression="Mobile_No1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Mobile_No1_Bin" runat="server" Text='<%# Eval("Mobile_No1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id%>" SortExpression="Email_Id1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Email_Id1_Bin" runat="server" Text='<%# Eval("Email_Id1") %>'></asp:Label>
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

    <div class="modal fade" id="QualificationModal" tabindex="-1" role="dialog" aria-labelledby="QualificationModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="QualificationModalLabel">
                        <asp:Label ID="lblProductHeader" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance,Qualification Setup %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label9" runat="server" Text="Qualification" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="Save_Qualification" Display="Dynamic"
                                                        SetFocusOnError="true" ControlToValidate="DDL_Qualification" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Brand %>" />
                                                    <asp:DropDownList ID="DDL_Qualification" runat="server" CssClass="form-control" />
                                                    <asp:HiddenField ID="hdnQualificationId" runat="server" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Lbl_ProductName" runat="server" Text="School/College Name" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save_Qualification"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_SchoolName" ErrorMessage="<%$ Resources:Attendance,Enter School/College Name%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="Txt_SchoolName" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblUnit" runat="server" Text="Board/University Name" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Save_Qualification"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_BoardName" ErrorMessage="<%$ Resources:Attendance,Enter Board/University Name%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="Txt_BoardName" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblRequestQty" runat="server" Text="Grade" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Save_Qualification"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Grade" ErrorMessage="<%$ Resources:Attendance,Enter Grade%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="Txt_Grade" runat="server" CssClass="form-control" />
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Btn_Qualification_Save" ValidationGroup="Save_Qualification" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success" OnClick="Btn_Qualification_Save_Click" />

                            <asp:Button ID="Btn_Qualification_Reset" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>" CausesValidation="False" OnClick="Btn_Qualification_Reset_Click" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                            <%--<asp:Button ID="btnQualificationClose" runat="server" CssClass="buttonCommman" Text="<%$ Resources:Attendance,Close %>" CausesValidation="False" OnClick="btnQualificationClose_Click" />--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
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
        function Qualification_Modal_Close() {
            document.getElementById('<%= Btn_Add_Qualification.ClientID %>').click();
        }
        function Chk_Address_Checked() {
            var Chk_box = document.getElementById('<%= Chk_Address_Check.ClientID %>');
            if (Chk_box.checked) {
                document.getElementById('<%= Txt_CorrespondenceAddress.ClientID %>').value = document.getElementById('<%= Txt_PermanentAddress.ClientID %>').value;
            }
            else {
                document.getElementById('<%= Txt_CorrespondenceAddress.ClientID %>').value = document.getElementById('<%= Txt_CorrespondenceAddress.ClientID %>').value;
            }
        }
        function Country_Change() {
            var e = document.getElementById('<%= DDL_CountryId_1.ClientID %>');
            var Country = e.options[e.selectedIndex].value;
            document.getElementById('<%= DDL_CountryId_2.ClientID %>').value = Country;
        }
        <%--function Clear_DOB() {
            document.getElementById('<%= Txt_Dob.ClientID %>').value = "";
        }--%>
        function Check_DOB() {
            var DOB_Date = new Date(document.getElementById('<%= Txt_Dob.ClientID %>').value);
            var ServerDateTime = new Date(document.getElementById('<%= ServerDateTime.ClientID %>').value);
            var Age_Date = new Date(document.getElementById('<%= ServerDateTime.ClientID %>').value);
            Age_Date.setFullYear(Age_Date.getFullYear() - 20);
            if (DOB_Date > ServerDateTime) {
                document.getElementById('<%= Txt_Dob.ClientID %>').value = '';
                alert('Date cannot be greater than current date');
                return false;
            }
            else {
                if (DOB_Date > Age_Date) {
                    document.getElementById('<%= Txt_Dob.ClientID %>').value = '';
                    alert('You cannot select a date. Because your age is below 20 year!');
                    return false;
                }
            }
            return true;
        }

        function Accept_Enter_Key_List(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered == 13)) {
                document.getElementById('<%= btnbind.ClientID %>').click();
                return false;
            }
        }
        function Accept_Enter_Key_Bin(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered == 13)) {
                document.getElementById('<%= btnbindBin.ClientID %>').click();
                return false;
            }
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
            document.getElementById('<%= Txt_CurrentCTC.ClientID %>').value = A_Salary;
        }
    </script>
</asp:Content>
