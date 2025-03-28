<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="GenerateEmpPayroll.aspx.cs" Inherits="HR_GenerateEmpPayroll" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-hand-holding-usd"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Generate Payroll%>"></asp:Label>
    </h1>
    <asp:HiddenField runat="server" ID="hdnCanPrint" />
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Generate Payroll%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="Btn_List_Click" Text="List" />
            <asp:Button ID="Btn_Report" Style="display: none;" runat="server" OnClick="Btn_Report_Click" Text="Report" />
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
            <div class="box box-primary">
                <div class="box-body">
                    <div class="form-group">
                        <asp:UpdatePanel ID="Update_Filter" runat="server">
                            <ContentTemplate>
                                <div class="col-md-6">
                                    <asp:Label ID="lblmonth" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                        ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                        ControlToValidate="ddlMonth" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_OnSelectedIndexChanged">
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
                                    <asp:Label ID="lblyeay" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                        ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                        ControlToValidate="TxtYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                    <div class="input-group">
                                        <asp:TextBox ID="TxtYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_OnSelectedIndexChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                            TargetControlID="TxtYear" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="imgValidEmployee" runat="server" CausesValidation="False"
                                                OnClick="imgValidEmployee_Click"
                                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px; margin-left:10px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Report"><a onclick="Li_Tab_Report()" href="#Report" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_Report" runat="server" Text="<%$ Resources:Attendance,Report%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div style="text-align: center;" class="form-control">
                                                            <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server"
                                                                Text="<%$ Resources:Attendance,Group %>" GroupName="EmpGroup" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbtnEmp" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                                                Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" visible="false" id="Div_Group" class="row">
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmp1_PageIndexChanging" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>" HeaderStyle-HorizontalAlign="Center">
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
                                                            <asp:LinkButton ID="ImgbuttonallRefresh"
                                                                runat="server" CausesValidation="False" Height="30px"
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
					<asp:Label ID="lblTotalRecordsPayroll" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i id="I1" runat="server" class="fa fa-plus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div style="display: none;" class="col-md-2">
                                                                        <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,All Record %>" Value="All"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Not Generated %>" Selected="True" Value="Not Generated"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Generated %>" Value="Generated"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:DropDownList ID="ddlOption1" runat="server" class="form-control">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnPayrollBind">
                                                                            <asp:TextBox ID="txtValue1" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                                        </asp:Panel>
                                                                    </div>
                                                                    <div class="col-md-2" style="text-align: center;">
                                                                        <asp:LinkButton ID="imgBtnPayrollBind" runat="server"
                                                                            CausesValidation="False" OnClick="btnbind_Click"
                                                                            ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                    <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                    <asp:LinkButton ID="ImageButton6" runat="server" OnClick="ImgbtnSelectAll_ClickPayroll"
                                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="box box-warning box-solid" <%= gvEmpPayroll.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="flow">
                                                                        <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Save" runat="server"
                                                                            ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                                                            ForeColor="Red"></asp:CustomValidator>
                                                                        <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpPayroll" runat="server" AllowPaging="True" OnSorting="gvEmpPayroll_Sorting"
                                                                            AllowSorting="true" AutoGenerateColumns="False" OnPageIndexChanging="gvEmpPayroll_PageIndexChanging"
                                                                            Width="100%" DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedPayroll"
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
                                                                                    SortExpression="Emp_Name" ItemStyle-Width="20%">
                                                                                    <ItemStyle Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                                    SortExpression="Emp_Name_L" ItemStyle-Width="20%">
                                                                                    <ItemStyle Width="20%" />
                                                                                </asp:BoundField>
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
                                                                        <asp:HiddenField ID="hdFSortgvEmpPayroll" runat="server" />
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
                                            <%-- <div class="box-header with-border">
                                                <h3 class="box-title">Details</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>--%>
                                            <div class="box-body">
                                                <div class="col-md-6" style="display: none;">
                                                    <asp:Label ID="lblAllowDeductionType" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                    <asp:TextBox ID="txtCreditAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCreditAccount"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" style="display: none">
                                                    <asp:Label ID="lblAllowDeduc" runat="server" Text="<%$ Resources:Attendance,For Debit Account%>"></asp:Label>
                                                    <asp:TextBox ID="txtDebitAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDebitAccount"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>


                                                <div style="text-align: center;" class="col-md-12">
                                                    <asp:Button ID="btnGenratePayroll" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Attendance,Generate Payroll %>"
                                                        ValidationGroup="Save" Width="130px" OnClick="btnGenratePayroll1_Click" />
                                                    <cc1:ConfirmButtonExtender ID="CnfirmGeneratePayroll" runat="server" TargetControlID="btnGenratePayroll"
                                                        ConfirmText="<%$ Resources:Attendance,Are you sure to Generate Payroll ? %>">
                                                    </cc1:ConfirmButtonExtender>
                                                    &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnpostpayroll" Visible="false" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Post Payroll %>"
                                            ValidationGroup="Save" Width="130px" OnClick="btnpostpayroll_Click" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval" runat="server" TargetControlID="btnpostpayroll"
                                                        ConfirmText="<%$ Resources:Attendance,Are you sure to Post Payroll ? %>">
                                                    </cc1:ConfirmButtonExtender>
                                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="BtnPayrollposted" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Payroll Posted Report %>"
                                        ValidationGroup="Save" OnClick="btnpayrollPostedReport_Click" />
                                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnDeletePayroll" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Delete Payroll %>"
                                        Visible="false" ValidationGroup="Save" Width="130px" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmDelete" runat="server" TargetControlID="btnDeletePayroll"
                                                        ConfirmText="Are u sure to delete payroll?">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>
                                                <div style="display: none;" class="col-md-12">
                                                    <asp:Label ID="lblPostFirst" runat="server" Visible="false" CssClass="labelComman"
                                                        Font-Bold="true" ForeColor="Red"></asp:Label><br />
                                                    <asp:Label ID="lblAlreadyPosted" runat="server" Visible="false" CssClass="labelComman"
                                                        Font-Bold="true" ForeColor="Red"></asp:Label><br />
                                                    <asp:Label ID="lblWrongSequence" runat="server" Visible="false" CssClass="labelComman"
                                                        Font-Bold="true" ForeColor="Red"></asp:Label><br />
                                                    <asp:Label ID="lblDojIssue" runat="server" Visible="false" CssClass="labelComman"
                                                        Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    <asp:Label ID="lblsumallow1" runat="server" Text="" CssClass="labelComman" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblsumdeduc1" runat="server" Text="" CssClass="labelComman" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblsum" runat="server" Text="" CssClass="labelComman" Visible="False"></asp:Label>
                                                    <asp:Label ID="lblpenaltyshow" runat="server" Text="" CssClass="labelComman" Visible="False"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Update_Report" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">


                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsPayrollReport" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="col-md-3">
                                                                            <asp:DropDownList ID="ddlField1Report" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetDateTextBox" AutoPostBack="true">
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Payroll No %>" Value="Voucher_no" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Created Date %>" Value="CreatedDate"></asp:ListItem>

                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <asp:DropDownList ID="ddlOption1Report" runat="server" class="form-control">
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-md-5">
                                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindReport">
                                                                                <asp:TextBox ID="txtValue1Report" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                                                <asp:TextBox ID="txtValueDate"  placeholder="Search from Date" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div class="col-md-2" style="text-align: center;">
                                                                            <asp:LinkButton ID="btnbindReport"  runat="server"
                                                                                CausesValidation="False"  OnClick="btnbindReport_Click"
                                                                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                            <asp:LinkButton ID="btnRefreshReport" runat="server"  CausesValidation="False"
                                                                                 OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>



                                                        <div class="box box-warning box-solid"  <%= gvEmp_Report.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                                          
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="flow">

                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp_Report" runat="server" AllowPaging="True" OnSorting="gvEmp_Report_Sorting"
                                                                                AllowSorting="true" AutoGenerateColumns="False" OnPageIndexChanging="gvEmp_Report_PageIndexChanging"
                                                                                Width="100%" DataKeyNames="Voucher_no" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                                <Columns>

                                                                                    <asp:TemplateField >
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Voucher_no") %>'
                                                                                                OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>" Visible='<%# hdnCanPrint.Value=="true"?true:false%>'	
                                                                                                ><i class="fa fa-print"></i></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Payroll No" SortExpression="Voucher_no">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Voucher_no") %>'></asp:Label>
                                                                                            <asp:Label ID="lblLocationId" Visible="false" runat="server" Text='<%# Eval("Location_Id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" SortExpression="Month">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblMonth" runat="server" Text='<%#GetType(Eval("Month").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="Year" HeaderText="<%$ Resources:Attendance,Year%>"
                                                                                        SortExpression="Year" ItemStyle-Width="20%">
                                                                                        <ItemStyle Width="20%" />
                                                                                    </asp:BoundField>


                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created by%>" SortExpression="CreatedBy">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created Date %>" SortExpression="CreatedDate">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblItemType" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Salary %>" HeaderStyle-HorizontalAlign="Center"
                                                                                        SortExpression="TotalAmount">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDoj" runat="server" Text='<%#Common.Get_Roundoff_Amount_By_Location(Common.GetAmountDecimal(Eval("TotalAmount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
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

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Filter">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlPayroll" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlEmployeePayroll" runat="server" Visible="false">
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

            $("#Li_Report").addClass("active");
            $("#Report").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_Report").removeClass("active");
            $("#Report").removeClass("active");
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_Report() {
            document.getElementById('<%= Btn_Report.ClientID %>').click();
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
                var gridView = document.getElementById("<%=gvEmpPayroll.ClientID %>");
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
    </script>
</asp:Content>
