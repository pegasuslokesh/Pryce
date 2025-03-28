<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMail.aspx.cs" Inherits="EmailSystem_SendMail" %>

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
            <asp:PostBackTrigger ControlID="ImgBtnAdd" />
         
        </Triggers>
        <ContentTemplate>
            <div>
                <center>
                    <table cellpadding="0" cellspacing="0">
                        <tr style="background-color: #90BDE9">
                            <td width="50px">
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
                            <td align="left" width="100px">
                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,New %>" CssClass="LableHeaderTitle"></asp:Label>
                            </td>
                            <td  style="padding-left:439px;" >


                            <table width="100%">
                            <tr>
                            
                            <td>
                            
                            <asp:Panel ID="pnlsendemail" runat="server" DefaultButton="btnSendMail" >
                            <asp:Button ID="btnSendMail"  runat="server" ValidationGroup="onSend"
                                    ToolTip="Send Email & Save" Text="<%$ Resources:Attendance,Send %>" CssClass="buttonCommman"
                                    OnClick="btnSendMail_Click" />
                            </asp:Panel>
                            </td>
                            <td>
                             <asp:Panel ID="pnlCancelMail" runat="server" DefaultButton="btnCancel">
                               <asp:Button ID="btnCancel" runat="server" ToolTip="<%$ Resources:Attendance,Cancel %>"
                                    Text="<%$ Resources:Attendance,Cancel %>" CssClass="buttonCommman" OnClick="btnCancel_Click" />
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
                                        <td width="63px" align="left">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,From %>" CssClass="labelComman"></asp:Label>
                                        </td>
                                        <td width="1px">
                                            :
                                        </td>
                                        <td width="733px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEmailUser" runat="server" CssClass="DropdownSearch" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlEmailUser_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="63px" align="left">
                                            <asp:Label ID="lblTo" runat="server" Text="<%$ Resources:Attendance,To %>" CssClass="labelComman"></asp:Label>
                                        </td>
                                        <td width="1px">
                                            :
                                        </td>
                                        <td width="733px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtMailTo" Width="710px" runat="server" CssClass="textComman"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtMailTo_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                                            ServicePath="" TargetControlID="txtMailTo" UseContextKey="True" CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgToContact" runat="server" ImageUrl="~/Images/add.png" OnClick="ImgToContact_Click"
                                                            ToolTip="<%$ Resources:Attendance,Add %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCc" runat="server" Text="<%$ Resources:Attendance,Cc %>" CssClass="labelComman"></asp:Label>
                                        </td>
                                        <td width="1px">
                                            :
                                        </td>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCC" CssClass="textComman" Width="710px" runat="server"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtCC_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtCC" UseContextKey="True"
                                                            CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                            CompletionListHighlightedItemCssClass="itemHighlighted" ShowOnlyCurrentWordInCompletionListItem="true">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgCcContant" runat="server" ImageUrl="~/Images/add.png" OnClick="ImgCcContant_Click"
                                                            ToolTip="<%$ Resources:Attendance,Add %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblBcc" runat="server" Text="<%$ Resources:Attendance,Bcc %>" CssClass="labelComman"></asp:Label>
                                        </td>
                                        <td width="1px">
                                            :
                                        </td>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtBcc" CssClass="textComman" Width="710px" runat="server"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtBcc_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                                            ServicePath="" TargetControlID="txtBcc" UseContextKey="True" CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgBccContant" runat="server" ImageUrl="~/Images/add.png" OnClick="ImgBccContant_Click"
                                                            ToolTip="<%$ Resources:Attendance,Add %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblSubject" runat="server" Text="<%$ Resources:Attendance,Subject %>"
                                                CssClass="labelComman"></asp:Label>
                                        </td>
                                        <td width="1px">
                                            :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtSubject" CssClass="textComman" Width="710px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="36">
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gvmultiple" runat="server" AutoGenerateColumns="false" Width="710px"
                                                ShowHeader="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="95%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgBtnRemove" runat="server" ImageUrl="~/Images/Att-Remove.png"
                                                                OnCommand="ImgBtnRemove_Command" CommandArgument='<%# Eval("TransId") %>' ToolTip="<%$ Resources:Attendance,Remove %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <%--<asp:FileUpload ID="FileUpload1" Width="720px" runat="server" CssClass="textComman" />--%>
                                                        <div class="input-group">
                                                            <cc1:AsyncFileUpload ID="FileUpload1"
                                                                OnClientUploadStarted="FUAll_UploadStarted"
                                                                OnClientUploadError="FUAll_UploadError"
                                                                OnClientUploadComplete="FUAll_UploadComplete"
                                                                OnUploadedComplete="FUAll_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de;">
                                                                <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td valign="bottom">
                                                        <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="~/Images/add.png" OnClick="ImgBtnAdd_Click"
                                                            ToolTip="<%$ Resources:Attendance,Add %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" colspan="3">
                                            <cc1:Editor ID="txtMessage" runat="server" Width="810px" Height="350px" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <asp:Panel ID="pnl1" runat="server" class="MsgOverlayAddressEmail" Visible="False">
                <asp:Panel ID="pnl2" runat="server" class="MsgPopUpPanelAddressEmail" Visible="False">
                    <asp:Panel ID="pnl3" runat="server" Style="width: 100%; height: 100%; text-align: center;">
                        <table width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblContactpopup" runat="server" Font-Size="14px" Font-Bold="true"
                                        CssClass="labelComman" Text="<%$ Resources:Attendance,List %>"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/Images/close.png" CausesValidation="False"
                                        OnClick="btnClose_Click" Height="20px" Width="20px" />
                                </td>
                            </tr>
                        </table>
                        <table width="800px" style="padding-left: 13px; height: 200px;">
                            <tr>
                                <td>
                                    <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnbind">
                                                    <table width="100%" style="padding-left: 10px; height: 38px">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblSearch" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                                                                    Text="<%$ Resources:Attendance,Search %>"></asp:Label>
                                                            </td>
                                                            <td width="50px" align="left">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                    Width="100px"  OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                                     <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="Name"></asp:ListItem>
                                                                     <asp:ListItem Text="<%$ Resources:Attendance,By Company%>" Value="CompanyName" Selected="True"></asp:ListItem>
                                                                     <asp:ListItem Text="<%$ Resources:Attendance,By Email ID%>" Value="Email_Id"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                                                    Width="95%" OnTextChanged="txtValue_TextChanged"
                                                                    AutoPostBack="true" BackColor="#eeeeee"/>
                                                                     <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                    MinimumPrefixLength="0" ServiceMethod="GetCompletionListName" ServicePath=""
                                                                    TargetControlID="txtValue" UseContextKey="True" CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtValueGroup" runat="server" CssClass="textComman" Width="82%"
                                                                    Visible="false" BackColor="#eeeeee" OnTextChanged="txtValueGroup_TextChanged"
                                                                    AutoPostBack="true" />
                                                                <cc1:AutoCompleteExtender ID="txtValueGroup_AutoCompleteExtender" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                    MinimumPrefixLength="0" ServiceMethod="GetCompletionListGroup" ServicePath=""
                                                                    TargetControlID="txtValueGroup" UseContextKey="True" CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtValueCompany" runat="server" CssClass="textComman" Width="82%"
                                                                    Visible="false" BackColor="#eeeeee" OnTextChanged="txtValueCompany_TextChanged"
                                                                    AutoPostBack="true" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender_txtValueCompany" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                    MinimumPrefixLength="0" ServiceMethod="GetCompletionListCompany" ServicePath=""
                                                                    TargetControlID="txtValueCompany" UseContextKey="True" CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>
                                                                 <asp:TextBox ID="txtValueEmail" runat="server" CssClass="textComman" Width="82%"
                                                                    Visible="false"   />
                                                              </asp:Panel>
                                                            </td>
                                                            <td align="left" width="100px">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Height="25px"
                                                                                ImageUrl="~/Images/search.png" Width="25px" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Panel ID="pnlRefresh" runat="server" DefaultButton="btnRefresh">
                                                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Height="25px"
                                                                                    ImageUrl="~/Images/refresh.png" Width="25px" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>">
                                                                                </asp:ImageButton>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="left" width="115px">
                                                                <asp:Label ID="lblTotalRecord" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                                    CssClass="labelComman"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlContact" runat="server" Height="350px" ScrollBars="Vertical">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvContact" runat="server" AutoGenerateColumns="False" Width="100%"
                                             ShowHeader="true">
                                            
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                            AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkselect_CheckedChanged"
                                                            Text='<%# Eval("Field1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:Attendance,Contact %>'>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvConatctName" runat="server" Text='<%# Eval("ContactName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            
                                            
                                            
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
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
        <script type="text/javascript">        
        function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }
        function FUAll_UploadStarted(sender, args) {
            
        }
</script>
    </form>
</body>
</html>
