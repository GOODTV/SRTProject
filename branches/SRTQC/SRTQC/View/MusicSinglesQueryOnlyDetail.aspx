<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MusicSinglesQueryOnlyDetail.aspx.cs" Inherits="View_MusicSinglesQueryOnlyDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>音樂單曲資料明細</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <style type="text/css">

        .table_v TD {
            border-bottom: #ccc dotted 1px;
            line-height: 30px;
            padding: 2px;
        }

        .table_v {
            border-collapse: collapse;
            font-size: 14px;
        }

    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v"> 
            <tr>
                <th style="text-align: right; width: 10%;">
                    錄影日期：
                </th>
                <td style="width: 40%;">
                    <asp:Label ID="txtVideoDate" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right; width: 10%;">
                    節目：
                </th>
                <td style="width: 40%;">
                    <asp:Label ID="txtCategory" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    演出者：
                </th>
                <td>
                    <asp:Label ID="txtPerformer_Singer" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    類別：
                </th>
                <td>
                    <asp:Label ID="txtItem" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    曲目名稱：
                </th>
                <td>
                    <asp:Label ID="txtTracks" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    速度：
                </th>
                <td>
                    <asp:Label ID="txtSpeed" runat="server"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    長度：
                </th>
                <td>
                    <asp:Label ID="txtMusicLengthMin" runat="server"></asp:Label> 分
                    <asp:Label ID="txtMusicLengthSec" runat="server"></asp:Label> 秒
                </td>
                <th style="text-align: right;">
                    評比：
                </th>
                <td>
                    <asp:Label ID="txtRating" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    SN編號：
                </th>
                <td>
                    <asp:Label ID="txtSN1Number" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    曲風：
                </th>
                <td>
                    <asp:Label ID="txtMusicalStyles" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    Take：
                </th>
                <td>
                    <asp:Label ID="txtTake" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    樂器：
                </th>
                <td>
                    <asp:Label ID="txtMusicalInstruments" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    審核：
                </th>
                <td>
                    <asp:Label ID="txtAudit" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    語言：
                </th>
                <td>
                    <asp:Label ID="txtLanguage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    作詞：
                </th>
                <td>
                    <asp:Label ID="txtAuthors" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    節期：
                </th>
                <td>
                    <asp:Label ID="txtFestivals" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    作曲：
                </th>
                <td>
                    <asp:Label ID="txtCompose" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="white-space: nowrap; text-align: right;">
                    腳本/集數：
                </th>
                <td>
                    <asp:Label ID="txtScriptMark" runat="server">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr>
                <th style="white-space: nowrap; text-align: right;">
                    編曲/改編：
                </th>
                <td>
                    <asp:Label ID="txtArranger_Adaptation" runat="server"></asp:Label>
                </td>
                <th style="text-align: right;">
                </th>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    敘述：
                </th>
                <td>
                    <asp:Label ID="txtDescribed" runat="server"></asp:Label>&nbsp;
                </td>
                <th style="text-align: right;">
                    備註：
                </th>
                <td>
                    <asp:Label ID="txtRemark" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                <asp:Button ID="btnExit" runat="server" Text="離開" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
