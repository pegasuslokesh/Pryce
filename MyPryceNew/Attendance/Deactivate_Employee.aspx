<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Deactivate_Employee.aspx.cs" Inherits="Attendance_Deactivate_Employee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/log_monitoring.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Deactivate Employee"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Deactivate Employee"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Page" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">

                                <div class="col-md-12" style="text-align: center;">

                                    <br />
                                    <asp:HyperLink ID="uploadEmpInfo" runat="server" Font-Bold="true" Font-Size="15px"
                                        NavigateUrl="~/CompanyResource/Deactivate_Employee.xlsx" Text="Download sample format for update information" Font-Underline="true"></asp:HyperLink>
                                    <br />

                                </div>

                                <div class="col-md-6" style="text-align: center;">
                                    <br />
                                    <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                    <div class="input-group" style="width: 100%;">
                                        <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                            runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
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
                                    </br>
                                                            
                                </div>
                                 <div class="col-md-12" style="text-align: right;" id="div_Record_count" runat="server" visible="false">
                                    <br />
                                    <asp:Label ID="lblTotalrecord" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                              
                                <div class="col-md-12">
                                    <br />

                                    <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                        ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                        ForeColor="Red"></asp:CustomValidator>

                                    <div style="overflow: auto; max-height: 300px;">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpList" runat="server" Width="100%"  >

                                              
                                            <PagerStyle CssClass="pagination-ys" />
                                            
                                        </asp:GridView>
                                    </div>
                                    <br />
                                </div>
                                  <div class="col-md-12" id="div_upload_button" style="text-align: center;" runat="server" visible="false">
                                    <br />
                                    <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" ValidationGroup ="Save"    
                                        OnClick="btnUpdate_Click" Visible="false" Text="Update" />

                                       <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to update record in database ?"
                                                                        TargetControlID="btnUpdate">
                                                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="btnDownload" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                        OnClick="btnDownload_Click" Visible="true" Text="Download" />
                                    <asp:Button ID="btnReset" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                        OnClick="btnReset_Click" Visible="true" Text="Reset" />


                                </div>
                               

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Page">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>


    <script type="text/javascript">

        function selectAllCheckbox_Click(id) {

            var gridView = document.getElementById('<%=gvEmpList.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                cell.getElementsByTagName("INPUT")[0].checked = id.checked;
            }
            //SetEmpList();
            //SelectedEmpCount();
        }


         function Grid_Validate(sender, args) {

            var gridView = document.getElementById("<%=gvEmpList.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }



        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function uploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
    </script>





</asp:Content>

