<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesFollowupReport.aspx.cs" Inherits="Sales_Report_SalesFollowupReport" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sales Follow Up Print</title>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #4e4a4a">
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnEXport" />
            </Triggers>
              <ContentTemplate>

    <div>
        <center>
            <table width="100%" cellpadding="0" cellspacing="0" >
                <tr style="background-color: #90BDE9">
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <img src="../Images/compare.png" width="31" height="30" alt="D" />
                                </td>
                                <td>
                                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding-left: 5px" align="left">
                        <asp:Label ID="lblHeader" runat="server" Text="Sales Follow Up Report"
                            CssClass="LableHeaderTitle"></asp:Label>
                    </td>
                    <td align="right" style="padding-right: 10px">
                        <asp:Button ID="btnEXport" runat="server" Text="Export"  CssClass="buttonCommman"  OnClick="btnEXport_Click" />
                    </td>
                </tr>
                <tr style="background-color: #fff">
                   
                    <td colspan="3" valign="top">
                        <table width="100%" style="height:500px;">
                            <tr>
                                <td align="left" colspan="6" valign="top">




                                 <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFollowupReport" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                 OnRowCreated="GvSalesOrderDetail_RowCreated" >
                                                                
                                                                <Columns>
                                                                  
                                                                
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvsNo" Width="30px" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="30px" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvsNo"  runat="server" Text='<%#Eval("Sales_Person")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"   />
                                                                    </asp:TemplateField>


                                                                  <asp:TemplateField HeaderText="Total Inquiry" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesInquiry"  runat="server" Text='<%#Eval("TotalSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>
                                                                       <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingSalesInquiry" runat="server" Text='<%#Eval("PendingSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesInquiry"  runat="server" Text='<%#Eval("CloseSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                         <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesQuotation"  runat="server" Text='<%#Eval("TotalSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                    
                                                                         <asp:TemplateField HeaderText="Open(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="OpenSalesQuotation"  runat="server" Text='<%#Eval("OpenSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesQuotation"  runat="server" Text='<%#Eval("CloseSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Lost(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LostSalesQuotation" runat="server" Text='<%#Eval("LostSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>





                                                                      <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesOrder"  runat="server" Text='<%#Eval("TotalSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                      <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingSalesOrder"  runat="server" Text='<%#Eval("PendingSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>

                                                                      <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesOrder"  runat="server" Text='<%#Eval("CloseSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>


                                                                      <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesInvoice"  runat="server" Text='<%#Eval("TotalSalesInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingPaymentInvoice"  runat="server" Text='<%#Eval("PendingPaymentInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>


                                                                       <asp:TemplateField HeaderText="Received(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ReceivedPaymentInvoice" runat="server" Text='<%#Eval("ReceivedPaymentInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                      <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ReturnPaymentInvoice" runat="server" Text='<%#Eval("TotalReturnAmount")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                  
                                                                </Columns>
                                                                
                                                                
                                                                
                                                            </asp:GridView>

  <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvMonthlyFollowupReport" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                 OnRowCreated="GvSalesOrderDetailMonthly_RowCreated" >
                                                                
                                                                <Columns>
                                                                  
                                                                
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvsNo" Width="30px" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="30px" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvsNo"  runat="server" Text='<%#Eval("Sales_Person")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"   />
                                                                    </asp:TemplateField>

                                                                      <asp:TemplateField >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMonthName"  runat="server" Text='<%#Eval("MonthName")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"   />
                                                                    </asp:TemplateField>

                                                                  <asp:TemplateField HeaderText="Total Inquiry" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesInquiry"  runat="server" Text='<%#Eval("TotalSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>
                                                                       <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingSalesInquiry" runat="server" Text='<%#Eval("PendingSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesInquiry"  runat="server" Text='<%#Eval("CloseSalesInquiry")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                         <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesQuotation"  runat="server" Text='<%#Eval("TotalSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                    
                                                                         <asp:TemplateField HeaderText="Open(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="OpenSalesQuotation"  runat="server" Text='<%#Eval("OpenSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesQuotation"  runat="server" Text='<%#Eval("CloseSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Lost(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LostSalesQuotation" runat="server" Text='<%#Eval("LostSalesQuotation")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>





                                                                      <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesOrder"  runat="server" Text='<%#Eval("TotalSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                      <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingSalesOrder"  runat="server" Text='<%#Eval("PendingSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>

                                                                      <asp:TemplateField HeaderText="Close(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="CloseSalesOrder"  runat="server" Text='<%#Eval("CloseSalesOrder")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>


                                                                      <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TotalSalesInvoice"  runat="server" Text='<%#Eval("TotalSalesInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="Pending(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PendingPaymentInvoice"  runat="server" Text='<%#Eval("PendingPaymentInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"  />
                                                                    </asp:TemplateField>


                                                                       <asp:TemplateField HeaderText="Received(%)" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ReceivedPaymentInvoice" runat="server" Text='<%#Eval("ReceivedPaymentInvoice")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>


                                                                     <asp:TemplateField HeaderText="Total Amount" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ReturnPaymentInvoice" runat="server" Text='<%#Eval("TotalReturnAmount")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"   />
                                                                    </asp:TemplateField>

                                                                  
                                                                </Columns>
                                                                
                                                                
                                                                
                                                            </asp:GridView>

                                <%--    <dx:ReportToolbar ID="ReportToolbar1" runat='server' ShowDefaultButtons='False' Width="100%">
                                        <Items>
                                            <dx:ReportToolbarButton ItemKind='Search' />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind='PrintReport' />
                                            <dx:ReportToolbarButton ItemKind='PrintPage' />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' />
                                            <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' />
                                            <dx:ReportToolbarLabel ItemKind='PageLabel' />
                                            <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                                            </dx:ReportToolbarComboBox>
                                            <dx:ReportToolbarLabel ItemKind='OfLabel' />
                                            <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
                                            <dx:ReportToolbarButton ItemKind='NextPage' />
                                            <dx:ReportToolbarButton ItemKind='LastPage' />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind='SaveToDisk' />
                                            <dx:ReportToolbarButton ItemKind='SaveToWindow' />
                                            <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                                                <Elements>
                                                    <dx:ListElement Value='pdf' />
                                                    <dx:ListElement Value='xls' />
                                                    <dx:ListElement Value='xlsx' />
                                                    <dx:ListElement Value='rtf' />
                                                    <dx:ListElement Value='mht' />
                                                    <dx:ListElement Value='html' />
                                                    <dx:ListElement Value='txt' />
                                                    <dx:ListElement Value='csv' />
                                                    <dx:ListElement Value='png' />
                                                </Elements>
                                            </dx:ReportToolbarComboBox>
                                        </Items>
                                        <Styles>
                                            <LabelStyle>
                                                <Margins MarginLeft='3px' MarginRight='3px' />
                                            </LabelStyle>
                                        </Styles>
                                    </dx:ReportToolbar>
                                    <dx:ReportViewer ID="ReportViewer1" runat="server">

                                    </dx:ReportViewer>--%>

                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </center>
    </div>
     </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
            <ProgressTemplate>
                <div id="Background">
                </div>
                <div id="Progress">
                    <center>
                        <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" />
                    </center>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
