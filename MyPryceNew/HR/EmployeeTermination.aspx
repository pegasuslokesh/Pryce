<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="EmployeeTermination.aspx.cs" Inherits="HR_EmployeeTermination" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-times"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Termination %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Termination%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>            
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
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

    <div class="modal fade" id="Modal_GST" tabindex="-1" role="dialog" aria-labelledby="Modal_GSTLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="Modal_GSTLabel">Update Name</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_body" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblHandledby" runat="server" Text="Handled By"></asp:Label>
                            <asp:TextBox ID="txtname" runat="server" Class="form-control" OnTextChanged="txtname_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListName" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtname"
                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSaveGST" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="Update" OnClick="btnSaveGST_Click" />

                            <asp:HiddenField ID="hdnEmpIdGST" runat="server" />
                            <asp:Button ID="btnRemoveID" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="Remove" OnClick="btnRemoveID_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnClosePopup">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList runat="server" ID="ddlLocation" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Name %>" Value="EmpName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="EmpCode"></asp:ListItem>
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
                                <div class="box box-warning box-solid" <%= gvTermination.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTermination" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvTermination_PageIndexChanging" OnSorting="gvTermination_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPayslipt" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Employee_Id") %>'
                                                                                    OnCommand="IbtnPayslipt_Command" ToolTip="<%$ Resources:Attendance,Print %>"
                                                                                    ><i class="fa fa-print"></i>Pay Slip</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Employee_Id") %>'
                                                                                    OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"
                                                                                    ><i class="fa fa-print"></i>Account Statement</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFoleUpload" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("EmpCode") %>'
                                                                                    OnCommand="btnFoleUpload_Command" CausesValidation="False"
                                                                                    ToolTip="FileUpload"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="EmpCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompanyId1" runat="server" Text='<%# Eval("EmpCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="EmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Termination Date %>" SortExpression="Termination_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblterminationdate" runat="server" Text='<%# setDateFormat(Eval("Termination_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Handled By" Visible="false" SortExpression="Termination_Date">
                                                                <ItemTemplate>
                                                                    <div class="divTest">
                                                                        <asp:Label ID="lblHandledBy" CssClass="lbl" runat="server" Text='<%# Eval("Emp_Name_PT") %>' onmouseover="ShowImage(this);"></asp:Label>
                                                                        <asp:ImageButton ID="btnEdit1" CssClass="txt" runat="server" OnCommand="btnEdit1_Command" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("Emp_Name_PT") %>'
                                                                            ImageUrl="~/Images/edit.png" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                    </div>
                                                                </ItemTemplate>

                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>


                                                    <style type="text/css">
                                                        .divTest {
                                                        }

                                                            .divTest:hover .lbl {
                                                                display: inline;
                                                            }

                                                        .lbl {
                                                            display: inline;
                                                        }

                                                        .txt {
                                                            var name = document.getElementById("lblHandledBy").innerHTML;
                                                            if(name==null) display:inline;
                                                            else display:none;
                                                        }
                                                    </style>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                        <asp:HiddenField ID="hdnHandledby" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Termination Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTermDate" ErrorMessage="<%$ Resources:Attendance,Enter Termination Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtTermDate" runat="server" CssClass="form-control" />

                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtTermDate_CalendarExtender" OnClientDateSelectionChanged="checkDate" runat="server" Enabled="True"
                                                            TargetControlID="txtTermDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblHandledby2" runat="server" Text="Handled By"></asp:Label>
                                                        <asp:TextBox ID="txtHandledby2" runat="server" Class="form-control" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtHandledby2_textChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListName1" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHandledby2"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="Termination" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Resignation" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Reason %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReason" ErrorMessage="<%$ Resources:Attendance,Enter Reason%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Style="max-height: 150px; min-height: 50px; height: 50px; width: 100%; resize: vertical" TextMode="MultiLine" />
                                                        <br />
                                                    </div>

                                                    <div id="pnlGo" runat="server" class="col-md-12" style="text-align: center">

                                                        <asp:Button ID="btnTerminated" runat="server" CssClass="btn btn-primary" ValidationGroup="Save" OnClick="btnApply1_Click"
                                                            Text="<%$ Resources:Attendance,Terminate %>" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure to Terminate this Employee ? %>"
                                                            TargetControlID="btnTerminated">
                                                        </cc1:ConfirmButtonExtender>
                                                        <asp:HiddenField ID="hdnTerminationId" runat="server" Value="0" />

                                                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click"
                                                            Text="<%$ Resources:Attendance,Reset %>" Visible="true" />

                                                        <%-- <asp:Button ID="btnApply" ValidationGroup="Go" runat="server" CssClass="btn btn-primary" OnClick="btnApply1_Click"
                                                            Text="<%$ Resources:Attendance,Go %>" Visible="true" />--%>
                                                        <%--<asp:Button ID="btnLogProcess" runat="server" CssClass="btn btn-primary" OnClick="btnLogProcess_Click"
                                                            Text="<%$ Resources:Attendance,Log Process %>" Visible="false" ValidationGroup="Go" />
                                                        <asp:Button ID="btnGeneratePayroll" runat="server" CssClass="btn btn-primary" OnClick="btnGeneratePayroll_Click"
                                                            Text="<%$ Resources:Attendance,Generate Payroll %>" Visible="false" ValidationGroup="Go" />--%>
                                                    </div>
                                                    <div id="pnlTermination" runat="server" visible="false">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Last Month Salary %>"></asp:Label>
                                                            <asp:TextBox ID="txtCurrentmonthsalary" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>
                                                        <div style="display: none" class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Next Month Adjustment %>"></asp:Label>
                                                            <asp:TextBox ID="txtMonthadjustment" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Pending Loan Amount %>"></asp:Label>
                                                            <asp:TextBox ID="txtLoanAmount" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Unpaid Leave Salary %>"></asp:Label>
                                                            <asp:TextBox ID="txtLeaveSalary" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>
                                                        <%--<div class="col-md-6">
                                                            <asp:Button ID="btnLeaveSal" runat="server" Text="Get" Visible="false" OnClick="btnLeaveSal_Click" />
                                                            <br />
                                                        </div>--%>
                                                        <div style="display: none;" class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Indemnity Salary %>"></asp:Label>
                                                            <asp:TextBox ID="txtIndemnitySalary" runat="server" CssClass="form-control" Enabled="false" />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Total Amount %>"></asp:Label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" Enabled="false" />
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="lblComp1" runat="server" CssClass="form-control" Text="In Company Currency"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div style="display: none" class="input-group">
                                                                <asp:TextBox ID="txtTotalAmountEmployeeCurrency" runat="server" CssClass="form-control" Enabled="false" />
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="Label15" runat="server" CssClass="form-control" Text="In Company Currency"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div style="display: none;" class="col-md-6">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Paid Amount %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Enter Paid Amount"
                                                                ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>

                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control" Enabled="true" onblur="checkDec(this);" />
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="Label17" runat="server" CssClass="form-control" Text="In Company Currency"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div style="display: none" class="input-group">
                                                                <asp:TextBox ID="txtPaidAmountEmployeeCurrency" runat="server" CssClass="form-control" Enabled="true" />
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="Label18" runat="server" CssClass="form-control" onblur="checkDec(this);" Text="In Employee Currency"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnTerminatePost" runat="server" CssClass="btn btn-primary" ValidationGroup="Save" OnClick="btnTerminatePost_Click"
                                                                Text="<%$ Resources:Attendance,Post & Terminate %>" Visible="true" />
                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to Terminate with Post for this Employee ?"
                                                                TargetControlID="btnTerminatePost">
                                                            </cc1:ConfirmButtonExtender>



                                                            <asp:Button ID="BtnCancel" runat="server" CssClass="btn btn-primary" OnClick="BtnCancel_Click"
                                                                Text="<%$ Resources:Attendance,Cancel %>" Visible="true" />


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


    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="Fileupload123Label" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <%-- <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Fileupload123Label">Approval</h4>
                </div>--%>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <%--<div class="box box-primary">--%>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <AT1:FileUpload1 runat="server" ID="FUpload1" />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Modal_body">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_New">
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
        function Validate(sender, args) {
            var PaidAmount = document.getElementById("<%=txtPaidAmount.ClientID %>");
            var PaidEmployeeCurrency = document.getElementById("<%=txtPaidAmountEmployeeCurrency.ClientID %>");
            if (PaidAmount.value != '' && PaidEmployeeCurrency.value != '') {
                args.IsValid = true;
                return;
            }
            args.IsValid = false;
        }

    </script>
    <script>
        function checkDec(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "";
                alert('Please Type Numeric Values');
            }
        }

        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                alert("Date must be earlier than today !");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>
    <script type="text/javascript">
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
    </script>
</asp:Content>
