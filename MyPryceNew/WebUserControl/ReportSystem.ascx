<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportSystem.ascx.cs" Inherits="WebUserControl_ReportSystem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:HiddenField runat="server" ID="hdnTransId" />
<asp:HiddenField runat="server" ID="hdnLocId" />
<asp:HiddenField runat="server" ID="hdnPageRef" />
<asp:HiddenField runat="server" ID="hdnProductCode" />
<asp:UpdatePanel runat="server" ID="asd">
    <ContentTemplate>
        <div class="col-md-12">
            <br />
            <table id="tblReport" class="table table-striped" style="width: 100%">
                <thead>
                    <tr>
                        <td>Report ID</td>
                        <td>Report Name</td>
                        <td>Is Default</td>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>

        <div class="col-md-2">
            <br />
            <asp:Button ID="btnOpenReport" runat="server" Text="Open Report" CssClass="btn btn-primary" OnClientClick="btnOpenReport();return false;" />
            <br />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>


<script>
    function showMsg(data) {
        alert(data);
        return;
    }
 
    function btnOpenReport()
    {
        var reportId = '';
        $('#tblReport tbody tr').each(function () {
            var row = $(this)[0];
            if(row.cells[2].childNodes[0].checked)
            {
                reportId = row.cells[0].innerHTML;
            }
        });

        if (reportId == '')
        {
            alert('Please select a default report to continue');
            return;
        }
        else
        {
            var transId = document.getElementById('<%= hdnTransId.ClientID %>').value;
            var LocId = document.getElementById('<%= hdnLocId.ClientID %>').value;
            var PageRef ="";
            try{
                var PageRef =  document.getElementById('<%= hdnPageRef.ClientID %>').value;
            }
            catch
            {

            }
            var ProductId ="";
            try{
                var ProductId =  document.getElementById('<%= hdnProductCode.ClientID %>').value;
              
            }
            catch
            {

            }
            window.open('../utility/reportViewer.aspx?reportId=' + reportId + '&t=' + transId + '&l=' + LocId + '&PageRef=' + PageRef + '&ProductId=' + ProductId + '');
        }
        
    }
</script>
