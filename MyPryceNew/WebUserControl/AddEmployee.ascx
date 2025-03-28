<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEmployee.ascx.cs" Inherits="WebUserControl_AddEmployee" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="tab-content">
    <div class="tab-pane active" id="AddNew">
        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="row">
                        <br />
                        <div class="col-md-12">
                            <div id="Div_Additional_Info" runat="server" class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Employee Master</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i id="Btn_Div_Additional_Info" runat="server" class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="form-group">
                                         <div class="col-md-4">
                                            <asp:Label ID="lblEmpCode" runat="server" Text="Employee Code"></asp:Label>
                                               <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpCode" ErrorMessage="<%$ Resources:Attendance,Enter Employee Code %>" />
                                            <asp:TextBox ID="txtEmpCode" runat="server"  CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblEmpName" runat="server" Text="Employee Name"></asp:Label>
                                             <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>" />
                                                           
                                            <asp:TextBox ID="txtEmpName" runat="server"  CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblEmpNameL" runat="server" Text="Employee Name L"></asp:Label>
                                            <asp:TextBox ID="txtEmpNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Gender %>"></asp:Label>
                                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,Male %>" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Female %>" Value="F"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:HiddenField ID="Hdn_Brand" runat="server" />
                                            <asp:Label ID="Label85" runat="server" Text="<%$ Resources:Attendance,Brand %>"></asp:Label><a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlBrand" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Brand %>" />
                                            <asp:DropDownList ID="ddlBrand" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged"
                                                runat="server" CssClass="form-control" />

                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:HiddenField ID="Hdn_Location" runat="server" />
                                            <asp:Label ID="Label86" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label><a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLocation" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Location%>" />
                                            <asp:DropDownList ID="ddlLocation" AutoPostBack="true" 
                                                runat="server" CssClass="form-control" />

                                            <br />
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Button runat="server" ValidationGroup="E_Save" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                            &nbsp;
                                             <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="btn btn-danger" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
