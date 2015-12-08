<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramEpisodeQueryOnlyDetail.aspx.cs" Inherits="View_ProgramEpisodeQueryOnlyDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>分集內容細節</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
    </script>
    <style type=text/css>

        .table_v tr th {
            height: 100px;
        }

        .table_v tr td {
            height: 100px;
        }

    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
    <asp:HiddenField runat="server" ID="HFD_Uid" />
    <asp:HiddenField runat="server" ID="HFD_PreviousUid" />
    <asp:HiddenField runat="server" ID="HFD_NextUid" />
    <asp:HiddenField ID="hfExportExcelVisible" runat="server" />
        <table width="90%" border="0" align="center" cellpadding="0" cellspacing="1"> 
            <tr>
                <td>
                    <asp:Label ID="lblGridList" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                <asp:Button ID="btnPrevious" runat="server" Text="上一筆" Width="70px" onclick="btnPrevious_Click" style="cursor: pointer;"/>
                <asp:Button ID="btnNext" runat="server" Text="下一筆" Width="70px" onclick="btnNext_Click" style="cursor: pointer;"/>
                <asp:Button ID="btnExportExcel" runat="server"  Width="20mm" Text="匯出" OnClick="btnExportExcel_Click" OnClientClick=" return confirm('您是否確定要將查詢結果匯出？');" style="cursor: pointer;"/>
                <%// <asp:Button ID="btnBack" runat="server"  Width="20mm" Text="返回" OnClick="btnBack_Click" OnClientClick=" return confirm('您是否確定要返回查詢結果？');"/>%>
                </td>
            </tr>
          </table>
        </div>
    </form>
</body>
</html>
