<%@ Page Title="" EnableViewState="true" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="true" AutoEventWireup="false" CodeFile="Purchase_Bill_Adjustment.aspx.cs" Inherits="Purchase_Purchase_Bill_Adjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/department_master.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Invoice Adjustment"></asp:Label>
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Invoice Adjustment"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Bin" runat="server">


        <Triggers>
            <asp:PostBackTrigger ControlID="btnList" />
        </Triggers>
        <ContentTemplate>

            


            <div class="box box-primary">

                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>

                            <a style="color: Red">*</a>
                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                ID="RequiredFieldValidator1" ValidationGroup="Update" Display="Dynamic"
                                SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Name %>" />


                            <div class="input-group">
                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                    AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />

                                <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                    ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                </cc1:AutoCompleteExtender>
                                <div class="input-group-btn">
                                    <asp:Button ID="btnGetRecord" runat="server" CssClass="btn btn-info" OnClick="btnGetRecord_OnClick"
                                        Text="Get Record" ValidationGroup="Update" />
                                </div>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-12">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Final Purchase Invoice</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-4">
                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                            ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                            ForeColor="Red"></asp:CustomValidator>

                                    </div>




                                    <div class="col-md-12">
                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInvoice" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                >
                                                <Columns>
                                                    <asp:TemplateField>

                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChangedDefault" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No %>" SortExpression="InvoiceNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbInvoiceNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>

                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("TransId") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="InvoiceDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("InvoiceDate").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Invoice No %>" SortExpression="OrderList">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderList" runat="server" Text='<%# Eval("SupInvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSupInvoiceDate" runat="server" Text='<%#Common.GetAmountDecimal(Eval("GrandTotal").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                            <asp:HiddenField ID="hdnTransId" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="col-md-12">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Running Purchase Invoice (Non Adjusted)</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-4">
                                        <asp:CustomValidator ID="CustomValidator3" ValidationGroup="Save" runat="server"
                                            ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate1"
                                            ForeColor="Red"></asp:CustomValidator>

                                    </div>

                                    <div class="col-md-12">

                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRunningInvoiceNonAdjusted" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                >
                                                <Columns>
                                                    <asp:TemplateField>

                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No %>" SortExpression="InvoiceNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbInvoiceNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("TransId") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="InvoiceDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("InvoiceDate").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Invoice No %>" SortExpression="OrderList">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderList" runat="server" Text='<%# Eval("SupInvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSupInvoiceDate" runat="server" Text='<%#Common.GetAmountDecimal(Eval("GrandTotal").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="col-md-12">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Running Purchase Invoice (Adjusted) </h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">





                                    <div class="col-md-12">

                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRunningInvoiceAdjusted" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cancel %>" >
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>'
                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command"
                                                                ToolTip="<%$ Resources:Attendance,Delete %>" />

                                                            <cc1:ConfirmButtonExtender ID="confirmdelete" runat="server" ConfirmText="Are you sure to cancel this record ?"
                                                                TargetControlID="IbtnDelete">
                                                            </cc1:ConfirmButtonExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No %>" SortExpression="InvoiceNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbInvoiceNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="InvoiceDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("InvoiceDate").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Invoice No %>" SortExpression="OrderList">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderList" runat="server" Text='<%# Eval("SupInvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSupInvoiceDate" runat="server" Text='<%#Common.GetAmountDecimal(Eval("GrandTotal").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Right" />
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


                    <div class="row">
                        <div class="col-md-12" style="text-align: center">
                            <br />


                             <asp:Button ID="btnList" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>" Visible="false"
                                class="btn btn-primary" OnClick="btnList_Click" />
                            <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>" Visible="false"
                                class="btn btn-primary" OnClick="Btn_Save_Click" />
                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to update record ?"
                                TargetControlID="Btn_Save">
                            </cc1:ConfirmButtonExtender>

                            <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                class="btn btn-primary" OnClick="Btn_Cancel_Click" />
                        </div>
                    </div>
                    <!-- /.box -->
                </div>


            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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

        function Grid_Validate(sender, args) {

            var gridView = document.getElementById("<%=GvPurchaseInvoice.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }

        function Grid_Validate1(sender, args) {

            var gridView = document.getElementById("<%=gvRunningInvoiceNonAdjusted.ClientID %>");
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



    <script type="text/javascript" language="javascript">





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
        }
    </script>


</asp:Content>

