<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true"
    CodeFile="WarningLetter.aspx.cs" Inherits="Attendance_WarningLetter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnList" />
            <asp:PostBackTrigger ControlID="btnNew" />
            <asp:PostBackTrigger ControlID="btnApply" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                <tr bgcolor="#90BDE9">
                    <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                        <table>
                            <tr>
                                <td>
                                    <img src="../Images/hr_leave_request.png" alt="D" />
                                </td>
                                <td>
                                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                </td>
                                <td style="padding-left: 5px">
                                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,HR Leave Request %>"
                                        CssClass="LableHeaderTitle"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
                                            padding-top: 3px; background-image: url(  '../Images/New.png' ); background-repeat: no-repeat;
                                            height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#ccddee" colspan="4" width="100%" valign="top">
                        <asp:Panel ID="dvNew" runat="server" Visible="false">
                            <table style="padding-left: 20px;">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                </td>
                                                <td width="1px">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="6">
                                                    <asp:TextBox ID="txtEmpName" Width="287px" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                    <a style="color: Red">*</a>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Button ID="btnApply" runat="server" Width="50px" Text="<%$ Resources:Attendance,Apply %>"
                                                        Visible="false" CssClass="buttonCommman" OnClick="btnApply_Click" />
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="Button1" runat="server" Width="50px" Text="<%$ Resources:Attendance,Reset %>"
                                                        Visible="true" CssClass="buttonCommman" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
