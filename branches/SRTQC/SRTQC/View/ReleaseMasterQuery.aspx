<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseMasterQuery.aspx.cs" Inherits="View_ReleaseMasterQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>交檔客戶管理</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td align="left">
                    查詢：<asp:TextBox ID="tbxCustomerName" CssClass="font9" runat="server" Width="250px" />　資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
                <td colspan="3" align="right">
                    <asp:Button ID="btnQuery" runat="server"   Width="20mm" Text="查詢" OnClick="btnQuery_Click"/>
                    <asp:Button ID="btnAdd" runat="server"  Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="4">
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
