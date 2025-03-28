<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Loan_Adjustment.ascx.cs" Inherits="WebUserControl_Loan_Adjustment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<asp:UpdatePanel ID="Update_Loan_Adjustment_control" runat="server">
    <ContentTemplate>--%>
<div class="row">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-body">
                <div class="form-group">
                    <div class="col-md-6">
                        <asp:HiddenField ID="HidEmpId_Loan" runat="server" />
                        <asp:Label ID="lblModuleName" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                        <a style="color: Red">*</a>
                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save_Loan"
                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Loan_Emp_Name" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                        <div class="input-group">
                            <asp:TextBox ID="Txt_Loan_Emp_Name" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                OnTextChanged="Txt_Loan_Emp_Name_TextChanged" CssClass="form-control" />
                            <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Loan_Emp_Name" UseContextKey="True"
                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <div class="input-group-btn">
                                <asp:Button ID="Btn_Get_Loan" runat="server" CssClass="btn btn-primary"
                                    Text="<%$ Resources:Attendance,Get %>" ValidationGroup="Save_Loan"
                                    OnClick="Btn_Get_Loan_Click" />
                            </div>
                        </div>
                        <br />
                    </div>

                    <div class="col-md-12">
                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Grid_View_Loan_Control" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                            OnPageIndexChanging="Grid_View_Loan_Control_PageIndexChanging" OnSorting="Grid_View_Loan_Control_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btn_Edit_Control" runat="server" CommandArgument='<%# Eval("Loan_Id") %>'
                                            ImageUrl="~/Images/Detail1.png" CausesValidation="False" OnCommand="Btn_Edit_Control_command"
                                            ToolTip="<%$ Resources:Attendance,View %>" Width="16px" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemStyle />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Request Date" SortExpression="Loan_Request_Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvLoanRequestDate" runat="server" Text='<%# GetDate(Eval("Loan_Request_Date")) %>'></asp:Label>
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
                                        <asp:Label ID="Lbl_Interest_Amount" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Intrest_Amt").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
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
                                        <asp:Label ID="Lbl_Currency" runat="server" Text='<%#Common.Get_Location_Currency_LControl(Session["CompId"].ToString(),Session["DBConnection"].ToString(),Session["LocId"].ToString()).ToString() %>'></asp:Label>
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
                        <asp:HiddenField ID="HDFSort_Loan" runat="server" />
                    </div>
                    <div class="col-md-12">
                        <div style="display: none; margin-top: 30px; margin-bottom: 30px;" id="Div_Details" runat="server">
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Grid_View_Loan_Detail_Control" runat="server" AllowPaging="false" AllowSorting="false"
                                    FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" FooterStyle-ForeColor="#555555"
                                    ShowFooter="true" AutoGenerateColumns="False" Width="100%" OnSorting="Grid_View_Loan_Detail_Control_Sorting" OnPageIndexChanging="Grid_View_Loan_Detail_Control_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_Gv_Select_LControl_All_LControl" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_LControl_All_LControl_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Gv_Select_LControl" onchange="javascript:Grv_Loan_Detail_Calculation_LControl(this)" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Month %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Month" runat="server" Text='<%# GetType(Eval("Month").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="Hdn_Month" Value='<%# Eval("Month") %>' runat="server" />
                                                <asp:HiddenField ID="Hdn_Loan_Detail_ID" Value='<%# Eval("Trans_Id")%>' runat="server" />
                                                <asp:HiddenField ID="Hdn_Loan_ID" Value='<%# Eval("Loan_Id") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None" Font-Bold="true" />
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_year" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
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
                                <asp:HiddenField ID="Hiddenid_Loan" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Pending Amount %>"></asp:Label>
                        <asp:TextBox ID="Txt_Pending_Amount_LControl" Text="0.00" ReadOnly="true" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Adjustment Amount%>"></asp:Label>
                        <asp:TextBox ID="Txt_Adjustment_Amount_LControl" Text="0.00" ReadOnly="true" runat="server" CssClass="form-control" />
                        <asp:HiddenField ID="Hdn_Adjustment_Amount_LControl" Value="0.00" runat="server" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,For Credit Account%>"></asp:Label>
                        <asp:TextBox ID="Txt_Account_Name_LControl" BackColor="#eeeeee" runat="server" AutoPostBack="true" OnTextChanged="Txt_Account_Name_LControl_TextChanged" CssClass="form-control" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Account_Name_LControl"
                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                        </cc1:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-6">
                        <br />
                        <asp:CheckBox ID="Chk_Emp_A_Salary_Ac_LControl" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Emp_A_Salary_Ac_LControl_CheckedChanged" Text="Adjustment against employee salary account" Checked="false" />
                    </div>
                    <div class="col-md-12" style="text-align: center;">
                        <br />
                        <asp:Button ID="Btn_Save_LControl" runat="server" CssClass="btn btn-primary"
                            Text="<%$ Resources:Attendance,Save%>" OnClick="Btn_Save_LControl_Click" />
                        <asp:Button ID="Btn_Reset_LControl" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                            Text="<%$ Resources:Attendance,Reset %>" OnClick="Btn_Reset_LControl_Click" />
                        <%--<asp:Button ID="Btn_Cancel_LControl" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                    Text="<%$ Resources:Attendance,Cancel %>" OnClick="Btn_Cancel_LControl_Click" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%-- </ContentTemplate>
</asp:UpdatePanel>--%>
<%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Loan_Adjustment_control">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
<script type="text/javascript">

    function resetPosition(object, args) {
        var tbposition = findPositionWithScrolling(100004);
        var xposition = tbposition[0] + 2;
        var yposition = tbposition[1] + 25;
        var ex = object._completionListElement;
        if (ex)
            $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
    }
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
</script>
<script type="text/javascript">
    function Grv_Loan_Detail_Calculation_LControl(id) {
        var sum = 0.00;
        var itemsum = 0.00;
        var isValid = false;
        document.getElementById('<%= Txt_Adjustment_Amount_LControl.ClientID %>').value = "0.00";
        var gridView = document.getElementById('<%= Grid_View_Loan_Detail_Control.ClientID %>');
        for (var i = 1; i < gridView.rows.length; i++) {
            var inputs = gridView.rows[i].getElementsByTagName('input');
            if (inputs[0].type == "checkbox") {
                if (inputs[0].checked) {
                    if (!isNaN(gridView.rows[i].cells[5].innerText)) {
                        itemsum = parseFloat(gridView.rows[i].cells[5].innerText);
                        sum = sum + itemsum;
                        document.getElementById('<%= Hdn_Adjustment_Amount_LControl.ClientID %>').value = sum;
                            document.getElementById('<%= Txt_Adjustment_Amount_LControl.ClientID %>').value = sum;
                        isValid = true;
                    }
                }
            }
        }
    }
</script>
