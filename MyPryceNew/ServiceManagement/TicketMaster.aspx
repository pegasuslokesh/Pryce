<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TicketMaster.aspx.cs" Inherits="ServiceManagement_TicketMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-ticket-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Ticket Master%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Ticket Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="txtconformmessageValue" runat="server" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Call_Logs" Style="display: none;" runat="server" OnClick="btnPRequest_Click" Text="Call Logs" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_TicketHistory" Style="display: none;" runat="server" data-toggle="modal" data-target="#TicketHistory" Text="Ticket History" />

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
                    <li id="Li_Call_Logs"><a href="#Call_Logs" onclick="Li_Tab_Call_Logs()" data-toggle="tab">
                        <i class="fa fa-phone"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Call Logs %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;
					

					<div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                            <i id="I3" runat="server" class="fa fa-plus"></i>
                        </button>
                    </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-6">
                                                    <br />
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblstatusFilter" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlStatusFilter_Click" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All%>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Hold%>" Value="Hold"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvTicketMaster.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>
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
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvTicketMaster"></dx:ASPxGridViewExporter>

                                                    <dx:ASPxGridView ID="GvTicketMaster" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id">
                                                        <Columns>
                                                            <dx:GridViewDataColumn VisibleIndex="0" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        TabIndex="9" ToolTip="<%$ Resources:Attendance,View %>"
                                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>

                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" TabIndex="10"
                                                                        OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>'
                                                                        TabIndex="11" OnCommand="IbtnDelete_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="ImgDownload" runat="server"
                                                                        CommandArgument='<%#Eval("Trans_Id") %>' CommandName='<%#Eval("Field1") %>' OnCommand="OnDownloadCommand"
                                                                        ToolTip='<%#Eval("Field1") %>'><i class="fas fa-download" style="font-size:15px"></i></asp:LinkButton>
                                                                    <asp:HiddenField ID="hndFileName" runat="server" Value='<%#Eval("Field1") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="imgFeedback" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        TabIndex="11" OnCommand="imgFeedback_OnCommand"
                                                                        ToolTip="<%$ Resources:Attendance,Ticket FeedBack %>"><i class="fa fa-phone" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn VisibleIndex="5">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkFeedbackDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        TabIndex="9"
                                                                        ToolTip="<%$ Resources:Attendance,Ticket FeedBack %>" OnCommand="lnkFeedbackDetail_Command"
                                                                        CausesValidation="False"><i class="fa fa-phone" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Ticket_No" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Ticket No. %>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Call_No" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Call No. %>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataDateColumn Caption="<%$ Resources:Attendance,Schedule Date %>" FieldName="Schedule_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="7" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Chargeable_Amount" Caption="<%$ Resources:Attendance,Chargeable Amount %>" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Customer Name %>" VisibleIndex="9">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Status" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Status %>" VisibleIndex="10">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Field2" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Invoice No %>" VisibleIndex="11">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="invoiceAmt" Caption="Invoice Amt" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>


                                                        </Columns>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="Chargeable_Amount" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="invoiceAmt" SummaryType="Sum" />
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
                                <div id="pnlFeedbacKdetail" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label9" runat="server" Text="Ticket Feedback" Font-Size="20px"></asp:Label></h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblFeedbackDate" runat="server" Text="<%$ Resources:Attendance,Feedback Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtFeedbackDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFeedbackDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Action %>"></asp:Label>
                                                        <asp:TextBox ID="txtAction" runat="server" TextMode="MultiLine"
                                                            TabIndex="42" CssClass="form-control" Font-Names="Arial" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlFeedbackStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,File Upload %>"></asp:Label>
                                                        <asp:FileUpload ID="UploadFeedbackFile" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkInvolveCustomer" runat="server" Text="Involve Customer" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkInvolveEmployee" runat="server" Text="Involve Employee" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnsaveFeedback" runat="server" Text="<%$ Resources:Attendance,Feedback %>"
                                                            TabIndex="45" CssClass="btn btn-primary" OnClick="btnsaveFeedback_Click" />

                                                        <asp:Button ID="btnFeedbackRefresh" runat="server" Text="<%$ Resources:Attendance,Refresh %>"
                                                            TabIndex="45" CssClass="btn btn-primary" OnClick="btnFeedbackRefresh_Click" />

                                                        <asp:Button ID="btnFeedbackCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            TabIndex="45" CssClass="btn btn-primary" OnClick="btnPICancel_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFeedback" runat="server" AutoGenerateColumns="false"
                                                            Width="100%" ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmployee" runat="server" Text='<%#Eval("Emp_Name")%>'
                                                                            Width="150px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Feedback Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFeedbackDate" runat="server" Text='<%#GetDate(Eval("Date").ToString()) %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAction" runat="server" Text='<%#Eval("Action")%>'
                                                                            Width="355px"></asp:Label>

                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Field1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Upload %>">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="ImgDownload" runat="server" CommandArgument='<%#Eval("Trans_Id") %>'
                                                                            CommandName='<%#Eval("Field2") %>' Text='<%#Eval("Field2") %>' OnCommand="gvFeedbackOnDownloadCommand"
                                                                            ForeColor="Blue" ToolTip="<%$ Resources:Attendance,Download %>"></asp:LinkButton>
                                                                        <%-- <asp:ImageButton ID="ImgDownload" runat="server" ImageUrl="~/Images/download.png" 
                                                                CommandArgument='<%#Eval("Trans_Id") %>' CommandName='<%#Eval("Field2") %>' Width="16px" OnCommand="gvFeedbackOnDownloadCommand"
                                                                ToolTip="<%$ Resources:Attendance,Download %>" />--%>
                                                                        <%--  <asp:Label ID="hdnFileName" runat="server" Text='<%#Eval("Field2") %>' />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlTicketDetail" runat="server" class="row">
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
                                                        <asp:Label ID="lblPINo" runat="server" Text="<%$ Resources:Attendance,Ticket No.%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtticketNo" ErrorMessage="<%$ Resources:Attendance,Enter Ticket No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtticketNo" runat="server" CssClass="form-control" TabIndex="14" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblTiDate" runat="server" Text="<%$ Resources:Attendance,Ticket Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTicketDate" ErrorMessage="<%$ Resources:Attendance,Enter Ticket Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtTicketDate" runat="server" CssClass="form-control" TabIndex="13" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtTicketDate" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlRefType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="By Call" Value="Call"></asp:ListItem>
                                                            <asp:ListItem Text="By Website" Value="Website"></asp:ListItem>
                                                            <asp:ListItem Text="By Email" Value="Email"></asp:ListItem>
                                                            <asp:ListItem Text="By Contract" Value="Contract"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Ref No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnCallId" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div id="trCallLogs" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblCIDate" runat="server" Text="<%$ Resources:Attendance, Call Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtCallDate" runat="server" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-11">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtECustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtECustomer" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtECustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-1">
                                                        <br />
                                                        <asp:LinkButton ID="btnHistory" runat="server" ToolTip="Customer History" OnCommand="btnHistory_Command"><i class="fas fa-history" style="font-size:25px"></i></asp:LinkButton>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCallType" runat="server" Text="<%$ Resources:Attendance,Task Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Sales Inquiry%>" Value="Sales Inquiry"
                                                                Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Service%>" Value="Service"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Job Cards%>" Value="Job Cards"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Installation and implementation%>" Value="Installation and implementation"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmailId" runat="server" Text="<%$ Resources:Attendance,Email ID %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmailId" ErrorMessage="<%$ Resources:Attendance,Enter Email ID%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div id="trCallLogsCallDetail" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Call Detail %>" />
                                                        <asp:TextBox ID="lblCalldetail" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="trCallLogsNotes" runat="server" visible="false">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Call Notes %>" />
                                                        <asp:TextBox ID="lblCallNotes" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Schedule Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtScheduleDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtExpectedEndDate_TextChanged" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtScheduleDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblExpectedEndDate" runat="server" Text="Expected End Date"></asp:Label><a style="color: red">*</a>
                                                        <asp:TextBox ID="txtExpectedEndDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtExpectedEndDate_TextChanged" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtExpectedEndDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                                            Enabled="false">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Cancel%>" Value="Cancel"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Hold%>" Value="Hold"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Chargeable Amount %>"></asp:Label>
                                                        <asp:TextBox ID="txtChargableAmount" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                            TargetControlID="txtChargableAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblEmp" runat="server" Text="Add Employee"
                                                                                Font-Bold="true"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gridView" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                    Width="100%" DataKeyNames="Employee_Id" ShowFooter="true" OnRowDeleting="gridView_RowDeleting"
                                                                                    OnRowCommand="gridView_RowCommand">

                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblempname" runat="server" Text='<%#Eval("EmpName").ToString()+"/"+Eval("Emp_Code").ToString() %>'></asp:Label>
                                                                                                <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%#Eval("Employee_Id") %>' />
                                                                                            </ItemTemplate>
                                                                                            <EditItemTemplate>
                                                                                                <asp:TextBox ID="txtempName" runat="server" Font-Names="Verdana" AutoPostBack="false"
                                                                                                    CssClass="form-control" BackColor="#eeeeee" CausesValidation="false"></asp:TextBox>

                                                                                                <a style="color: Red">*</a>
                                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtempName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>

                                                                                            </EditItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtEmpFooter" runat="server" Font-Names="Verdana" AutoPostBack="false"
                                                                                                    OnTextChanged="txtEmpFooter_TextChanged" CssClass="form-control"
                                                                                                    BackColor="#eeeeee" CausesValidation="true"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender ID="autoComplete12256660" runat="server" DelimiterCharacters=""
                                                                                                    Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpFooter"
                                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <EditItemTemplate>
                                                                                                <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true" CommandArgument='<%#Eval("Employee_Id") %>' />
                                                                                                <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-info" CommandName="Cancel" Text="Cancel" />
                                                                                            </EditItemTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Button ID="ButtonEdit" runat="server" CssClass="btn btn-info" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                                <asp:Button ID="ButtonDelete" runat="server" CssClass="btn btn-danger" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Employee_Id") %>' />
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Panel ID="pnlGridview" runat="server" DefaultButton="ButtonAdd">
                                                                                                    <asp:LinkButton ID="ButtonAdd" runat="server" CommandName="AddNew" ToolTip="Add New Row"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                                </asp:Panel>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>

                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                </asp:GridView>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblItem" runat="server" Text="Add Product"
                                                                                Font-Bold="true"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTools" ShowHeader="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                                    ShowFooter="true" OnRowDeleting="gvTools_RowDeleting" OnRowEditing="gvTools_RowEditing"
                                                                                    OnRowCancelingEdit="gvTools_OnRowCancelingEdit" OnRowCommand="gvTools_RowCommand">

                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Product Id" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblproductCode" runat="server" Text='<%#Eval("ProductCode") %>'></asp:Label>
                                                                                            </ItemTemplate>
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
                                                                                        <asp:TemplateField HeaderText="Product">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblproductName" runat="server" Text='<%#Eval("ProductName") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtERelatedProduct" runat="server" CssClass="form-control"
                                                                                                    BackColor="#eeeeee" OnTextChanged="txtToolsERelatedProduct_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListRelatedProductName"
                                                                                                    ServicePath="" TargetControlID="txtERelatedProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Problem %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblproductproblem" runat="server" Text='<%#Eval("Problem") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtproblem" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <EditItemTemplate>
                                                                                                <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                    CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                                <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-info" CommandName="Cancel" Text="Cancel" />
                                                                                            </EditItemTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Button ID="ButtonEdit" runat="server" CssClass="btn btn-info" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                                <asp:Button ID="ButtonDelete" runat="server" CssClass="btn btn-danger" Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'
                                                                                                    CommandName="Delete" />
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Panel ID="pnlGridviewTools" runat="server" DefaultButton="ButtonAdd">
                                                                                                    <asp:LinkButton ID="ButtonAdd" runat="server" CommandName="AddNew" ToolTip="Add New Row"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                                </asp:Panel>
                                                                                            </FooterTemplate>
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
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="display: none;">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lnkAddvisit" runat="server" Text="<%$ Resources:Attendance,Add Visits %>"
                                                                                Font-Bold="true"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Visit Date %>"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVisitDate" ErrorMessage="<%$ Resources:Attendance,Enter Visit Date%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control" />
                                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtVisitDate" runat="server" TargetControlID="txtVisitDate">
                                                                                </cc1:CalendarExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Visit Time %>"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVisitTime" ErrorMessage="<%$ Resources:Attendance,Enter Visit Time%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtVisitTime" runat="server" CssClass="form-control" />
                                                                                <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtVisitTime"
                                                                                    UserTimeFormat="TwentyFourHour">
                                                                                </cc1:MaskedEditExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Vehicle Name %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtvehiclename" ErrorMessage="<%$ Resources:Attendance,Enter Vehicle Name%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtvehiclename" runat="server" AutoPostBack="true"
                                                                                    OnTextChanged="txtvehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtvehiclename"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance, Driver Name %>"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtdrivername" ErrorMessage="<%$ Resources:Attendance,Enter Driver Name%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtdrivername" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                    OnTextChanged="txtEmpName_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                    TabIndex="5"></asp:TextBox>
                                                                                <cc1:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtdrivername"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <%--<asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                                    Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" Width="35px" ID="btnvisitsave"
                                                                                    OnClick="btnvisitsave_Click"></asp:ImageButton>--%>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:Button ID="btnvisitsave" runat="server" OnClick="btnvisitsave_Click" Text="<%$ Resources:Attendance,Add %>" CssClass="btn btn-info" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitMaster" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                    BorderStyle="Solid" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>

                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" Visible="true">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="IbtnDeleteVisit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                    ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>" Width="16px"
                                                                                                    OnCommand="IbtnDeleteVisit_Command" />
                                                                                                <%-- <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>'
                                                                                                        ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                        OnCommand="IbtnDeletePay_Command" Visible="false" />--%>
                                                                                            </ItemTemplate>
                                                                                            <FooterStyle BorderStyle="None" />
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Visit Date %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblvisitdate" runat="server" Text='<%# GetDate(Eval("Visit_Date").ToString()) %>' />
                                                                                            </ItemTemplate>
                                                                                            <FooterStyle BorderStyle="None" />
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Visit Time%>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblvisittime" runat="server" Text='<%# Eval("Visit_Time").ToString() %>' />
                                                                                            </ItemTemplate>
                                                                                            <FooterStyle BorderStyle="None" />
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblvehiclename" runat="server" Text='<%# Eval("VehicleName").ToString() %>' />
                                                                                            </ItemTemplate>
                                                                                            <FooterStyle BorderStyle="None" />
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Driver Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lbldrivername" runat="server" Text='<%# Eval("EmpName").ToString() %>' />
                                                                                            </ItemTemplate>
                                                                                            <FooterStyle BorderStyle="None" />
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>


                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                </asp:GridView>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblUploadfile" runat="server" Text="<%$ Resources:Attendance,File Upload %>"></asp:Label>
                                                        <asp:FileUpload ID="UploadFile" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesription" runat="server" Text="<%$ Resources:Attendance,Problem %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDescription" ErrorMessage="<%$ Resources:Attendance,Enter Problem%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                                            TabIndex="42" CssClass="form-control" Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPISave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            TabIndex="45" CssClass="btn btn-success" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait..';" OnClick="btnPISave_Click"
                                                            Visible="false" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            TabIndex="46" CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnPICancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            TabIndex="47" CausesValidation="False" OnClick="btnPICancel_Click" />
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label20" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I4" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Ticket No. %>" Value="Ticket_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Ticket Date %>" Value="Ticket_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Call No.%>" Value="Call_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Status %>" Value="Status"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        TabIndex="61" OnClick="btnbindBin_Click"
                                                        ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                    TabIndex="62" OnClick="btnRefreshBin_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                    TabIndex="63" runat="server" OnClick="imgBtnRestore_Click"
                                                    ToolTip="<%$ Resources:Attendance, Active %>" Visible="False"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvTicketMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvTicketMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        TabIndex="65" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        OnPageIndexChanging="GvTicketMasterBin_PageIndexChanging" OnSorting="GvTicketMasterBin_OnSorting"
                                                        AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        TabIndex="66" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged"
                                                                        TabIndex="67" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Ticket_No" HeaderText="<%$ Resources:Attendance,Ticket No. %>"
                                                                SortExpression="Ticket_No" />
                                                            <asp:TemplateField SortExpression="Ticket_Date" HeaderText="<%$ Resources:Attendance,Ticket Date%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDate" runat="server" Text='<%# GetDate(Eval("Ticket_Date").ToString()) %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Call_No" HeaderText="<%$ Resources:Attendance,Call No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefType" runat="server" Text='<%#Eval("Call_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Schedule_Date" HeaderText="<%$ Resources:Attendance,Schedule Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefNo" runat="server" Text='<%#GetDate(Eval("Schedule_Date").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Chargeable Amount %>" SortExpression="Chargeable_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%# Eval("Chargeable_Amount")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDept" runat="server" Text='<%# Eval("Status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Call_Logs">
                        <asp:UpdatePanel ID="Update_Call_Logs" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div5" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label21" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsRequest" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecordRequest" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I5" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameRequest" runat="server" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlFieldNameRequest_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Call No. %>" Value="Call_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customer_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Name %>" Value="Contact_Person"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact No %>" Value="Contact_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Received By%>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Priority%>" Value="Priority"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Call Type%>" Value="Call_Type"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionRequest" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindRequest">
                                                        <asp:TextBox ID="txtValueRequest" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueRequestDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueRequestDate" runat="server" TargetControlID="txtValueRequestDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindRequest" runat="server" CausesValidation="False"
                                                        TabIndex="73" OnClick="btnbindRequest_Click"
                                                        ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="btnRefreshRequest" runat="server" CausesValidation="False"
                                                    OnClick="btnRefreshRequest_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;


                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvCallRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCallRequest" runat="server" AllowPaging="True" AllowSorting="True"
                                                        TabIndex="75" AutoGenerateColumns="False" OnPageIndexChanging="GvCallRequest_PageIndexChanging"
                                                        OnSorting="GvCallRequest_Sorting" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None"
                                                                        TabIndex="76" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        CssClass="btnPull" OnCommand="btnPREdit_Command" />
                                                                    <%--  <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/edit.png" OnCommand="btnPREdit_Command" />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reject %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Reject %>" ImageUrl="~/Images/disapprove.png"
                                                                        OnCommand="IbtnUpdateCallLogs_Command" Width="16px" Visible="true" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call Id %>" SortExpression="Trans_Id"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call No.%>" SortExpression="Call_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("Call_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Call Date %>" SortExpression="Call_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("Call_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact Name %>" SortExpression="ContactName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("ContactName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact No %>" SortExpression="Contact_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactNo" runat="server" Text='<%#Eval("Contact_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call Type %>" SortExpression="Call_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCallType" runat="server" Text='<%#Eval("Call_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Received By %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvEmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Priority">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPriority" runat="server" Text='<%#Eval("Priority") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnTransNo" runat="server" Value="0" />
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

    <div class="modal fade" id="TicketHistory" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">


                    <div class="col-md-12" id="div_quote" runat="server">
                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="lblTicketdata" runat="server" Text="Ticket History" Font-Bold="true"></asp:Label>
                                </h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="form-group">

                                    <asp:UpdatePanel ID="upTicketHistory" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTicketHistory" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Ticket No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="gvTicketNo" runat="server" Text='<%#Eval("Ticket_No")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvQuoteAmt" runat="server" Text='<%#GetDate(Eval("Ticket_Date").ToString())%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EditRowStyle BackColor="#2461BF" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-md-12">
                        <br />
                    </div>


                    <div class="col-md-12" id="div_order" runat="server">
                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="lblVisit" runat="server" Text="Work Visit History" Font-Bold="true"></asp:Label>

                                </h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitHistory" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="work Order No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("Work_Order_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# GetDate(Eval("Work_Order_Date").ToString())%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Reference Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvRef_Type" runat="server" Text='<%#Eval("Ref_Type")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Reference No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvReferenceNo" runat="server" Text='<%#Eval("Ref_Id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="gvStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EditRowStyle BackColor="#2461BF" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="col-md-12">
                        <br />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="upTicketHistory">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Call_Logs">
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
    <script src="../Script/master.js"></script>
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

        function LI_Call_Log_Active() {
            $("#Li_Call_Logs").removeClass("active");
            $("#Call_Logs").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
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
        function Li_Tab_Call_Logs() {
            document.getElementById('<%= Btn_Call_Logs.ClientID %>').click();
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to send mail?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function Modal_TicketHistory() {
            document.getElementById('<%= Btn_TicketHistory.ClientID %>').click();
        }
        function getDocNo(ctrl) {
            var txtBox = document.getElementById('<%= txtticketNo.ClientID %>');
            getDocumentNo(ctrl, txtBox);
        }
    </script>
</asp:Content>
