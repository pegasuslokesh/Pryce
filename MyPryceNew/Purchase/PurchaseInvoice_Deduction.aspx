<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="true" AutoEventWireup="true" CodeFile="PurchaseInvoice_Deduction.aspx.cs" Inherits="Purchase_PurchaseInvoice_Deduction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/department_master.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Invoice Deduction"></asp:Label>
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Invoice Deduction"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="Btn_List_Click"
                Text="list" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="Btn_New_Click"
                Text="list" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="Btn_Bin_Click"
                Text="list" />
            <asp:Button ID="Btn_Purchase_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Purchase_Modal" Text="Purchase Modal" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13"
                            runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3"
                            runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">

                                            <div class="col-md-12" style="text-align: center">
                                                <asp:RadioButton ID="rbtndeductionsystem" runat="server" Text="Deduction System" GroupName="system" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtndeductionsystem_OnCheckedChanged" />
                                                <asp:RadioButton ID="rbtnRefundsystem" Style="margin-left: 15px;" runat="server" Text="Refund System" GroupName="system" AutoPostBack="true" OnCheckedChanged="rbtndeductionsystem_OnCheckedChanged" />
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                            </div>

                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>


                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Ref Type %>" Value="Ref_Type"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Reference Name %>" Value="Field3"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Invoice No %>" Value="Field2"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </asp:Panel>                                                
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                <asp:ImageButton ID="btnRefresh" runat="server" Style="width: 33px;" CausesValidation="False"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5 class="text-center">
                                                    <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlan" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvsalaryPlan_PageIndexChanging" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        AllowSorting="True"  OnSorting="GvsalaryPlan_Sorting">
                                                        
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' TabIndex="9"
                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="View"
                                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" Visible="false" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/Erase.png" Visible="false" OnCommand="IbtnDelete_Command"
                                                                        Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle  />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Refund" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkRefundDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'  CommandName='<%# Eval("Ref_Id") %>' TabIndex="9"
                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="View"
                                                                        OnCommand="lnkRefundDetail_Command" CausesValidation="False" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ref Type" SortExpression="Ref_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrefType" runat="server" Text='<%#Eval("Ref_Type").ToString()%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Reference Name" SortExpression="Field3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrefName" runat="server" Text='<%#Eval("Field3").ToString()%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Invoice No" SortExpression="Field2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("Field2").ToString()%>'></asp:Label>
                                                                    <asp:Label ID="lblInvoiceamt" runat="server" Text='<%#Eval("Field4")%>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblsupplierInvoiceNo" runat="server" Text='<%#Eval("Field6")%>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblRefidvalue" runat="server" Text='<%#Eval("Other_Account_No")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                
                                                                <ItemStyle  />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Deduction Type" SortExpression="Deduction_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeductiontype" runat="server" Text='<%#Eval("Deduction_Type")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deduction Amount" SortExpression="Deduction_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeductionamt" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Deduction_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Refund Amount" SortExpression="Refund_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefundamt" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Refund_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Refund" SortExpression="Is_Refund">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIsRefund" runat="server" Text='<%#Eval("Is_Refund")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CreatedUser" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="CreatedUser" >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModifiedUser" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="ModifiedUser" >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        
                                                    </asp:GridView>

                                                    <asp:HiddenField ID="hdnInvoiceType" runat="server" />
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
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="Ref Type"></asp:Label>

                                                            <asp:DropDownList ID="ddlrefType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlrefType_OnSelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Supplier%>" Value="Supplier"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Customer%>" Value="Customer"></asp:ListItem>

                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Text="Ref. Name"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator8" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtCustomer" ErrorMessage="Enter Reference Name"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtCustomer_TextChanged" />


                                                            <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomer"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <br />
                                                        </div>
                                                    </div>



                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="Deduction Type"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeductionType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDeductionType_OnSelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="TDS" Value="TDS"></asp:ListItem>
                                                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>

                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="ddlInvoice" InitialValue="0" ErrorMessage="Select Invoice" />

                                                            <asp:DropDownList ID="ddlInvoice" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlInvoice_OnSelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Text="Invoice Amount"></asp:Label>
                                                            <asp:TextBox ID="txtInvoiceAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label7" runat="server" Text="Applicable Amount"></asp:Label>

                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtApplicableAmount" ErrorMessage="Enter Applicable Amount"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtApplicableAmount" runat="server" CssClass="form-control" OnTextChanged="CommonCalculation" AutoPostBack="true"></asp:TextBox>

                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtApplicableAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>

                                                    </div>

                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="Deduction Method"></asp:Label>
                                                            <asp:DropDownList ID="ddldeductionMethod" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddldeductionMethod_OnSelectedIndexChanged">
                                                                <asp:ListItem Text="Percentage (%)" Value="Percentage"></asp:ListItem>
                                                                <asp:ListItem Text="Fixed" Value="Fixed"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="Deduction Value"></asp:Label>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtdeductionValue" ErrorMessage="Enter Deduction Value"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtdeductionValue" runat="server" CssClass="form-control" OnTextChanged="CommonCalculation" AutoPostBack="true"></asp:TextBox>

                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtdeductionValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>

                                                        </div>
                                                    </div>


                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="Deduction Amount"></asp:Label>
                                                            <asp:TextBox ID="txtdeductionAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                            <%-- <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtDebitAccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>--%>
                                                            <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="false"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidatorCreditaccount" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtpaymentCreditaccount" ErrorMessage="Enter Credit Account" />
                                                            <asp:TextBox ID="txtpaymentCreditaccount" runat="server" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>" Visible="false"
                                                            class="btn btn-primary" OnClick="Btn_Save_Click" />

                                                        <asp:Button ID="btn_Post" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Post%>"
                                                            class="btn btn-primary" OnClick="Btn_Post_Click" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to post record ?"
                                                            TargetControlID="btn_Post">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset%>"
                                                            class="btn btn-primary" OnClick="Btn_Reset_Click" />

                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            class="btn btn-primary" OnClick="Btn_Cancel_Click" />
                                                    </div>
                                                    <asp:HiddenField ID="hdnEditId" runat="server" />
                                                    <asp:HiddenField ID="hdnsupplierInvoiceNo" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Ref Type %>" Value="Ref_Type"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Reference Name %>" Value="Field3"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Invoice No %>" Value="Field2"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                    <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnbinbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                <asp:ImageButton ID="btnbinRefresh" Style="width: 33px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                <asp:ImageButton ID="imgBtnRestore" Style="width: 33px;" CausesValidation="False" Visible="false"
                                                    runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" />
                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                            </div>
                                            <div class="col-lg-2">
                                                <h5 class="text-center">
                                                    <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlanBin" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" DataKeyNames="Trans_Id" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        OnPageIndexChanging="GvsalaryPlanBin_PageIndexChanging" OnSorting="GvsalaryPlanBin_Sorting"
                                                        AllowSorting="true" >
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ref Type" SortExpression="Ref_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrefType" runat="server" Text='<%#Eval("Ref_Type").ToString()%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Field3" HeaderText="Reference Name" SortExpression="Field3"
                                                                >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Field2" HeaderText="Invoice No" SortExpression="Field2"
                                                                >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Deduction Type" SortExpression="Deduction_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeductiontype" runat="server" Text='<%#Eval("Deduction_Type")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deduction Amount" SortExpression="Deduction_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeductionamt" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Deduction_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CreatedUser" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="CreatedUser" >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModifiedUser" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="ModifiedUser" >
                                                                <ItemStyle  />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        
                                                        
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="Purchase_Modal" tabindex="-1" role="dialog" aria-labelledby="Purchase_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Purchase_ModalLabel">
                        <asp:UpdatePanel ID="Update_Modal_Title" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Lbl_Modal_Title" runat="server" Font-Bold="true" Text="SD Refund"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator4" ValidationGroup="Refund" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtVoucherDate" ErrorMessage="Enter Voucher Date"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control"></asp:TextBox>

                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtVoucherDate" runat="server" TargetControlID="txtVoucherDate" />

                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>

                                                    <asp:TextBox ID="lblSupplierNameValue" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                    <br />
                                                </div>


                                                <div class="col-md-6">
                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Invoice No. %>"></asp:Label>

                                                    <asp:TextBox ID="lblInvoiceNoValue" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Invoice Amount %>"></asp:Label>

                                                    <asp:TextBox ID="lblInvoiceAmountValue" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label19" runat="server" Text="Deduction Amount"></asp:Label>

                                                    <asp:TextBox ID="lbldeductionAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label22" runat="server" Text="Refund Amount"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator5" ValidationGroup="Refund" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtRefundAmount" ErrorMessage="Enter Refund Amount"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtRefundAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtRefundAmount" runat="server" Enabled="True"
                                                        TargetControlID="txtRefundAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>


                                                <asp:HiddenField ID="hdnRefId" runat="server" />

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator256" ValidationGroup="Refund" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtdebitRefundAccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtdebitRefundAccount" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="true"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtdebitRefundAccount"
                                                        UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator6" ValidationGroup="Refund" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtCreditRefundAccount" ErrorMessage="Enter Credit Account" />
                                                    <asp:TextBox ID="txtCreditRefundAccount" runat="server" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCreditRefundAccount"
                                                        UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="editid" runat="server" />

                            <asp:Button ID="btnRefundSave" runat="server" ValidationGroup="Refund" Text="<%$ Resources:Attendance,Save%>" Visible="false"
                                class="btn btn-primary" OnClick="btnRefundSave_Click" />

                            <asp:Button ID="btnrefundPost" runat="server" ValidationGroup="Refund" Text="<%$ Resources:Attendance,Post%>"
                                class="btn btn-primary" OnClick="btnrefundPost_Click" Visible="false" />
                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Are you sure to post record ?"
                                TargetControlID="btnrefundPost">
                            </cc1:ConfirmButtonExtender>
                            <%--<asp:Button ID="btnCancelConfig" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelConfig_Click" Text="<%$ Resources:Attendance,Cancel %>"/>--%>

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
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
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false">
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_List_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function LI_Bin_Active() {
            $("#Li_Bin").removeClass("active");
            $("#Bin").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
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

        function Show_Purchase_Modal() {
            document.getElementById('<%=Btn_Purchase_Modal.ClientID%>').click();
        }

     
    </script>
</asp:Content>

