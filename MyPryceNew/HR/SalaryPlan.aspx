<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalaryPlan.aspx.cs" Inherits="HR_SalaryPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Salary Plan%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Salary Plan%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="Btn_List_Click" Text="list" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="Btn_New_Click" Text="list" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="Btn_Bin_Click" Text="list" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Plan Name" Value="Plan_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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

                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvsalaryPlan.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlan" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="GvsalaryPlan_PageIndexChanging" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        AllowSorting="True" OnSorting="GvsalaryPlan_Sorting">

                                                        <Columns>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnEdit_Command" Visible="false" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/Erase.png" Visible="false" OnCommand="IbtnDelete_Command"
                                                                        Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Plan_Name" HeaderText="Plan Name"
                                                                SortExpression="Plan_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="Created_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">

                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Plan Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="Txt_Plan_Name" ErrorMessage="Enter Plan Name"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="Txt_Plan_Name" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Gross Salary %>"></asp:Label>
                                                            <asp:TextBox ID="Txt_Gross_Salary" runat="server" class="form-control" OnTextChanged="Txt_Gross_Salary_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">


                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Basic Salary %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="Txt_Basic_Salary" ErrorMessage="Enter Basic Salary(%)"></asp:RequiredFieldValidator>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="Txt_Basic_Salary" runat="server" CssClass="form-control" OnTextChanged="Txt_Gross_Salary_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <span class="input-group-addon">%</span>

                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                    TargetControlID="Txt_Basic_Salary" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>

                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Enabled="false" Text="<%$ Resources:Attendance,Basic Salary Amount %>"></asp:Label>
                                                            <asp:TextBox ID="Txt_Basic_Sal_Amt" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Allowance%>"></asp:Label>

                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator2" ValidationGroup="Add" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="Txt_Allowance" ErrorMessage="Enter Allowance Name"></asp:RequiredFieldValidator>


                                                            <asp:TextBox ID="Txt_Allowance" runat="server" BackColor="#eeeeee" CssClass="form-control" OnTextChanged="Txt_Allowance_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="Txt_Allowance_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" ServiceMethod="Get_Allowance" ServicePath=""
                                                                CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Allowance"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">

                                                            <asp:CheckBox ID="chkAdjustRemainingAmount" runat="server" CssClass="form-control" Text="Adjust Remaining Amount" AutoPostBack="true" OnCheckedChanged="chkAdjustRemainingAmount_OnCheckedChanged" />

                                                        </div>

                                                        <asp:HiddenField ID="hdnEditId" runat="server" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Calculation Method1%>"></asp:Label>
                                                            <asp:DropDownList ID="Ddl_Calculation_Method" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="Ddl_Calculation_Method_OnTextChanged">

                                                                <asp:ListItem Text="Fixed" Value="Fixed"></asp:ListItem>
                                                                <asp:ListItem Text="Percent" Value="Percent" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>

                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator4" ValidationGroup="Add" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="Txt_Value" ErrorMessage="Enter Value"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="Txt_Value" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="Ddl_Calculation_Method_OnTextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                TargetControlID="Txt_Value" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Enabled="false" Text="<%$ Resources:Attendance,Amount %>"></asp:Label>
                                                            <asp:TextBox ID="Txt_Amount" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Enabled="false" Text="<%$ Resources:Attendance,Applicable Deduction %>"></asp:Label>
                                                            <asp:TextBox ID="Txt_Applicable_Deduction" BackColor="#eeeeee" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="Txt_Applicable_Deduction_OnTextChanged"></asp:TextBox>

                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="Get_Applicable_Deduction"
                                                                ServicePath="" TargetControlID="Txt_Applicable_Deduction" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>




                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: right">
                                                        <asp:Button ID="Btn_Add" runat="server" ValidationGroup="Add" Text="<%$ Resources:Attendance,Add%>"
                                                            class="btn btn-warning" OnClick="Btn_Add_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" OnClick="btndeductionCancel_Click" Text="cancel" CssClass="btn btn-warning" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalaryPlanDetail" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" DataKeyNames="Trans_Id" ShowFooter="false">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Edit">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgBtnEmployeeEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                Height="16px" ImageUrl="~/Images/Edit.png"
                                                                                Width="16px" OnCommand="imgBtnEmployeeEdit_Command" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnEmpoloyeeDelete_Command" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Allowance">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAllowanceName" runat="server" Text='<%#getAllowanceNamebyId(Eval("Ref_Id").ToString()) %>' />
                                                                            <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                            <asp:Label ID="lblAllowanceId" Visible="false" runat="server" Text='<%#Eval("Ref_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Calculation Method">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcalcMethod" runat="server" Text='<%#Eval("Calculation_Method") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Calculated Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCalculatedAmount" runat="server" Text='<%#Eval("Amount") %>' />
                                                                            <asp:Label ID="lblisadjustRemaining" runat="server" Text='<%#Eval("Field1") %>' Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Deduction Applicable">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label15" runat="server" Text='<%#getDeductionNamebyId(Eval("Deduction_Applicable").ToString()) %>' />
                                                                            <asp:Label ID="lbldeductionApplicable" runat="server" Text='<%#Eval("Deduction_Applicable") %>' Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>

                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="hdndeductionTransId" runat="server" />
                                                        </div>

                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>"
                                                            class="btn btn-success" OnClick="Btn_Save_Click" Visible="false" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            class="btn btn-danger" OnClick="Btn_Cancel_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>





                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label16" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Plan Name" Value="Plan_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                        runat="server" OnClick="imgBtnRestore_Click"
                                                        ToolTip="<%$ Resources:Attendance, Active %>"><span class="fas fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                  
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvsalaryPlanBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlanBin" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" DataKeyNames="Trans_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' OnPageIndexChanging="GvsalaryPlanBin_PageIndexChanging"
                                                        OnSorting="GvsalaryPlanBin_Sorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Plan_Name" HeaderText="Plan Name"
                                                                SortExpression="Plan_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="Created_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>


                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false"></asp:Panel>
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_List_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function LI_Bin_Active() {
            $("#Li_Bin").removeClass("active");
            $("#Bin").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }
    </script>
</asp:Content>




