<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="VoucherDetailReport.aspx.cs" Inherits="VoucherEntries_VoucherDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_category.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Voucher Detail Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Voucher Entries%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Voucher Detail Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
  <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Voucher_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Voucher_Detail" Text="Voucher Detail Modal" />
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
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">												
												<div class="col-md-6">
                                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                                        Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                        Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="AllRecord_Rb" runat="server" GroupName="RegularMenu" 
                                                        Text="All Record" Checked="True" />
                                                    <asp:RadioButton ID="EqualRecord_Rb" style="margin-left:20px;" runat="server" GroupName="RegularMenu" 
                                                        Text="Equal Record" />
                                                    <asp:RadioButton ID="NotEqualRecord_Rb" style="margin-left:20px;" runat="server" AutoPostBack="True" 
                                                        GroupName="RegularMenu" Text="Not Equal Record" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    	<div class="col-md-12">
		<div class="col-md-2">
		</div>
		<div class="col-md-3">
			<asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
				SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
				ForeColor="Gray"></asp:ListBox>
		</div>
		<div class="col-lg-2" style="text-align: center">
			<div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

				<asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

				<asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />

				<asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

				<asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />
			</div>
		</div>
		<div class="col-md-3">
			<asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
				SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
				ForeColor="Gray"></asp:ListBox>
		</div>
		<div class="col-md-2">
		</div>
		<br />
	</div>
                                                    <div class="col-md-12" style="text-align:center">
                                                        <br />
                                                        <asp:Button ID="btnGetReport" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                                                    CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                                        <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                                                    CssClass="btn btn-primary" OnClick="btnShowReport_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                        ></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow:auto;max-height:500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSStatement" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                        >
                                                        
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Type" SortExpression="Voucher_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>"
                                                                        />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>" SortExpression="Debit Total">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("DebitTotal") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvDebitAmountTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>" SortExpression="Credit Total">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("CreditTotal") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvCreditAmountTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Difference" SortExpression="Difference">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDiffrence" runat="server" Text='<%#Eval("Diffrence") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvDiffrenceTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"  />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        
                                                    </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
    
    <div class="modal fade" id="Voucher_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label25" runat="server" Text="Voucher Detail" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                    <asp:TextBox ID="txtToLocation" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                    <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                                        <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                                        <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                                        <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                                        <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                                        <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                                        <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                                        <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                                        <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                                        <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                                        <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                                        <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                                        <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                                        <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator10" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtVoucherNo" errormessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtVoucherNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <asp:TextBox ID="txtVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender13" runat="server" TargetControlID="txtVoucherDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:RadioButton ID="rbCashPayment" runat="server" CssClass="labelComman" Text="Cash Payment"
                                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                    <asp:RadioButton ID="rbChequePayment" runat="server" CssClass="labelComman" Text="Cheque Payment"
                                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                    <asp:CheckBox ID="chkReconcile" Visible="false" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                </div>
                                                
                                                <div id="trCheque1" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender14" runat="server" TargetControlID="txtChequeIssueDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender15" runat="server" TargetControlID="txtChequeClearDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="trCheque2" runat="server" visible="false" class="col-md-6">
                                                    <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtReference" ReadOnly="true" runat="server" CssClass="form-control"/>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="box box-warning box-solid">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title">
                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Detail View%>" />
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:HiddenField ID="hdnVAccountNo" runat="server" />
                                                        <asp:HiddenField ID="hdnVAccountName" runat="server" />
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" ShowFooter="true" runat="server" Width="100%" 
                                                                                AutoGenerateColumns="False">
                                                                                
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                        <ItemTemplate>
                                                                                            <%#Container.DataItemIndex+1 %>
                                                                                            <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                                        <ItemTemplate>
                                                                                            <%--<asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#GetCustomerNameByContactId(Eval("Other_Account_No").ToString())%>' />--%>
                                                                                            <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.Get_Other_Account_Name(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                            <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                                                Text="<%$ Resources:Attendance,Total%>" />
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                        <FooterStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                        <FooterStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle  HorizontalAlign="Center" />
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                            <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                            <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                
                                                                            </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
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
                    <asp:UpdatePanel ID="Update_Voucher_Detail_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                                        OnClick="btnCancelPopLeave_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail_Button">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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

        function Voucher_Detail_Modal_Show() {
            document.getElementById('<%= Btn_Voucher_Detail.ClientID %>').click();
        }
    </script>
</asp:Content>
