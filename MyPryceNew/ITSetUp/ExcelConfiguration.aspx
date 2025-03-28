<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ExcelConfiguration.aspx.cs" Inherits="ITSetUp_ExcelConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Excel Configuration %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Excel Configuration%>"></asp:Label></li>
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
                                                <asp:DropDownList ID="ddlTableName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTableName_OnSelectedIndexChanged">
                                                </asp:DropDownList>
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
                                            <div class="col-md-6">
                                                <asp:Label ID="Label8" runat="server" Text="Field Type"></asp:Label>
                                                <asp:DropDownList ID="ddlFieldType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="String" Value="String"></asp:ListItem>
                                                    <asp:ListItem Text="Boolean" Value="Boolean"></asp:ListItem>
                                                    <asp:ListItem Text="DateTime" Value="DateTime"></asp:ListItem>
                                                    <asp:ListItem Text="Date" Value="Date"></asp:ListItem>
                                                    <asp:ListItem Text="Number" Value="Number"></asp:ListItem>
                                                    <asp:ListItem Text="Decimal" Value="Decimal"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label9" runat="server" Text="Field Caption"></asp:Label>
                                                <asp:TextBox ID="txtFieldCaption" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label11" runat="server" Text="Default Value"></asp:Label>
                                                <asp:TextBox ID="txtdefaultValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label12" runat="server" Text="Field Name Required"></asp:Label>
                                                <asp:CheckBox ID="chkIsRequired" Text="Is Required" CssClass="form-control" runat="server" />
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label13" runat="server" Text="Sorting Order"></asp:Label>
                                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label14" runat="server" Text="Foreign Key"></asp:Label>
                                                <asp:CheckBox ID="chkIsForeignKey" Text="Is Foreign Key" CssClass="form-control" runat="server" OnCheckedChanged="chkIsForeignKey_OnCheckedChanged" AutoPostBack="true" />
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label15" runat="server" Text="Foreign Table"></asp:Label>
                                                <asp:DropDownList ID="ddlForeignTable" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlForeignTable_OnSelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>


                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label16" runat="server" Text="Foreign Key Field"></asp:Label>
                                                <asp:DropDownList ID="ddlForeignKeyField" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label17" runat="server" Text="Foreign value Field"></asp:Label>
                                                <asp:DropDownList ID="ddlForeignValueField" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label18" runat="server" Text="Source and Variable Auto Insert"></asp:Label>
                                                <asp:CheckBox ID="chkAutoInsert" Text="Is Auto Insert" CssClass="form-control" runat="server" AutoPostBack="true" OnCheckedChanged="chkAutoInsert_OnCheckedChanged" />
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label19" runat="server" Text="Source and Variable"></asp:Label>
                                                <div style="width: 100%%;">
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:DropDownList ID="ddlautoupdatesorce" runat="server" CssClass="form-control" Width="100%" Enabled="false">
                                                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="Session" Value="Session"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div class="input-group-btn" style="width: 50%;">
                                                            <asp:DropDownList ID="ddlautoupdateVariable" runat="server" CssClass="form-control" Width="100%" Enabled="false">
                                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="CompId" Value="CompId"></asp:ListItem>
                                                                <asp:ListItem Text="BrandId" Value="BrandId"></asp:ListItem>
                                                                <asp:ListItem Text="LocId" Value="LocId"></asp:ListItem>
                                                                <asp:ListItem Text="UserId" Value="UserId"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label21" runat="server" Text="Visible"></asp:Label>
                                                <asp:CheckBox ID="chkIsVisible" Text="Is Visible" CssClass="form-control" runat="server" />
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label22" runat="server" Text="Suggested value"></asp:Label>
                                                <asp:TextBox ID="txtSuggestedValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label23" runat="server" Text="Duplicate"></asp:Label>
                                                <asp:CheckBox ID="chkIsDuplicaate" Text="Is Duplicate" CssClass="form-control" runat="server" Checked="true" />
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align: center">
                                                <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnsave_OnClick" />
                                                <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnreset_OnClick" />
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExcelConfig" runat="server" Width="100%" >
                                                        
                                                        
                                                        
                                                    </asp:GridView>
                                                </div>
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
    </script>
</asp:Content>
