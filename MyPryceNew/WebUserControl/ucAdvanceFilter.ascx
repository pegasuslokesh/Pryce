<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucAdvanceFilter.ascx.cs" Inherits="WebUserControl_ucAdvanceFilter" %>
<script runat="server">

    protected void btnRemoveFilter_Click(object sender, EventArgs e)
    {

    }
</script>


<script type="text/javascript">
    function hideAdvfilterPopup() {
        $('#AdvanceFilterModal').modal('hide');
    }
</script>
<div class="row">
    <asp:UpdatePanel ID="uPnlAdvFilter" runat="server">
        <ContentTemplate>
            <div class="col-md-2" runat="server" id="divAndOr" visible="false">
                <asp:DropDownList ID="ddlAndOr" CssClass="form-control" runat="server">
                    <asp:ListItem Value="And"> And</asp:ListItem>
                    <asp:ListItem Value="Or"> Or</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFields" CssClass="form-control" runat="server" />
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlConditions" CssClass="form-control" runat="server">
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtValue" CssClass="form-control" runat="server" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnAdd" CssClass="btn btn-primary" runat="server" Text="+" OnClick="btnAdd_Click" />
                <br />
            </div>

            <div class="col-md-12">
               <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFltConditions" AllowPaging="false"
                                runat="server" AutoGenerateColumns="False" Width="100%" BorderColor="White"
                                AllowSorting="true" BorderWidth="0">
                   <Columns>
                       <asp:TemplateField>
                           <ItemTemplate>
                               <asp:Label ID="lblFltString" runat="server" Text='<%# Eval("fltString") %>' /> 
                               &nbsp;&nbsp;
                               <asp:LinkButton ID="btnDelCondition" runat="server" CssClass="fa fa-times" OnCommand="btnDelCondition_Command" CommandArgument='<%# Eval("sNo") %>' ></asp:LinkButton>
                               <asp:HiddenField ID="hdnSno" runat="server" Value='<%# Eval("sNo") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>
                   </Columns>
               </asp:GridView>
                <br />
                <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Flter" CssClass="btn btn-primary" OnClick="btnApply_Click" Enabled="false" />
                <asp:Button ID="btnRemoveFilter" runat="server" Text="Remove Filter" CssClass="btn btn-primary" OnClick="btnRemoveFilter_Click1" Enabled="false" />
                 <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />--%>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="uPnlAdvFilter">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</div>


