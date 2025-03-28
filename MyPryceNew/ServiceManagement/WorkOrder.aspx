<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="WorkOrder.aspx.cs" Inherits="ServiceManagement_WorkOrder" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function alertMe() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_Edit_Active1() {
        }


        function resetPosition1() {

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Work Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Work Order%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hasvalue" />
            <asp:HiddenField runat="server" ID="canEdit" />
            <asp:HiddenField runat="server" ID="canView" />
            <asp:HiddenField runat="server" ID="canDelete" />
            <asp:HiddenField runat="server" ID="canSave" />
            <asp:HiddenField runat="server" ID="canPrint" />
            <asp:HiddenField runat="server" ID="canUpload" />
            <asp:HiddenField runat="server" ID="canFollowup" />
            <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="WorkOrder" />

            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:Button ID="Btn_NewAddress" Style="display: none;" runat="server" data-toggle="modal" data-target="#NewAddress" Text="New Address" />
            <asp:Button ID="Btn_Followup_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup123" Text="Add Followup" />
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
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Li">
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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
                                            <asp:Label ID="Label7"
                                                runat="server" Text="Advance Search"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-lg-12">
                                                <asp:DropDownList runat="server" ID="ddlLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Completed %>" Value="Complete"></asp:ListItem>
                                                    <asp:ListItem Text="In Progress" Value="In progress" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Extended" Value="Extended"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:HiddenField ID="hdnEmpList" runat="server" />
                                                <%--<asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"></asp:DropDownList>--%>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Visit No." Value="Work_Order_No"></asp:ListItem>
                                                    <asp:ListItem Text="Visit date" Value="Work_Order_Date"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Reference Type %>" Value="Ref_Type"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Ref No. %>" Value="Ref_Id"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Invoice No %>" Value="Invoice_No"></asp:ListItem>
                                                    <%-- <asp:ListItem Text="Work Type" Value="Work_Type"></asp:ListItem>--%>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                    <asp:ListItem Text="Contact Person" Value="ContactPersonName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Handled Employee %>" Value="EmployeeName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
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
                                                    <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnHavePermission" runat="server" />
                                <div class="box box-warning box-solid">
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
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvWorkOrder"></dx:ASPxGridViewExporter>
                                                    <dx:ASPxGridView ID="GvWorkOrder" OnFillContextMenuItems="GvWorkOrder_FillContextMenuItems" OnContextMenuItemClick="GvWorkOrder_ContextMenuItemClick" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Trans_Id">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="Work_Order_No" Settings-AutoFilterCondition="Contains" Caption="Visit No" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Visit Date" FieldName="Work_Order_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="7" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Customer Name %>" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Ref_Type" Settings-AutoFilterCondition="Contains" Caption="Ref Type" VisibleIndex="9">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Ref_Id" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Ref No. %>" VisibleIndex="10">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Invoice_No" Settings-AutoFilterCondition="Contains" Caption="Invoice No." VisibleIndex="11">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="invoiceAmt" Caption="Invoice Amt" VisibleIndex="12">
                                                                <DataItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# SetDecimal(Eval("invoiceAmt").ToString()) %>'></asp:Label>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="ContactPersonName" Settings-AutoFilterCondition="Contains" Caption="Contact Name" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="EmployeeName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Handled Employee%>" VisibleIndex="14">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Status" Caption="<%$ Resources:Attendance,Status %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Employee Visited Date" FieldName="ActualVisitDate"
                                                                ShowInCustomizationForm="True" VisibleIndex="16" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblvisiteddate" runat="server" Text='<%# GetDate(Eval("ActualVisitDate").ToString()) %>' BackColor='<%# getColor(Eval("ActualVisitDate").ToString()) %>'></asp:Label>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Trans_Id" Visible="false">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Customer_Id" Visible="false">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <ClientSideEvents ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }" />
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="invoiceAmt" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <GroupSummary>
                                                            <dx:ASPxSummaryItem SummaryType="Count" />
                                                        </GroupSummary>
                                                        <SettingsContextMenu Enabled="true" />
                                                        <SettingsBehavior EnableRowHotTrack="true" />
                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" ShowFooter="true" />
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
                                                        <asp:Label ID="Label4" runat="server" Text="Visit No."></asp:Label>
                                                        <asp:TextBox ID="txtorderNo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCINo" runat="server" Text="Visit Date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOrderDate" ErrorMessage="<%$ Resources:Attendance,Enter Visit Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtOrderDate"
                                                            Format="dd/MM/yyyy/hh/mm/ss" PopupButtonID="txtCIDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" OnTextChanged="txtCustomer_TextChanged"
                                                                BackColor="#eeeeee" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                                ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomer"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddNewCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddNewCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="Site Address" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtSiteAddress" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtShipingAddress_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender_shipaddress" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSiteAddress"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="AddAddress" runat="server" CssClass="btn btn-primary" OnClick="AddAddress_Click"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblContact" runat="server" Text="<%$ Resources:Attendance,Contact Name %>"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtEContact" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee" onchange="validateContactPerson(this)" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                                ServiceMethod="GetCompletionListContactPerson" ServicePath="" TargetControlID="txtEContact"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlReftype" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlReftype_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                                                            <asp:ListItem Text="Project" Value="Project"></asp:ListItem>
                                                            <asp:ListItem Text="Contract" Value="Contract"></asp:ListItem>
                                                            <asp:ListItem Text="Invoice" Value="Invoice"></asp:ListItem>
                                                            <asp:ListItem Text="Ticket" Value="Ticket"></asp:ListItem>
                                                            <asp:ListItem Text="Campaign" Value="Campaign"></asp:ListItem>
                                                            <asp:ListItem Text="Marketing" Value="Marketing"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Ref No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true"
                                                            OnTextChanged="txtRefNo_OnTextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderProject" runat="server" DelimiterCharacters=""
                                                            Enabled="false" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListProjectNo" ServicePath="" TargetControlID="txtRefNo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderContract" runat="server" DelimiterCharacters=""
                                                            Enabled="false" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContractNo" ServicePath="" TargetControlID="txtRefNo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderInvoiceNo" runat="server" DelimiterCharacters=""
                                                            Enabled="false" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" TargetControlID="txtRefNo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderTicket" runat="server"
                                                            DelimiterCharacters="" Enabled="false" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListTicketNo" ServicePath=""
                                                            TargetControlID="txtRefNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderCampaign" runat="server"
                                                            DelimiterCharacters="" Enabled="false" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCampaign" ServicePath=""
                                                            TargetControlID="txtRefNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trtasklist" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label19" runat="server" Text="Project Task"></asp:Label>
                                                        <asp:DropDownList ID="ddltask" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <asp:HiddenField ID="hdnTicketid" runat="server" />
                                                    <asp:TextBox ID="txtticketno" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        Visible="false" />
                                                    <%-- <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Ticket No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtticketno" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtticketno_OnTextChanged" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListTicketNo" ServicePath=""
                                                            TargetControlID="txtticketno" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <%--<asp:Label ID="lnkticketdesc" runat="server" Text="<%$ Resources:Attendance,Detail%>"
                                                        Font-Underline="true"  Visible="false" ForeColor="Blue"></asp:Label>
                                                        <br />
                                                    </div>--%>
                                                    <div class="col-md-6" id="Div_Work_type" runat="server" visible="false">
                                                        <asp:Label ID="Label15" runat="server" Text="Work Type"></asp:Label>
                                                        <asp:DropDownList ID="DdlWorkType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Warranty Support" Value="Warranty Support"></asp:ListItem>
                                                            <asp:ListItem Text="Maintanance Contract" Value="Maintanance Contract"></asp:ListItem>
                                                            <asp:ListItem Text="Installation" Value="Installation"></asp:ListItem>
                                                            <asp:ListItem Text="Onsite Repair charges" Value="Onsite Repair charges"></asp:ListItem>
                                                            <asp:ListItem Text="Office Work" Value="Office Work"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="trTicketDetail" runat="server" class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblDeviceParameter" Font-Names="Times New roman" Font-Size="18px"
                                                                                Font-Bold="true" runat="server" Text="Ticket Detail"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblTiDate" runat="server" Text="<%$ Resources:Attendance,Ticket Date %>"
                                                                                    Font-Bold="true"></asp:Label>
                                                                                &nbsp:&nbsp<asp:Label ID="lblTickeDate" runat="server"></asp:Label>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"
                                                                                    Font-Bold="true"></asp:Label>
                                                                                &nbsp:&nbsp<asp:Label ID="lblCustomerNameValue" runat="server"></asp:Label>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblCallType" runat="server" Text="<%$ Resources:Attendance,Task Type %>"
                                                                                    Font-Bold="true"></asp:Label>
                                                                                &nbsp:&nbsp<asp:Label ID="lblTaskType" runat="server">
                                                                                </asp:Label>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Status%>"
                                                                                    Font-Bold="true"></asp:Label>
                                                                                &nbsp:&nbsp<asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Schedule Date %>"
                                                                                    Font-Bold="true"></asp:Label>
                                                                                &nbsp:&nbsp<asp:Label ID="lblScheduledate" runat="server"></asp:Label>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblDesription" runat="server" Text="<%$ Resources:Attendance,Description %>"
                                                                                    Font-Bold="true" />
                                                                                &nbsp:&nbsp<asp:Label ID="lblDescriptionvalue" runat="server"></asp:Label>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label16" runat="server" Text="Visit Employee"></asp:Label>
                                                        <asp:TextBox ID="txtselectEmployee" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="txtTablename_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                                            ServicePath="" TargetControlID="txtselectEmployee" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:ListBox ID="listtaskEmployee" Visible="false" runat="server" Height="200px" SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblHandledEmp" runat="server" Text="<%$ Resources:Attendance,Handled Employee %>" />
                                                        <asp:TextBox ID="txtHandledEmp" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            onchange="validateEmployee(this)" />
                                                        <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListRefTo" ServicePath=""
                                                            TargetControlID="txtHandledEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                                            Enabled="false">
                                                            <asp:ListItem Text="Open" Value="In progress"></asp:ListItem>
                                                            <asp:ListItem Text="Close" Value="Complete"></asp:ListItem>
                                                            <asp:ListItem Text="Extended" Value="Extended"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:HiddenField ID="hdnExtendedID" runat="server" />
                                                        <asp:Label ID="lblExtendedID" runat="server" Text="Extended Work Order"></asp:Label>
                                                        <asp:TextBox ID="txtExtendID" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtExtendID_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListVisit" ServicePath=""
                                                            TargetControlID="txtExtendID" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblActualVisitDate" runat="server" Text="Employee Visited Date"></asp:Label>
                                                        <asp:TextBox ID="txtActualVisitDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesc" runat="server" Text="Task"></asp:Label>
                                                        <asp:TextBox ID="txtProbelm" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="div_inTime" runat="server">
                                                        <asp:Label ID="Label14" runat="server" Text="Start Time" /><a style="color: red">*</a>
                                                        <asp:TextBox ID="txtInTime" runat="server" onchange="workingHours()" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Add_Date"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInTime" InitialValue="__:__" ErrorMessage="<%$ Resources:Attendance,Enter Start Time %>"></asp:RequiredFieldValidator>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtInTime"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                            ControlToValidate="txtInTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                            SetFocusOnError="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="div_outTime" runat="server">
                                                        <asp:Label ID="Label17" runat="server" Text="End Time"></asp:Label><a style="color: red">*</a>
                                                        <asp:TextBox ID="txtOuttime" onchange="workingHours()" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add_Date"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOuttime" InitialValue="__:__" ErrorMessage="<%$ Resources:Attendance,Enter End Time %>"></asp:RequiredFieldValidator>
                                                        <cc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOuttime"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                            ControlToValidate="txtOuttime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                            SetFocusOnError="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="div_workingHrs" runat="server">
                                                        <asp:Label ID="lblWorkingHr" runat="server" Text="Working Hours"></asp:Label>
                                                        <asp:TextBox ID="txtWorkingHr" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtWorkingHr"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                    </div>
                                                    <div class="col-md-3" id="div_call" runat="server" style="display: none;">
                                                        <br />
                                                        <asp:CheckBox ID="ChkCourtesyCall" runat="server" Text="Courtesy Call" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-11">
                                                        <asp:Label ID="Label18" runat="server" Text="Employee Comments"></asp:Label>
                                                        <asp:TextBox ID="txtEngComments" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-1">
                                                        <br />
                                                        <asp:Button ID="btnShowMaps" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Map %>" OnClientClick="window.open('../SystemSetup/GoogleMap.aspx','window','width=1024');return false;" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label20" runat="server" Text="Customer Feedback"></asp:Label>
                                                        <asp:TextBox ID="txtCustomerfeedback" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme" Width="100%">
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Job Plan">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Job_Plan" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Job Plan Id %>"></asp:Label>&nbsp&nbsp;:&nbsp&nbsp;
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtjobPlanId" TabIndex="104" runat="server" BackColor="#eeeeee"
                                                                                            AutoPostBack="true" CssClass="form-control" OnTextChanged="txtjobPlanId_OnTextChanged" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                                            ServiceMethod="GetCompletionListJobPlan" ServicePath="" TargetControlID="txtjobPlanId"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:TextBox ID="txtjobPlanName" TabIndex="105" Width="200px" runat="server" CssClass="form-control"
                                                                                                Enabled="false" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitTask" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                        Width="100%" ShowFooter="true" OnRowDeleting="gvVisitTask_RowDeleting"
                                                                                        OnRowEditing="gvVisitTask_RowEditing" OnRowCancelingEdit="gvVisitTask_OnRowCancelingEdit"
                                                                                        OnRowUpdating="gvVisitTask_OnRowUpdating" OnRowCommand="gvVisitTask_RowCommand">

                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblsno1" runat="server" Text='<%# Container.DataItemIndex+1 %>'
                                                                                                        Width="20px"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                                                                <FooterStyle Width="20px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblItemTask" runat="server" Text='<%#Eval("Work") %>'
                                                                                                        Width="660px"></asp:Label>
                                                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:TextBox ID="txteditTask" runat="server" CssClass="form-control" Width="680px"
                                                                                                        Text='<%#Eval("Work") %>'></asp:TextBox>
                                                                                                </EditItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtFooterTask" runat="server" CssClass="form-control" Width="660px"></asp:TextBox>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="660px" />
                                                                                                <FooterStyle Width="660px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Minutes %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblsno" runat="server" Text='<%# Eval("Minute") %>'
                                                                                                        Width="20px"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:TextBox ID="txEdittMinutes" Width="95px" runat="server" CssClass="form-control"
                                                                                                        Text='<%#Eval("Minute") %>' />
                                                                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                        Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txEdittMinutes"
                                                                                                        UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                    </cc1:MaskedEditExtender>
                                                                                                </EditItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtMinutes" Width="95px" runat="server" CssClass="form-control" />
                                                                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                                                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                        Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtMinutes"
                                                                                                        UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                    </cc1:MaskedEditExtender>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                <FooterStyle Width="60px" HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:Panel ID="pnlupdatework" runat="server" DefaultButton="imgupdatework">
                                                                                                        <asp:ImageButton ID="imgupdatework" runat="server" ImageUrl="~/Images/Allow.png"
                                                                                                            CommandArgument='<%#Eval("Trans_Id") %>' CommandName="Update" ToolTip="<%$ Resources:Attendance,Update %>" />
                                                                                                        &nbsp;&nbsp;
                                                                                                    <asp:ImageButton ID="IbtnCancel" runat="server" CausesValidation="False" ImageUrl="~/Images/Erase.png"
                                                                                                        CommandName="Delete" Width="16px" ToolTip="<%$ Resources:Attendance,Cancel %>" />
                                                                                                    </asp:Panel>
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;                                                                                                
                                                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName="Delete" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="IbtnAddTask">
                                                                                                        <asp:LinkButton ID="IbtnAddTask" runat="server" CausesValidation="False" CommandName="AddNew" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                                    </asp:Panel>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                                <FooterStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Job_Plan">
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
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Parts & Tools">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Parts_Tools" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTools" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                        Width="100%" ShowFooter="true" OnRowDeleting="gvTools_RowDeleting"
                                                                                        OnRowEditing="gvTools_RowEditing" OnRowCancelingEdit="gvTools_OnRowCancelingEdit"
                                                                                        OnRowCommand="gvTools_RowCommand">

                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Tools Id">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblproductCode" runat="server" Text='<%#Eval("ProductCode") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:TextBox ID="f" runat="server"></asp:TextBox>
                                                                                                </EditItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                        OnTextChanged="txttoolsProductCode_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtendertxtProductCode" runat="server"
                                                                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                                        ServiceMethod="GetCompletionListRelatedProductCode" ServicePath="" TargetControlID="txtProductCode"
                                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Tools Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblproductName" runat="server" Text='<%#Eval("EProductName") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtERelatedProduct" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                        OnTextChanged="txtToolsERelatedProduct_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListRelatedProductName"
                                                                                                        ServicePath="" TargetControlID="txtERelatedProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                    <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("Unit_Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:DropDownList ID="ddlunit" runat="server" CssClass="form-control" Width="80px">
                                                                                                    </asp:DropDownList>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblquantity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isNumber(event,this);" Width="50px"></asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtquantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Unit Price">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblUnitprice" runat="server" Text='<%#Eval("Field2") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isNumber(event,this);" Width="50px"></asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderUnitPrice" runat="server"
                                                                                                        Enabled="True" TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                        CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                                    <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-info" CommandName="Cancel" Text="Cancel" />
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" CssClass="btn btn-info" Text="Edit" Visible="false" />
                                                                                                    <asp:ImageButton ID="ButtonDelete" ImageUrl="~/Images/Erase.png" Width="16px" Height="16px" runat="server" CommandArgument='<%#Eval("Trans_Id") %>' CommandName="Delete" />
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Panel ID="pnlGridviewTools" runat="server" DefaultButton="ButtonAdd">
                                                                                                        <asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" CssClass="btn btn-info" />
                                                                                                    </asp:Panel>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Parts_Tools">
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
                                                            CssClass="btn btn-success" OnClick="btnInquirySave_Click" OnClientClick="this.disabled='true'; this.value='please wait...'; " Visible="false" ValidationGroup="Add_Date" />
                                                        <asp:Button ID="btnInquiryCloseandInvoice" runat="server" Text="Create Invoice" ValidationGroup="Add_Date"
                                                            CssClass="btn btn-primary" OnClick="btnInquiryCloseandInvoice_Click"
                                                            Visible="false" />
                                                        <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:Attendance,Close %>"
                                                            CssClass="btn btn-primary" OnClick="btnClose_Click" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to close Work order ?"
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                               <asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="Visit No." Value="Work_Order_No"></asp:ListItem>
                                                        <asp:ListItem Text="Visit Date" Value="Work_Order_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Handled Employee %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="Work Type" Value="Work_Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false" TabIndex="36"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="box box-warning box-solid" <%= GvCustomerInquiryBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerInquiryBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomerInquiryBin_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCustomerInquiryBin_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Visit No." SortExpression="Work_Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvworkNo" runat="server" Text='<%#Eval("Work_Order_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Visit Date" SortExpression="Work_Order_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvorderDate" runat="server" Text='<%#GetDate(Eval("Work_Order_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Handled Employee %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Work Type" SortExpression="Work_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactNo" runat="server" Text='<%#Eval("Work_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCallType" runat="server" Text='<%#Eval("Status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
    <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                    </h4>
                </div>
                <div class="modal-body">
                    <RS:ReportSystem runat="server" ID="reportSystem" />
                </div>
                <div class="modal-footer">
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
    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="NewAddress" tabindex="-1" role="dialog" aria-labelledby="NewAddress_ModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AA1:AddAddress ID="addaddress1" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Followup123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:AddFollowup ID="FollowupUC" runat="server" />
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
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function OnContextMenuItemClick(sender, args) {
            //IbtnAcceptancePrint IbtnPrint
            //if (args.item.name == "IbtnTrainingPrint" || args.item.name == "IbtnTrainingPrint" || args.item.name == "IbtnTrainingPrint") {
            args.processOnServer = true;
            args.usePostBack = true;
            //} else if (args.item.name == "SumSelected")
            //    args.processOnServer = true;
        }
        function workingHours() {
            startTime = document.getElementById('<%= txtInTime.ClientID %>').value;
            endTime = document.getElementById('<%= txtOuttime.ClientID %>').value;
            if (startTime == "") {
                return;
            }
            if (endTime == "") {
                return;
            }
            var start = startTime.split(":");
            var end = endTime.split(":");
            var startDate = new Date(0, 0, 0, start[0], start[1], 0);
            var endDate = new Date(0, 0, 0, end[0], end[1], 0);
            var diff = endDate.getTime() - startDate.getTime();
            var hours = Math.floor(diff / 1000 / 60 / 60);
            diff -= hours * 1000 * 60 * 60;
            var minutes = Math.floor(diff / 1000 / 60);
            if (hours < 0)
                hours = hours + 24;
            var timeDiff = (hours <= 9 ? "0" : "") + hours + ":" + (minutes <= 9 ? "0" : "") + minutes;
            document.getElementById('<%= txtWorkingHr.ClientID %>').value = timeDiff;
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }
        function Modal_NewAddress_Open() {
            document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
        }
        function Modal_Followup_Open() {
            document.getElementById('<%= Btn_Followup_Modal.ClientID %>').click();
        }
        function LI_List_Active1() {
            $("#Li_FollowupList").addClass("active");
            $("#FollowupList1").addClass("active");
            $("#Li_New1").removeClass("active");
            $("#New1").removeClass("active");
        }
        function LI_Edit_Active1() {
            $("#Li_FollowupList1").removeClass("active");
            $("#FollowupList1").removeClass("active");
            $("#Li_New1").addClass("active");
            $("#New1").addClass("active");
        }

        function btnVisible(id) {
            document.getElementById('btncall').style.display = 'none';
            document.getElementById('btnvisit').style.display = 'none';

            if (id.value == "Call") {
                document.getElementById('btncall').style.display = 'block';
            }
            if (id.value == "Visit") {
                document.getElementById('btnvisit').style.display = 'block';
            }
        }
        function getDocNo(ctrl) {
            var txtBox = document.getElementById('<%= txtorderNo.ClientID %>');
            getDocumentNo(ctrl, txtBox);
        }
        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
        }
    </script>
    <script src="../Script/customer.js"></script>
    <script src="../Script/master.js"></script>
    <script src="../Script/employee.js"></script>
    <script src="../Script/ReportSystem.js"></script>
</asp:Content>
