<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_GratuityPlan.aspx.cs" Inherits="HR_HR_GratuityPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-briefcase"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Indeminity/Gratuity Plan"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="Indeminity/Gratuity Plan"></asp:Label></a></li>


    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" runat="server" Style="display: none;" Text="List" OnClick="btnList_Click" />
            <asp:Button ID="Btn_New" runat="server" Style="display: none;" Text="New" OnClick="btnNew_Click" />
            <asp:Button ID="Btn_Bin" runat="server" Style="display: none;" Text="Bin" OnClick="btnBin_Click" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="Update_Progress1" runat="server" AssociatedUpdatePanelID="Update_Button">
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
                    <li id="Li_Bin"><a onclick="Li_Bin_Click()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_New_Click()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_List_Click()" href="#List" data-toggle="tab">
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
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
                                                        <asp:ListItem Selected="True" Text="Plan Name"
                                                            Value="Plan_Name"></asp:ListItem>

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
                                <div class="box box-warning box-solid" <%= gvLeaveMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMaster" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvLeaveMaster_PageIndexChanging" OnSorting="gvLeaveMaster_OnSorting">
                                                        <Columns>
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

                                                            <asp:TemplateField HeaderText="Plan Name" SortExpression="Plan_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Plan_Name") %>'></asp:Label>
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
                                                        <asp:Label ID="Label5" runat="server" Text="Plan Name"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator3" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtPlanName" ErrorMessage="Enter Plan Name"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtPlanName" runat="server"
                                                            CssClass="form-control" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="Eligible Month"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator1" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtEligibleMonth" ErrorMessage="Enter Eligible Month"></asp:RequiredFieldValidator>


                                                        <asp:TextBox ID="txtEligibleMonth" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtEligibleMonth" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="Benefit Amount Limit"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator2" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtBenefitAmountLimit" ErrorMessage="Enter Benefit Amount Limit"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtBenefitAmountLimit" runat="server" AutoPostBack="true" OnTextChanged="txtBenefitAmountLimit_OnTextChanged"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                            TargetControlID="txtBenefitAmountLimit" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="Benefit Wage Month"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator4" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitwagemonth" ErrorMessage="Enter Benefit Wage Month"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitwagemonth" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtBenefitAmountLimit_OnTextChanged" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitwagemonth" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Label ID="Label17" runat="server" Text="Service Calculation Period"></asp:Label>

                                                        <asp:RadioButton ID="rbtnserviceCalc_Nearestinteger" runat="server" Text="Rounded of nearest Year" GroupName="srccalc" />
                                                        <asp:RadioButton ID="rbtnserviceCalc_proratebasis" runat="server" Text="Pro rate basis" GroupName="srccalc" />


                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label18" runat="server" Text="Applicable Allowances"></asp:Label>


                                                        <a style="color: Red">*</a>



                                                        <asp:TextBox ID="txtApplicableAllowance" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtLsApplicableAllowances_OnTextChanged" />


                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=","
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="Get_Applicable_Allowance"
                                                            ServicePath="" TargetControlID="txtApplicableAllowance" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">

                                                        <br />
                                                        <asp:CheckBox ID="chkIsTaxFree" runat="server" Text="Is Tax Free" />

                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">

                                                        <br />
                                                        <asp:CheckBox ID="chkIsforefeitprovision" runat="server" Text="Is Forefeit provision" />

                                                        <br />
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label19" runat="server" Text="Month Days Count(For Calculate per day salary)"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator5" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtMonthDaysCount" ErrorMessage="Enter Month Day Count"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtMonthDaysCount" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                            TargetControlID="txtMonthDaysCount" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Benefit(%) on termination"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator6" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitperonTermination" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitperonTermination" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitperonTermination" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="Benefit(%) on resign"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator7" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitperonresign" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitperonresign" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitperonresign" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label22" runat="server" Text="Benefit(%) on retirement"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator8" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitperonretirement" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitperonretirement" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitperonretirement" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label23" runat="server" Text="Benefit(%) on death"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator9" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitperondeath" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitperondeath" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitperondeath" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="Benefit(%) on other"></asp:Label>

                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator10" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtbenefitperonother" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtbenefitperonother" runat="server" CssClass="form-control" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                            TargetControlID="txtbenefitperonother" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />

                                                        <asp:CheckBox ID="chkisabsent" runat="server" Text="Count absent days for indemnity" CssClass="form-control" />

                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />

                                                        <asp:CheckBox ID="chkisweekoff" runat="server" Text="Count week off days for indemnity" CssClass="form-control" />

                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />

                                                        <asp:CheckBox ID="chkispaidleave" runat="server" Text="Count paid leave days for indemnity" CssClass="form-control" />

                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />

                                                        <asp:CheckBox ID="chkisunpaidLeave" runat="server" Text="Count unpaid leave days for indemnity" CssClass="form-control" />

                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />

                                                        <asp:CheckBox ID="chkisholiday" runat="server" Text="Count holidays for indemnity" CssClass="form-control" />

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
                                                        <asp:Label ID="Label4" runat="server" Text="Gratuity Days Detail" Font-Bold="true"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="From year(In Days)"></asp:Label>
                                                        <asp:TextBox ID="txtFromYear" runat="server" CssClass="form-control" Enabled="false" Text="0" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDays" runat="server" Enabled="True"
                                                            TargetControlID="txtFromYear" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="To Year(In Days)"></asp:Label>
                                                        <asp:TextBox ID="txttoYear" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDaysto" runat="server" Enabled="True"
                                                            TargetControlID="txttoYear" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="Remuneration Days/Years"></asp:Label>
                                                        <asp:TextBox ID="txtremunerationDays" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtdeduction" runat="server" Enabled="True"
                                                            TargetControlID="txtremunerationDays" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button ID="btndeduction" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                            CssClass="btn btn-primary" OnClick="btndeduction_Click" />

                                                        <asp:Button ID="btndeductionCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-primary" OnClick="btndeductionCancel_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDeductionDetail" runat="server" AutoGenerateColumns="False"
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
                                                                <asp:TemplateField HeaderText="From Year(In Days)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("From_Year") %>' />
                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To Year(In Days)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("To_Year") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remuneration Days/Years">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeductionpercentage" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Remuneration_days").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdndeductionTransId" runat="server" />
                                                        <br />
                                                    </div>


                                                    <hr />
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                        <br />
                                                        <asp:HiddenField ID="editid" runat="server" />
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
                                                    <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="Plan Name"
                                                            Value="Plan_Name"></asp:ListItem>
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
                                                 <div class="col-lg-3">
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
                                <div class="box box-warning box-solid" <%= gvLeaveMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_Id" Width="100%" AllowPaging="True" OnPageIndexChanging="gvLeaveMasterBin_PageIndexChanging"
                                                        OnSorting="gvLeaveMasterBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Plan Name" SortExpression="Plan_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Plan_Name") %>'></asp:Label>
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Visible="false"></asp:Panel>
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

        function Li_List_Click() {
            document.getElementById("<%=Btn_List.ClientID %>").click();
        }

        function Li_New_Click() {
            document.getElementById("<%=Btn_New.ClientID %>").click();
        }

        function Li_Bin_Click() {
            document.getElementById("<%=Btn_Bin.ClientID %>").click();
        }

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

    </script>
</asp:Content>

