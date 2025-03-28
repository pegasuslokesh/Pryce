<%@ Page Title="" Language="C#"  AutoEventWireup="true"
    CodeFile="TicketFeedback.aspx.cs" Inherits="ServiceManagement_TicketFeedback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body >
   
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server" >
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upallpage" runat="server">
        <Triggers>
           
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
                                                <img src="../Images/ticket_feedback_page.png" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Ticket Feedback%>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                           
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                                  
                                 
                                        <table width="100%" style="padding-left: 43px;">
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCINo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Ticket No.%>"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtticketno" runat="server" CssClass="textComman" Width="250px"
                                                        BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtticketno_OnTextChanged" Enabled="false" />
                                                    <asp:HiddenField ID="hdnTicketid" runat="server" />
                                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListTicketNo" ServicePath=""
                                                        TargetControlID="txtticketno" UseContextKey="True" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Panel ID="pnlTicket" runat="server">
                                                        <fieldset style="width: 836px;">
                                                            <legend>
                                                                <asp:Label ID="lblDeviceParameter" Font-Names="Times New roman" Font-Size="18px"
                                                                    Font-Bold="true" runat="server" Text="Ticket Detail" CssClass="labelComman"></asp:Label>
                                                            </legend>
                                                            <table>
                                                                <tr>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                        <asp:Label ID="lblTiDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Ticket Date %>"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        :
                                                                    </td>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="334px">
                                                                        <asp:Label ID="lblTickeDate" runat="server" CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="108px">
                                                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        :
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCustomerNameValue" runat="server" CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                        <asp:Label ID="lblCallType" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Task Type %>"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        :
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTaskType" runat="server" CssClass="labelComman">
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                        <asp:Label ID="Label2" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Status%>"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        :
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblStatus" runat="server" CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                        <asp:Label ID="Label8" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Schedule Date %>"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        :
                                                                    </td>
                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                        <asp:Label ID="lblScheduledate" runat="server" CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblDesription" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Description %>"
                                                                            Font-Bold="true" />
                                                                    </td>
                                                                    <td>
                                                                        :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:Label ID="lblDescriptionvalue" runat="server" CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvViewFeedback" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                            AutoGenerateColumns="False" Width="100%" AllowPaging="false" 
                                                                            AllowSorting="false"  >
                                                                            
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvEmpName" runat="server" Text='<%#Eval("Emp_Name")%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"  Width="150px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Feedback Date%>" SortExpression="Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvdate" runat="server" Text='<%#GetDate(Eval("Date").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"  Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>" SortExpression="Action">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvAction" runat="server" Text='<%#Eval("Action")%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            
                                                                            
                                                                            
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                          
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' >
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Action %>" />
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                    <asp:TextBox ID="txtAction" runat="server" Width="713px" TextMode="MultiLine" Height="50px"
                                                        TabIndex="42" CssClass="textComman" Font-Names="Arial" /><a style="color: Red;">*</a>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" style="padding-left: 43px;">
                                            <tr>
                                                <td colspan="6" align="center">
                                                    <table>
                                                        <tr>
                                                            <td width="90px">
                                                                <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    CssClass="buttonCommman" OnClick="btnInquirySave_Click"  />
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

    </form>
</body>
</html>