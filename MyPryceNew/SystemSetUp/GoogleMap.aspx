<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoogleMap.aspx.cs" Inherits="GoogleMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no">
    <meta charset="utf-8">
 
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.3.1/dist/leaflet.css" integrity="sha512-Rksm5RenBEKSKFjgI3a41vrjkw4EVPlJ3+OiI65vTjIdo9brlAacEuKOiQ5OFh7cOI1bkDwLqdLw3Zg0cRJAAQ==" crossorigin=""/>
      <script src="https://unpkg.com/leaflet@1.3.1/dist/leaflet.js" integrity="sha512-/Nsx9X4HebavoBvEBuyp3I7od5tA0UzAxs+j83KgC8PU0kgB4XiK4Lfe4y4cgBtaRJQEIFCW+oC506aPT2L1zw==" crossorigin=""></script>


</head>
<body>
    <form runat="server">
    <center>
        <input id="txtLati" type="text" class="apply" runat="server" placeholder="Latitude" />
        <input id="txtLong" type="text" class="apply" runat="server" placeholder="Logitute" />
        <asp:Button ID="btnSet" runat="server" Text="Set this Logitute & Latitude" CssClass="button_example" OnClick="btnSet_Click"/>
    </center>

    </form>
    <div id="mapDiv" style="width: 800px; height: 500px">
    </div>

    
    <script>
        // position we will use later
        var lat = document.getElementById('txtLati').value;
        var lon = document.getElementById('txtLong').value;
        // initialize map
        map = L.map('mapDiv').setView([lat, lon], 13);
        // set map tiles source
        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
          attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
          maxZoom: 18,
        }).addTo(map);
        // add marker to the map
        marker = L.marker([lat, lon]).addTo(map);
        // add popup to the marker
        //marker.bindPopup("<b>ACME CO.</b><br />This st. 48<br />New York").openPopup();
      </script>
</body>
</html>
