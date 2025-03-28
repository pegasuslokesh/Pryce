<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucControlsSetting.ascx.cs" Inherits="WebUserControl_ucControlsSetting" %>
<script type="text/javascript">
    function onChkCtlVisibilityClick(chk) {
        if (!chk.checked) {
            var td = $(chk).parent();
            var chkMandatory = $(td).find('input[id = chkCtlMandatory]');
            $(chkMandatory).prop("checked", false)
        }
    }

    function onChkCtlMandatoryClick(chk) {
        var td = $(chk).parent();
        var chkVisibility = $(td).find('input[id = chkCtlVisibility]');
        if (!(chkVisibility).is(':checked')) {
            $(chk).prop("checked", false)
        }
    }

    function hidePopup() {
        $('#ControlSettingModal').modal('hide');
    }
</script>
<div class="row">
    <asp:UpdatePanel ID="uPnl" runat="server">
        <ContentTemplate>
            <div class="col-md-12" class="flow">
                <asp:HiddenField ID="hdnPageName" Value="" runat="server" />
                <asp:HiddenField ID="hdnGrdName" Value="" runat="server" />
                <asp:DataList ID="dlGrdCols" runat="server" Visible="false" RepeatColumns="4" RepeatDirection="Vertical" Width="100%">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnColIndex" Value='<%# Eval("ControlId").ToString() %>' runat="server" />
                        <asp:CheckBox ID="chkGrdColVisibility" runat="server" Text='<%# Eval("Caption").ToString() %>' Checked='<%# Eval("IsVisible") %>' />
                        <br />
                    </ItemTemplate>
                </asp:DataList>
                <asp:DataList ID="dlPageControls" runat="server" Visible="false" RepeatColumns="3" RepeatDirection="Vertical" Width="100%">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnCtlName" Value='<%# Eval("ControlId").ToString() %>' runat="server" />
                        <asp:Label ID="lblCtlCaption" Text='<%# Eval("Caption").ToString() %>' runat="server" />
                        <br />
                        <asp:CheckBox ID="chkCtlVisibility" runat="server" Text="Visible" Checked='<%# Eval("IsVisible") %>' ClientIDMode="Static" onclick="onChkCtlVisibilityClick(this)" />
                        <asp:HiddenField ID="hdnCtlContainer" Value='<%# Eval("ContainerId").ToString() %>' runat="server" />
                        <br />
                        <asp:CheckBox ID="chkCtlMandatory" runat="server" Text="Mandatory" Checked='<%# Eval("IsMandatory") %>' ClientIDMode="Static" onclick="onChkCtlMandatoryClick(this)" Visible='<%# Eval("MandatoryControlId").ToString() == string.Empty ? false : true %>' />
                        <asp:HiddenField ID="hdnMandatoryControlId" Value='<%# Eval("MandatoryControlId").ToString() %>' runat="server" />
                        <br />
                    </ItemTemplate>
                </asp:DataList>
            </div>

            <div class="col-md-12">
                <br />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="uPnl">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</div>


