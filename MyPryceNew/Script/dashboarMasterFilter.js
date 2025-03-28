$(document).ready(function () {
    $('#FromDate').datepicker({
        dateFormat: 'dd/MMM/yyyy',
        yearRange: "-10:+0"
    });
    $('#ToDate').datepicker({
        dateFormat: 'dd/MMM/yyyy'
    });
});

function fnDateFilterOnChange() {
    try {
        debugger;
        var _result = fnValidateCustomFilter();
        if (_result == false) {
            return;
        }
        if ($('#lstFilterType').val() == gCustomFilerEnumVal) {
            $('#divCustomDateFilter').show();
        }
        else {
            $('#divCustomDateFilter').hide();
            fnOnChangeFilterType();
            //$('#frmDateFilter').submit();
            fnShowProgress();
        }

    }
    catch (err) {
        alert(err.message);
    }
}
function fnCustomDateOnChange() {
    try {
        var _result = fnValidateCustomFilter();
        if (_result == false) {
            return;
        }
        gFromDate = $('#FromDate').val();
        gToDate = $('#ToDate').val();
        fnOnChangeFilterType();
    }
    catch (err) {
        alert(err.message);
    }
}

function fnValidateCustomFilter() {
    try {
        debugger;
        if ($('#lstFilterType').val() != gCustomFilerEnumVal) {
            return true;
        }
        var fromDate = Date.parse($('#FromDate').val());
        var toDate = Date.parse($('#ToDate').val());
        if (isNaN(fromDate) == true) {
            throw "From Date is not valid";
        }
        if (isNaN(toDate) == true) {
            throw "To Date is not valid";
        }

        if (fromDate > toDate) {
            throw "From Date should be less then To Date";
        }
        return true;
    }
    catch (err) {
        alert(err);
        return false;
    }
}