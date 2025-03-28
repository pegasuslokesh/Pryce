<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Stock_Analysis.ascx.cs"
    Inherits="WebUserControl_Stock_Analysis" %>


<link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />

<table width="100%" border="1">
    <tr>
        <td >
           <b> Product Id</b>
        </td>
        <td >
         <b>   Product Name</b>
        </td>
        <td >
            <b>Model No.</b>
        </td>
    </tr>
    <tr>
    <td>
    <asp:Label ID="lblproductid" runat="server" CssClass="labelComman" ></asp:Label>
    
   

    </td>
    <td>
    <asp:Label ID="lblProductName" runat="server" CssClass="labelComman" ></asp:Label>
    
   

    </td>
    <td>
    <asp:Label ID="lblModel" runat="server" CssClass="labelComman" ></asp:Label>
    
   

    </td>

     </tr>


</table>




<table width="100%">
    <tr>
        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockInfo" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowPaging="True" AllowSorting="True" >
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>">
                        <ItemTemplate>
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Present  Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblPurQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  />
                    </asp:TemplateField>
                </Columns>
                
                
                
                
            </asp:GridView>
        </td>
    </tr>
</table>
