<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SystemParameter.aspx.cs" Inherits="SystemSetUp_SystemParameter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cog"></i>
        <asp:HiddenField runat="server" ID="hdnCanEdit" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,System Parameter Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,System Parameter Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button_Popup" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Button_Popup">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="Update_Full" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblParameterName" runat="server" Text="<%$ Resources:Attendance,Parameter Name %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="txtParameterName" ErrorMessage="<%$ Resources:Attendance,Enter Parameter Name%>" />

                                    <asp:TextBox ID="txtParameterName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblParameterValue" runat="server" Text="<%$ Resources:Attendance,Parameter Value %>"></asp:Label>
                                    <asp:TextBox ID="txtParameterValue" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCurrency" Visible="false" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlApprovalType" runat="server" CssClass="form-control" Visible="false">
                                        <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Brand" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Location" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Department" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DDL_Tax_System" runat="server" CssClass="form-control" Visible="false">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Company" Value="Company"></asp:ListItem>
                                        <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                                        <asp:ListItem Text="System" Value="System"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center; padding: 0px 15px 15px 15px;">
                                    <asp:HiddenField ID="Hdn_Edit_Value" runat="server" />
                                    <asp:HiddenField ID="Hdn_Product_Count" runat="server" />
                                    <asp:HiddenField ID="editid" runat="server" />
                                    <asp:Button ID="btnCSave" runat="server" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save" CssClass="btn btn-success" OnClick="btnCSave_Click" Visible="false" />
                                    <asp:Button ID="Btn_Tax_Save" runat="server" OnClick="Btn_Tax_Save_Click" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save" CssClass="btn btn-primary" Visible="false" OnClientClick="Confirm()" />
                                    <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
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
                                <asp:HiddenField ID="HDFSort" runat="server" />
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
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
                                    <asp:ListItem Text="<%$ Resources:Attendance,Parameter Name %>" Value="Param_Name" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Parameter Id %>" Value="TransId"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User" />
                                </asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-lg-5">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                    <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                                <br />
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            </div>


                        </div>
                    </div>
                </div>
            </div>

            <div class="box box-warning box-solid" <%= GvParameter.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="flow">

                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvParameter" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                    AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvParameter_PageIndexChanging"
                                    AllowSorting="True" OnSorting="GvParameter_Sorting">

                                    <Columns>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                            <ItemTemplate>
                                                <div class="dropdown" style="position: absolute;">
                                                    <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                        <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                    </button>
                                                    <ul class="dropdown-menu">

                                                        <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId") %>'
                                                                OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                        </li>

                                                    </ul>
                                                </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Id %>" SortExpression="TransId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParameterId" runat="server" Text='<%# Eval("TransId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Name %>" SortExpression="Param_Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParameterName" runat="server" Text='<%# Eval("Param_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Value %>" SortExpression="Param_Value">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParameterValue" runat="server" Text='<%# Eval("Param_Value") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerStyle CssClass="pagination-ys" />

                                </asp:GridView>

                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">Approval</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="box box-primary">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Grv_Approval" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="True">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approval Name %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="Hdn_Approval_ID" Value='<%# Eval("Approval_Id") %>' runat="server" />
                                                                    <asp:Label ID="Lbl_Approval_Name" runat="server" Text='<%# Eval("Approval_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approval Level %>">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="Ddl_Approval_Type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Approval_Type_SelectedIndexChanged" CssClass="form-control">
                                                                        <%--<asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Brand" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Location" Value="3"></asp:ListItem>
                                                                        <asp:ListItem Text="Department" Value="4"></asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="Hdn_Approval_Level" Value='<%# Eval("Approval_Level") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Btn_Approval_Save" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="Btn_Approval_Save_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Full">
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

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Confirm() {
            var ParameterName = document.getElementById('<%= txtParameterName.ClientID %>').value;
            if (ParameterName != "" && ParameterName != null) {
                var Edit_Value = document.getElementById('<%= Hdn_Edit_Value.ClientID %>').value;
                var Product_Count = document.getElementById('<%= Hdn_Product_Count.ClientID %>').value;
                var Tax_Value = document.getElementById('<%= DDL_Tax_System.ClientID %>').value;
                if (Tax_Value == "")
                    After_Value = "--Select--";
                else
                    After_Value = Tax_Value;

                if (Edit_Value == "")
                    Before_Value = "--Select--";
                else
                    Before_Value = Edit_Value;

                if (Before_Value != After_Value) {
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm_value";
                    if (confirm("If You want to change Tax System from " + Before_Value + " To " + After_Value + ", then already applied taxes for product (" + Product_Count + ") will be delete. \n Do you want to continue?")) {
                        confirm_value.value = "Yes";
                    } else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);
                }
            }

        }

        function Show_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>

</asp:Content>






