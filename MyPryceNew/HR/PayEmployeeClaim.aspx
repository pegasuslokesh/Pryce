<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="PayEmployeeClaim.aspx.cs" Inherits="HR_PayEmployeeClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="fas fa-file-contract"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Claim%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Claim%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnLeast_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnClaim_Click" Text="New" />
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

    <div id="myTab" class="nav-tabs-custom">
        <ul class="nav nav-tabs pull-right bg-blue-gradient">
            <li id="Li_Claim"><a onclick="Li_Tab_New()" href="#Claim" data-toggle="tab">
                <asp:UpdatePanel ID="Update_Li" runat="server">
                    <ContentTemplate>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_New_tab" runat="server" Text="<%$ Resources:Attendance,Claim%>"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </a></li>
            <li class="active" id="Li_List"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                <i class="fa fa-list"></i>&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
        </ul>
        <div class="tab-content">
            <div class="tab-pane" id="Claim">
                <asp:UpdatePanel ID="Update_Claim" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div style="text-align: center;" class="form-control">
                                    <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server" Style="margin-left: 15px; margin-right: 15px;" Text="<%$ Resources:Attendance,Group %>" GroupName="EmpGroup" AutoPostBack="true" />
                                    <asp:RadioButton ID="rbtnEmp" Style="margin-left: 15px; margin-right: 15px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                </div>
                            </div>
                        </div>
                        <div visible="false" runat="server" id="Div_Group" class="row">
                            <div class="col-md-12">
                                <div class="box box-warning">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Group</h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-4">
                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                                ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                                ForeColor="Red"></asp:CustomValidator>
                                            <asp:ListBox ID="lbxGroup" runat="server" Height="211px" SelectionMode="Multiple"
                                                AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="form-control"></asp:ListBox>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="flow">
                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmp1_PageIndexChanging" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                    SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                    SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date Of Joining %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
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
                        <div id="Div_Employee" runat="server" class="row">
                            <div class="col-md-12">
                                <div class="box box-warning">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Employee</h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                                OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                            <div class="input-group">
                                                <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true"
                                                    OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <div class="input-group-btn">
                                                    <asp:LinkButton ID="ImgbtnAllRefresh"
                                                        runat="server" CausesValidation="False"
                                                        OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px; margin-left:10px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">


                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title">
                                                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsClaim" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                            <div class="box-tools pull-right">
                                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="col-lg-3">
                                                                <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlOption1" runat="server" class="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-5">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnClaimBind">
                                                                    <asp:TextBox ID="txtValue1" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2" style="text-align: center;">
                                                                <asp:LinkButton ID="imgBtnClaimBind"
                                                                    runat="server" CausesValidation="False" OnClick="btnbind_Click"
                                                                    ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                                OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton6" runat="server" OnClick="ImgbtnSelectAll_ClickClaim"
                                                                ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                            </div>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="box box-warning box-solid"  <%= gvEmployeeClaim.Rows.Count>0?"style='display:block'":"style='display:none'"%> >

                                                <div class="box-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="flow">
                                                                <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Save" runat="server"
                                                                    ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                                                    ForeColor="Red"></asp:CustomValidator>
                                                                <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeClaim" runat="server" AllowSorting="true" AllowPaging="True"
                                                                    AutoGenerateColumns="False" OnPageIndexChanging="gvEmployeeClaim_PageIndexChanging"
                                                                    OnSorting="gvEmployeeClaim_Sorting" Width="100%" DataKeyNames="Emp_Id"
                                                                    PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' Visible="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedClaim"
                                                                                    AutoPostBack="true" />
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                            SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                            SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>" HeaderStyle-HorizontalAlign="Center"
                                                                            SortExpression="Doj">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    
                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdFSortgvEmpClaim" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-warning">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Details</h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblAllowDeductionType" runat="server" Text="<%$ Resources:Attendance,Claim Name%>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="TxtClaimName" ErrorMessage="<%$ Resources:Attendance,Enter Claim Name %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="TxtClaimName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                    OnTextChanged="TxtClaimName_textChanged" AutoPostBack="true"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetClaimName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="TxtClaimName"
                                                    UseContextKey="True"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblClaimDiscription" runat="server" Text="<%$ Resources:Attendance,Claim Description %>"></asp:Label>
                                                <asp:TextBox ID="TxtClaimDiscription" runat="server" Height="100px" TextMode="MultiLine"
                                                    CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblPaidAllowDeduction" runat="server" Text="<%$ Resources:Attendance,Value Type %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic"
                                                    SetFocusOnError="true" ControlToValidate="DdlValueType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Value Type %>" />
                                                <asp:DropDownList ID="DdlValueType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>" Value="0" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Fixed %>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Percentage %>" Value="2" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblPerOfSal" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtCalValue" ErrorMessage="<%$ Resources:Attendance,Enter Value %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtCalValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtCalValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblMonth" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic"
                                                    SetFocusOnError="true" ControlToValidate="ddlMonth" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />

                                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblYear" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="TxtYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="TxtYear" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                    TargetControlID="TxtYear" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div style="text-align: center;" class="col-md-12">
                                            <asp:HiddenField ID="Hdn_btn_click" runat="server" />
                                            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" CssClass="btn btn-success"
                                                Visible="false" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save"
                                                OnClick="btnSaveClaim_Click" />
                                            <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                Text="<%$ Resources:Attendance,Reset %>" OnClick="btnReset_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel_Click" />
                                            <asp:Label ID="lblWrongSequence" Visible="false" runat="server"
                                                Font-Bold="true" ForeColor="Red"></asp:Label><br />
                                            <asp:Label ID="lblDojIssue" Visible="false" runat="server"
                                                Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane active" id="List">
                <asp:UpdatePanel ID="Update_List" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-primary">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblApplicationName" runat="server" Text="<%$ Resources:Attendance,Month %>" />
                                                <asp:DropDownList ID="DdlMonthList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlMonthList_SelectedIndexChanged" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblApplicationNameL" runat="server" Text="<%$ Resources:Attendance,Year %>" />
                                                <div class="input-group">
                                                    <asp:TextBox ID="TxtYearList" runat="server" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        TargetControlID="TxtYearList" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <div class="input-group-btn">
                                                        <asp:LinkButton ID="BtnBindList" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"
                                                            OnClick="BtnBindList_Click" ><span class="fa fa-search"  style="font-size:25px; margin-left:5px;"></span></asp:LinkButton>
                                                    </div>
                                                    <div class="input-group-btn">
                                                        <asp:LinkButton ID="BtnRefreshList"  runat="server" CausesValidation="False"  ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                            OnClick="BtnRefreshList_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label13" runat="server" Text="Advance Search"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordSearchPanel" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">

                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlField1SearchPanel" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Id %>" Value="Emp_Id"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:DropDownList ID="ddlOption1SearchPanel" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-5">
                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                <asp:TextBox ID="txtValue1SearchPanel" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="btnbindBin" runat="server"
                                                CausesValidation="False" OnClick="btnClaimbindSearchPanel_Click"
                                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnRefreshBin_" runat="server" CausesValidation="False"
                                            OnClick="btnClaimRefreshSearchPanel_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box box-warning box-solid" <%= GridViewClaimList.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewClaimList" AllowSorting="true" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" Width="100%" DataKeyNames="Emp_Id"
                                                PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' OnPageIndexChanging="GridViewClaimList_PageIndexChanging"
                                                OnSorting="GridViewClaimList_Sorting">
                                                <Columns>


                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">
                                                                  

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                           <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Claim_Id") %>'
                                                                 CausesValidation="False" OnCommand="btnEdit_command"
                                                                ToolTip="<%$ Resources:Attendance,Edit %>" ><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                         <asp:LinkButton ID="IbtnDelete"  runat="server" CausesValidation="False"
                                                                CommandArgument='<%# Eval("Claim_Id") %>' OnCommand="IbtnDelete_command"
                                                                ToolTip="<%$ Resources:Attendance,Delete %>" ><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>

                                                                   
                                                                </ul>
                                                            </div>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id %>" SortExpression="Emp_Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpIdList" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpnameList" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name Local%>" SortExpression="Emp_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpnameList_L" runat="server" Text='<%# Eval("Emp_Name_L") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="Doj">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Claim Name %>" SortExpression="Claim_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimnameList" runat="server" Text='<%# Eval("Claim_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value Type %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblvaluetype" runat="server" Text='<%# GetType(Eval("Value_Type").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimvalue" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Value").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>

                                                
                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                            <asp:HiddenField ID="HdfSort" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div runat="server" visible="false" id="Edit_Claim" class="row">
                            <div class="col-md-12">
                                <div class="box box-warning">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Details</h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Employee Id %>"></asp:Label>
                                            <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                            <asp:TextBox ID="TxtEmployeeName" runat="server" ReadOnly="true"
                                                CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Claim Name%>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                ID="RequiredFieldValidator1" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true"
                                                ControlToValidate="TxtClaimNameList" ErrorMessage="<%$ Resources:Attendance,Enter Claim Name %>"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="TxtClaimNameList" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Claim Description %>"></asp:Label>
                                            <asp:TextBox ID="TxtClaimDiscList" runat="server" Style="width: 100%; min-width: 100%; max-width: 100%; height: 100px; min-height: 100px; max-height: 300px;" TextMode="MultiLine"
                                                CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Value Type %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="E_Save" Display="Dynamic"
                                                SetFocusOnError="true" ControlToValidate="DdlvalueTypelist" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Value Type %>" />
                                            <asp:DropDownList ID="DdlvalueTypelist" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>" Value="0" />
                                                <asp:ListItem Text="<%$ Resources:Attendance,Fixed %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, Percentage %>" Value="2" />
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                ID="RequiredFieldValidator4" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true"
                                                ControlToValidate="Txtvaluelist" ErrorMessage="<%$ Resources:Attendance,Enter Value %>"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="Txtvaluelist" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                Enabled="True" TargetControlID="Txtvaluelist" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                            </cc1:FilteredTextBoxExtender>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="E_Save" Display="Dynamic"
                                                SetFocusOnError="true" ControlToValidate="DdlMonthListPanel" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />

                                            <asp:DropDownList ID="DdlMonthListPanel" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="E_Save"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="TxtpanelYearList" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>

                                            <asp:TextBox ID="TxtpanelYearList" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                TargetControlID="TxtpanelYearList" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                            <br />
                                        </div>
                                        <div style="display: none; text-align: center;" class="col-md-12">
                                            <asp:RadioButton ID="RbtnApproved" runat="server" Style="margin-left: 15px; margin-right: 15px;" Text="<%$ Resources:Attendance,Approved %>" GroupName="Loan" />
                                            <asp:RadioButton ID="RbtnCancelled" runat="server" Style="margin-left: 15px; margin-right: 15px;" Text="<%$ Resources:Attendance,Cancelled %>" GroupName="Loan" />
                                            <br />
                                        </div>
                                        <div style="text-align: center;" class="col-md-12">
                                            <asp:Button ID="BtnUpdate" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Update %>"
                                                ValidationGroup="E_Save" OnClick="BtnUpdate_Click" />
                                            <asp:Button ID="BtnResetClaimList" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                Text="<%$ Resources:Attendance,Cancel %>" OnClick="BtnResetClaimList_Click" />
                                            <asp:HiddenField ID="hdnObjectId" runat="server" Value="0" />
                                            <asp:HiddenField ID="HiddeniD" runat="server" />
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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Claim">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlClaim" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PanelList" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnl2" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlEmployeeAllowDeduction" runat="server" Visible="false">
    </asp:Panel>
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

        function Group_Show_Employee_Hide() {
            document.getElementById("Div_Group").style.display = "";
            document.getElementById("Div_Employee").style.display = "none";
        }
        function Group_Hide_Employee_Show() {
            document.getElementById("Div_Employee").style.display = "";
            document.getElementById("Div_Group").style.display = "none";
        }
        function edit_claim_show() {
            document.getElementById("Edit_Claim").style.display = "";
        }
        function edit_claim_hide() {
            document.getElementById("Edit_Claim").style.display = "none";
        }

        function Grid_Validate(sender, args) {
            if (document.getElementById("<%=rbtnGroup.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroup.ClientID %>");
                var Select_Index = groupbox.getElementsByTagName("option");
                for (var i = 0; i < Select_Index.length; i++) {
                    if (Select_Index[i].selected) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }
            else {
                var gridView = document.getElementById("<%=gvEmployeeClaim.ClientID %>");
                var checkBoxes = gridView.getElementsByTagName("input");
                for (var i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }
        }
        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
    </script>
</asp:Content>
