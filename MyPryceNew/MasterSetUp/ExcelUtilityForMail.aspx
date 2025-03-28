<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ExcelUtilityForMail.aspx.cs" Inherits="MasterSetUp_ExcelUtilityForMail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row" id="Div_device_upload_operation" runat="server">
        
        <div class="col-md-6" style="text-align: center;">
            <br />
            <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
            <div class="input-group" style="width: 100%;">
                <cc1:asyncfileupload id="fileLoad" onuploadedcomplete="FileUploadComplete" onclientuploaderror="uploadError" onclientuploadstarted="uploadStarted" onclientuploadcomplete="uploadComplete"
                    runat="server" cssclass="form-control" completebackcolor="White" uploaderstyle="Traditional" uploadingbackcolor="#CCFFFF" throbberid="imgLoader" width="100%" />
                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                    <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                    <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                </div>

            </div>
            <br />

            <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

        </div>
        <div class="col-md-6" style="text-align: center;">
            <br />
            <asp:Label runat="server" Text="Select Sheet" ID="Label170"></asp:Label>
            <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
            </asp:DropDownList>
            <br />

          
            <asp:Button ID="Button6" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                OnClick="btnConnect_Click" Visible="true" Text="GetRecord" />

            
                                                             <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
</asp:Content>

