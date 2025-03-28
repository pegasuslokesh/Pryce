<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Reminder.aspx.cs" Inherits="CRM_Reminder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Reminder.ascx" TagPrefix="uc1" TagName="Reminder1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-clock-o"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Reminder"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Lead%>"></asp:Label></li>
    </ol>
      <script>
        function resetPosition1()
        {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" Text="Edit" />
            <asp:Button ID="Btn_Modal_Reminder" Style="display: none;" runat="server" data-toggle="modal" data-target="#Reminder123" Text="Reminder" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">

                <ul class="nav nav-tabs pull-right bg-blue-gradient">

                    <li id="Li_New" runat="server" visible="false"><a href="#New" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,Edit%>"></asp:Label>
                    </a></li>

                    <li id="Li_NewReminder"><a onclick="Li_Tab_New()" href="#Reminder123" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                    </a></li>

                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>

                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <%--<div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                     <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlUser" runat="server" Class="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">

                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" Class="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                                                    <asp:ListItem Text="Upcoming" Value="Upcoming"></asp:ListItem>
                                                    <asp:ListItem Text="Gone" Value="Gone"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlPerameter" runat="server" Class="form-control" OnSelectedIndexChanged="ddlPerameter_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Text" Value="Reminder_text"></asp:ListItem>
                                                    <asp:ListItem Text="Remind To" Value="Remind_to"></asp:ListItem>
                                                    <asp:ListItem Text="Repeat Type" Value="Repeat_type"></asp:ListItem>
                                                    <asp:ListItem Text="Days" Value="Days"></asp:ListItem>
                                                    <asp:ListItem Text="Due Date" Value="Due_date"></asp:ListItem>
                                                    <asp:ListItem Text="Expiry Date" Value="Expiry_date"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption" runat="server" Class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtValue" runat="server" Class="form-control" Visible="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtValueDays" runat="server" Class="form-control" Visible="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filter1" runat="server" TargetControlID="txtValueDays" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtValueDate" runat="server" Class="form-control" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" Format="dd-MMM-yyyy" />
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnSearch" runat="server" CausesValidation="False" Style="margin-top: -5px;" OnClick="btnSearch_Click"
                                                    ImageUrl="~/Images/search.png" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"
                                                    ImageUrl="~/Images/refresh.png" Style="width: 33px;"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <asp:HiddenField ID="hdnURL" runat="server" />

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:HiddenField runat="server" ID="hdntrans_id" />
                                                <asp:HiddenField runat="server" ID="hdnlogTrans_id" />
                                                <asp:HiddenField runat="server" ID="hdndue_date" />

                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvReminderList" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvReminderList_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvReminderList_Sorting" >
                                                        
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Reminder_ID") %>' CommandName='<%# Eval("LogTrans_Id") %>' ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="View"
                                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Reminder_ID") %>' CommandName='<%# Eval("Trans_Id") %>' ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="lnkViewDetail_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnReminderDelete" runat="server" Height="20px" Width="20px" ImageUrl="~/Images/Delete1.png" CausesValidation="False" CommandArgument='<%# Eval("Reminder_ID") %>' OnCommand="btnReminderDelete_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Change State">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnchangeStateTransID" runat="server" Value='<%# Eval("Reminder_ID") %>' />
                                                                    <asp:DropDownList ID="ddlChangeState" runat="server" OnSelectedIndexChanged="ddlChangeState_SelectedIndexChanged1" AutoPostBack="true" SelectedValue='<%# Bind("Status") %>' DataTextField="Status" DataValueField="Status">

                                                                        <asp:ListItem Value="On">On</asp:ListItem>
                                                                        <asp:ListItem Value="Off">Off</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Reminder Text" SortExpression="Reminder_text">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="hypReminder_text" NavigateUrl='<%# Eval("Target_url") %>' runat="server" Text='<%#Eval("Reminder_text") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="15%" />
                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField HeaderText="Remind To" SortExpression="RemindTo_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemindTo_Name" runat="server" Text='<%#Eval("RemindTo_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="8%" />
                                                            </asp:TemplateField>--%>


                                                            <asp:TemplateField HeaderText="Repeat Type" SortExpression="Repeat_type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRepeat_type" runat="server" Text='<%#Eval("Repeat_type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Category" SortExpression="Ref_table_name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRef_table_name" runat="server" Text='<%#Eval("Ref_table_name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Due Date" SortExpression="Due_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDue_date" runat="server" Text='<%#GetDate(Eval("Due_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="4%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expiry Date" SortExpression="Expiry_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpiry_date" runat="server" Text='<%#GetDate(Eval("Expiry_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="4%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Created By" SortExpression="CreatedBy_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreatedBy_Name" runat="server" Text='<%#Eval("CreatedBy_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"  Width="6%" />
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

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblStart_Date" runat="server" Text="Start Date" />
                                                        <asp:TextBox ID="txtStart_Date" runat="server" Class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="calendarShown" ID="Calender" runat="server" TargetControlID="txtStart_Date" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblRepeatType" runat="server" Text="Repeat Type" />
                                                        <asp:DropDownList ID="ddlRepeatType" runat="server" Class="form-control" OnSelectedIndexChanged="ddlRepeatType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="Once"></asp:ListItem>
                                                            <asp:ListItem Text="Daily"></asp:ListItem>
                                                            <asp:ListItem Text="Weekly"></asp:ListItem>
                                                            <asp:ListItem Text="Monthly"></asp:ListItem>
                                                            <asp:ListItem Text="Yearly"></asp:ListItem>
                                                        </asp:DropDownList><br>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblRepeatCount" runat="server" Text="Repeat Count" />
                                                        <asp:TextBox ID="txtRepeatCount" runat="server" Class="form-control" MaxLength="2" Enabled="false" OnTextChanged="txtRepeatCount_TextChanged" AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRepeatCount" FilterType="Numbers"></cc1:FilteredTextBoxExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblExpiry_Date" runat="server" Text="Due Date" />
                                                        <asp:TextBox ID="txtExpiry_Date" runat="server" Class="form-control" OnTextChanged="txtExpiry_Date_TextChanged" AutoPostBack="true" />
                                                        <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender1" runat="server" TargetControlID="txtExpiry_Date" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-8">

                                                        <asp:Label ID="lblRemindTo" runat="server" Text="Remind To" />
                                                        <br />
                                                        <asp:TextBox ID="txtRemindTo" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="false" />
                                                        <%--<cc1:AutoCompleteExtender ID="txtTablename_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                                            ServicePath="" TargetControlID="txtRemindTo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
			                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>      --%>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblStatus" runat="server" Text="Status" />
                                                        <asp:DropDownList ID="ddlStatus" runat="server" Class="form-control">

                                                            <asp:ListItem Text="On" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Off"></asp:ListItem>

                                                        </asp:DropDownList><br>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <br />
                                                        <asp:CheckBox ID="chkEmail" runat="server" Text="Notify by Email" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <br />
                                                        <asp:CheckBox ID="chkSMS" runat="server" Text="Notify by SMS" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <br />
                                                        <asp:CheckBox ID="chkNotification" runat="server" Text="Notify by Notification" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblReminderText" runat="server" Text="Reminder Text" />
                                                        <asp:TextBox ID="txtReminderText" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" runat="server" Class="form-control" TextMode="MultiLine"
                                                            Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Panel ID="PnlOperationButton" runat="server">

                                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />

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


                </div>
            </div>
        </div>
    </div>



    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_List">
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





    <div class="modal fade" id="Reminder123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">

                    <uc1:Reminder1 runat="server" ID="Reminder" />

                </div>
                <%--<div class="modal-footer">
                   <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>--%>
            </div>
        </div>
    </div>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>

    <script>
      
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

        function Modal_Open_Reminder() {
            document.getElementById('<%= Btn_Modal_Reminder.ClientID %>').click();
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

        function Li_Tab_New() {
            document.getElementById('<%= Btn_Modal_Reminder.ClientID %>').click();

        }

        function recordSave() {
            alert("Record Saved Successfully !!!");
        }


    </script>
</asp:Content>

