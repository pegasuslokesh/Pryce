<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Duty_Master_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
        
<%--<link href="../Bootstrap_Files/Multi/css/style.css" rel="stylesheet" />--%>
<%--<link href="../Bootstrap_Files/Multi/css/normalize.css" rel="stylesheet" />--%>
    <link href="../Bootstrap_Files/Additional/jquery.magicsearch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="row">
        <div class="col-md-12">
            <asp:TextBox ID="basic" runat="server" CssClass="form-control"></asp:TextBox>
            <br />
            <input id="basicsas" class="form-control" placeholder="search names...">
        </div>
    </div>
    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
    
    <%--<script src="../Bootstrap_Files/Multi/js/jquery.magicsearch.js"></script>--%>
    <script src="../Bootstrap_Files/Additional/jquery.magicsearch.min.js"></script>
    <script>
        $(function() {
            var dataSource = [
                {id: 1, firstName: 'Tim'},
                {id: 2, firstName: 'Eric'},
                {id: 3, firstName: 'Victor'},
                {id: 4, firstName: 'Lisa'},
                {id: 5, firstName: 'Oliver'},
                {id: 6, firstName: 'Zade'},
                {id: 7, firstName: 'David'},
                {id: 8, firstName: 'George'},
                {id: 9, firstName: 'Tony'},
                {id: 10, firstName: 'Bruce'},
            ];
            $('#basicsas').magicsearch({
                dataSource: dataSource,
                fields: ['firstName'],
                id: 'id',
                format: '%firstName%',
                multiple: true,
                multiField: 'firstName',
                multiStyle: {
                    space: 5,
                    width: 80
                }
            });
            $('[id$=basic]').magicsearch({
                dataSource: dataSource,
                fields: ['firstName'],
                id: 'id',
                format: '%firstName%',
                multiple: true,
                multiField: 'firstName',
                multiStyle: {
                    space: 5,
                    width: 80
                }
            });
        });
    </script>
</asp:Content>

