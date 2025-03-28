<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="VehicleRentCalculation.aspx.cs" Inherits="Transport_VehicleRentCalculation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calculator"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Vehicle Rent Calculation"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Vehicle Contract%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="Btn_Bin_Click" Text="list" />
        </ContentTemplate>
    </asp:UpdatePanel>
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
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3"
                            runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contract No%>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Vehicle Name%>" Value="Field2"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contractor Name%>" Value="Field3"></asp:ListItem>
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
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlan" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvsalaryPlan_PageIndexChanging" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        AllowSorting="True" OnSorting="GvsalaryPlan_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Transaction Date" SortExpression="Field7">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractdate" runat="server" Text='<%#GetDate(Eval("Trans_date").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractFromdate" runat="server" Text='<%#GetDate(Eval("From_Date").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractTodate" runat="server" Text='<%#GetDate(Eval("To_Date").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract No%>" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltrnDate" runat="server" Text='<%#Eval("Field1")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name%>" SortExpression="Field2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleName" runat="server" Text='<%#Eval("Field2")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contractor Name%>" SortExpression="Field3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractorName" runat="server" Text='<%#Eval("Field3")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rent Amount" SortExpression="Rent_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrentAmount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Rent_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Excess Use" SortExpression="Rent_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExcessUse" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Excess_Use_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deduction Amount" SortExpression="Deduction_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeductionAmount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Deduction_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net Payable" SortExpression="Net_Payable">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNet_Payable" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Net_Payable").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString())%>'></asp:Label>
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
                                                            <asp:Label ID="Label11" runat="server" Text="Transaction Date"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator8" ValidationGroup="Save_1" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txttrnDate" ErrorMessage="<%$ Resources:Attendance,Enter Contract date%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txttrnDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="Calender" runat="server" TargetControlID="txttrnDate" OnClientShown="showCalendar" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="Month"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator7" ValidationGroup="Save_1" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="ddlToMonth" InitialValue="0" ErrorMessage="Select to Month" />
                                                            <asp:DropDownList ID="ddlToMonth" runat="server" CssClass="form-control">
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
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Text="Year"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator2" ValidationGroup="Save_1" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txttoYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txttoYear" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    TargetControlID="txttoYear" FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="btnGetRentList" ValidationGroup="Save_1" runat="server" Text="Rent Calculation"
                                                                        class="btn btn-info" OnClick="btnGetRentList_Click" />
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                                            ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                                            ForeColor="Red"></asp:CustomValidator>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRentCalculation" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            AllowPaging="True" OnPageIndexChanging="gvRentCalculation_PageIndexChanging" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                            AllowSorting="True" OnSorting="gvRentCalculation_Sorting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract No%>" SortExpression="Contract_No">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvselect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvHeaderselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvHeaderselect_OnCheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract No%>" SortExpression="Contract_No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContractNo" runat="server" Text='<%#Eval("Contract_No")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name%>" SortExpression="VehicleName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVehicleName" runat="server" Text='<%#Eval("VehicleName")%>'></asp:Label>
                                                                        <asp:Label ID="lblvehicleId" runat="server" Visible="false" Text='<%#Eval("Vehicle_Id")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Contractor Name" SortExpression="ContractorName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContractorName" runat="server" Text='<%#Eval("ContractorName")%>'></asp:Label>
                                                                        <asp:Label ID="lblContractorId" runat="server" Visible="false" Text='<%#Eval("Contractor_Id")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rent Amount" SortExpression="Rent_Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblrentAmount" runat="server" Text='<%#Eval("Rent_Amount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Excess Use Amount" SortExpression="Excess_Use_Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblexcessAmount" runat="server" Text='<%#Eval("Excess_Use_Amount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Deduction" SortExpression="Total_Deduction">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeductionAmount" runat="server" Text='<%#Eval("Total_Deduction")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Net Amount" SortExpression="NetPayable">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNetAmount" runat="server" Text='<%#Eval("NetPayable")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator_txtpaymentdebitaccount" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtpaymentdebitaccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
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
                                                            <asp:TextBox ID="txtpaymentCreditaccount" runat="server" Enabled="false" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>" Visible="false"
                                                            class="btn btn-primary" OnClick="Btn_Save_Click" />
                                                        <asp:Button ID="btn_Post" runat="server" ValidationGroup="Save" Text="Save & Post"
                                                            class="btn btn-success" OnClick="Btn_Post_Click" Visible="false" Enabled="false" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure to post record ?%>"
                                                            TargetControlID="btn_Post">
                                                        </cc1:ConfirmButtonExtender>
                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset%>"
                                                            class="btn btn-primary" OnClick="Btn_Reset_Click" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                            class="btn btn-danger" OnClick="Btn_Cancel_Click" />
                                                    </div>
                                                    <asp:HiddenField ID="hdnEditId" runat="server" />
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
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Contract No%>" Value="Contract_No"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Contractor Name%>" Value="Field3"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Vehicle Name%>" Value="Field2"></asp:ListItem>
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
                                            <div class="col-lg-3">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                    <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnbinbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                <asp:ImageButton ID="btnbinRefresh" Style="width: 33px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                <asp:ImageButton ID="imgBtnRestore" Style="width: 33px;" CausesValidation="False" Visible="false"
                                                    runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" />
                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                            </div>
                                            <div class="col-lg-2">
                                                <h5 class="text-center">
                                                    <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlanBin" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" DataKeyNames="Trans_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        OnPageIndexChanging="GvsalaryPlanBin_PageIndexChanging" OnSorting="GvsalaryPlanBin_Sorting"
                                                        AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract No%>" SortExpression="Contract_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltrnDate" runat="server" Text='<%#Eval("Contract_No")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contract Date%>" SortExpression="Field7">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractdate" runat="server" Text='<%#GetDate(Eval("Field7").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date%>" SortExpression="From_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractFromdate" runat="server" Text='<%#GetDate(Eval("From_Date").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date%>" SortExpression="To_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractTodate" runat="server" Text='<%#GetDate(Eval("To_Date").ToString())%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contractor Name%>" SortExpression="Field3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractorName" runat="server" Text='<%#Eval("Field3")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name%>" SortExpression="Field2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleName" runat="server" Text='<%#Eval("Field2")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
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
    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false">
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
        function Grid_Validate(sender, args) {
            var gridView = document.getElementById("<%=gvRentCalculation.ClientID %>");
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
