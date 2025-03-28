<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerHistory.aspx.cs" Inherits="Purchase_CustomerHistory" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer / Supplier History</title>
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
</head>
<body>




    <form id="form1" runat="server">



        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div class="row">
            <div class="col-md-12" style="background-color: #90BDE9;">

                <div class="col-md-1">
                    <img src="../Images/sales_inquiry.png" alt="D" id="imgObject" runat="server" />
                </div>

                <%--                <div class="col-md-1">
                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                </div>--%>

                <div class="col-md-2" style="margin-top: 5px; padding-right: 20px">
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Inquiry%>"
                        CssClass="LableHeaderTitle"></asp:Label>
                    <br />
                </div>


                <div class="col-md-8" style="margin-top: 5px">
                    <asp:Label ID="lblContactname" runat="server" CssClass="LableHeaderTitle" Font-Bold="true"></asp:Label>
                </div>

            </div>


            <div class="col-md-12" style="background-color: #ccddee;">
                <div class="col-md-2">
                    <br />
                    <asp:DropDownList ID="ddlProductSerach" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProductSerach_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="ProductName"></asp:ListItem>
                        <%-- <asp:ListItem Text="<%$ Resources:Attendance,Sales Order No %>" Value="SalesOrderNo"></asp:ListItem>--%>
                    </asp:DropDownList>

                </div>
                <div class="col-md-6">
                    <br />
                    <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control" AutoPostBack="True"
                        OnTextChanged="txtProductCode_TextChanged" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                        ServicePath="" TargetControlID="txtProductId" UseContextKey="True" CompletionListCssClass="completionList"
                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                    </cc1:AutoCompleteExtender>
                    <asp:TextBox ID="txtSearchProductName" runat="server" BackColor="#eeeeee" CssClass="textComman"
                        AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" Width="100%" Visible="false" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                        ServicePath="" TargetControlID="txtSearchProductName" UseContextKey="True" CompletionListCssClass="completionList"
                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                    </cc1:AutoCompleteExtender>

                    <asp:HiddenField ID="hdnProductId" runat="server" />

                </div>

                <div class="col-md-4">
                    <br />
                    <div style="float: left">
                        <asp:Panel ID="pnlpSearch" runat="server" DefaultButton="imgbtnsearch">
                            <asp:ImageButton ID="imgbtnsearch" runat="server" CausesValidation="False" Height="25px"
                                ImageUrl="~/Images/search.png" OnClick="imgbtnsearch_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>" />
                        </asp:Panel>

                    </div>
                    <div>
                        <asp:Panel ID="Panel4" runat="server" DefaultButton="ImgbtnRefresh">
                            <asp:ImageButton ID="ImgbtnRefresh" runat="server" CausesValidation="False" Height="25px"
                                ImageUrl="~/Images/refresh.png" OnClick="ImgbtnRefresh_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-md-12">
                    <br />
                </div>
            </div>

            <div class="col-md-12">
                <br />
                <br />
            </div>
            <div class="col-md-12" style="overflow: auto;">
                <div class="col-md-12" style="overflow: auto">
                    <asp:GridView CssClass="table-striped table-bordered table table-hover table-responsive" ID="gvPurchaseOrder"
                        DataKeyNames="Trans_Id" runat="server" AutoGenerateColumns="true" Width="100%"
                        AllowPaging="false" AllowSorting="false" OnRowDataBound="gvPurchaseOrder_OnRowDataBound">
                        <Columns>

                            <asp:TemplateField HeaderText="Product Detail">
                                <ItemTemplate>
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover table-responsive" ID="gvDetail"
                                        runat="server" AutoGenerateColumns="true" Width="100%"
                                        AllowPaging="false" AllowSorting="false">
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>




                    </asp:GridView>
                </div>
            </div>

            <asp:Label ID="lblresult" runat="server" />
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                CancelControlID="btnCancel" BackgroundCssClass="modalBackground" Drag="true" PopupDragHandleControlID="pnlpopup">
            </cc1:ModalPopupExtender>


            <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="400px" Width="800px" Style="display: none">
                <table width="100%" class="table-striped table-bordered table table-hover table-responsive">
                    <tr style="background-color: #74B6D4">
                        <td style="height: 10%; color: White; font-weight: bold; font-size: larger; padding-left: 15PX;" align="left">
                            <asp:Label ID="lblVoucherNo" runat="server" Font-Bold="true"></asp:Label>

                        </td>
                        <td style="height: 10%; color: White; font-weight: bold; font-size: larger; padding-right: 15PX;" align="right">

                            <asp:LinkButton ID="btnCancel" runat="server" Text="X" ForeColor="Red" Font-Underline="false" ToolTip="Close"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <%--<tr>

<td colspan="2" align="center">

<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonCommman"  />
</td>
</tr>--%>
                <asp:Panel ID="PopupHeader" runat="server" BackColor="White" Height="400px" Width="800px" ScrollBars="Auto">

                    <table width="100%" style="border: Solid 3px #74B6D4; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" valign="top">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover table-responsive" ID="gvProductInfo"
                                    runat="server" AutoGenerateColumns="true" Width="100%"
                                    AllowPaging="false" AllowSorting="false">
                                    <Columns>
                                    </Columns>




                                </asp:GridView>
                            </td>
                        </tr>

                    </table>

                </asp:Panel>
            </asp:Panel>



        </div>

    </form>
</body>
</html>
