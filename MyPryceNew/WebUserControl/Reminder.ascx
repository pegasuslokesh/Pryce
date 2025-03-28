<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reminder.ascx.cs" Inherits="WebUserControl_Reminder" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="tc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="tc1" %>

<asp:HiddenField ID="hdnTableName" runat="server" />

<%--<asp:UpdatePanel ID="Update_Button" runat="server">
    <ContentTemplate>

       <asp:Button ID="Btn_ReminderList1" Style="display: none;" runat="server" Text="ReminderList" OnClick="Btn_ReminderList1_Click" />
        <asp:Button ID="Btn_New1" Style="display: none;" runat="server" Text="New" OnClick="Btn_New1_Click" />
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
</asp:UpdateProgress>--%>

<div class="row">

    <asp:UpdatePanel ID="Update_Header" runat="server">
        <ContentTemplate>

            <div class="col-lg-6" style="max-width: 100%;">
                <h3>
                    <i class="fa fa-clock-o"></i>
                    <asp:Label ID="lblHeaderL" runat="server" Text="Reminder"></asp:Label>
                </h3>
            </div>
            <div class="col-lg-6" style="text-align: right; margin-top: 12px; max-width: 100%;">
                <h4>
                    <asp:Label ID="lblHeaderR" runat="server"></asp:Label>
                </h4>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="RemID" runat="server" />

</div>

<asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Header">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<div class="tab-content">


    <div class="tab-pane active" id="NewReminder1">
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
                                        <tc1:CalendarExtender OnClientShown="calendarShown" ID="Calender" runat="server" TargetControlID="txtStart_Date" Format="dd-MMM-yyyy" />
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
                                        <asp:TextBox ID="txtRepeatCount" runat="server" Class="form-control" Enabled="false" MaxLength="2" OnTextChanged="txtRepeatCount_TextChanged" AutoPostBack="true" />
                                        <tc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRepeatCount" FilterType="Numbers"> </tc1:FilteredTextBoxExtender>
                                               
                                        <br />
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Label ID="lblExpiry_Date" runat="server" Text="Due Date" />
                                        <asp:TextBox ID="txtExpiry_Date" runat="server" Class="form-control" OnTextChanged="txtExpiry_Date_TextChanged" AutoPostBack="true" />
                                        <tc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender1" runat="server" TargetControlID="txtExpiry_Date" Format="dd-MMM-yyyy" />
                                        <br />
                                    </div>


                                    <div class="col-md-8">

                                        <asp:Label ID="lblRemindTo" runat="server" Text="Remind To" />
                                        <br />
                                        <asp:TextBox ID="txtRemindTo" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                        <tc1:AutoCompleteExtender ID="txtTablename_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                            ServicePath="" TargetControlID="txtRemindTo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </tc1:AutoCompleteExtender>
                                        <br />
                                    </div>




                                    <div class="col-md-12" style="overflow: auto; max-height: 250px">
                                        <div class="progress-group">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvRemindTo" runat="server" DataKeyNames="contact_id"
                                                AutoGenerateColumns="False" Width="100%">
                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete%>">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="IBtn_Delete_Duty" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Emp_Id") %>' ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remind To Employee">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblRemindToEmp" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>

                                                </Columns>
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                        </div>
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
                                        <asp:CheckBox ID="chkEmail" runat="server" Text="Notify by Email" Enabled="false"/>
                                        <br />
                                    </div>
                                    <div class="col-md-3">
                                        <br />
                                        <asp:CheckBox ID="chkSMS" runat="server" Text="Notify by SMS" Enabled="false"/>
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

                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClientClick="this.disabled='true'; this.value='please wait..';"  UseSubmitBehavior="false" Text="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnReset_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="disableModle();" />
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

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
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
    function LI_New_ActivePopup() {
        $("#Li_ReminderList1").removeClass("active");
        $("#ReminderList1").removeClass("active");

        $("#Li_NewReminder1").addClass("active");
        $("#NewReminder1").addClass("active");
    }

    function LI_List_ActivePopup() {
        $("#Li_ReminderList1").addClass("active");
        $("#ReminderList1").addClass("active");

        $("#Li_NewReminder1").removeClass("active");
        $("#NewReminder1").removeClass("active");
    }



<%--    function Li_Tab_ReminderList1() {
        document.getElementById('<%= Btn_ReminderList1.ClientID %>').click();
}
function Li_Tab_New1() {
    document.getElementById('<%= Btn_New1.ClientID %>').click();
}--%>


    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }


    function AddContact() {
        window.open("../EMS/ContactMaster.aspx?Page=SINQ");
    }


    function CustomerHistory(id) {
        window.open("../Purchase/CustomerHistory.aspx?ContactId=" + id + "&&Page=SINQ");
    }

    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }

    function disableModle()
    {
        this.Modal_Open_Reminder();
    }
   
    <%-- function FUAll_UploadStartedReminder(sender, args) {

    }
    function FUAll_UploadCompleteReminder(sender, args) {
        document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
        document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
    }
        function FUAll_UploadErrorReminder(sender, args) {
        document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
        document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
    }--%>
</script>



