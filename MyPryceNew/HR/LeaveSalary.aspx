<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LeaveSalary.aspx.cs" Inherits="HR_LeaveSalary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/leave_salary.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Leave Salary %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Leave Salary%>"></asp:Label></li>
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
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" ValidationGroup="Get_Report" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtEmpName" errormessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                            AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <br />
                                        <asp:Button ID="btnLeaveSalary" ValidationGroup="Get_Report"  runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary" OnClick="btnLeaveSalary_Click" />

                                        <asp:Button ID="btnReport" ValidationGroup="Get_Report"  runat="server" Text="<%$ Resources:Attendance,Report %>" CssClass="btn btn-primary" OnClick="btnReport_Click" />
                                        <br />
                                    </div>
                                </div>
                                <div id="divLeave" runat="server" visible="false">

                                    <div class="col-md-12">
                                        <hr />
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text='<%$ Resources:Attendance,Leave Request Detail  %>'></asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveDetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                 OnPageIndexChanging="gvLeaveDetail_PageIndexChanging" OnSorting="gvLeaveDetail_Sorting">
                                                <Columns>
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
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Rejoin Date%>" SortExpression="RejoiningDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndemnityDate" runat="server" Text='<%# GetDate(Eval("RejoiningDate")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div id="divLeaveSalary" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <hr />
                                        <asp:Label ID="lblLeaveSection" Font-Bold="true" runat="server" Text='<%$ Resources:Attendance,Leave Salary %>'></asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" SetFocusOnError="true" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Validate_Emp_Grid" ForeColor="Red"></asp:CustomValidator>
                                        <div style="overflow: auto">
                                            <asp:HiddenField ID="hdnEmpId" runat="server" />

                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSalary" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                AllowSorting="false"  OnPageIndexChanging="gvLeaveSalary_PageIndexChanging"
                                                OnSorting="gvLeaveSalary_Sorting">
                                                <Columns>
                                                    <asp:TemplateField SortExpression="Is_Report">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" Checked='<%# Eval("Is_Report") %>' AutoPostBack="true"
                                                                OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" SortExpression="L_Month">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>" SortExpression="L_Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("L_Year") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type%>" SortExpression="LeaveTypeName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("LeaveTypeName") %>'></asp:Label>
                                                            <asp:Label ID="lblLeaveTypeId" Visible="false" runat="server" Text='<%# Eval("Leave_Type_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Count%>" SortExpression="Leave_Count">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeaveCount" runat="server" Text='<%# Eval("Leave_Count") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Per Day Salary%>" SortExpression="Per_Day_Salary">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPerDaySalary" runat="server" Text='<%# Eval("Per_Day_Salary") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total%>" SortExpression="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblLeaveCount" runat="server" Text='<%$ Resources:Attendance,Leave Count Total %>'></asp:Label>
                                        <asp:TextBox ID="txtLeaveCount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label4" runat="server" Text="Apply Leave Count Total"></asp:Label>
                                        <asp:TextBox ID="txtApplyLeaveCount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbltextFinalTotal" runat="server" Text='<%$ Resources:Attendance,Leave Salary Total %>'></asp:Label>
                                        <div class="input-group">
                                            <asp:TextBox ID="lblFinalTotal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:Label ID="lblFinalTotalCurr" CssClass="form-control" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label2" runat="server" Text="Apply Leave Salary Total"></asp:Label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtApplyLeaveSalarytotal" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:Label ID="lblApplyFinalTotalCurr" CssClass="form-control" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                <div id="divClaim" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <hr />
                                        <asp:Label ID="lblEmployeeClaim" Font-Bold="true" runat="server" Text="Taken Leave Salary"></asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeTakenLeave" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                AllowSorting="false"  OnPageIndexChanging="gvEmployeeClaim_PageIndexChanging"
                                                OnSorting="gvEmployeeClaim_Sorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date" SortExpression="F1">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblYear" runat="server" Text='<%#GetDate(Eval("F1").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                            <%--  <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeClaim" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                            runat="server" AutoGenerateColumns="False" Width="800px" AllowPaging="True" AllowSorting="True" 
                                             OnPageIndexChanging="gvEmployeeClaim_PageIndexChanging" OnSorting="gvEmployeeClaim_Sorting" >
                                            <Columns>
                                               <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" SortExpression="Claim_Month">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>" SortExpression="Claim_Year">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Claim_Year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value%>" SortExpression="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                              
                                            </Columns>
                                          
                                            
                                            
                                            <PagerStyle CssClass="pagination-ys" />
                                            
                                        </asp:GridView>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                    <div class="col-md-6">
                                        <br />
                                        <asp:Label ID="lbltextClaimTotal" runat="server" Text="Total"></asp:Label>
                                        <div class="input-group">
                                            <asp:TextBox ID="lblClaimTotal" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:Label ID="lblClaimTotalCurr" CssClass="form-control" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                <div id="divPending" runat="server" visible="false" class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text='<%$ Resources:Attendance,Save %>' CssClass="btn btn-primary"
                                        OnClick="btnSave_Click" Visible="false" />
                                    <asp:Button ID="btnCancel" runat="server" Text='<%$ Resources:Attendance,Cancel %>'
                                        CssClass="btn btn-primary" OnClick="btnCancel_Click" />
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
        function Validate_Emp_Grid(sender, args) {
            var gridView = document.getElementById("<%=gvLeaveSalary.ClientID %>");
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
</asp:Content>
