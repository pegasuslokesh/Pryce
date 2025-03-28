<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailInfo.aspx.cs" Inherits="EmailSystem_MailInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #4e4a4a">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvDownload" />
        </Triggers>
        <ContentTemplate>
            <div>
                <center>
                    <table cellpadding="0" cellspacing="0">
                        <tr style="background-color: #90BDE9">
                            <td width="35px">
                                <table>
                                    <tr>
                                        <td>
                                            <img src="../Images/compare.png" width="31" height="30" alt="D" />
                                        </td>
                                        <td>
                                            <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" width="20px">
                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,New %>" CssClass="LableHeaderTitle"></asp:Label>
                            </td>
                            <td align="left" style="padding-left:303px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlReply" runat="server" DefaultButton="btnReply">
                                                <asp:Button ID="btnReply" runat="server" CausesValidation="False" CssClass="buttonCommman"
                                                    Text="Reply" ToolTip="Reply" Width="150px" OnClick="btnReply_Click" />
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="PnlReplyAll" runat="server" DefaultButton="btnReplyAll">
                                                <asp:Button ID="btnReplyAll" runat="server" CausesValidation="False" Text="Reply All"
                                                    ToolTip="Reply" Width="150px" OnClick="btnReplyAll_Click" CssClass="buttonCommman" />
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="PnlForward" runat="server" DefaultButton="btnForward">
                                                <asp:Button ID="btnForward" runat="server" CausesValidation="False"  Text="Forward" ToolTip="Forward"
                                                    Width="150px" OnClick="btnForward_Click" CssClass="buttonCommman" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="background-color: #e7e7e7">
                            <td colspan="3">
                                <table style="margin-left: 10px; margin-right: 10px" width="820px" height="465px"
                                    border="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlHeader" runat="server" Width="100%" BackColor="#e6e6e6" ScrollBars="Auto">
                                                            <asp:HiddenField ID="hdnTransId" runat="server" />
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <asp:Literal ID="litheader" runat="server"></asp:Literal>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                             <hr />
                                                            <asp:DataList ID="gvDownload" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                                                                Width="100%">
                                                                <ItemTemplate>
                                                                   
                                                                    <asp:LinkButton ID="lnlFileName" runat="server" CommandArgument='<%# Eval("TransId") %>'
                                                                        Text='<%# Eval("File_Name") %>' Width="200px" OnCommand="lnlFileName_Command"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </asp:Panel>
                                                        <cc1:Editor ID="Editor1" runat="server" Width="100%" Height="500px" ActiveMode="Preview"
                                                            TopToolbarPreservePlace="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
    </form>
</body>
</html>
