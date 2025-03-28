<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LeaveEncashment.aspx.cs" Inherits="Attendance_LeaveEncashment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-week"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Leave Encashment"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Leave Encashment"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_RemUpdate" runat="server" visible="false"><a onclick="Li_Rem_Click()" href="#Rem" data-toggle="tab"><i class="fa fa-Pencil"></i>&nbsp;&nbsp;<asp:Label ID="LblRem" runat="server" Text="Leave Update"></asp:Label></a></li>
                    <li id="Li_New" class="active"><a onclick="Li_New_Click()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List"><a onclick="Li_List_Click()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-12">
                                                    <asp:DropDownList runat="server" ID="ddlLoc" OnSelectedIndexChanged="ddlLoc_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlLeaveStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLeaveStatus_SelectedIndexChanged">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Pending" Value="Pending" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                                        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Apply Date%>" Value="Request_Date"></asp:ListItem>                                                     
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvLeaveStatus.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveStatus" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        DataKeyNames="Emp_Id" Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                                        Width="100%" OnPageIndexChanging="gvLeaveStatus_PageIndexChanging" OnSorting="gvLeaveStatus_OnSorting" AllowPaging="true">
                                                        <Columns>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Print %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/print.png" CommandName='<%# Eval("Emp_Code") %>' OnCommand="IbtnPrint_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Print %>" Width="16px" Visible="true" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>" OnCommand="lnkViewDetail_Command"
                                                                        CausesValidation="False" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>" SortExpression="Request_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Request_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>" SortExpression="DaysCount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotaldays" runat="server" Text='<%# Eval("DaysCount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Pay" SortExpression="TotalPay">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalPay" runat="server" Text='<%# Eval("TotalPay")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnTransid" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRequestDate" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtRequestDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSummary" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name %>"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnLeaveTypeId" runat="server" Value='<%# Eval("Leave_Type_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>"  ItemStyle-HorizontalAlign="Center"> 
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblScheduleType" runat="server" Text='<%# Eval("Shedule_Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMonthNameNew" runat="server" Text='<%# GetMonthName(Eval("Month"),Eval("MonthName")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblYearName" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Last Year Balance"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPreviousDays" runat="server" Text='<%# Eval("Previous_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="This Year Assign Leaves"  ItemStyle-HorizontalAlign="Center">
                                                                        <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Leave%>">--%>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblassignDays" runat="server" Text='<%# Eval("ThisYearAssignLeave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                  <asp:TemplateField HeaderText="OT Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("OT_Leave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Used Leave %>"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Used_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pending Approval"  ItemStyle-HorizontalAlign="Center">
                                                                        <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Pending Approval%>">--%>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsedDaysPending" runat="server" Text='<%# Eval("Pending_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Balance%>"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblActRemainingDays" runat="server" Text='<%#Eval("Remaining_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Applicable Balance%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemainingDays" runat="server" Text='<%#Att_Leave_Request.GetRoundValue(Eval("Remaining_Days").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="Encash Days"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLeaveDay" runat="server" Width="60px"
                                                                                OnTextChanged="OnTextChanged" Text="0" AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pay Amount"  ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPayAmount" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblTotalLevaeDays" runat="server" Text="Total Encash Days"></asp:Label>
                                                            <asp:TextBox ID="txtTotalLeaveDays" ReadOnly="true" runat="server"  CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text="Total Pay Amount"></asp:Label>
                                                            <asp:TextBox ID="txtTotalPayAmount" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="1000" Height="60px"
                                                                runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <hr />
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Attendance,Apply %>"
                                                                Visible="false" CssClass="btn btn-primary" OnClick="btnApply_Click" />

                                                            <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                            <br />
                                                        </div> <div class="col-md-12" class="flow">
                                                            <br />
                                                            <asp:Label ID="lblTotalPayAmount" runat="server" Text="Total Pay Amount :" HorizontalAlign="Right"></asp:Label>
                                                            <asp:Label ID="TotalPayAmount" runat="server" Text="" HorizontalAlign="Right"></asp:Label>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSumary_Approved" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                OnPageIndexChanging="gvLeaveSumary_Approved_PageIndexChanging" AllowPaging="true">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Leave Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblScheduleType" runat="server" Text='<%#Eval("ScheduleType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Previous Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PreviousLeave" runat="server" Text='<%# Eval("Previous_Leave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Assigned Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="AssignedLeave" runat="server" Text='<%#Eval("Assigned_Leave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Used Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="UsedLeave" runat="server" Text='<%#Eval("Used_Leave")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Balance">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ActualBalance" runat="server" Text='<%#Eval("ActualBalance")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Applicable Balance">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ApplicableBalance" runat="server" Text='<%# Eval("ApplicableBalance") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Leave Days">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveDays" runat="server" Text='<%# Eval("Leave_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PayAmount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PayAmount" runat="server" Text='<%# Eval("PayAmount") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <br />                                                            
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
                    <div class="tab-pane" id="RemainigUpdate">
                         <asp:UpdatePanel ID="Update_Remainig" Visible="false" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblEmpName" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtEmpNameRem" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtRemEmpName_textChanged" />
                                                             <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemRequestDate" InitialValue="" ErrorMessage="Select Employee Name"/>
                                                            <asp:HiddenField ID="hdnRemEmpId" runat="server" />
                                                 <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpNameRem" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRemRequestDate" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtRemRequestDate" runat="server" CssClass="form-control"/>
                                                             <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemRequestDate" InitialValue="" ErrorMessage="Select Request Date"/>
                                                          <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtRemRequestDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveRemainingChanges" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%">
                                                                    <Columns>
                                                                      <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name %>"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnRemLeaveTypeId" runat="server" Value='<%# Eval("Leave_Type_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField> 
                                                                    <asp:TemplateField HeaderText="Previous Days"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  >                                                                      
                                                                        <ItemTemplate>                                                                            
                                                                            <asp:TextBox ID="lblRemPreviousDays" runat="server" Text='<%# Eval("Previous_Days") %>' AutoPostBack="True" OnTextChanged="PreviousDaysChanged"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>                                                                       
                                                                    <asp:TemplateField HeaderText="Assign Days"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  >                                                                      
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemAssignDays" runat="server" Text='<%# Eval("Assign_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Used Days"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  >                                                                      
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemUsedDays" runat="server" Text='<%# Eval("Used_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remaining Days"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >                                                                      
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemDays" runat="server" Text='<%# Eval("Remaining_Days") %>' AutoPostBack="True" OnTextChanged="RemaningDaysChanged"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>                                                                    
                                                                    </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
                                                        <hr />
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnRemApply" ValidationGroup="E_Save" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-primary" OnClick="btnRemSave_Click"/>
                                                            <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                            <br />
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
        function Li_Edit_Active() {
            $('#Li_List').removeClass('active');
            $('#List').removeClass('active');

            $('#Li_New').addClass('active');
            $('#New').addClass('active');
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Rem_Click(){            
            $("#RemainigUpdate").addClass("active");
            $("#Li_New").removeClass("active");        
            $("#New").removeClass("active");
            $('#Li_List').removeClass('active');
            $('#List').removeClass('active');
        }
    </script>
</asp:Content>



