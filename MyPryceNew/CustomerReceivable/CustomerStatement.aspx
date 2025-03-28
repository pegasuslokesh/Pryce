<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CustomerStatement.aspx.cs" Inherits="CustomerReceivable_CustomerStatement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .FixedHeader {
            position: relative;
            font-weight: bold;
        }
        /* Style for the table container */
        .table-container {
            overflow: auto; /* Enable scrolling for the container */
            max-height: 400px; /* Set a max height for the scrollable area */
            position: relative; /* Relative positioning is needed for the sticky header */
        }

        /* Style for the table header */
        #tblStatement thead {
            position: sticky;
            top: 0; /* Stick the header to the top of the container */
            background-color: white; /* Adjust background color as needed */
            z-index: 1; /* Ensure the header is above the body when scrolling */
        }

        #tblStatement tfoot {
            position: sticky;
            bottom: 0; /* Stick the header to the top of the container */
            background-color: white; /* Adjust background color as needed */
            z-index: 1;
        }
        /* Style for the table body */
        #tblStatement tbody {
            /* Adjust styling as needed */
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/account_voucher.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Customer Statement%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Customer Receivable%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Customer Statement %>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Invoice_Staement_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Ageing_Detail" Text="View Modal" />
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" Text="Add Contact" />

            <asp:Button ID="Btn_Voucher_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Voucher_Detail" Text="View Modal" />
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
            <div id="PnlCustomerStatement" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <%--<div class="col-md-12" id="div_inAactive_account" runat="server" visible="false">
                                    <asp:CheckBox ID="chkShowInActiveAccount" runat="server" Text="InActive Account Only" OnClick="SetContextKey();" ClientIDMode="Static" />
                                    <br />
                                </div>--%>
                                <div class="row-md-12">
                                    <div class="col-md-11">

                                        <asp:Label ID="lblAccountName" runat="server" Text="<%$ Resources:Attendance,Customer%>" />
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                            CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                            ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />

                                    </div>
                                    <div class="col-md-1">
                                        <br />
                                        <asp:LinkButton ID="lnkgvCustomer" runat="server" OnClick="lnkgvCustomer_Click" ToolTip="Contact Information" OnClientClick="SetSelectedRow(this)">
    <i class="fas fa-user-plus" style="width:48px" ></i> <!-- Font Awesome icon for "Add Customer" -->
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblSVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                    <asp:DropDownList ID="ddlCVoucherType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                        <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                        <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                        <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                        <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                        <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div id="trclosing" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="lblClosingCreditBalance" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                    <asp:TextBox ID="txtClosingCreditBalance" ReadOnly="true" runat="server"
                                        CssClass="form-control" />
                                    <br />
                                </div>
                                <div class="col-md-12">
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
                                        <asp:Label ID="lblReconciled" runat="server" Text="<%$ Resources:Attendance,Reconciled %>" />
                                        <asp:TextBox ID="txtReconciled" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="txtPresentColorCode_ColorPickerExtender" runat="server"
                                            Enabled="True" TargetControlID="txtReconciled" SampleControlID="txtReconciled">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                        <asp:Label ID="lblConflicted" runat="server" Text="<%$ Resources:Attendance,Conflicted %>" />
                                        <asp:TextBox ID="txtConflicted" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender1" runat="server" Enabled="True"
                                            TargetControlID="txtConflicted" SampleControlID="txtConflicted">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                        <asp:Label ID="lblNotReconciled" runat="server" Text="<%$ Resources:Attendance,Not Reconciled %>" />
                                        <asp:TextBox ID="txtNotReconciled" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender2" runat="server" Enabled="True"
                                            TargetControlID="txtNotReconciled" SampleControlID="txtNotReconciled">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox ID="chkAgeingAnalysis" Visible="false" OnCheckedChanged="chkAgeingAnalysis_CheckedChanged"
                                        AutoPostBack="true" runat="server" Text="<%$ Resources:Attendance,Ageing Analysis %>" />
                                    <asp:HiddenField ID="hdnAgeing" runat="server" Value="0" />

                                    <asp:CheckBox ID="chkAllCustomer" Visible="false" runat="server" Text="<%$ Resources:Attendance,All Customer Print %>" />

                                    <asp:CheckBox ID="chkOnlyDue" Visible="false" runat="server" Text="<%$ Resources:Attendance,All Customer Due %>" />

                                    <asp:CheckBox ID="chkOnlyPaid" Visible="false" runat="server" Text="<%$ Resources:Attendance,All Customer Paid %>" />
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGetReport" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                        CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnCancel_Click" />

                                    <asp:Button ID="btnShowReport" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" />

                                    <asp:Button ID="btnShowProductReport" runat="server" Text="<%$ Resources:Attendance,With Product Detail %>"
                                        CssClass="btn btn-primary" OnClick="btnShowProductReport_Click" />

                                    <asp:Button ID="btnAgeingData" runat="server" Text="Get Ageing Detail"
                                        CssClass="btn btn-primary" OnClick="btnAgeingData_Click" />

                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblOpeningBalance" runat="server" Text="<%$ Resources:Attendance,Opening Balance %>" />
                                    <asp:TextBox ID="txtOpeningBalance" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFOpening" runat="server" Text="Opening (Foreign)" />
                                    <asp:TextBox ID="txtFOpening" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lblClosingBalance" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                    <asp:TextBox ID="txtClosingBalance" runat="server" CssClass="form-control" ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFClosing" runat="server" Text="Closing (Foreign)" />
                                    <asp:TextBox ID="txtFClosing" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="table-container">
                                        <table class="table table-bordered" id="tblStatement">
                                            <!-- Table Header Will Go Here -->
                                            <thead>
                                                <tr>
                                                    <!-- Header Cells -->
                                                </tr>
                                            </thead>
                                            <!-- Table Body Will Go Here -->
                                            <tbody>
                                                <tr>
                                                    <!-- Data Cells -->
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                            </tfoot>
                                        </table>
                                    </div>


                                    <asp:HiddenField ID="tblTransId" runat="server" />
                                    <asp:HiddenField ID="tblVoucherNo" runat="server" />
                                    <div style="display: none">
                                        <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='0'
                                            ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                            OnCommand="lnkViewDetail_Command" CausesValidation="False" OnClientClick="SetSelectedRow(this)" />
                                        <asp:ImageButton ID="lnkgvVoucherNo" runat="server" CommandArgument='0'
                                            ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                            OnCommand="lblgvVoucherNo_Click" CausesValidation="False" OnClientClick="SetSelectedRow(this)" />
                                    </div>

                                </div>
                                <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdfCurrentRow" runat="server" />
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" HeaderStyle-CssClass="FixedHeader" ID="GVCStatement" Width="100%" runat="server" AutoGenerateColumns="false"
                                        ShowFooter="true">

                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                <ItemTemplate>
                                                    <%-- <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Header_Trans_Id") %>'
                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False" OnClientClick="SetSelectedRow(this)" />--%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                <ItemTemplate>
                                                    <%--    <asp:LinkButton ID="lnkgvVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'
                                                        OnClick="lblgvVoucherNo_Click" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Header_Trans_Id")+ "," + Eval("Location_Id")%>' OnClientClick="SetSelectedRow(this)" />--%>
                                                    <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Refrence %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvRefrenceNumber" runat="server" Text='<%#Eval("RefrenceNumber") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>" SortExpression="Debit_Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvBalance" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Foreign(Amt)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFAmount" runat="server" Text='<%# Eval("Foreign_Amount") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Foreign(balance)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFBalance" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblGvFBalanceTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>

                                        <PagerStyle CssClass="pagination-ys" />

                                    </asp:GridView>
                                </div>

                                <div id="tdAgeing" runat="server" visible="false" class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="lblAgeingAnalysis" runat="server" Text="<%$ Resources:Attendance,Ageing Analysis %>" /></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbl0to30" runat="server" Text="<%$ Resources:Attendance,0-30 Days %>" />
                                                            <asp:TextBox ID="txt0to30" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbl31to60" runat="server" Text="<%$ Resources:Attendance,31-60 Days %>" />
                                                            <asp:TextBox ID="txt31to60" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbl61to90" runat="server" Text="<%$ Resources:Attendance,61-90 Days %>" />
                                                            <asp:TextBox ID="txt61to90" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbl91to180" runat="server" Text="<%$ Resources:Attendance,91-180 Days %>" />
                                                            <asp:TextBox ID="txt91to180" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbl181to365" runat="server" Text="<%$ Resources:Attendance,181-365 Days %>" />
                                                            <asp:TextBox ID="txt181to365" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAbove365" runat="server" Text="<%$ Resources:Attendance,Above 365 Days %>" />
                                                            <asp:TextBox ID="txtAbove365" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
       <asp:HiddenField ID="HDFcurrentCustomerID" runat="server" />
    <div class="modal fade" id="Voucher_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel1" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="reloadtblData(); return false;" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel1">
                        <asp:Label ID="Label3" runat="server" Text="Voucher Detail" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                    <asp:TextBox ID="txtToLocation" ReadOnly="true" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
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
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
                                                    <%--   <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>--%>

                                                    <asp:TextBox ID="txtVoucherNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <a style="color: Red">*</a>
                                                    <%--  <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>--%>

                                                    <asp:TextBox ID="txtVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtVoucherDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:RadioButton ID="rbCashPayment" runat="server" Text="Cash Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbChequePayment" runat="server" Text="Cheque Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkReconcile" Visible="false" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                </div>
                                                <div id="trCheque1" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtChequeIssueDate" runat="server" TargetControlID="txtChequeIssueDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_ChequeClearDate" runat="server" TargetControlID="txtChequeClearDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="trCheque2" runat="server" visible="false" class="col-md-6">
                                                    <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtReference" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="box box-warning box-solid">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Detail View%>" /></h3>
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" ShowFooter="true" runat="server" Width="100%"
                                                                            AutoGenerateColumns="False">

                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                    <ItemTemplate>
                                                                                        <%#Container.DataItemIndex+1 %>
                                                                                        <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                                        <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                        <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                                            Text="<%$ Resources:Attendance,Total%>" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <%--<asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />--%>
                                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                        <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                        <asp:HiddenField ID="hdnAccountNo" runat="server" />
                                                                        <asp:HiddenField ID="hdnAccountName" runat="server" />
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
                            <button type="button" onclick="reloadtblData(); return false;" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                OnClick="btnCancelPopLeave_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


            </div>
        </div>
    </div>
    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail_Button">
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


    <div class="modal fade" id="Ageing_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label1" runat="server" Text="Customer Invoice Statement" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
                                                    <%--<dx:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
                                                    </dx:ReportViewer>--%>
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
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>


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
        function Voucher_Detail_Popup() {
            document.getElementById('<%= Btn_Voucher_Detail.ClientID %>').click();
            reloadtblData();

        }
      
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GVCStatement.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;

            }
            catch (e) {

            }

        }

        function SetSelectedRow(lnk) {
            //Reference the GridView Row.
            try {
                var row = lnk.parentNode.parentNode;
                $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);
                row.style.backgroundColor = "#A1DCF2";
            }
            catch (e) {

            }
        }

        function SetDivPosition() {
            try {
                var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                h.value = intY;
            }
            catch (e) {

            }
        }
        var GlobalTblData;

        function GetReport(resp) {
            debugger;
            GlobalTblData = resp;
            var data = resp;
            var table = document.getElementById('tblStatement');
            console.log(data);
            // Clear the existing table
            table.innerHTML = '';

            // Create a table header
            var thead = table.createTHead();
            var headerRow = thead.insertRow(0);

            var headers = [
                "",
                "Voucher No",
                "Voucher Date",
                "Voucher Type",
                "Narration",
                "Reference No",
                "Debit Amount",
                "Credit Amount",
                "Local balance",
                "Foreign Amount",
                "Created by",
                "Sales Person"
            ];

            for (var i = 0; i < headers.length; i++) {
                var th = document.createElement('th');
                th.textContent = headers[i];
                headerRow.appendChild(th);
            }

            // Create a table body
            var tbody = table.createTBody();
            var OpeningBalance = $('#<%=txtOpeningBalance.ClientID%>').val();
            var fBalance = parseFloat(0);
            for (var i = 0; i < data.length; i++) {
                var row = tbody.insertRow(i);

                if (i == 0) {
                    fBalance = (parseFloat(OpeningBalance) + parseFloat(data[i].Debit_Amount)) - parseFloat(data[i].Credit_Amount);
                }
                else {
                    fBalance = (parseFloat(fBalance) + parseFloat(data[i].Debit_Amount)) - parseFloat(data[i].Credit_Amount)
                }
                // Add data cells
                row.insertCell(0).innerHTML = "<a class='btn btn-default btn-sm' title='View' onclick='showVoucherDetail(" + data[i].Header_Trans_Id + ")'><img src='../Images/Detail1.png' alt='Pegasus' height='15px' width='15px'></a>";
                row.insertCell(1).innerHTML = "<a style='cursor: pointer' onclick='openVoucherSource(" + data[i].ref_id + ", \"" + data[i].ref_type + "\", " + data[i].Header_Trans_Id + ", " + data[i].location_id + ")'>" + data[i].voucher_no + "</a>";
                row.insertCell(2).textContent = data[i].voucher_date;
                row.insertCell(3).textContent = data[i].voucher_type;
                row.insertCell(4).textContent = data[i].narration;
                row.insertCell(5).textContent = data[i].RefrenceNumber;
                row.insertCell(6).textContent = data[i].Debit_Amount;
                row.insertCell(7).textContent = data[i].Credit_Amount;
                //row.insertCell(8).textContent = data[i].fBalanceAmount;
                row.insertCell(8).textContent = fBalance.toFixed(3);
                row.insertCell(9).textContent = data[i].foreign_amount;
                row.insertCell(10).textContent = data[i].CreatedBy_User;

                row.insertCell(11).textContent = data[i].SalesPerson;


            }

            // Calculate and display totals
            var debitTotal = 0;
            var creditTotal = 0;
            var fBalanceTotal = 0;
            var foreignTotal = 0;

            for (var i = 0; i < data.length; i++) {
                debitTotal += data[i].Debit_Amount;
                creditTotal += data[i].Credit_Amount;
                //fBalanceTotal += data[i].fBalanceAmount;
                // fBalanceTotal += fBalance;
                foreignTotal += data[i].foreign_amount;
            }

            var tfoot = table.createTFoot();
            var footerRow = tfoot.insertRow(0);

            // Add empty cells for the first two columns
            for (var i = 0; i < 11; i++) {
                footerRow.insertCell(i);
            }

            $('#<%=txtClosingBalance.ClientID%>').val(fBalance.toFixed(3));
            $('#<%=txtFClosing.ClientID%>').val(fBalance.toFixed(3));
            //console.log(debitTotal, creditTotal);
            footerRow.insertCell(2).textContent = "Total";
            footerRow.insertCell(6).textContent = debitTotal.toFixed(3);
            footerRow.insertCell(7).textContent = creditTotal.toFixed(3);
            footerRow.insertCell(8).textContent = fBalance.toFixed(3);
            //footerRow.insertCell(9).textContent = foreignTotal.toFixed(3);

            $('#tblStatement').dataTable();
        }


        function showVoucherDetail(Trans_Id) {
            // alert(Trans_Id);
            $('#<%= tblTransId.ClientID %>').val(Trans_Id);

            $('#<%= lnkViewDetail.ClientID %>').click();
        }
        function Invoice_Statement_Popup() {
            document.getElementById('<%= Btn_Invoice_Staement_Detail.ClientID %>').click();
          }
        function reloadtblData() {
            GetReport(GlobalTblData);
        }

        function openVoucherSource(refId, refType, headerTransId, locationId) {
            var Details = refId + ',' + refType + ',' + headerTransId + ',' + locationId;
            $('#<%= tblVoucherNo.ClientID %>').val(Details);

            $('#<%= lnkgvVoucherNo.ClientID %>').click();

        }
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }

        $('#Ageing_Detail').on('hidden.bs.modal', function () {
            $('#<%=HDFcurrentCustomerID.ClientID %>').val("0");
              setScrollAndRow();
          });

    </script>
</asp:Content>
