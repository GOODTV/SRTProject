<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmShow.aspx.cs" Inherits="View_ShortFilmShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>短片記錄查詢</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        function ReturnOpener(val) {
            var url = window.location.toString();
            var str = "";
            var txtFilmID = "";
            var lblShortFilmName = "";
            var txtFilename = "";
            if (url.indexOf("?") != -1) {
                var ary = url.split("?")[1].split("&");
                for (var i in ary) {
                    str = ary[i].split("=")[0];
                    if (str == "filmID") {
                        txtFilmID = decodeURI(ary[i].split("=")[1]);
                    }
                    if (str == "filmName") {
                        lblShortFilmName = decodeURI(ary[i].split("=")[1]);
                    }
                    if (str == "fileName") {
                        txtFilename = decodeURI(ary[i].split("=")[1]);
                    }
                }
            }
            var filmID = val.split("|")[0];
            var filmName = val.split("|")[1];

            opener.document.getElementById(txtFilmID).value = filmID;
            opener.document.getElementById(lblShortFilmName).value = filmName;
            opener.document.getElementById(txtFilename).value = filmID;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
        <tr>
            <th align="right" colspan="1">
                短片代號：
            </th>
            <td align="left" colspan="2" >
                <asp:TextBox runat="server" ID="txtFilmID" CssClass="font9" Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th align="right" colspan="1">
                短片名稱：
            </th>
            <td align="left" colspan="1" >
                <asp:TextBox runat="server" ID="txtFilmName" CssClass="font9" Width="200px"></asp:TextBox>
            </td>
            <td align="right" colspan="1" >
                <asp:Button ID="btnQuery" CssClass="npoButton npoButton_Search" runat="server"  Width="20mm"
                    Text="查詢" OnClick="btnQuery_Click"/>
            </td>
        </tr> 
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
