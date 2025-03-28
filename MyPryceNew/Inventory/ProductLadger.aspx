<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductLadger.aspx.cs" Inherits="Inventory_ProductLadger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-contract"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Ledger%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Product Ledger%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_List" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Radio"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelNo" ErrorMessage="<%$ Resources:Attendance,Enter Model No %>"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdnModelId" runat="server" Value="0" />
                                    <asp:TextBox ID="txtModelNo" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                        AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelNo"
                                        ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>


                                <div class="col-md-6">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                    <a style="color: Red">*</a>
                                    <asp:DropDownList ID="ddlModelProduct" runat="server" CssClass="form-control" Visible="false" OnSelectedIndexChanged="ddlModelProduct_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                        OnTextChanged="txtProductCode_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                        ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnTextChanged="txtProductName_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date  %>"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date  %>"></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtToDate_CalendarExtender" runat="server" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Reference Type %> "></asp:Label>
                                    <asp:DropDownList ID="Ddlreftype" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <br />
                                </div>
                                 <div class="col-md-12">
                                     <div class="col-md-2">
                                     </div>
                                     <div class="col-md-3">
                                         <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                             SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                             ForeColor="Gray"></asp:ListBox>
                                     </div>
                                     <div class="col-lg-2" style="text-align: center">
                                         <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

                                             <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

                                             <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />

                                             <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

                                             <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />
                                         </div>
                                     </div>
                                     <div class="col-md-3">
                                         <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                             SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                             ForeColor="Gray"></asp:ListBox>
                                     </div>
                                     <div class="col-md-2">
                                     </div>
                                     <br />
                                 </div>
                              
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngo" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Go %>"
                                        CssClass="btn btn-primary" OnClick="btngo_Click" />

                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    <br />
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
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Opening Balance%>"></asp:Label>
                                    <asp:TextBox ID="txtOpeningBalance" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label11" runat="server" Text="Closing Balance"></asp:Label>
                                    <asp:TextBox ID="txtClosingBalance" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
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
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Date %>" Value="ModifiedDate"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Reference Type %> " Value="Ref_Type"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,User Name %> " Value="ModifiedBy"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Alternate Id %> " Value="AlternateId"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <div class="col-lg-3">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" OnClick="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>" Visible="true"><span class="fa fa-print"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnExportExcel" runat="server" CausesValidation="False" OnClick="btnExportExcel_Click" ToolTip="Export Data To Excel"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning box-solid" <%= gvProductLadger.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow: auto; max-height: 500px;">
                                <asp:HiddenField ID="HDFSort" runat="server" />
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductLadger" runat="server" AutoGenerateColumns="False" Width="100%"
                                    ShowHeader="true" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateTime" runat="server" Text='<%# GetDate(Eval("ModifiedDate").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product ID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                <asp:HiddenField ID="HdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer/Supplier">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcontactName" runat="server" Text='<%# Eval("ContactName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                <asp:Label ID="lblgvCurrency" Visible="false" runat="server" Text='<%# Eval("CurrencyID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityIn" runat="server" Text='<%# SetDecimal(Eval("QuantityIn").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotQuantityIn" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Out Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityOut" runat="server" Text='<%# SetDecimal(Eval("QuantityOut").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotQuantityOut" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  Width="5%" />
                                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Balance %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# SetDecimal(Eval("BalQty").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotBalance" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref. ID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRefId" runat="server" Text='<%# string.IsNullOrEmpty(Eval("Ref_Id").ToString()) ? "0" : Eval("Ref_Id").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref. Type %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRef" runat="server" Text='<%# Eval("Ref_Type") %>' OnClick="lnkRef_Click"
                                                    CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("TransTypeId")+ "," + Eval("Location_Id")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
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
            <div class="box box-warning box-solid" <%= gvProductLadger.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                           <table id="tblSubDetail" style="text-align:center" width="100%" border="1" runat="server">
                              <tr>
                                  <td colspan="2">Opening Balance :</td>
                                  <td colspan="2" ><label id="tblOpeningBalance" runat="server" >0.00</label> </td>
                              </tr>
                               <tr>
                                   <td colspan="2">IN</td>
                                   <td colspan="2">Out</td>
                               </tr>
                               <tr>
                                   <td>Purchase</td>
                                   <td><label id="tblPurchaseAmount" runat="server">0.000</label> </td>
                                   <td>Sales</td>
                                   <td><label id="tblSalesAmount" runat="server">0.000</label> </td>
                               </tr>
                               <tr>
                                   <td>Adjustment In</td>
                                   <td><label id="tblAdjustmentIn" runat="server">0.000</label> </td>
                                   <td>Adjustment Out</td>
                                   <td>
                                       <label id="tblAdjustmentOut" runat="server">0.000</label></td>                                   
                               </tr>
                               <tr>
                                   <td>Physical In</td>
                                   <td><label id="tblPhysicalIn" runat="server">0.000</label></td>
                                   <td>Physical Out</td>
                                   <td>
                                       <label id="tblPhysicalOut" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td>Sales Return</td>
                                   <td><label id="tblReturn" runat="server">0.000</label></td>
                                   <td>Purchase Return</td>
                                   <td><label id="tblPurchaseReturn" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td>Transfer In</td>
                                   <td>
                                       <label id="tblTransferIn" runat="server">0.000</label></td>
                                   <td>Transfer Out</td>
                                   <td>
                                       <label id="tblTransferOut" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td>Production In</td>
                                   <td>
                                       <label id="tblProductionIn" runat="server">0.000</label></td>
                                   <td>Production Out</td>
                                   <td>
                                       <label id="tblProductionOut" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td></td>
                                   <td></td>
                                       
                                   <td>RMA Outword</td>
                                   <td>
                                       <label id="tblRmaOutword" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td> </td>
                                   <td></td>
                                   <td>Delivery</td>
                                   <td>
                                       <label id="tblDelivery" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td></td>
                                   <td><label id="tblINTotalAmount" runat="server">0.000</label></td>
                                   <td></td>
                                   <td><label id="tblOutTotalAmount" runat="server">0.000</label></td>
                               </tr>
                               <tr>
                                   <td colspan="2">Balance</td>
                                   <td colspan="2"><label id="tblRemaningBalance" runat="server">0.000</label></td>
                               </tr>
                           </table>
                        </div>
                    </div>
                </div>
            </div>
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
    </script>
</asp:Content>
