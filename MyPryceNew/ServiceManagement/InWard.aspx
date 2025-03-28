<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="InWard.aspx.cs" Inherits="ServiceManagement_InWard" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-compress-arrows-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Inward%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Inward%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Pending" Style="display: none;" runat="server" OnClick="btnPending_Click" Text="Pending" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
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
                    <li id="Li_Pending"><a href="#Pending" onclick="Li_Tab_Pending()" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Pending %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
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
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="box box-primary collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="lblDeviceParameter" runat="server" Text="Advance Search"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-lg-6">
                                                <asp:DropDownList runat="server" ID="ddlLocation" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                    <asp:ListItem Text="Posted" Value="Post"></asp:ListItem>
                                                    <asp:ListItem Text="Unposted" Value="UnPost" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Inward No" Value="Inward_Voucher_No"></asp:ListItem>
                                                    <asp:ListItem Text="Inward date" Value="Inward_Date"></asp:ListItem>
                                                    <asp:ListItem Text="Manufacturer Name" Value="ManufacturerName"></asp:ListItem>
                                                    <asp:ListItem Text="Contact Person" Value="ContactPersonName"></asp:ListItem>
                                                    <asp:ListItem Text="Received By" Value="receivedEmp"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center; display: none;">
                                                <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshReport_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvInWard.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="export" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="BtnExportPDF" />
                                                        <asp:PostBackTrigger ControlID="BtnExportExcel" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Button ID="BtnExportPDF" runat="server" Text="Export PDF" OnClick="BtnExportPDF_Click" />
                                                        <asp:Button ID="BtnExportExcel" runat="server" Text="Export Excel" OnClick="BtnExportExcel_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div class="col-md-12">
                                                <div class="flow">                                                                
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvInWard"></dx:ASPxGridViewExporter>

                                                    <dx:ASPxGridView ID="GvInWard" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Trans_Id">
                                                        <Columns>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command" CommandName='<%# Eval("Location_Id") %>'
                                                                        CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>

                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        OnCommand="IbtnDelete_Command"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn VisibleIndex="4" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnFileUpload" ToolTip="File-Upload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Inward_Voucher_No") %>' OnCommand="IbtnFileUpload_Command"><i class="fas fa-upload" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Inward_Voucher_No" Settings-AutoFilterCondition="Contains" Caption="InWard No" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataDateColumn Caption="InWard Date" FieldName="Inward_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="8" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="ManufacturerName" Settings-AutoFilterCondition="Contains" Caption="Manufacturer Name" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="ContactPersonName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Contact Name %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="receivedEmp" Settings-AutoFilterCondition="Contains" Caption="Received By" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>

                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                                        <SettingsCommandButton>
                                                            <EditButton>
                                                                <Image ToolTip="Edit" Url="~/Images/edit.png" />
                                                            </EditButton>


                                                        </SettingsCommandButton>
                                                        <Styles>
                                                            <CommandColumn Spacing="2px" Wrap="False" />
                                                        </Styles>

                                                    </dx:ASPxGridView>

                                                    <asp:HiddenField ID="hdnValue" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                                                      <div class="col-md-4">
                                                        <asp:Label ID="Label23" runat="server" Text="Location"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlLoc" CssClass="form-control" onchange="getDocNo(this)"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server" Text="InWard No."></asp:Label>
                                                        <asp:TextBox ID="txtInWardNo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCINo" runat="server" Text="InWard Date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInWarddate" ErrorMessage="<%$ Resources:Attendance,Enter InWard Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtInWarddate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtInWarddate"
                                                            Format="dd/MM/yyyy/hh/mm/ss" PopupButtonID="txtCIDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label2" runat="server" Text="Manufacturer Name"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSuppliername" ErrorMessage="<%$ Resources:Attendance,Enter Manufacturer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSuppliername" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtSuppliername"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblContact" runat="server" Text="<%$ Resources:Attendance,Contact Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtEContact" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtEContact_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtEContact"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblContactNo" runat="server" Text="<%$ Resources:Attendance,Contact No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtContactNo" ErrorMessage="<%$ Resources:Attendance,Enter Contact No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtContactNo" ValidChars="1,2,3,4,5,6,7,8,9,0,">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmailId" runat="server" Text="<%$ Resources:Attendance,Email ID %>"></asp:Label>
                                                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="Received By"></asp:Label>
                                                        <asp:TextBox ID="txtHandledEmp" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtHandledEmp_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListRefTo" ServicePath=""
                                                            TargetControlID="txtHandledEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesc" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                        <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme" Width="100%">
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Item Detail">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Item_Detail" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label14" runat="server" Text="Get Pass No."></asp:Label>
                                                                                    <asp:DropDownList ID="ddLgetPass" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddLgetPass_OnSelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label15" runat="server" Text="Verified By"></asp:Label>
                                                                                    <asp:TextBox ID="txtAssignedTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                        OnTextChanged="txtHandledEmp_TextChanged" AutoPostBack="true" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                                        ServiceMethod="GetCompletionListRefTo" ServicePath="" TargetControlID="txtAssignedTo"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_OnSelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:HiddenField ID="hdnInvoiceId" runat="server" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label8" runat="server" Text="Problem"></asp:Label>
                                                                                    <asp:TextBox ID="txtproblem" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label9" runat="server" Text="Job No."></asp:Label>
                                                                                    <asp:DropDownList ID="ddlJobNo" runat="server" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label11" runat="server" Text="Status"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
                                                                                        <asp:ListItem Text="InComplete" Value="InComplete"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label12" runat="server" Text="Job Type"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" Enabled="false">

                                                                                        <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                                                        <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label16" runat="server" Text="Qty"></asp:Label>
                                                                                    <asp:TextBox ID="txtqty" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label17" runat="server" Text="Repairing Detail"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpDetail" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label18" runat="server" Text="Repairing Charge"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpCharge" MaxLength="8" runat="server" CssClass="form-control" OnTextChanged="txtExpCharge_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpCharge" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <asp:Button ID="btnItemSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>" CssClass="btn btn-primary" OnClick="btnItemSave_Click" />

                                                                                    <asp:Button ID="btnItemCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                        CausesValidation="False" OnClick="btnItemCancel_Click" />

                                                                                    <asp:HiddenField ID="hdnItemEditId" runat="server" Value="0" />
                                                                                    <asp:HiddenField ID="hdnItemId" runat="server" Value="0" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="overflow: auto; max-height: 500px">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvItemDetail" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Edit">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton  ID="imgBtnItemEdit" runat="server" CommandArgument='<%# Eval("GetPass_Id") %>' OnCommand="imgBtnItemEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="imgBtnItemDelete" runat="server" CommandArgument='<%# Eval("GetPass_Id") %>' OnCommand="imgBtnItemDelete_Command" ><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="GetPass No.">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgetPassId" runat="server" Visible="false" Text='<%#Eval("GetPass_Id") %>' />
                                                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("GetPassNo") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Job No.">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblJobId" runat="server" Visible="false" Text='<%#Eval("Job_No") %>' />
                                                                                                    <asp:Label ID="lblJobNo" runat="server" Text='<%#Eval("Job_Ref_No") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvProduct" runat="server" Text='<%#ProductCode(Eval("Item_Id").ToString())%>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("Item_Id") %>' Visible="false" />
                                                                                                    <asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("Item_Id").ToString()) %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("Item_Id").ToString()) %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Verified By">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvAssignedto" runat="server" Text='<%#GetAssignedPersonName(Eval("Verified_By").ToString()) %>' />
                                                                                                    <asp:Label ID="lblgvAssignedtoId" runat="server" Text='<%#Eval("Verified_By") %>'
                                                                                                        Visible="false" />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Problem">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvProblem" runat="server" Text='<%#Eval("Problem") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Repairing Detail">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvExpdetail" runat="server" Text='<%#Eval("ExpDetail") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Repairing Charge">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvExpcharge" runat="server" Text='<%#Eval("ExpCharge") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Item_Detail">
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
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Shipping Information">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Shipping_Information" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Shipping Line %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingLine" runat="server" BackColor="#eeeeee" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtShippingLine_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                                                                                        ServicePath="" TargetControlID="txtShippingLine" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Select Expenses %>" />
                                                                                    <asp:DropDownList ID="ddlExpense" runat="server" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlExpense_SelectedIndexChanged" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblFCExpAmount" runat="server" Text="<%$ Resources:Attendance,Paid Amount %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtpaidamount" runat="server" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                                                        TargetControlID="txtpaidamount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblShippingAcc" runat="server" Text="Account (Credit)"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingAcc" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtShippingAcc_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShippingAcc"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpAccount" runat="server" Text="Expenses Account (Debit)"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpensesAccount" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtExpensesAccount_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Ship By %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipBy" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="By Truck" Value="By Track"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Ship %>" Value="By Ship"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Train %>" Value="By Train"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Air Freight %>" Value="By Air Freight"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Courier %>" Value="By Courier"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Airway Bill No. %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAirwaybillno" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Shipping_Information">
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
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnInquirySave_Click" Visible="false" />

                                                        <asp:Button ID="btnClose" runat="server" Text="Post" CssClass="btn btn-primary" OnClick="btnInquirySave_Click"
                                                            Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post record ?"
                                                            TargetControlID="btnClose">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnInquiryCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnInquiryCancel_Click" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Pending">
                        <asp:UpdatePanel ID="Update_Pending" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblQTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlQSeleclField_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="Gate Pass No" Value="Get_Pass_No"></asp:ListItem>
                                                        <asp:ListItem Text="Gate Pass date" Value="Get_Pass_Date"></asp:ListItem>
                                                        <asp:ListItem Text="Shipping date" Value="Shipping_date"></asp:ListItem>
                                                        <asp:ListItem Text="Expected Receive date" Value="Expected_Receive_Date"></asp:ListItem>
                                                        <asp:ListItem Text="Manufacturer Name" Value="ManufacturerName"></asp:ListItem>
                                                        <asp:ListItem Text="Contact Person" Value="ContactPersonName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlQOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="ImgBtnQBind">
                                                        <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtQValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtQValue" runat="server" TargetControlID="txtQValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" 
                                                        OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" 
                                                        OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSalesOrder" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging" OnSorting="gvPurchaseOrder_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvType" runat="server" Text='<%#Eval("Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gate Pass No" SortExpression="Get_Pass_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvworkNo" runat="server" Text='<%#Eval("Get_Pass_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gate Pass date" SortExpression="Get_Pass_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobrDate" runat="server" Text='<%#GetDate(Eval("Get_Pass_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Shipping date" SortExpression="Shipping_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpJobrDate" runat="server" Text='<%#GetDate(Eval("Shipping_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expected Receive date" SortExpression="Expected_Receive_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%#GetDate(Eval("Expected_Receive_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Manufacturer Name" SortExpression="ManufacturerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("ManufacturerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact Name" SortExpression="ContactPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactpersoneName" runat="server" Text='<%#Eval("ContactPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Id" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Name" SortExpression="EProductName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("EProductName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvQty" runat="server" Text='<%#Eval("Qty") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Problem" SortExpression="Problem">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvProblem" runat="server" Text='<%#Eval("Problem") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Script/master.js"></script>
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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Tab_Pending() {
            document.getElementById('<%= Btn_Pending.ClientID %>').click();
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function getDocNo(ctrl)
        {   
            var txtBox = document.getElementById('<%= txtInWardNo.ClientID %>');
            getDocumentNoByModuleNObjectId(ctrl, txtBox, '158', '350');
        }

    </script>
</asp:Content>
