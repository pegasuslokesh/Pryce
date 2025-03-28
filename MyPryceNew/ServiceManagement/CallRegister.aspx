<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CallRegister.aspx.cs" Inherits="ServiceManagement_CallRegister" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-address-book"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Call Register%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Call Register%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                                                <i class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">

                                            <asp:HiddenField runat="server" ID="hdnEmpList" />
                                            <div class="col-lg-6">
                                                <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                               
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, All%>" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Cancel%>" Value="Cancel"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Hold%>" Value="Hold"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                 <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvCallRegister.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>
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


                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvCallRegister"></dx:ASPxGridViewExporter>

                                                    <dx:ASPxGridView ID="GvCallRegister" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id">
                                                        <Columns>
                                                            <dx:GridViewDataColumn VisibleIndex="2">
                                                                <DataItemTemplate>

                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") + "," + Eval("Location_Id") %>'
                                                                        TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command" Visible='<%# hdnCanView.Value=="true"?true:false%>'
                                                                        CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id")  + "," + Eval("Location_Id") %>' Visible='<%# hdnCanEdit.Value=="true"?true:false%>'
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i> </asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") + "," + Eval("Location_Id") %>' Visible='<%# hdnCanDelete.Value=="true"?true:false%>'
                                                                        OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkFeedbackDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") + "," + Eval("Location_Id") %>'
                                                                        TabIndex="9" Visible='<%# hdnCanView.Value=="true"?true:false%>'
                                                                        ToolTip="<%$ Resources:Attendance,Ticket FeedBack %>" OnCommand="lnkFeedbackDetail_Command"
                                                                        CausesValidation="False"><i class="fa fa-phone" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Trans_Id" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Call Id %>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Call_No" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Call No. %>" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataDateColumn Caption="<%$ Resources:Attendance, Call Date %>" VisibleIndex="8" ReadOnly="True">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("Call_Date").ToString()) %>' />
                                                                    <asp:Label ID="lblgvcalltime" runat="server" Text='<%#Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("hh:mm") %>' />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Customer Name %>" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="ContactName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Contact Name %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="HandledEmployee" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Handled Employee %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Contact_No" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Contact No %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Call_Type" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Call Type %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Status" Settings-AutoFilterCondition="Contains" Caption="Call Status" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Field1" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance, Ticket Status %>" VisibleIndex="15">
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
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label7" runat="server" Text="Location"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlLoc" CssClass="form-control" onchange="getDocNo(this)"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Call No. %>"></asp:Label>
                                                        <asp:TextBox ID="txtCINo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div id="TrIn" runat="server" visible="false">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblCINo" runat="server" Text="<%$ Resources:Attendance,Call Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCIDate" ErrorMessage="<%$ Resources:Attendance,Enter Call Date%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtCIDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtCIDate" Format="dd/MM/yyyy/hh/mm/ss"
                                                                PopupButtonID="txtCIDate" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCIDate" runat="server" Text="<%$ Resources:Attendance, Call Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtCallTime" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtECustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtECustomer" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee" OnTextChanged="txtECustomer_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtECustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlContact" runat="server">
                                                        <asp:Label ID="lblContact" runat="server" Text="<%$ Resources:Attendance,Contact Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtEContact" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtEContact_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtEContact"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlContactNo" runat="server">
                                                        <asp:Label ID="lblContactNo" runat="server" Text="<%$ Resources:Attendance,Contact No %>"></asp:Label>
                                                        
                                                        
                                                        <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtContactNo" ValidChars="1,2,3,4,5,6,7,8,9,0,">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlEmail" runat="server">
                                                        <asp:Label ID="lblEmailId" runat="server" Text="<%$ Resources:Attendance,Email ID %>"></asp:Label>
                                                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlRefTo" runat="server">
                                                        <asp:Label ID="lblRefTo" runat="server" Text="<%$ Resources:Attendance,Received By%>"></asp:Label>
                                                        <asp:TextBox ID="txtRefTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtRefTo_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListRefTo" ServicePath="" TargetControlID="txtRefTo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlHandledEmp" runat="server">
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCallType" runat="server" Text="<%$ Resources:Attendance,Call Type %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCallType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Call Type %>" />

                                                        <asp:DropDownList ID="ddlCallType" runat="server" CssClass="form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlCallType_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Sales Inquiry%>" Value="Sales Inquiry"
                                                                Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Service%>" Value="Service"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Job Cards%>" Value="Job Cards"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Installation and implementation%>" Value="Installation and implementation"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Cancel%>" Value="Cancel"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Hold%>" Value="Hold"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Priority %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, High%>" Value="High" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Medium%>" Value="Medium"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Low%>" Value="Low"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblSetReminder" runat="server" Text="Set Reminder"></asp:Label>
                                                        <asp:DropDownList ID="ddlSetReminder" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDesc" runat="server" Text="<%$ Resources:Attendance,Call Detail %>"
                                                            Font-Bold="true"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCallDetail" ErrorMessage="<%$ Resources:Attendance,Enter Call Detail%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCallDetail" runat="server" Style="min-height: 50px; max-height: 100%; min-width: 100%; max-width: 100%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Notes %>"
                                                            Font-Bold="true"></asp:Label>
                                                        <asp:TextBox ID="txtNotes" runat="server" Style="min-height: 50px; max-height: 100%; min-width: 100%; max-width: 100%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnInquirySave_Click" Visible="false" />

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
                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Call No. %>" Value="Call_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Name %>" Value="Contact_Person"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact No %>" Value="Contact_No"></asp:ListItem>
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
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerInquiryBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomerInquiryBin_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCustomerInquiryBin_Sorting">

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
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="export">
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


        function alertMe() {

            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function getDocNo(ctrl) {
            var txtBox = document.getElementById('<%= txtCINo.ClientID %>');
            getDocumentNo(ctrl, txtBox);
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
</asp:Content>
