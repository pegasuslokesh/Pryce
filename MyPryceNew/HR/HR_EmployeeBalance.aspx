<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_EmployeeBalance.aspx.cs" Inherits="HR_HR_EmployeeBalance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/department_master.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Employee Balance"></asp:Label>
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Employee Balance"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Bin" runat="server">
        <ContentTemplate>
            <div class="alert alert-info ">

                <div class="row">
                    <div class="col-md-6">
                        <asp:Label ID="lblFromDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>" />
                        <br />


                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                        <cc1:CalendarExtender ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate" OnClientShown="showCalendar"
                            Format="dd/MMM/yyyy" />


                    </div>
                    <div class="col-md-6">

                        <asp:Label ID="lblToDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>" />
                        <br />
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" OnClientShown="showCalendar"
                            Format="dd/MMM/yyyy" />

                    </div>

                </div>


                <div class="col-md-12" style="text-align: center">
                    <br />
                    <hr />
                    <asp:Label runat="server" Font-Bold="true" ID="lblSelectDept" Text="<%$ Resources:Attendance,Select Location %>"></asp:Label>
                    <br />
                </div>
                <div class="col-md-12" style="text-align: center">
                    <br />
                </div>
                <div class="col-md-12">
                    <div class="col-md-2"></div>
                    <div class="col-md-3">
                        <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                            SelectionMode="Multiple" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                    </div>
                    <div class="col-lg-2" style="text-align: center">
                        <div style="margin-top: 30px; margin-bottom: 30px;" class="btn-group-vertical">
                            <asp:Button ID="btnPushLoc" class="btn btn-info" runat="server" Text=">" OnClick="btnPushLoc_Click" />
                            <asp:Button ID="btnPullLoc" runat="server" class="btn btn-info" Text="<" OnClick="btnPullLoc_Click" />
                            <asp:Button ID="btnPushAllLoc" runat="server" class="btn btn-info" Text=">>" OnClick="btnPushAllLoc_Click" />
                            <asp:Button ID="btnPullAllLoc" runat="server" class="btn btn-info" Text="<<" OnClick="btnPullAllLoc_Click" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <asp:ListBox ID="lstLocationSelect" Style="width: 100%;" runat="server" Height="200px"
                            SelectionMode="Multiple" Font-Bold="true" Font-Names="Trebuchet MS" Font-Size="Small"
                            ForeColor="Gray"></asp:ListBox>
                    </div>
                    <div class="col-md-2"></div>
                </div>




                <div class="row">

                    <div class="col-md-12" style="text-align: center">
                        <br />
                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="Get record"
                            class="btn btn-primary" OnClick="Btn_Save_Click" />


                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                            class="btn btn-primary" OnClick="Btn_Cancel_Click" />
                    </div>

                </div>


            </div>
            <br />


            <div class="box box-warning box-solid">

                <div class="box-body">

                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="false" DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                    AllowSorting="false" >
                                    <Columns>


                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                            <ItemTemplate>


                                                <asp:LinkButton ID="lnkempStatement" runat="server" Text='<%# Eval("Emp_Name") %>' CommandArgument='<%# Eval("Emp_Id") %>' CommandName='<%# Eval("Emp_Name") %>' Font-Underline="true" OnCommand="lnkempStatement_Command"></asp:LinkButton>


                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Opening Balance" SortExpression="Opening_balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOpeningBalance" runat="server" Text='<%# Eval("Opening_balance") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Amount %>" SortExpression="Credit_Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreditamt" runat="server" Text='<%# Eval("Credit_Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Debit Amount %>" SortExpression="Debit_Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldebitamt" runat="server" Text='<%# Eval("Debit_Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Balance %>" SortExpression="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  HorizontalAlign="Right" />
                                            <HeaderStyle  HorizontalAlign="Right" />
                                        </asp:TemplateField>


                                    </Columns>
                                    
                                    
                                    <PagerStyle CssClass="pagination-ys" />
                                    
                                </asp:GridView>
                                <asp:HiddenField ID="HDFSortbin" runat="server" />
                            </div>
                        </div>
                    </div>
                    <br />

                    <!-- /.box -->
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false">
    </asp:Panel>
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

        function Grid_Validate(sender, args) {

            var gridView = document.getElementById("<%=gvPayment.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }
    </script>



    <script type="text/javascript" language="javascript">

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

        sender._popupDiv.parentElement.style.top = positionTop  + 2 + 'px';
        sender._popupDiv.parentElement.style.left = positionLeft + 'px';
    }
    </script>


</asp:Content>
