<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Magic_Stock_Analysis.aspx.cs" Inherits="Inventory_Magic_Stock_Analysis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="wrapper" style="background-color: #ecf0f5;">
                    <section class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-primary box-solid">
                                    <div class="box-header with-border">
                                        <h2 class="box-title">
                                            <img src="../Images/compare.png" width="31" height="30" alt="" />
                                            <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Stock Analysis %>"></asp:Label>
                                        </h2>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="row">
                                                <div style="text-align: center" class="col-md-12">
                                                    <table style="width:100%; text-align:center">
                                                        <tr style="height:auto; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                            <td style="width:20%; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="Lbl_product_id" runat="server" CssClass="form-control" Text="Product ID"></asp:Label>
                                                            </td>
                                                            <td style="border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="Label1" runat="server" CssClass="form-control" Text="Product Name"></asp:Label>
                                                            </td>
                                                            <td style="width:20%; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="Label2" runat="server" CssClass="form-control" Text="Model No."></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:auto; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                            <td style="width:20%; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="lblproductid" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width:20%; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                <asp:Label ID="lblModel" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                </div>                                                
                                                <div class="col-md-12">
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockInfo" runat="server" AutoGenerateColumns="true" Width="100%"
                                                                AllowPaging="True" AllowSorting="True" >
                                                                <Columns>
                                                                </Columns>
                                                                
                                                                
                                                                <PagerStyle CssClass="pagination-ys" />
                                                                
                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="alert alert-info ">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Customer/Supplier" Value="ContactName"></asp:ListItem>
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
                                                            <div class="col-lg-2">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListContact" ServicePath=""
                                                                        TargetControlID="txtValue" UseContextKey="True" CompletionListCssClass="completionList"
                                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                                <asp:DropDownList ID="Ddlreftype" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbind">
                                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                                                </asp:DropDownList>
                                                                    </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:ImageButton ID="btnbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
                                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                <asp:ImageButton ID="btnRefresh" runat="server" Style="width: 33px;" CausesValidation="False"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                <asp:HiddenField ID="HDFSort" runat="server" />
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
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductLadger" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                        ShowHeader="true" ShowFooter="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                                <ItemTemplate>
                                                                                    <%# Container.DataItemIndex+1 %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="3%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDateTime" runat="server" Text='<%# GetDate(Eval("ModifiedDate").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="8%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product ID %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                    <asp:HiddenField ID="HdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="12%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Customer/Supplier">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ContactName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="23%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit%>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                                                    <asp:Label ID="lblgvCurrency" Visible="false" runat="server" Text='<%# Eval("CurrencyID") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="8%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="In Qty.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblQuantityIn" runat="server" Text='<%# SetDecimal(Eval("QuantityIn").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="txttotQuantityIn" runat="server" Text="0" CssClass="labelComman" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemStyle  Width="5%" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Out Qty.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblQuantityOut" runat="server" Text='<%# SetDecimal(Eval("QuantityOut").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="txttotQuantityOut" runat="server" Text="0" CssClass="labelComman" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemStyle  Width="5%" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  Width="5%" />
                                                    </asp:TemplateField>--%>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Balance %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBalance" runat="server" Text='<%# SetDecimal(Eval("Field1").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="txttotBalance" runat="server" Text="0" CssClass="labelComman" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemStyle  Width="5%" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref. ID %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRefId" runat="server" Text='<%# string.IsNullOrEmpty(Eval("Ref_Id").ToString()) ? "0" : Eval("Ref_Id").ToString() %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref. Type %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblReftype" runat="server" Text='<%# Eval("Ref_Type") %>' CssClass="labelComman" Visible="false"></asp:Label>

                                                                                    <asp:LinkButton ID="lnkRef" runat="server" Text='<%# Eval("Ref_Type") %>' OnClick="lnkRef_Click"
                                                                                        CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("TransTypeId")+ "," + Eval("Location_Id")%>' />

                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="7%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="9%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="9%" />
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

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_New">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
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
</body>
</html>
