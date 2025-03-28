<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_Indemnity_Master.aspx.cs" Inherits="HR_HR_Indemnity_Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-briefcase"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Indemnity Master %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,HR Indemnity Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div id="dvNew" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblIndemnityYear" runat="server" Text="<%$ Resources:Attendance,Indemnity Date%>"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtIndemnityDate" ErrorMessage="<%$ Resources:Attendance,Enter Indemnity Date %>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtIndemnityDate" runat="server" CssClass="form-control" />
                                        <cc1:CalendarExtender ID="txtIndemnityDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtIndemnityDate" OnClientShown="showCalendar">
                                        </cc1:CalendarExtender>
                                        <asp:HiddenField ID="editid" runat="server" />
                                    </div>
                                    <div class="col-md-6">
                                        <br />
                                        <asp:Button ID="btnIndemnityUpdate" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnIndemnityUpdate_Click" Visible="false" CssClass="btn btn-success" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Indemnity Id%>" Value="Indemnity_Id"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Name%>" Value="Emp_Name"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Indemnity Date%>" Value="Indemnity_Date"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-3">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                    OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning box-solid" <%= gvIndemnityMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>  >

                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:HiddenField ID="HDFSort" runat="server" />
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvIndemnityMaster" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                    runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                    OnPageIndexChanging="gvIndemnityMaster_PageIndexChanging" OnSorting="gvIndemnityMaster_OnSorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Indemnity_Id") %>'
                                                    ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnEdit_Command"
                                                    Visible="true" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemStyle />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Indemnity Id%>" SortExpression="Indemnity_Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndemnity_Id" runat="server" Text='<%# Eval("Indemnity_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Indemnity Date%>" SortExpression="Indemnity_Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndemnityDate" runat="server" Text='<%# GetDate(Eval("Indemnity_Date")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                    </Columns>


                                    <PagerStyle CssClass="pagination-ys" />

                                </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>
                <!-- /.box-body -->
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:Panel ID="dvList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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

