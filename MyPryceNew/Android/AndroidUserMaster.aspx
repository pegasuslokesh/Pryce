<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" CodeFile="AndroidUserMaster.aspx.cs" Inherits="Android_AndroidUserMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cog"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Android User Setup %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Android User Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Android User Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Android User Setup%>"></asp:Label></li>
        <asp:HiddenField runat="server" ID="hdnCanEdit" />
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_List" runat="server">
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="gvEmployee" />
        </Triggers>--%>
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <div class="row">
			<div class="col-md-12">
				<div id="Div1" runat="server" class="box box-info collapsed-box">
					<div class="box-header with-border">
						<h3 class="box-title">
							<asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
						&nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:Label ID="lblTotalRecord" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

						<div class="box-tools pull-right">
							<button type="button" class="btn btn-box-tool" data-widget="collapse">
								<i id="I1" runat="server" class="fa fa-plus"></i>
							</button>
						</div>
					</div>
					<div class="box-body">
                         <div class="col-lg-3">
                            <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control">
                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>

                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2">
                            <asp:DropDownList ID="ddlOption1" runat="server" CssClass="form-control">
                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4">
                            <asp:Panel ID="Panel8" runat="server" DefaultButton="ImageButton8">
                                <asp:TextBox ID="txtValue1" runat="server" CssClass="form-control" placeholder="Search from Content" />
                            </asp:Panel>

                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="ImageButton8" runat="server" CausesValidation="False" 
                                OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="ImageButton9" runat="server" 
                                OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                    <asp:Label ID="lblDevice" runat="server" Text="<%$ Resources:Attendance,Select Device %>"></asp:Label>
                                    <asp:DropDownList ID="ddlDevice" runat="server" Style="margin-left: -868px;" AutoPostBack="true" OnSelectedIndexChanged="ddlDevice_SelectedIndexChanged"></asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <div style="overflow: auto; max-height: 500px;">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            OnPageIndexChanging="gvEmp_PageIndexChanging"  Width="100%" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' Visible="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' ToolTip="Edit"
                                                            OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                    SortExpression="Emp_Name"  ItemStyle-Width="40%" />
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                            </Columns>
                                                                                       
                                            <PagerStyle CssClass="pagination-ys" />
                                            
                                        </asp:GridView>
                                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>

     <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title" id="myModalLabel">
                                <asp:Label ID="Label12" runat="server" Font-Size="14px" Font-Bold="true"
                                    Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="update_Modal" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Font-Size="14px" Font-Bold="true"
                                                                Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                                            &nbsp; : &nbsp;
                                                            <asp:Label ID="lblEmpCode1" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label13" runat="server" Font-Size="14px" Font-Bold="true"
                                                                Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                            &nbsp; : &nbsp;
                                                            <asp:Label ID="lblEmpName1" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:Attendance,User Id %>"></asp:Label>
                                                            <asp:TextBox ID="txtUserName" Enabled="false" BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                            <asp:TextBox ID="txtPassword" MaxLength="10" runat="server"
                                                                CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Card No. %>"></asp:Label>
                                                            <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control" />
                                                            <br />
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

                            <asp:Button ID="btnSaveUser" runat="server" CssClass="btn btn-success" CausesValidation="false"
                                OnClick="btnSaveUser_Click1" Text="<%$ Resources:Attendance,Save %>" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                          

                        </div>
                    </div>
                </div>
            </div>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="display: none;">
        <asp:Panel ID="Panel" runat="server"></asp:Panel>
        <asp:Panel ID="Panel1" runat="server"></asp:Panel>
        <asp:Panel ID="Panel2" runat="server"></asp:Panel>
        <asp:Panel ID="Panel3" runat="server"></asp:Panel>
        <asp:Panel ID="Panel4" runat="server"></asp:Panel>
        <asp:Panel ID="Panel5" runat="server"></asp:Panel>
        <asp:Panel ID="Panel6" runat="server"></asp:Panel>
        <asp:Panel ID="Panel7" runat="server"></asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
    <script type="text/javascript">
        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>
</asp:Content>
