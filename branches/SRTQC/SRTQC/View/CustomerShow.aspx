<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerShow.aspx.cs" Inherits="View_CustomerShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>交檔客戶查詢</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        function ReturnOpener(val) {
            var url = window.location.toString();
            var str = "";
            var txtCustomerID = "";
            var lblCustomerName = "";
            if (url.indexOf("?") != -1) {
                var ary = url.split("?")[1].split("&");
                for (var i in ary) {
                    str = ary[i].split("=")[0];
                    if (str == "customerID") {
                        txtCustomerID = decodeURI(ary[i].split("=")[1]);
                    }
                    if (str == "customerName") {
                        lblCustomerName = decodeURI(ary[i].split("=")[1]);
                    }
                }
            }
            var customerID = val.split("|")[0];
            var customerName = val.split("|")[1];

            opener.document.getElementById(txtCustomerID).value = customerID;
            opener.document.getElementById(lblCustomerName).value = customerName;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
        <tr>
            <td  align="center" width="100%" colspan="5">
                 <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
