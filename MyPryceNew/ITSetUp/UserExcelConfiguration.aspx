<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="UserExcelConfiguration.aspx.cs" Inherits="ITSetUp_UserExcelConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,User Excel Configuration%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,User Excel Configuration%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="tab-pane" id="New">
                <asp:UpdatePanel ID="Update_New" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-primary">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label7" runat="server" Text="Object Name"></asp:Label>
                                                <asp:DropDownList ID="ddlObjectname" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlObjectname_OnSelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label2" runat="server" Text="Operation Type"></asp:Label>
                                                <asp:DropDownList ID="ddlOperationType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Insert" Value="Insert"></asp:ListItem>
                                                    <asp:ListItem Text="Display" Value="Display"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label3" runat="server" Text="Error Action"></asp:Label>
                                                <asp:DropDownList ID="ddlConsistency" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Skip" Value="Skip"></asp:ListItem>
                                                    <asp:ListItem Text="Rollback" Value="Rollback"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label20" runat="server" Text="Consistency Exception Action"></asp:Label>
                                                <asp:DropDownList ID="ddlForeingExcetion" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Insert" Value="Insert"></asp:ListItem>
                                                    <asp:ListItem Text="Skip" Value="Skip" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Rollback" Value="Rollback"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label1" runat="server" Text="Table Name"></asp:Label>
                                                <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlTableName_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <br />
                                                <asp:Button ID="btnDefaultSetting" runat="server" CssClass="btn btn-primary" Text="Default Setting" Width="150px" OnClick="btnDefaultSetting_OnClick" />
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <hr />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label4" runat="server" Text="Field name"></asp:Label>
                                                <asp:DropDownList ID="ddlFieldname" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldname_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label8" runat="server" Text="Field Caption"></asp:Label>
                                                <asp:TextBox ID="txtFieldCaption" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label9" runat="server" Text="Sorting Order"></asp:Label>
                                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align:center">
                                                <div class="col-md-2">
                                                    <asp:CheckBox ID="chkIsRequired" runat="server" Text="Is Required" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:CheckBox ID="chkIsVisible" runat="server" Text="Is Visible" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:CheckBox ID="chkIsDuplicaate" runat="server" Text="Is Duplicate" />
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="text-align: center">
                                                <br />
                                                <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnsave_OnClick" />
                                                <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnreset_OnClick" />
                                                <asp:Button ID="btnDemo" runat="server" CssClass="btn btn-primary" Text="Demo data" OnClick="btnDemo_OnClick" />
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <hr />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label11" runat="server" Text="Upload Excel File"></asp:Label>
                                                <div class="input-group" style="width: 100%;">
                                                    <cc1:AsyncFileUpload ID="fileLoad"
                                                        OnClientUploadStarted="FUExcel_UploadStarted"
                                                        OnClientUploadError="FUExcel_UploadError"
                                                        OnClientUploadComplete="FUExcel_UploadComplete"
                                                        OnUploadedComplete="FUExcel_FileUploadComplete"
                                                        runat="server" CssClass="form-control"
                                                        CompleteBackColor="White"
                                                        UploaderStyle="Traditional"
                                                        UploadingBackColor="#CCFFFF"
                                                        ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                        <asp:Image ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                        <asp:Image ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                        <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <br />
                                                <asp:Button ID="btnUpload" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                    OnClick="btnUpload_Click" Text="<%$ Resources:Attendance,Upload %>" />
                                                <asp:Button ID="btnFinalSheet" runat="server" Text="Get Final Sheet For Insert" Width="200px" OnClick="btnFinalSheet_click" CssClass="btn btn-primary" />
                                                <asp:Button ID="btnInsert" runat="server" Text="Insert Record" Width="200px" OnClick="btnInsert_click" CssClass="btn btn-primary" />
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align: center;">
                                                <br />
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridView1" runat="server" Width="100%" >
                                                        
                                                        
                                                        
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align: center">
                                                <br />
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExcelConfig" runat="server" Width="100%" >
                                                        
                                                        
                                                        
                                                    </asp:GridView>
                                                </div>
                                                <br />
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

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
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

        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function FUExcel_UploadStarted(sender, args) {
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
