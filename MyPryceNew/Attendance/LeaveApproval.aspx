<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LeaveApproval.aspx.cs" Inherits="Attendance_LeaveApproval" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-week"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,HR Leave Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,HR Leave Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                   

                    <li id="Li_OTLeave" visible="false" runat="server"><a onclick="Li_OT_Click()" href="#OTLeave" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="OT Leave"></asp:Label></a></li>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Apply Date%>" Value="Application_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Leave Date%>" Value="From_Date"></asp:ListItem>
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Print %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel51" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="IbtnPrint" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:ImageButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/print.png" CommandName='<%# Eval("Emp_Id") %>' OnCommand="IbtnPrint_Command"
                                                                                ToolTip="<%$ Resources:Attendance,Print %>" Width="16px" Visible="true" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkViewDetail" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>" OnCommand="lnkViewDetail_Command"
                                                                                CausesValidation="False" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>

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

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>" SortExpression="Application_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Application_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>" SortExpression="From_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>" SortExpression="To_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTodays" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>" SortExpression="DaysCount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotaldays" runat="server" Text='<%# Eval("DaysCount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="ImgDownloadList" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblFileFullPathList" runat="server" Visible="false" Text='<%#Eval("Field4") %>' />
                                                                            <asp:Label ID="lblFileDownloadList" runat="server" Visible="false" Text='<%#Eval("Field5") %>' />
                                                                            <asp:LinkButton ID="ImgDownloadList" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandList" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                                                        <div class="col-md-12">
                                                            <asp:CheckBox ID="ChkExtraLeave" runat="server" Text="Give Extra Leave" />

                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
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
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Replacement%>"></asp:Label>
                                                            <asp:TextBox ID="txtResponsiblePerson" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsiblePerson"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-5">
                                                            <asp:Label ID="lblHolidayCode" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtToDate_textChanged" />

                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtToDate_textChanged" />

                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Days %>"></asp:Label>
                                                            <asp:TextBox Enabled="false" CssClass="form-control" Style="text-align: center" ID="lblDays" runat="server"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-5">
                                                            <asp:RadioButton ID="rbtnYearly" runat="server" AutoPostBack="true"
                                                                OnCheckedChanged="rbtnMonthlyYearly" GroupName="leave" Text="<%$ Resources:Attendance,Yearly %>" />
                                                            <asp:RadioButton ID="rbtnMonthly" runat="server" Visible="false" Style="margin-left: 50px;" AutoPostBack="true"
                                                                OnCheckedChanged="rbtnMonthlyYearly" GroupName="leave" Text="<%$ Resources:Attendance,Monthly %>" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:DropDownList ID="ddlLeaveType" CssClass="form-control" runat="server"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged" />
                                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lblRDays" runat="server" Style="text-align: center" CssClass="form-control" ForeColor="White" Text="" BackColor="Red"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" id="Leave_Dep_Return" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDepartureDate" runat="server" Text="Actual Departure Date Planned"></asp:Label>
                                                            <asp:TextBox ID="txtDepartureDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtDepartureDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblreturnDate" runat="server" Text="Returning to work on "></asp:Label>
                                                            <asp:TextBox ID="txtreturnDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtreturnDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="1000" Height="60px"
                                                            runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblEmpLogo" runat="server" Text="Leave Attachment"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath1"
                                                                OnClientUploadStarted="FuLogo_UploadStarted"
                                                                OnClientUploadError="FuLogo_UploadError"
                                                                OnClientUploadComplete="FuLogo_UploadComplete"
                                                                OnUploadedComplete="FuLogo_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FULogo_ImgLoader1" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FULogo_Img_Right1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FULogo_Img_Wrong1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FULogo_ImgLoader1" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>


                                                        </div>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                            CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                                        <br />
                                                    </div>
                                                    <hr />
                                                    <div class="col-md-12" class="flow">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvleaveDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete%>">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="imgBtnLeaveDetailDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="imgBtnLeaveDetailDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaveType" runat="server" Text='<%#Common.GetleaveNameById(Eval("Leave_Type_id").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>' />
                                                                        <asp:Label ID="lblLeaveTypeId" Visible="false" runat="server" Text='<%#Eval("Leave_Type_id")%>' />
                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScheduleType" runat="server" Text='<%#Eval("Field3")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,From date%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromdate" runat="server" Text='<%#GetDate(Eval("From_date")) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToDate" runat="server" Text='<%#GetDate(Eval("To_Date")) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Rejoin Date%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRejoinDate" runat="server" Text='<%#GetDate(Eval("Field7")) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Count%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaveCount" runat="server" Text='<%#Eval("LeaveCount") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Responsible Person%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblResponsiblePersonName" runat="server" Text='<%#Common.GetEmployeeName(Eval("Field1").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>' />
                                                                        <asp:Label ID="lblResponsiblePersonNameID" Visible="false" runat="server" Text='<%#Eval("Field1").ToString()%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldesc" runat="server" Text='<%#Eval("Emp_Description") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Departure_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDepartureDate" runat="server" Text='<%#GetDate(Eval("Departure_Date")) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Returning_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReturningDate" runat="server" Text='<%#GetDate(Eval("Returning_Date")) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="ImgDownload" />
                                                                            </Triggers>
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="lblFileFullPath" runat="server" Visible="false" Text='<%#Eval("Field4") %>' />
                                                                                <asp:Label ID="lblFileDownload" runat="server" Visible="false" Text='<%#Eval("Field5") %>' />
                                                                                <asp:LinkButton ID="ImgDownload" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommand1" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <hr />
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Attendance,Apply %>"
                                                            Visible="false" CssClass="btn btn-primary" OnClick="btnApply_Click" />

                                                        <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSummary" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name %>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScheduleType" runat="server" Text='<%# Eval("Shedule_Type") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMonthName" runat="server" Text='<%# GetMonthName(Eval("Month"),Eval("MonthName")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMonthName" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Previous Leave%>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPreviousDays" runat="server" Text='<%# Eval("Previous_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Assign Leave/Yearly" HeaderStyle-HorizontalAlign="Center">
                                                                    <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Leave%>">--%>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassignDays" runat="server" Text='<%# Eval("Assign_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Current Assign Leave" HeaderStyle-HorizontalAlign="Center">
                                                                    <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Leave%>">--%>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentDays" runat="server" Text='<%#GetCurrentLeave(Eval("Assign_Days").ToString(),Eval("Leave_Type_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="OT Leave" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassignOT" runat="server" Text='<%# Eval("OT_Leave") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Total Leave %>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Used Leave %>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsedDays" runat="server" Text='<%#GetUsedLeave( Eval("Used_Days").ToString(),Eval("Encash_Days").ToString(),Eval("Leave_Type_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Encash Leave" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEncashDays" runat="server" Text='<%# Eval("Encash_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Pending Approval" HeaderStyle-HorizontalAlign="Center">
                                                                    <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Pending Approval%>">--%>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Pending_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Balance%>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblActRemainingDays" runat="server" Text='<%#Eval("Remaining_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Applicable Balance%>" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemainingDays" runat="server" Text='<%#Att_Leave_Request.GetRoundValue(Eval("Remaining_Days").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSumary_Pending" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            OnPageIndexChanging="gvLeaveSumary_Pending_PageIndexChanging" AllowPaging="true">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Leave Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
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
                                                                        <asp:Label ID="lblScheduleType" runat="server" Text='<%# GetScheduleType(Eval("From_Date"),Eval("Emp_Id"),Eval("Leave_Type_Id")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotaldays" runat="server" Text='<%# Eval("DaysCount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Application_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTodays" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# GetLeaveStatus(Eval("Trans_Id"))%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="ImgDownloadPending" />
                                                                            </Triggers>
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="lblFileFullPathPending" runat="server" Visible="false" Text='<%#Eval("Field4") %>' />
                                                                                <asp:Label ID="lblFileDownloadPending" runat="server" Visible="false" Text='<%#Eval("Field5") %>' />
                                                                                <asp:LinkButton ID="ImgDownloadPending" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandPending" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="ChoosefileUpload" runat="server" OnClientClick='<%# string.Format("javascript:return OpenModel({0})", Eval("Trans_id"))%>' Text="Upload" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSumary_Approved" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="true" OnSorting="GvLeaveSummary_Approved_Shorting"
                                                            OnPageIndexChanging="gvLeaveSumary_Approved_PageIndexChanging" AllowPaging="true">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Leave Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>" SortExpression="Emp_Name" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name %>" SortExpression="Leave_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>" SortExpression="Emp_Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScheduleType" runat="server" Text='<%# GetScheduleType(Eval("From_Date"),Eval("Emp_Id"),Eval("Leave_Type_Id")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>" SortExpression="DaysCount" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotaldays" runat="server" Text='<%# Eval("DaysCount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>" SortExpression="Application_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Application_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>" SortExpression="From_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>"  SortExpression="To_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTodays" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Next Rejoin Date %>" SortExpression="Field7" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTodaysTwo" runat="server" Text='<%# GetDate(Eval("Field7")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Rejoin Date %>" SortExpression="Actual_Rejoin">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTodaysNew" runat="server" Text='<%# Eval("Actual_Rejoin") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Trans_Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# GetLeaveStatus(Eval("Trans_Id"))%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="ImgDownloadApproved" />
                                                                            </Triggers>
                                                                            <ContentTemplate>
                                                                                <asp:Label ID="lblFileFullPathApproved" runat="server" Visible="false" Text='<%#Eval("Field4") %>' />
                                                                                <asp:Label ID="lblFileDownloadApproved" runat="server" Visible="false" Text='<%#Eval("Field5") %>' />
                                                                                <asp:LinkButton ID="ImgDownloadApproved" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandApproved" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
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
                    <div class="tab-pane" id="OTLeave">
                        <asp:UpdatePanel ID="Update_OT" Visible="false" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="OTEmplbl" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtOTEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtOTEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOTEmpName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="OTAssignlbl" runat="server" Text="<%$ Resources:Attendance,Assign Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtOTAssignDate" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="txtOTAssignDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <%--<div class="col-md-6">
                                                           <asp:Label ID="lblTotalhrs" runat="server" Text="Total Hours"></asp:Label>
                                                            <asp:TextBox ID="txtOTTotalhrs" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <br />
                                                        </div>--%>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lbltotaldays" runat="server" Text="Total Days"></asp:Label>
                                                            <asp:TextBox ID="txtOTTotalDays" type="number" min='1' step="1" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-10">
                                                            <asp:Label ID="lblOTDescription" runat="server" Text="Description"></asp:Label>
                                                            <asp:TextBox ID="txtOTDescription" runat="server" TextMode="MultiLine" MaxLength="1000" Height="60px" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="OTSubmit" CssClass="btn btn-primary" Text="Apply" runat="server" OnClick="btnOTSubmit_Clcik" />
                                                    </div>

                                                    <div class="col-md-12" class="flow">

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="OTLeaveSummary" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            OnPageIndexChanging="gvLeaveOTSumary_PageIndexChanging" AllowPaging="true">
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="imgBtnOTLeaveDelete" AutoPostBack="true" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="imgBtnOTLeaveDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTEmp_Code" runat="server" Text='<%#Eval("Emp_Code")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Employee_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTEmp_Name" runat="server" Text='<%#Eval("Emp_Name")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Leave_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTLeave_Name" runat="server" Text='<%#Eval("Leave_Name")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Assign Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTAssign_Date" runat="server" Text='<%# GetDate(Eval("Assign_Date"))%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Days">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTTotalDays" runat="server" Text='<%#Eval("TotalDays")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOTDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
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
    <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Upload File</h4>
                    <asp:HiddenField ID="hidtransId" runat="server" Value="" />
                </div>

                <div class="modal-body">
                    <div class="col-md-12">
                        <%-- <asp:Label ID="lblEmpLogo" runat="server" Text="Leave Attachment"></asp:Label>--%>
                        <div class="input-group" style="width: 100%;">
                            <cc1:AsyncFileUpload ID="FULogoPath" OnClientUploadStarted="FuLogo_UploadStarted" OnClientUploadError="FuLogo_UploadError" OnClientUploadComplete="FuLogo_UploadComplete" OnUploadedComplete="FuLogo_FileUploadComplete" runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="FULogo_ImgLoader" Width="100%" />
                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />

                            </div>
                        </div>
                        <br />
                    </div>

                </div>
                <div class="modal-footer">
                    <%--<button class="btn btn-info" id="CloseModel" data-dismiss="modal" aria-hidden="true">Close</button>  --%>
                    <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Attendance,Close %>" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                </div>
            </div>
        </div>
    </div>
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
    </script>
    <script>
        function OpenModel(transId) {
            debugger;
            //$('#< %=hidtransId.ClientID% >').val(transId);
            $('#<% =hidtransId.ClientID %>').attr('value', transId);
            //document.getElementById('< %=hidtransId.ClientID%>').val = transId;
            $('#myModal').modal('show');
            return true;
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";
            document.getElementById('<%= FULogo_Img_Wrong1.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right1.ClientID %>').style.display = ""; 

            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();--%>
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            document.getElementById('<%= FULogo_Img_Right1.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong1.ClientID %>').style.display = "";
           <%-- var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";--%>
            alert('Invalid File Type, Select Only .png, .jpg, .jpeg extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpeg" || filext == "pdf") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpeg , .pdf extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpeg , .pdf extension file"
                }
                return false;
            }
        }
    </script>
</asp:Content>



