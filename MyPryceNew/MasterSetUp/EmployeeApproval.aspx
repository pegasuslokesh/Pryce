<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="EmployeeApproval.aspx.cs" Inherits="MasterSetUp_EmployeeApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">

    <style>
        .heading1 {
            background-color: black;
            color: white;
        }

        .row1 {
            background-color: white;
        }

        .alt-row1 {
            background-color: #e8e8e8;
        }

        .empty-row1 {
            background-color: red;
            color: green;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-check"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Approval %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Approval Master%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Approval Master%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Approval%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#View_Approval_Modal" Text="View Modal" />
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

    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
        <ContentTemplate>
            <div id="pnlApprovalType" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Button ID="btnViewAllPendings" runat="server" ValidationGroup="Next" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnViewAllPendings_Click" Text="View All Pendings" Visible="false" />
                                    <br />
                                </div>
                                <div class="col-md-4" style="text-align: center">
                                    <asp:Label ID="lblApprovalType" runat="server" Text="<%$ Resources:Attendance,Select Approval Type %>"></asp:Label>

                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Next" Display="Dynamic"
                                        SetFocusOnError="true" ControlToValidate="ddlApprovalType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Approval Type %>" />
                                    &nbsp:&nbsp
                                    <asp:DropDownList ID="ddlApprovalType" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <br />
                                    <div style="text-align: center">
                                        <asp:Button ID="btnSave" runat="server" ValidationGroup="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" Text="<%$ Resources:Attendance,Next %>" />
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnlApproval" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="col-md-12">
                                    <h3>
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Approval Type %>"></asp:Label>
                                        &nbsp:&nbsp<asp:Label ID="lblApprovalType1" runat="server"></asp:Label></h3>
                                </div>
                                <div class="col-md-12" id="divFilter" runat="server">
                                    <div>
                                        <div class="col-md-4">

                                            <span>Location : </span>
                                            <asp:DropDownList ID="ddlFilterLoc" runat="server" Style="width: 150px; height: 35px;">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">

                                            <span>Month : </span>
                                            <asp:DropDownList ID="ddlMonth" runat="server" Style="width: 150px; height: 35px;">
                                                <asp:ListItem Text="--Select Month--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="November    " Value="11"></asp:ListItem>
                                                <asp:ListItem Text="December" Value="12"></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <span></span>
                                        <span>Year : </span>
                                        <asp:TextBox ID="txtSelectYear" runat="server" Style="width: 150px; height: 35px;" />


                                        <asp:Button ID="btnFilterApply" runat="server" CssClass="btn btn-primary" Text="Filter Apply" OnClick="ddlStatus_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" OnClick="btnBackClick" Text="<%$ Resources:Attendance,Back %>" />
                                        <asp:Button ID="btnShiftApprove" runat="server" CssClass="btn btn-primary" OnClick="btnShiftApprove_Click"
                                            Text="<%$ Resources:Attendance,Approve %>" />
                                        <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval1" runat="server" TargetControlID="btnShiftApprove"
                                            ConfirmText="Are you sure you want to Approve this request?">
                                        </cc1:ConfirmButtonExtender>

                                        <asp:Button ID="btnShiftReject" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="btnShiftReject_Click" Text="<%$ Resources:Attendance,Reject %>" />
                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnShiftReject"
                                            ConfirmText="Are you sure you want to Reject this request?">
                                        </cc1:ConfirmButtonExtender>


                                    </div>

                                    <div class="col-md-6" id="div_shift_Ref" runat="server" visible="false">
                                        <asp:Label ID="Label68" runat="server" class="control-label" Style="margin-top: 5px;" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>

                                        <a style="color: Red">*</a>
                                        <div class="input-group">

                                            <asp:DropDownList ID="ddlReferenceNo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlReferenceNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                            <div class="input-group-btn">
                                            </div>
                                            <div class="input-group-btn">
                                                <asp:Button ID="btnRefRefresh" runat="server" Text="<%$ Resources:Attendance,Reset%>" CssClass="btn btn-primary" OnClick="btnRefRefresh_Click" />
                                            </div>
                                            <div class="input-group-btn">
                                                <asp:Button ID="btnDownload" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnDownload_Click" />
                                            </div>
                                        </div>

                                    </div>


                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label20" runat="server" class="control-label" Style="margin-top: 5px;" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                            : &nbsp<asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlserachrecord" runat="server" DefaultButton="btnRefresh">
                    <div id="pnlSearchRecords" runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label77" runat="server" Text="Advance Search"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					        <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="RequestEmp_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="RequestEmp_Code"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="SalesOrderNo"></asp:ListItem>
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
                                                <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center">
                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                        </div>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>





                </asp:Panel>
                <div class="box box-warning box-solid">
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <br />
                                <div class="flow">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvApproval" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                        AutoGenerateColumns="false" Width="100%" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvApproval_OnRowDataBound"
                                        OnPageIndexChanging="gvApproal_PageIndexChanging" OnSorting="gvApproval_OnSorting">
                                        <Columns>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,ID %>" SortExpression="Ref_Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" />

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,ID %>" SortExpression="Ref_Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApptype1" runat="server" Text='<%# Eval("Ref_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>" SortExpression="Approval_Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApptype" runat="server" Text='<%# Eval("Approval_Type") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("RequestEmp_Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="NetAmount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetAmount" runat="server" Text='<%# GetCurrencySymbol(Eval("NetAmount").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempName" runat="server" Text='<%# Eval("RequestEmp_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Request Date" SortExpression="Request_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbrequestdate" runat="server" Text='<%# Convert.ToDateTime(Eval("Request_Date")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                <ItemTemplate>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" Width="100%" runat="server" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnApprove" ToolTip="Approve" OnCommand="Approve_Command"
                                                                        CommandName='<%#Eval("Priority") %>' CommandArgument='<%#Eval("Trans_Id") %>'
                                                                        Width="15px" Height="15px" ImageUrl="~/Images/approve.png" runat="server" />
                                                                    <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval" runat="server" TargetControlID="imgBtnApprove"
                                                                        ConfirmText="Are you sure you want to Approve this request?">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnReject" ToolTip="Reject" OnCommand="Reject_Command" CommandName='<%#Eval("Priority") %>'
                                                                        CommandArgument='<%#Eval("Trans_Id") %>' Width="15px" Height="15px" ImageUrl="~/Images/disapprove.png"
                                                                        runat="server" />
                                                                    <cc1:ConfirmButtonExtender ID="ConfirmBtnReject" runat="server" TargetControlID="imgBtnReject"
                                                                        ConfirmText="Are you Sure to Reject this Request ?">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnView" OnCommand="View_Command" ToolTip="View" CommandName='<%#Eval("Priority") %>'
                                                                        CommandArgument='<%#Eval("Trans_Id") %>' Width="15px" Height="15px" ImageUrl="~/Images/detail.png"
                                                                        runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee Name" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblempame" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnGvEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("Priority") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date" SortExpression="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDateTime" runat="server" Text='<%#  GetDate(Eval("Date").ToString()) %>'></asp:Label>

                                                                    <asp:HiddenField ID="hdnTransId" Value='<%# Eval("Trans_Id") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnRefId" Value='<%# Eval("Ref_Id") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnRequestDate" runat="server" Value='<%# Eval("Request_Date") %>' />
                                                    <asp:HiddenField ID="hdnApprovalType" runat="server" Value='<%# Eval("Approval_Type") %>' />
                                                    <asp:HiddenField ID="hdnApprovalId" runat="server" Value='<%# Eval("Approval_Id") %>' />

                                                    <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='10' align='left'>" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftView" runat="server" AutoGenerateColumns="true">
                                                        <Columns>
                                                        </Columns>
                                                        <%--   
                                                        <PagerStyle CssClass="pagination-ys" />--%>
                                                        <%-- --%>
                                                        <%-- <RowStyle CssClass="row1"></RowStyle>
                                                        <AlternatingRowStyle CssClass="alt-row1"></AlternatingRowStyle>--%>
                                                    </asp:GridView>
                                                    <asp:Literal runat="server" ID="lit2" Text="</td></tr>" />


                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle />
                                        <%-- --%>

                                        <PagerStyle CssClass="pagination-ys" />
                                        <RowStyle CssClass="row1"></RowStyle>
                                        <AlternatingRowStyle CssClass="alt-row1"></AlternatingRowStyle>
                                    </asp:GridView>
                                    <asp:CheckBox ID="chkselectall" runat="server" Text="Select All" onclick="checkItem_All1_Header(this)" />

                                    <%--   <div class="col-md-12" style="max-height: 300px; overflow: auto;">--%>
                                    <asp:Panel runat="server" ID="pnlExport">

                                        <asp:DataList ID="dtlistShift" runat="server" RepeatColumns="1" RepeatDirection="Horizontal" Width="100%">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <br style="mso-data-placement: same-cell; text-align: center; vertical-align: middle;" />
                                                    <table border="1" id='<%# Eval("ReferenceNo") %>' cellpadding="5" cellspacing="0" style="border: solid 1 Silver; font-size: x-small; width: 100%;">
                                                        <tr style="width: 100%;">

                                                            <td align="Center" bgcolor="#F79646" style="font-weight: bold; color: White; font-size: 14px;">Location </td>
                                                            <td align="Center" bgcolor="#F79646" style="font-weight: bold; color: White; font-size: 14px;">Ref. No. </td>
                                                            <td align="Center" bgcolor="#F79646" style="font-weight: bold; color: White; font-size: 14px;">Status </td>
                                                            <td align="Center" bgcolor="#F79646" style="font-weight: bold; color: White; font-size: 14px;">Uploaded By </td>
                                                            <td align="Center" bgcolor="#F79646" style="font-weight: bold; color: White; font-size: 14px;">Uploaded Date </td>
                                                        </tr>
                                                        <tr style="width: 100%;">
                                                            <td align="Center" style="font-weight: bold; font-size: 14px;">
                                                                <asp:Label ID="lbllocation" runat="server"></asp:Label></td>

                                                            <td align="Center" style="font-weight: bold; font-size: 14px;">
                                                                <asp:Label ID="lblRefno" runat="server" Text='<%# Eval("ReferenceNo") %>'></asp:Label>
                                                            </td>

                                                            <td align="Center" style="font-weight: bold; font-size: 14px;">
                                                                <asp:Label ID="lblstatus" runat="server"></asp:Label>
                                                            </td>

                                                            <td align="Center" style="font-weight: bold; font-size: 14px;">
                                                                <asp:Label ID="lbluploadedby" runat="server"></asp:Label>
                                                            </td>

                                                            <td align="Center" style="font-weight: bold; font-size: 14px;"><span style="font-size: 1px;">.</span><asp:Label ID="lbluploaddate" runat="server"></asp:Label>
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td colspan="5" style="font-size: 14px; text-align: center;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftView" AllowPaging="false" Width="100%" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" DataKeyNames="Code">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkitemselect" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <%--<asp:CheckBox ID="chkHeaderitemselect" runat="server" onclick="checkItem_All1_Header(this)" />--%>
                                                                                <asp:CheckBox ID="chkHeaderitemselect" runat="server" onclick="checkall(this)" />
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>


                                                                    </Columns>

                                                                </asp:GridView>
                                                                <br />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvshiftsummary" AllowPaging="false" Width="100%" ClientIDMode="Static" runat="server" AutoGenerateColumns="true">
                                                                    <Columns>
                                                                    </Columns>

                                                                </asp:GridView>

                                                            </td>
                                                        </tr>
                                                    </table>


                                                </div>

                                            </ItemTemplate>
                                        </asp:DataList>


                                    </asp:Panel>
                                    <%--  </div>--%>
                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                    <asp:HiddenField ID="hdnApprovalType" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="View_Approval_Modal" tabindex="-1" role="dialog" aria-labelledby="View_Approval_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="View_Approval_ModalLabel">Approval</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div id="pnlLeave" runat="server">
                                                    <div id="trleave" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLvId" runat="server" Text="<%$ Resources:Attendance, Leave Id %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblLeaveId" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLName" runat="server" Text="<%$ Resources:Attendance, Leave Name%>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblLeaveName" runat="server" Font-Bold="true"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCode" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblEmpCode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmpN" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblEmpName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trleave1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Schedule Type %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblScheduleType" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Status%>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblStatus" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="trleave2" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance, Total Leave %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblTotalLeave" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Apply Date%>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblApplyDate" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="trleave3" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblFromDate" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,To Date%>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblToDate" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="trleave4" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label65" runat="server" Text="Replacement"></asp:Label>
                                                            &nbsp:&nbsp<asp:TextBox ID="txtResponsibePerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                OnTextChanged="txtResponsibePerson_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                                TargetControlID="txtResponsibePerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <%--<asp:Literal ID="ltrLeaveDetail" runat="server"></asp:Literal>--%>
                                                         <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveView" AutoGenerateColumns="false" AllowPaging="false" runat="server" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Leave Name" SortExpression="Status" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Leave_Name" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From_date" SortExpression="From_date" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="From_date" runat="server" Text='<%# Convert.ToDateTime(Eval("From_date")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To_Date" SortExpression="To_Date" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="To_Date" runat="server" Text='<%# Convert.ToDateTime(Eval("To_Date")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Applied Leave Day" SortExpression="LeaveCount" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LeaveCount" runat="server" Text='<%# Eval("LeaveCount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description" SortExpression="Emp_Description" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Emp_Description" runat="server" Text='<%# Eval("Emp_Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Actual Balance" SortExpression="Remaining_Days" HeaderStyle-CssClass="header-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Remaining_Days" runat="server" Text='<%# Eval("Remaining_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>                                                               
                                                            </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <div id="trleave5" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblLeaveDesc" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="pnlClaim" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance, Claim Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimName" Font-Bold="true" runat="server"></asp:Label>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                                                        <asp:Label ID="lblClaimId" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimEmpCode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimEmpName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimMonth" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Year%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimYear" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance, Type %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimType" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Value%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimValue" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimReqDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Status%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimStatus" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Claim Description %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblClaimDescription" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlLoan" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance, Loan Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanName" Font-Bold="true" runat="server"></asp:Label>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label18" runat="server" Text=""></asp:Label>
                                                        <asp:Label ID="lblLoanId" Font-Bold="true" runat="server"></asp:Label>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanEmpCode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanEmpName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Loan Amount %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanAmount" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Duration%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanDuration" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Interest %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanInterest" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Gross Amount%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanGrossAmount" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Monthly Installment %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanInstallment" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Status%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanStatus" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanRequestDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Approval Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblLoanApprovalDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlHalfDay" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance, Half Day Type %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDayLeaveType" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDAyStatus" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDAyEmpcode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfdayEmpName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance, Request Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDayReqDAte" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:Attendance,Apply Date%>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDayApplyDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblHalfDayDescription" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" id="div_InLog" runat="server">
                                                        <asp:Label ID="lblHLInLog" runat="server" Text="<%$ Resources:Attendance,HL In Log %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:TextBox ID="txtHLInLog" runat="server" CssClass="form-control" />
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtHLInLog"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender2"
                                                            ControlToValidate="txtHLInLog" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                            SetFocusOnError="True" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" id="div_OutLog" runat="server">
                                                        <asp:Label ID="lblHLOutLog" runat="server" Text="<%$ Resources:Attendance,HL Out Log %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:TextBox ID="txtHLOutLog" runat="server" CssClass="form-control" />
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtHLOutLog"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender3"
                                                            ControlToValidate="txtHLOutLog" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                            SetFocusOnError="True" />
                                                        <br />
                                                    </div>

                                                    <div id="pnHLLog" runat="server" class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvHLLog" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" DataKeyNames="Emp_Id">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemType1" runat="server" Text='<%# Convert.ToDateTime(Eval("Event_Time")).ToString("HH:mm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemType2" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Function Key %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemType3" runat="server" Text='<%# Eval("Func_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Verified By %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVerifiedby1" runat="server" Text='<%# Eval("Verified_Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="PnlPartialLeave" runat="server">

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblPartialLName" runat="server" Text="<%$ Resources:Attendance,Partial Leave Name %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblPartialLeaveName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" visible="false">
                                                        <asp:Label ID="lblPartialLId" runat="server" Text="<%$ Resources:Attendance,Partial Leave Id %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblPartialLeaveId" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblECode" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblEmployeeCode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEName" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblEmployeeName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPartialLDate" runat="server" Text="<%$ Resources:Attendance,Partial Leave Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblPartialLeaveDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReqDate" runat="server" Text="<%$ Resources:Attendance,Request Date%>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblRequestDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div id="TrWithTime" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblFTime" runat="server" Text="<%$ Resources:Attendance, From Time %>"></asp:Label>
                                                            &nbsp:&nbsp<asp:Label ID="lblFromTime" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTTime" runat="server" Text="<%$ Resources:Attendance,To Time%>"></asp:Label>
                                                            &nbsp:&nbsp
                                                            <asp:Label ID="lblToTime" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="TrWithOutTime" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPLType" runat="server" Text="<%$ Resources:Attendance, PL Type %>"></asp:Label>
                                                            &nbsp:&nbsp
                                                            <asp:Label ID="txtPLType" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTimeTable" runat="server" Text="<%$ Resources:Attendance, In TimeTable %>"></asp:Label>
                                                            &nbsp:&nbsp
                                                            <asp:Label ID="txtTimeTable" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblStats" runat="server" Text="<%$ Resources:Attendance, Status %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblPartialStatus" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPartialDesc" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblPartialDescription" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="trInsertLog" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPLInLog" runat="server" Text="<%$ Resources:Attendance,PL In Log %>"></asp:Label>
                                                            &nbsp:&nbsp
                                                            <asp:TextBox ID="txtPLInLog" runat="server" CssClass="form-control" />
                                                            <cc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtPLInLog"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                                ControlToValidate="txtPLInLog" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPLOutLog" runat="server" Text="<%$ Resources:Attendance,PL Out Log %>"></asp:Label>
                                                            &nbsp:&nbsp
                                                            <asp:TextBox ID="txtPLOutLog" runat="server" CssClass="form-control" />
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtPLOutLog"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlExtender="MaskedEditExtender1"
                                                                ControlToValidate="txtPLOutLog" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="div_Pl_Request" visible="false">
                                                            <asp:CheckBox ID="chkModifyPLTime" runat="server" Text="Update Request Time also ?" />
                                                        </div>

                                                    </div>
                                                    <div id="trLogDetail" runat="server" visible="false">
                                                        <div id="pnlPLLogs" runat="server" class="col-md-12">
                                                            <br />
                                                            <div style="overflow: auto">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmp_Log" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%" DataKeyNames="Emp_Id">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType1" runat="server" Text='<%# Convert.ToDateTime(Eval("Event_Time")).ToString("HH:mm") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType2" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Function Key %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType3" runat="server" Text='<%# Eval("Func_Code") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Verified By %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblVerifiedby1" runat="server" Text='<%# Eval("Verified_Type") %>'></asp:Label>
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
                                                <div id="pnlPurchaseRequest" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Request No %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="txtRequestNo" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtRequestDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance, Expected Delivery Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtExpectedDeliveryDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductRequest" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSerialNO" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%# Eval("SuggestedProductName") %>' />
                                                                                        <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# SuggestedProductName(Eval("Product_Id").ToString(),Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--<asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>

                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReqQty" runat="server" Text='<%# Eval("ReqQty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label59" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtDesc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlPurchaseOrder" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,Purchase Order %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtPONo" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Attendance,Purchase Order Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtOrderDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Attendance, Ref. Type %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblRefTypePO" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:Attendance,Ref No. %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblRefNo" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:Attendance,Supplier %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblPOSupplier" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductOrder" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                ShowFooter="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSerialNOOrder" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductcodeOrder" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPOPID" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnitPO" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUIDPO" runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOrderQtyPO" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalamt" runat="server" Text="<%$ Resources:Attendance,Net Price %>" />
                                                                        </FooterTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblamtPO" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblPOFooterAmount" runat="server" Text='0'></asp:Label>
                                                                        </FooterTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPO" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="txtDescPO" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div runat="server" id="pnlSalesQuotation">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label5View" runat="server" Text="<%$ Resources:Attendance,Quotation Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblSQDateView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9View" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblQuotationNoView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11View" runat="server" Text="<%$ Resources:Attendance,Inquiry No. %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblInquiryNoView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12View" runat="server" Text="" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblInquiryDateView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrencySalesQ" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblCurrencyView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14View" runat="server" Text="<%$ Resources:Attendance,Employee%>" />
                                                        &nbsp:&nbsp<asp:Label ID="lblEmployeeView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="pnlDetailView" runat="server" class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetailView" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductcode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("Product_Id").ToString()) %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--  <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />

                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Description %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductDescription1" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantity" runat="server" Text='<%#Eval("Quantity") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnitPrice" Width="50px" runat="server" ForeColor="#4d4c4c" Text='<%#Eval("UnitPrice") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" Width="70px" ReadOnly="true" runat="server" ForeColor="#4d4c4c"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div runat="server" id="pnlSalesOrder">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="txtSODate" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                      <%--  <asp:Label ID="txtSONo" Font-Bold="true" runat="server" />--%>
                                                        <asp:HyperLink Target="_blank" ID="hypSONo" Font-Bold="true" Font-Underline="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOrderType" runat="server" Text="<%$ Resources:Attendance,Order Type %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtOrderType" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEstimateDeliveryDate" runat="server" Text="<%$ Resources:Attendance,Estimate Delivery Date %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtEstimateDeliveryDate" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOverDue" runat="server" Text="Overdue" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtOverDue" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCreditInfo" runat="server" Text="Credit Info" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtCreditInfo" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerStatement" runat="server" Text="Statement Link" />
                                                        &nbsp:&nbsp
                                                        <%--<asp:Label ID="txtCustomerStatement" Font-Bold="true" runat="server" />--%>
                                                        <asp:HyperLink Text="Customer Statement" Target="_blank" ID="hypCutomerStatement" Font-Bold="true" Font-Underline="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblFileDownload" runat="server" Text="File Download" />
                                                        &nbsp:&nbsp
                                                      <asp:Label ID="txtFileDownload" Font-Bold="true" runat="server" />
                                                        <asp:HyperLink Text="File Download" Target="_blank" ID="hypFileDownload" Font-Bold="true"
                                                            Font-Underline="true" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtPaymentMode" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblContactEmail" runat="server" Text="Contact/Email & Mobile No." />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtContactEmail" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>

                                                    <div id="trTransfer" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTransFrom" runat="server" Text="<%$ Resources:Attendance,Transfer From %>" />
                                                            &nbsp:&nbsp<asp:Label ID="txtTransFrom" Font-Bold="true" runat="server" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblQuotationNo" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>" />
                                                            &nbsp:&nbsp
                                                            <asp:Label ID="QuoteNo" Font-Bold="true" runat="server" />
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        &nbsp:&nbsp<asp:Label ID="txtCustomer" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label58" runat="server" Text="<%$ Resources:Attendance,Remarks %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblsalesOrderRemarks" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductDetailQuotation" runat="server" AutoGenerateColumns="False"
                                                                Width="100%">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#UnitName(Eval("UnitId").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantity" runat="server" Text='<%#Eval("Quantity") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvFreeQuantity" runat="server" Text='<%#Eval("FreeQty") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvRemainQuantity" runat="server" Text='<%#Eval("RemainQty") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#SetDecimal(Eval("UnitPrice").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" Text='<%#  SetDecimal((Convert.ToDouble(Eval("UnitPrice").ToString())*Convert.ToDouble(Eval("Quantity").ToString())).ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" Text='<%#SetDecimal((Convert.ToDouble(Eval("DiscountV").ToString())*Convert.ToDouble(Eval("Quantity").ToString())).ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" Text='<%#SetDecimal((Convert.ToDouble(Eval("TaxV").ToString())*Convert.ToDouble(Eval("Quantity").ToString())).ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" Text='<%#SetDecimal(((Convert.ToDouble(Eval("UnitPrice").ToString())-Convert.ToDouble(Eval("DiscountV").ToString())+Convert.ToDouble(Eval("TaxV").ToString()))*Convert.ToDouble(Eval("Quantity").ToString())).ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div runat="server" id="pnlSalesInvoice">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8View" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblSInvDateView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:Attendance,Invoice No. %>"></asp:Label>
                                                        &nbsp:&nbsp 
                                                        <asp:Label ID="lblInvoiceNumberView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label51" runat="server" Text="<%$ Resources:Attendance,Order Type %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblorderTypeView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label53" runat="server" Visible="false" Text="<%$ Resources:Attendance,Sales Order No. %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblSalesOrderNoView" Font-Bold="true" Visible="false" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div id="tr1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:Attendance,Transfer From %>" />
                                                            &nbsp:&nbsp
                                                            <asp:Label ID="lblTransFromView" Font-Bold="true" runat="server"></asp:Label>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label13View" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        &nbsp:&nbsp<asp:Label ID="lblSICustName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvInvoiceProductDetail" runat="server" AutoGenerateColumns="False"
                                                                Width="100%">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#UnitName(Eval("Unit_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantity" runat="server" Text='<%#Eval("Quantity") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div runat="server" id="pnlSalaryIncrement">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblEmployeeName_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label56" runat="server" Text="<%$ Resources:Attendance,Basic Salary %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblBasicsalary_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label57" runat="server" Text="Increment(%)" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblIncPer_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label60" runat="server" Text="Increment Value" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblIncValue_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label62" runat="server" Text="Increment Salary" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblIncSalary_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label61" runat="server" Text="Request Date" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblIncRequestDate_SalInc" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlOvertime" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOvertimeId" runat="server" Text="<%$ Resources:Attendance, OverTime Id %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtOvertimeId" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmployeeC" runat="server" Text="<%$ Resources:Attendance, Employee Code %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtEmployeeCode" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmployeeN" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtEmployeeName" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label70" runat="server" Text="<%$ Resources:Attendance,Status%>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblOTStatus" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label74" runat="server" Text="<%$ Resources:Attendance,OverTime Date%>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblOTDate" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label76" runat="server" Text="<%$ Resources:Attendance,From Time %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblOTFromTime" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label78" runat="server" Text="<%$ Resources:Attendance,To Time%>"></asp:Label>
                                                        &nbsp:&nbsp 
                                                        <asp:Label ID="lblOTtoTime" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label63" runat="server" Text="<%$ Resources:Attendance,Time Duration (In Hours) %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblOTTimeDuration" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label80" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblOTDescription" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlCreditApproval" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblCreditCustomerName" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCreditLimit" runat="server" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtCreditLimit" runat="server" />&nbsp;&nbsp;
                                                                            <asp:Label ID="lblCurrencyCreditLimit" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCreditDays1" runat="server" Text="<%$ Resources:Attendance,Credit Days %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblCreditDays" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label81" runat="server" Text="Current Balance" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="lblCurrentBalance" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label66" runat="server" Text="Credit Parameter" />
                                                        &nbsp:&nbsp
                                                        <asp:RadioButton ID="rbtnAdvanceCheque" runat="server" Style="margin-left: 10px;" Text="Advance Cheque Basis" GroupName="Parameter" Enabled="false" /><br />
                                                        <asp:RadioButton ID="rbtnInvoicetoInvoice" Style="margin-left: 10px;" runat="server" GroupName="Parameter" Text="Invoice to Invoice(First Clear Old Invoice than Issue New invoice)" Enabled="false" /><br />
                                                        <asp:RadioButton ID="rbtnAdvanceHalfpayment" GroupName="Parameter" Style="margin-left: 10px;" runat="server" Text="50% Advance and 50% on Delivery" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label67" runat="server" Text="Financial Statement" />
                                                        &nbsp:&nbsp<asp:LinkButton ID="lnkDownloadFiancialstatement" runat="server" ToolTip="Download" OnClick="btnDownloadFiancialstatement_Click" ForeColor="Blue" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnlPaymentApproval" runat="server">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                        &nbsp:&nbsp<asp:Label ID="txtVoucherNo" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                        &nbsp:&nbsp
                                                        <asp:Label ID="txtVoucherDate" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVNarration" runat="server" Text="<%$ Resources:Attendance,Narration %>" />
                                                        &nbsp:&nbsp 
                                                        <asp:Label ID="txtVNarration" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucherDetail" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Supplier Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdngvOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                            <asp:Label ID="lblgvOtherAccountName" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#SystemParameter.SetDecimal(Eval("Debit_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#SystemParameter.SetDecimal(Eval("Credit_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                            <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#SystemParameter.SetDecimal(Eval("Foreign_Amount").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#SystemParameter.SetDecimal(Eval("Exchange_Rate").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="pnlLeaveSalary" runat="server">

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label75" runat="server" Text="<%$ Resources:Attendance, Employee Code %>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblempcodeValueLS" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label79" runat="server" Text="<%$ Resources:Attendance,Employee Name%>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblempnameValueLS" runat="server"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />


                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeavesalary" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="100%">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="LeaveName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("LeaveName") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>

                                                                        <asp:Label ID="lblTotal" Font-Bold="true" runat="server" Text="Total"></asp:Label>
                                                                    </FooterTemplate>

                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvFromdate" runat="server" Text='<%#GetDate(Eval("from_date").ToString())%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvTodate" runat="server" Text='<%#GetDate(Eval("to_date").ToString())%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Leave">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvtotalLeave" runat="server" Text='<%#Common.GetAmountDecimal(Eval("TotalLeave").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Actual Leave">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvactualLeave" runat="server" Text='<%#Common.GetAmountDecimal(Eval("ActualLeave").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Used Leave">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvusedLeave" runat="server" Text='<%#Common.GetAmountDecimal(Eval("UsedLeave").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Balance Leave">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvbalanceLeave" runat="server" Text='<%#Common.GetAmountDecimal(Eval("BalanceLeave").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Per Day Salary">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvperdaysalary" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Per_Day_Salary").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvtotal" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>

                                                                        <asp:Label ID="lblTotalSum" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                    </div>

                                                </div>

                                                <div id="pnlEmailMarketing" runat="server">

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label69" runat="server" Text="<%$ Resources:Attendance, Template Name %>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lbltemplatename" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label72" runat="server" Text="<%$ Resources:Attendance,Display Text %>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lbldisplayText" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label71" runat="server" Text="<%$ Resources:Attendance,Mail Header %>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblMailHeader" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label73" runat="server" Text="<%$ Resources:Attendance,Total Emails %>" Font-Bold="true"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblTotalEmail" runat="server"></asp:Label>
                                                        <br />

                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>



                                                </div>



                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Label ID="lblDesc" runat="server" Text="<%$ Resources:Attendance,Comment %>"></asp:Label>
                                                    &nbsp:&nbsp<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px"></asp:TextBox>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnApprovePopup" runat="server" CssClass="btn btn-primary" OnClick="btnUpdateApproval_Click"
                                Text="<%$ Resources:Attendance,Approve %>" />

                            <asp:Button ID="btnRejectPopup" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopReject_Click" Text="<%$ Resources:Attendance,Reject %>" />
                            <asp:HiddenField ID="hdnTransId" runat="server" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlApproval1" runat="server" Visible="false"></asp:Panel>

    <div class="modal fade" id="modal_pendings_detail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="up_pendings" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label82" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></span></button>
                            <h4 class="modal-title">
                                <asp:Label ID="Label83" runat="server" Font-Bold="true" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPendingApprovals" runat="server"
                                                            AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                            AllowSorting="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("PairedValues") %>'
                                                                            OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                            TargetControlID="IbtnDelete">
                                                                        </cc1:ConfirmButtonExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approval Person">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvApprovalPerson" runat="server" Text='<%#Eval("approval_person") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approval Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvApprovalType" runat="server" Text='<%#Eval("Approval_Name") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Request Person">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvRequestPerson" runat="server" Text='<%#Eval("request_person") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Pendings">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalPendings" runat="server" Text='<%#Eval("TotalPendings") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Request Location">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRequestLocation" runat="server" Text='<%#Eval("Request_Location") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" Width="30%" />
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
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        <asp:Label ID="Label84" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="up_pendings">
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
        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }



        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;

            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {



                        inputList[i].checked = true;

                    }

                    else {



                        inputList[i].checked = false;

                    }

                }

            }

        }

        function checkItem_All1(objRef) {

            // $('#GvEmpListSelected tbody tr td input[type="checkbox"]').each(function () {
            if (objRef.checked) {
                $(objRef).parents('tr').find(':checkbox').prop('checked', true);
            }
            else {
                // $(this).closest('tr').prop('checked', false);
                $(objRef).parents('tr').find(':checkbox').prop('checked', false);
            }



            // });
        }



        //function checkItem_All1_Header(objRef) {
        //    debugger;
        //    $('#gvShiftView tbody tr td input[type="checkbox"]').each(function () {
        //        debugger;
        //        if (objRef.checked) {
        //            $(this).parents('tr').find(':checkbox').prop('checked', true);
        //        }
        //        else {
        //            // $(this).closest('tr').prop('checked', false);
        //            $(this).parents('tr').find(':checkbox').prop('checked', false);
        //        }
        //    });
        //}


        function checkItem_All1_Header(objRef) {
            debugger;
            $('#gvShiftView tbody tr td input[type="checkbox"]').each(function () {
                debugger;
                if (objRef.checked) {
                    $(this).parents('tr').find(':checkbox').prop('checked', true);
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this).parents('tr').find(':checkbox').prop('checked', false);
                }
            });
        }

        function checkall(data) {
            debugger;
            var table = data.offsetParent.offsetParent.children[0].children;


            $(table).each(function () {
                debugger;
                var gg = $(this)[0].cells[0];
                if (data.checked) {
                    $(this)[0].cells[0].children[0].checked = true;
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this)[0].cells[0].children[0].checked = false;
                }
            });
        }

        function View_Modal_Pendings() {
            $('#modal_pendings_detail').modal("show");
        }
    </script>
</asp:Content>
