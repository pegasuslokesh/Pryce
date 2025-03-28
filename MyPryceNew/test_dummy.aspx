<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_dummy.aspx.cs" Inherits="test_dummy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script>
//        $(function () {

//            $(".MyTreeView").find(":checkbox").change(function () {
//                //check or uncheck childs
//                var nextele = $(this).closest("table").next()[0];
//                if (nextele && nextele.tagName == "DIV") {
//                    $(nextele).find(":checkbox").prop("checked", $(this).prop("checked"));

//                }
//                //check nodes all with the recursive method
//                CheckChildNodes($(".MyTreeView").find(":checkbox").first());

//            });
//            //method check filial nodes
//            function CheckChildNodes(Parentnode) {

//                var nextele = $(Parentnode).closest("table").next()[0];

//                if (nextele && nextele.tagName == "DIV") {
//                    $(nextele).find(":checkbox").each(function () {
//                        CheckChildNodes($(this));
//                    });

//                    if ($(nextele).find("input:checked").length == 0) {
//                        $(Parentnode).removeAttr("checked");
//                    }
//                    if ($(nextele).find("input:checked").length > 0) {
//                        $(Parentnode).prop("checked", "checked");
//                    }

//                }
//                else { return; }

//            }

        //        }) 

        function GetMonthName(monthNumber) {
            var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
            return months[monthNumber - 1];
        }
        $(document).ready(function () {
            $('#txt1').change(function () {
                var len = $('#txt1').val().length;
                if (len == 6 || len == 8)
                {
                    if (len == 6)
                    {
                        var d = $('#txt1').val().substr(0, 2);
                        var m = $('#txt1').val().substr(2, 2);
                        var y = $('#txt1').val().substr(4, 2);


                        console.warn(d + '-' + GetMonthName(m) + '-' + y);
                        console.warn(m);
                        console.warn(y);

                        $('#txt2').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                    }
                    if (len == 8) {
                        var d = $('#txt1').val().substr(0, 2);
                        var m = $('#txt1').val().substr(2, 2);
                        var y = $('#txt1').val().substr(6, 2);


                        console.warn(d + '-' + GetMonthName(m).substr(0,3) + '-' + y);
                        console.warn(m);
                        console.warn(y);

                        $('#txt2').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                    }
                   
                }
                else
                {
                    $('#txt2').val('');

                }
                
            });
        });
    </script>

   
      

   
    <script language="javascript" type="text/javascript">
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

</script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:TextBox ID="txt1" runat="server" ></asp:TextBox>
        <asp:TextBox ID="txt2" runat="server" ></asp:TextBox>
    <div>
        <asp:TreeView ID="TreeView1" runat="server"  ShowCheckBoxes="All" CssClass="MyTreeView" >
            <Nodes>
                <asp:TreeNode Text="B" Value="B" >
                    <asp:TreeNode Text="C" Value="C">
                         <asp:TreeNode Text="C1" Value="C1"></asp:TreeNode>
                         <asp:TreeNode Text="C2" Value="C3"></asp:TreeNode> 
                    </asp:TreeNode>
                    <asp:TreeNode Text="D" Value="D">
                         <asp:TreeNode Text="D1" Value="D1"></asp:TreeNode>
                         <asp:TreeNode Text="D2" Value="D2"></asp:TreeNode>
                    </asp:TreeNode>  
                </asp:TreeNode>
            </Nodes>
        </asp:TreeView>
    </div>
    <div>
    <asp:CheckBox ID="chkAdd" Text="Select all" onClick="new_validation();" runat="server"  
     />
<script type="text/javascript">
    function validation() {
        if (confirm("Are you sure you want to post back?")) {
            __doPostBack('<%= chkAdd.ClientID %>', '');
        } else {
            // Optional: change back the CheckBox to original position
            return false;
        }
    }

    function new_validation() {
    
        if ($('#chkAdd').is(':checked')) // if changed state is "CHECKED"
        {

            alert('Now it is going to unchecked');

            $('[id*=TreeView1] input[type=checkbox]').prop('checked', true);
            alert($('#chkAdd').is(':checked'));
            // do the magic here
        }
        else {
            // Optional: change back the CheckBox to original position
           
            $('[id*=TreeView1] input[type=checkbox]').prop('checked', false);
            alert('now it is going to checked');
            alert($('#chkAdd').is(':checked'));
        }


       
    }
</script>
    </div>
    </form>
</body>
</html>

