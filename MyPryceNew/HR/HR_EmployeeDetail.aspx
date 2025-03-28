<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="HR_EmployeeDetail.aspx.cs" Inherits="HR_HR_EmployeeDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Attendance Detail%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Attendance Detail%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnUpload_Click" Text="Bin" />
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

    <div id="myTab" class="nav-tabs-custom">
        <ul class="nav nav-tabs pull-right bg-blue-gradient">
            <li id="Li_Upload"><a onclick="Li_Tab_Upload()" href="#Upload" data-toggle="tab">
                <i class="fa fa-upload"></i>&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>
            <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                <i class="fa fa-trash"></i>&nbsp;&nbsp;
                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
            <li id="Li_New"><a href="#New" onclick="Li_Tab_New()" data-toggle="tab">
                <asp:UpdatePanel ID="Update_Li" runat="server">
                    <ContentTemplate>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_New_tab" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </a></li>
            <li class="active" id="Li_List"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                <i class="fa fa-list"></i>&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                            <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Name%>" Value="Emp_Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-5">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                            </asp:Panel>

                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="btnbind"  runat="server"
                                                CausesValidation="False"  OnClick="btnbind_Click"
                                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="btnRefresh" runat="server"  CausesValidation="False"
                                                OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="box box-warning box-solid"  <%= gvEmpDetailMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                         
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:HiddenField ID="Edit_ID" runat="server" />
                                            <asp:HiddenField ID="HDFSort" runat="server" />
                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Post" runat="server" ErrorMessage="Please select at least one record."
                                                ClientValidationFunction="Validate_Grid" ForeColor="Red"></asp:CustomValidator>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpDetailMaster" runat="server" AutoGenerateColumns="False" Width="100%"
                                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvEmpDetailMaster_PageIndexChanging"
                                                OnSorting="gvEmpDetailMaster_OnSorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                 CausesValidation="False" OnCommand="btnEdit_Command"
                                                                 ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i> </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged"
                                                                AutoPostBack="true" />
                                                            <asp:Label ID="lblFileId" runat="server" Visible="false" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmp_Name" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" SortExpression="Month">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonth" runat="server" Text='<%#GetMonth(Eval("Month").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>" SortExpression="Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Present Days%>" SortExpression="Present_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPresent_Days" runat="server" Text='<%# Eval("Present_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Days%>" SortExpression="Holiday_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoliday_Days" runat="server" Text='<%# Eval("Holiday_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Week Off Days%>" SortExpression="WeekOff_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInterviewDate" runat="server" Text='<%# Eval("WeekOff_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Days %>" SortExpression="Leave_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeave_Days" runat="server" Text='<%# Eval("Leave_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Absent Days %>" SortExpression="Absent_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAbsent_Days" runat="server" Text='<%# Eval("Absent_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Days %>" SortExpression="Total_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal_Days" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>


                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                        </div>
                                        <div style="text-align: center;">
                                            <br />
                                            <asp:Button ID="btnAllPost" runat="server" ValidationGroup="Post" Visible="false" OnClick="btnAllPost_Click" CssClass="btn btn-primary"
                                                Text="<%$ Resources:Attendance,Post %>" />
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
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,User%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblEmpName" runat="server" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtEmpName" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtEmpName_OnTextChanged"
                                                        runat="server" CssClass="form-control" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListEmpName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblMonth" runat="server" Text="<%$ Resources:Attendance,Month%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="ddlMonth" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />
                                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblYear" runat="server" Text="<%$ Resources:Attendance,Year%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="TxtYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="TxtYear" runat="server" MaxLength="4" CssClass="form-control" AutoPostBack="true"
                                                        OnTextChanged="TxtYear_TextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        TargetControlID="TxtYear" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblTotalDays" runat="server" Text="<%$ Resources:Attendance,Total Month Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtMonthDays" ErrorMessage="<%$ Resources:Attendance,Enter Total Month Days %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtMonthDays" MaxLength="10" runat="server" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                        TargetControlID="txtMonthDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblPresentDays" runat="server" Text="<%$ Resources:Attendance,Present Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtPresentDays" ErrorMessage="<%$ Resources:Attendance,Enter Present Days %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtPresentDays" runat="server" MaxLength="4" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        TargetControlID="txtPresentDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblHoliday" runat="server" Text="<%$ Resources:Attendance,Holiday Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtHoliday" ErrorMessage="<%$ Resources:Attendance,Enter Holiday Days%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtHoliday" runat="server" MaxLength="4" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtHoliday" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblWeekOff" runat="server" Text="<%$ Resources:Attendance,Week Off Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtWeekoffDays" ErrorMessage="<%$ Resources:Attendance,Enter Week Off Days %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtWeekoffDays" runat="server" MaxLength="4" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtWeekoffDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="LabelAbsent" runat="server" Text="<%$ Resources:Attendance,Absent Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator8" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtAbsentDays" ErrorMessage="<%$ Resources:Attendance,Enter Absent Days %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtAbsentDays" runat="server" MaxLength="4" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        TargetControlID="txtAbsentDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblLeaveDays" runat="server" Text="<%$ Resources:Attendance,Leave Days%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator9" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtLeaveDays" ErrorMessage="<%$ Resources:Attendance,Enter Leave Days %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtLeaveDays" runat="server" MaxLength="4" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                        TargetControlID="txtLeaveDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblLatePenalty" runat="server" Text="<%$ Resources:Attendance,Late Penalty Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator10" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtLatePenaltyMin" ErrorMessage="<%$ Resources:Attendance,Enter Late Penalty Min %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtLatePenaltyMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtLatePenaltyMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblEarlyPenaltyMin" runat="server" Text="<%$ Resources:Attendance,Early Penalty Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator11" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtEarlyPenaltyMin" ErrorMessage="<%$ Resources:Attendance,Enter Early Penalty Min %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtEarlyPenaltyMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                        TargetControlID="txtEarlyPenaltyMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblPartialPenaltyMin" runat="server" Text="<%$ Resources:Attendance,Partial Penalty Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator12" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtPartialPenaltyMin" ErrorMessage="<%$ Resources:Attendance,Enter Partial Penalty Min %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtPartialPenaltyMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                        TargetControlID="txtPartialPenaltyMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblNormalOTMin" runat="server" Text="<%$ Resources:Attendance,Normal OT Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator13" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtNormalOTMin" ErrorMessage="<%$ Resources:Attendance,Enter Normal OT Min%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtNormalOTMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                        TargetControlID="txtNormalOTMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblWeekOffOTMin" runat="server" Text="<%$ Resources:Attendance,Week Off OT Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator14" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtWeekOffOTMin" ErrorMessage="<%$ Resources:Attendance,Enter Week Off OT Min%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtWeekOffOTMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                        TargetControlID="txtWeekOffOTMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblHolidayOTMin" runat="server" Text="<%$ Resources:Attendance,Holiday OT Min%>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator15" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtHolidayOTMin" ErrorMessage="<%$ Resources:Attendance,Enter Holiday OT Min%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtHolidayOTMin" runat="server" MaxLength="3" CssClass="form-control" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                        TargetControlID="txtHolidayOTMin" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div style="display: none;">
                                                    <asp:Label ID="lblPost" runat="server" Style="color: Green; margin-left: 100px;"></asp:Label>
                                                    <asp:Label ID="lblNonPost" runat="server" Style="color: Red; margin-left: 100px;"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div style="height: 50px; text-align: center;" class="form-control">
                                    <asp:UpdatePanel ID="Update_New_Button" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click"
                                                Visible="true" CssClass="btn btn-success" ValidationGroup="Save" />
                                            <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" />
                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                OnClick="btnCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />
                                            <asp:HiddenField ID="editid" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
						<asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                        <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Name%>" Value="Emp_Name"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList ID="ddlbinOption" runat="server" class="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-5">
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                            <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center;">
                                        <asp:LinkButton ID="btnbinbind"  runat="server"
                                            CausesValidation="False"  OnClick="btnbinbind_Click"
                                            ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnbinRefresh" runat="server"  CausesValidation="False"
                                            OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="imgBtnRestore"  runat="server"  CausesValidation="False"
                                            OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="ImgbtnSelectAll"  runat="server"  CausesValidation="False"
                                             OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance,Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                    </div>
                                  
				</div>
			</div>
		</div>
	</div>
                        <div class="box box-warning box-solid"  <%= gvEmpDetailBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                           
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpDetailBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvEmpDetailBin_PageIndexChanging"
                                                OnSorting="gvEmpDetailBin_OnSorting" AllowSorting="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBEmp_Name" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month%>" SortExpression="Month">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBMonth" runat="server" Text='<%#GetMonth(Eval("Month").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year%>" SortExpression="Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Present Days%>" SortExpression="Present_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBPresent_Days" runat="server" Text='<%# Eval("Present_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Days%>" SortExpression="Holiday_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBHoliday_Days" runat="server" Text='<%# Eval("Holiday_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Week Off Days%>" SortExpression="WeekOff_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBInterviewDate" runat="server" Text='<%# Eval("WeekOff_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Days %>" SortExpression="Leave_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBLeave_Days" runat="server" Text='<%# Eval("Leave_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Absent Days %>" SortExpression="Absent_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBAbsent_Days" runat="server" Text='<%# Eval("Absent_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Days %>" SortExpression="Total_Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBTotal_Days" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>


                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="Upload">
                <asp:UpdatePanel ID="Update_Upload" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Search%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:HyperLink ID="HyperLink2" Visible="true" runat="server" NavigateUrl="~/CompanyResource/EmployeeSalaryDetail .xlsx"
                                                    Text="<%$ Resources:Attendance,Download Excel To Upload Employee Monthly Transaction %>"></asp:HyperLink>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-4">
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label runat="server" Text="Browse Excel File" ID="Label66"></asp:Label>
                                                    <div class="input-group" style="width: 100%;">
                                                        <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                            runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                        <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                            <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                            <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                            <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                        </div>
                                                    </div>

                                                    <br />
                                                    <div style="text-align: center;">
                                                        <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                            OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                                                    </div>
                                                    <br />
                                                    <asp:Label ID="Label67" runat="server" Text="Select Sheet"></asp:Label>
                                                    <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <div style="text-align: center;">
                                                        <asp:Button ID="btnviewcolumns" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                            OnClick="btnviewcolumns_Click" Visible="true" Text="<%$ Resources:Attendance,Map Column %>" />
                                                    </div>
                                                    <br />
                                                    <div id="div_Grid" runat="server" style="height: 500px; display: none; overflow: auto;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFieldMapping" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            DataKeyNames="Nec" OnRowDataBound="gvFieldMapping_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCompulsery" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Column %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColName" runat="server" Text='<%# Eval("Column_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlExcelCol" CssClass="form-control" runat="server">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            
                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                                        <div style="text-align: center;">
                                                            <asp:Button ID="btnUploadTemp" CssClass="btn btn-primary" runat="server" OnClick="btnUpload_Click2"
                                                                Text="<%$ Resources:Attendance,Show Data %>" />&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btncancel1" CssClass="btn btn-primary" runat="server" OnClick="btncancel_Click"
                                                                Text="<%$ Resources:Attendance,Reset %>" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Div_showdata" runat="server" style="display: none;">
                                    <div class="box box-warning box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title"></h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected" runat="server" Width="100%">

                                                            
                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: center">
                                                    <asp:Button ID="Button21" runat="server" CssClass="btn btn-primary" OnClick="btnUpload_Click1"
                                                        Text="<%$ Resources:Attendance,Upload Data %>" />
                                                    <asp:Button ID="btnBackToMapData" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Back To FileUpload %>"
                                                        OnClick="btnBackToMapData_Click" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Button21"
                                                        ConfirmText="Are you sure to Save Records in Database.">
                                                    </cc1:ConfirmButtonExtender>
                                                    <asp:DropDownList ID="ddlFiltercol" Visible="false" CssClass="form-control" Height="25px"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtfiltercol" Visible="false" CssClass="form-control" runat="server"></asp:TextBox>
                                                    &nbsp;&nbsp;
                                                        <asp:Button ID="btnFilter" CssClass="btn btn-primary" Visible="false" runat="server"
                                                            OnClick="btnFilter_Click" Text="<%$ Resources:Attendance,Filter %>" />
                                                    &nbsp;&nbsp;
                                                        <asp:Button ID="btnresetgv" CssClass="btn btn-primary" Visible="false" runat="server"
                                                            OnClick="btnresetgv_Click" Text="<%$ Resources:Attendance,Reset %>" />
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


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_New_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

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
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Upload">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:Panel ID="pnlUpload1" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlUpload" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlEmpUpload" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false">
    </asp:Panel>
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

        function on_Edit_tab_position() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

    </script>
    <script type="text/javascript">


        function DisableCheckBoxes(treeviewID) {
            TREEVIEW_ID = treeviewID;

            var treeView = document.getElementById(TREEVIEW_ID);

            if (treeView) {
                var childCheckBoxes = treeView.getElementsByTagName("input");
                for (var i = 0; i < childCheckBoxes.length; i++) {
                    var textSpan = GetCheckBoxTextSpan(childCheckBoxes[i]);

                    if (textSpan.firstChild)
                        if (textSpan.firstChild.className == "disabledTreeviewNode")
                            childCheckBoxes[i].disabled = true;
                }
            }
        }

        function GetCheckBoxTextSpan(checkBox) {
            // Set label text to node name
            var parentDiv = checkBox.parentNode;
            var nodeSpan = parentDiv.getElementsByTagName("span");

            return nodeSpan[0];
        }


        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
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
        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
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
    <script type="text/javascript">
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function uploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }

        function Validate_Grid(sender, args) {
            var gridView = document.getElementById("<%=gvEmpDetailMaster.ClientID %>");
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
