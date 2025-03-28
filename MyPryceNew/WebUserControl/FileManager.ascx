<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileManager.ascx.cs" Inherits="WebUserControl_FileManager" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




   <%--   <asp:UpdatePanel ID="UpdatePanel_UC" runat="server" UpdateMode="Conditional">

       <Triggers>
            <asp:PostBackTrigger ControlID="ASPxFileManager1" />
            </Triggers>

      <ContentTemplate>--%>
    

   
  <asp:HiddenField runat="server" id="imgUrl" ClientIDMode="Static" />
    <dx:ASPxFileManager ID="ASPxFileManager1" ClientInstanceName="fileManager" runat="server" 
        onselectedfileopened="ASPxFileManager1_SelectedFileOpened" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"   >
      
        <SettingsEditing AllowCopy="True" AllowCreate="True" AllowDelete="True" 
            AllowMove="True" AllowRename="True" AllowDownload="true"   />
        <SettingsToolbar ShowDownloadButton="True" ShowMoveButton="true" ShowPath="true"  />
       <SettingsFileList View="Details">
            <DetailsViewSettings AllowColumnResize="true" ThumbnailSize="100px" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false"  />
        </SettingsFileList>
        <ClientSideEvents SelectedFileOpened="function(s, e) {
            popup.Hide();
        }" />
    </dx:ASPxFileManager>

<script>
    function setImageUrlUsingAspxFileManager()
    {
        alert(document.getElementById('imgUrl').value);
        var imgCtl = document.getElementById('imgProduct');
        imgCtl.src = document.getElementById('imgUrl').value;
        alert(imgCtl.src);
        }
</script>
     
<%--      </ContentTemplate>
      </asp:UpdatePanel>
    --%>