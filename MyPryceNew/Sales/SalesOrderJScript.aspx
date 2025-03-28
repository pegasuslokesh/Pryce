<%@ Page Title="" Language="C#" EnableEventValidation="False"  MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesOrderJScript.aspx.cs" Inherits="Sales_SalesOrderJScript" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register assembly="DevExpress.XtraReports.v18.1.Web.WebForms" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .pager {
            background-color: #3AC0F2;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .active_pager {
            background-color: #2E99C1;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .grid td, .grid th {
            text-align: center;
        }

        tooltip-inner {
            max-width: none;
            white-space: nowrap;
        }

        #progressBar {
            visibility: hidden;
            min-width: 250px;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            top: 50%;
            left: 50%;
            font-size: 17px;
        }

            #progressBar.show {
                visibility: visible;
                -webkit-animation: fadein 0.5s;
                animation: fadein 0.5s;
            }

            #progressBar.hide {
                visibility: hidden;
                -webkit-animation: fadeout 0.5s 2.5s;
                animation: fadeout 0.5s 2.5s;
            }

        @-webkit-keyframes fadein {
            from {
                top: 0;
                opacity: 0;
            }

            to {
                top: 30px;
                opacity: 1;
            }
        }

        @keyframes fadein {
            from {
                top: 0;
                opacity: 0;
            }

            to {
                top: 30px;
                opacity: 1;
            }
        }

        @-webkit-keyframes fadeout {
            from {
                top: 30px;
                opacity: 1;
            }

            to {
                top: 0;
                opacity: 0;
            }
        }

        @keyframes fadeout {
            from {
                top: 30px;
                opacity: 1;
            }

            to {
                top: 0;
                opacity: 0;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
      <h1>
        <i class="fa fa-file-text"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label532" runat="server" Text="<%$ Resources:Attendance,Sales Order%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnDecimalCount" runat="server" />
            <asp:HiddenField ID="hdnIsTaxEnabled" runat="server" Value="false" />
            <asp:HiddenField ID="hdnIsDiscountEnable" runat="server" Value="false" />
            <asp:HiddenField ID="hdnDenomination" runat="server" Value="" />
            <asp:HiddenField ID="hdnLocationId" runat="server" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" Value="" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField runat="server" ID="hdnCanViewAllRec" />
            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentRow" runat="server" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Quotation" Style="display: none;" runat="server" OnClick="btnPRequest_Click" Text="Quotation" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_NewAddress" Style="display: none;" runat="server" data-toggle="modal" data-target="#NewAddress" Text="New Address" />
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
                    <li id="Li_Quotation"><a href="#Quotation" onclick="Li_Tab_Quotation()" data-toggle="tab">
                        <i class="fas fa-file-invoice-dollar"></i>&nbsp;&nbsp;<asp:Label ID="Label223" runat="server" Text="<%$ Resources:Attendance,Quotation %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server" UpdateMode="Conditional">
                            <Triggers>
                             <%--   <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />--%>
                               <%-- <asp:AsyncPostBackTrigger ControlID="btnSOrderSave" EventName="Click" />--%>
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                            </Triggers>
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
                 <%--               <asp:AsyncPostBackTrigger ControlID="btnSOrderSave" EventName="Click" />--%>
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Panel ID="pnlGetOrder" runat="server" DefaultButton="btnGetOrder" Visible="false">
                                                        <asp:Button ID="btnGetOrder" runat="server" Text="Get Order" CssClass="btn btn-primary"
                                                            OnClick="btnGetOrder_Click" />
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" onchange="ddlPosted_SelectedIndexChanged();return false;" >
                                                        <asp:ListItem Selected="True" Text="Invoice Pending" Value="Pending"></asp:ListItem>
                                                        <asp:ListItem Text="Invoice Created" Value="Created"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Reference Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Ref No. %>" Value="QauotationNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Order No" Value="Field3"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Payment Mode %>" Value="PaymentModeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" placeholder="Search from Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"  OnClientClick="btnbindrpt_Click(); return false;" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClientClick="btnRefreshReport_Click(); return false;" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                 <table id="tblOrderList" class="table-striped table-bordered table table-hover" >
                                                    <thead>
                                                        <th>Action</th>
                                                         <th>Order No</th>
                                                        <th>Order Date</th>
                                                        <th>Customer Name</th>
                                                        <th>Customer Order No.</th>
                                                        <th>Reference Type</th>
                                                        <th>Ref. No.</th>
                                                        <th>Status</th>
                                                        <th>Payment Mode</th>
                                                        <th>Created By</th>
                                                        <th>Amount</th>
                                                    </thead>
                                                     <tbody>

                                                     </tbody>
                                                 </table>
                                                    <asp:HiddenField ID="hdnGvSalesOrderCurrentPageIndex" Value="1" runat="server" />
                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                              <%--  <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />--%>
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
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
                                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSODate" ErrorMessage="<%$ Resources:Attendance,Enter Order Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtSODate" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSONo" ErrorMessage="<%$ Resources:Attendance,Enter Order No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSONo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOrderType" runat="server" Text="<%$ Resources:Attendance,Order Type %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlOrderType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Order Type %>" />

                                                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Direct%>" Value="D"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,By Quotation%>" Value="Q"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlCustomerOrderNO" runat="server">
                                                        <asp:Label ID="lblCustOrderNo" runat="server" Text="<%$ Resources:Attendance,Customer Order No. %>" />
                                                        <asp:TextBox ID="txtCustOrderNo" TabIndex="1" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency%>" />

                                                        <asp:DropDownList ID="ddlCurrency" TabIndex="2" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPaymentMode" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Payment Mode%>" />

                                                        <asp:DropDownList ID="ddlPaymentMode" TabIndex="3" runat="server" CssClass="form-control"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentTypeMode_SelectedIndexChanged" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="ctlSalesPerson" runat="server">
                                                        <asp:HiddenField ID="hdnSalesPersonId" runat="server" />
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="Sales Person" />
                                                        <asp:TextBox ID="txtSalesPerson" TabIndex="4" runat="server" CssClass="form-control" OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trTransfer" runat="server">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblTransFrom" runat="server" Text="<%$ Resources:Attendance,Transfer From %>" />
                                                            <asp:TextBox ID="txtTransFrom" runat="server" CssClass="form-control" ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblQuotationNo" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:DropDownList ID="ddlQuotationNo" runat="server" CssClass="form-control"
                                                                Visible="false" OnSelectedIndexChanged="ddlQuotationNo_SelectedIndexChanged"
                                                                AutoPostBack="true" />
                                                            <asp:TextBox ID="txtQuotationNo" runat="server" ReadOnly="true" CssClass="form-control" Visible="true" />
                                                            <br />
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomer" TabIndex="5" runat="server" CssClass="form-control" AutoPostBack="false"
                                                                BackColor="#eeeeee" onchange="txtCustomer_TextChanged(this)" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <button type="button" id="btnnewcustomer" title="Add Customer"  onclick="btnNewCustomer(); return false;" name="btnnewCustomer" class="btn btn-primary" >Add</button>
                                                                <asp:Button ID="lnkcustomerHistory" runat="server" Text="History" CssClass="btn btn-primary" CausesValidation="False"
                                                                    OnClientClick="lnkcustomerHistory_OnClick()"></asp:Button>
                                                                <input type="button" class="btn btn-primary" value="Credit Detail" onclick="GetCreditInfo()" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <%--<a style="color: Red">*</a>--%>
                                                        <asp:DropDownList ID="ddlTransType" TabIndex="6" onchange="ddlTransTypeChange();return false;" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlTransType" InitialValue="-1" ErrorMessage="<%$ Resources:Attendance,Select Transaction Type%>" />--%>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="hdnContactPersonId" runat="server" />
                                                        <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person" />

                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtContactPerson" TabIndex="7" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtContactId_TextChanged" AutoPostBack="true" />
                                                           <small><asp:Label ID="lblSubDetail"   runat="server" Text="" ></asp:Label></small>
                                                             <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                                ServiceMethod="GetContactListCustomer" ServicePath="" TargetControlID="txtContactPerson"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_Click"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label14" runat="server" Text="Agent Name" />
                                                        <asp:TextBox ID="txtAgentName" TabIndex="8" runat="server" CssClass="form-control" onchange="toggleCommissionColumn(); return false;"  Enabled="false" BackColor="#eeeeee" />
                                                           
                                                          
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderAgentName" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                            ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtAgentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInvoiceTo" runat="server" Text="<%$ Resources:Attendance,Invoice Address %>" />
                                                        <asp:TextBox ID="txtInvoiceTo" TabIndex="9" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="false" onchange="GetInvoiceAddress(); return false;" />
                                                        <small><asp:Label ID="InvoiceAddress"  runat="server" Text="" ></asp:Label></small>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceTo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEstimateDeliveryDate" runat="server" Text="<%$ Resources:Attendance,Estimate Delivery Date %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEstimateDeliveryDate" ErrorMessage="<%$ Resources:Attendance,Enter Estimate Delivery Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtEstimateDeliveryDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender" runat="server" TargetControlID="txtEstimateDeliveryDate" />
                                                        <br />
                                                    </div>                                                      
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label332" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                        <asp:TextBox ID="txtShipCustomerName" TabIndex="10" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            onchange="txtCustomer_TextChanged(this)" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtShipCustomerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-5" id="ctlShipingAddress" runat="server">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                        <asp:TextBox ID="txtShipingAddress" TabIndex="11" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="false" onchange="ShipingAddressDetail(); return false;" />
                                                        <small><asp:Label ID="ShippingAddress" runat="server"  Text="" ></asp:Label></small>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShipingAddress"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br/>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <br />
                                                        <asp:Button ID="btnNewaddress" runat="server" Text="New Address" class="btn btn btn-primary" OnClick="btnNewaddress_Click" />
                                                        &nbsp;                                                         
                                                        <button id="btnGetLocation" type="button"  class="btn btn-default" onclick="GetGeoLocation(); return false;"><i class="fa fa-map-marker" style="color:green"></i></button>
                                                        <br />
                                                    </div>   
                                                   <div class="col-md-6">
                                                       <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                           Visible="false" GroupName="Product" OnClick="rbtnFormView_OnCheckedChanged();" />
                                                       <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 20px;" Font-Bold="true" runat="server" Visible="false"
                                                           Text="<%$ Resources:Attendance,Advance Search View%>"
                                                           GroupName="Product" OnClick="rbtnFormView_OnCheckedChanged();" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddNewProduct" runat="server" Style="display: none" Text="<%$ Resources:Attendance,Add Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddNewProduct_Click" />
                                                       <%-- <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info"  OnClientClick="btnAddProductScreen_Click();return false;" />
                                                        <asp:Button ID="btnAddtoList" OnClientClick="FillProduct(); return false;" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" CssClass="btn btn-info" Visible="false"  />--%>
                                                        <button type="button" id="btnAddProductScreen" style="display: none;" class="btn btn-info" onclick="btnAddProductScreen_Click();">
                                                            Add Product List
                                                        </button>
                                                        <button type="button" id="btnAddtoList" style="display: none;" class="btn btn-info" onclick="FillProduct();">
                                                           Fill Your Product
                                                        </button>

                                                        <br />
                                                    </div>
                                                    <div id="pnlProduct1" runat="server" class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lbladdproduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div id="ProductPanel" runat="server" class="box-body">
                                                                        <div class="form-group">
                                                                            <asp:UpdatePanel ID="updPnlPayment" runat="server" UpdateMode="Conditional">                                                                              
                                                                                <ContentTemplate>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                                        <asp:TextBox ID="txtProductcode" TabIndex="12" runat="server" CssClass="form-control" onchange="txtProduct_TextChanged(); return false;" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                            ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                        <asp:TextBox ID="txtProductName" TabIndex="13" runat="server" CssClass="form-control" onchange="txtProductName_TextChanged(); return false;" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                        <asp:DropDownList ID="ddlUnit" TabIndex="15" runat="server" CssClass="form-control" />
                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPQuantity" runat="server" Text="<%$ Resources:Attendance,Quantity %>" />
                                                                                        <asp:TextBox ID="txtPQuantity" TabIndex="16" runat="server" CssClass="form-control" onchange="calculatePrice(0);return false;" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPFreeQuantity" runat="server" Text="<%$ Resources:Attendance,Free Quantity %>" />
                                                                                        <asp:TextBox ID="txtPFreeQuantity" TabIndex="17" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPFreeQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPUnitPrice" runat="server" Text="<%$ Resources:Attendance,Unit Price %>" />
                                                                                        <asp:TextBox ID="txtPUnitPrice" TabIndex="18" runat="server" CssClass="form-control" Text="0" onchange="UnitPriceCahnge();return false;" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblQuantityPrice" runat="server" Text="<%$ Resources:Attendance,Gross Price %>" />
                                                                                        <asp:TextBox ID="txtPQuantityPrice" TabIndex="18" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPDiscount" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtPDiscountPUnit" TabIndex="20" runat="server" CssClass="form-control"
                                                                                                onchange="calculatePrice(0);return false;" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                                TargetControlID="txtPDiscountPUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <div class="input-group-addon">
                                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                                            </div>
                                                                                            <div class="input-group-btn">
                                                                                                <asp:TextBox ID="txtPDiscountVUnit" TabIndex="21" Width="120px" runat="server" CssClass="form-control"
                                                                                                    onchange="calculatePrice(1);return false;"/>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtPDiscountVUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div id="Div_Tax" runat="server" class="col-md-6">
                                                                                        <asp:Label ID="lblPTax" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>"
                                                                                            Visible="false" />
                                                                                        <asp:TextBox ID="txtPTaxPUnit" runat="server" CssClass="form-control"
                                                                                            OnTextChanged="txtPTaxP_TextChanged" AutoPostBack="true" Visible="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPTaxPUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:Label ID="Label4" runat="server" Visible="false" Text="<%$ Resources:Attendance,Value %>" />
                                                                                        <asp:TextBox ID="txtPTaxVUnit" runat="server" CssClass="form-control"
                                                                                            OnTextChanged="txtPTaxV_TextChanged" AutoPostBack="true" Visible="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPTaxVUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPTotalAmount" runat="server" Text="<%$ Resources:Attendance,Net Price%>" />
                                                                                        <asp:TextBox ID="txtPTotalAmount" TabIndex="21" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                        <asp:Panel ID="pnlPDescription" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" runat="server" CssClass="form-control"
                                                                                            BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical">
                                                                                            <span runat="server" id="innerId"><asp:Literal ID="txtPDescription" runat="server"></asp:Literal></span>
                                                                                        </asp:Panel>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-12" style="text-align: center">
                                                                                        <br />
                                                                                        <asp:Button ID="btnProductSave" runat="server" OnClientClick="AddProductDetail(); return false;" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                            CssClass="btn btn-primary" />

                                                                                        <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                            CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                                                        <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                                        <br />
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                   <asp:AsyncPostBackTrigger ControlID="btnProductCancel" EventName="Click" />                                                                       
                                                                                    <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />                                                                         
                                                                                </Triggers>
                                                                             
                                                                            </asp:UpdatePanel>
                                                                    
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br/>
                                                    </div>
                                                    <asp:UpdatePanel ID="updPnlDetail" runat="server" UpdateMode="Conditional">
                                                        <Triggers>
                                                     <%--       <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />--%>
                                                            <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnProductSave" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddNewProduct" />                                                         
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlDetail" runat="server">
                                                             <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div style="overflow: auto; max-height: 500px;" class="grid">
                                                                            <table id="tblProduct" class="table-striped table-bordered table table-hover">
                                                                                <thead>                                                                                    <tr>
                                                                                    <th colspan="1">Delete</th>
                                                                                    <th colspan="1">S No.</th>
                                                                                    <th colspan="3">Product Detail</th>
                                                                                    <th colspan="3">Quantity</th>
                                                                                    <th colspan="1">Unit</th>
                                                                                    <th colspan="1">Gross Price</th>
                                                                                    <th colspan="2">Discount</th>
                                                                                    <th colspan="2">Tax</th>
                                                                                    <th colspan="1">Total</th>
                                                                                    <th colspan="1">Is Production</th>
                                                                                    <th colspan="1">Stock</th>
                                                                                    <th colspan="1">Commission</th>
                                                                                    <tr style="background: #f0f0f0;">
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">Product Id</th>
                                                                                        <th scope="col">
                                                                                            <span id="tblProductName">Product Name</span> &nbsp;<asp:CheckBox ID="ViewFullName" onchange = "ViewFullProduct()" runat="server" />                                                                                         
                                                                                        </th>
                                                                                        <th scope="col">Unit</th>
                                                                                        <th scope="col">Quantity</th>
                                                                                        <th scope="col">Free</th>
                                                                                        <th scope="col">Remain</th>
                                                                                        <th scope="col">Price</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">%</th>
                                                                                        <th scope="col">Value</th>
                                                                                        <th scope="col">%</th>
                                                                                        <th scope="col">Value</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                        <th scope="col">&nbsp;</th>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody>
                                                                                </tbody>
                                                                            </table>                                                                    
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            </asp:Panel>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Total %>" />
                                                                <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="form-control"
                                                                    OnTextChanged="txtAmount_TextChanged" AutoPostBack="true" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtDiscountP" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                       
                                                                    <cc1:FilteredTextBoxExtender ID="filtertextbox13" runat="server" Enabled="True" TargetControlID="txtDiscountP"
                                                                        ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <div class="input-group-addon">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                    </div>
                                                                    <div class="input-group-btn">
                                                                        <asp:TextBox ID="txtDiscountV" runat="server" Width="120px" CssClass="form-control" ReadOnly="true" />
                                                                          
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                            TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblafterDiscountPrice" runat="server" Text="<%$ Resources:Attendance,Price After Discount %>"
                                                                    Visible="false" />
                                                                <asp:TextBox ID="txtPriceAfterDiscount" runat="server" CssClass="form-control" ReadOnly="True"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12" style="display: none">
                                                                <div style="overflow: auto; max-height: 500px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gridView" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                        Width="100%" DataKeyNames="Tax_Id" ShowFooter="true" OnRowCancelingEdit="gridView_RowCancelingEdit"
                                                                        OnRowDeleting="gridView_RowDeleting" OnRowEditing="gridView_RowEditing" OnRowUpdating="gridView_RowUpdating"
                                                                        OnRowCommand="gridView_RowCommand">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Tax Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTaxname" runat="server" Text='<%#Eval("TaxName") %>'></asp:Label>
                                                                                    <asp:HiddenField ID="hdnTaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtTaxName" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                         CssClass="form-control" BackColor="#eeeeee"
                                                                                        Text='<%#Eval("TaxName") %>' CausesValidation="false"></asp:TextBox>
                                                                                    <a style="color: Red">*</a>
                                                                                    <cc1:AutoCompleteExtender ID="autoComplete122566" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxName" UseContextKey="True"
                                                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                </EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTaxFooter" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                        OnTextChanged="txtTaxFooter_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                        CausesValidation="true"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="autoComplete12256660" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxFooter"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tax(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTaxper" runat="server" Text='<%#Eval("Tax_Per") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtTaxper" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_Per") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488taxper" runat="server"
                                                                                        Enabled="True" TargetControlID="txtTaxper" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTaxperFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_per") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxperFooter_OnTextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2taxper" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTaxperFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tax Value">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTaxValue" runat="server" Text='<%#Eval("Tax_Value") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtTaxValue" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTaxValueFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxValueFooter_OnTextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTaxValueFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <EditItemTemplate>
                                                                                    <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                        CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                                    <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                    <asp:Button ID="ButtonDelete" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblPriceAfterTax" runat="server" Text="<%$ Resources:Attendance,Price After Tax %>"
                                                                    Visible="false" />
                                                                <asp:TextBox ID="txtPriceAfterTax" runat="server" CssClass="form-control" ReadOnly="True"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                                <asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" OnTextChanged="txtTaxP_TextChanged"
                                                                    AutoPostBack="true" Enabled="false" />
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                <asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" 
                                                                    AutoPostBack="true" Enabled="false" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                    TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                    TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>

                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTotalAmount" runat="server" Text="<%$ Resources:Attendance, Net Price %>" />
                                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblShippingCharge" runat="server" Text="<%$ Resources:Attendance,Shipping Charge %>" />
                                                                <asp:TextBox ID="txtShippingCharge" Text="0.000" TabIndex="22" runat="server" CssClass="form-control" onchange="handleShippingChargeChange(); return false;"/>

                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                    TargetControlID="txtShippingCharge" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblNetAmount" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                                <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                <br />
                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="col-md-12" id="ctlRemark" runat="server">
                                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark%>" />
                                                        <asp:TextBox ID="txtRemark" TabIndex="23" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabProductPaymentMode" runat="server" HeaderText="Advance Payment">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabProductPaymentMode" runat="server">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlTabPaymentMode" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                                                       <asp:DropDownList ID="ddlTabPaymentMode" runat="server" CssClass="form-control"
                                                                                             onchange="ddlPaymentMode_Selected()">
                                                                                        </asp:DropDownList>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayAmount" runat="server" CssClass="form-control" OnTextChanged="txtPayAmount_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPayAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div id="pnlpaybank" runat="server" class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            OnTextChanged="txtPayAccountNo_TextChanged" BackColor="#eeeeee"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPayAccountNo"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayBank" runat="server" Text="<%$ Resources:Attendance,Bank %>"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:DropDownList ID="ddlPayBank" runat="server" CssClass="form-control" Visible="false">
                                                                                        </asp:DropDownList>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div id="trcheque" runat="server" visible="false">
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblPayChequeNo" runat="server" Text="<%$ Resources:Attendance,Cheque No %>"></asp:Label>
                                                                                            <asp:TextBox ID="txtPayChequeNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblPayChequeDate" runat="server" Text="<%$ Resources:Attendance,Cheque Date %>"></asp:Label>
                                                                                            <asp:TextBox ID="txtPayChequeDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtChequedate_CalenderExtender" runat="server" TargetControlID="txtPayChequeDate">
                                                                                            </cc1:CalendarExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div id="trcard" runat="server" visible="false">
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblPayCardNo" runat="server" Text="<%$ Resources:Attendance,Card No %>"></asp:Label>
                                                                                            <asp:TextBox ID="txtPayCardNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblPayCardName" runat="server" Text="<%$ Resources:Attendance,Card Name %>"></asp:Label>
                                                                                            <asp:TextBox ID="txtPayCardName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <br />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtExchangerate" runat="server" CssClass="form-control" Text="1" Enabled="false"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label16" runat="server" Text="Local Amount"></asp:Label>
                                                                                        <asp:TextBox ID="txtLocalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-12" style="text-align: center">
                                                                                        <asp:Button ID="Btn_Payment_Save" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Add %>"  OnClientClick="AddPayment(); return false;" />
                                                                                        <br /><br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;" class="grid">
                                                                                        <br />
                                                                                        <table id="tblPayment" class="table-striped table-bordered table table-hover">
                                                                                            <thead>
                                                                                                <tr>
                                                                                                    <th>Delete</th>
                                                                                                    <th>Payment Mode</th>
                                                                                                    <th>Account Name</th>
                                                                                                    <th>Net Price</th>
                                                                                                    <th>Exchange Rate</th>
                                                                                                    <th>Payment Ammount (Local)</th>                                                                                                   
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <br />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                        <Triggers>
<%--                                                                            <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />--%>
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_TabProductPaymentMode">
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


                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbldeliveryVoucher" runat="server" Text="Create Delivery Voucher After Sales Invoice"
                                                            Visible="false" />
                                                        <asp:DropDownList ID="ddlDeliveryvoucher" runat="server" CssClass="form-control" Visible="false">
                                                            <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                                            <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:CheckBox ID="chksendInproduction" runat="server" Text="Send In Production" Visible="false" />
                                                        <asp:CheckBox ID="chkSendInPO" runat="server" Style="margin-left: 20px;" Text="<%$Resources:Attendance, Send In Purchase Order%>" Visible="false" />
                                                        <asp:CheckBox ID="chkSendInProjectManagement" Style="margin-left: 20px;" runat="server" Text="Send In Project Management" />
                                                        <asp:CheckBox ID="chkPartialShipment" runat="server" Style="margin-left: 20px;" Text="<%$Resources:Attendance,Partial Shipment%>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <asp:UpdatePanel ID="Update_Save" runat="server">
                                                        <Triggers>
                                                           <%-- <asp:AsyncPostBackTrigger ControlID="nSOrderSave" />--%>
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <div class="col-md-12" style="text-align: left">

                                                                <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                                                <asp:Button ID="btnSOrderSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    CssClass="btn btn-success"  OnClientClick="btnSaveClick(); return false;"  Visible="false" />
                                                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="btn btn-primary" CausesValidation="False" OnClientClick="BtnReset_Click(); return false;" />
                                                                <asp:Button ID="btnSOrderCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    CausesValidation="False" OnClientClick="BtnReset_Click();return false;" />
                                                                <asp:HiddenField ID="editid" runat="server" />

                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnSalesQuotationId" runat="server" Value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                   <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name%>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer No. %>" Value="QauotationNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Payment Mode %>" Value="PaymentModeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>

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
                                                    <asp:UpdatePanel ID="updPnlBin" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>

                                                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrderBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesOrderBin_PageIndexChanging"
                                                                OnSorting="GvSalesOrderBin_OnSorting" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                            <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="TransType">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("TransType") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer No.%>" SortExpression="QauotationNo">
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
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
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
                                                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Quotation">
                        <asp:UpdatePanel ID="Update_Quotation" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                   <asp:Label ID="lblTotalRecordsQuote" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">


                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameQuote" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameQuote_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Id %>" Value="SQuotation_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customer_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionQuote" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindQuote">
                                                        <asp:TextBox ID="txtValueQuote" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueQuoteDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueQuoteDate" runat="server" TargetControlID="txtValueQuoteDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindQuote" runat="server" CausesValidation="False" OnClick="btnbindrptQuote_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshQuote" runat="server" CausesValidation="False" OnClick="btnRefreshQuoteReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel_sales" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="flow">
                                                            <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                            <asp:HiddenField ID="HDN_Sales_Quotation_ID" runat="server" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesQuotation" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesQuotation_PageIndexChanging"
                                                                AllowSorting="True" OnSorting="GvSalesQuotation_Sorting" CurrentSortField="SQuotation_Id" CurrentSortDirection="DESC">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None" TabIndex="76" CausesValidation="False" CommandName='<%#Eval("Location_Id") %>' CommandArgument='<%# Eval("SQuotation_Id") + "," + Eval("Customer_id")  %>' CssClass="btn fa fa-angle-left" OnCommand="btnSIEdit_Command"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Id %>" SortExpression="SQuotation_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSQuotationId" runat="server" Text='<%#Eval("SQuotation_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="SQuotation_No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSINo" runat="server" Text='<%#Eval("SQuotation_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="Quotation_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuotationDate" runat="server" Text='<%#GetDate(Eval("Quotation_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" SortExpression="EmployeeName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="Customer_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCustomerName1" runat="server" Text='<%# Eval("Customer_Name") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="TotalAmount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvamtId" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TotalAmount").ToString(),Eval("CurrencyDecimalCount").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>

                                                            <asp:HiddenField ID="hdnGvSqCurrentPageIndex" Value="1" runat="server" />
                                                        </div>
                                                        <br />

                                                        <asp:Repeater ID="rptGvSqPager" runat="server">
                                                            <ItemTemplate>
                                                                <ul class="pagination">
                                                                    <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                        <asp:LinkButton ID="lnkGvSqPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                            CssClass="page-link"
                                                                            OnClick="GvSq_Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="Btn_Quotation" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="UpdatePanel_sales">
                                                    <ProgressTemplate>
                                                        <div class="modal_Progress">
                                                            <div class="center_Progress">
                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
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

    <asp:UpdatePanel ID="Update_Report" runat="server">
        <ContentTemplate>
            <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
                aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                            </h4>
                        </div>
                        <div class="modal-body">
                            <RS:ReportSystem runat="server" ID="reportSystem" />
                        </div>
                        <div class="modal-footer">
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div class="modal fade" id="creditTermsNConditions" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="box-title">
                        <asp:Label ID="Label22" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true" runat="server" Text="Credit Terms & Condition"></asp:Label>
                    </h3>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-6">
                            <asp:Label ID="lblCreditLimit" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Limit %>" />
                            &nbsp:&nbsp<asp:Label ID="lblCreditLimitValue" runat="server"></asp:Label>
                            <asp:Label ID="lblCurrencyCreditLimit" runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblCreditDays" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Days %>" />
                            &nbsp:&nbsp<asp:Label ID="lblCreditDaysValue" runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label47" runat="server" Font-Bold="true" Text="Credit Parameter" />
                            &nbsp:&nbsp<asp:Label ID="lblCreditParameterValue" runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Current Balance" />
                            &nbsp:&nbsp<asp:Label ID="lblCustomercurrentBalance" runat="server"></asp:Label>
                            <br />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">

                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
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

    <asp:Panel ID="pnlorderreport" runat="server" Visible="false">
        <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
            Width="100%" AccessibilityCompliant="True">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        <dx:ReportViewer ID="rptViewer" runat="server">
        </dx:ReportViewer>
    </asp:Panel>

    <div class="modal fade" id="Modal_GST" tabindex="-1" role="dialog" aria-labelledby="Modal_GSTLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_GSTLabel">TAX Calculation
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_GST" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                   <table id="TblTaxCalculation" width="100%" class="table-bordered">
                                       <thead>
                                           <tr>
                                               <th>Tax Name</th>
                                               <th>Tax Value</th>
                                           </tr>
                                       </thead>
                                       <tbody>

                                       </tbody>
                                   </table>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<asp:Button ID="btnSaveGST" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="S_update"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSaveGST_Click" />

                            <asp:Button ID="btnCancelGST" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Reset %>" Visible="false" />--%>
                            <asp:HiddenField ID="Hdn_Serial_No_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Product_Id_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_unit_Price_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Discount_Tax" runat="server" />
                            <asp:Button ID="Btn_Update_Tax" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>"  />
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="NewAddress" tabindex="-1" role="dialog" aria-labelledby="NewAddress_ModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AA1:AddAddress ID="addaddress" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Product Description</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label id="pDescription" class="control-label"></label>
                </div>                
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress15" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Quotation">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Save">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="modal fade" id="ModelAcMaster" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <uc1:AccountMaster runat="server" ID="UcAcMaster" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
     <div id="prgBar" class="modal_Progress" style="display: none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script> 
 
    <script type="text/javascript">
        //This function create by Rahul Of Binding in List
        $(document).ready(function () {
            
            GetSalesOrderList();
           
        });

        function UnitPriceCahnge()
        {
            var unitPrice = $('#<%=txtPUnitPrice.ClientID%>').val();
            var productId = $('#<%=hdnNewProductId.ClientID%>').val();
            if (productId != null && productId != "")
            {
                var Customer = $('#<%=txtCustomer.ClientID%>').val();

                $.ajax({
                    url: 'SalesOrderJScript.aspx/GetProductPrice',
                    method: 'post',
                    contentType: "application/json;",
                    data: "{ProductId:" + productId + ",CustomerID:\"" + Customer + "\"}",
                    dataType: 'json',
                    success: function (data) {
                        var response = data.d;
                        var salesPrice = parseFloat(response);
                        if (unitPrice < salesPrice) {
                            alert("Sales Price not less than " + salesPrice);
                            $("#prgBar").css("display", "none");
                            $('#<%=txtPUnitPrice.ClientID%>').val(salesPrice);
                            // Move the code that depends on salesPrice here
                            calculatePrice(0);
                            return;
                        } else {
                            calculatePrice(0);
                        }
                        $("#prgBar").css("display", "none");
                    },
                    error: function (request, status, error) {
                        alert(request.responseText);
                        $("#prgBar").css("display", "none");
                    }
                });
            }
        }
        function rbtnFormView_OnCheckedChanged() {
            
              var btnAddProductScreen = document.getElementById('btnAddProductScreen');
        var btnAddtoList = document.getElementById('btnAddtoList');
        
            var radioButton = document.getElementById('<%= rbtnFormView.ClientID %>');
             var divElement = document.getElementById('<%= pnlProduct1.ClientID %>');               
            // Check if the RadioButton is checked
            if (radioButton.checked) {
                btnAddProductScreen.style.display = 'none'; // Hide the button
                btnAddtoList.style.display = 'none';
                // Check if the div element exists
                if (divElement) {
                    // Hide the div by setting its display property to "none"
                    divElement.style.display = 'block';

                }
            } else {
                btnAddProductScreen.style.display = 'block'; // Show the button
                btnAddtoList.style.display = 'block';
                divElement.style.display = 'none';
            }
        }
        function FillProduct() {
               $.ajax({
                url: 'SalesOrderJScript.aspx/FillProducts',
                method: 'post',
                contentType: "application/json;",
                dataType: 'json',
                success: function (data) {
                    var myArr = JSON.parse(data.d);
                    debugger;
                    ProductDetail = myArr;
                    Product_Tblbind(); 
                    //alert(data);
                }
            });
        }
        function btnAddProductScreen_Click() {
           var txtCustomer = $('#<%=txtCustomer.ClientID%>').val(); 
            if (txtCustomer == "") {
                alert("Please Add Customer");
                $('#<%=txtProductcode.ClientID%>').val('');
                return;
            }
           const parts = txtCustomer.split('/');

            // Access the last element of the `parts` array
           const lastPart = parts[1];
           ///alert(lastPart);
           window.open('../Inventory/AddItem.aspx?Page=SO&&CustomerId=' + lastPart + '', 'window', 'width=1024');
        
        }
        function btnRefreshReport_Click() {
            GetSalesOrderList();
        }
        function btnNewCustomer() {
           // window.open('../EMS/ContactMaster.aspx', 'window', 'width=1024, ');
            window.open('../Sales/AddContact.aspx?Page=SINV', 'window', 'width=1024');
        }
        function txtProduct_TextChanged() {
            var ProductCode = $('#<%=txtProductcode.ClientID%>').val();
            var txtCustomer = $('#<%=txtCustomer.ClientID%>').val(); 
            if (txtCustomer == "") {
                alert("Please Add Customer");
                $('#<%=txtProductcode.ClientID%>').val('');
                return;
            }
            $.ajax({
                url: 'SalesOrderJScript.aspx/txtProduct_TextChanged',
                method: 'post',
                contentType: "application/json;",
                data: "{'ProductCode':'" + ProductCode + "','Customer':'" + txtCustomer + "'}",
                dataType: 'json',
                success: function (data) {
                    var myArr = JSON.parse(data.d);
                    for(var d = 0; d < myArr.length; d++)
                    {
                       
                        $('#<%=hdnNewProductId.ClientID%>').val(myArr[d].ProductId);
                        $('#<%=txtProductName.ClientID%>').val(myArr[d].ProductName);
                        $('#<%=ddlUnit.ClientID%>').val(myArr[d].UnitId);
                        $('#<%=ddlUnit.ClientID%>').find('option:selected').text(myArr[d].UnitName);
                        $('#<%=txtPQuantity.ClientID%>').val("0.000");
                        $('#<%=txtPFreeQuantity.ClientID%>').val("0.000");
                        $('#<%=txtPUnitPrice.ClientID%>').val(myArr[d].UnitPrice);
                        $('#<%=txtPQuantityPrice.ClientID%>').val(myArr[d].UnitPrice);
                        $('#<%=txtPDiscountPUnit.ClientID%>').val("0.000"); 
                        $('#<%=txtPDiscountVUnit.ClientID%>').val("0.000");    
                        $('#<%=txtPTotalAmount.ClientID%>').val(myArr[d].UnitPrice);  
                    }     
                }
            });
        }
        function ddlTransTypeChange()
        {
            Product_Tblbind();
        }
        function txtProductName_TextChanged() {
            var ProductName = $('#<%=txtProductName.ClientID%>').val();
            var txtCustomer = $('#<%=txtCustomer.ClientID%>').val(); 
            if (txtCustomer == "") {
                alert("Please Add Customer");
                 $('#<%=txtProductName.ClientID%>').val('');
                return;
            }
            $.ajax({
                url: 'SalesOrderJScript.aspx/txtProductName_TextChanged',
                method: 'post',
                contentType: "application/json;",
                data: "{'ProductName':'" + ProductName + "','Customer':'" + txtCustomer + "'}",
                dataType: 'json',
                success: function (data) {
                    var myArr = JSON.parse(data.d);
                    for(var d = 0; d < myArr.length; d++)
                    {                       
                        $('#<%=hdnNewProductId.ClientID%>').val(myArr[d].ProductId);
                        $('#<%=txtProductcode.ClientID%>').val(myArr[d].ProductCode);
                        $('#<%=ddlUnit.ClientID%>').val(myArr[d].UnitId);
                        $('#<%=ddlUnit.ClientID%>').find('option:selected').text(myArr[d].UnitName);
                        $('#<%=txtPQuantity.ClientID%>').val("0.000");
                        $('#<%=txtPFreeQuantity.ClientID%>').val("0.000");
                        $('#<%=txtPUnitPrice.ClientID%>').val(myArr[d].UnitPrice);
                        $('#<%=txtPQuantityPrice.ClientID%>').val(myArr[d].UnitPrice);
                        $('#<%=txtPDiscountPUnit.ClientID%>').val("0.000"); 
                        $('#<%=txtPDiscountVUnit.ClientID%>').val("0.000");    
                        $('#<%=txtPTotalAmount.ClientID%>').val(myArr[d].UnitPrice);  
                    }
                }
            });
        }
        function ddlPosted_SelectedIndexChanged() {
            btnbindrpt_Click();
        }
        function btnbindrpt_Click()
        {
                 
          var ddlLocation = $('#<%=ddlLocation.ClientID%>').val();
          var ddlUser=$('#<%=ddlUser.ClientID%>').val();
          var ddlPosted = $('#<%=ddlPosted.ClientID%>').val();
          var ddlFieldName = $('#<%=ddlFieldName.ClientID%>').val();
          var ddlOption = $('#<%=ddlOption.ClientID%>').val();
          var txtValue = $('#<%=txtValue.ClientID%>').val();
            $.ajax({
                url: 'SalesOrderJScript.aspx/btnbindrpt_Click',
                method: 'post',
                contentType: "application/json;",
                data: "{'ddlLocation':'" + ddlLocation + "','ddlUser':'" + ddlUser + "','ddlPosted':'" + ddlPosted + "','ddlFieldName':'" + ddlFieldName + "','ddlOption':'" + ddlOption + "','txtValue':'" + txtValue + "'}",
                dataType: 'json',
                beforeSend: function () {                    $("#prgBar").css("display", "block");                },                complete: function () {                    $("#prgBar").css("display", "none");                },
                success: function (data) {
                    var response = data.d[0];
                    var myArr = JSON.parse(response);
                    console.log(myArr);
                    var table = $('#tblOrderList').DataTable();
                    table.clear().destroy();                
                    var table = $('#tblOrderList').DataTable({
                        // DataTable configuration options
                        // For example, searching, paging, etc.
                    });
                    $('#<%=lblTotalRecords.ClientID%>').text("Total Records:" + myArr.length+"");
                    for (var i = 0; i < myArr.length; i++) {
                        var htmlRow = "<tr>";
                        htmlRow = htmlRow + "<td><div class='dropdown' style='position: absolute;'><button class='btn btn-default dropdown-toggle' style='align:right' type='button' data-toggle='dropdown'><i class='fa fa-ellipsis-h' aria-hidden='true'></i></button>";
                        htmlRow = htmlRow + "<ul class='dropdown-menu'>";
                        htmlRow = htmlRow + "<li><a href=''  onclick='PritOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-print'></i>Print</a></li>";
                        htmlRow = htmlRow + "<li><a href=''  onclick='PritOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-print'></i>Report System</a></li>";
                        if (myArr[i].SOfromTransType == "Q") {

                            htmlRow = htmlRow + "<li><a href=''   onclick='ViewSalesOrder(" + myArr[i].SOfromTransNo + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                            htmlRow = htmlRow + "<li><a href='' onclick='editSalesOrder(" + myArr[i].SOfromTransNo + ");return false;'><i class='fa fa-pencil'></i>Edit</a></li>";
                        }
                        else {
                            htmlRow = htmlRow + "<li><a href=''   onclick='ViewSalesOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                            htmlRow = htmlRow + "<li><a href='' onclick='editSalesOrder(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-pencil'></i>Edit</a></li>";
                        }
                        htmlRow = htmlRow + "<li><a href='' onclick='DeleteOrder(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-trash'></i>Delete</a></li>";
                        htmlRow = htmlRow + "<li><a href='' onclick='btnFileUpload(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-upload'></i>File Upload</a></li>";
                        htmlRow = htmlRow + "</ul></div></td>";
                        //htmlRow = htmlRow + "<td>" + myArr[0].RowNumber + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].SalesOrderNo + "</td>";
                        var date = formatDate(myArr[i].SalesOrderDate);
                        htmlRow = htmlRow + "<td>" + date + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].CustomerName + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].Field3 + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].TransType + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].QauotationNo + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].Field4 + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].PaymentModeName + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].CreatedBy + "</td>";
                        htmlRow = htmlRow + "<td>" + getAmountWithDecimal(myArr[i].NetAmount) + "</td>";
                        htmlRow = htmlRow + "</tr>";
                        //  $('#tblOrderList > tbody:last-child').append(htmlRow);
                        table.row.add($(htmlRow)).draw();
                    }                   
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });
            $("#prgBar").css("display", "none");
        }
        function GetSalesOrderList() {
          <%--  $("#prgBar").css("display", "block");
            $.ajax({
                url: 'SalesOrderJScript.aspx/FillSalesOrderList',
                method: 'post',
                contentType: "application/json;",                
                dataType: 'json',
                success: function (data) {
                    var response = data.d;                   
                    var myArr = JSON.parse(response);
                    console.log(myArr);
                    $('#<%=lblTotalRecords.ClientID%>').text("Total Records:" + myArr.length+ "");
                    var table = $('#tblOrderList').DataTable();
                    table.clear().destroy();
                    var table = $('#tblOrderList').DataTable({
                        // DataTable configuration options
                        // For example, searching, paging, etc.
                    });
                    for (var i = 0; i < myArr.length; i++)
                    {
                        var htmlRow = "<tr>";
                        htmlRow = htmlRow + "<td><div class='dropdown' style='position: absolute;'><button class='btn btn-default dropdown-toggle' style='align:right' type='button' data-toggle='dropdown'><i class='fa fa-ellipsis-h' aria-hidden='true'></i></button>";
                        htmlRow = htmlRow + "<ul class='dropdown-menu'>"; 
                        htmlRow = htmlRow + "<li><a href=''  onclick='PritOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-print'></i>Print</a></li>";
                        htmlRow = htmlRow + "<li><a href=''  onclick='PritOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-print'></i>Report System</a></li>";
                        if (myArr[i].SOfromTransType == "Q")
                        {

                            htmlRow = htmlRow + "<li><a href=''   onclick='ViewSalesOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                            htmlRow = htmlRow + "<li><a href='' onclick='editSalesOrder(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-pencil'></i>Edit</a></li>";
                        }
                        else {
                            htmlRow = htmlRow + "<li><a href=''   onclick='ViewSalesOrder(" + myArr[i].Trans_Id + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                            htmlRow = htmlRow + "<li><a href='' onclick='editSalesOrder(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-pencil'></i>Edit</a></li>";
                        }
                        htmlRow = htmlRow + "<li><a href='' onclick='DeleteOrder(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-trash'></i>Delete</a></li>";
                        htmlRow = htmlRow + "<li><a href='' onclick='btnFileUpload(" + myArr[i].Trans_Id + ");return false;'><i class='fa fa-upload'></i>File Upload</a></li>";
                        htmlRow = htmlRow + "</ul></div></td>";
                        //htmlRow = htmlRow + "<td>" + myArr[0].RowNumber + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].SalesOrderNo + "</td>";
                        var date = formatDate(myArr[i].SalesOrderDate);
                        htmlRow = htmlRow + "<td>" + date + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].CustomerName + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].Field3 + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].TransType + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].QauotationNo + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].Field4 + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].PaymentModeName + "</td>";
                        htmlRow = htmlRow + "<td>" + myArr[i].CreatedBy + "</td>";
                        htmlRow = htmlRow + "<td>" + getAmountWithDecimal(myArr[i].NetAmount) + "</td>";
                        htmlRow = htmlRow + "</tr>";
                        // $('#tblOrderList > tbody:last-child').append(htmlRow);
                        table.row.add($(htmlRow)).draw();
                    }                   
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });--%>
            btnbindrpt_Click();
        }
        function ViewSalesOrder(TransId)
        {
            $('#<%=btnSOrderSave.ClientID%>').css("display", "none");
            editSalesOrder(TransId);
             $('#<%= Lbl_Tab_New.ClientID %>').text("View");
        }

        function DeleteOrder(TransID)
        {
            $("#prgBar").css("display", "block");
            $.ajax({
                url: 'SalesOrderJScript.aspx/DeleteOrder',
                method: 'post',
                contentType: "application/json;",
                data: "{TransId:" + TransID + "}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    alert(response[0]);
                    GetSalesOrderList();
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });
        }
        function PritOrder(TransID) {
            window.open('../Sales/SalesOrder_Print.aspx?Id='+ TransID+'');
        }

      
        function btnFileUpload(TransID)
        {
            Modal_Open_FileUpload();
        }
        //function create for Sales Order Edit
        function editSalesOrder(TransId)
        {
            setTimeout(function () { $("#prgBar").css("display", "block") }, 500);
            
            $.ajax({
                url: 'SalesOrderJScript.aspx/EditCommand',
                method: 'post',
                contentType: "application/json;",
                data: "{TransID:" + TransId + "}",
                dataType: 'json',
                success: function (data) {
                    var text = JSON.parse(data.d);                    
                    $('#<%=txtAgentName.ClientID%>').val(text[0].AgentName);
                    $('#<%=txtSODate.ClientID%>').val(text[0].OrderDate);
                    $('#<%=txtSONo.ClientID%>').val(text[0].OrderNo); 
                    $('#<%=hdnSalesQuotationId.ClientID%>').val(text[0].hdnQutationID); 
                    $('#<%= ddlOrderType.ClientID %>').val(text[0].OrderType);
                    $('#<%=txtCustOrderNo.ClientID%>').val(text[0].CustOrderNo);
                    $('#<%= ddlCurrency.ClientID %> option:selected').val(text[0].Currency);
                    $('#<%= ddlCurrency.ClientID %> option:selected').val(text[0].Currency);
                    $('#<%=ddlPaymentMode.ClientID%>').val(text[0].PaymentMode);
                    $('#<%=txtSalesPerson.ClientID%>').val(text[0].SalesPerson);
                    $('#<%=hdnSalesPersonId.ClientID%>').val(text[0].hdnSalesPersonId);
                    $('#<%=txtCustomer.ClientID%>').val(text[0].CustomerName); 
                    $('#<%=txtContactPerson.ClientID%>').val(text[0].ContactPerson);
                    $('#<%=hdnContactPersonId.ClientID%>').val(text[0].hdnContactPersonId);
                    $('#<%=txtEstimateDeliveryDate.ClientID%>').val(text[0].DeliveryDate);
                    $('#<%=txtShipCustomerName.ClientID%>').val(text[0].ShipTo);
                    $('#<%=txtShipingAddress.ClientID%>').val(text[0].ShippingAddress);
                    $('#<%=txtInvoiceTo.ClientID%>').val(text[0].InvoiceAddress); 
                    $('#<%=txtAmount.ClientID%>').val(text[0].GrossAmount); 
                    $('#<%=txtQuotationNo.ClientID%>').val(text[0].QuotationNo);
                    $('#<%=txtTaxP.ClientID%>').val(text[0].TaxPercent);
                    $('#<%=txtTaxV.ClientID%>').val(text[0].Taxvalue);
                    $('#<%=ddlTransType.ClientID%>').val(text[0].ddlTransType);
                    if (text[0].OrderType === "Q")
                    {
                         
                        $('#<%=txtTransFrom.ClientID%>').val("Sales Quotation");
                        var updatePanel = document.getElementById('<%= updPnlPayment.ClientID %>');
                        if (updatePanel) {
                            updatePanel.style.display = 'none';
                        }                     
                        var productPanel = document.getElementById('<%= ProductPanel.ClientID %>');
                        if (productPanel) {
                            productPanel.style.display = 'none';
                        }
                    }
                    else
                    {
                         $('#<%=txtTransFrom.ClientID%>').val("Direct");
                    }
                    $('#<%=txtDiscountP.ClientID%>').val(text[0].DiscountPercent);
                    $('#<%=txtDiscountV.ClientID%>').val(text[0].DiscountValue);
                    $('#<%=txtPriceAfterDiscount.ClientID%>').val(text[0].NetAmount);
                    $('#<%=txtTotalAmount.ClientID%>').val(text[0].NetAmount);                    
                    $('#<%=txtShippingCharge.ClientID%>').val(getAmountWithDecimal(text[0].ShippingCharge));
                    $('#<%=editid.ClientID%>').val(text[0].editId);
                    $('#<%=txtRemark.ClientID%>').val(text[0].Remark); 
                    ProductDetail = text[0].ProductDetails;
                    PaymentDetail = text[0].PaymentDetails;
                    AddPaymentDetail();
                    Product_Tblbind();
                    ShipingAddressDetail();
                    GetInvoiceAddress();
                    $('#<%=txtNetAmount.ClientID%>').val(getAmountWithDecimal(text[0].NetAmount + text[0].ShippingCharge));
                    console.log(text);
                    //var hangoutButton = document.getElementById("hangout-giLkojRpuK");
                     $('#<%= Lbl_Tab_New.ClientID %>').text("Edit");
                    Li_Tab_New();
                    setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                }
            });
            setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
        }

        //format date function
        function formatDate(dateString) {
            var months = [
              "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            ];

            var date = new Date(dateString);
            var day = date.getDate();
            var monthIndex = date.getMonth();
            var year = date.getFullYear();

            return day + "-" + months[monthIndex] + "-" + year;
        }

        function GetTaxPercentage() {
            $.ajax({
                url: 'SalesOrderJScript.aspx/GetProductTax',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductID + "}",
                dataType: 'json',
                success: function (data) {
                    var text = JSON.parse(data.d);
                    var TaxArray = Object.keys(text).length;
                    $('#TblTaxCalculation > tbody > tr').remove();
                    for (var i = 0; i < TaxArray; i++)
                    {
                        var htmlRow = "<tr>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Name + "</td>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Value + "</td>";
                        htmlRow = htmlRow + "</tr>";
                        $('#TblTaxCalculation > tbody:last-child').append(htmlRow);
                    }
                    document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
                }
            })
        }
        function GetProductTaxPercentage(ProductId)
        {
            if ($('#<%=ddlTransType.ClientID%>').find('option:selected').val() == null || $('#<%=ddlTransType.ClientID%>').find('option:selected').val() == "") {
                return 0;
            }
            var TransType = $('#<%=ddlTransType.ClientID%>').find('option:selected').val();           
            debugger;
               var taxPercentage = "0";
            $.ajax({
                url: 'SalesOrderJScript.aspx/GetAddTaxPercentage',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductId + ",transType:" + TransType + "}",
                dataType: 'json',
                async:false,
                success: function (data) {
                    debugger;
                    var response = data.d;
                    taxPercentage = response;   
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    console.log(request.responseText);
                }
            });
           return taxPercentage;
        }
        // This Function Create By Rahul Sharma And The ProductDetail is A global Object for Detail Bind In Order
        var ProductDetail = {};
        //This PaymentDetail Object Create For Payment Detail Created by Rahul Sharma
        var PaymentDetail = {};
        function AddProductDetail() {
            
            var ProductId = $('#<%=txtProductcode.ClientID%>').val();
            if (ProductId == "") {
                alert('Please enter Product');
                return;
            }
            var hdnNewProductId=$('#<%=hdnNewProductId.ClientID%>').val();
            var ProductName = $('#<%=txtProductName.ClientID%>').val();
            var UnitId = $('#<%=ddlUnit.ClientID%>').val();
            if (UnitId == "--Select--") {
                alert('Please Select Unit')
                return;
            }
            var UnitName = $('#<%=ddlUnit.ClientID%>').find('option:selected').text();
            var Quantity = getAmountWithDecimal($('#<%=txtPQuantity.ClientID%>').val());
            if (Quantity == "") {
                Quantity = "0";
            }
            var FreeQuantity = getAmountWithDecimal($('#<%=txtPFreeQuantity.ClientID%>').val());
            if (FreeQuantity == "") {
                FreeQuantity = "0";
            }
            var UnitPrice = getAmountWithDecimal($('#<%=txtPUnitPrice.ClientID%>').val());
            if (UnitPrice == "") {
                UnitPrice = "0";
            }
            var GrossPrice = getAmountWithDecimal($('#<%=txtPQuantityPrice.ClientID%>').val());
            if (GrossPrice == "") {
                GrossPrice = "0";
            }
            var Discount =getAmountWithDecimal( $('#<%=txtPDiscountPUnit.ClientID%>').val());
            var DiscountValue =getAmountWithDecimal( $('#<%=txtPDiscountVUnit.ClientID%>').val());
                  var Description = $('#<%=innerId.ClientID%>').text();
            var TotalAmount = getAmountWithDecimal($('#<%=txtPTotalAmount.ClientID%>').val());
            var commision = "0.00";
            var Tax = GetProductTaxPercentage(hdnNewProductId);
            var new_arr = [];
            ProductDetail.length;
            if (Object.keys(ProductDetail).length > 0) {
                new_arr = {
                    'ProductId': ProductId,
                    'hdnNewProductId': hdnNewProductId,
                    'ChkQuatationDetail':'',
                    'ProductName': ProductName,
                    'UnitId': UnitId,
                    'UnitName':UnitName,
                    'Quantity': Quantity,
                    'FreeQuantity': FreeQuantity,
                    'UnitPrice': UnitPrice,
                    'GrossPrice': GrossPrice,
                    'Discount': Discount,
                    'DiscountValue': DiscountValue,
                    'TaxPer': '0.000',
                    'TaxVal':'0.000',
                    'Description': Description,
                    'TotalAmount': TotalAmount,
                    'AggetnCommission': commision
                };
                ProductDetail[parseInt(Object.keys(ProductDetail).length)] = new_arr;
            }
            else {
                new_arr[Object.keys(ProductDetail).length] = {
                    'ProductId': ProductId,
                    'hdnNewProductId': hdnNewProductId,
                    'ProductName': ProductName,
                    'ChkQuatationDetail':'',
                    'UnitId': UnitId,
                    'UnitName':UnitName,
                    'Quantity': Quantity,
                    'FreeQuantity': FreeQuantity,
                    'UnitPrice': UnitPrice,
                    'GrossPrice': GrossPrice,
                    'Discount': Discount,
                    'DiscountValue': DiscountValue,
                    'TaxPer': '0.000',
                    'TaxVal': '0.000',
                    'Description': Description,
                    'TotalAmount': TotalAmount,
                    'AggetnCommission': commision
                };
                ProductDetail = new_arr;
            }
          
            Product_Tblbind();
            AddPaymentDetail();
            $('#<%=txtProductcode.ClientID%>').val('');
          $('#<%=hdnNewProductId.ClientID%>').val('');
           $('#<%=txtProductName.ClientID%>').val('');
            $('#<%=ddlUnit.ClientID%>').val('');             
                $('#<%=txtPQuantity.ClientID%>').val('');
               $('#<%=txtPFreeQuantity.ClientID%>').val('');
                $('#<%=txtPUnitPrice.ClientID%>').val('');
                  $('#<%=txtPQuantityPrice.ClientID%>').val('');
                 $('#<%=txtPDiscountPUnit.ClientID%>').val('');
                 $('#<%=txtPDiscountVUnit.ClientID%>').val('');
              $('#<%=innerId.ClientID%>').text('');
                 $('#<%=txtPTotalAmount.ClientID%>').val('');
        }
        function OrderSHowByQuatation(obj) {
            $("#prgBar").css("display", "block");
            
           // var Detail = JSON.parse(obj);
            ProductDetail = obj;
            Product_Tblbind();
            var ProductArray = Object.keys(ProductDetail).length;           
            for (var p = 0; p < ProductArray; p++)
            {
                
                $("#btndelete" + p + "").css("display", "none");
                //var checkbox = document.getElementById("ChkQuatation" + p + "");
                $("#ChkQuatation"+p+"").css("display", "block");
                //checkbox.style.display = "block";               
            }
            $('#<%= Lbl_Tab_New.ClientID %>').text("Edit");
            $("#Li_Quotation").removeClass("active");
            $("#Quotation").removeClass("active");           
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
            GetInvoiceAddress();
            ShipingAddressDetail();
            $("#prgBar").css("display", "none");
            
 setTimeout(function () { jQuery('#<%=txtCustOrderNo.ClientID%>').focus() }, 500);
        }


        function GetTaxAllowOrnot() {
              $.ajax({
                  url: 'SalesOrderJScript.aspx/GetTaxPerameter',
                method: 'post',
                contentType: "application/json;",                
                dataType: 'json',
                success: function (data) {
                    var text = data.d;
                    console.log(text);
                         var table = document.getElementById("tblProduct");        

                         var headerRow = table.tHead.rows[0];
                         var headerRow2 = table.tHead.rows[1];
                    if (text === "False") {

                        // Hide the header cell
                        headerRow.cells['7'].style.display = "none";
                        headerRow2.cells['12'].style.display = "none";
                        headerRow2.cells['13'].style.display = "none";
                        // Loop through all the rows in the tbody and hide the corresponding cell
                        var tbody = table.tBodies[0];
                        var rows = tbody.rows;
                        for (var i = 0; i < rows.length; i++) {
                            rows[i].cells['12'].style.display = "none";
                            rows[i].cells['13'].style.display = "none";
                        }                       
                    }
                    else {
                         //Show the header cell
                        headerRow.cells['7'].removeAttribute('style');
                        headerRow2.cells['12'].removeAttribute('style');
                        headerRow2.cells['13'].removeAttribute('style');
                        // Loop through all the rows in the tbody and hide the corresponding cell
                        var tbody = table.tBodies[0];
                        var rows = tbody.rows;
                        for (var i = 0; i < rows.length; i++) {
                            rows[i].cells['12'].removeAttribute('style');
                            rows[i].cells['13'].removeAttribute('style');
                        }
                    }
                }
            });
        }
        function Show_Modal_GST(ProductID) {

            $.ajax({
                url: 'SalesOrderJScript.aspx/GetProductTax',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductID + "}",
                dataType: 'json',
                success: function (data) {
                    var text = JSON.parse(data.d);
                    var TaxArray = Object.keys(text).length;
                    $('#TblTaxCalculation > tbody > tr').remove();
                    for (var i = 0; i < TaxArray; i++)
                    {
                        var htmlRow = "<tr>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Name + "</td>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Value + "</td>";
                        htmlRow = htmlRow + "</tr>";
                        $('#TblTaxCalculation > tbody:last-child').append(htmlRow);
                    }
                    document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
                }
            });
           
        }
     //This Function Created for SalesPrice Get According Parameter
        function txtUnitPrice_TextChanged(row, ProductId) {
            $("#prgBar").css("display", "block");
            const unitPrice = parseFloat(document.getElementById('tblUnitPrice' + row + '').value) || 0;
            var Customer = $('#<%=txtCustomer.ClientID%>').val();

            $.ajax({
                url: 'SalesOrderJScript.aspx/GetProductPrice',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductId + ",CustomerID:\"" + Customer + "\"}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    var salesPrice = parseFloat(response);

                    if (unitPrice < salesPrice) {
                        alert("Sales Price not less than " + salesPrice);
                        $("#prgBar").css("display", "none");
                        document.getElementById('tblUnitPrice' + row + '').value = salesPrice;
                        // Move the code that depends on salesPrice here
                        calculateDetail(row, 0);
                        return;
                    } else {
                        calculateDetail(row, 0);
                    }                    
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });
        }

        //this function change Create By Rahul For Table Bind
        function Product_Tblbind() {
            $("#prgBar").css("display", "block");
            
            var ProductArray = Object.keys(ProductDetail).length;
            var LocationOut = LoctionList();
            var GrossAmount = "0";
            var NetAmount = "0";
            var UnitPrice = "0";
            var DiscountValue = "0";
            console.log(ProductDetail);
            $('#tblProduct > tbody > tr').remove();
            for(var i = 0; i < ProductArray; i++)
            {
                GrossAmount = parseFloat(GrossAmount) + parseFloat(ProductDetail[i].GrossPrice);
                NetAmount = parseFloat(NetAmount) + parseFloat(ProductDetail[i].TotalAmount);
                UnitPrice = parseFloat(UnitPrice) + parseFloat(ProductDetail[i].UnitPrice);
                DiscountValue = parseFloat(DiscountValue) + parseFloat(ProductDetail[i].DiscountValue);

                $('#<%=txtAmount.ClientID%>').val(getAmountWithDecimal(GrossAmount));
                $('#<%=txtTotalAmount.ClientID%>').val(getAmountWithDecimal(NetAmount));
                $('#<%=txtNetAmount.ClientID%>').val(getAmountWithDecimal(NetAmount));  
                $('#<%=txtPriceAfterDiscount.ClientID%>').val(getAmountWithDecimal(NetAmount));         
                var StockQTy = GetStockQty(ProductDetail[i].hdnNewProductId);
                var d = GetProductTaxPercentage(ProductDetail[i].hdnNewProductId);
                if (d != null || d != "") {
                    ProductDetail[i].TaxPer = d;
                }
                row = $('#tblProduct tbody tr').length;
                row = row + 1;
                var htmlRow = "<tr>";
                console.log('table bind');
                if ($('#<%= ddlOrderType.ClientID %>').val() == "Q") {
                    if (ProductDetail[i].ChkQuatationDetail == null || ProductDetail[i].ChkQuatationDetail == "") {
                        ProductDetail[i].ChkQuatationDetail = true;
                    }
                    htmlRow = htmlRow + "<td><input type='checkbox' id='ChkQuatation" + i + "' checked=" + ProductDetail[i].ChkQuatationDetail + " name='ChkQuatation' onchange='checkQuatation(" + i + "); return false;'> &nbsp;<i class='fa fa-trash' id='btndelete" + i + "' onClick='DeleteTableRow(" + row + "); return false;' style='font-size:15px'></i></td>";

                }
                else {
                    htmlRow = htmlRow + "<td><input type='checkbox' style='display: none;' id='ChkQuatation" + i + "' name='ChkQuatation' onchange='checkQuatation(" + i + "); return false;'> &nbsp;<i class='fa fa-trash' id='btndelete" + i + "' onClick='DeleteTableRow(" + row + "); return false;' style='font-size:15px'></i></td>";

                }
                htmlRow = htmlRow + "<td>" + row + "</td>";
                htmlRow = htmlRow + "<td>" + ProductDetail[i].ProductId + "</td>";
                htmlRow = htmlRow + "<td><label id='shortProductName'>" + ProductDetail[i].ProductName.substring(0, 8) + '...' + "</label><input id='txtDescription" + row + "' value='" + ProductDetail[i].Description + "' type='hidden'></input><button type='button' id='ModelHover' onClick='ProModelopen(" + row + ");' class='btn btn-primary'><i class='fa fa-search'></i></button></td>";
                //htmlRow = htmlRow + "<td><select name='ddlUnittbl' id='ddlUnittbl'>";
                //htmlRow+= '<option value="">----Select----</option>';
                //htmlRow += '<option value="' + ProductDetail[i].UnitId + '">' + ProductDetail[i].UnitName + '</option>';
                //htmlRow += "</select></td>";
                htmlRow += "<td>" + ProductDetail[i].UnitName + "</td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblQuantity" + i + "' value=" + getAmountWithDecimal(ProductDetail[i].Quantity) + " onchange='calculateDetail(" + i + ",0);return false;' style='color:#4D4C4C;width:50px;' ></input></td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblFreeQuantity' value=" +getAmountWithDecimal( ProductDetail[i].FreeQuantity) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                htmlRow = htmlRow + "<td><label id='tblRemainQuantity" + i + "'>" + getAmountWithDecimal(ProductDetail[i].Quantity )+ "</label></td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblUnitPrice" + i + "' onchange='txtUnitPrice_TextChanged(" + i + ",\"" + ProductDetail[i].hdnNewProductId + "\");return false;' value=" + getAmountWithDecimal(ProductDetail[i].UnitPrice) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                htmlRow = htmlRow + "<td><label id='tblGross" + i + "'>" +getAmountWithDecimal(ProductDetail[i].GrossPrice )+ "</label></td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblDiscount" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].Discount) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblDiscountValue" + i + "' onchange='calculateDetail(" + i + ",1);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].DiscountValue) + " style='color:#4D4C4C;width:50px;' ></input></td>";

                htmlRow = htmlRow + "<td><input type='text' id='tblTaxPercentage" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxPer + " style='color:#4D4C4C;width:50px;' readonly>&nbsp;</input><i class='fa fa-plus' onclick='Show_Modal_GST(" + ProductDetail[i].hdnNewProductId+ "); return false;' style='font-size:24px;color:blue'></i></td>";
                htmlRow = htmlRow + "<td><input type='text' id='tblTaxValue" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxVal + " style='color:#4D4C4C;width:50px;' readonly></input></td>";





                htmlRow = htmlRow + "<td><label id='tblTotalAmount" + i + "'>" + getAmountWithDecimal(ProductDetail[i].TotalAmount) + "</label></td>";
                htmlRow += "<td> <select name='Location_Out" + i + "' onchange='LocationOutChange(" + i + ");return false;' id='Location_Out"+i+"'>";
                htmlRow += '<option value="">----Select----</option>';
                for (j = 0; j < LocationOut.d.length; j++) {
                    htmlRow += '<option value="' + LocationOut.d[j] + '">' + LocationOut.d[j] + '</option>';
                }
                htmlRow += "</select></td>";
                if (StockQTy.d.length === 0) {
                    htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >0.000</a></td>"
                }
                else
                {
                    htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >" + StockQTy.d[0] + "</a></td>"
                }
                htmlRow += "<td><input type='text' id=tblAgentCommission" + i + " onchange='CommisionChange(" + i + "); return false;' value=" + ProductDetail[i].AggetnCommission + "><td/>"
                htmlRow = htmlRow + "</tr>";
                $('#tblProduct > tbody:last-child').append(htmlRow);
                calculateDetail(i, 0);              
            }
            toggleCommissionColumn();
            GetTaxAllowOrnot();
            //GetHeaderTotal();
            $("#prgBar").css("display", "none");
        }
        function CommisionChange(row) {
            
            var Commision = $('#tblAgentCommission' + row + '').val();
            ProductDetail[row].AggetnCommission = Commision;
        }
        function toggleCommissionColumn() {
            var table = document.getElementById("tblProduct");

            // Find the index of the "Commission" column in the second row of the header
            var Agent = $('#<%=txtAgentName.ClientID%>').val();

  var headerRow = table.tHead.rows[0]; // Second row in the header 
            //alert(Agent);
  
  if (Agent == "") {
      // Hide the header cell
      headerRow.cells['11'].style.display = "none";

      // Loop through all the rows in the tbody and hide the corresponding cell
      var tbody = table.tBodies[0];
      var rows = tbody.rows;
      for (var i = 0; i < rows.length; i++) {
          rows[i].cells['17'].style.display = "none";
      }
  }
  else {
      // Show the header cell
      headerRow.cells['11'].style.display = "block";

      // Loop through all the rows in the tbody and hide the corresponding cell
      var tbody = table.tBodies[0];
      var rows = tbody.rows;
      for (var i = 0; i < rows.length; i++) {
          rows[i].cells['17'].style.display = "block";
      }
  }
}

        function checkQuatation(row) {

            var checkbox = document.getElementById("ChkQuatation"+row+"");
            if (checkbox.checked) {
                ProductDetail[row].ChkQuatationDetail =true;
                
            } else {
                ProductDetail[row].ChkQuatationDetail =false;
            }
           
        }

        //This function create by rahul Sharma For Location Change 
        function LocationOutChange(row)
        {            
            var a = ProductDetail[row];            
            ProductDetail[row].loc = $('#Location_Out' + row + ' option:selected').text();
            console.log(ProductDetail);
        }
        
        // this Function for  Reset form
        function BtnReset_Click() {
            $("#prgBar").css("display", "block");
            Li_Tab_New();           
            $('#<%= Lbl_Tab_New.ClientID %>').text("New");
            $('#<%= hdnContactPersonId.ClientID %>').val('');
            var inputString= $('#<%= txtSONo.ClientID %>').val();
            var parts = inputString.split("-");
            $('#<%= txtSONo.ClientID %>').val(parts[0] + "-");
            $('#<%= hdnSalesPersonId.ClientID %>').val('');           
           
            $('#<%= hdnSalesQuotationId.ClientID %>').val('');
           $('#<%= editid.ClientID %>').val('');     
           $('#<%= ddlOrderType.ClientID %> option:selected').val('');
            $('#<%= txtCustOrderNo.ClientID %>').val('');
             $('#<%= txtQuotationNo.ClientID %>').val('');
            $('#<%= txtPriceAfterDiscount.ClientID %>').val('');
             $('#<%= txtTotalAmount.ClientID %>').val('');
             $('#<%= ddlCurrency.ClientID %> option:selected').val('');
           $('#<%= ddlPaymentMode.ClientID %> option:selected').val('');
         $('#<%= txtSalesPerson.ClientID %>').val('');
            $('#<%= txtCustomer.ClientID %>').val(''); 
           
           $('#<%= txtContactPerson.ClientID %>').val('');
     $('#<%= txtAgentName.ClientID %>').val('');
            $('#<%= txtInvoiceTo.ClientID %>').val('');
             $('#<%= txtEstimateDeliveryDate.ClientID %>').val('');
           $('#<%= txtShipCustomerName.ClientID %>').val('');
           $('#<%= txtShipingAddress.ClientID %>').val('');
           ProductDetail=null;
           $('#<%= txtAmount.ClientID %>').val('');
           $('#<%= txtDiscountP.ClientID %>').val('');
            $('#<%= txtDiscountV.ClientID %>').val('');
            $('#<%= txtNetAmount.ClientID %>').val('');
           $('#<%= txtShippingCharge.ClientID %>').val('');           
           $('#<%= txtRemark.ClientID %>').val('');         
            PaymentDetail = null;                       
            $('#<%= ddlDeliveryvoucher.ClientID %> option:selected').val('');

           $('#<%=txtProductcode.ClientID%>').val('');
          $('#<%=hdnNewProductId.ClientID%>').val('');
           $('#<%=txtProductName.ClientID%>').val('');
            $('#<%=ddlUnit.ClientID%>').val('');             
                $('#<%=txtPQuantity.ClientID%>').val('');
               $('#<%=txtPFreeQuantity.ClientID%>').val('');
                $('#<%=txtPUnitPrice.ClientID%>').val('');
                  $('#<%=txtPQuantityPrice.ClientID%>').val('');
                 $('#<%=txtPDiscountPUnit.ClientID%>').val('');
                 $('#<%=txtPDiscountVUnit.ClientID%>').val('');
              $('#<%=innerId.ClientID%>').text('');
                 $('#<%=txtPTotalAmount.ClientID%>').val('');
            $('#tblProduct > tbody > tr').remove();
            $('#tblPayment > tbody > tr').remove();
            $("#prgBar").css("display", "none");
        }
       function Modal_NewAddress_Open() {
           document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
           Product_Tblbind();
           GetInvoiceAddress();
           ShipingAddressDetail();
           AddPaymentDetail();
        }
        //This Function Create By Rahul Sharma For View Full name of Product in Table 
        function ViewFullProduct()
        {
            setTimeout(function () { $("#prgBar").css("display", "block") }, 200);                      
            var ViewFullName = document.getElementById('<%=ViewFullName.ClientID%>');
            if (ViewFullName.checked) {
                var ProductArray = ProductDetail.length;
                var LocationOut = LoctionList();
                // console.log(ProductDetail);
                $('#tblProduct > tbody > tr').remove();
                for (var i = 0; i < ProductArray; i++) {
                    var StockQTy = GetStockQty(ProductDetail[i].hdnNewProductId);
                    row = $('#tblProduct tbody tr').length;
                    row = row + 1;
                    var d = GetProductTaxPercentage(ProductDetail[i].hdnNewProductId);
                    if (d != null || d != ""){
                        ProductDetail[i].TaxPer = d;
                    }
                    var htmlRow = "<tr>";
                    //console.log('table bind');
                    htmlRow = htmlRow + "<td><i class='fa fa-trash' onClick='DeleteTableRow(" + row + ");' style='font-size:15px'></i></td>";
                    htmlRow = htmlRow + "<td>" + row + "</td>";
                    htmlRow = htmlRow + "<td>" + ProductDetail[i].ProductId + "</td>";
                    htmlRow = htmlRow + "<td><label id='shortProductName'>" + ProductDetail[i].ProductName + "</label><input id='txtDescription" + row + "' value='" + ProductDetail[i].Description + "' type='hidden'></input><button type='button' id='ModelHover' onClick='ProModelopen(" + row + ");' class='btn btn-primary'><i class='fa fa-search'></i></button></td>";
                    htmlRow += "<td>" + ProductDetail[i].UnitName + "</td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblQuantity" + i + "' value=" + getAmountWithDecimal(ProductDetail[i].Quantity) + " onchange='calculateDetail(" + i + ",0);return false;' style='color:#4D4C4C;width:50px;' ></input></td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblFreeQuantity' value=" + getAmountWithDecimal(ProductDetail[i].FreeQuantity) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                    htmlRow = htmlRow + "<td><label id='tblRemainQuantity" + i + "'>" + getAmountWithDecimal(ProductDetail[i].Quantity) + "</label></td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblUnitPrice" + i + "' onchange='txtUnitPrice_TextChanged(" + i + ",\"" + ProductDetail[i].hdnNewProductId + "\");return false;' value=" + getAmountWithDecimal(ProductDetail[i].UnitPrice) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                    htmlRow = htmlRow + "<td><label id='tblGross" + i + "'>" + getAmountWithDecimal(ProductDetail[i].GrossPrice) + "</label></td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblDiscount" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].Discount) + " style='color:#4D4C4C;width:50px;' ></input></td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblDiscountValue" + i + "' onchange='calculateDetail(" + i + ",1);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].DiscountValue) + " style='color:#4D4C4C;width:50px;' ></input></td>";

                    htmlRow = htmlRow + "<td><input type='text' id='tblTaxPercentage" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxPer + " style='color:#4D4C4C;width:50px;' readonly>&nbsp;</input><i class='fa fa-plus' onclick='Show_Modal_GST(" + ProductDetail[i].hdnNewProductId + "); return false;' style='font-size:24px;color:blue'></i></td>";
                    htmlRow = htmlRow + "<td><input type='text' id='tblTaxValue" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxVal + " style='color:#4D4C4C;width:50px;' readonly></input></td>";



                    htmlRow = htmlRow + "<td><label id='tblTotalAmount" + i + "'>" + getAmountWithDecimal(ProductDetail[i].TotalAmount) + "</label></td>";
                    htmlRow += "<td> <select name='Location_Out" + i + "' onchange='LocationOutChange(" + i + ");return false;' id='Location_Out" + i + "'>";
                    htmlRow += '<option value="">----Select----</option>';
                    for (j = 0; j < LocationOut.d.length; j++) {
                        htmlRow += '<option value="' + LocationOut.d[j] + '">' + LocationOut.d[j] + '</option>';
                    }
                    htmlRow += "</select></td>";
                    if (StockQTy.d.length === 0) {
                        htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >0.000</a></td>"
                    }
                    else {
                        htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >" + StockQTy.d[0] + "</a></td>"
                    }
                    htmlRow = htmlRow + "</tr>";
                    $('#tblProduct > tbody:last-child').append(htmlRow);
                    calculateDetail(i, 0);
                }
                setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                GetTaxAllowOrnot();
            }
            else {
                Product_Tblbind();
            }
            setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
        }

        function AddPayment()
        {
           // $("#prgBar").css("display", "block");
      
            var PaymentMode = $('#<%=ddlTabPaymentMode.ClientID%> option:selected').text();
            var PaymentModeId = $('#<%=ddlTabPaymentMode.ClientID%> option:selected').val();
            if (PaymentMode === "--Select--") {
                alert('Please Select Payment Mode');
                return;
            }
            else if (PaymentMode == null) {
                alert('Please Select Payment Mode');
                return;
            }
            else if (PaymentMode=="") {
                alert('Please Select Payment Mode');
                return;
            }
            var BalanceAmount = $('#<%=txtPayAmount.ClientID%>').val(); 
            var AccountNo = $('#<%=txtPayAccountNo.ClientID%>').val();
            if (AccountNo == "") {
                alert('Please Select Account No');
                return;
            }
            var ExchangeRate = $('#<%=txtExchangerate.ClientID%>').val(); 
            var LocalAmount = $('#<%=txtLocalAmount.ClientID%>').val(); 
            var Payment_arr = [];
            if (PaymentDetail == null) {
                PaymentDetail = {};
            }
            
            if (Object.keys(PaymentDetail).length > 0) {
                Payment_arr = {
                    'PaymentMode': PaymentMode,
                    'PaymentModeId': PaymentModeId,
                    'BalanceAmount': BalanceAmount,
                    'AccountNo': AccountNo,
                    'ExchangeRate': ExchangeRate,
                    'LocalAmount': LocalAmount,
                    'BankAccountName': '',
                    'BankAccountNo': '',
                    'BankId': '',
                    'CardName': '',
                    'CardNo': '',
                    'ChequeDate': '',
                    'ChequeNo': '',
                    'FCPayAmount': '',
                };
                PaymentDetail[parseInt(Object.keys(PaymentDetail).length)] = Payment_arr;
            }
            else {
                Payment_arr[Object.keys(PaymentDetail).length] = {
                    'PaymentMode': PaymentMode,
                    'PaymentModeId': PaymentModeId,
                    'BalanceAmount': BalanceAmount,
                    'AccountNo': AccountNo,
                    'ExchangeRate': ExchangeRate,
                    'LocalAmount': LocalAmount,
                    'BankAccountName': '',
                    'BankAccountNo': '',
                    'BankId': '',
                    'CardName': '',
                    'CardNo': '',
                    'ChequeDate': '',
                    'ChequeNo': '',
                    'FCPayAmount': '',
                };
                PaymentDetail = Payment_arr;
            }
            AddPaymentDetail();
            $('#<%=ddlTabPaymentMode.ClientID%> option:selected').text('--Select--');
            $('#<%=ddlTabPaymentMode.ClientID%> option:selected').val('--Select--');         
            $('#<%=txtPayAmount.ClientID%>').val(''); 
            $('#<%=txtPayAccountNo.ClientID%>').val('');            
            $('#<%=txtLocalAmount.ClientID%>').val(''); 
        }
        <%--   function AddPayment() {
            $("#prgBar").css("display", "block");
            
            var PaymentMode = $('#<%=ddlTabPaymentMode.ClientID%> option:selected').text();
            var PaymentModeId = $('#<%=ddlTabPaymentMode.ClientID%> option:selected').val();
            var BalanceAmount = $('#<%=txtPayAmount.ClientID%>').val();
            var AccountNo = $('#<%=txtPayAccountNo.ClientID%>').val();
            var ExchangeRate = $('#<%=txtExchangerate.ClientID%>').val();
            var LocalAmount = $('#<%=txtLocalAmount.ClientID%>').val();

            var paymentObj = {
                'PaymentMode': PaymentMode,
                'PaymentModeId': PaymentModeId,
                'BalanceAmount': BalanceAmount,
                'AccountNo': AccountNo,
                'ExchangeRate': ExchangeRate,
                'LocalAmount': LocalAmount,
                'BankAccountName': '',
                'BankAccountNo': '',
                'BankId': '',
                'CardName': '',
                'CardNo': '',
                'ChequeDate': '',
                'ChequeNo': '',
                'FCPayAmount': '',
            };

            if (!PaymentDetail) {
                PaymentDetail = [];
            }
            
            var new_arr = [];
            var paymentIndex = Object.keys(PaymentDetail).length;
            new_arr[paymentIndex] = paymentObj
            PaymentDetail[paymentIndex] = paymentObj;

            AddPaymentDetail();
            $("#prgBar").css("display", "none");
        }--%>
        
        function AddPaymentDetail()
        {
            $("#prgBar").css("display", "block");
            
            if (PaymentDetail!= null && PaymentDetail!="") {
                var ProductArray = Object.keys(PaymentDetail).length;

                $('#tblPayment > tbody > tr').remove();
                for (var k = 0; k < ProductArray; k++) {
                    var htmlRow = "<tr>";
                    htmlRow = htmlRow + "<td><i class='fa fa-trash' onClick='DeletePaymentDetail(" + k + ");' style='font-size:15px'></i></td>";
                    htmlRow = htmlRow + "<td>" + PaymentDetail[k].PaymentMode + "</td>";
                    htmlRow = htmlRow + "<td>" + PaymentDetail[k].AccountNo + "</td>";
                    htmlRow = htmlRow + "<td>" +getAmountWithDecimal(PaymentDetail[k].BalanceAmount) + "</td>";
                    htmlRow = htmlRow + "<td>" + getAmountWithDecimal(PaymentDetail[k].ExchangeRate) + "</td>";
                    htmlRow = htmlRow + "<td>" + getAmountWithDecimal(PaymentDetail[k].LocalAmount )+ "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblPayment > tbody:last-child').append(htmlRow);

                }
                $("#prgBar").css("display", "none");
            }
            else {
                $("#prgBar").css("display", "none");
            }
           
          
        }
        function DeletePaymentDetail(row)
        {
            $("#prgBar").css("display", "block");
            
            var product = Object.keys(PaymentDetail).length;;
            var NewDetail = {};
            NewDetail = PaymentDetail;
            PaymentDetail = {};
            for (P = 0; P < product; P++) {
                if (P != row) {
                    var new_arr = {
                        'PaymentMode': NewDetail[P].PaymentMode,
                        'PaymentModeId': NewDetail[P].PaymentModeId,
                        'BalanceAmount': NewDetail[P].BalanceAmount,
                        'AccountNo': NewDetail[P].AccountNo,
                        'ExchangeRate': NewDetail[P].ExchangeRate,
                        'LocalAmount': NewDetail[P].LocalAmount,
                        'BankAccountName': '',
                        'BankAccountNo': '',
                        'BankId': '',
                        'CardName': '',
                        'CardNo': '',
                        'ChequeDate': '',
                        'ChequeNo': '',
                        'FCPayAmount': '',

                    };
                    PaymentDetail[P] = new_arr;

                }
            }
            AddPaymentDetail();
            $("#prgBar").css("display", "none");
        }


    function ddlPaymentMode_Selected() {
    var ddlPaymentMode_ = document.getElementById('<%= ddlTabPaymentMode.ClientID%>');
    var txtPayAccountNo_ = document.getElementById('<%=txtPayAccountNo.ClientID%>');
    var txtPayAmount_ = document.getElementById('<%=txtPayAmount.ClientID%>');
        var txtLocalAmount_ = document.getElementById('<%=txtLocalAmount.ClientID%>'); 
        var txtPaymentExchangerate_ = document.getElementById('<%=txtExchangerate.ClientID%>');
        var txtNetAmount = document.getElementById('<%=txtNetAmount.ClientID%>');
       

    if (ddlPaymentMode_.selectedOptions[0].innerHTML == "--Select--") {
        txtPayAccountNo_.value = "";
    } else {
        $("#prgBar").css("display", "block");
        PageMethods.ddlPaymentMode_SelectedIndexChanged(ddlPaymentMode_.value, function (data) {
            console.log(data);
            txtPayAccountNo_.value = data;
            txtPayAmount_.value = txtNetAmount.value;
            txtLocalAmount_.value = txtPaymentExchangerate_.value * txtPayAmount_.value;
            //console.log(txtLocalAmount_.value);
            $("#prgBar").css("display", "none");
        }, function (error) {
            console.error("Error:", error);
            // Handle the error using errorMessages or other methods
            // For example, you could show an error message to the user
        });
    }
}


        function handleShippingChargeChange()
        {
            
            var shippingCharge = $('#<%= txtShippingCharge.ClientID %>').val();
            if (shippingCharge == null || shippingCharge == "") {
                shippingCharge = "0.000";
                $('#<%= txtShippingCharge.ClientID %>').val(shippingCharge);
            }
            var NetAmount = $('#<%= txtNetAmount.ClientID %>').val();
            var TotalAmount = parseFloat(NetAmount) + parseFloat(shippingCharge);
            $('#<%= txtNetAmount.ClientID %>').val(getAmountWithDecimal(TotalAmount));
        }

        //This function Create For Save Order Created By Rahul Sharma on Date 19-07-2023
        function btnSaveClick()
        {
            $("#<%=btnSOrderSave.ClientID %>").prop("disabled", true);
            
            setTimeout(function () { $("#prgBar").css("display", "block") }, 800);
         //   $("#prgBar").css("display", "block");
            var objSalesOrder = new Object();
            objSalesOrder.hdnContactPersonId = $('#<%= hdnContactPersonId.ClientID %>').val();
            if (objSalesOrder.hdnContactPersonId == "" || objSalesOrder.hdnContactPersonId == null) {
                alert("Please Add Contact Person");              
                setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                 setTimeout(function () { jQuery('#<%=txtContactPerson.ClientID%>').focus() }, 500);
                $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
                return;
            }
            objSalesOrder.hdnSalesPersonId = $('#<%= hdnSalesPersonId.ClientID %>').val();           
            if (objSalesOrder.hdnSalesPersonId == "" || objSalesOrder.hdnSalesPersonId == null) {
                alert("Please Add Sales Person");
                setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
            setTimeout(function () { jQuery('#<%=txtSalesPerson.ClientID%>').focus() }, 500);
                return;
            }
            objSalesOrder.hdnQutationID = $('#<%= hdnSalesQuotationId.ClientID %>').val();
            objSalesOrder.editId=$('#<%= editid.ClientID %>').val();
            objSalesOrder.OrderDate = $('#<%= txtSODate.ClientID %>').val();
            objSalesOrder.OrderNo = $('#<%= txtSONo.ClientID %>').val();
            objSalesOrder.OrderType = $('#<%= ddlOrderType.ClientID %> option:selected').val();
            objSalesOrder.CustOrderNo = $('#<%= txtCustOrderNo.ClientID %>').val();
            if (objSalesOrder.CustOrderNo == "" || objSalesOrder.CustOrderNo == null) {
                alert("Please Add Customer Order No.");
                setTimeout(function () { $("#prgBar").css("display", "none") }, 200);                
                    setTimeout(function () { jQuery('#<%=txtCustOrderNo.ClientID%>').focus() }, 500);
                 $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
                return;
            }
             objSalesOrder.Currency = $('#<%= ddlCurrency.ClientID %> option:selected').val();
            objSalesOrder.PaymentMode = $('#<%= ddlPaymentMode.ClientID %> option:selected').val();
            objSalesOrder.SalesPerson = $('#<%= txtSalesPerson.ClientID %>').val();
            objSalesOrder.CustomerName = $('#<%= txtCustomer.ClientID %>').val(); 
            if (objSalesOrder.CustomerName == "" || objSalesOrder.CustomerName == null) {
                alert("Please Add Customer Name");
                setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
                   setTimeout(function () { jQuery('#<%=txtCustomer.ClientID%>').focus() }, 500);
                return;
            }
            objSalesOrder.ContactPerson = $('#<%= txtContactPerson.ClientID %>').val();
            objSalesOrder.AgentName = $('#<%= txtAgentName.ClientID %>').val();
            objSalesOrder.InvoiceAddress = $('#<%= txtInvoiceTo.ClientID %>').val();
            objSalesOrder.DeliveryDate = $('#<%= txtEstimateDeliveryDate.ClientID %>').val();
            objSalesOrder.ShipTo = $('#<%= txtShipCustomerName.ClientID %>').val();
            objSalesOrder.ShippingAddress = $('#<%= txtShipingAddress.ClientID %>').val();
            objSalesOrder.ddlTransType = $('#<%= ddlTransType.ClientID %>').val();
            if (objSalesOrder.ddlTransType == "" || objSalesOrder.ddlTransType == null) {
                objSalesOrder.ddlTransType = "0";
            }
            objSalesOrder.ProductDetails = ProductDetail;
            objSalesOrder.GrossAmount = $('#<%= txtAmount.ClientID %>').val();
            objSalesOrder.DiscountPercent = $('#<%= txtDiscountP.ClientID %>').val();
            objSalesOrder.DiscountValue = $('#<%= txtDiscountV.ClientID %>').val();
            objSalesOrder.NetAmount = $('#<%= txtNetAmount.ClientID %>').val();
            objSalesOrder.TaxPercent = $('#<%= txtTaxP.ClientID %>').val();
            objSalesOrder.Taxvalue=$('#<%= txtTaxV.ClientID %>').val();
            objSalesOrder.ShippingCharge = $('#<%= txtShippingCharge.ClientID %>').val();
            if (objSalesOrder.ShippingCharge == "" || objSalesOrder.ShippingCharge == null) {
                objSalesOrder.ShippingCharge = "0.00";
            }
            objSalesOrder.Remark = $('#<%= txtRemark.ClientID %>').val();
            debugger;
            objSalesOrder.PaymentDetails = PaymentDetail;
            if (PaymentDetail === null || Object.keys(PaymentDetail).length === 0) {
             
            }
            else {
                var paymentAmount=0;
                //Generate json for payment detail
                $('#tblPayment tbody tr').each(function () {
                    var row = $(this);
                    if ($(row).find("td:eq(3)").html() != undefined) {
                        paymentAmount = paymentAmount + parseFloat($(row).find("td:eq(3)").html());
                    }
                });

                if (paymentAmount != objSalesOrder.NetAmount) {
                    alert("Order and Payment amount should be same");
                    setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                     $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
                    return;
                }
            }
            
            objSalesOrder.InvoiceCreate = $('#<%= ddlDeliveryvoucher.ClientID %> option:selected').val();
            objSalesOrder.SendProjectManagement=$('#<%=chkSendInProjectManagement.ClientID%>').is(':checked');
            objSalesOrder.PartialShipment = $('#<%=chkPartialShipment.ClientID%>').is(':checked');
            // console.log(objSalesOrder);
            debugger;
            //alert(JSON.stringify(objSalesOrder.PaymentDetails));
           
            //ajax call to set data
            $.ajax({
                url: 'SalesOrderJScript.aspx/SaveOrder',
                method: 'post',
                contentType: "application/json;",
                data: "{clsSalesOrder:" + JSON.stringify(objSalesOrder) + "}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    alert(response[0]);                 
                        
                    GetSalesOrderList();
                    location.reload(true);
                     $("#<%=btnSOrderSave.ClientID %>").prop("disabled", false);
                        //if (confirm("Do you want to print the order")) {
                           // window.open('../Sales/SalesInvoicePrint.aspx?Id=' + response[2] + '', 'window', 'width=1024');
                        //}
                        //fillTblInvoice();
                    
                    setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
                }
            });

            setTimeout(function () { $("#prgBar").css("display", "none") }, 200);
        }

        function lnkStockInfo_Command(productId)
        {           
             var CustomerName = "";
             CustomerName = document.getElementById('<%=txtCustomer.ClientID%>').value;
             window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=SALES&&Contact=' + CustomerName + '');
        }
        function ProModelopen(row)
        {
                     
            var description = $('#txtDescription'+row+'').val();
            //console.log(description);
            $('#pDescription').text(description);
            $('#exampleModal').modal({
                show: 'true'
            });
        }
        /*function DeleteTableRow(row)
        {

            $("#prgBar").css("display", "block");
            
            var product = ProductDetail.length;
            var NewDetail = {};
            NewDetail = ProductDetail;
            ProductDetail = {};
            for (i = 0; i < product; i++) {
                if (i!= row-1)
                {
                   var new_arr = {
                       'ProductId': NewDetail[i].ProductId,
                       'hdnNewProductId': NewDetail[i].hdnNewProductId,
                       'ProductName': NewDetail[i].ProductName,
                       'UnitId': NewDetail[i].UnitId,
                       'UnitName':NewDetail[i].UnitName,
                       'Quantity': NewDetail[i].Quantity,
                       'FreeQuantity': NewDetail[i].FreeQuantity,
                       'UnitPrice': NewDetail[i].UnitPrice,
                       'GrossPrice': NewDetail[i].GrossPrice,
                       'Discount': NewDetail[i].Discount,
                       'DiscountValue': NewDetail[i].DiscountValue,
                       'Description': NewDetail[i].Description,
                       'TotalAmount': NewDetail[i].TotalAmount

                    };
                    ProductDetail[i-1] = new_arr;
                    
                }
            }
            
            Product_Tblbind();

            $("#prgBar").css("display", "none");
        }*/
        function DeleteTableRow(row) {
            

            // Assuming ProductDetail is an array of objects
            if (row >= 1 && row <= ProductDetail.length) {
                ProductDetail.splice(row - 1, 1); // Remove the row at the specified index
            }

            Product_Tblbind();
        }
        function Li_Tab_New() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }
        function Li_Tab_Quotation()
        {
          

         
        }
        function Eventhandler() {
            Product_Tblbind();
            AddPaymentDetail();
        }
        function Li_Tab_Bin() {
        }
        function Li_Tab_New() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
            $('#<%=txtCustOrderNo.ClientID%>').focus();
        }
        function GetStockQty(ProductId) {
            
            var stock = [];
            $.ajax({
                type: "POST",
                url: 'SalesOrderJScript.aspx/GetStockList',              
                async: false,
                contentType: "application/json; charset=utf-8",
                data: "{'ProductId':'" + ProductId + "'}",
                success: function (data) {

                    stock = data;

                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
            return stock;

        }
      function LoctionList() {
            
            var location = [];
            $.ajax({
                url: 'SalesOrderJScript.aspx/GetLocationList',
                method: 'post',
                async: false,               
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //console.log(data.d[0]);
                    //console.log(data.d.length);
                    //console.log(data);
                    location = data;



                }
            });
            console.log(location);
            return location;

      }
//This function Created by Rahul for Currency Convertion
      function getAmountWithDecimal(amount) {
          
        let Amt = 0;
        let decimalCount = 0;
        let convertedAmt = 0;
        let DotValue = "";
        var strDecimalCount = $('#<%=hdnDecimalCount.ClientID%>').val();
          //Amt = parseFloat(amount);
          decimalCount = parseInt(strDecimalCount);
        
          return parseFloat(amount).toFixed(decimalCount);



    }

        function calculatePrice(type)
        {
            const quantity = parseFloat(document.getElementById('<%= txtPQuantity.ClientID %>').value) || 0;
             const unitPrice = parseFloat(document.getElementById('<%= txtPUnitPrice.ClientID %>').value) || 0;
             const discountPercentage = parseFloat(document.getElementById('<%= txtPDiscountPUnit.ClientID %>').value) || 0;
             const discountValue = parseFloat(document.getElementById('<%= txtPDiscountVUnit.ClientID %>').value) || 0;

            const grossPrice = quantity * unitPrice;
            let calculatedDiscountPercentage = 0;

            if (type == 1)
            {
                // Calculate discount percentage based on the discount value
                calculatedDiscountPercentage = (discountValue / grossPrice) * 100;
                document.getElementById('<%= txtPDiscountPUnit.ClientID %>').value = calculatedDiscountPercentage.toFixed(2);
            }
            const calculatedDiscountValue = (grossPrice * discountPercentage) / 100;
            const netPrice = grossPrice - calculatedDiscountValue;           
            document.getElementById('<%= txtPQuantityPrice.ClientID %>').value = grossPrice.toFixed(2);
            document.getElementById('<%= txtPDiscountVUnit.ClientID %>').value = calculatedDiscountValue.toFixed(2);
            document.getElementById('<%= txtPTotalAmount.ClientID %>').value = netPrice.toFixed(2);
   
        }
         function Modal_CustomerInfo_Open() {
             document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
             AddPaymentDetail();
             Product_Tblbind();
             ShipingAddressDetail();
             GetInvoiceAddress();

        }
        function calculateDetail(row,type) {


            var ProductId = document.getElementById('tblQuantity' + row + '').value





            const quantity = parseFloat(document.getElementById('tblQuantity' + row + '').value) || 0;           
            const unitPrice = parseFloat(document.getElementById('tblUnitPrice' + row + '').value) || 0;
            const discountPercentage = parseFloat(document.getElementById('tblDiscount' + row + '').value) || 0;
            const discountValue = parseFloat(document.getElementById('tblDiscountValue' + row + '').value) || 0;
            const TaxPer = parseFloat(document.getElementById('tblTaxPercentage' + row + '').value) || 0;
            const TaxValue = parseFloat(document.getElementById('tblTaxValue' + row + '').value) || 0;
            document.getElementById('tblRemainQuantity' + row).innerText = getAmountWithDecimal(quantity);
            const grossPrice = quantity * unitPrice;
            let calculatedDiscountPercentage = 0;
              var strDecimalCount = $('#<%=hdnDecimalCount.ClientID%>').val();      
          decimalCount = parseInt(strDecimalCount);
            if (type== 1) {
                calculatedDiscountValue = discountValue;
                // Calculate discount percentage based on the discount value
                calculatedDiscountPercentage = (discountValue / grossPrice) * 100;
                document.getElementById('tblDiscount' + row + '').value = calculatedDiscountPercentage.toFixed(decimalCount);
                ProductDetail[row].Discount = getAmountWithDecimal(calculatedDiscountPercentage);
                const netPrice = grossPrice - calculatedDiscountValue;
                document.getElementById('tblGross' + row + '').innerText = grossPrice.toFixed(decimalCount);
                //document.getElementById('tblKwdPrice1' + row + '').innerText = grossPrice.toFixed(2);
               // document.getElementById('tblKwdPrice' + row).innerText = grossPrice.toFixed(2);
                document.getElementById('tblDiscountValue' + row + '').value = calculatedDiscountValue.toFixed(decimalCount);
                document.getElementById('tblTotalAmount' + row).innerText = netPrice.toFixed(decimalCount);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].DiscountValue = getAmountWithDecimal(calculatedDiscountValue);
                ProductDetail[row].UnitPrice = getAmountWithDecimal(unitPrice);
                ProductDetail[row].GrossPrice = getAmountWithDecimal(grossPrice);
                ProductDetail[row].RequiredQuantity = getAmountWithDecimal(quantity);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].Discount = getAmountWithDecimal(calculatedDiscountPercentage);
            }
            else {
                const calculatedDiscountValue = (grossPrice * discountPercentage) / 100;
                const calculatedTaxValue = ((grossPrice - calculatedDiscountValue) * TaxPer) / 100;
                document.getElementById('tblTaxValue' + row + '').value = calculatedTaxValue.toFixed(decimalCount);
                const TotalNetPrice=grossPrice - calculatedDiscountValue
                const netPrice = TotalNetPrice + calculatedTaxValue;
                
                document.getElementById('tblGross' + row + '').innerText = grossPrice.toFixed(decimalCount);
                //document.getElementById('tblKwdPrice1' + row + '').innerText = grossPrice.toFixed(2);
                //document.getElementById('tblKwdPrice' + row).innerText = grossPrice.toFixed(2);
                document.getElementById('tblDiscountValue' + row + '').value = calculatedDiscountValue.toFixed(decimalCount);
                document.getElementById('tblTotalAmount' + row).innerText = netPrice.toFixed(decimalCount);
                ProductDetail[row].TaxVal = calculatedTaxValue.toFixed(decimalCount);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].DiscountValue = getAmountWithDecimal(calculatedDiscountValue);
                ProductDetail[row].UnitPrice = getAmountWithDecimal(unitPrice);
                ProductDetail[row].GrossPrice = getAmountWithDecimal(grossPrice);
                ProductDetail[row].RequiredQuantity = getAmountWithDecimal(quantity);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].Discount = getAmountWithDecimal(discountPercentage);
            }
            GrossCalculate();

        }
          //this comman function is created by Rahul sharma its calculate all total value and discount value and percentage 
        function GrossCalculate()        
        {
            var grossAmount = 0;
            var NetAmount = 0;
            var DiscountValue = 0;
            var DiscountPercentage = 0;
            var TaxValue = 0;
              var strDecimalCount = $('#<%=hdnDecimalCount.ClientID%>').val();      
          decimalCount = parseInt(strDecimalCount);
            var TaxPer = $('#<%= txtTaxP.ClientID %>').val();
            $('#tblProduct tbody tr').each(function () {
                var row = $(this);
                grossAmount += parseFloat($('#tblGross' + row.index()).text()) || 0;
                NetAmount += parseFloat($('#tblTotalAmount' + row.index()).text()) || 0;
                DiscountValue += parseFloat($('#tblDiscountValue' + row.index()).val()) || 0;
            }); 
            $('#<%= txtAmount.ClientID %>').val(grossAmount.toFixed(decimalCount)); 
            var ShippingCharges = $('#<%= txtShippingCharge.ClientID %>').val(); 
            if (ShippingCharges == "" || ShippingCharges == null) {
                ShippingCharges = "0.000";
            }
            $('#<%= txtNetAmount.ClientID %>').val(getAmountWithDecimal(NetAmount + parseFloat(ShippingCharges))); 
            $('#<%= txtPriceAfterDiscount.ClientID %>').val(getAmountWithDecimal(grossAmount - DiscountValue));
              $('#<%= txtTotalAmount.ClientID %>').val(getAmountWithDecimal(NetAmount));
            TaxValue = (grossAmount - DiscountValue) * TaxPer/100;
            DiscountPercentage = ((DiscountValue) / grossAmount) * 100;
            $('#<%=txtTaxV.ClientID%>').val(TaxValue.toFixed(decimalCount));
            $('#<%= txtDiscountP.ClientID %>').val(DiscountPercentage.toFixed(decimalCount));
            $('#<%= txtDiscountV.ClientID %>').val(DiscountValue.toFixed(decimalCount));
        }
        function txtCustomer_TextChanged(ctl) {
             try {
                debugger;
                if (ctl.value == "") {
                    resetCustomerIdForContact();
                    return;
                }
                validateCustomer(ctl);
                document.getElementById('<%=txtContactPerson.ClientID %>').value = "";

                var data = getCustomerAddress(ctl);
                if (data[0] == "false") {
                    alert(data[1]);
                    ctl.value = "";
                    ctl.focus();
                }
                else {
                    if (ctl.id == "<%= txtCustomer.ClientID %>") {
                        $('#<%= txtShipingAddress.ClientID %>').val(data[1]);
                        $('#<%= txtInvoiceTo.ClientID %>').val(data[1]);
                        $('#<%= txtShipCustomerName.ClientID %>').val(ctl.value);
                        GetInvoiceAddress();
                        ShipingAddressDetail();
                        setTimeout(function () { jQuery('#<%=txtContactPerson.ClientID%>').focus() }, 500);
                    }
                    else {
                        $('#<%= txtShipingAddress.ClientID %>').val(data[1]);
                        ShipingAddressDetail();
                        setTimeout(function () { jQuery('#<%=txtContactPerson.ClientID%>').focus() }, 500);
                    }

                }
               
            }
            catch (ex) {
                showAlert(ex, 'orange', 'white');
            }
        }
        function getCustomerId() {
            var ctl = $("#<%= txtCustomer.ClientID %>").val();
            if (ctl == "") {
                return "";
            }
            return ctl.split('/')[1];
        }
          function GetCreditInfo() {
            try {
                var id = getCustomerId();
                if (id == "") {
                    throw "Please enter customer name";
                }
                var currency = $('#<%= ddlCurrency.ClientID %>').val();
                if (currency == "") {
                    throw "Please select currency";
                }

                $.ajax({
                    url: '../WebServices/customer.asmx/getCreditInfo',
                    method: 'post',
                    contentType: "application/json; charset=utf-8",
                    data: "{'strCustomerId':'" + id + "','strCurrencyId':'" + currency + "'}",
                    async: true,
                    success: function (data) {
                        debugger;
                        var data = data.d;
                        if (data == null) {
                            showAlert('Not found any credit information', 'red', 'white');
                        }
                        else {

                            $('#<%= lblCreditLimitValue.ClientID %>').text(data.split(',')[0]);
                            $('#<%= lblCreditDaysValue.ClientID %>').text(data.split(',')[1]);
                            $('#<%= lblCreditParameterValue.ClientID %>').text(data.split(',')[2]);
                            $('#<%= lblCustomercurrentBalance.ClientID %>').text(data.split(',')[3]);
                            $('#creditTermsNConditions').modal('show')
                        }

                    },
                    error: function (ex) {
                        alert(ex);
                    }
                });
            }
            catch (ex) {
                alert(ex);
            }

        }
        function lnkcustomerHistory_OnClick() {
            var id = getCustomerId();
            if (id == "") {
                showAlert('Please enter customer name', 'orange', 'white');
                return;
            }
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + id + '&&Page=SINV', 'window', 'width=1024, ');
        }
function GetGeoLocation() {
    if ($('#<%= txtShipingAddress.ClientID %>').val() == "") {
        alert("Please Add Shipping Address");
    } else {
        $.ajax({
            url: 'SalesOrderJScript.aspx/GetGeoLocation',
            method: 'post',
            contentType: 'application/json',
            data: JSON.stringify({ ShippingAddress: $('#<%= txtShipingAddress.ClientID %>').val() }),
            success: function (data)
            {
                if (data.d) {
                    // Process the data received from the server here
                    var linkToOpen = "https://www.google.com/maps/search/?api=1&query=" + data.d[1] + "," + data.d[0];
                    // Open the link in a new window or tab
                    window.open(linkToOpen, '_blank');
                }
                else
                {
                    // Handle the case when data.d is 0 or false
                }
            },
            error: function (ex) {
                console.error("AJAX error: " + ex.responseText);
            }
        });
    }
}

        function ShipingAddressDetail()
        {

            if ($('#<%= txtShipingAddress.ClientID %>').val() == "")
            {
                $('#<%= ShippingAddress.ClientID %>').text('');
            }
            else
            {
                $.ajax({
                    url: 'SalesOrderJScript.aspx/GetShippingAddressByName',
                    method: 'post',
                    contentType: "application/json; charset=utf-8",
                    data: "{'ShippingAddress':'" + $('#<%= txtShipingAddress.ClientID %>').val() + "' }",
            async: true,
            success: function (data) {
                if (data.d != 0) {
                    $('#<%= ShippingAddress.ClientID %>').text(data.d);
                  }

              },
            error: function (ex) {

            }
        });
             }
        }

        function GetInvoiceAddress() {
         
 if ($('#<%= txtInvoiceTo.ClientID %>').val() == "")
            {
                $('#<%= txtInvoiceTo.ClientID %>').text('');
            }
            else
            {
                $.ajax({
                    url: 'SalesOrderJScript.aspx/GetShippingAddressByName',
                    method: 'post',
                    contentType: "application/json; charset=utf-8",
                    data: "{'ShippingAddress':'" + $('#<%= txtInvoiceTo.ClientID %>').val() + "' }",
            async: true,
            success: function (data) {
                if (data.d != 0) {
                    $('#<%= InvoiceAddress.ClientID %>').text(data.d);
                  }

              },
            error: function (ex) {

            }
        });
             }
        }



    </script>
     <script src="../Script/customer.js"></script>
    <script src="../Script/employee.js"></script>
    <script src="../Script/address.js"></script>
    <script src="../Script/common.js"></script>
</asp:Content>
