<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddContact.ascx.cs" Inherits="WebUserControl_AddContact" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
  <%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>



  <table width="100%">
                <tr>
                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                        <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0" style="border-bottom:1;">
                            <tr bgcolor="#90BDE9">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/contact_master.png" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Contact Setup %>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                   
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                                    
                                    <asp:Panel ID="PnlNew" runat="server">
                                     <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                        <asp:HiddenField ID="hdnContactId" runat="server" />
                                        <asp:HiddenField ID="hdnCompId" runat="server" />
                                        <asp:Panel ID="Panel6" runat="server">
                                            <table width="100%" style="padding-left: 43px">
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Panel ID="PnlNewEdit" runat="server">
                                                            <asp:Panel ID="pnlbasic" runat="server">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="RdolistSelect" runat="server" CssClass="labelComman" RepeatDirection="Horizontal"
                                                                                CellSpacing="0" CellPadding="0" AutoPostBack="true" OnSelectedIndexChanged="RdolistSelect_SelectedIndexChanged">
                                                                                <asp:ListItem Selected="True" Text="Individual" Value="Individual">
                                                                                </asp:ListItem>
                                                                                <asp:ListItem Text="Company" Value="Company">
                                                                                </asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                            <asp:Label ID="lblId" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance,Id %>"></asp:Label>
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtId" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtId_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListContactId" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtId" UseContextKey="True"
                                                                                CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                                CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblName" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance, Name %>"></asp:Label>
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td colspan="4" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:DropDownList ID="ddlNamePrefix" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                                Width="60px">
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Mr. %>" Selected="True" Value="Mr."></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Miss %>" Value="Miss"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Mrs. %>" Value="Mrs."></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Dr. %>" Value="Dr."></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtName" runat="server" CssClass="textComman"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblNameL" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance, Name (Local) %>"></asp:Label>
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td colspan="4" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtNameL" runat="server" Width="90%" CssClass="textComman"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblAddressName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Address Name %>" />
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                                            <asp:Panel ID="pnlAddress" runat="server" DefaultButton="imgAddAddressName">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtAddressName" runat="server" CssClass="textComman" AutoPostBack="true"
                                                                                                OnTextChanged="txtAddressName_TextChanged" BackColor="#eeeeee" />
                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                                Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAddressName"
                                                                                                UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                                                CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                                            </cc1:AutoCompleteExtender>
                                                                                        </td>
                                                                                        <td valign="bottom">
                                                                                            <asp:Button ID="imgAddAddressName" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                                BackColor="Transparent" BorderStyle="None" Font-Bold="true" CssClass="btnAddAdress"
                                                                                                Font-Size="13px" Font-Names=" Arial" Width="92px" Height="32px" OnClick="imgAddAddressName_Click" />
                                                                                        </td>
                                                                                        <td valign="bottom">
                                                                                            <asp:Button ID="btnAddNewAddress" runat="server" Text="<%$ Resources:Attendance,New %>"
                                                                                                BackColor="Transparent" BorderStyle="None"  Font-Bold="true" CssClass="btnNewAdress"
                                                                                                Font-Size="13px" Font-Names=" Arial" Width="92px" Height="32px" OnClick="btnAddNewAddress_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="6" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Panel ID="panelgrid" runat="server" Width="980px" ScrollBars="Auto">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressName" runat="server"  AutoGenerateColumns="False">
                                                                                                
                                                                                                <Columns>
                                                                                                    <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="imgBtnAddressEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                                ImageUrl="~/Images/edit.png" Visible="false" Width="16px" OnCommand="btnAddressEdit_Command" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                                Height="16px" ImageUrl="~/Images/Erase.png" Visible="false" Width="16px" OnCommand="btnAddressDelete_Command" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSNo" Width="30px" runat="server" CssClass="labelComman" Text='<%#Eval("Trans_Id") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address Name %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvAddressName" Width="100px" runat="server" CssClass="labelComman"
                                                                                                                Text='<%#Eval("Address_Name") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="10%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvAddress" runat="server" CssClass="labelComman" Text='<%#Eval("Address") %>' />
                                                                                                            <asp:Label ID="lblgvLongitude" runat="server" CssClass="labelComman" Text='<%#Eval("Longitude") %>'
                                                                                                                Visible="false" />
                                                                                                            <asp:Label ID="lblgvLatitude" runat="server" CssClass="labelComman" Text='<%#Eval("Latitude") %>'
                                                                                                                Visible="false" /><br />
                                                                                                            <asp:LinkButton ID="lnkgvAddressNameGoToMap" runat="server" Text="Go to Map" OnClick="lnkGetLatLong_Click"
                                                                                                                CommandArgument='<%#Eval("Address") + ";" + Eval("Longitude") + ";" + Eval("Latitude")%>' ForeColor="Blue" Font-Underline="true"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="15%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <%-- <asp:BoundField ControlStyle-Width="200px" HeaderText="<%$ Resources:Attendance, Address %>" DataField="Address" />--%>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailId %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvEmailId" runat="server" CssClass="labelComman" Text='<%#GetContactEmailId(Eval("Address_Name").ToString()) %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="10%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,FaxNo. %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvFaxNo" Width="80px" runat="server" CssClass="labelComman" Text='<%#GetContactFaxNo(Eval("Address_Name").ToString()) %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo.%>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvPhoneNo" runat="server" CssClass="labelComman" Text='<%#GetContactPhoneNo(Eval("Address_Name").ToString()) %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo.%>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvMobileNo" runat="server" CssClass="labelComman" Text='<%#GetContactMobileNo(Eval("Address_Name").ToString()) %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="10%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkdefault" runat="server" Checked='<%#Eval("Is_Default") %>' AutoPostBack="true"
                                                                                                                OnCheckedChanged="chkgvSelect_CheckedChangedDefault" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                
                                                                                                
                                                                                                
                                                                                            </asp:GridView>
                                                                                            <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                                                            <asp:HiddenField ID="hdnAddressName" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                
                                                                <table>
                                                                    <tr>
                                                                        <td width="129px" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblPermanentEMailId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Permanent EmailId%>" />
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td width="345px" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="2">
                                                                            <asp:TextBox ID="txtPermanentMailId" runat="server" CssClass="textComman" BackColor="#eeeeee"/>
                                                                             <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListEmailMaster" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPermanentMailId" UseContextKey="True"
                                                                                CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                                CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                            </cc1:AutoCompleteExtender>
                                                                       
                                                                       
                                                                       <asp:ImageButton ID="btnAddEmail" runat="server" CausesValidation="False" Height="29px"
                                                            ImageUrl="~/Images/add.png" OnClick="btnAddEmail_Click" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" ImageAlign="Middle"/>
                                                           &nbsp;&nbsp;<asp:ImageButton ID="btnRemoveEmail" runat="server" CausesValidation="False" ImageUrl="~/Images/Erase.png" OnClick="btnRemoveEmail_Click" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                Width="16px" Visible="false" ImageAlign="Middle"/> </td>
                                                                                        
                                                                                       
                                                                                        </tr>
                                                                                        <tr>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                        <td>
                                                                                        <asp:RadioButtonList ID="rbnEmailList" runat="server" CssClass="labelComman"></asp:RadioButtonList>
                                                                                        </td></tr>
                                                                                        </table>
                                                                                      
                                                                                      
                                                                
                                                                <table width="100%" id="TbRefContact" runat="server">
                                                                  <tr>
                                                                        <td width="130px" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblPermanentMobileNo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="326px">
                                                                         <asp:DropDownList ID="ddlCountryCode" runat="server" CssClass="textComman" Width="100px"  ></asp:DropDownList>
                                                                            <asp:TextBox ID="txtPermanentMobileNo" runat="server" CssClass="textComman" Width="150px" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtPermanentMobileNo" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                           
                                                                           
                                                                        </td>
                                                                          <td width="130px" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="Label8" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Country Name%>" />
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtCountryName" runat="server" CssClass="textComman" OnTextChanged="txtCountryName_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                                           <cc1:AutoCompleteExtender ID="AutoCompleteExtendercountry" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCountryName"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdncountryid" runat="server" />
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                            <asp:Label ID="lblCompanyName" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance,Company Name %>"></asp:Label>
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                                            <asp:TextBox ID="txtCompany" Width="721px" BackColor="#eeeeee" runat="server" CssClass="textComman"
                                                                                AutoPostBack="true" OnTextChanged="txtCompany_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListComapnyName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCompany" UseContextKey="True"
                                                                                CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                                CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                            <asp:Label ID="lblDepartment" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="260px" CssClass="textComman" />
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Designation %>" />
                                                                        </td>
                                                                        <td width="1px">
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                       
                                                                         <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="textComman" Width="260px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblReligion" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Religion %>" />
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:DropDownList ID="ddlReligion" runat="server" CssClass="textComman" Width="260px" />
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="lblCivilId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Civil Id %>" />
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtCivilId" runat="server" CssClass="textComman" />
                                                                        </td>
                                                                    </tr>
                                                                     <tr id="tr_tincst" runat="server" visible="false">
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="TIN No." />
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtTinno" runat="server" CssClass="textComman"  />
                                                                        </td>
                                                                          <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:Label ID="Label4" runat="server" CssClass="labelComman" Text="CST No." />
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:TextBox ID="txtCstNo" runat="server" CssClass="textComman"  />
                                                                        </td>
                                                                      
                                                                    </tr>
                                                                </table>
                                                                <asp:HiddenField ID="HiddenField6" runat="server" />
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                        </td>
                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                            <asp:CheckBox ID="chkIsEmail" CssClass="labelComman" runat="server" Text="<%$ Resources:Attendance,Send Email %>" />
                                                                            <asp:CheckBox ID="chkIsSMS" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Send SMS %>" />
                                                                              <asp:CheckBox ID="chkVerify" CssClass="labelComman" runat="server" Text="Verify" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <br />
                                                                            <table>
                                                                                <tr>
                                                                                    <td width="90px">
                                                                                        <asp:Button ID="btnsave" ValidationGroup="a" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                                            CssClass="buttonCommman"  OnClick="btnSave_Click" />
                                                                                    </td>
                                                                                    <td width="90px">
                                                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                                            CssClass="buttonCommman" OnClick="btnReset_Click" />
                                                                                    </td>
                                                                                  
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                         
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                  
                                   
                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
              <asp:Panel ID="pnlAddress1" runat="server" class="MsgOverlayAddress" Visible="False">
                <asp:Panel ID="pnlAddress2" runat="server" class="MsgPopUpPanelAddress" Visible="False">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
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
                    <asp:Panel ID="pnlAddress3" runat="server" Style="width: 100%; height: 100%; text-align: center;"
                        >
                        <table width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                            <tr>
                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                    <asp:Label ID="lblAddressHeader" runat="server" Font-Size="14px" Font-Bold="true"
                                        CssClass="labelComman" Text="<%$ Resources:Attendance, Address Setup %>"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="btnClosePanel" runat="server" ImageUrl="~/Images/close.png"
                                        CausesValidation="False" OnClick="btnClosePanel_Click" Height="20px" Width="20px" />
                                </td>
                            </tr>
                        </table>
                     <UC:Addressmaster ID="addaddress" runat="server" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>