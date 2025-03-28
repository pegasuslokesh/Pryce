<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="VisitMaster.aspx.cs" Inherits="ProjectManagement_VisitMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-bus-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnnew_Click" Text="New" />
            <asp:Button ID="Btn_TrackUser" Style="display: none;" runat="server" OnClick="btnTrackUser_Click" Text="Track User" />
            <asp:Button ID="Btn_MenuReport" Style="display: none;" runat="server" OnClick="btnMenuReport_Click" Text="Menu Report" />
            <asp:Button ID="Btn_View_Feedback" Style="display: none;" runat="server" data-toggle="modal" data-target="#View_Feedback" Text="View Modal" />
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
                    <li id="Li_MenuReport"><a href="#MenuReport" onclick="Li_Tab_MenuReport()" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,MenuReport %>"></asp:Label></a></li>
                    <li id="Li_TrackUser"><a href="#TrackUser" onclick="Li_Tab_TrackUser()" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,TrackUser %>"></asp:Label></a></li>
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
                              
                                <div class="box box-primary collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label38" 
                                                runat="server" Text="Advance Search"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                          <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlStatusFilter_Click" AutoPostBack="true">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,All%>" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Close%>" Value="Close"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Cancel%>" Value="Cancel"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display:none;">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"
                                                        Selected="True" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Visit Date %>" Value="Visit_Date" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Project Name %>" Value="Project_Name" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Vehicle Name %>" Value="VehicleName" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Driver Name %>" Value="EmpName" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedEmployee" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display:none;">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" style="display:none;">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                        </asp:Panel>                                                
                                            </div>
                                            <div class="col-lg-2" style="text-align:center; display:none;">
                                                <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" style="margin-top:-5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" style="width:33px;"
                                                    ImageUrl="~/Images/refresh.png" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                    OnClick="btnRefresh_Click"></asp:ImageButton>

                                                <asp:ImageButton ID="btnreport" runat="server" CausesValidation="False" style="width:33px;"
                                                    ImageUrl="~/Images/Report.png" ToolTip="<%$ Resources:Attendance,Report %>"
                                                    OnClick="btnreport_OnClick"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2" style="display:none;">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>  

                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvVisitMaster.VisibleRowCount>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                      <dx:ASPxGridView ID="GvVisitMaster" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Trans_Id">
                                                        <Columns>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                        CausesValidation="False"><i class="fa fa-eye" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="IbtnDelete_Command"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                                   

                                                            <dx:GridViewDataTextColumn FieldName="CustomerName" Caption="<%$ Resources:Attendance,Customer Name %>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataDateColumn Caption="<%$ Resources:Attendance,Visit Date %>" FieldName="Visit_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="8" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Visit_Time" Caption="<%$ Resources:Attendance,Visit Time %>" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="VehicleName" Caption="<%$ Resources:Attendance,Vehicle Name %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="EmpName" Caption="<%$ Resources:Attendance,Driver Name %>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>
                                                             <dx:GridViewDataTextColumn FieldName="CreatedEmployee" Caption="<%$ Resources:Attendance,Created By%>" VisibleIndex="12">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="AreaName" Caption="<%$ Resources:Attendance, Area Name%>" VisibleIndex="15">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Attendance, Status%>" VisibleIndex="15">
                                                                <DataItemTemplate>
                                                                     <asp:LinkButton ID="lnkstatus" runat="server" Text='<%# Eval("Status") %>' CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,View Feedback%>" OnCommand="lnkstatus_OnCommand"
                                                                        Font-Underline="true" ForeColor="Blue"></asp:LinkButton>
                                                                </DataItemTemplate>
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


                                                
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="hdnvisitid" runat="server" />
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
                                <div id="pnlprojectrecord" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance, Status %>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                            <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddlReftype" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlReftype_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                                                            <asp:ListItem Text="Project" Value="Task"></asp:ListItem>
                                                            <asp:ListItem Text="Ticket" Value="Ticket"></asp:ListItem>
                                                            <asp:ListItem Text="Work Order" Value="WORK"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="trticketlist" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblCINo" runat="server"  Text="<%$ Resources:Attendance,Ticket No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtticketno" runat="server" CssClass="form-control" 
                                                            BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtticketno_OnTextChanged" />
                                                        <asp:HiddenField ID="hdnTicketid" runat="server" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListTicketNo" ServicePath=""
                                                            TargetControlID="txtticketno" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Label ID="lnkticketdesc" runat="server" Text="<%$ Resources:Attendance,Detail%>"
                                                            Font-Underline="true"  Visible="false" ForeColor="Blue"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trTicketDetail" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="box box-primary">
                                                                        <div class="box-header with-border">
                                                                            <h3 class="box-title">
                                                                                <asp:Label ID="lblDeviceParameter" Font-Names="Times New roman" Font-Size="18px"
                                                                                    Font-Bold="true" runat="server" Text="Ticket Detail" ></asp:Label></h3>
                                                                            <div class="box-tools pull-right">
                                                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                    <i class="fa fa-minus"></i>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                        <div class="box-body">
                                                                            <div class="form-group">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblTiDate" runat="server"  Text="<%$ Resources:Attendance,Ticket Date %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp
                                                                                    <asp:Label ID="lblTickeDate" runat="server" ></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblCustomerName" runat="server"  Text="<%$ Resources:Attendance,Customer Name %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblCustomerNameValue" runat="server" ></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblCallType" runat="server"  Text="<%$ Resources:Attendance,Task Type %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblTaskType" runat="server" >
                                                                                    </asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label7" runat="server"  Text="<%$ Resources:Attendance,Status%>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label8" runat="server"  Text="<%$ Resources:Attendance,Schedule Date %>"
                                                                                        Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblScheduledate" runat="server" ></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblDesription" runat="server"  Text="<%$ Resources:Attendance,Description %>"
                                                                                        Font-Bold="true" />
                                                                                    &nbsp:&nbsp<asp:Label ID="lblDescriptionvalue" runat="server" ></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div id="trprojectlist" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Project Name %>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddlprojectname" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlprojectname_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="trtasklist" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Task List%>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddltask" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddltask_OnSelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lnkTaskdesc" runat="server" Text="<%$ Resources:Attendance,Detail%>"
                                                            Font-Underline="true"  Visible="false" ForeColor="Blue"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trtaskDetail" runat="server" visible="false" class="col-md-12">
                                                        <cc2:Editor ID="txttaskdescription" runat="server" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div id="trWorkOrder" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label31" runat="server" Text="Work Order"
                                                            ></asp:Label>
                                                        <asp:TextBox ID="txtworkorder" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtcustomername" errormessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtcustomername" runat="server" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcustomername_TextChanged" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListCustomerName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtcustomername"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAddressCategory" runat="server"  Text="<%$ Resources:Attendance,Address Category %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlAddressCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAddressCategory_OnSelectedIndexChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Address %>"
                                                            ></asp:Label>
                                                        <asp:TextBox ID="txtCustomerAddress" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPermanentMobileNo" runat="server"  Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                        <asp:TextBox ID="txtPermanentMobileNo" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtPermanentMobileNo" ValidChars="+,0,1,2,3,4,5,6,7,8,9">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server"  Text="<%$ Resources:Attendance,Area Name  %>"></asp:Label>
                                                        <asp:TextBox ID="txtAreaName" TabIndex="104" BackColor="#eeeeee" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAreaName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAreaName"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnParentid" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLongitude" runat="server"  Text="<%$ Resources:Attendance,Longitude %>" />
                                                        <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control" onkeypress="return NumberFloatAndOneDOTSign(this)"
                                                            Enabled="false" Text="0.0000" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLatitude" runat="server" Text="<%$ Resources:Attendance,Latitude %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" onkeypress="return NumberFloatAndOneDOTSign(this)"
                                                                Enabled="false" Text="0.0000" />
                                                            <div style="margin-left:5px;" class="input-group-btn">
                                                                <asp:Button ID="BtnUpdateLatLong" runat="server" Text="Set Value" CssClass="btn btn-primary" OnClick="BtnUpdateLatLong_Click" />
                                                            </div>
                                                            <div style="margin-left:5px;" class="input-group-btn">
                                                                <asp:Button ID="btnGetLatLong" runat="server" Text="Map" CssClass="btn btn-primary" OnClick="btnGetLatLong_Click" />
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
                                                                            <asp:Label ID="lnkEmployeelist" runat="server" Text="For Add Employee click here"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="Label16" runat="server" Text="Select Employee" ></asp:Label>
                                                                                <asp:ListBox ID="listtaskEmployee" runat="server" Height="200px" SelectionMode="Multiple"
                                                                                    CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
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
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Visit Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator1" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtVisitDate" errormessage="<%$ Resources:Attendance,Enter Visit Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendarExtendertxtVisitDate" runat="server" TargetControlID="txtVisitDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Visit Time %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator2" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtVisitTime" errormessage="<%$ Resources:Attendance,Enter Visit Time%>"></asp:RequiredFieldValidator>

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
                                                        <asp:Label ID="lblSelectExp" runat="server"  Text="<%$ Resources:Attendance,Vehicle Name %>" />
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator4" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtvehiclename" errormessage="<%$ Resources:Attendance,Enter Vehicle Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtvehiclename" runat="server" AutoPostBack="true"
                                                            OnTextChanged="txtvehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtvehiclename"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance, Driver Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator5" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtdrivername" errormessage="<%$ Resources:Attendance,Enter Driver Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtdrivername" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                            OnTextChanged="txtdrivername_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                            TabIndex="5"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDriverName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtdrivername"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server"  Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtInvoiceNo_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceNo"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnInvoiceId" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label19" runat="server"  Text="<%$ Resources:Attendance,Chargeable Amount %>"></asp:Label>
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
                                                                            <asp:Label ID="lnkAddTask" runat="server" Text="For Add task click here"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitTask" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                    Width="100%" ShowFooter="true"  OnRowDeleting="gvVisitTask_RowDeleting"
                                                                                    OnRowEditing="gvVisitTask_RowEditing" OnRowCancelingEdit="gvVisitTask_OnRowCancelingEdit"
                                                                                    OnRowUpdating="gvVisitTask_OnRowUpdating" OnRowCommand="gvVisitTask_RowCommand">
                                                                                    
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblsno" runat="server"  Text='<%# Container.DataItemIndex+1 %>'
                                                                                                    Width="20px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle  HorizontalAlign="Left" Width="20px" />
                                                                                            <FooterStyle Width="20px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Task List%>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblItemTask" runat="server"  Text='<%#Eval("Task") %>'
                                                                                                    Width="660px"></asp:Label>
                                                                                                <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <EditItemTemplate>
                                                                                                <asp:TextBox ID="txteditTask" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                                                                    Text='<%#Eval("Task") %>'></asp:TextBox>
                                                                                            </EditItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtFooterTask" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle  HorizontalAlign="Left" Width="660px" />
                                                                                            <FooterStyle Width="660px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkItemStatus" runat="server"  Enabled="false" />
                                                                                            </ItemTemplate>
                                                                                            <EditItemTemplate>
                                                                                                <asp:CheckBox ID="chkeditItemStatus" runat="server"  Checked='<%#Eval("Status") %>' />
                                                                                            </EditItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:CheckBox ID="chkFooterStatus" runat="server"  />
                                                                                                <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle  HorizontalAlign="Center" Width="60px" />
                                                                                            <FooterStyle Width="60px" HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <EditItemTemplate>
                                                                                                <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" CssClass="btn btn-primary"
                                                                                                    Text="Update" CausesValidation="true" CommandArgument='<%#Eval("Trans_Id") %>' />&nbsp;&nbsp;
                                                                                                    <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-primary"/>
                                                                                            </EditItemTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Images/edit.png" CommandName="Edit"
                                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>" Style="width: 14px" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    <%-- <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="true" />--%>
                                                                                                <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                    ImageUrl="~/Images/Erase.png" CommandName="Delete" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                                <%--<asp:Button ID="ButtonDelete" runat="server"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  CommandName="Delete" />--%>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="IbtnAddTask">
                                                                                                    <asp:ImageButton ID="IbtnAddTask" runat="server" CausesValidation="False" Height="29px"
                                                                                                        ImageUrl="~/Images/add.png" CommandName="AddNew" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                                                </asp:Panel>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle  HorizontalAlign="Center" Width="150px" />
                                                                                            <FooterStyle Width="150px" HorizontalAlign="Center" />
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
                                                        <asp:Label ID="Label32" runat="server" Text="Notification" ></asp:Label>
                                                        <asp:TextBox ID="txtdescription" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkIsurgent" CssClass="form-control" runat="server" Text="Is Urgent"  />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnsave" runat="server" Visible="false" CssClass="btn btn-success"
                                                            OnClick="btnsave_Click" Text="<%$ Resources:Attendance,Save%>" />

                                                        <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                                            Text="<%$ Resources:Attendance,Reset%>" />

                                                        <asp:Button ID="btncencel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            OnClick="btncencel_Click" CssClass="btn btn-danger" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
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
                    <div class="tab-pane" id="TrackUser">
                        <asp:UpdatePanel ID="Update_TrackUser" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label27" runat="server"  Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtFromDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server"  Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label22" runat="server"  Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtemployeename" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                            BackColor="#eeeeee" TabIndex="5"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDriverName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtemployeename"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnTrack" runat="server" OnClick="btnTrack_OnClick" Text="Track User"
                                                            CssClass="btn btn-primary" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnLiveTrack" runat="server" OnClick="btnLiveTrack_OnClick" Text="Live Track"
                                                                    CssClass="btn btn-primary" />
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
                    <div class="tab-pane" id="MenuReport">
                        <asp:UpdatePanel ID="Update_MenuReport" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label25" runat="server"  Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtFromdateReport" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendarExtender_txtFromdateReport" runat="server" Enabled="True"
                                                            TargetControlID="txtFromdateReport">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label26" runat="server"  Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <asp:TextBox ID="txttodatereport" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendarExtender_txttodatereport" runat="server" Enabled="True" TargetControlID="txttodatereport">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance, Status %>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddlreportStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                            <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"
                                                            ></asp:Label>
                                                        <asp:DropDownList ID="ddlreportRefType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                                                            <asp:ListItem Text="Project" Value="Task"></asp:ListItem>
                                                            <asp:ListItem Text="Ticket" Value="Ticket"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator6" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtReportCustomername" errormessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtReportCustomername" runat="server" BackColor="#eeeeee"
                                                            CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListCustomerName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReportCustomername"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label29" runat="server"  Text="<%$ Resources:Attendance,Vehicle Name %>" />
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator7" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtReportVehiclename" errormessage="<%$ Resources:Attendance,Enter Vehicle Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtReportVehiclename" runat="server" AutoPostBack="true"
                                                            OnTextChanged="txtReportVehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReportVehiclename"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance, Driver Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator8" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtReportDrivername" errormessage="<%$ Resources:Attendance,Enter Driver Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtReportDrivername" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                            OnTextChanged="txtdrivername_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                            TabIndex="5"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDriverName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReportDrivername"
                                                            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnTransportreport" runat="server" OnClick="btnTransportreport_OnClick" Text="Report"
                                                            CssClass="btn btn-primary" />
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

    <div class="modal fade" id="View_Feedback" tabindex="-1" role="dialog" aria-labelledby="View_FeedbackLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="View_FeedbackLabel"><asp:Label ID="lblProductHeader" runat="server" Font-Size="14px" Font-Bold="true"
                                                                             Text="<%$ Resources:Attendance, View Feedback %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_View_Feedback" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Receiver Name %>"
                                                        ></asp:Label>
                                                    <asp:Label ID="lblReceiverName"  runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Receiver Contact No. %>"
                                                        ></asp:Label>
                                                    <asp:Label ID="lblreciverContactNo"  runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Feedback Date %>"
                                                        ></asp:Label>
                                                    <asp:Label ID="lblfeedbackDate"  runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Feedback%>"
                                                        ></asp:Label>
                                                    <asp:TextBox ID="txtfeedback" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                        Enabled="false"></asp:TextBox>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,Employee Signature%>"></asp:Label>
                                                    <img id="imgsignature" runat="server" width="132" height="102" />
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_View_Feedback_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <button type="button" class="btn btn-primary">
                                Save changes</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_View_Feedback">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_View_Feedback_Button">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_TrackUser">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_MenuReport">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="PanelList" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlnew" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlTrack" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlTabtrackuser" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlReport" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnllist" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlMenuReport" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlFeedback1" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="pnlFeedback2" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel9" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel10" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel11" runat="server" Visible="false"></asp:Panel>
		

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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_TrackUser() {
            document.getElementById('<%= Btn_TrackUser.ClientID %>').click();
        }

        function Li_Tab_MenuReport() {
            document.getElementById('<%= Btn_MenuReport.ClientID %>').click();
        }

        function View_Feedback_Popup() {
            document.getElementById('<%= Btn_View_Feedback.ClientID %>').click();
        }
    </script>
    <script>
        function NumberFloatAndOneDOTSign(CurrentElement) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
            if (charCode == 46 && (elementRef.value.indexOf('.') >= 0))
                return false;
            else if (charCode == 46 && (elementRef.value.indexOf('.') <= 0)) //allow to enter dot sign if not entered.
                return true;

            return true;
        }
    </script>
</asp:Content>
