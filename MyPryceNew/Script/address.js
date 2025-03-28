function validateAddressName(ctl) {
    try {
        if (ctl.value == "") {
            return;
        }

        var AddressName = "";
        
        try
        {
            AddressName = ctl.value.split('/')[0];
        }
        catch(error)
        {
            AddressName = ctl.value;
        }

        

        $.ajax({
            url: '../WebServices/address.asmx/validateAddressName',
            method: 'post',
            contentType: "application/json; charset=utf-8",
            data: "{'addressName':'" + AddressName + "'}",
            async: false,
            success: function (data) {
                if (data.d[0] == "false")
                {
                    alert(data.d[1]);
                    ctl.value = "";
                    ctl.focus();
                    return;
                }                
            },
            error: function (ex) {
                return ["false", "Address is not valid"];
            }
        });
      
    }
    catch (ex) {
        alert(ex);
    }
}