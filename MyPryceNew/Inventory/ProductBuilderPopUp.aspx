<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductBuilderPopUp.aspx.cs"
    Inherits="Inventory_ProductBuilderPopUp" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #4e4a4a">

    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upallpage" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="gvOptionCategory" />
               
                <asp:PostBackTrigger ControlID="btnReset" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <%--<asp:PostBackTrigger ControlID="GridProduct" />
--%>
            </Triggers>
            <ContentTemplate>
            
               <div>
        <center>
            <table width="70%" cellpadding="0" cellspacing="0">
                <tr style="background-color: #90BDE9">
                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
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
                                                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Builder %>"
                                                        CssClass="LableHeaderTitle"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                                 <tr style="background-color: #fff">
                                    <td colspan="4" width="100%" height="500px" valign="top">
                                        <asp:Panel ID="pnlNew" runat="server">
                                            <table style="width: 100%; padding-left: 43px">
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="110px">
                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"
                                                            CssClass="labelComman"></asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="280px">
                                                        <asp:TextBox ID="txtModelNo" runat="server" BackColor="#eeeeee" CssClass="textComman"
                                                            AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelNo"
                                                            ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblModelName" runat="server" Text="<%$ Resources:Attendance,Model Name %>"
                                                            CssClass="labelComman"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtModelName" runat="server" CssClass="textComman" AutoPostBack="true"
                                                            BackColor="#eeeeee" OnTextChanged="txtModelName_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelName"
                                                            ServicePath="" TargetControlID="txtModelName" UseContextKey="True" CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblPartNo" runat="server" Text="<%$ Resources:Attendance,Product Part No .%>"
                                                            CssClass="labelComman"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                    <asp:TextBox ID="txtProductPartNo" runat="server" AutoPostBack="True" CssClass="textComman"
                                                                        OnTextChanged="txtProductPartNo_TextChanged"></asp:TextBox>-
                                                                </td>
                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="390px">
                                                                    <asp:TextBox ID="txtOptionPartNo" runat="server" CssClass="textComman" Width="350px"
                                                                        AutoPostBack="True" OnTextChanged="txtOptionPartNo_TextChanged" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                    <asp:LinkButton ID="btnCopyPartNo" runat="server" Text="<%$ Resources:Attendance, Copy %>" OnClick="btnCopyPartNo_Click"
                                                                        ToolTip="Copy Part No" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblDes" runat="server" Text="<%$ Resources:Attendance,Description%>"
                                                            CssClass="labelComman"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="textComman" TextMode="MultiLine"
                                                            Height="100px" Width="700px" Font-Names="Arial;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblPrice" runat="server" Text="<%$ Resources:Attendance,Price%>" CssClass="labelComman"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="textComman"></asp:TextBox>
                                                    </td>
                                                     <td width="90px">
                                                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                        CssClass="buttonCommman" OnClick="btnReset_Click" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                        CssClass="buttonCommman" OnClick="btnCancel_Click" />
                                                                </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td colspan="6">
                                                        <table width="89%" cellpadding="0" cellspacing="0" id="tblgrid" runat="server" visible="false">
                                                            <tr>
                                                                <td valign="top" width="70%">
                                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Invgridheader">
                                                                                <asp:Label ID="lblOptionCategoryName" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Option Category %>"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Panel ID="pnlgvoptionCate" runat="server" Style="max-height: 300px; overflow: auto;">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOptionCategory" runat="server" AutoGenerateColumns="False" 
                                                                            OnDataBound="gvOptionCategory_DataBound" Width="100%" ShowHeader="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Option Category %>" ItemStyle-Width="150px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOptionCategoryName" Font-Bold="true" runat="server" Text='<%# GetOpCateName(Eval("OptionCategoryId").ToString()) %>'></asp:Label>
                                                                                        <br />
                                                                                        <asp:Label ID="lblOptionCategoryId" runat="server" Visible="false" Text='<%# Eval("OptionCategoryId") %>'></asp:Label>
                                                                                        <asp:RadioButtonList ID="rdoOption" runat="server" Width="100%" CssClass="labelComman"
                                                                                            AutoPostBack="True" OnSelectedIndexChanged="rdoOption_SelectedIndexChanged">
                                                                                        </asp:RadioButtonList>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle  />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            
                                                                            
                                                                            
                                                                            
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td valign="top" width="30%">
                                                                   
                                                                   
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
                        <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" />
                    </center>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
