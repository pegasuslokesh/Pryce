<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Expenses_Tax.ascx.cs" Inherits="WebUserControl_Expenses_Tax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="box-body">
    <div class="form-group">
        <div class="col-md-6">            
            <asp:HiddenField ID="Hdn_Expenses_Id_Web_Control" runat="server" />
            <asp:HiddenField ID="Hdn_Expenses_Name_Web_Control" runat="server" />
            <asp:HiddenField ID="Hdn_Expenses_Amount_Web_Control" runat="server" />            
            <asp:HiddenField ID="Hdn_Page_Name_Web_Control" runat="server" />
            <asp:HiddenField ID="Hdn_Tax_Entry_Type" runat="server" />
            <asp:HiddenField ID="Hdn_Tax_Account_Id" runat="server" />

            <asp:HiddenField ID="Hdn_Saved_Expenses_Tax_Session" runat="server" />
            <asp:HiddenField ID="Hdn_Local_Expenses_Tax_Session" runat="server" />
            <asp:HiddenField ID="HiddenField2" runat="server" />

            <asp:Label ID="Lbl_Tax_Type_Web_Control" runat="server" Text="<%$ Resources:Attendance,Tax Type %>"></asp:Label>
            <a style="color: Red">*</a>
            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Add_Tax_WUC"
                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Tax_Type_Web_Control" ErrorMessage="<%$ Resources:Attendance,Enter Tax Type%>"></asp:RequiredFieldValidator>
            <asp:TextBox ID="Txt_Tax_Type_Web_Control" MaxLength="50" runat="server" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="Txt_Tax_Type_Web_Control_TextChanged" CssClass="form-control"></asp:TextBox>
            <cc1:AutoCompleteExtender ID="Txt_Tax_Type_AutoCompleteExtender" runat="server"
                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                ServiceMethod="Get_Complete_List_Of_Tax" ServicePath="" TargetControlID="Txt_Tax_Type_Web_Control"
                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
            </cc1:AutoCompleteExtender>
            <br />
        </div>
        <div class="col-md-6">
            <asp:Label ID="Lbl_Tax_Percentage_Web_Control" runat="server" Text="<%$ Resources:Attendance,Tax Percentage %>"></asp:Label>
            <a style="color: Red">*</a>
            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add_Tax_WUC"
                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Tax_Percenage_Web_Control" ErrorMessage="<%$ Resources:Attendance,Enter Tax Percentage %>"></asp:RequiredFieldValidator>
            <div class="input-group">
                <asp:TextBox ID="Txt_Tax_Percenage_Web_Control" MaxLength="6" onkeypress="return isNumber_For_Tax(event,this);" runat="server" CssClass="form-control"></asp:TextBox>
                <div class="input-group-addon">
                    <asp:Label ID="Label1" runat="server" Text="%"></asp:Label>
                </div>
            </div>
            <br />
        </div>
        <div class="col-md-12" style="text-align: center">
            <asp:Button ID="Btn_Add_to_Grid" runat="server" OnClick="Btn_Add_to_Grid_Click" ValidationGroup="Add_Tax_WUC" CssClass="btn btn-info" Text="<%$ Resources:Attendance,Add%>" />
            <br />
        </div>
        <div class="col-md-12">
            <br />
            <div class="flow">
                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Tax" runat="server" AutoGenerateColumns="False" Width="100%" >
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="Img_Tax_Delete_GV" runat="server" CausesValidation="False" CommandName='<%# Eval("Expenses_Id") %>' CommandArgument='<%# Eval("Tax_Type_Id") %>' ImageUrl="~/Images/Erase.png" OnCommand="Img_Tax_Delete_GV_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemStyle  />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses Name%>">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_Expenses_Name_GV" runat="server" CssClass="form-control" Text='<%# Eval("Expenses_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="30%"  />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax Type%>">
                            <ItemTemplate>
                                <asp:HiddenField ID="Hdn_Tax_Type_ID_GV" runat="server" Value='<%# Eval("Tax_Type_Id") %>' />                                
                                <asp:HiddenField ID="Hdn_Expenses_Amount_GV" runat="server" Value='<%# Eval("Expenses_Amount") %>' />
                                <asp:HiddenField ID="Hdn_Tax_Value_GV" runat="server" Value='<%# Eval("Tax_Value") %>' />
                                <asp:HiddenField ID="Hdn_Tax_Account_Id_GV" runat="server" Value='<%# Eval("Tax_Account_Id") %>' />
                                <asp:HiddenField ID="Hdn_Tax_Entry_Type_GV" runat="server" Value='<%# Eval("Tax_Entry_Type") %>' />
                                <asp:HiddenField ID="Hdn_Expenses_Id_GV" runat="server" Value='<%# Eval("Expenses_Id") %>' />                                
                                <asp:Label ID="Lbl_Tax_Type_GV" runat="server" CssClass="form-control" Text='<%# Eval("Tax_Type_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="30%"  />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax Percentage%>">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Tax_Percentage_GV" ReadOnly="true" BackColor="WhiteSmoke" CssClass="form-control" MaxLength="6" onkeyup="Check_Range_For_Tax(event,this);" onkeypress="return isNumber_For_Tax(event,this);" runat="server" Text='<%# Eval("Tax_Percentage") %>'></asp:TextBox>

                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add_Tax_WUC"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Tax_Percentage_GV" ErrorMessage="<%$ Resources:Attendance,Enter Tax Percentage %>"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                            <ItemStyle Width="30%"  />
                        </asp:TemplateField>
                    </Columns>
                    
                    
                    <PagerStyle CssClass="pagination-ys" />
                    
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

<div class="modal-footer">
    <asp:Button ID="Btn_Save_Web" runat="server" OnClick="Btn_Save_Web_Click" Visible="false" CssClass="btn btn-info" Text="<%$ Resources:Attendance,Save%>" />
    <button id="Btn_Close_Expenses_Tax" type="button" class="btn btn-default" data-dismiss="modal">Close</button>
</div>
<script type="text/javascript">
    function resetPosition(object, args) {
        //var tb = object._element;
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

    function resetPosition1(object, args) {


        var tbposition = findPositionWithScrolling(100004);
        var xposition = tbposition[0] + 2;
        var yposition = tbposition[1] + 25;
        var ex = object._completionListElement;

        if (ex)
            $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
    }
    function findPositionWithScrolling1(oElement) {
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
    function isNumber_For_Tax(evt, element) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (
            //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
            (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
            (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function Check_Range_For_Tax(evt, element) {
        var Cur_Val = element.value;
        if (Cur_Val > 100) {
            alert('Please enter Tax Percentage between 0 to 100');
        }
    }
</script>
