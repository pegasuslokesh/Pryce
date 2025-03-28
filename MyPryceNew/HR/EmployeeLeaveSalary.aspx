<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="EmployeeLeaveSalary.aspx.cs" Inherits="HR_EmployeeLeaveSalary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-dollar-sign"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Leave Salary %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance, HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Leave Salary%>"></asp:Label></a></li>
        <%--        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Time Table%>"></asp:Label></li>--%>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="Btn_List_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="Btn_New_Click" Text="New" />

            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
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
                    <li id="Li_Approved" style="display: none;"><a href="#Approved" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="Approved"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,Payment%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,New %>"></asp:Label></a></li>





                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="dvNew" runat="server" visible="true">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Get_LS_Report"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLSEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtLSEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLSEmpName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Get_LS_Report"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="false"
                                                                TargetControlID="txtFromDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Get_LS_Report"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                            </cc1:CalendarExtender>

                                                            <br />
                                                        </div>



                                                        <div class="col-md-12" align="center">
                                                            <br />
                                                            <asp:Button ID="btnGetLeaveSalary" ValidationGroup="Get_LS_Report" runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary" OnClick="btnGetLeaveSalary1_Click" />

                                                            <asp:Button ID="btnresetLeaveSalary" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnresetLeaveSalary_Click" />
                                                            <br />
                                                        </div>
                                                    </div>


                                                    <div id="divPendingLeaveSalary" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <hr />
                                                            <asp:Label ID="Label17" Font-Bold="true" runat="server" Text="Pending Leave Salary"></asp:Label>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPendingLeaveSalary" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="100%">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="LeaveName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("LeaveName") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotal" Font-Bold="true" runat="server" Text="Total"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="From Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvFromdate" runat="server" Text='<%#GetDate(Eval("from_date").ToString())%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvTodate" runat="server" Text='<%#GetDate(Eval("to_date").ToString())%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvtotalLeave" runat="server" Text='<%#Eval("TotalLeave") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvactualLeave" runat="server" Text='<%#Eval("ActualLeave")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Used Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvusedLeave" runat="server" Text='<%#Eval("UsedLeave") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Balance Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvbalanceLeave" runat="server" Text='<%#Eval("BalanceLeave").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Per Day Salary">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvperdaysalary" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Per_Day_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvtotal" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total").ToString().ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>' />
                                                                        </ItemTemplate>

                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>


                                                    </div>




                                                    <div id="divGenerateLeaveSalary" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <hr />
                                                            <asp:Label ID="lblLeaveSection" Font-Bold="true" runat="server" Text='<%$ Resources:Attendance,Leave Salary %>'></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">

                                                            <asp:HiddenField ID="hdnEmpId" runat="server" />

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSalaryDetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" ShowFooter="true"
                                                                AllowSorting="false">
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFromdate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                            <%--<asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month_Name") %>'></asp:Label>--%>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTodate" runat="server" Text='<%#GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                            <%--<asp:Label ID="lblYear" runat="server" Text='<%# Eval("L_Year") %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type%>" SortExpression="LeaveTypeName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveType" runat="server" Text='<%#Common.GetleaveNameById(Eval("Leave_Type_Id").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString()) %>'></asp:Label>
                                                                            <asp:Label ID="lblLeaveTypeId" Visible="false" runat="server" Text='<%# Eval("Leave_Type_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalLabel" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total%>"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Leave" SortExpression="Total_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_LeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Actual Leave" SortExpression="Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>' OnTextChanged="lblLeaveCount_TextChanged" AutoPostBack="true"></asp:TextBox>

                                                                            <%--<asp:Label ID="lblLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Leave_Count").ToString()) %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_ActualLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />

                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Used Leave" SortExpression="Used_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsedLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Used_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_UsedLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />

                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Balance Leave" SortExpression="Balance_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBalanceLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Balance_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalSum_BalanceLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Per Day Salary%>" SortExpression="Per_Day_Salary">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPerDaySalary" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Per_Day_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                        <ItemStyle />

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total%>" SortExpression="Total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" visible="false">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                                <a style="color: Red">*</a>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidator_txtDebitAccount" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                    ControlToValidate="txtpaymentdebitaccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="true"
                                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                                    UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                                <a style="color: Red">*</a>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidatorCreditaccount" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                    ControlToValidate="txtpaymentCreditaccount" ErrorMessage="Enter Credit Account" />
                                                                <asp:TextBox ID="txtpaymentCreditaccount" runat="server" BackColor="#eeeeee"
                                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                                    UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">

                                                            <asp:Button ID="btnLSSave" ValidationGroup="Save" runat="server" Text="Save & Post" CssClass="btn btn-success"
                                                                OnClick="btnLSSave_Click" Visible="false" />

                                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to Save Leave Salary ?"
                                                                TargetControlID="btnLSSave">
                                                            </cc1:ConfirmButtonExtender>

                                                            <asp:Button ID="btnLSCancel" runat="server" Text='<%$ Resources:Attendance,Cancel %>'
                                                                CssClass="btn btn-danger" OnClick="btnLSCancel_Click" />



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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="Div1" runat="server" visible="true">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Get_Report"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnLeaveSalary" ValidationGroup="Get_Report" runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary" OnClick="btnLeaveSalary_Click" />

                                                            <asp:Button ID="btnReport" ValidationGroup="Get_Report" runat="server" Text="<%$ Resources:Attendance,Report %>" CssClass="btn btn-primary" OnClick="btnReport_Click" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="divLeave" runat="server" visible="false">

                                                        <div class="col-md-12">
                                                            <hr />
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Text='<%$ Resources:Attendance,Leave Request Detail  %>'></asp:Label>
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
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTo_Date" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Rejoin Date%>" SortExpression="RejoiningDate">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblIndemnityDate" runat="server" Text='<%# GetDate(Eval("RejoiningDate")) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
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
                                                            <asp:Label ID="Label5" Font-Bold="true" runat="server" Text='<%$ Resources:Attendance,Leave Salary %>'></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" SetFocusOnError="true" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Validate_Emp_Grid" ForeColor="Red"></asp:CustomValidator>
                                                            <div style="overflow: auto">
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />

                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSalary" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                    runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                                    AllowSorting="false" OnPageIndexChanging="gvLeaveSalary_PageIndexChanging"
                                                                    OnSorting="gvLeaveSalary_Sorting">
                                                                    <Columns>
                                                                        <asp:TemplateField SortExpression="Is_Report">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="Chk_Gv_Select_All" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_CheckedChanged" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkselect" runat="server" Checked='<%# Eval("Is_Report") %>' AutoPostBack="true"
                                                                                    OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFromdate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                                <%--<asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month_Name") %>'></asp:Label>--%>
                                                                                <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTodate" runat="server" Text='<%#GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                                <%--<asp:Label ID="lblYear" runat="server" Text='<%# Eval("L_Year") %>'></asp:Label>--%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type%>" SortExpression="LeaveTypeName">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("LeaveTypeName") %>'></asp:Label>
                                                                                <asp:Label ID="lblLeaveTypeId" Visible="false" runat="server" Text='<%# Eval("Leave_Type_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Count%>" SortExpression="Leave_Count">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLeaveCount" runat="server" Text='<%#Eval("F4") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Per Day Salary%>" SortExpression="Per_Day_Salary">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPerDaySalary" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Per_Day_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total%>" SortExpression="Total">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Common.GetAmountDecimal( Eval("Total").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
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
                                                            <asp:Label ID="Label8" runat="server" Text="Apply Leave Count Total"></asp:Label>
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
                                                            <asp:Label ID="Label9" runat="server" Text="Apply Leave Salary Total"></asp:Label>
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
                                                                    AllowSorting="false" OnPageIndexChanging="gvEmployeeClaim_PageIndexChanging"
                                                                    OnSorting="gvEmployeeClaim_Sorting">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Date" SortExpression="F1">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblYear" runat="server" Text='<%#GetDate(Eval("F1").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="Total">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
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
                                                    <div id="divPending" runat="server" visible="false">

                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Narration %>"></asp:Label>
                                                            <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                            <br />
                                                        </div>


                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtLSDebitAccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtLSDebitAccount" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="true"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLSDebitAccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtLSCreditaccount" ErrorMessage="Enter Credit Account" />
                                                            <asp:TextBox ID="txtLSCreditaccount" runat="server" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLSCreditaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12" style="text-align: center">

                                                            <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text='<%$ Resources:Attendance,Save %>' CssClass="btn btn-primary"
                                                                OnClick="btnSave_Click" Visible="false" />
                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to Pay Leave Salary ?"
                                                                TargetControlID="btnSave">
                                                            </cc1:ConfirmButtonExtender>


                                                            <asp:Button ID="btnCancel" runat="server" Text='<%$ Resources:Attendance,Cancel %>'
                                                                CssClass="btn btn-primary" OnClick="btnCancel_Click" />
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


                    <div class="tab-pane" id="Approved">
                        <asp:UpdatePanel ID="Update_Approved" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="Div2" runat="server" visible="true">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Get_LS_Report_Approved"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtempName_Approved" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtempName_Approved" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtempName_Approved" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnGetLeaveSalaryApproved" ValidationGroup="Get_LS_Report_Approved" runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary" OnClick="btnGetLeaveSalaryApproved_Click" />


                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div id="divGenerateLeaveSalaryApproved" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <hr />
                                                            <asp:Label ID="Label19" Font-Bold="true" runat="server" Text='<%$ Resources:Attendance,Leave Salary %>'></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">

                                                            <asp:HiddenField ID="HiddenField2" runat="server" />

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeavesalaryApproved" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" ShowFooter="true"
                                                                AllowSorting="false">
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFromdate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                            <%--<asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month_Name") %>'></asp:Label>--%>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTodate" runat="server" Text='<%#GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                            <%--<asp:Label ID="lblYear" runat="server" Text='<%# Eval("L_Year") %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type%>" SortExpression="LeaveTypeName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveType" runat="server" Text='<%#Eval("LeaveName") %>'></asp:Label>
                                                                            <asp:Label ID="lblLeaveTypeId" Visible="false" runat="server" Text='<%# Eval("Leave_Type_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalLabel" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total%>"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Leave" SortExpression="Total_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_LeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Actual Leave" SortExpression="Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_ActualLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />

                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Used Leave" SortExpression="Used_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsedLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Used_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum_UsedLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />

                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Balance Leave" SortExpression="Balance_Leave_Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBalanceLeaveCount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Balance_Leave_Count").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalSum_BalanceLeaveCount" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Per Day Salary%>" SortExpression="Per_Day_Salary">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPerDaySalary" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Per_Day_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                        <ItemStyle />

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total%>" SortExpression="Total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblTotalSum" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                        </FooterTemplate>

                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" visible="false">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                                <a style="color: Red">*</a>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidator10" ValidationGroup="Save_Approved" Display="Dynamic" SetFocusOnError="true"
                                                                    ControlToValidate="txtpaymentdebitaccountApproved" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtpaymentdebitaccountApproved" runat="server" CssClass="form-control" BackColor="#eeeeee" Enabled="true"
                                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccountApproved"
                                                                    UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                                <a style="color: Red">*</a>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidator11" ValidationGroup="Save_Approved" Display="Dynamic" SetFocusOnError="true"
                                                                    ControlToValidate="txtpaymentCreditaccountApproved" ErrorMessage="Enter Credit Account" />
                                                                <asp:TextBox ID="txtpaymentCreditaccountApproved" runat="server" BackColor="#eeeeee"
                                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccountApproved"
                                                                    UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">

                                                            <asp:Button ID="btnLsSaveApproved" ValidationGroup="Save_Approved" runat="server" Text="Save & Post" CssClass="btn btn-primary"
                                                                OnClick="btnLsSaveApproved_Click" Visible="false" />

                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Are you sure to Save Leave Salary ?"
                                                                TargetControlID="btnLsSaveApproved">
                                                            </cc1:ConfirmButtonExtender>

                                                            <asp:Button ID="btnLSCancelApproved" runat="server" Text='<%$ Resources:Attendance,Cancel %>'
                                                                CssClass="btn btn-primary" OnClick="btnLSCancelApproved_Click" />



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
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Approved">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }


    </script>
</asp:Content>
