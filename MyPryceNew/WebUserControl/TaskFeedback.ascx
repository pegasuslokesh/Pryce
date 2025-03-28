<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskFeedback.ascx.cs" Inherits="WebUserControl_TaskFeedback" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:HiddenField runat="server" ID="hdnFeedbackTaskId" />
<asp:HiddenField runat="server" ID="hdnFeedbackId" />
<asp:HiddenField runat="server" ID="hdnRowNo" />

<asp:UpdatePanel runat="server" ID="asd">
    <ContentTemplate>
        <div class="col-md-12">
            <asp:Label runat="server" ID="lblFeedback" Text="Enter Feedback"></asp:Label>
            <asp:TextBox runat="server" ID="Editor1" TextMode="MultiLine" Width="100%" Style="max-height: 300px; min-height: 80px"></asp:TextBox>
            <br />
        </div>

        <div class="col-md-4">
            <asp:Label ID="lblTaskCompletion1" runat="server" Text="Task Completion (%)"></asp:Label>
            <asp:DropDownList ID="ddlCompleted" runat="server" CssClass="form-control">
                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                <asp:ListItem Text="70" Value="70"></asp:ListItem>
                <asp:ListItem Text="80" Value="80"></asp:ListItem>
                <asp:ListItem Text="90" Value="90"></asp:ListItem>
                <asp:ListItem Text="100" Value="100"></asp:ListItem>
            </asp:DropDownList>
            <br />
        </div>

        <div class="col-md-2">
            <br />
            <asp:Button ID="btnStop" runat="server" Text="Stop Task" CssClass="btn btn-primary" OnClientClick="btnStop();return false;" />
            <br />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>


<script>
    function showMsg(data) {
        alert(data);
        return;
    }
    function btnStop() {
        
        var complete = document.getElementById('<%= ddlCompleted.ClientID %>');        
        var feedbackId = document.getElementById('<%= hdnFeedbackId.ClientID %>');
        var taskId = document.getElementById('<%= hdnFeedbackTaskId.ClientID %>');
        var row = document.getElementById('<%= hdnRowNo.ClientID %>');
        var feedback = $('#<%= Editor1.ClientID %>')[0].value;
        if (feedback.trim() == "") {
            showMsg("Please Enter Feedback");
            return;
        }
        btnStop_Click(row.value, feedbackId.value, taskId.value, feedback, complete.value);
        //$('#<%= Editor1.ClientID %>')[0].value = "";
    }
</script>
