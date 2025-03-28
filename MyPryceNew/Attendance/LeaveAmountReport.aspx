<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LeaveAmountReport.aspx.cs" Inherits="Attendance_LeaveAmountReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        /*This Css Add By Rahul Sharma on Date 21-12-2023 for header fixed*/
        .scrollable-container {
            max-height: 300px; /* Set the maximum height for the container */
            overflow-y: auto; /* Enable vertical scroll */
        }

        .fixed-header {
            position: sticky;
            top: 0;
            background-color: #f8f9fa; /* Set your preferred background color */
            z-index: 1000; /* Set a higher z-index value */
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-calendar-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Leave Application Schedule"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Leave Application Schedule"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />

            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>

                                <asp:PostBackTrigger ControlID="gvLeaveAmountReport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <asp:HiddenField ID="HDFSort" runat="server" />

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-6">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <%-- OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"--%>
                                                </div>

                                                <div class="col-lg-12">
                                                    <br />
                                                </div>

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="search"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValue" ErrorMessage="<%$ Resources:Attendance,Please Fill Value %>"></asp:RequiredFieldValidator>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" ValidationGroup="search" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblYear" runat="server" Text="Years"></asp:Label>
                                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-lg-6">
                                                    <br />
                                                    <asp:Button ID="btnSaveReport" CssClass="btn btn-success" runat="server" Text="Update" OnClick="btnSaveReport_Click" />
                                                    &nbsp;
                                                     <asp:Button ID="btnCancelReport" CssClass="btn btn-danger" runat="server" Text="Cancel" Visible="false" OnClick="btnCancelReport_Click" />&nbsp;
                                                    <asp:Button ID="btnPrintReport" CssClass="btn btn-primary" runat="server" Text="Print" OnClick="btnPrintReporrt_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvLeaveAmountReport.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="boxSet" runat="server" class="flow" style="height: 500px; overflow-y: auto;">
                                                    <asp:HiddenField ID="hdnLoc_Id" runat="server" />


                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover fixed-header" ID="gvLeaveAmountReport" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowSorting="True"
                                                        OnSorting="gvLeaveAmountReport_OnSorting">

                                                        <%--  AllowPaging="True" PageSize="<%# PageControlCommon.GetPageSize() %>" OnPageIndexChanging="gvLeaveAmountReport_PageIndexChanging" --%>

                                                        <Columns>


                                                            <asp:TemplateField HeaderText="S No." SortExpression="SNo">
                                                                <ItemTemplate>
                                                                    <%-- <asp:Label ID="lblSNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>--%>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Emp Code" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmployeeCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employees" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmployees" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Joint/Change in B.S." SortExpression="Joining_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDOJ" runat="server" Text='<%# Eval("Joining_Date") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Basic Salary" SortExpression="Basic_Salary">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBasicSalary" runat="server" Text='<%# Eval("Basic_Salary") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Total_Days">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblHeaderTodayDate" runat="server" Text='<%# Eval("CalculationDate") %>'></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Total_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Absent Days" SortExpression="Absent_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblAbsentDays" ReadOnly="true" OnClientClick="setScrollPosition()" runat="server" Width="70px" Text='<%# Eval("Absent_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net Leave Days" SortExpression="Net_Leave_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblNetLeaveDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Net_Leave_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Leave Years" SortExpression="Leave_Years">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLeaveYears" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Leave_Years") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LDG" SortExpression="LDG">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLDG" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("LDG") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Leave Days" SortExpression="Leave_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLeaveDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Leave_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="OT Leave Days" SortExpression="OT_Leave_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblOTLeaveDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("OT_Leave_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Leave Days" SortExpression="Total_Leave_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblTotalLeaveDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Total_Leave_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Leave Till Last Year" SortExpression="Old_Leave">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLeaveTillLastYear" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Old_Leave") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Current Year Leave" SortExpression="Current_Leave">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblCurrentYearLeave" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Current_Leave") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Days Previous years Paid" SortExpression="Previous_Year_paid">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDaysPreviousYearsPaid" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Previous_Year_paid") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Due Days Before Taking Leave This Year" SortExpression="Due_Days_Before_Taking_Leave">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDueDaysBeforeTakingLeaveThisYear" ReadOnly="true" Width="70px" runat="server" Text='<%# Eval("Due_Days_Before_Taking_Leave") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Current Year Days Paid" SortExpression="Current_Year_Days_Paid">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblCurrentYearDaysPaid" runat="server" Width="70px"
                                                                        OnTextChanged="txtAmount_Change" AutoPostBack="true"
                                                                        Text='<%# Eval("Current_Year_Days_Paid") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Due Days" SortExpression="Due_Days">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDueDays" ReadOnly="true" runat="server" Width="70px" Text='<%# Eval("Due_Days") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Leave Dues Paid in Current Year" SortExpression="Paid_In_Current_Year">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLeaveDuesPaidinCurrentYear" ReadOnly="true" Width="70px" runat="server" Text='<%# Eval("Paid_In_Current_Year") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Leaves AMT Payable" SortExpression="Leave_AMT_Payable">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblLeavesAMTPayable" runat="server" ReadOnly="true" Width="70px" Text='<%# Eval("Leave_AMT_Payable") %>'></asp:TextBox>
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
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">


        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        //This Code is Created By Rahul Sharama on date 21-12-2023
        //Note:In this Code take a first row clone and add thead in Gridview table then impliment css in header and body
        $(document).ready(function () {
            var header = $("#<%= gvLeaveAmountReport.ClientID %> tbody tr:first").clone();
            $("#<%= gvLeaveAmountReport.ClientID %>").append('<thead class="fixed-header"></thead>');
            $("#<%= gvLeaveAmountReport.ClientID %>").find('thead').append(header);
            var table = $("#<%= gvLeaveAmountReport.ClientID %>");

            // Remove the first row
            table.find('tbody tr:first').remove();
            $("#<%= gvLeaveAmountReport.ClientID %> tbody").css({ "overflow": "auto", "height": "500px" });
           
            <%--$('#<%= boxSet.ClientID %>').before(headerContainer);--%>
        });

        function setScroll() {
            setScrollPositionAfterPostback();
            setScrollPosition();
        }

        function setScrollPosition() {

            // Save the current scroll position before postback
            $('#<%= boxSet.ClientID %>').data('scrollPosition', $('#<%= boxSet.ClientID %>').scrollTop());
        }

        function setScrollPositionAfterPostback() {
            // Restore the scroll position after postback
            debugger;
            var scrollPosition = $('#<%= boxSet.ClientID %>').data('scrollPosition') || 0;
            $('#<%= boxSet.ClientID %>').scrollTop(scrollPosition);
        }

    </script>
    <!-- jQuery -->
    <%--<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>--%>
</asp:Content>
