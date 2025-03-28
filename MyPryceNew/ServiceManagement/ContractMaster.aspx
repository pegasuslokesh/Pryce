<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ContractMaster.aspx.cs" Inherits="ServiceManagement_ContractMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-contract"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Contract Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Contract Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_GeneratedContracts" Style="display: none;" data-toggle="modal" data-target="#GeneratedContracts" runat="server" Text="Generated Contracts Data" />
            <asp:Button ID="Btn_ScheduleDetails" Style="display: none;" data-toggle="modal" data-target="#ScheduleDetails" runat="server" Text="Schedule Details" />
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
                                            <asp:Label ID="lblDeviceParameter" runat="server" Text="Advance Search"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
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
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Running" Value="Running" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                    <asp:ListItem Text="Pending for Invoice" Value="PendingInvoiceCount"></asp:ListItem>
                                                    <asp:ListItem Text="Due Amt" Value="DueAmt"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvContractMaster.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>

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
                                            <asp:HiddenField ID="hdnIsRenewal" runat="server" />
                                            <asp:HiddenField ID="hdnRenewalParentTransId" runat="server" />
                                            <asp:HiddenField ID="hdnRenewalImmidiateTransId" runat="server" />
                                            <asp:HiddenField ID="hdnImmidiateContractEndDate" runat="server" />
                                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvContractMaster"></dx:ASPxGridViewExporter>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdntransID" runat="server" />
                                                    <asp:HiddenField ID="hdnparentId" runat="server" />

                                                    <dx:ASPxGridView ID="GvContractMaster" EnableViewState="false" ClientInstanceName="grid" OnCustomButtonCallback="grid_CustomButtonCallback" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id,Field5">
                                                        <Columns>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' TabIndex="9" ToolTip="<%$ Resources:Attendance,View %>" OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" ToolTip='<%# ChangeToolTip(Eval("Remaining_Days").ToString()) %>' runat="server" CommandName='<%# Eval("Location_Id") %>' CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" ToolTip="<%$ Resources:Attendance,Delete %>" runat="server" CommandName='<%# Eval("Location_Id") %>' CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnFileUpload" ToolTip="File-Upload" runat="server" CausesValidation="False" Height="25px" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Field5") %>' OnCommand="IbtnFileUpload_Command"><i class="fa fa-upload" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataColumn VisibleIndex="5">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lblAllContractHistory" ToolTip="Renewal History" runat="server" CommandName='<%#Eval("Field5")%>' CommandArgument='<%#Eval("Trans_Id") %>' Text="Renewal" OnCommand="lblAllContractHistory_Command" /><br />
                                                                    
                                                                    <%--<a onclick="VisitHistory(<%#Eval("Contract_No") %>)" style="cursor: pointer" title="Visit History">Visit</a>--%>
                                                                    <asp:LinkButton ID="lblVisitHistory" ToolTip="Visit History" runat="server" CommandArgument='<%#Eval("Contract_No") %>'  Text="Visit" OnCommand="lblVisitHistory_Command" /><br />
                                                                    <asp:LinkButton ID="lblCustomerStatement" ToolTip="Customer Statement" runat="server" CommandArgument='<%#Eval("Customer_Id") %>' Text="Customer Statement" OnCommand="lblCustomerStatement_Command" />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Contract_No" Settings-AutoFilterCondition="Contains" Caption="Contract No" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Field3" Settings-AutoFilterCondition="Contains" Caption="Contract Name" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataDateColumn Caption="Contract Date" FieldName="Contract_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="8" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataDateColumn Caption="Start Date" FieldName="Start_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="9" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataDateColumn Caption="End Date" FieldName="End_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="10" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Remaining_Days" Caption="Remaining Days" VisibleIndex="11">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblgvRemDay" runat="server" Text='<%# removeNegativeNo(Eval("Remaining_Days").ToString())  %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Customer Name %>" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Field2" Settings-AutoFilterCondition="Contains" Caption="Description" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="14" Caption="Sales Order No">
                                                                <DataItemTemplate>
                                                                    <a onclick="openSalesOrder('<%#Eval("SalesOrderNo") %>')" style="cursor: pointer"><%#Eval("SalesOrderNo") %></a>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("SalesOrderNo") %>' OnCommand="lblorder_Command" Visible="false" Text='<%#Eval("SalesOrderNo") %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn Caption="Contract Amt" VisibleIndex="15" FieldName="Contract_Amount">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="gvlblamt" runat="server" Text='<%#SetDecimal(Eval("Contract_Amount").ToString()) %>'></asp:Label>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataColumn VisibleIndex="16" Caption="Invoice Amt" FieldName="invoice_amount">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lblInvoice_Amount" CommandName='<%#Eval("CustomerName") +"/"+ Eval("Customer_Id") %>' runat="server" Text='<%#SetDecimal( Eval("Invoice_Amount").ToString()) %>' OnCommand="lblInvoice_Amount_Command" CommandArgument='<%#Eval("Trans_Id")+"/"+Eval("Contract_No") %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="balance" VisibleIndex="17" Caption="Due Amt">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblbalance" runat="server" Text='<%#SetDecimal(Eval("balance").ToString()) %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>

                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="Contract_Amount" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="invoice_amount" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="balance" SummaryType="Sum" />
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
                                                    <asp:HiddenField ID="hdnCustomerName" runat="server" />
                                                    <asp:HiddenField ID="hdnContractNo" runat="server" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-md">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                                    </h4>
                                </div>
                                <div class="modal-body">
                                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="ScheduleDetails" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="upScheduleData" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvScheduledDataEdit" runat="server" AutoGenerateColumns="False" Width="100%">

                                                <Columns>

                                                    <asp:TemplateField HeaderText="Scheduled Date">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnScheduledTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                            <asp:Label ID="gvScheduledDateEdit" runat="server" Text='<%# GetDate(Eval("Schedule_Date").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Invoice No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="gvinvoiceNoEdit" runat="server" Text='<%# Eval("Invoice_No") %>' CommandArgument='<%# Eval("Invoice_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="gvlbtnInvoiceNo_Command"></asp:LinkButton>
                                                            <%--<asp:TextBox ID="gvinvoiceNoEdit" runat="server" Text='<%# Eval("Invoice_No") %>' OnTextChanged="gvinvoiceNoEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="gvinvoiceNoEdit"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>--%>
                                                            <asp:HiddenField ID="gvHdnInvoiceIDEdit" runat="server" Value='<%# Eval("Invoice_Id") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Invoice Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvinvoiceDtEdit" runat="server" Text='<%# GetDate(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Net Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvInvoiceAmtEdit" runat="server" Text='<%# SetDecimal(Eval("InvoiceAmt").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvDueAmtEdit" runat="server" Text='<%# SetDecimal(Eval("DueAmt").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="col-md-12">
                                        <br />
                                    </div>

                                    <div class="col-md-5" style="display: none;">
                                        <asp:Button ID="btnsaveScheduleDtls" Visible="false" CssClass="btn btn-primary" runat="server" Text="Save" />
                                    </div>

                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>


                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upScheduleData">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upData">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>


                    <div class="modal fade" id="GeneratedContracts" tabindex="-1" role="dialog" aria-labelledby="GeneratedContracts_ModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-mg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title" id="AllGeneratedContracts">Generated Contract List:</h4>
                                </div>
                                <div class="modal-body">



                                    <asp:UpdatePanel ID="upData" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvContractData" Width="100%" runat="server" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Contract No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("Contract_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Contract Date">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# GetDate( Eval("Contract_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Start Date">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# GetDate(Eval("Start_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="End Date">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# GetDate( Eval("End_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Sales Order No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Contract Amt">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#SetDecimal( Eval("Contract_Amount").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Invoice Amt">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#SetDecimal( Eval("invoice_amount").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Due Amt">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# SetDecimal( Eval("balance").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                </Columns>


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

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label9" runat="server" Text="Location"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlLoc" CssClass="form-control" onchange="getDocNo(this)"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Contract No. %>"></asp:Label>
                                                        <asp:TextBox ID="txtContractNo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance, Contract Date%>"></asp:Label>
                                                        <asp:TextBox ID="txtContractDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtContractDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlContractName" runat="server">
                                                        <asp:Label ID="lblContractName" runat="server" Text="Contract Name"></asp:Label>
                                                        <asp:TextBox ID="txtContractName" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlOldContractNo" runat="server">
                                                        <asp:Label ID="lbloldContractDate" runat="server" Text="<%$ Resources:Attendance,Old Contract No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtOldContractNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtECustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtECustomer" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtECustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblStartdate" runat="server" Text="<%$ Resources:Attendance,Start Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtStartdate" ErrorMessage="<%$ Resources:Attendance,Enter Start Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtStartdate" runat="server" CssClass="form-control" OnTextChanged="txtStartdate_TextChanged" AutoPostBack="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="calenderStartdate" runat="server" TargetControlID="txtStartdate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblEndDate" runat="server" Text="<%$ Resources:Attendance,End Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEnddate" ErrorMessage="<%$ Resources:Attendance,Enter End Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtEnddate" runat="server" CssClass="form-control" OnTextChanged="txtEnddate_TextChanged" AutoPostBack="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="calenderenddate" runat="server" TargetControlID="txtEnddate" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblContractAmt" runat="server" Text="Contract Amount"></asp:Label>
                                                        <asp:TextBox ID="txtContractAmt" runat="server" CssClass="form-control" MaxLength="8" OnTextChanged="txtContractAmt_TextChanged" AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtContractAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCost" runat="server" Text="Cost"></asp:Label>
                                                        <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" MaxLength="8" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtCost" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="ctlSalesOrderId" runat="server">
                                                        <asp:HiddenField ID="hdnSalesOrderID" runat="server" />
                                                        <asp:Label ID="lblSalesOrderID" runat="server" Text="Sales Order ID"></asp:Label>
                                                        <asp:TextBox ID="txtSalesOrderID" runat="server" CssClass="form-control" OnTextChanged="txtSalesOrderID_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListOrderNo" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSalesOrderID"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%">

                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvsNo" Width="30px" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                    <ItemTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td width="90%">

                                                                                    <asp:Label ID="lblgvProductName" Width="90%" runat="server" Text='<%#Eval("EProductName") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <%--                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                            <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                            <tr>
                                                                                                <td height="21" colspan="2">
                                                                                                    <div align="center" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                        <asp:Label ID="lblDetail1" runat="server" Text="<%$ Resources:Attendance,Details %>"></asp:Label>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="background-color: whitesmoke;">
                                                                                                <td colspan="2" align="left" valign="top">
                                                                                                    <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                        <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                                                    </asp:Panel>
                                                                                                    <br />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%#SetDecimal(Eval("UnitPrice").ToString()) %>'></asp:Label>

                                                                        <%--<asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblqty" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>


                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscountValue" runat="server" Text='<%# SetDecimal(Eval("DiscountV").ToString()) %>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLineTotal" runat="server" Text='<%#SetDecimal(Eval("NetTotal").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div id="Div_Box_Add1" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="lblAddContact" runat="server" Text="Payment Scheduled Details"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_Add_Div1" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">

                                                                    <div id="div_insert" runat="server">
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblScheduler" runat="server" Text="Operation Type" />
                                                                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged" AutoPostBack="true" />
                                                                            <br />
                                                                        </div>
                                                                    </div>

                                                                    <div id="div_edit" style="display: none" runat="server">
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblScheduledDate" runat="server" Text="Scheduled Date" />
                                                                            <asp:TextBox ID="txtScheduledDate" runat="server" CssClass="form-control" />
                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtScheduledDate" Format="dd-MMM-yyyy" />
                                                                            <br />

                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblInvoiceNum" runat="server" Text="Invoice No" />
                                                                            <asp:TextBox ID="txtInvoiceNum" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtInvoiceNo_OnTextChanged" AutoPostBack="true" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceNum"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>

                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <br />
                                                                            <asp:Button ID="btnAddDtls" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddDtls_Click" />
                                                                            <br />
                                                                        </div>

                                                                    </div>



                                                                    <div class="col-md-12" style="overflow: auto; max-height: 250px">
                                                                        <div class="progress-group">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvScheduledData" runat="server" OnRowDeleting="GvScheduledData_RowDeleting" AutoGenerateColumns="False" Width="100%">

                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Delete">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="gvTrans_Id" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Trans_Id") %>' ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Scheduled Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="gvScheduledDate" runat="server" Text='<%# GetDate(Eval("Schedule_Date").ToString()) %>' OnTextChanged="gvScheduledDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="gvScheduledDate" Format="dd-MMM-yyyy" />

                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Invoice No">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtInvNo" runat="server" OnTextChanged="txtInvNo_TextChanged" AutoPostBack="true" Text='<%# Eval("Invoice_No") %>'></asp:TextBox>
                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                                Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvNo"
                                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                            </cc1:AutoCompleteExtender>
                                                                                            <asp:HiddenField ID="hdnLoc_Id" runat="server" Value='<%# Eval("Location_Id") %>' />
                                                                                            <asp:HiddenField ID="gvHdnInvoiceID" runat="server" Value='<%# Eval("Invoice_Id") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Invoice Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvinvoiceDt" runat="server" Text='<%# GetDate(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Net Amount">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvInvoiceAmt" runat="server" Text='<%# SetDecimal(Eval("InvoiceAmt").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Due Balance">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvDueAmt" runat="server" Text='<%# SetDecimal(Eval("DueAmt").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                </Columns>

                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6" style="display: none;">
                                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtInvoiceNo_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceNo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnInvoiceId" runat="server" Value="0" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" style="display: none;">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="display: none;">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Gross Amount%>"></asp:Label>
                                                        <asp:TextBox ID="txtgrossAmount" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="display: none;">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Discount %>" />
                                                        <asp:TextBox ID="txtDiscountAmount" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="display: none;">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Net Amount%>"></asp:Label>
                                                        <asp:TextBox ID="txtNetAmount" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-12" id="ctlDesc" runat="server">
                                                        <asp:Label ID="Label11" runat="server" Text="Description"></asp:Label>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesc" runat="server" Text="Terms & Condition" Font-Bold="true"></asp:Label>
                                                        <cc1:Editor ID="txtTermsandconditon" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnSave_Click" Visible="false" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnCancel_Click" />
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contract No. %>" Value="Contract_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contract Date %>" Value="Contract_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDateBin" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtValueDateBin" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                        Visible="false" runat="server" OnClick="imgBtnRestore_Click"
                                                        ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                   
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContractMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvContractMasterBin_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvContractMasterBin_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract No.%>" SortExpression="Contract_No">


                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false" />
                                                                    <asp:Label ID="lblContractNo" runat="server" Text='<%#Eval("Contract_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract Date%>" SortExpression="Contract_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractDate" runat="server" Text='<%#GetDate(Eval("Contract_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start date%>" SortExpression="Start_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStartdate" runat="server" Text='<%#GetDate(Eval("Start_Date").ToString()) %>' />

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,End date%>" SortExpression="End_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEnddate" runat="server" Text='<%#GetDate(Eval("End_Date").ToString()) %>' />

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Invoice No. %>" SortExpression="InvoiceNumber">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNumber") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Net Amount %>" SortExpression="Net_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNetamount" runat="server" Text='<%#Eval("Net_Amount") %>' />
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        <%--function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }--%>
        function FUAll_UploadStarted(sender, args) {

        }

        function openSalesOrder(id) {
            window.open("../Sales/SalesOrder1.aspx?OrderNo=" + id);
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }

        function Modal_Generated_Contracts(data) {
            document.getElementById('<%= Btn_GeneratedContracts.ClientID %>').click();
        }

        function Modal_ScheduledDetails() {
            document.getElementById('<%= Btn_ScheduleDetails.ClientID %>').click();
        }


        function VisitHistory(Contract_No) {
            window.open("../ServiceManagement/WorkOrder.aspx?SearchField=" + Contract_No);
        }


        function openSalesInvoice(id, locId) {
            window.open("../Sales/SalesInvoice.aspx?Id=" + id + "&LocId=" + locId);
        }
        function openCustomerStatement(id) {
            window.open("../CustomerReceivable/CustomerStatement.aspx?Id=" + id);
        }
        function getDocNo(ctrl) {
            var txtBox = document.getElementById('<%= txtContractNo.ClientID %>');
            getDocumentNo(ctrl, txtBox);
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
</asp:Content>
