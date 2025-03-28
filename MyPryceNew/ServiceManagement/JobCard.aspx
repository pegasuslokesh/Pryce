<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="JobCard.aspx.cs" Inherits="ServiceManagement_JobCard" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-id-card"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Job Card%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Job Card%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_RepairHistory" Style="display: none;" data-toggle="modal" data-target="#RepairHistory" runat="server" Text="Repair History" />
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
                                            <asp:Label ID="Label29" runat="server" Text="Advance Search"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-lg-6">
                                                <asp:DropDownList runat="server" ID="ddlLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Close %>" Value="Close"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Open %>" Value="Open" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display: none;">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                    <asp:ListItem Text="Type" Value="Field3"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="Job No." Value="Job_No"></asp:ListItem>
                                                    <asp:ListItem Text="Job Date" Value="Job_date"></asp:ListItem>
                                                    <asp:ListItem Text="Invoice No" Value="InvoiceNo"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Handled Employee %>" Value="SalesPersonName"></asp:ListItem>
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
                                                    <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvJobCard.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>

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
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvJobCard"></dx:ASPxGridViewExporter>
                                                    <dx:ASPxGridView ID="GvJobCard"  ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id">
                                                       
                                                        <Columns>
                                                             <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <a href="#" onclick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("location_id") %>');return false;" ><i class="fa fa-print" style="font-size:15px;cursor: pointer;"></i></a>
                                                                    <%--<asp:LinkButton ID="IbtnReportSystem" runat="server" OnClick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("location_id") %>');return false;"   ToolTip="Print Job Order"><i class="fa fa-print" style="font-size:15px;cursor: pointer;"></i></asp:LinkButton>--%>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnPrint" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'   CommandName='<%# Eval("Customer_id") %>' OnCommand="IbtnPrint_Command"  CausesValidation="False"   ToolTip="Print Job Order"><i class="fa fa-print" style="font-size:15px;cursor: pointer;"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                        CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkEditDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        OnCommand="IbtnDelete_Command"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Field3" Settings-AutoFilterCondition="Contains" Caption="Type" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Job_No" Settings-AutoFilterCondition="Contains" Caption="Job No" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>



                                                            <dx:GridViewDataDateColumn Caption="Job Date" FieldName="Job_date"
                                                                ShowInCustomizationForm="True" VisibleIndex="8" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataDateColumn Caption="Expected End Date" FieldName="Expected_Job_End_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="9" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataDateColumn Caption="End Date" FieldName="Job_End_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="10" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="InvoiecNo" Settings-AutoFilterCondition="Contains" Caption="Invoice No" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Customer Name %>" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>




                                                            <dx:GridViewDataTextColumn FieldName="ContactPersonName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Contact Name %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="SalesPersonName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Handled Employee %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Status" Caption="Status" Settings-AutoFilterCondition="Contains" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Total" VisibleIndex="17" Caption="Total Amt">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblgvUnitPrice" runat="server" Text='<%# SetDecimal(Eval("Total").ToString()) %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="labourCharges" VisibleIndex="17" Caption="Labour Charges">
                                                                <DataItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# SetDecimal(Eval("labourCharges").ToString()) %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="otherCharges" VisibleIndex="17" Caption="Other Charges">
                                                                <DataItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# SetDecimal(Eval("otherCharges").ToString()) %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="labourCharges" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="otherCharges" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="Total" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <GroupSummary>
                                                            <dx:ASPxSummaryItem SummaryType="Count" />
                                                        </GroupSummary>
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
						  <Triggers>
                                
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlJobType_OnSelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                            <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label28" runat="server" Text="Location"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlLoc" CssClass="form-control" onchange="getDocNo(this)"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server" Text="Job No."></asp:Label>
                                                        <asp:TextBox ID="txtJobNo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCINo" runat="server" Text="Job Date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtjobdate" ErrorMessage="<%$ Resources:Attendance,Enter Job Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtjobdate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtjobdate" Format="dd/MM/yyyy/hh/mm/ss"
                                                            PopupButtonID="txtCIDate" />
                                                        <br />
                                                    </div>
                                                    <div id="trCustomer" runat="server" class="col-md-12">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtECustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtECustomer" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtECustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div id="trCustomerhistory" runat="server" class="input-group-btn">
                                                                <asp:Button ID="lnkcustomerHistory" runat="server" CssClass="btn btn-info" Text="Customer History" OnClick="lnkcustomerHistory_OnClick" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="trContact" runat="server">
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
                                                    <div id="trContactNo" runat="server">
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
                                                    </div>
                                                    <div id="trinvType" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="Invoice Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Cash" Value="Repair"></asp:ListItem>
                                                            <asp:ListItem Text="Credit" Value="Exhibition"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label3" runat="server" Text="Expected End Date"></asp:Label>
                                                        <asp:TextBox ID="txtExpectedEnddate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtExpectedEnddate" runat="server" TargetControlID="txtExpectedEnddate"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="div_endDate" style="display: none;" runat="server">
                                                        <asp:Label ID="Label13" runat="server" Text="End Date"></asp:Label>
                                                        <asp:TextBox ID="txtEnddate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtEnddate" runat="server" TargetControlID="txtEnddate"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="div_call" style="display: none;" runat="server">
                                                        <br />
                                                        <asp:CheckBox ID="ChkCall" runat="server" Text="Courtesy Call Done" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="div_br" style="display: none;" runat="server">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblHandledEmp" runat="server" Text="<%$ Resources:Attendance,Handled Employee %>" />
                                                        <asp:TextBox ID="txtHandledEmp" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtHandledEmp_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListRefTo" ServicePath=""
                                                            TargetControlID="txtHandledEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label8" runat="server" Text="Status"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" Enabled="false">
                                                            <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                            <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblTotalCharges" runat="server" Text="Total Charges"></asp:Label>
                                                        <asp:TextBox runat="server" ID="txtTotalCharges" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>


                                                    <div id="trticket" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Ticket No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtticketno" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtticketno_OnTextChanged" />
                                                        <asp:HiddenField ID="hdnTicketid" runat="server" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListTicketNo" ServicePath=""
                                                            TargetControlID="txtticketno" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Label ID="lnkticketdesc" runat="server" Text="<%$ Resources:Attendance,Detail%>"
                                                            Font-Underline="true" Visible="false" ForeColor="Blue"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trTicketDetail" runat="server">
                                                        <div class="col-md-12">
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
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblTiDate" runat="server" Text="<%$ Resources:Attendance,Ticket Date %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblTickeDate" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblCustomerNameValue" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblCallType" runat="server" Text="<%$ Resources:Attendance,Task Type %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblTaskType" runat="server">
                                                                                    </asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Status%>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Schedule Date %>"
                                                                                        Font-Bold="true"></asp:Label>

                                                                                    &nbsp:&nbsp<asp:Label ID="lblScheduledate" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblDesription" runat="server" Text="<%$ Resources:Attendance,Description %>"
                                                                                        Font-Bold="true" />
                                                                                    &nbsp:&nbsp<asp:Label ID="lblDescriptionvalue" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="box-footer"></div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesc" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme" Width="100%">
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Item Detail"><ContentTemplate><asp:UpdatePanel ID="Update_Item_Detail" runat="server"><ContentTemplate><div class="row"><div class="col-md-6"><asp:Label ID="Label14" runat="server" Text="Serial No"></asp:Label><asp:TextBox ID="txtSerialNo" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSerialNo_OnTextChanged"></asp:TextBox><br /></div><div class="col-md-6"><asp:HiddenField ID="hdnEmpEmailId" runat="server" /><asp:Label ID="Label15" runat="server" Text="Assigned To"></asp:Label><asp:TextBox ID="txtAssignedTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                        OnTextChanged="txtHandledEmp_TextChanged" AutoPostBack="true" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListRefTo" ServicePath=""
                                                                                        TargetControlID="txtAssignedTo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><br /></div><div class="col-md-6"><asp:Label ID="lblinvoice" runat="server" Text="Invoice No."></asp:Label><asp:TextBox ID="txtsalesinvoice" runat="server" CssClass="form-control" OnTextChanged="txtsalesinvoice_OnTextChanged"
                                                                                        AutoPostBack="true" BackColor="#eeeeee"></asp:TextBox><cc1:AutoCompleteExtender ID="AutoCompleteExtende_SalesInvoice" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListSalesInvoiceNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtsalesinvoice"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><cc1:AutoCompleteExtender ID="AutoCompleteExtender_PurchaseInvoice" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="false" MinimumPrefixLength="1" ServiceMethod="GetCompletionListPurchaseInvoiceNo"
                                                                                        ServicePath="" TargetControlID="txtsalesinvoice" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><br /></div><div class="col-md-6"><asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label><asp:TextBox ID="txtInvoicedate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox><br /></div><div class="col-md-6"><asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Product Id%>" /><asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control" AutoPostBack="True" TabIndex="17"
                                                                                        OnTextChanged="txtItemCode_TextChanged" BackColor="#eeeeee" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListItemCode"
                                                                                        ServicePath="" TargetControlID="txtItemCode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><br /></div><div class="col-md-6"><br /><asp:Button ID="btnRepairHistory" runat="server" Text="Repair History" CssClass="btn btn-primary" OnClick="btnRepairHistory_Click" /></div><div class="col-md-12"><asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Product Name %>" /><asp:TextBox ID="txtItemname" runat="server" CssClass="form-control" AutoPostBack="True" TabIndex="18"
                                                                                        OnTextChanged="txtItemCode_TextChanged" BackColor="#eeeeee" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListItemName"
                                                                                        ServicePath="" TargetControlID="txtItemname" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_OnSelectedIndexChanged"></asp:DropDownList><asp:HiddenField ID="hdnItemId" runat="server" /><asp:HiddenField ID="hdnInvoiceId" runat="server" /><br /></div><div class="col-md-6"><asp:Label ID="Label17" runat="server" Text="Quantity"></asp:Label><asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox><br /></div><div class="col-md-6"><asp:Label ID="Label18" runat="server" Text="Status"></asp:Label><asp:DropDownList ID="ddlIemStatus" runat="server" CssClass="form-control"><asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem><asp:ListItem Text="Waiting For Parts" Value="Waiting For Parts"></asp:ListItem><asp:ListItem Text="Not Repairable" Value="Not Repairable"></asp:ListItem><asp:ListItem Text="Complete" Value="Complete"></asp:ListItem><asp:ListItem Text="Send For RMA" Value="Send For RMA"></asp:ListItem></asp:DropDownList><br /></div><div class="col-md-6"><asp:Label ID="Label20" runat="server" Text="Problem"></asp:Label><asp:TextBox ID="txtItemProblem" runat="server" CssClass="form-control" TextMode="MultiLine" /><br /></div><div class="col-md-6"><asp:Label ID="lblAction" runat="server" Text="Action"></asp:Label><asp:TextBox ID="txtAction" runat="server" CssClass="form-control" TextMode="MultiLine" /><br /></div><div class="col-md-6"><asp:Label ID="Label19" runat="server" Text="Delivery Date"></asp:Label><asp:TextBox ID="txtdelivertydate" runat="server" CssClass="form-control" /><cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtdelivertydate" runat="server" TargetControlID="txtdelivertydate"></cc1:CalendarExtender><br /></div><div class="col-md-6"><asp:Label ID="Label24" runat="server" Text="Warranty"></asp:Label><asp:DropDownList ID="ddlWarranty" runat="server" CssClass="form-control" Enabled="false"><asp:ListItem Text="No" Value="No"></asp:ListItem><asp:ListItem Text="Yes" Value="Yes"></asp:ListItem></asp:DropDownList><br /></div><div class="col-md-12" style="text-align: center"><asp:Button ID="btnItemSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                        CssClass="btn btn-primary" OnClick="btnItemSave_Click" /><asp:Button ID="btnItemCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                        CausesValidation="False" OnClick="btnItemCancel_Click" /><asp:HiddenField ID="hdnItemEditId" runat="server" Value="0" /><br /></div><div class="col-md-12"><br /></div><div class="col-md-12" style="overflow: auto; max-height: 500px"><asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvItemDetail" runat="server" Width="100%" AutoGenerateColumns="False"><Columns><asp:TemplateField><ItemTemplate><asp:LinkButton ID="imgBtnItemhistory" runat="server" CommandArgument='<%#Eval("ProductId") %>' CommandName='<%#SuggestedProductName(Eval("ProductId").ToString()) %>' ToolTip="History" OnCommand="imgBtnItemhistory_Command"><i class="fa fa-history"></i></asp:LinkButton></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="5%" /></asp:TemplateField><asp:TemplateField><ItemTemplate><asp:LinkButton ID="imgBtnItemEdit" runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnItemEdit_Command" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="5%" /></asp:TemplateField><asp:TemplateField><ItemTemplate><asp:LinkButton ID="imgBtnItemDelete" runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnItemDelete_Command" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="5%" /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>"><ItemTemplate><asp:Label ID="lblSNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' /><asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("ProductSerialNo") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="5%" /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>"><ItemTemplate><asp:Label ID="lblgvProduct" runat="server" Text='<%#ProductCode(Eval("ProductId").ToString())%>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>"><ItemTemplate><asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("ProductId") %>' Visible="false" /><asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("ProductId").ToString()) %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>"><ItemTemplate><asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("ProductId").ToString()) %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Quantity"><ItemTemplate><asp:Label ID="lblgvRequiredQty" runat="server" Text='<%#Eval("qty") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Invoice No"><ItemTemplate><asp:Label ID="lblgvInvoiceNo" runat="server" Text='<%#Eval("Invoice_No") %>' /><asp:Label ID="lblgvInvoiceId" runat="server" Text='<%#Eval("Invoice_Id") %>' Visible="false" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Invoice Date"><ItemTemplate><asp:Label ID="lblgvInvoiceDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString()) %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Assigned To"><ItemTemplate><asp:Label ID="lblgvAssignedto" runat="server" Text='<%#GetAssignedPersonName(Eval("Responsible_Person").ToString()) %>' /><asp:Label ID="lblgvAssignedtoId" runat="server" Text='<%#Eval("Responsible_Person") %>' Visible="false" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Status"><ItemTemplate><asp:Label ID="lblgvStatus" runat="server" Text='<%#Eval("Status") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Delivery date"><ItemTemplate><asp:Label ID="lblDeliverydate" runat="server" Text='<%#Eval("DeliveryDate") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Problem"><ItemTemplate><asp:Label ID="lblgvProblem" runat="server" Text='<%#Eval("Problem") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField><asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Label ID="lblgvAction" runat="server" Text='<%#Eval("Field3") %>' /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField></Columns><PagerStyle CssClass="pagination-ys" /></asp:GridView><br /></div></div></ContentTemplate></asp:UpdatePanel><asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Item_Detail"><ProgressTemplate><div class="modal_Progress"><div class="center_Progress"><img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" /></div></div></ProgressTemplate></asp:UpdateProgress></ContentTemplate></cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Parts & Tools"><ContentTemplate><asp:UpdatePanel ID="Update_Parts_Tools" runat="server"><ContentTemplate><div class="row"><div class="col-md-6"><asp:Label ID="Label25" runat="server" Text="Ref Product Name" /><asp:DropDownList ID="ddlrefProductName" runat="server" CssClass="form-control"></asp:DropDownList><br /></div><div class="col-md-12"></div><div class="col-md-6"><asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Product Id%>" /><a style="color: Red">*</a> <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Add_Product_part"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator><asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True" TabIndex="17"
                                                                                        OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                        ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><br /><asp:HiddenField ID="hdnItemType" runat="server" /></div><div class="col-md-12"><asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" /><a style="color: Red">*</a> <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Add_Product_part"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="<%$ Resources:Attendance,Enter Product Name%>"></asp:RequiredFieldValidator><asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                        OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" /><cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                                        ServicePath="" TargetControlID="txtProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender><br /></div><div class="col-md-6"><asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" /><a style="color: Red">*</a> <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Add_Product_part" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" /><asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" TabIndex="19" /><asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" Visible="False" /><br /></div><div class="col-md-6"><asp:Label ID="lblRequestQty" runat="server" Text="<%$ Resources:Attendance,Request Quantity %>" /><a style="color: Red">*</a> <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Add_Product_part"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequestQty" ErrorMessage="<%$ Resources:Attendance,Enter Request Quantity%>"></asp:RequiredFieldValidator><asp:TextBox ID="txtRequestQty" runat="server" CssClass="form-control" Text="1" TabIndex="20" /><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                        TargetControlID="txtRequestQty" ValidChars="1,2,3,4,5,6,7,8,9,0,."></cc1:FilteredTextBoxExtender><br /></div><div class="col-md-6"><asp:Label ID="Label27" runat="server" Text="Unit Price" /><a style="color: Red">*</a> <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Add_Product_part"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUnitPrice" ErrorMessage="<%$ Resources:Attendance,Enter Unit Price%>"></asp:RequiredFieldValidator><asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" Text="1" TabIndex="20" /><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3txtUnitPrice" runat="server" Enabled="True"
                                                                                        TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,."></cc1:FilteredTextBoxExtender><br /></div><div class="col-md-6"><asp:Label ID="lblCharges" runat="server" Text="Charges Details" /><asp:DropDownList runat="server" ID="ddlCharges" CssClass="form-control"><asp:ListItem Text="Labour" Value="Labour"></asp:ListItem><asp:ListItem Text="Other" Value="Other"></asp:ListItem></asp:DropDownList><br /></div><div class="col-md-12" style="text-align: center"><asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                        CssClass="btn btn-primary" OnClick="btnProductSave_Click" ValidationGroup="Add_Product_part" TabIndex="23" /><asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                        CausesValidation="False" OnClick="btnProductCancel_Click" TabIndex="24" /><asp:HiddenField ID="hdnProductId" runat="server" /><br /></div><div class="col-md-12"><br /></div><div class="col-md-12" style="overflow: auto; max-height: 500px;"><asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductRequest" runat="server" AutoGenerateColumns="False" Width="100%"><Columns><asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>"><ItemTemplate><asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id").ToString() %>' Visible="false"></asp:Label><asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                        ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>" Style="height: 14px"
                                                                                                        OnCommand="IbtnDelete_Command1" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>"><ItemTemplate><asp:Label ID="lblSerialNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'
                                                                                                        Visible="false"></asp:Label><asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' /></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="Ref Product Code"><ItemTemplate><asp:Label ID="lblrefproductcode" runat="server" Text='<%# ProductCode(Eval("Ref_Product_Id").ToString()) %>'></asp:Label><asp:Label ID="lblrefproductId" runat="server" Text='<%# Eval("Ref_Product_Id").ToString() %>' Visible="false"></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>"><ItemTemplate><asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>"><ItemTemplate><asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label><asp:Label ID="lblProductName" runat="server" Text='<%# SuggestedProductName(Eval("ProductId").ToString()) %>'></asp:Label><asp:HiddenField ID="hdncharge" runat="server" Value='<%# Eval("Field1") %>'></asp:HiddenField></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>"><ItemTemplate><asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("Unit") %>'></asp:Label><asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("Unit").ToString()) %>'></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>"><ItemTemplate><asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Quantity").ToString()) %>'></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>"><ItemTemplate><asp:Label ID="lblUnitPrice" runat="server" Text='<%# SetDecimal(Eval("Unit_Price").ToString()) %>'></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField><asp:TemplateField HeaderText="Line Total"><ItemTemplate><asp:Label ID="lblLineTotal" runat="server" Text='<%# getLineTotal(Eval("Quantity").ToString(),Eval("Unit_Price").ToString()) %>'></asp:Label></ItemTemplate><ItemStyle /></asp:TemplateField></Columns><PagerStyle CssClass="pagination-ys" /></asp:GridView><br /></div><div class="col-md-12" style="float: right; padding-left: 76%;"><asp:Label ID="lblLabourCharges" runat="server"></asp:Label><br />&#160;&#160;<asp:Label ID="lblOtherCharges" runat="server"></asp:Label></div><div class="col-md-12" style="float: right; padding-left: 80%;"><asp:Label ID="lblTotal" runat="server"></asp:Label></div></div></ContentTemplate></asp:UpdatePanel><asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Parts_Tools"><ProgressTemplate><div class="modal_Progress"><div class="center_Progress"><img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" /></div></div></ProgressTemplate></asp:UpdateProgress></ContentTemplate></cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Terms & Condition"><ContentTemplate><asp:UpdatePanel ID="Update_Terms_Condition" runat="server"><ContentTemplate><div class="row"><div class="col-md-12" class="flow"><cc1:Editor ID="txtTerms" CssClass="form-control" Height="400px" runat="server" /><br /></div></div></ContentTemplate></asp:UpdatePanel><asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Terms_Condition"><ProgressTemplate><div class="modal_Progress"><div class="center_Progress"><img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" /></div></div></ProgressTemplate></asp:UpdateProgress></ContentTemplate></cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnInquirySave_Click" Visible="false" />

                                                        <asp:Button ID="btnClose" runat="server" Text="Close"
                                                            CssClass="btn btn-primary" OnClick="btnInquirySave_Click" Visible="false" />

                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to close job card ?"
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
    <div class="modal fade" id="RepairHistory" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-header">

                    <h4>Repair History:</h4>
                    <asp:UpdatePanel ID="upitemName" runat="server">
                        <ContentTemplate>
                            <asp:Label Font-Bold="true" ID="itemHistory" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                <div class="modal-body">
                    <asp:UpdatePanel ID="upRepairHistory" runat="server">
                        <ContentTemplate>
                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRepairHistory" runat="server" AutoGenerateColumns="False" Width="100%">

                                <Columns>

                                    <asp:TemplateField HeaderText="Product Serial No">
                                        <ItemTemplate>
                                            <asp:Label ID="gvProductSerialNo" runat="server" Text='<%# Eval("ProductSerialNo").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Job No">
                                        <ItemTemplate>
                                            <asp:Label ID="gvJobNo" runat="server" Text='<%# Eval("Job_no").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Job Date">
                                        <ItemTemplate>
                                            <asp:Label ID="gvJob_date" runat="server" Text='<%# GetDate(Eval("Job_date").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Job End Date">
                                        <ItemTemplate>
                                            <asp:Label ID="gvJobEndDate" runat="server" Text='<%# GetDate(Eval("Job_End_Date").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Problem">
                                        <ItemTemplate>
                                            <asp:Label ID="gvProblem" runat="server" Text='<%# Eval("Problem") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Solution">
                                        <ItemTemplate>
                                            <asp:Label ID="gvSolution" runat="server" Text='<%# Eval("Solution") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Service Engineer">
                                        <ItemTemplate>
                                            <asp:Label ID="gvServiceEngineer" runat="server" Text='<%# Eval("service_engineer") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>

                                </Columns>

                                <PagerStyle CssClass="pagination-ys" />

                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upRepairHistory">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


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
        function Modal_repairHistory() {
            document.getElementById('<%= Btn_RepairHistory.ClientID %>').click();
        }
        function getDocNo(ctrl) {
            var txtBox = document.getElementById('<%= txtJobNo.ClientID %>');
            getDocumentNoByModuleNObjectId(ctrl, txtBox, '158', '348');
        }
        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
            setReportData();
        }
    </script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
</asp:Content>
