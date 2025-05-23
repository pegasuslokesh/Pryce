﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <link href="Bootstrap_Files/plugins/select2/select2.min.css" rel="stylesheet" />
    <link href="Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Bootstrap_Files/plugins/select2/select2.min.js"></script>


    <title></title>
    <style>
        select2-container {
  width: 90% !important;
}

.select2-container .select-all {
		position: absolute;
		top: 6px;
		right: 4px;
		width: 20px;
		height: 20px;
		margin: auto;
		display: block;
		background: url('data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTYuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNDc0LjggNDc0LjgwMSIgc3R5bGU9ImVuYWJsZS1iYWNrZ3JvdW5kOm5ldyAwIDAgNDc0LjggNDc0LjgwMTsiIHhtbDpzcGFjZT0icHJlc2VydmUiPgo8Zz4KCTxnPgoJCTxwYXRoIGQ9Ik0zOTYuMjgzLDI1Ny4wOTdjLTEuMTQtMC41NzUtMi4yODItMC44NjItMy40MzMtMC44NjJjLTIuNDc4LDAtNC42NjEsMC45NTEtNi41NjMsMi44NTdsLTE4LjI3NCwxOC4yNzEgICAgYy0xLjcwOCwxLjcxNS0yLjU2NiwzLjgwNi0yLjU2Niw2LjI4M3Y3Mi41MTNjMCwxMi41NjUtNC40NjMsMjMuMzE0LTEzLjQxNSwzMi4yNjRjLTguOTQ1LDguOTQ1LTE5LjcwMSwxMy40MTgtMzIuMjY0LDEzLjQxOCAgICBIODIuMjI2Yy0xMi41NjQsMC0yMy4zMTktNC40NzMtMzIuMjY0LTEzLjQxOGMtOC45NDctOC45NDktMTMuNDE4LTE5LjY5OC0xMy40MTgtMzIuMjY0VjExOC42MjIgICAgYzAtMTIuNTYyLDQuNDcxLTIzLjMxNiwxMy40MTgtMzIuMjY0YzguOTQ1LTguOTQ2LDE5LjctMTMuNDE4LDMyLjI2NC0xMy40MThIMzE5Ljc3YzQuMTg4LDAsOC40NywwLjU3MSwxMi44NDcsMS43MTQgICAgYzEuMTQzLDAuMzc4LDEuOTk5LDAuNTcxLDIuNTYzLDAuNTcxYzIuNDc4LDAsNC42NjgtMC45NDksNi41Ny0yLjg1MmwxMy45OS0xMy45OWMyLjI4Mi0yLjI4MSwzLjE0Mi01LjA0MywyLjU2Ni04LjI3NiAgICBjLTAuNTcxLTMuMDQ2LTIuMjg2LTUuMjM2LTUuMTQxLTYuNTY3Yy0xMC4yNzItNC43NTItMjEuNDEyLTcuMTM5LTMzLjQwMy03LjEzOUg4Mi4yMjZjLTIyLjY1LDAtNDIuMDE4LDguMDQyLTU4LjEwMiwyNC4xMjYgICAgQzguMDQyLDc2LjYxMywwLDk1Ljk3OCwwLDExOC42Mjl2MjM3LjU0M2MwLDIyLjY0Nyw4LjA0Miw0Mi4wMTQsMjQuMTI1LDU4LjA5OGMxNi4wODQsMTYuMDg4LDM1LjQ1MiwyNC4xMyw1OC4xMDIsMjQuMTNoMjM3LjU0MSAgICBjMjIuNjQ3LDAsNDIuMDE3LTguMDQyLDU4LjEwMS0yNC4xM2MxNi4wODUtMTYuMDg0LDI0LjEzNC0zNS40NSwyNC4xMzQtNTguMDk4di05MC43OTcgICAgQzQwMi4wMDEsMjYxLjM4MSw0MDAuMDg4LDI1OC42MjMsMzk2LjI4MywyNTcuMDk3eiIgZmlsbD0iIzAwMDAwMCIvPgoJCTxwYXRoIGQ9Ik00NjcuOTUsOTMuMjE2bC0zMS40MDktMzEuNDA5Yy00LjU2OC00LjU2Ny05Ljk5Ni02Ljg1MS0xNi4yNzktNi44NTFjLTYuMjc1LDAtMTEuNzA3LDIuMjg0LTE2LjI3MSw2Ljg1MSAgICBMMjE5LjI2NSwyNDYuNTMybC03NS4wODQtNzUuMDg5Yy00LjU2OS00LjU3LTkuOTk1LTYuODUxLTE2LjI3NC02Ljg1MWMtNi4yOCwwLTExLjcwNCwyLjI4MS0xNi4yNzQsNi44NTFsLTMxLjQwNSwzMS40MDUgICAgYy00LjU2OCw0LjU2OC02Ljg1NCw5Ljk5NC02Ljg1NCwxNi4yNzdjMCw2LjI4LDIuMjg2LDExLjcwNCw2Ljg1NCwxNi4yNzRsMTIyLjc2NywxMjIuNzY3YzQuNTY5LDQuNTcxLDkuOTk1LDYuODUxLDE2LjI3NCw2Ljg1MSAgICBjNi4yNzksMCwxMS43MDQtMi4yNzksMTYuMjc0LTYuODUxbDIzMi40MDQtMjMyLjQwM2M0LjU2NS00LjU2Nyw2Ljg1NC05Ljk5NCw2Ljg1NC0xNi4yNzRTNDcyLjUxOCw5Ny43ODMsNDY3Ljk1LDkzLjIxNnoiIGZpbGw9IiMwMDAwMDAiLz4KCTwvZz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K') no-repeat center;
		background-size: contain;
		cursor: pointer;
		z-index: 999999;
	}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="form-group">
                <label>Multiple</label>
                <select class="form-control select2" multiple="multiple" data-placeholder="Select a State"
                    style="width: 100%;">
                    <option>Alabama sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>Alaska sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>California sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>Delaware sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>Tennessee sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>Texas sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                    <option>Washington sadfksdfjklsdjfkl sadfjskldfjl sadfjklsdfj lsajfklsdjfkl lsfklsdj</option>
                </select>   
            </div>

            <div class="form-group">
                <label>Multiple</label>
                <asp:DropDownList ID="abc" ClientIDMode="Static" runat="server" multiple="multiple" CssClass="form-control select2" >
                    <asp:ListItem Text="ab" Value="1"></asp:ListItem>
                    <asp:ListItem Text="cd" Value="2"></asp:ListItem>
                    <asp:ListItem Text="ef" Value="3"></asp:ListItem>
                    <asp:ListItem Text="gh" Value="4"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="ab" runat="server" Text="submit" OnClick="ab_Click" OnClientClick="getselect2Value()" />
            <input type="button" onclick="setSelect2Value()" value="SET VALUES" />
            <asp:TextBox ID="txtscaleamount" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtdeninimation" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtscaleamountresult" runat="server" Visible="false"></asp:TextBox>

            <asp:Button ID="btnupdate" runat="server" Text="Set Password" OnClick="btnupdate_Click" />
            <asp:Image ID="img1" runat="server" />

            <asp:Button ID="btnDummy" runat="server" Text="button test" OnClick="btnDummy_Click" />
            <asp:Button ID="btnbytearaytoimage" runat="server" Text="Byte Array To Image" OnClick="btnbytearaytoimage_Click" />
            <asp:Image ID="Image1" runat="server" />

        </div>
    </form>
    <script>
        $(document).ready(function () {

            $('.select2').select2({
                placeholder: 'Press CTRL+A for selecr or unselect all options'
            });

            $('.select2[multiple]').siblings('.select2-container').append('<span class="select-all"></span>');
        });

        $(document).on('click', '.select-all', function (e) {
            selectAllSelect2($(this).siblings('.selection').find('.select2-search__field'));
        });

        $(document).on("keyup", ".select2-search__field", function (e) {
            var eventObj = window.event ? event : e;
            if (eventObj.keyCode === 65 && eventObj.ctrlKey)
                selectAllSelect2($(this));
        });


        function selectAllSelect2(that) {

            var selectAll = true;
            var existUnselected = false;
            var id = that.parents("span[class*='select2-container']").siblings('select[multiple]').attr('id');
            var item = $("#" + id);

            item.find("option").each(function (k, v) {
                if (!$(v).prop('selected')) {
                    existUnselected = true;
                    return false;
                }
            });

            selectAll = existUnselected ? selectAll : !selectAll;

            item.find("option").prop('selected', selectAll).trigger('change');
        }

            

        var selectedValues = new Array();
        selectedValues[0] = "1";
        selectedValues[1] = "2";
        selectedValues[1] = "3";

        function setSelect2Value()
        {
            debugger;
            $('#abc').val(selectedValues).change();
        }

        function getselect2Value()
        {
            alert($('#abc').val());
        }
    </script>
</body>
</html>
