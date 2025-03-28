<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="EmployeeRejoining.aspx.cs" Inherits="HR_EmployeeRejoining" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-check"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Rejoining Detail %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Rejoining%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div id="dvNew" runat="server" visible="true">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator1" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtEmpName" errormessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                            AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblRejoiningDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Rejoin Date%>"></asp:Label>
                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtRejoinDate" errormessage="<%$ Resources:Attendance,Enter Rejoin Date%>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtRejoinDate" runat="server" CssClass="form-control" Enabled="false" />
                                        <cc1:CalendarExtender ID="txtRejoinDates" runat="server" Enabled="True" TargetControlID="txtRejoinDate">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="btnRejoinUpdate" runat="server" ValidationGroup="Save"  Text="<%$ Resources:Attendance,Save %>"
                                            OnClick="btnRejoinUpdate_Click" Visible="false" CssClass="btn btn-success" />
                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                            OnClick="btnCancel_Click" Visible="true" CssClass="btn btn-danger" />
                                    </div>
                                </div>
                                <div id="dvList" runat="server">
                                    <div class="col-md-12">
                                        <br />
                                        <div style="overflow: auto">
                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                            <asp:HiddenField ID="editid" runat="server" />
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRejoining" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                OnPageIndexChanging="gvRejoining_PageIndexChanging" OnSorting="gvRejoining_Sorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnEdit_Command"
                                                                Visible="false" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFrom_Date" runat="server" Text='<%#GetDate(Eval("From_Date")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTo_Date" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Next Rejoin Date%>" SortExpression="RejoiningDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndemnityDate" runat="server" Text='<%# GetDate(Eval("RejoiningDate")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Rejoin Date %>" SortExpression="Actual_Rejoin">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTodays" runat="server" Text='<%# Eval("Actual_Rejoin") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
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

    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
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
