<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramEpisodeQuery.aspx.cs" Inherits="View_ProgramEpisodeQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>供片系統分集主檔查詢</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CheckFieldMustFillBasic() {
            var tbxProgramCode = document.getElementById('tbxProgramCode');
            var tbxEpisodeNo_Start = document.getElementById('tbxEpisodeNo_Start');
            var tbxEpisodeNo_End = document.getElementById('tbxEpisodeNo_End');

            if (tbxProgramCode.value == "") {
                alert('節目代號必填！');
                tbxProgramCode.focus();
                return false;
            }
            if (isNaN(Number(tbxEpisodeNo_Start.value)) == true) {
                alert('集數 必須為數字！');
                tbxEpisodeNo_Start.focus();
                return false;
            }
            if (isNaN(Number(tbxEpisodeNo_End.value)) == true) {
                alert('集數 必須為數字！');
                tbxEpisodeNo_End.focus();
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
        tr {
            background: -ms-linear-gradient(top, #feffff 0%,#ddf1f9 35%,#a0d8ef 100%);
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td align="left" colspan="1">
                節目代號：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxProgramCode" CssClass="font9" runat="server" Width="100px" />
                </td>
                <td align="left" colspan="1">
                集數：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxEpisodeNo_Start" CssClass="font9" runat="server" Width="50px" />～
                    <asp:TextBox ID="tbxEpisodeNo_End" CssClass="font9" runat="server" Width="50px" />　資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnQuery" runat="server"   Width="20mm" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click"/>
                    <asp:Button ID="btnAdd" runat="server"  Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                    <%-- <asp:Button ID="btnDelete" runat="server"  Width="20mm" Text="刪除" onclick="btnDelete_Click" OnClientClick="return confirm('您是否確定要刪除？');" />--%>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="5">
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
