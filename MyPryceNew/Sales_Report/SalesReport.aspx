<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true"
    CodeFile="SalesReport.aspx.cs" Inherits="Sales_Report_SalesReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upallpage" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                        <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                            <tr bgcolor="#90BDE9">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/product_icon.png" width="31" height="30" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlMenuInquiry" runat="server" CssClass="a">
                                                    <asp:Button ID="btnMenuInquiry" runat="server" Text="<%$ Resources:Attendance,Inquiry%>"
                                                        Width="100px" BorderStyle="none" BackColor="Transparent" Style="padding-left: 25px;
                                                        background-image: url('../Images/New.png' ); background-repeat: no-repeat; padding-top: 3px;
                                                        height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;"
                                                        OnClick="btnMenuInquiry_Click" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuQuotation" runat="server" CssClass="a" Width="100%">
                                                    <asp:Button ID="btnMenuQuotation" runat="server" Text="<%$ Resources:Attendance,Quotation %>"
                                                        Width="100px" BorderStyle="none" BackColor="Transparent" Style="padding-left: 30px;
                                                        background-image: url('../Images/New.png' ); background-repeat: no-repeat; padding-top: 3px;
                                                        height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;"
                                                        OnClick="btnMenuQuotation_Click" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuOrder" runat="server" CssClass="a" Width="100%">
                                                    <asp:Button ID="btnMenuOrder" runat="server" Text="<%$ Resources:Attendance,Order %>"
                                                        Width="100px" BorderStyle="none" BackColor="Transparent" Style="padding-left: 25px;
                                                        background-image: url('../Images/New.png' ); background-repeat: no-repeat; padding-top: 3px;
                                                        height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;"
                                                        OnClick="btnMenuOrder_Click" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuInvoice" runat="server" CssClass="a" Width="100%">
                                                    <asp:Button ID="btnMenuInvoice" runat="server" Text="<%$ Resources:Attendance,Invoice %>"
                                                        Width="100px" BorderStyle="none" BackColor="Transparent" Style="padding-left: 25px;
                                                        background-image: url('../Images/New.png' ); background-repeat: no-repeat; padding-top: 3px;
                                                        height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;"
                                                        OnClick="btnMenuInvoice_Click" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuReturn" runat="server" CssClass="a" Width="100%">
                                                    <asp:Button ID="btnMenuReturn" runat="server" Text="<%$ Resources:Attendance,Return %>"
                                                        Width="100px" BorderStyle="none" BackColor="Transparent" Style="padding-left: 25px;
                                                        background-image: url('../Images/New.png' ); background-repeat: no-repeat; padding-top: 3px;
                                                        height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;"
                                                        OnClick="btnMenuReturn_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                                    <asp:Panel ID="PnlInquiry" runat="server" Visible="false" DefaultButton="btngo">
                                        <table width="100%" style="padding-left: 43px">
                                            <tr>
                                                <td width="100px" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label5" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Report Type %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlReportType" runat="server" AutoPostBack="true" Width="71%"
                                                        OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" CssClass="textComman">
                                                      <asp:ListItem Text="<%$ Resources:Attendance,Header %>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Detail %>" Value="1"></asp:ListItem>
                      
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label6" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Group By %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlGroupBy" runat="server" Width="71%" CssClass="textComman">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="Tr1" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label27" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="250px">
                                                    <asp:TextBox ID="txtFromDate" runat="server" Width="200" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtFromDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDate" runat="server" Width="200px" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label2" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Inquiry No. %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtInquiryNo" BackColor="#eeeeee" runat="server" Width="70%" CssClass="textComman"
                                                        AutoPostBack="True" OnTextChanged="txtInquiryNo_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListInquiryNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInquiryNo"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnRequestId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" Width="70%" />
                                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr id="tr2">
                                                <td id="td1" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblTender" Visible="false" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Tender No %>"></asp:Label>
                                                </td>
                                                <td id="td2" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblTendercolon" Visible="false" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" id="td3" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtTenderNo" runat="server" Visible="false" Width="70%" CssClass="textComman"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trProduct">
                                                <td id="tdProductName" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblProductName" Visible="false" runat="server" CssClass="labelComman"
                                                        Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                </td>
                                                <td id="tdColon" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblcolon" Visible="false" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" id="tdproductlist" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="textComman" AutoPostBack="true"
                                                        Visible="false" BackColor="#eeeeee" Width="70%" OnTextChanged="txtProductName_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="padding-right: 235px;" colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btngo_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btnReset_Click" CausesValidation="False" />
                                                            </td>
                                                            <%--<td>
                                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnResetSreach">
                                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CssClass="buttonCommman"  Height="25px" />
                                                                    </asp:Panel>
                                                                </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="PnlQuotation" runat="server" Visible="false" DefaultButton="btnSaveQuotationReport">
                                        <table width="100%" style="padding-left: 43px">
                                            <tr>
                                                <td width="100px" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Report Type %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlReportTypeQuotation" runat="server" AutoPostBack="true"
                                                        Width="71%" OnSelectedIndexChanged="ddlReportTypeQuotation_SelectedIndexChanged"
                                                        CssClass="textComman">
                                                       <asp:ListItem Text="<%$ Resources:Attendance,Header %>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Detail %>" Value="1"></asp:ListItem>
                      
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label4" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Group By %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlGroupByQuotation" runat="server" Width="71%" CssClass="textComman">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="Tr3" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label7" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="250px">
                                                    <asp:TextBox ID="txtFromDateQuotation" runat="server" Width="200" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtFromDateQuotation">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label8" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDateQuotation" runat="server" Width="200px" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtToDateQuotation">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label15" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Quotation No.%>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtQuotationNo" BackColor="#eeeeee" runat="server" Width="70%" CssClass="textComman"
                                                        AutoPostBack="True" OnTextChanged="txtQuotationNo_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListQuotationNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtQuotationNo"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField4" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label9" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Inquiry No. %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtInquiryNoInquotation" BackColor="#eeeeee" runat="server" Width="70%"
                                                        CssClass="textComman" AutoPostBack="True" OnTextChanged="txtInquiryNoInquotation_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListInquiryNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInquiryNoInquotation"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label10" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtCustomerNamequotation" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtCustomerNamequotation_TextChanged" Width="70%" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer"
                                                        ServicePath="" TargetControlID="txtCustomerNamequotation" UseContextKey="True"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr id="tr5">
                                                <td id="td7" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblProductNameQuotation" Visible="false" runat="server" CssClass="labelComman"
                                                        Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                </td>
                                                <td id="td8" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblQuotationColon" Visible="false" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" id="td9" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtProductNameQuotation" runat="server" CssClass="textComman" AutoPostBack="true"
                                                        Visible="false" BackColor="#eeeeee" Width="70%" OnTextChanged="txtProductNameQuotation_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductNameQuotation"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField3" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="padding-right: 235px;" colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSaveQuotationReport" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btnSaveQuotationReport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnResetQuotationReport" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btnResetQuotationReport_Click"
                                                                    CausesValidation="False" />
                                                            </td>
                                                            <%--<td>
                                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnResetSreach">
                                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CssClass="buttonCommman"  Height="25px" />
                                                                    </asp:Panel>
                                                                </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="PnlOrder" runat="server" Visible="false" DefaultButton="btnSaveOrderReport">
                                        <table width="100%" style="padding-left: 43px">
                                            <tr>
                                                <td width="100px" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label11" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Report Type %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlReportTypeOrder" runat="server" AutoPostBack="true" Width="71%"
                                                        OnSelectedIndexChanged="ddlReportTypeOrder_SelectedIndexChanged" CssClass="textComman">
                                                       <asp:ListItem Text="<%$ Resources:Attendance,Header %>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Detail %>" Value="1"></asp:ListItem>
                      
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label12" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Group By %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlGroupByOrder" runat="server" Width="71%" CssClass="textComman">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="Tr4" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label13" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="250px">
                                                    <asp:TextBox ID="txtFromDateOrder" runat="server" Width="200" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtFromDateOrder">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label14" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDateOrder" runat="server" Width="200px" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="txtToDateOrder">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 10px;" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label21" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order Type %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textComman" Width="71%">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Select Order Type%>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Direct%>" Value="D"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,By Sales Quotation%>" Value="Q"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 10px;" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label22" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order No.%>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtOrderNo" BackColor="#eeeeee" runat="server" Width="70%" CssClass="textComman"
                                                        AutoPostBack="True" OnTextChanged="txtOrderNo_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListOrderNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOrderNo" UseContextKey="True"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField9" runat="server" Value="0" />
                                                </td>
                                            </tr>
                            <tr>
                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:Label ID="Label16" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Quotation No.%>"></asp:Label>
                                </td>
                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    :
                                </td>
                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:TextBox ID="txtQuotationNoInOrder" BackColor="#eeeeee" runat="server" Width="70%" CssClass="textComman"
                                        AutoPostBack="True" OnTextChanged="txtQuotationNoInOrder_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListQuotationNo" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtQuotationNoInOrder"
                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HiddenField5" runat="server" Value="0" />
                                </td>
                            </tr>
                           
                            <tr>
                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:Label ID="Label18" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                </td>
                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    :
                                </td>
                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:TextBox ID="txtCustomerNameInOrder" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                        AutoPostBack="True" OnTextChanged="txtCustomerNameInOrder_TextChanged" Width="70%" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" CompletionInterval="100"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer"
                                        ServicePath="" TargetControlID="txtCustomerNameInOrder" UseContextKey="True"
                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HiddenField7" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr id="tr6">
                                <td id="td4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:Label ID="lblProductNameOrder" Visible="false" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                </td>
                                <td id="td5" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:Label ID="lblProductNameColon" Visible="false" runat="server" Text=":"></asp:Label>
                                </td>
                                <td colspan="4" id="td6" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:TextBox ID="txtProductNameOrder" runat="server" CssClass="textComman" AutoPostBack="true"
                                        Visible="false" BackColor="#eeeeee" Width="70%" OnTextChanged="txtProductNameOrder_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductNameOrder"
                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HiddenField8" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 235px;" colspan="6">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSaveOrderReport" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                    CssClass="buttonCommman" Height="25px" OnClick="btnSaveOrderReport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnResetOrderReport" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                    CssClass="buttonCommman" Height="25px" OnClick="btnResetOrderReport_Click"
                                                    CausesValidation="False" />
                                            </td>
                                            <%--<td>
                                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnResetSreach">
                                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CssClass="buttonCommman"  Height="25px" />
                                                                    </asp:Panel>
                                                                </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                         
                        <asp:Panel ID="PnlInvoice" runat="server" Visible="false">
                         <table width="100%" style="padding-left: 43px">
                                            <tr>
                                                <td width="100px" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label31" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Report Type %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlInvoiceReportType" runat="server" AutoPostBack="true" Width="72%"
                                                        CssClass="textComman" OnSelectedIndexChanged="ddlInvoiceReportType_SelectedIndexChanged">
                                                      <asp:ListItem Text="<%$ Resources:Attendance,Header %>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Detail %>" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label32" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Group By %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlGroupByInvoice" runat="server" Width="72%" CssClass="textComman">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="Tr8" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label33" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="250px">
                                                    <asp:TextBox ID="txtFromDateInvoice" runat="server" Width="200" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender8" runat="server" Enabled="True" TargetControlID="txtFromDateInvoice">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label34" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDateInvoice" runat="server" Width="200px" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender9" runat="server" Enabled="True" TargetControlID="txtToDateInvoice">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="120px">
                                                    <asp:Label ID="Label35" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Invoice No%>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtInvoiceNo_TextChanged" Width="71%" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server"
                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionList_InvoiceNo" ServicePath="" TargetControlID="txtInvoiceNo"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                           <asp:HiddenField ID="hdnInvoiceNo" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                              <tr>
                                                             <td  align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    <asp:Label ID="Label42" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                                </td>
                                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    :
                                                                </td >
                                                                <td colspan="4"   align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                <asp:TextBox ID="txtCustomerNameInvoice" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                        AutoPostBack="True" OnTextChanged="txtCustomerNameInvoice_TextChanged" Width="71%" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" CompletionInterval="100"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer"
                                        ServicePath="" TargetControlID="txtCustomerNameInvoice" UseContextKey="True"
                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                 
                                                               </td>
                                                               </tr>
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblsalesperson" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Sales Person %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblsalespersoncolon" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                   <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" Width="71%" />
                                                    <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                        TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnsalespersonId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                           
                                            <tr id="tr9">
                                                <td id="td10" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblProductNameInvoice" Visible="false" runat="server" CssClass="labelComman"
                                                        Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                </td>
                                                <td id="td11" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="lblColonInvoice" Visible="false" runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" id="td12" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtProductNameInvoice" runat="server" CssClass="textComman" AutoPostBack="true"
                                                        Visible="false" BackColor="#eeeeee" Width="71%" OnTextChanged="txtProductNameInvoice_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductNameInvoice"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="padding-right: 235px;" colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="BtnInvoiceReport" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="BtnInvoiceReport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="BtnResetInvoiceReport" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="BtnResetInvoiceReport_Click"
                                                                    CausesValidation="False" />
                                                            </td>
                                                            <%--<td>
                                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnResetSreach">
                                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CssClass="buttonCommman"  Height="25px" />
                                                                    </asp:Panel>
                                                                </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                        </asp:Panel>
                        <asp:Panel ID="PnlReturn" runat="server" Visible="false">
                        <table width="100%" style="padding-left: 43px">
                                          
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label40" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Group By %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:DropDownList ID="ddlGroupByReturn" runat="server" Width="71%" CssClass="textComman">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="Tr12" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label46" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="250px">
                                                    <asp:TextBox ID="txtFromDateReturn" runat="server" Width="200" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtenderReturnfrom" runat="server" Enabled="True" TargetControlID="txtFromDateReturn">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label47" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDateReturn" runat="server" Width="200px" CssClass="textComman"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtenderReturnto" runat="server" Enabled="True" TargetControlID="txtToDateReturn">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <hr />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="120px">
                                                    <asp:Label ID="Label48" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Invoice No%>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtInvoiceNoReturn" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtInvoiceNoReturn_TextChanged" Width="71%" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server"
                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionList_InvoiceNo" ServicePath="" TargetControlID="txtInvoiceNoReturn"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                           
                                                </td>
                                            </tr>
                                             <tr>
                                                <td  align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" width="120px">
                                                    <asp:Label ID="Label17" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order No.%>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td colspan="4" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtOrderNoReturn" BackColor="#eeeeee" runat="server" Width="71%" CssClass="textComman"
                                                        AutoPostBack="True" OnTextChanged="txtOrderNoReturn_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListOrderNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOrderNoReturn" UseContextKey="True"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="HiddenField6" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                              
                                         
                                            <tr id="tr13">
                                                <td id="td16" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label55"  runat="server" CssClass="labelComman"
                                                        Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                </td>
                                                <td id="td17" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:Label ID="Label56"  runat="server" Text=":"></asp:Label>
                                                </td>
                                                <td colspan="4" id="td18" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtProductnameReturn" runat="server" CssClass="textComman" AutoPostBack="true"
                                                        BackColor="#eeeeee" Width="71%" OnTextChanged="txtProductnameReturn_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender22" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductnameReturn"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="padding-right: 235px;" colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnReturnReport" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btnReturnReport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnResetReturnReport" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="buttonCommman" Height="25px" OnClick="btnResetReturnReport_Click"
                                                                    CausesValidation="False" />
                                                            </td>
                                                            <%--<td>
                                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnResetSreach">
                                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CssClass="buttonCommman"  Height="25px" />
                                                                    </asp:Panel>
                                                                </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                        </asp:Panel>
                    </td>
                </tr>
                </table> </td> </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" alt="Pegasus Technologies" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
