function getCustomerAddress(ctl) {
    try {

        var result = [2];
        if (ctl.value == "") {
            return ["false", ""];
        }
        var id = ctl.value.split('/')[3];
        var email = ctl.value.split('/')[2];
        var number = ctl.value.split('/')[1];
        var name = ctl.value.split('/')[0];
        if (!$.isNumeric(id)) {
            return ["false", "Customer is not valid"];
        }
        $.ajax({
            url: '../WebServices/customer.asmx/getCustomerAddress',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'strCustomerName':'" + name + "','strCustomerId':'" + id + "'}",
            async: false,
            success: function (data) {
                result[0] = "true";
                result[1] = data.d;

            },
            error: function (ex) {
                return ["false", "Customer is not valid"];
            }
        });
        return result;
    }
    catch (ex) {
        return ["false", ex.message];
    }
}

function customerHistory(ctrl) {
    // format: "customerName/customerId"
    var customerId = "0";
    if (ctrl.value != "" && ctrl.value != undefined) {
        customerId = ctrl.value.split('/')[3];
    }
    window.open('../Purchase/CustomerHistory.aspx?ContactId=' + customerId + '&&Page=SINQ', 'window', 'width=1024, ');
}

function getContactIDFromName(name) {
    var id = '';
    $.ajax({
        url: '../WebServices/customer.asmx/getContactIdFromName',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{'Name':'" + name + "'}",
        success: function (data) {
            id = data.d;
        },
        error: function (ex) {
            alert("Customer is not valid");
        }
    });
    return id;
}

function getContactIdFromNameNId(name, Custid) {
    var id = '';
    $.ajax({
        url: '../WebServices/customer.asmx/getContactIdFromNameNId',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{'Name':'" + name + "','id':'" + Custid + "'}",
        success: function (data) {
            id = data.d;
        },
        error: function (ex) {
            alert("Customer is not valid");
        }
    });
    return id;
}
function getContactIdFromNameNIdNSetSession(ctrl, sessionName) {
    var name = "";
    var Custid = "";

    name = ctrl.value.split('/')[0];
    number = ctrl.value.split('/')[1];
    email = ctrl.value.split('/')[2];
    Custid = ctrl.value.split('/')[3];
    debugger;
    if (isNaN(Custid)) {
        alert("Not a Valid Customer");
        $.ajax({
            url: '../WebServices/customer.asmx/setContactId',
            method: 'post',
        });
        ctrl.value = "";
        return;
    }

    var id = '';
    $.ajax({
        url: '../WebServices/customer.asmx/getContactIdFromNameNIdWithSetSession',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{'Name':'" + name + "','id':'" + Custid + "','sessionName':'" + sessionName + "'}",
        success: function (data) {
            id = data.d;
            if (id == "") {
                alert("Customer is not valid");
                ctrl.value = "";
                return;
            }
        },
        error: function (ex) {
            alert("Customer is not valid");
        }
    });
    return id;
}

function validateCustomer(ctl) {
    try {
        var id = ctl.value.split('/')[3];
        var email = ctl.value.split('/')[2];
        var number = ctl.value.split('/')[1];
        var name = ctl.value.split('/')[0];
        if (!$.isNumeric(id)) {
            throw "Customer is not valid";
        }
        $.ajax({
            url: '../webServices/customer.asmx/customerValidation',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'strCustomerName':'" + name + "','strCustomerId':'" + id + "','sessionName':'ContactID'}",
            success: function (data) {
                debugger;
                if (data.d == "false") {
                    alert("Customer is not valid");
                    ctl.value = "";
                    ctl.focus();
                }
            }
        });
    }
    catch (ex) {
        alert(ex);
    }
}

function resetCustomerIdForContact() {
    $.ajax({
        url: '../webServices/customer.asmx/resetCustomerIdSessionForContact',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
        }
    });
}
function setContactId(id) {
    $.ajax({
        url: '../webServices/customer.asmx/setContactId',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{'data':'" + id + "'}",
        success: function (data) {
        }
    });
}

function validateContactPerson(ctl) {
    try {
        debugger;
        var id = ctl.value.split('/')[3];
        var email = ctl.value.split('/')[2];
        var mobile = ctl.value.split('/')[1];
        var name = ctl.value.split('/')[0];
        if (!$.isNumeric(id)) {
            throw "Not a valid Contact Person";
        }

        $.ajax({
            url: '../webservices/customer.asmx/contactPersonValidation',
            method: 'post',
            async: false,
            contentType: "application/json; charset=utf-8",
            data: "{'name':'" + name + "','id':'" + id + "'}",
            success: function (data) {
                debugger;
                if (!data.d) {
                    alert("Select from suggestions only");
                    ctl.value = "";
                    ctl.focus();
                    return;
                }
            }
        });
    }
    catch (er) {
        alert(er);
        ctl.value = "";
        ctl.focus();
    }
}

function getCustomerAddressName(ctl) {
    try {
        debugger;
        var result = [2];
        if (ctl.value == "") {
            return ["false", ""];
        }
        var id = ctl.value.split('/')[3];
        var email = ctl.value.split('/')[2];
        var number = ctl.value.split('/')[1];
        var name = ctl.value.split('/')[0];
        if (!$.isNumeric(id)) {
            return ["false", "Customer is not valid"];
        }
        $.ajax({
            url: '../WebServices/customer.asmx/getCustomerAddressName',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'customerName':'" + name + "','customerId':'" + id + "'}",
            async: false,
            success: function (data) {
                result[0] = data.d[0];
                result[1] = data.d[1];
            },
            error: function (ex) {
                return ["false", "Customer is not valid"];
            }
        });
        return result;
    }
    catch (ex) {
        return ["false", ex.message];
    }
}


function getSupplierAddressName(ctl) {
    try {
        debugger;
        var result = [2];
        if (ctl.value == "") {
            return ["false", ""];
        }
        var id = ctl.value.split('/')[1];
        var name = ctl.value.split('/')[0];
        if (!$.isNumeric(id)) {
            return ["false", "Customer is not valid"];
        }
        $.ajax({
            url: '../WebServices/customer.asmx/getSupplierAddressName',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'Name':'" + name + "','Id':'" + id + "'}",
            async: false,
            success: function (data) {
                result[0] = data.d[0];
                result[1] = data.d[1];
            },
            error: function (ex) {
                return ["false", "Customer is not valid"];
            }
        });
        return result;
    }
    catch (ex) {
        return ["false", ex.message];
    }
}
