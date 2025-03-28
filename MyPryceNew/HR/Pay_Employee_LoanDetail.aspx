<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="Pay_Employee_LoanDetail.aspx.cs" Inherits="HR_Pay_Employee_LoanDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Loan Detail%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Loan Detail%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Loan" runat="server">
        <ContentTemplate>

            <div class="box box-warning box-solid">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">

                            <div class="row">
                                <asp:HiddenField runat="server" ID="hdnCanView" />
                                <div class="col-md-4">
                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-4">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLoanType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Running" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Closed" Value="2"></asp:ListItem>

                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-2">
                                    <br />
                                    <asp:LinkButton ID="btnfilter" Style="" runat="server" OnClick="btnfilter_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                </div>

                                <div class="col-md-2" style="text-align: right;">
                                    <br />
                                    <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                               
                                </div>


                                <div class="col-md-12" <%= GridViewLoan.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewLoan" runat="server" AllowPaging="True" AllowSorting="true"
                                            AutoGenerateColumns="False" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                            OnPageIndexChanging="GridViewLoan_PageIndexChanging" OnSorting="GridViewLoan_Sorting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Loan_Id") %>'
                                                            CausesValidation="False" OnCommand="btnEdit_command" Visible='<%# hdnCanView.Value=="true"?true:false%>'
                                                            ToolTip="<%$ Resources:Attendance,View %>"><i class="fa fa-eye"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Loan_Id") %>'
                                                            ImageUrl="~/Images/Erase.png"  ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                --%>
                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Id %>" SortExpression="Loan_Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpIdList" runat="server" Text='<%# Eval("Loan_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle  />
                                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Request Date" SortExpression="Loan_Request_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvLoanRequestDate" runat="server"  Text='<%# GetDate(Eval("Loan_Request_Date")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approval Date" SortExpression="Loan_Approval_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvLoanApprovalDate" runat="server" Text='<%# GetDate(Eval("Loan_Approval_Date")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Emp Code %>" SortExpression="Emp_Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpIdList" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Name %>" SortExpression="Loan_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltynameList" runat="server" Text='<%# Eval("Loan_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,No Of Installments%>" SortExpression="Loan_Duration">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_No_Of_Installments" runat="server" Text='<%# Eval("Loan_Duration") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Principal Amount %>" SortExpression="Loan_Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Principal_Amount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Loan_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interest Rate%>" SortExpression="Loan_Interest">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Interest_Rate" runat="server" Text='<%# GetIntrest(Eval("Loan_Interest")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Interest Amount %>" SortExpression="Loan_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Interest_Amount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Intrest_Amt").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Amount %>" SortExpression="Gross_Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Net_Amount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Gross_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Currency" runat="server" Text='<%#Common.Get_Currency_By_Location(Eval("Location_Id").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString()).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Applied By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvAppliedBy" runat="server" Text='<%# Eval("AppliedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Update_Loan_Details" runat="server">
        <ContentTemplate>
            <div style="display: none;" id="Div_Details" runat="server">
                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <div class="col-md-3">
                            <h3 class="box-title">
                                <asp:Label ID="Lblemployeeid" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Code %>" Visible="false"></asp:Label>
                                <asp:Label ID="lblempIdColon" runat="server" Visible="false" Text=":"></asp:Label>
                                <asp:Label ID="SetEmployeeId" runat="server" Visible="false"></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3 class="box-title">
                                <asp:Label ID="Lblemployeename" runat="server" Font-Bold="true"
                                    Text="<%$ Resources:Attendance,Employee Name %>" Visible="false"></asp:Label>
                                <asp:Label ID="lblEmpNameColon" runat="server" Visible="false" Text=":"></asp:Label>
                                <asp:Label ID="setemployeename" runat="server" Visible="false"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="flow">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewLoanDetailrecord"
                                        runat="server"
                                        AllowPaging="false"
                                        AllowSorting="false"
                                        FooterStyle-HorizontalAlign="Center"
                                        FooterStyle-Font-Bold="true"
                                        FooterStyle-ForeColor="#555555"
                                        ShowFooter="true"
                                        AutoGenerateColumns="False" Width="100%" OnSorting="GridViewLoanDetailrecord_Sorting"
                                        OnPageIndexChanging="GridViewLoanDetailrecord_PageIndexChanging">
                                        <Columns>
                                            <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Loan_Id") %>'
                                                            ImageUrl="~/Images/edit.png" CausesValidation="False" 
                                                            ToolTip="<%$ Resources:Attendance,Edit %>" Width="16px" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Loan_Id") %>'
                                                            ImageUrl="~/Images/Erase.png"  ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpIdList" runat="server" Text='<%# GetType(Eval("Month").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Previous Balance %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenaltynameList" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Previous_Balance").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvTotal_Paid" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                </FooterTemplate>
                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Monthly Installment %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvaluetype1" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Montly_Installment").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvMonth_Installment" runat="server" Text="0.00" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Amount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvaluetype2" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Total_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Paid %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvaluetype3" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Employee_Paid").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div>
                                                        <div style="border-bottom: 1px solid #808080">
                                                            <asp:Label ID="lblgvPaid" runat="server" Text="0.00" />
                                                        </div>
                                                        <div>
                                                            <asp:Label ID="lblgvPending" runat="server" Text="0.00" />
                                                        </div>
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,status %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvaluetype4" runat="server" Text='<%# Eval("Is_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle Font-Bold="true" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />

                                    </asp:GridView>
                                    <asp:HiddenField ID="HdfSortDetail" runat="server" />
                                    <asp:HiddenField ID="hiddenid" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Loan">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Loan_Details">
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
    </script>
</asp:Content>
