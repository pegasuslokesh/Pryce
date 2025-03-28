<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Pay_Employee_Loan.aspx.cs" Inherits="HR_Pay_Employee_Loan" %>


<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/Loan_Adjustment.ascx" TagName="Loan_Adjustment" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-coins"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Loan%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Loan%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_Bin" runat="server" Style="display: none;" Text="Bin" OnClick="btnBin_Click" />
            <asp:Button ID="Btn_LoanRequestReport" Style="display: none;" runat="server" data-toggle="modal" data-target="#LoanRequestReport" />

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
            <li id="Li_RePayment"><a href="#RePayment" data-toggle="tab">
                <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Re Payment %>"></asp:Label></a></li>
            <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Payment %>"></asp:Label></a></li>

            <li id="Li_Loan"><a href="#Loan" data-toggle="tab">
                <i class="fa fa-file"></i>&nbsp;&nbsp;
                <asp:Label ID="Lbl_Loan_Tab" runat="server" Text="<%$ Resources:Attendance,Loan%>"></asp:Label></a></li>
            <li class="active" id="Li_List"><a href="#List" data-toggle="tab">
                <i class="fa fa-list"></i>&nbsp;&nbsp;
                <asp:Label ID="Lbl_New_tab" runat="server" Text="<%$ Resources:Attendance,List%>"></asp:Label>
            </a></li>

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
					<asp:Label ID="lblTotalRecordsLoan" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-12">
                                            <asp:DropDownList runat="server" ID="ddlLocation" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlField1" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Loan Name %>" Value="Loan_Name"></asp:ListItem>
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
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnLoanBind">
                                                <asp:TextBox ID="txtValue1" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="imgBtnLoanBind" runat="server"
                                                CausesValidation="False" OnClick="btnLoanbind_Click"
                                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                OnClick="btnLoanRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="box box-warning box-solid" <%= GridViewLoan.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:HiddenField ID="HDFSort" runat="server" />
                                            <asp:HiddenField ID="hdnRPTClick" runat="server" />
                                            <asp:HiddenField ID="hdnLoanId" runat="server" />
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewLoan" runat="server" AllowPaging="True" AllowSorting="true"
                                                AutoGenerateColumns="False" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                OnPageIndexChanging="GridViewLoan_PageIndexChanging" OnSorting="GridViewLoan_Sorting">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">
                                                                    <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Loan_Id") %>'
                                                                            OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                    </li>

                                                                    <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("Loan_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                            CausesValidation="False" OnCommand="btnView_Command"
                                                                            ToolTip="<%$ Resources:Attendance,View %>"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                    </li>

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Loan_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                            CausesValidation="False" OnCommand="btnEdit_command"
                                                                            ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Loan_Id") %>'
                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_command" CommandName='<%# Eval("Location_Id") %>'
                                                                            ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>


                                                                </ul>
                                                            </div>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpCodeList" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpnameList" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblEmpIdList" runat="server" Text='<%# Eval("Loan_Id") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="Loan_Request_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblrequestdate" runat="server" Text='<%#GetDate(Eval("Loan_Request_Date").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Name %>" SortExpression="Loan_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPenaltynameList" runat="server" Text='<%# Eval("Loan_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Amount %>" SortExpression="Loan_Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblvaluetype" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Loan_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
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
            <div class="tab-pane" id="Loan">
                <asp:UpdatePanel ID="Update_Loan" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-body">
                                        <div class="form-group">

                                            <div class="col-md-6">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Loan Request Date %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtRequestDate" ErrorMessage="Enter Loan Request Date"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtRequestDate" />

                                            </div>


                                            <div class="col-md-6">
                                                <asp:Label ID="lblModuleName" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtEmpName" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                                    OnTextChanged="TxtEmpName_TextChanged" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Loan Name %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLoanName" ErrorMessage="<%$ Resources:Attendance,Enter Loan Name %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtLoanName" runat="server" BackColor="#eeeeee"
                                                    CssClass="form-control"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetLoanName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="TxtLoanName"
                                                    UseContextKey="True"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblLoanAmount" runat="server" Text="<%$ Resources:Attendance,Loan Amount %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLoanAmount" ErrorMessage="<%$ Resources:Attendance,Enter Loan Amount %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtLoanAmount" runat="server" OnTextChanged="TxtDuration_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    TargetControlID="TxtLoanAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Interest(%) %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="M_Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInterest" ErrorMessage="<%$ Resources:Attendance,Enter Interest %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtInterest" OnTextChanged="TxtDuration_TextChanged" AutoPostBack="true"
                                                    runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                    TargetControlID="txtInterest" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Duration(In Months) %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="M_Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDuration" ErrorMessage="<%$ Resources:Attendance,Enter Duration(In Months) %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtDuration" MaxLength="3" runat="server" AutoPostBack="true" CssClass="form-control"
                                                    OnTextChanged="TxtDuration_TextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtDuration" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Gross Amount %>"></asp:Label>
                                                <asp:TextBox ID="txtGrossAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Monthly Installment %>"></asp:Label>
                                                <asp:TextBox ID="txtMonthlyInstallment" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label13" runat="server" Text="Deduction Start From"></asp:Label>
                                                <%--<asp:Label ID="lbldeductionfrom" runat="server" Font-Bold="true"></asp:Label>--%>
                                                <asp:DropDownList ID="ddldeductionstart" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align: center;">
                                                <asp:Button ID="BtnSave" Visible="false" runat="server" CssClass="btn btn-success"
                                                    Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save"
                                                    OnClick="BtnSave_Click" />
                                                <asp:Button ID="BtnReset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                    Text="<%$ Resources:Attendance,Reset %>" OnClick="BtnReset_Click" />
                                                <asp:Button ID="BtnCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                    Text="<%$ Resources:Attendance,Cancel %>" OnClick="BtnCancel_Click" />
                                                <asp:HiddenField ID="HiddeniD" runat="server" />
                                                <asp:HiddenField ID="HidEmpId" runat="server" />
                                                <asp:HiddenField ID="hidLoanType" runat="server" />
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
                                <div class="box box-info">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Get_Report"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName_Payment" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                <asp:TextBox ID="txtEmpName_Payment" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                                    OnTextChanged="TxtEmpName_TextChanged" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName_Payment" UseContextKey="True"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                            </div>
                                            <div class="col-md-6">
                                                <br />
                                                <asp:Button ID="btnGetRecord" ValidationGroup="Get_Report" runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary" OnClick="btnGetRecord_Click" />


                                                <br />
                                                <br />
                                            </div>

                                            <div class="col-md-12">

                                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Payment" SetFocusOnError="true" runat="server" ErrorMessage="Please select at least one record."
                                                    ClientValidationFunction="Validate_Emp_Grid" ForeColor="Red"></asp:CustomValidator>
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AllowPaging="false"
                                                        AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField SortExpression="Is_Report">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="true"
                                                                        OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Name %>" SortExpression="Loan_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPenaltynameList" runat="server" Text='<%# Eval("Loan_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Loan_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approval Date" SortExpression="Loan_Approval_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpaymentapprovaldate" runat="server" Text='<%# GetDate(Eval("Loan_Approval_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Amount %>" SortExpression="Loan_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblvaluetype" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Loan_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <br />
                                                </div>
                                            </div>

                                            <div id="divPending" runat="server" visible="false">

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator8" ValidationGroup="Payment" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtLSDebitAccount" ErrorMessage="Configure Loan Account"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtLSDebitAccount" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="false"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLSDebitAccount"
                                                        UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator9" ValidationGroup="Payment" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtLSCreditaccount" ErrorMessage="Enter Credit Account" />
                                                    <asp:TextBox ID="txtLSCreditaccount" runat="server" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLSCreditaccount"
                                                        UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>

                                                <div class="col-md-12" style="text-align: center">

                                                    <asp:Button ID="btnPayment" ValidationGroup="Payment" runat="server" Text='<%$ Resources:Attendance,Payment %>' CssClass="btn btn-primary"
                                                        OnClick="btnPayment_Click" Visible="false" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to Pay Loan Amount ?"
                                                        TargetControlID="btnPayment">
                                                    </cc1:ConfirmButtonExtender>


                                                    <asp:Button ID="btnPaymentCancel" runat="server" Text='<%$ Resources:Attendance,Cancel %>'
                                                        CssClass="btn btn-primary" OnClick="btnPaymentCancel_Click" />
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
            <div class="tab-pane" id="RePayment">
                <asp:UpdatePanel ID="Update_Loan_Adjustment_control" runat="server">
                    <ContentTemplate>
                        <UC:Loan_Adjustment ID="Control_Loan_Adjustment" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>


    <div class="modal fade" id="LoanRequestReport" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Loan">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Loan_Adjustment_control">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script>

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

        function Validate_Emp_Grid(sender, args) {
            var gridView = document.getElementById("<%=gvPayment.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function On_Edit_Tab_Position() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_Loan").addClass("active");
            $("#Loan").addClass("active");
        }
        function On_Cancel_Tab_Position() {
            $("#Li_Loan").removeClass("active");
            $("#Loan").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function resetReportField() {
            document.getElementById('<%= hdnRPTClick.ClientID%>').value = "0";
            document.getElementById('<%= hdnLoanId.ClientID%>').value = "0";

        }

        function openLoanRequestReport() {
            document.getElementById('<%= Btn_LoanRequestReport.ClientID%>').click();
        }

    </script>

</asp:Content>
