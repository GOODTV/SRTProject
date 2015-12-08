<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MusicSinglesCode.aspx.cs" Inherits="SysMgr_MusicSinglesCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>音樂單曲對照檔設定</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <link href="../include/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <!--[if lt IE 7]>
    <script src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE7.js"></script>
    <![endif]-->
    <script type="text/javascript">

        $(document).ready(function () {
        });

    </script>
    <style type="text/css">

        .table_h tr {
            color: black;
        }

        .table_h tr td:nth-child(1) {
            width: 40%;
        }

        .table_h tr td:nth-child(2) {
            width: 10%;
        }

        .table_h tr td:nth-child(3) {
            width: 50%;
        }

    </style>
</head>
<body>
    <form id="Form1" name="form" runat="server">
    <div id="container">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th width="10%" align="right" style="white-space: nowrap;">類別名稱：</th>
                <td width="30%" align="left">
                    <asp:TextBox runat="server" ID="txtCodeType" Width="95%"></asp:TextBox>
                 </td>
                 <th width="10%" align="right" style="white-space: nowrap;">項目名稱：</th>
                 <td width="30%" align="left">
                     <asp:TextBox runat="server" ID="txtCodeName" Width="95%"></asp:TextBox>
                 </td>
                 <td width="20%" align="right" style="white-space: nowrap;">
                     <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Width="80px" Text="查詢" OnClick="btnQuery_Click" style="cursor: pointer;"/>
                     <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Width="80px" Text="新增" OnClick="btnAdd_Click" style="cursor: pointer;"/>
                 </td>
            </tr>
            <tr>
                <td colspan="5"><span style="color: #0000FF;">排序欄位用意：每個類別內，對於項目名稱的排序，用在下拉式選單的排序，有勾選欄位的排序，及匯出Excel內Sheet的排序。</span></td>
            </tr>
            <tr>
                <td width="100%" colspan="5">
                    <br/>
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
