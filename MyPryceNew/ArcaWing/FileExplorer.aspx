<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileExplorer.aspx.cs" Inherits="FileExplorer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <link href="CSS/InvStyle.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
   

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

       <Triggers>
            <asp:PostBackTrigger ControlID="ASPxFileManager1" />
            </Triggers>

      <ContentTemplate>
    <div    style="background-color:#ccddee;">
  

  <%--<table>
   <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblDocName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Document Name %>"></asp:Label>
                                                </td>
                                                <td width="1px">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:TextBox ID="txtDocumentName" BackColor="#eeeeee" Width="400px" runat="server"
                                                        CssClass="textComman" AutoPostBack="true" OnTextChanged="txtDocumentName_OnTextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListDocName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDocumentName"
                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdndocumentid" runat="server" Value="0" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
  
  </table>--%>


   
  
    <dx:ASPxFileManager ID="ASPxFileManager1"  runat="server" Height="600px"  


   


        Width="100%"    OnFolderCreating="ASPxFileManager1_OnFolderCreating"  
            OnFileUploading="ASPxFileManager1_OnFileUploading"      OnItemDeleting="ASPxFileManager1_OnItemDeleting" OnItemRenaming="ASPxFileManager1_OnItemRenaming" OnItemMoving="ASPxFileManager1_OnItemMoving" 
           
        >
        <Settings RootFolder="~\ArcaWing\"   ThumbnailFolder="~\ArcaWing\Thumbnail\"  EnableMultiSelect="true"  
            InitialFolder="~\ArcaWing\" AllowedFileExtensions=".docx,.odt,.pdf,.rtf,.tex,.txt,.wks,.wpd,.jpeg,.jpg,.png,.html,.gif,.xlsx,.xls,.rar" />
        <SettingsEditing  AllowCopy="false" AllowCreate="false" AllowDelete="True"  
            AllowMove="false" AllowRename="false" AllowDownload="true"    />
        <SettingsToolbar    ShowDownloadButton="True" ShowMoveButton="true"  ShowPath="true" ShowCreateButton="false" ShowDeleteButton="false"   />

      <SettingsUpload Enabled="false"></SettingsUpload>
       <SettingsFileList View="Details">
      
            <DetailsViewSettings  AllowColumnResize="true" ThumbnailSize="100px" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false"    />
        </SettingsFileList>
    
    </dx:ASPxFileManager>
    
      </div>
      </ContentTemplate>
      </asp:UpdatePanel>
    </form>
</body>
</html>
