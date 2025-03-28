<%@ Page Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true"
    CodeFile="CustomerInquiry.aspx.cs" Inherits="Sales_CustomerInquiry" Title="Untitled Page" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upallpage" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="GvCustomerInquiry" />
            <asp:PostBackTrigger ControlID="btnNew" />
        </Triggers>
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
                                                <img src="../Images/customer_inquiry.png" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Customer Call %>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="DropdownSearch" Height="25px"
                                        Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                    </asp:DropDownList>
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlMenuList" runat="server" CssClass="a">
                                                    <asp:Button ID="btnList" runat="server" Text="<%$ Resources:Attendance,List %>" Width="90px"
                                                        BorderStyle="none" BackColor="Transparent" OnClick="btnList_Click" Style="padding-left: 30px;
                                                        padding-top: 3px; background-image: url(  '../Images/List.png' ); background-repeat: no-repeat;
                                                        height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuNew" runat="server" CssClass="a">
                                                    <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:Attendance,New %>" Width="90px"
                                                        BorderStyle="none" BackColor="Transparent" OnClick="btnNew_Click" Style="padding-left: 30px;
                                                        padding-top: 3px; background-image: url('../Images/New.png' ); background-repeat: no-repeat;
                                                        height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlMenuBin" runat="server" CssClass="a" Width="100%">
                                                    <asp:Button ID="btnBin" runat="server" Text="<%$ Resources:Attendance,Bin %>" Width="90px"
                                                        BorderStyle="none" BackColor="Transparent" OnClick="btnBin_Click" Style="padding-left: 30px;
                                                        padding-top: 3px; background-image: url(  '../Images/Bin.png' ); background-repeat: no-repeat;
                                                        height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                                    <asp:Panel ID="PnlList" runat="server">
                                        <asp:Panel ID="pnlSearchRecords" runat="server" DefaultButton="btnbind">
                                            <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                                <table width="100%" style="padding-left: 20px; height: 38px">
                                                    <tr>
                                                        <td width="90px">
                                                            <asp:Label ID="lblSelectField" runat="server" Text="<%$ Resources:Attendance,Select Field %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                        <td width="180px">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="270px">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Inquiry No. %>" Value="Inquiry_No" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact Name %>" Value="ContactName"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact No %>" Value="Field2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="135px">
                                                            <asp:DropDownList ID="ddlOption" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="120px">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="270px">
                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                                                Width="250px"></asp:TextBox>
                                                        </asp:Panel>                                                            
                                                        </td>
                                                        <td width="50px" align="center">
                                                            <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Height="25px"
                                                                ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="PnlRefresh" runat="server" DefaultButton="btnRefresh">
                                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Height="25px"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshReport_Click" Width="25px"
                                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerInquiry" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomerInquiry_PageIndexChanging"
                                                AllowSorting="True" OnSorting="GvCustomerInquiry_Sorting" >
                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                Visible="false" ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" Width="16px" Visible="false" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Inquiry Id %>" SortExpression="Trans_Id"
                                                        Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Inquiry No. %>" SortExpression="Inquiry_No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("Inquiry_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Inquiry Date %>" SortExpression="InquiryDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("InquiryDate").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact Name %>" SortExpression="ContactName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("ContactName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact No %>" SortExpression="Field2">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvContactNo" runat="server" Text='<%#Eval("Field2") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Reference To %>" SortExpression="EmployeeName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvRefId" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                
                                            </asp:GridView>
                                            <asp:HiddenField ID="hdnValue" runat="server" />
                                        </asp:Panel>
                                    </asp:Panel>
                                    <asp:Panel ID="PnlNewEdit" runat="server" DefaultButton="btnInquirySave">
                                        <table width="100%" style="padding-left: 43px;">
                                            <tr id="TrIn" runat="server" visible="false">
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCINo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Inquiry No. %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtCINo" runat="server" CssClass="textComman" Width="250px" /><a
                                                        style="color: Red;">*</a>
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCIDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Inquiry Date %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtCIDate" runat="server" CssClass="textComman" Width="250px" /><a
                                                        style="color: Red;">*</a>
                                                    <cc1:CalendarExtender ID="Calender" runat="server" TargetControlID="txtCIDate" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:RadioButtonList ID="RdoListCustomer" runat="server" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="RdoListCustomer_SelectedIndexChanged" AutoPostBack="true"
                                                        CssClass="labelComman">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, New Customer%>" Value="0" Selected="True">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Existing Customer%>" Value="1">
                                                        </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                    <asp:TextBox ID="txtNCustomer" runat="server" CssClass="textComman" Width="740px" />
                                                    <asp:TextBox ID="txtECustomer" runat="server" CssClass="textComman" Width="740px"
                                                        Visible="false" BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged"
                                                        AutoPostBack="true" /><a style="color: Red;">*</a>
                                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                        TargetControlID="txtECustomer" UseContextKey="True" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblContact" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Contact Name %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                    <asp:TextBox ID="txtContact" runat="server" CssClass="textComman" Width="740px" />
                                                    <asp:TextBox ID="txtEContact" runat="server" CssClass="textComman" Width="740px"
                                                        Visible="false" BackColor="#eeeeee" OnTextChanged="txtEContact_TextChanged" AutoPostBack="true" />
                                                    <a style="color: Red;">*</a>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtEContact"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblContactNo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Contact No %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtContactNo" runat="server" CssClass="textComman" Width="250px" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtContactNo" ValidChars="1,2,3,4,5,6,7,8,9,0,">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <a style="color: Red;">*</a>
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblEmailId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Email ID %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="textComman" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblRefTo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Reference To %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                    <asp:TextBox ID="txtRefTo" runat="server" CssClass="textComman" Width="740px" BackColor="#eeeeee"
                                                        OnTextChanged="txtRefTo_TextChanged" AutoPostBack="true" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionListRefTo" ServicePath="" TargetControlID="txtRefTo"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCallType" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Call Type %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:DropDownList ID="ddlCallType" runat="server" CssClass="textComman" Width="260px">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Select%>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Sales Inquiry%>" Value="Sales Inquiry"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Service%>" Value="Service"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Job Cards%>" Value="Job Cards"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <a style="color: Red;">*</a>
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="textComman" Enabled="false"
                                                        Width="260px">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Select%>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Hold%>" Value="Hold"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" style="padding-left: 43px;">
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblDesc" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="5">
                                                    <cc1:Editor ID="Editor1" runat="server" Width="91%" Height="400px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" align="center">
                                                    <table>
                                                        <tr>
                                                            <td width="90px">
                                                                <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    CssClass="buttonCommman" OnClick="btnInquirySave_Click" Visible="false" />
                                                            </td>
                                                            <td width="90px">
                                                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="buttonCommman" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            </td>
                                                            <td width="90px">
                                                                <asp:Button ID="btnInquiryCancel" runat="server" CssClass="buttonCommman" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    CausesValidation="False" OnClick="btnInquiryCancel_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="PnlBin" runat="server">
                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                            <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                                <table width="100%" style="padding-left: 20px; height: 38px">
                                                    <tr>
                                                        <td width="90px">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Select Field %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                        <td width="180px">
                                                            <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="170px">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Inquiry No. %>" Value="Inquiry_No" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact Name %>" Value="ContactName"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact No %>" Value="Field2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="135px">
                                                            <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="120px">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="270px">
                                                            <asp:Panel ID="Panel4" runat="server" DefaultButton="btnbindBin">
                                                                <asp:TextBox ID="txtValueBin" runat="server" CssClass="textCommanSearch" Height="14px"
                                                                Width="250px"></asp:TextBox>
                                                        </asp:Panel>                                                            
                                                        </td>
                                                        <td width="50px" align="center">
                                                            <asp:ImageButton ID="btnbindBin" runat="server" CausesValidation="False" Height="25px"
                                                                ImageUrl="~/Images/search.png" OnClick="btnbindBin_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="Panel3" runat="server" DefaultButton="btnRefreshBin">
                                                                <asp:ImageButton ID="btnRefreshBin" runat="server" CausesValidation="False" Height="25px"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshBin_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>">
                                                                </asp:ImageButton>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="30px" align="center">
                                                            <asp:Panel ID="pnlimgBtnRestore" runat="server" DefaultButton="imgBtnRestore">
                                                                <asp:ImageButton ID="imgBtnRestore" Height="25px" Width="25px" CausesValidation="False"
                                                                    Visible="false" runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click"
                                                                    ToolTip="<%$ Resources:Attendance, Active %>" />
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="pnlImgbtnSelectAll" runat="server" DefaultButton="ImgbtnSelectAll">
                                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"
                                                                    ImageUrl="~/Images/selectAll.png" />
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblTotalRecordsBin" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerInquiryBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomerInquiryBin_PageIndexChanging"
                                                AllowSorting="True" OnSorting="GvCustomerInquiryBin_Sorting" >
                                                
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Inquiry Id %>" SortExpression="Trans_Id"
                                                        Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Inquiry No. %>" SortExpression="Inquiry_No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("Inquiry_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Inquiry Date %>" SortExpression="InquiryDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("InquiryDate").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact Name %>" SortExpression="ContactName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("ContactName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact No %>" SortExpression="Field2">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvContactNo" runat="server" Text='<%#Eval("Field2") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Reference To %>" SortExpression="EmployeeName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvRefId" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                
                                            </asp:GridView>
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
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
