<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testjs.aspx.cs" Inherits="testjs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>  
   <script type="text/javascript">
       $(function () {
           $("[id*=chkDefaultMailHeader]").click(function () {
               if (this.checked) {
                   $("[id*=chkDefaultMailList]").attr('checked', this.checked);
                   //$("[id*=assignChkBx]").parent().parent().addClass('selected-row');  
               }
               else {
                   $("[id*=chkDefaultMailList]").removeAttr('checked');
                   // $("[id*=assignChkBx]").parent().parent().removeClass('selected-row');  
               }
           });
           $("[id*=chkDefaultMailList]").click(function () {
               //if (this.checked)  
               // $(this).parent().parent().addClass('selected-row');  
               // else  
               // $(this).parent().parent().removeClass('selected-row');  
               if (($("[id*=chkDefaultMailList]").length) == $("input[name='chkDefaultMailList']:checked").length)
                   $("[id*=chkDefaultMailHeader]").attr('checked', this.checked);
               else
                   $("[id*=chkDefaultMailHeader]").removeAttr('checked');
           });
       });  
   </script>


</head>
<body>
    <form id="form2" runat="server">
        <div>
            <h3>Please choose type of subscription letters:</h3>
            <asp:CheckBox ID="chkboxSelectAll" runat="server" Text="Select All" />
            <asp:CheckBoxList ID="chkboxList" runat="server">
                <asp:ListItem>News</asp:ListItem>
                <asp:ListItem>Offers</asp:ListItem>
                <asp:ListItem>Products</asp:ListItem>
                <asp:ListItem>Advertisement</asp:ListItem>
            </asp:CheckBoxList>
        </div>
    </form>
</body>
</html>

