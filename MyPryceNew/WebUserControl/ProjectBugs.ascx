<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectBugs.ascx.cs" Inherits="WebUserControl_ProjectBugs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc11" %>



<asp:HiddenField runat="server" ID="hdnFeedbackTaskId" />
<asp:HiddenField runat="server" ID="hdnFeedbackId" />
<asp:HiddenField runat="server" ID="hdnRowNo" />
<asp:HiddenField runat="server" ID="hdnProjectId" />
<asp:HiddenField runat="server" ID="hdnAssignById" ClientIDMode="Static" />

<asp:UpdatePanel runat="server" ID="asd">
    <ContentTemplate>
        <div class="col-md-6">
            <asp:Label runat="server" ID="lblFeedback" Text="Project Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtProjectName" CssClass="form-control" onchange="txtProjectName_Change(this);"></asp:TextBox>
            <cc11:AutoCompleteExtender ID="AutoCompleteExtender_ProjectList" runat="server" DelimiterCharacters=""
                Enabled="True" ServiceMethod="GetCompletionListProject" ServicePath="../WebServices/projectManagement.asmx" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProjectName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="positionReset">
            </cc11:AutoCompleteExtender>
            <br />
        </div>
        <div class="col-md-6">
            <asp:Label runat="server" ID="lblBugName" Text="Bug Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtBugName" CssClass="form-control"></asp:TextBox>
            <br />
        </div>
        <div class="col-md-6">
            <asp:Label runat="server" ID="lblAssignBy" Text="Assigned By"></asp:Label>
            <asp:TextBox runat="server" ID="txtAssignedBy" ClientIDMode="Static" CssClass="form-control" onchange="checkEmployee(this)"></asp:TextBox>
            <cc11:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionListAssignBy" ServicePath="../WebServices/projectManagement.asmx" CompletionInterval="100"
                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAssignedBy"
                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="positionReset">
            </cc11:AutoCompleteExtender>
            <br />
        </div>
        <div class="col-md-6">
            <asp:Label runat="server" ID="lblPriority" Text="Priority"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlPriority" CssClass="form-control">
                <asp:ListItem Text="None" Value="None"></asp:ListItem>
                <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                <asp:ListItem Text="High" Value="High"></asp:ListItem>
            </asp:DropDownList>
            <br />
        </div>
        <div class="col-md-12">
            <asp:Label runat="server" ID="lblProblem" Text="Problem"></asp:Label>
            <asp:TextBox runat="server" ID="txtProblem" TextMode="MultiLine" Width="100%" Style="max-height: 300px; min-height: 80px"></asp:TextBox>
            <br />
        </div>

        <div class="col-md-2">
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="btnSave();return false;" />
            <br />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

    <div id="progressBar" class="modal_Progress" style="display: none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
        </div>
    </div>

<script src="../Script/employee.js"></script>

<script>

    function positionReset(object, args) {
        var tbposition = findPositionWithScrolling1(100004);
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

    function showMsg(data) {
        alert(data);
        return;
    }

    function btnSave() {
        var projectId = document.getElementById('<%= hdnProjectId.ClientID %>').value;
        var projectName = document.getElementById('<%= txtProjectName.ClientID %>');
        var bugName = document.getElementById('<%= txtBugName.ClientID %>').value;
        var problem = document.getElementById('<%= txtProblem.ClientID %>').value;
        var priority = document.getElementById('<%= ddlPriority.ClientID %>').value;
        var assignById = document.getElementById('<%= hdnAssignById.ClientID %>').value;
        var assignByName = document.getElementById('<%= txtAssignedBy.ClientID %>');

        if (projectId.trim() == "") {
            showMsg("Please Enter Project");
            projectName.value = "";
            return;
        }
        if (assignById.trim() == "") {
            showMsg("Please Enter Assigned By Name");
            assignByName.value = "";
            return;
        }
        if (bugName.trim() == "") {
            showMsg("Please Enter Bug Name");
            return;
        }
        if (problem.trim() == "") {
            showMsg("Please Enter Problem");
            return;
        }
        var bugdata = new Object();
        bugdata.projectId = projectId;
        bugdata.bugName = bugName;
        bugdata.description = problem;
        bugdata.priority = priority;
        bugdata.assignedBy = assignById;
        $("#progressBar").css("display", "block");
        $.ajax({
            url: '../WebServices/projectManagement.asmx/saveNewBug',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'pbData':" + JSON.stringify(bugdata) + "}",
            success: function (data) {
                var hdnProjectId = document.getElementById('<%= hdnProjectId.ClientID%>');
                if (data.d == "0" && data.d == "") {
                    showMsg('Cant Save this bug, please try after some time');
                    reset();
                }
                else {
                    showMsg('Bug Saved Successfully');
                    reset();
                }
                $("#progressBar").css("display", "none");
            }
        });
    }

    function txtProjectName_Change(ctrl) {

        if (ctrl.value == "") {
            document.getElementById('<%= hdnProjectId.ClientID%>').value = "";
            return;
        }
        $("#progressBar").css("display", "block");
        $.ajax({
            url: '../WebServices/projectManagement.asmx/txtProjectName_TextChanged',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'projectName':'" + ctrl.value + "'}",
            success: function (data) {
                var hdnProjectId = document.getElementById('<%= hdnProjectId.ClientID%>');
                if (data.d == "0") {
                    alert('Please select from suggestions');
                    hdnProjectId.value = "0";
                    document.getElementById('<%= txtProjectName.ClientID%>').value = "";
                }
                else {
                    hdnProjectId.value = data.d;
                }
                $("#progressBar").css("display", "none");
            }
        });
    }
    function checkEmployee(ctrl) {
        var empId = document.getElementById('<%= hdnAssignById.ClientID%>');
        if (ctrl.value == "") {
            empId.value = "";
            return;
        }
        validateEmployeeNSetValue(ctrl, empId);
    }
    function reset()
    {
        document.getElementById('<%= hdnProjectId.ClientID %>').value="";
        document.getElementById('<%= txtProjectName.ClientID %>').value = "";
        document.getElementById('<%= txtBugName.ClientID %>').value = "";
        document.getElementById('<%= txtProblem.ClientID %>').value = "";
        document.getElementById('<%= ddlPriority.ClientID %>').value = "";
    }
    function setDefaultEmpName()
    {
         $.ajax({
             url: '../WebServices/employee.asmx/getDefaultUserName',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                document.getElementById('<%= txtAssignedBy.ClientID %>').value = data.d;                
            }
        });
    }
</script>
