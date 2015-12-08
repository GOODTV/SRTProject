<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MusicSinglesCodeEdit.aspx.cs" Inherits="SysMgr_MusicSinglesCodeEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>音樂單曲對照檔設定</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="HFD_id" />
    <table class="table_v" width="100%">
        <tr>
            <th width="15%" align="right" style="white-space: nowrap;">
                類別名稱：
            </th>
            <td width="85%" colspan="3">
                <asp:DropDownList ID="ddlCodeType" runat="server" Width="39%"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="15%" align="right" style="white-space: nowrap;">
                項目名稱：
            </th>
            <td width="35%">
                <asp:TextBox runat="server" ID="txtCodeName" Width="95%"></asp:TextBox>
            </td>
            <th width="15%" align="right" style="white-space: nowrap;">
                排序：
            </th>
            <td width="35%">
                <asp:TextBox runat="server" ID="txtCodeNo" Width="95%"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="function" style="white-space: nowrap;">
        <asp:Button ID="btnUpdate" class="ui-button ui-corner-all" runat="server" Width="80px" Text="儲存" onclick="btnUpdate_Click" style="cursor: pointer;"/>
        <asp:Button ID="btnDelete" class="ui-button ui-corner-all" runat="server" Width="80px" Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"  style="cursor: pointer;"/>
        <asp:Button ID="btnExit" class="ui-button ui-corner-all" runat="server" Width="80px" Text="離開" onclick="btnExit_Click"  style="cursor: pointer;"/>
    </div>
    </form>
</body>
</html>
