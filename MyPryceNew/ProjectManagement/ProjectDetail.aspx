<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProjectDetail.aspx.cs" Inherits="ProjectManagement_ProjectDetail" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/project_task_feedback.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Team Availability%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Team Availability%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"
                                        ></asp:Label>
                                    <dx:ASPxComboBox ID="ddlEmployeeName" runat="server" CssClass="form-control" DropDownWidth="550"
                                        OnSelectedIndexChanged="ddlempname_OnSelectedIndexChanged" DropDownStyle="DropDownList"
                                        ValueField="Emp_Id" ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true"
                                        IncrementalFilteringMode="Contains" AutoPostBack="true" CallbackPageSize="30">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Emp_Name" Caption="Employee Name" />
                                            <dx:ListBoxColumn FieldName="Emp_Code" Caption="Employee Code" />
                                            <dx:ListBoxColumn FieldName="Designation"/>
                                        </Columns>
                                    </dx:ASPxComboBox>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Project Name  %>"
                                        ></asp:Label>
                                    <dx:ASPxComboBox ID="ddlprojectname" runat="server" CssClass="form-control" DropDownWidth="550"
                                        OnSelectedIndexChanged="ddlprojectname_SelectedIndexChanged" DropDownStyle="DropDownList"
                                        ValueField="Project_Id" ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true"
                                        IncrementalFilteringMode="Contains" AutoPostBack="true" CallbackPageSize="30">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Project_Name" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label15" runat="server" Text="Task Status"
                                        ></asp:Label>
                                    <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Assigned" Value="Assigned" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                        <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12"></div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label2" runat="server" Text="Assign From date" ></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="Assign to date" ></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label1" runat="server" Text="Task Type" ></asp:Label>
                                    <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Both" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Bugs" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="Task" Value="False"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label19" runat="server" Text="Priority" ></asp:Label>
                                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                        <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                                        <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="High"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngo" runat="server" CausesValidation="False"
                                        Text="<%$ Resources:Attendance,Go %>" Visible="true" CssClass="btn btn-primary"
                                        OnClick="btngo_Click" />

                                    <asp:Button ID="btnReport" runat="server" Text="<%$ Resources:Attendance,Report %>"
                                        CssClass="btn btn-primary" OnClick="btnReport_Click" CausesValidation="False" />
                                    <br />
                                    <br />
                                </div>
                                <asp:Panel ID="pnllist" runat="server">
                                    <div id="Div_EmbValue" runat="server" visible="false" class="col-md-6">
                                        <asp:Label ID="lblEmpName" runat="server" Font-Bold="true"
                                            Text="Employee Name" Visible="false"></asp:Label>
                                        &nbsp:&nbsp<asp:Label ID="lblEmpValue" runat="server" Visible="false"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6" id="Div_AssigndateNameValue" runat="server" visible="false">
                                        <asp:Label ID="lblAssigndateName" runat="server" Font-Bold="true"
                                            Text="Assigned Date" Visible="false"></asp:Label>
                                        &nbsp:&nbsp<asp:Label ID="lblAssigndateNameValue" runat="server" Visible="false"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="overflow: auto; max-height: 500px">
                                        
                                        <%--<asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvrstatus" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="false" OnPageIndexChanging="gvrstatus_PageIndexChanging" AllowSorting="True"
                                             OnSorting="gvrstatus_Sorting">
                                            
                                            <Columns>--%>
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvrstatus" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="#000"
                                            RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000"
                                            runat="server" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="gvrstatus_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Project No." SortExpression="Field7">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectNo" runat="server" Text='<%# Eval("Field7") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectIdList4" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Task">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectIdList41" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                        <asp:Label ID="lbltskId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectIdassignedBy" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList12" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                        <asp:Label ID="lblEmpnameList13" runat="server" Text='<%# FormatTime(Eval("Assign_Time")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expected Date" SortExpression="Emp_Close_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                        <asp:Label ID="lblEmpnameList131" runat="server" Text='<%# FormatTime(Eval("Emp_Close_Time")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned To">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectIdList2" runat="server" Text='<%# Eval("AssignTo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprojectIdList3" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Field4">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpriority" runat="server" Text='<%# Eval("Field41") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Completed %" SortExpression="Field5">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcompletion" runat="server" Text='<%# Eval("Field51") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Task Type" SortExpression="TaskType_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBugs" runat="server" Text='<%# Eval("TaskType_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" >
                                                                <ItemTemplate>

                                                                    <asp:CheckBox ID="chkTaskClose" AutoPostBack="true" OnCheckedChanged="chkTaskClose_OnCheckedChanged" runat="server" Checked ='<%# GetStatus(Eval("Status")) %>'></asp:CheckBox >
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
                                            </Columns>
                                            
                                            <PagerStyle CssClass="pagination-ys" />
                                            
                                        </asp:GridView>
                                        <br />
                                    </div>
                                </asp:Panel>
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

    
											<asp:Panel ID="Panel1" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel2" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel3" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel4" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel5" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel6" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel7" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel8" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel9" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel10" runat="server" Visible="false"></asp:Panel>
											<asp:Panel ID="Panel11" runat="server" Visible="false"></asp:Panel>

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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
    </script>
</asp:Content>
