<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="OpportunityDashboard.aspx.cs" Inherits="CRM_OpportunityDashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function LI_Edit_Active1() { }
        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/sales_inquiry.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Opportunity Dashboard"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="CRM"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="Opportunity Dashboard"></asp:Label></a></li>
    </ol>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>

            <asp:Button ID="Btn_DetailData" Style="display: none;" data-toggle="modal" data-target="#DetailData" runat="server" Text="Detailed Data" />
            <asp:Button ID="Btn_FillGrid" Style="display: none;" runat="server" OnClick="Btn_FillGrid_Click" />
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

    <div class="tab-content">
        <div class="tab-pane active" id="List">
            <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Btn_FillGrid" EventName="Click" />
                    <asp:PostBackTrigger ControlID="btnExportExcel" />
                </Triggers>
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">

                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div4" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
                                               <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I4" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">


                                            <div class="col-lg-3">
                                                <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                                <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblDepartment" runat="server" Text="Department"> </asp:Label>
                                                <asp:DropDownList ID="ddlDepartment" runat="server" Class="form-control" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Label ID="lblEmp" runat="server" Text="Employee"></asp:Label>
                                                <asp:DropDownList ID="ddlUser" runat="server" Class="form-control" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-3" id="div_productCat" runat="server">
                                                <asp:Label ID="lblProductCat" runat="server" Text="Product Category"></asp:Label>
                                                <asp:DropDownList ID="ddlProductCat" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProductCat_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-12">
                                                <br />
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlInqData" runat="server" Class="form-control" OnSelectedIndexChanged="ddlInqData_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Header" Value="Header"></asp:ListItem>
                                                    <asp:ListItem Text="Detail" Value="Detail"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2" id="DivHeader" runat="server">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" Class="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Opportunity Name" Value="Opportunity_name"></asp:ListItem>
                                                    <asp:ListItem Text="Customer" Value="CustomerName"></asp:ListItem>
                                                    <asp:ListItem Text="Generated By" Value="HandledByName"></asp:ListItem>
                                                    <asp:ListItem Text="Opportunity Amt" Value="Opportunity_amount"></asp:ListItem>
                                                    <asp:ListItem Text="Sales Stage" Value="sales_stage"></asp:ListItem>
                                                    <asp:ListItem Text="Access Type" Value="Field4"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2" runat="server" id="DivDetail">
                                                <asp:DropDownList ID="ddlDetailFieldName" runat="server" Class="form-control" OnSelectedIndexChanged="ddlDetailFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Opportunity Name" Value="Opportunity_name"></asp:ListItem>
                                                    <asp:ListItem Text="Gone Opportunity" Value="CreatedDate/1"></asp:ListItem>
                                                    <asp:ListItem Text="Upcoming Opportunity" Value="CreatedDate/2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption" runat="server" Class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-4">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" runat="server" Class="form-control" Visible="true" placeholder="Search From Content"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlAccesstype" runat="server" Visible="false" Class="form-control">
                                                        <asp:ListItem Text="Public" Value="Public"></asp:ListItem>
                                                        <asp:ListItem Text="Private" Value="Private"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtnumeric" runat="server" Class="form-control" Visible="false" placeholder="Search From Content"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtnumeric" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                  <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnExportExcel" runat="server" CausesValidation="False" OnClick="btnExportExcel_Click" ToolTip="Export Data To Excel"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>



                            <div class="box box-warning box-solid">
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">

                                            <asp:UpdatePanel ID="export" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="exportPDF" />
                                                    <asp:PostBackTrigger ControlID="exportEXCEL" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                            <asp:Button ID="exportPDF" CssClass="btn btn-primary" Visible="false" runat="server" Text="Export PDF" OnClick="exportPDF_Click" />
                                            <asp:Button ID="exportEXCEL" runat="server" Visible="false" CssClass="btn btn-primary" Text="Export Excel" OnClick="exportEXCEL_Click" />

                                        </div>
                                        <div class="col-lg-12">
                                            <br />
                                        </div>
                                        <div class="col-md-12">
                                            <asp:HiddenField runat="server" ID="hdnGvOppoHeaderCurrentPageIndex" Value="1" />
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvListData" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="SInquiryID" CurrentSortDirection="DESC"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" OnSorting="GvListData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle CssClass="InvgridAltRow" BackColor="White" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Opportunity Name" SortExpression="Opportunity_name">
                                                        <ItemTemplate>
                                                            <a onclick="RedirectOpportunity('<%#Eval("SInquiryID") %>')" style="cursor: pointer">
                                                                <%#Eval("Opportunity_name") %>
                                                            </a>                                                            
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Date" SortExpression="IDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGeneratedDate" runat="server" Text='<%#GetDate(Eval("IDate").ToString()) %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CustomerName")%>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sales Stage" SortExpression="sales_stage">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblSalesStage" runat="server" Text='<%#Eval("sales_stage")%>' OnCommand="lblSalesStage_Command" CommandArgument='<%#Eval("SInquiryID")%>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Amount" SortExpression="Opportunity_amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOpportunityAmt" runat="server" Text='<%#SetDecimal(Eval("Opportunity_amount").ToString(),Eval("field1").ToString())+" "+Eval("oppoCurrency_code") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Handled By" SortExpression="HandledByName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHandledByName" runat="server" Text='<%#Eval("HandledByName")%>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Followup Date" SortExpression="followup_date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfollowup_date" runat="server" Text='<%#GetDate(Eval("followup_date").ToString())%>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quotation">
                                                        <ItemTemplate>
                                                            <a onclick="RedirectQuotation('<%#Eval("SInquiryID") %>')" style="cursor: pointer" title="Generate Quotation">
                                                                <i class="fas fa-money-check-alt"></i>
                                                            </a>
                                                        </ItemTemplate>                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FollowUp">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnGenerateFollowup" runat="server" ToolTip="Generate Follow-Up" OnCommand="btnGenerateFollowup_Command" CommandName='<%#Eval("SInquiryID") %>'><i class="fa fa-phone"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                                
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="black" HorizontalAlign="Center" />
                                                <PagerStyle CssClass="pagination-ys" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                
                                            </asp:GridView>


                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDetailData" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="SInquiryID" CurrentSortDirection="DESC"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" OnSorting="gvDetailData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                <AlternatingRowStyle CssClass="InvgridAltRow" BackColor="White" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Opportunity Name" SortExpression="Opportunity_name">
                                                        <ItemTemplate>
                                                            <a onclick="RedirectOpportunity('<%#Eval("SInquiryID") %>')" style="cursor: pointer" title='<%#"Created On: "+GetDate(Eval("IDate").ToString())+" For Amount:"+Eval("Opportunity_amount") %>'>
                                                                <%#Eval("Opportunity_name") %>
                                                            </a>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("CustomerName") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="SuggestedProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("SuggestedProductName") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Unit Price" SortExpression="EstimatedUnitPrice">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstimatedUnitPrice" runat="server" Text='<%#Common.GetAmountDecimal(Eval("EstimatedUnitPrice").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Quantity").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Frequency Of Days" SortExpression="DaysFrequency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDaysFrequency" runat="server" Text='<%#Eval("DaysFrequency") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Created By" SortExpression="CreatedByName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreatedByName" runat="server" Text='<%#Eval("CreatedByName") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Upcoming Opportunity Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpcomingOppo" runat="server" Text='<%# Eval("UpComingOppoDays") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Gone Opportunity Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGoneOppo" runat="server" Text='<%# Eval("GoneOppoDays") %>' />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Last Followup Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfollowup_date" runat="server" Text='<%# GetDate(Eval("followup_date").ToString()) %>' />
                                                        </ItemTemplate>
                                                       
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quotation">
                                                        <ItemTemplate>
                                                            <a onclick="RedirectQuotation('<%#Eval("SInquiryID") %>')" style="cursor: pointer" title="Generate Quotation">
                                                               <i class="fas fa-money-check-alt"></i>
                                                            </a>
                                                        </ItemTemplate>
                                                       
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="FollowUp">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnGenerateFollowup" runat="server" ToolTip="Generate Follow-Up" OnCommand="btnGenerateFollowup_Command" CommandArgument='<%#Eval("Location_ID") %>' CommandName='<%#Eval("SInquiryID") %>'><i class="fa fa-phone"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>

                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="black" HorizontalAlign="Center" />
                                                <PagerStyle CssClass="pagination-ys" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />

                                            </asp:GridView>
                                            <div class="col-md-12">
                                                <asp:Repeater ID="rptPager" runat="server">
                                                    <ItemTemplate>
                                                        <ul class="pagination">
                                                            <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                    CssClass="page-link"
                                                                    OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:Repeater>
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



        <div class="modal fade" id="DetailData" tabindex="-1" role="dialog" aria-labelledby="DetailData_ModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title" id="myNumbers">Detailed Data:</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="up123" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvListData" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="col-md-6">
                                    <asp:Label ID="lblOppoName" runat="server" Text="Opportunity Name:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="OppoName" runat="server"></asp:Label>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lblOppoAmt" runat="server" Text="Opportunity Amt:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="OppAmt" runat="server"></asp:Label>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lblGeneratedBy" runat="server" Text="Generted By:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="GeneratedBy" runat="server" Text=""></asp:Label>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="lblGeneratedOn" runat="server" Text="Generated On:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="GeneratedOn" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblCustomer" runat="server" Text="Generated for:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFollowup" runat="server" Text="Followup Generated:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="tbFollowup" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCall" runat="server" Text="Call Generated:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="tbCall" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblVisit" runat="server" Text="Visit Generated:" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="tbVisit" runat="server" Text=""></asp:Label>
                                </div>



                                <div class="col-md-12">
                                    <br />
                                </div>

                                <div class="col-md-12">
                                    <div id="Div_Box_Add" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="lblAddProductDetail" Font-Bold="true" runat="server" Text="Product History"></asp:Label>
                                            </h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">

                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Product Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvProductId" runat="server" Text='<%#Eval("ProductCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Product Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvProductName" runat="server" Text='<%#Eval("SuggestedProductName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Expected Business Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvExpectedBusiness" ToolTip="Days Between Opportunity and Current Date * Required Frequency" runat="server" Text='<%# Eval("Expectedbusiness")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Expected Business Amt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvExpectedBusiness" runat="server" ToolTip="Opportunity Amount * Expected Business Count" Text='<%# SetDecimal(Eval("ExpectedbusinessAmt").ToString(),Eval("field1").ToString())+" "+Eval("oppoCurrency_code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="col-md-12">
                                    <br />
                                </div>


                                <div class="col-md-12" style="display: none;" id="div_quote" runat="server">
                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="lblGeneratedQuote" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblOpenTotal" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblClosedTotal" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblLostTotal" runat="server" Font-Bold="true"></asp:Label>

                                            </h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">

                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvquoteData" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Quotation No">
                                                            <ItemTemplate>
                                                                <a onclick="quoteRedirect('<%#Eval("SQuotation_Id")%>')" style="cursor: pointer">
                                                                    <%#Eval("SQuotation_No")%>
                                                                </a>
                                                                <%--<asp:LinkButton ID="gvQuotationNo" runat="server" Text='<%#Eval("SQuotation_No")%>' CommandArgument='<%#Eval("SQuotation_Id")%>' OnCommand="gvQuotationNo_Command"></asp:LinkButton>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Quote Amt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvQuoteAmt" runat="server" Text='<%#SetDecimal(Eval("amount").ToString(),Eval("field1").ToString())+""+Eval("quoteCurrency_Code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvReason" runat="server" Text='<%#Eval("Reason")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Generated By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvGeneratedBy" runat="server" Text='<%#Eval("emp_name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Created Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvcreatedDate" runat="server" Text='<%# GetDate(Eval("CreatedDate").ToString())%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-md-12">
                                    <br />
                                </div>


                                <div class="col-md-12" style="display: none;" id="div_order" runat="server">
                                    <div id="Div2" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="lblGeneratedOrder" runat="server" Font-Bold="true"></asp:Label>

                                            </h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I2" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOrderDate" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="OrderID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvOrderID" runat="server" Text='<%#Eval("order_trans_id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Order Amt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvOrderAmt" runat="server" Text='<%#SetDecimal(Eval("NetAmount").ToString(),Eval("field1").ToString())+" "+Eval("orderCurrency_Code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Generated By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvGeneratedBy" runat="server" Text='<%#Eval("emp_name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Created Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvcreatedDate" runat="server" Text='<%# GetDate(Eval("CreatedDate").ToString())%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="col-md-12">
                                    <br />
                                </div>


                                <div class="col-md-12" style="display: none;" id="div_invoice" runat="server">
                                    <div id="Div3" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="lblGeneratedInvoice" runat="server" Font-Bold="true"></asp:Label>


                                            </h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I3" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvInvoiceDate" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="InvoiceID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvInvoiceID" runat="server" Text='<%#Eval("invoice_no")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Invoice Amt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvInvoiceAmt" runat="server" Text='<%# SetDecimal(Eval("grandTotal").ToString(),Eval("field1").ToString())+" "+Eval("invoiceCurrency_code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Generated By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvGeneratedBy" runat="server" Text='<%#Eval("emp_name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Created Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="gvcreatedDate" runat="server" Text='<%# GetDate(Eval("CreatedDate").ToString())%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                    Close</button>--%>
                    </div>
                </div>
            </div>
        </div>



        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup123" Text="Add Followup" />

            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="export">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="up123">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="up1">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="Inv_SalesInquiryHeader" />


        <div class="modal fade" id="Followup123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <div class="modal-body">
                        <AT1:AddFollowup ID="FollowupUC" runat="server" />
                    </div>


                    <div class="modal-footer">
                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                            Close</button>
                    </div>
                </div>
            </div>
        </div>

    </div>

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
        function LI_Customer_Active() {
            $("#Li_Customer_Call").removeClass("active");
            $("#Customer_Call").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function LI_Edit_Active1() {

            $("#Li_FollowupList1").removeClass("active");
            $("#FollowupList1").removeClass("active");

            $("#Li_New1").addClass("active");
            $("#New1").addClass("active");
        }

        function LI_List_Active1() {
            $("#Li_FollowupList").addClass("active");
            $("#FollowupList1").addClass("active");

            $("#Li_New1").removeClass("active");
            $("#New1").removeClass("active");
            btnSearchClick();
        }

        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        <%-- function Li_Tab_FollowupList() {
            document.getElementById('<%= Btn_FollowupList.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }--%>

        function Modal_DetailedDate_Open() {
            document.getElementById('<%= Btn_DetailData.ClientID %>').click();
        }


        function quoteRedirect(id) {
            window.open("../Sales/SalesQuotation.aspx?ReminderID=" + id);
        }

        function RedirectOpportunity(id) {
            window.open("../Sales/SalesInquiry.aspx?OpportunityID=" + id);
        }
        function RedirectQuotation(id) {
            window.open("../Sales/SalesQuotation.aspx?InquiryId=" + id);
        }
        function btnSearchClick() {
            document.getElementById('<%= Btn_FillGrid.ClientID %>').click();
        }
    </script>
    <script src="../JS/gridViewDesign.js"></script>
</asp:Content>

