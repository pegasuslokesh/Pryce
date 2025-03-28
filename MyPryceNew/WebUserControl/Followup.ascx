<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Followup.ascx.cs" Inherits="WebUserControl_Followup" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="tc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="tc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ContactNo.ascx" TagPrefix="uc1" TagName="ContactNo1" %>

<asp:HiddenField ID="hdnTableName" runat="server" />

<asp:UpdatePanel ID="Update_Button" runat="server">
    <ContentTemplate>
        <asp:Button ID="Btn_FollowupList1" Style="display: none;" runat="server" Text="FollowupList" OnClick="Btn_FollowupList1_Click" />
        <asp:Button ID="Btn_New1" Style="display: none;" runat="server" Text="New" />
        <asp:Button ID="Btn_Bin1" Style="display: none;" runat="server" Text="Bin" OnClick="Btn_Bin1_Click" />
        <asp:HiddenField ID="hdnFollowupLocationId" runat="server" />
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

    <asp:UpdatePanel ID="Update_Header" runat="server">
        <ContentTemplate>
            <div class="col-lg-6" style="max-width: 100%;">
                <h3>
                    <img src="../Images/follow-up.png" alt="Followup" /><asp:Label ID="lblHeaderL" runat="server"></asp:Label>
                </h3>
            </div>
            <div class="col-lg-6" style="text-align: right; margin-top: 12px; max-width: 100%;">
                <h4>
                    <asp:Label ID="lblHeaderR" runat="server"></asp:Label>
                </h4>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Header">
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

    <div class="col-lg-12">
        <div class="nav-tabs-custom">
            <ul class="nav nav-tabs pull-right bg-blue-gradient">
                <li id="Li_Bin1"><a href="#Bin1" onclick="Li_Tab_Bin1()" data-toggle="tab">
                    <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                <li id="Li_New1" class="active"><a onclick="Li_Tab_New1()" href="#New1" data-toggle="tab">
                    <asp:UpdatePanel ID="Update_Li" runat="server">
                        <ContentTemplate>
                            <i class="fa fa-file"></i>&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </a></li>

                <li id="Li_FollowupList1"><a href="#FollowupList1" onclick="Li_Tab_FollowupList1()" data-toggle="tab">
                    <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Follow-Up List%>"></asp:Label></a></li>

            </ul>

        </div>
    </div>
</div>

<div class="tab-content">

    <div class="tab-pane" id="FollowupList1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFollowupSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="Btn_FollowupList1" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="alert alert-info ">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlFieldNameFollowup" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameFollowup_SelectedIndexChanged">
                                    <asp:ListItem Text="FollowUp Date" Value="Followup_date"></asp:ListItem>
                                    <asp:ListItem Text="Next FollowUp Date" Value="Next_followup_date"></asp:ListItem>
                                    <asp:ListItem Text="Contact Name" Value="ContactName"></asp:ListItem>
                                    <asp:ListItem Text="Generated By" Value="FollowupByName"></asp:ListItem>
                                    <asp:ListItem Text="FollowUp Type" Value="Followup_type"></asp:ListItem>
                                    <asp:ListItem Text="Remark" Value="Description"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOptionFollowup" runat="server" class="form-control">
                                    <asp:ListItem Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Text="Equal"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="Contains"></asp:ListItem>
                                    <asp:ListItem Text="Like"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:TextBox ID="txtValueFollowup" runat="server" Visible="false" Class="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtValueDateFollowup" runat="server" Class="form-control"></asp:TextBox>
                                <tc1:CalendarExtender OnClientShown="calendarShown" Format="dd-MMM-yyyy" ID="CalendarExtender22" runat="server" Enabled="True" TargetControlID="txtValueDateFollowup" OnClientDateSelectionChanged="Date">
                                </tc1:CalendarExtender>


                            </div>
                            <div class="col-lg-3">
                                <asp:ImageButton ID="btnListSearch" runat="server" CausesValidation="False"
                                    ImageUrl="~/Images/search.png" OnClick="btnListSearch_Click" Style="margin-left: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                <asp:ImageButton ID="btnRefreshFollowup" runat="server" CausesValidation="False" Style="width: 33px;"
                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshFollowup_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>


                                <asp:HiddenField ID="hdnCheck" runat="server" Value="true" />

                            </div>
                            <div class="col-lg-2">
                                <h5>
                                    <asp:Label ID="lblTotalRecordsFollowup" Font-Bold="true" runat="server"></asp:Label></h5>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                        <asp:Label ID="lblheaderTitle" runat="server"></asp:Label>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="flow">
                                    <asp:HiddenField ID="hdntransID" runat="server"></asp:HiddenField>
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvListData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvListData_PageIndexChanging" OnSorting="GvListData_Sorting"
                                        AllowSorting="true" >
                                        <Columns>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkViewDetailFollowup" runat="server" CommandArgument='<%# Eval("Trans_id") %>'
                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="View" CommandName='<%# Eval("Location_id") %>'
                                                        OnCommand="lnkViewDetailFollowup_Command" CausesValidation="False" />
                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEditFollowup" runat="server" CommandArgument='<%# Eval("Trans_id") %>' CommandName='<%# Eval("Location_id") %>' ToolTip="Edit"
                                                        ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="lnkViewDetailFollowup_Command" />

                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtnDeleteFollowup" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_id") %>' OnCommand="IbtnDeleteFollowup_Command"
                                                        ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="Delete" />
                                                    <tc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                        TargetControlID="IbtnDeleteFollowup">
                                                    </tc1:ConfirmButtonExtender>
                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" Width="4%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="FileUpload">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_id") %>' ToolTip="File-Upload"
                                                        ImageUrl="~/Images/ModuleIcons/archiving.png" Height="30px" Width="30px" CausesValidation="False" OnCommand="btnFileUpload_Command" />
                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Follow-Up No%>" SortExpression="Followup_No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_No" runat="server" Text='<%#Eval("Followup_No") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,FollowUp Date%>" SortExpression="Followup_date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_date" runat="server" Text='<%#GetDate(Eval("Followup_date").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>" SortExpression="ContactName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("ContactName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,FollowUp Type%>" SortExpression="Followup_type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_type" runat="server" Text='<%#Eval("Followup_type") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description%>" SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblDescription" BorderStyle="None" Enabled="false" ToolTip='<%#Eval("Description") %>' BackColor="White" runat="server" Text='<%#Eval("Description") %>' MaxLength="50" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Generated By%>" SortExpression="FollowupByName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowupByName" runat="server" Text='<%#Eval("FollowupByName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Next Follow-Up Date%>" SortExpression="Next_followup_date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNext_followup_date" runat="server" Text='<%# GetDate(Eval("Next_followup_date").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>



                                        </Columns>
                                        
                                        
                                        <PagerStyle CssClass="pagination-ys" />
                                        
                                    </asp:GridView>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div class="tab-pane active" id="New1">
        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">

                                <div class="form-group">

                                    <div class="col-md-4">
                                        <asp:Label ID="lblfollowupId" runat="server" Text="<%$ Resources:Attendance,Follow-Up No%>" /><a style="color: Red">*</a><asp:Label ID="lblfollowupDuplicacy" runat="server" ForeColor="Red" Visible="false" Text="Followup No Already Exists" />
                                        <asp:TextBox ID="txtfollowupId" runat="server" Class="form-control" OnTextChanged="txtfollowupId_TextChanged" AutoPostBack="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblReferenceNo" runat="server" Text="<%$ Resources:Attendance,Reference No%>" /><a style="color: Red">*</a>
                                        <asp:TextBox ID="txtReferenceNo" runat="server" Class="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblFollowupDt" runat="server" Text="<%$ Resources:Attendance,FollowUp Date%>" /><a style="color: Red">*</a>
                                        <asp:TextBox ID="txtFollowupDt" runat="server" Class="form-control" />
                                        <tc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender2" runat="server" TargetControlID="txtFollowupDt" Format="dd-MMM-yyyy" />
                                        <br />
                                    </div>



                                    <div class="col-md-6">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" /><a style="color: Red">*</a>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCustomerName" runat="server" Class="form-control" BackColor="#eeeeee" Enabled="false" />
                                            <div class="input-group-btn">
                                                <asp:ImageButton ID="lnkcustomerHistory" ToolTip="Customer History" runat="server" ImageUrl="~/Images/history.png" OnClientClick="lnkcustomerHistory_OnClick();return false;"  AlternateText="Customer History" CausesValidation="False" />
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                    <asp:HiddenField ID="hdncust_Id" runat="server" />
                                    <asp:HiddenField ID="hdnMob_no" runat="server" />
                                    <asp:HiddenField ID="hdnEmail_Id" runat="server" />
                                    <asp:HiddenField ID="hdnContact_Id" runat="server" />

                                    <div class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Contact Name %>" /><a style="color: Red">*</a>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtContactList" runat="server" Class="form-control" BackColor="#eeeeee"
                                                OnTextChanged="txtContactList_TextChanged" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator ID="requiredfieldvalidator4" runat="server" ControlToValidate="txtContactList" ErrorMessage="Please Enter Contact Name !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                            <tc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                TargetControlID="txtContactList" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                            </tc1:AutoCompleteExtender>
                                            <div class="input-group-btn">
                                                <asp:ImageButton ID="btnAddCustomer" ToolTip="Add New Contact" runat="server" ImageUrl="~/Images/add-new.png" OnClick="btnAddCustomer_OnClick"
                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                            </div>

                                        </div>
                                        <br />
                                    </div>

                                    <div class="col-md-12" runat="server" id="div_followupType">
                                        <div class="col-md-6">
                                        <asp:Label ID="lblFollowupBy" runat="server" Text="<%$ Resources:Attendance,FollowUp Type%>" />
                                        <asp:DropDownList ID="ddlFollowupType" runat="server" Class="form-control" OnSelectedIndexChanged="ddlFollowupType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem>Select</asp:ListItem>
                                            <asp:ListItem>Call</asp:ListItem>
                                            <asp:ListItem>Email</asp:ListItem>
                                            <asp:ListItem>Visit</asp:ListItem>
                                            <asp:ListItem>Other</asp:ListItem>
                                        </asp:DropDownList>

                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblTypeReference" runat="server" Text="<%$ Resources:Attendance,Reference Type ID%>" />
                                        <asp:TextBox ID="txtTypeReference" runat="server" Class="form-control" Enabled="false" BackColor="#eeeeee"></asp:TextBox>
                                        <tc1:AutoCompleteExtender ID="AutoCompleteExtenderCall" runat="server"
                                            DelimiterCharacters="" Enabled="false" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListCallLogs" ServicePath=""
                                            TargetControlID="txtTypeReference" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                        </tc1:AutoCompleteExtender>
                                        <tc1:AutoCompleteExtender ID="AutoCompleteExtenderVisit" runat="server"
                                            DelimiterCharacters="" Enabled="false" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListVisitLogs" ServicePath=""
                                            TargetControlID="txtTypeReference" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                        </tc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    </div>
                                    
                                    <div class="col-md-2" id="btncall" runat="server" style="display: none;">
                                        <br />
                                        <asp:Button ID="btnGenerateCall" Text="Generate Call" CssClass="btn btn-primary" runat="server" OnClick="btnGenerateCall_Click" />

                                        <br />
                                    </div>

                                    <div class="col-md-2" id="btnvisit" runat="server" style="display: none;">
                                        <br />
                                        <asp:Button ID="btnGenerateVisit" Text="Generate Visit" CssClass="btn btn-primary" runat="server" />
                                        <br />
                                    </div>


                                    <div class="col-md-12" id="div_WorkOrder" style="display: none;" runat="server">
                                        <div class="box box-primary">
                                            <div id="Div_Box_Add3" class="box box-primary">
                                                <div class="box-header with-border">
                                                    <h4>
                                                        <asp:Literal runat="server" Text="Time Details" />:</h4>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="Btn_Add_Div3" class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">

                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label4" runat="server" Text="Visit Date" /><a style="color: Red">*</a>
                                                            <asp:TextBox ID="txtVisitDate" runat="server" Class="form-control" />
                                                            <tc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender3" runat="server" TargetControlID="txtVisitDate" Format="dd-MMM-yyyy" />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label14" runat="server" Text="Start Time" /><a style="color: red">*</a>
                                                            <asp:TextBox ID="txtInTime" runat="server" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add_Date"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInTime" ErrorMessage="<%$ Resources:Attendance,Enter From Time %>"></asp:RequiredFieldValidator>
                                                            <tc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtInTime"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </tc1:MaskedEditExtender>
                                                            <tc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                                ControlToValidate="txtInTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label17" runat="server" Text="End Time"></asp:Label><a style="color: red">*</a>


                                                            <asp:TextBox ID="txtOuttime" runat="server" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Add_Date"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOuttime" ErrorMessage="<%$ Resources:Attendance,Enter From Time %>"></asp:RequiredFieldValidator>

                                                            <tc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOuttime"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </tc1:MaskedEditExtender>
                                                            <tc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                                ControlToValidate="txtOuttime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>




                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="col-md-12" id="div_AddNewContact" style="display: none;" runat="server">
                                        <div class="box box-primary">
                                            <div id="Div_Box_Add2" class="box box-primary">
                                                <div class="box-header with-border">
                                                    <h4>
                                                        <asp:Literal runat="server" Text="Add New Contact" />:</h4>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="Btn_Add_Div2" class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">

                                                        <div class="col-md-4">
                                                            <br />
                                                            <asp:DropDownList ID="ddlSalutation" Class="form-control" runat="server">
                                                                <asp:ListItem Text="Mr." Value="Mr"></asp:ListItem>
                                                                <asp:ListItem Text="Mrs." Value="Mrs"></asp:ListItem>
                                                                <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>
                                                                <asp:ListItem Text="Dr." Value="Dr"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                                            <asp:TextBox ID="txtName" Class="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                                                            <asp:TextBox ID="txtEmail" Class="form-control" runat="server"></asp:TextBox>
                                                            <tc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmailMaster" ServicePath=""
                                                                TargetControlID="txtEmail" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                            </tc1:AutoCompleteExtender>
                                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                                                                ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblMobile" runat="server" Text="Mobile No."></asp:Label>
                                                            <asp:TextBox ID="txtMob" Class="form-control" runat="server"></asp:TextBox>
                                                            <tc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtMob" FilterType="Numbers" FilterMode="ValidChars"></tc1:FilteredTextBoxExtender>
                                                            <tc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListContactNumber" ServicePath=""
                                                                TargetControlID="txtMob" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                            </tc1:AutoCompleteExtender>

                                                        </div>

                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                                            <asp:TextBox ID="txtDepartment" Class="form-control" runat="server" onchange="validateDepartment(this)"></asp:TextBox>
                                                            <tc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListDepartmentMaster" ServicePath=""
                                                                TargetControlID="txtDepartment" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                            </tc1:AutoCompleteExtender>

                                                        </div>

                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblGroup" runat="server" Text="Group"></asp:Label>
                                                            <asp:DropDownList ID="ddlGroup" Class="form-control" runat="server"></asp:DropDownList>
                                                        </div>


                                                        <div class="col-md-3">
                                                            <br />
                                                            <asp:Button ID="BtnSaveContact" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="BtnSaveContact_Click" />
                                                            <br />
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="col-md-12" id="div_MobileNo" runat="server">
                                        <div class="box box-primary">
                                            <div id="Div_Box_Add1" class="box box-primary">
                                                <div class="box-header with-border">
                                                    <h4>
                                                        <asp:Literal runat="server" Text="Mobile Numbers" />:</h4>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="Btn_Add_Div1" class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">

                                                        <uc1:ContactNo1 runat="server" ID="ContactNo1" />

                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="col-md-12" id="product_div" runat="server" style="display: block">
                                        <div class="box box-primary">
                                            <div id="Div_Box_Add" class="box box-info collapsed-box">
                                                <div class="box-header with-border">
                                                    <h4>
                                                        <asp:Literal runat="server" Text="<%$ Resources:Attendance,Product Details%>" />:</h4>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="Btn_Add_Div" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">

                                                        <div class="flow">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductDataFollowup" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                                AllowSorting="True" >
                                                                
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product code %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%#Eval("Product_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("SuggestedProductName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%#setUnitName(Eval("UnitId").ToString()) %>' />
                                                                            <asp:HiddenField ID="hdnUnit_ID" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCurrency" runat="server" Text='<%#setCurrency(Eval("Currency_Id").ToString()) %>' />
                                                                            <asp:HiddenField ID="hdnCurrencyID" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPrice" runat="server" Text='<%#Eval("EstimatedUnitPrice","{0:n}") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Quantity","{0:n}") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"  Width="9%" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark%>" /><a style="color: Red">*</a>
                                        <asp:TextBox ID="txtRemark" Style="resize: vertical; height: 50px; min-height: 50px; max-height: 150px;" runat="server" Class="form-control" TextMode="MultiLine" />
                                        <br />
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Label ID="lblNextFollowUpDate" runat="server" Text="<%$ Resources:Attendance,Next Follow-Up Date%>" /><a style="color: Red">*</a>
                                        <asp:TextBox ID="txtNextFollowUpDate" runat="server" Class="form-control" />
                                        <tc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender1" runat="server" TargetControlID="txtNextFollowUpDate" Format="dd-MMM-yyyy" />
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Generated By%>" /><a style="color: Red">*</a>
                                        <asp:TextBox ID="txtGeneratedByfollowup" runat="server" Class="form-control" BackColor="#eeeeee" onchange="validateEmployeeWithId(this)" />
                                        <tc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                            TargetControlID="txtGeneratedByfollowup" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                        </tc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <br />
                                        <asp:CheckBox runat="server" ID="chkReminder" Text="Set Reminder" />
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Panel ID="PnlOperationButton" runat="server">
                                            <asp:Button ID="btnQuote" runat="server" Text="Quote" OnClick="btnQuote_Click" Visible="false"
                                                class="btn btn-primary" />
                                            <asp:Button ID="btnFollowupSave" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                                class="btn btn-success" OnClick="btnFollowupSave_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait...'; " />
                                            <asp:Button ID="BtnFollowupReset" runat="server" OnClick="BtnFollowupReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                class="btn btn-primary" />
                                            <asp:HiddenField ID="hdnNo" runat="server" Value="0" />

                                            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />

                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>


    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="tab-pane" id="Bin1">
        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Btn_Bin1" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="alert alert-info ">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlFieldNameBinFollowup" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBinFollowup_SelectedIndexChanged">
                                    <asp:ListItem Text="FollowUp Date" Value="Followup_date"></asp:ListItem>
                                    <asp:ListItem Text="Company Name" Value="PartyName"></asp:ListItem>
                                    <asp:ListItem Text="Contact Name" Value="ContactName"></asp:ListItem>
                                    <asp:ListItem Text="Generated By" Value="Followup_by"></asp:ListItem>
                                    <asp:ListItem Text="FollowUp Type" Value="Followup_type"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOptionBinFollowup" runat="server" class="form-control">
                                    <asp:ListItem Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Text="Equal"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="Contains"></asp:ListItem>
                                    <asp:ListItem Text="Like"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnsearchBinFollowup">
                                    <asp:TextBox ID="txtValueBinFollowup" runat="server" Class="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtValueDateBinFollowup" runat="server" Class="form-control"></asp:TextBox>
                                    <tc1:CalendarExtender OnClientShown="calendarShown" Format="dd-MMM-yyyy" ID="CalendarExtenderFollowup2" runat="server" Enabled="True" TargetControlID="txtValueDateBinFollowup" OnClientDateSelectionChanged="Date">
                                    </tc1:CalendarExtender>

                                </asp:Panel>
                            </div>
                            <div class="col-lg-3">
                                <asp:ImageButton ID="btnsearchBinFollowup" runat="server" CausesValidation="False"
                                    ImageUrl="~/Images/search.png" OnClick="btnsearchBinFollowup_Click" Style="margin-left: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                <asp:ImageButton ID="btnRefreshBinFollowup" runat="server" CausesValidation="False" Style="width: 33px;"
                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshBinFollowup_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                <asp:ImageButton ID="imgBtnRestoreFollowup" CausesValidation="False" Style="width: 33px;" OnClick="imgBtnRestoreFollowup_Click" Visible="false"
                                    runat="server" ImageUrl="~/Images/active.png" ToolTip="<%$ Resources:Attendance, Active %>" />

                                <asp:ImageButton ID="ImgbtnSelectAllFollowup" runat="server" OnClick="ImgbtnSelectAllFollowup_Click" Style="width: 33px;" Visible="false"
                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />

                                <asp:HiddenField ID="hdnCheckFollowup" runat="server" Value="true" />

                            </div>
                            <div class="col-lg-2">
                                <h5>
                                    <asp:Label ID="lblTotalRecordsBinFollowup" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                    <asp:Label ID="lblSelectedRecordFollowup" runat="server" Visible="false"></asp:Label>
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvFollowupBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                        AllowSorting="true" OnPageIndexChanging="GvFollowupBin_PageIndexChanging" OnSorting="GvFollowupBin_Sorting" >
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkgvSelectAllFollowup" runat="server" OnCheckedChanged="chkgvSelectAllFollowup_CheckedChanged"
                                                        AutoPostBack="true" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectFollowup" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectFollowup_CheckedChanged" />
                                                    <asp:HiddenField ID="hdntransIdFollowup" runat="server" Value='<%#Eval("Trans_id") %>' Visible="false" />
                                                    <asp:Label ID="lbltransId" runat="server" Visible="false" Text='<%#Eval("Trans_id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  HorizontalAlign="Center" Width="2%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Follow-Up No" SortExpression="Followup_No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_NoFollowup" runat="server" Text='<%#Eval("Followup_No") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,FollowUp Date%>" SortExpression="Followup_date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_dateFollowup" runat="server" Text='<%#GetDate(Eval("Followup_date").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Company Name%>" SortExpression="PartyName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartyNameFollowup" runat="server" Text='<%#Eval("PartyName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>" SortExpression="ContactName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContactNameFollowup" runat="server" Text='<%#Eval("ContactName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,FollowUp Type%>" SortExpression="Followup_type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowup_typeFollowup" runat="server" Text='<%#Eval("Followup_type") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Generated By%>" SortExpression="FollowupByName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFollowupByNameFollowup" runat="server" Text='<%#Eval("FollowupByName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
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


    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>



<div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
    aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">

            <asp:UpdatePanel ID="up_fu" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <AT1:FileUpload1 runat="server" ID="FUpload1" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="" class="btn btn-default" data-dismiss="modal">
                            Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>


<asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="up_fu">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>



<script type="text/javascript">
    function resetPosition1(object, args) {


        var tbposition = findPositionWithScrolling(100004);
        var xposition = tbposition[0] + 2;
        var yposition = tbposition[1] + 25;
        var ex = object._completionListElement;

        if (ex)
            $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
    }
    function findPositionWithScrolling1(oElement) {
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



    function alertMe() {
        $("#Li_List").removeClass("active");
        $("#List").removeClass("active");

        $("#Li_New").addClass("active");
        $("#New").addClass("active");
    }


    function Li_Tab_FollowupList1() {
        document.getElementById('<%= Btn_FollowupList1.ClientID %>').click();
    }
    function Li_Tab_New1() {
        document.getElementById('<%= Btn_New1.ClientID %>').click();
    }
    function Li_Tab_Bin1() {
        document.getElementById('<%= Btn_Bin1.ClientID %>').click();
    }

    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }
    function Modal_Open_FileUpload() {
        document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
    }


    function AddContact() {
        document.getElementById('div_AddNewContact').style.display = 'block';

        document.getElementById('<%= txtContactList.ClientID %>').value = "";
        document.getElementById('<%= txtName.ClientID %>').focus();
        //window.open("../EMS/ContactMaster.aspx?Page=SINQ");
    }


    function CustomerHistory(id) {
        if (document.getElementById('<%=txtCustomerName.ClientID%>').value == "") {
            alert("Please Enter Customer Name");
            return;
        }
        window.open("../Purchase/CustomerHistory.aspx?ContactId=" + id + "&&Page=SINQ");
    }

    function CallRegister(id) {
        var ddl = document.getElementById('<%=ddlFollowupType.ClientID%>');
        if (ddl.value == "Call") {
            document.getElementById('btncall').style.display = 'block';
        }
        if (ddl.value == "Visit") {
            document.getElementById('btnvisit').style.display = 'block';
        }
        if (document.getElementById('<%=txtCustomerName.ClientID%>').value == "") {
            alert("Please Enter Customer Name");
            return;
        }
        window.open("../ServiceManagement/CallRegister.aspx?Followup_CustomerID=" + id);
    }


    function WorkOrder(id) {
        var ddl = document.getElementById('<%=ddlFollowupType.ClientID%>');
        if (ddl.value == "Call") {
            document.getElementById('btncall').style.display = 'block';
        }
        if (ddl.value == "Visit") {
            document.getElementById('btnvisit').style.display = 'block';
        }
        if (document.getElementById('<%=txtCustomerName.ClientID%>').value == "") {
            alert("Please Enter Customer Name");
            return;
        }

        window.open("../ServiceManagement/WorkOrder.aspx?Followup_CustomerID=" + id);
    }

    function btnCall_visit() {
        var ddl = document.getElementById('<%=ddlFollowupType.ClientID%>');
        if (ddl.value == "Call") {
            document.getElementById('btncall').style.display = 'block';
        }
        if (ddl.value == "Visit") {
            document.getElementById('btnvisit').style.display = 'block';
        }
    }

</script>

<script>


    function myTimer() {
        document.getElementById('<%=lblfollowupDuplicacy.ClientID%>').style.visibility = 'visible';
        var myVar = setInterval(function () { myStopFunction() }, 5000);
    }

    function myStopFunction() {
        document.getElementById('<%=lblfollowupDuplicacy.ClientID%>').style.visibility = 'hidden';
        clearInterval(myVar);
    }

    function DisplayMsg(str) {
        alert(str);
        return;
    }


    function Open_Div_Mobile_Number() {
        var NAME = document.getElementById("Btn_Add_Div1");
        var BODY = document.getElementById("Div_Box_Add1");
        var currentClass = NAME.className;
        if (currentClass == "fa fa-plus") { // Check the current class name
            NAME.className = "fa fa-minus";
            BODY.className = "box box-primary";
        } else {
            NAME.className = "fa fa-minus";  // Otherwise, use `second_name`
        }
    }

    function showNewTab() {
        $("#Li_FollowupList1").removeClass("active");
        $("#FollowupList1").removeClass("active");
        $("#Li_Bin1").removeClass("active");
        $("#Bin1").removeClass("active");
        $("#Li_New1").addClass("active");
        $("#New1").addClass("active");
    }
    function lnkcustomerHistory_OnClick() {
        var Id = "";
        try {
            Id = document.getElementById('<%=txtCustomerName.ClientID%>').value.split('/')[1];
    }
    catch (err) {
        Id = "0";
    }
    CustomerHistory(Id);
}

</script>
<script src="../Script/employee.js"></script>
<script src="../Script/master.js"></script>
