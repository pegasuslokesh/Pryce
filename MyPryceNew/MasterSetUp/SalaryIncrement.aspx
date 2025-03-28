<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalaryIncrement.aspx.cs" Inherits="MasterSetUp_SalaryIncrement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Salary Increment Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Salary Increment Setup%>"></asp:Label></li>
    </ol>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <asp:UpdatePanel ID="Update_Container" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">


                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecord" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Field3"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Category %>" Value="Category"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-5">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" class="form-control" runat="server" placeholder="Search from Content"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center;">
                                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkEmpInc" Font-Size="20px" Style="margin-top: -15px;" runat="server" Text="All" OnCheckedChanged="chkEmpInc_CheckedChanged" AutoPostBack="true" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box box-warning box-solid" <%= gvEmp.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto">

                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" AllowPaging="true" OnPageIndexChanging="gvEmp_OnPageIndexChanging" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a id="IbtnPrint" title="Print" onclick="printReport('<%# Eval("Emp_Id") %>')"><i class="fa fa-print"></i></a>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request %>">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnApprove" ToolTip="<%$ Resources:Attendance,Request %>" OnCommand="Approve_Command"
                                                                    CommandName='<%#Eval("Emp_Id") %>' Width="25px" Visible="false" Height="25px"
                                                                    ImageUrl="~/Images/Request_New.png" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Basic Salary %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBasicSal" runat="server" Text='<%# Eval("Basic_Salary") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Increment% %>">
                                                            <ItemTemplate>
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="ddlIncrPer" runat="server" Style="width: 33%;" CssClass="form-control"
                                                                        OnSelectedIndexChanged="ddlIncrPer_OnSelectedIndexChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <div class="input-group" style="width: 66%">
                                                                        <asp:TextBox ID="txtIncrementValue" OnTextChanged="txtIncrementValue_TextChanged" AutoPostBack="true" placeholder="Enter Percentage" runat="server" Style="width: 50%;" Visible="false" CssClass="form-control" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                            TargetControlID="txtIncrementValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>

                                                                        <asp:TextBox ID="Txt_Invrement_Amount" OnTextChanged="Txt_Invrement_Amount_TextChanged" AutoPostBack="true" placeholder="Enter Amount" runat="server" Style="width: 50%;" CssClass="form-control" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                            TargetControlID="Txt_Invrement_Amount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <%--<asp:HiddenField ID="HdnIncrPer" runat="server" Value='<%# Eval("Increment_Per") %>' />--%>
                                                                        <asp:HiddenField ID="HdnEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                        <asp:HiddenField ID="hdncategory" runat="server" Value='<%# Eval("Category") %>' />
                                                                    </div>
                                                                </div>
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
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Container">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script>
        function printReport(data) {
            window.open('../HR_Report/SalaryIncrementReport.aspx?Emp_Id=' + data + '', '_blank', 'width=1024');
        }
    </script>
</asp:Content>
