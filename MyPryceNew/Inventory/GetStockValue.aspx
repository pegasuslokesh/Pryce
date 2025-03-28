<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="GetStockValue.aspx.cs" Inherits="Inventory_GetStockValue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" Runat="Server">
    <h3>Get Stock Value</h3>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanelStock" runat="server" >
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="NetValue" runat="server" ></asp:Label>

                   <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvStockDetail" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        Width="100%">
                       <Columns>
                             <asp:TemplateField HeaderText="ProductId">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Quantity in">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantityIn" runat="server" Text='<%# Eval("QtyIn") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Quantity Out">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantityOut" runat="server" Text='<%# Eval("QtyOut") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Actual Stock">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualStock" runat="server" Text='<%# Eval("ActualStock") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Avg Cost">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAvarageStock" runat="server" Text='<%# Eval("AvgCost") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Stock Value">
                               <ItemTemplate>
                                   <asp:Label ID="lblStockValue" runat="server" Text='<%# Eval("StockValue") %>'></asp:Label>
                               </ItemTemplate>
                               <ItemStyle />
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Current Stock">
    <ItemTemplate>
        <asp:Label ID="lblCurrentStock" runat="server" Text='<%# Eval("CurrentStock") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle />
</asp:TemplateField>
                           <asp:TemplateField HeaderText="IsValid">
    <ItemTemplate>
        <asp:Label ID="lblIsValid" runat="server" Text='<%# Eval("IsValid") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle />
</asp:TemplateField>
                       </Columns>
                       </asp:GridView>

                </div>
            </div>
        </ContentTemplate>


    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" Runat="Server">
</asp:Content>

