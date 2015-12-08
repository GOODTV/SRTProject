<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MusicSinglesQuery.aspx.cs" Inherits="View_MusicSinglesQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>音樂單曲主檔查詢</title>
    <link type="text/css" href="../include/main.css" rel="stylesheet" />
    <link type="text/css" href="../include/table.css" rel="stylesheet" />
    <link href="../include/calendar-win2k-cold-1.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function(){


            initCalendar();

            $('#lblGridList .table_h tr td:nth-child(18)').each(function () {

                var intSpeed = $(this).text() == '' ? 0 : parseInt($(this).text());
                var SpeedText = "";
                for (var i = 1; i <= intSpeed; i++) {
                    SpeedText += '<IMG style="WIDTH: 12px;" src="../images/circle_tick.png">';
                }
                $(this).html(SpeedText);

            });

            $('#lblGridList .table_h tr td:nth-child(19)').each(function () {

                var intRating = $(this).text() == '' ? 0 : parseInt($(this).text());
                var RatingText = "";
                for (var i = 1; i <= intRating; i++) {
                    RatingText += '<IMG style="WIDTH: 13px;" src="../images/star_tick.png">';
                }
                $(this).html(RatingText);

            });

        });

        function initCalendar() {
            Calendar.setup({
                inputField: "txtVideoDateBegin",   // id of the input field
                button: "imgVideoDateBegin"     // 與觸發動作的物件ID相同
            });
            Calendar.setup({
                inputField: "txtVideoDateEnd",   // id of the input field
                button: "imgVideoDateEnd"     // 與觸發動作的物件ID相同
            });
        }

        function CheckFieldMustFillBasic() {

            //全文檢索
            var strFullTextSearch = $('#tbFullTextSearch').val();
            if (strFullTextSearch != "") {
                if (strFullTextSearch.indexOf("'") != -1) {
                    alert('全文檢索 不能輸入特殊符號！');
                    $('#tbFullTextSearch').focus();
                    return false;
                }
            }
            /*
            if ($('#tbFullTextSearch').val() == "") {
                alert('全文檢索 欄位必填！');
                $('#tbFullTextSearch').focus();
                return false;
            }
            */

            //檢查查詢錄影日期起日與迄日的範圍
            if ($('#txtVideoDateBegin').val() != '' && $('#txtVideoDateEnd').val() != '') {
                var dt1 = new Date($('#txtVideoDateBegin').val());
                var dt2 = new Date($('#txtVideoDateEnd').val());
                if ((dt1 - dt2) > 0) {
                    alert("查詢錄影日期的範圍錯誤！");
                    return false;
                }
            }

            //音樂長度
            if (isNaN(Number($('#tbxMusicLengthMin_Start').val())) == true) {
                alert('音樂長度的起分 必須為數字！');
                $('#tbxMusicLengthMin_Start').focus();
                return false;
            }
            if ($('#tbxMusicLengthMin_Start').val() != '' && Number($('#tbxMusicLengthMin_Start').val()) < 1) {
                alert('音樂長度的起分 必須在1分以上！');
                $('#tbxMusicLengthMin_Start').focus();
                return false;
            }
            if (isNaN(Number($('#tbxMusicLengthSec_Start').val())) == true) {
                alert('音樂長度的起秒 必須為數字！');
                $('#tbxMusicLengthSec_Start').focus();
                return false;
            }
            if ($('#tbxMusicLengthSec_Start').val() != '' && (Number($('#tbxMusicLengthSec_Start').val()) >= 60 || Number($('#tbxMusicLengthSec_Start').val()) < 1)) {
                alert('音樂長度的起秒 必須在1~59秒內！');
                $('#tbxMusicLengthSec_Start').focus();
                return false;
            }
            if (isNaN(Number($('#tbxMusicLengthMin_End').val())) == true) {
                alert('音樂長度的迄分 必須為數字！');
                $('#tbxMusicLengthMin_End').focus();
                return false;
            }
            if ($('#tbxMusicLengthMin_End').val() != '' && Number($('#tbxMusicLengthMin_End').val()) < 1) {
                alert('音樂長度的迄分 必須在1分以上！');
                $('#tbxMusicLengthMin_End').focus();
                return false;
            }

            if (isNaN(Number($('#tbxMusicLengthSec_End').val())) == true) {
                alert('音樂長度的迄秒 必須為數字！');
                $('#tbxMusicLengthSec_End').focus();
                return false;
            }
            if ($('#tbxMusicLengthSec_End').val() != '' && (Number($('#tbxMusicLengthSec_End').val()) >= 60 || Number($('#tbxMusicLengthSec_End').val()) < 1)) {
                alert('音樂長度的起秒 必須在1~59秒內！');
                $('#tbxMusicLengthSec_End').focus();
                return false;
            }

            if (($('#tbxMusicLengthMin_Start').val() != '' || $('#tbxMusicLengthSec_Start').val() != '')
                && ($('#tbxMusicLengthMin_End').val() != '' || $('#tbxMusicLengthSec_End').val() != '')) {
                if (Number($('#tbxMusicLengthMin_Start').val()) * 60 + Number($('#tbxMusicLengthSec_Start').val()) >
                    Number($('#tbxMusicLengthMin_End').val()) * 60 + Number($('#tbxMusicLengthSec_End').val())) {
                    alert('音樂長度的查詢範圍有誤！');
                    $('#tbxMusicLengthMin_Start').focus();
                    return false;
                }
            }

            //速度
            /*
            if (isNaN(Number($('#tbxSpeed_Start').val())) == true) {
                alert('速度的起數 必須為數字！');
                $('#tbxSpeed_Start').focus();
                return false;
            }
            if (isNaN(Number($('#tbxSpeed_End').val())) == true) {
                alert('速度的迄數 必須為數字！');
                $('#tbxSpeed_End').focus();
                return false;
            }
            */
            if ($('#tbxSpeed_Start').val() != '' && $('#tbxSpeed_End').val() != '') {
                if (Number($('#tbxSpeed_Start').val()) > Number($('#tbxSpeed_End').val())) {
                    alert('速度的查詢範圍有誤！');
                    $('#tbxSpeed_Start').focus();
                    return false;
                }
            }

            //評比
            /*
            if (isNaN(Number($('#tbxRating_Start').val())) == true) {
                alert('評比的起數 必須為數字！');
                $('#tbxRating_Start').focus();
                return false;
            }
            if (isNaN(Number($('#tbxRating_End').val())) == true) {
                alert('評比的迄數 必須為數字！');
                $('#tbxRating_End').focus();
                return false;
            }
            */
            if ($('#tbxRating_Start').val() != '' && $('#tbxRating_End').val() != '') {
                if (Number($('#tbxRating_Start').val()) > Number($('#tbxRating_End').val())) {
                    alert('評比的查詢範圍有誤！');
                    $('#tbxRating_Start').focus();
                    return false;
                }
            }

            //return true;
        }

        
        function CheckCategoryToExcel() {

            if (confirm('您是否確定要匯出？')) {
                if ($('#ddlCategory').val() != '') {
                    return true;
                }
                else {
                    alert('請先選擇要匯出的類別');
                    return false;
                }
            }
            else {
                return false;
            }

        }

    </script>
    <style type="text/css">

        .table_background {
            background: #E6F8FF;
        }
        .table_hover {
            background: #dedede;
            /*color: #333;*/
        }
        
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th style="text-align: right; width: 10%;">
                全文檢索：
                </th>
                <td align="left" colspan="3">
                    <asp:TextBox ID="tbFullTextSearch" runat="server" Width="550px"></asp:TextBox>
                </td>
                <th style="text-align: right; width: 10%; white-space: nowrap;">錄影日期：</th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtVideoDateBegin" onchange="CheckDateFormat(this, '錄影日期起日');" Width="90px"></asp:TextBox>
                    <img id="imgVideoDateBegin" alt="" src="../images/date.gif" /> ～
                    <asp:TextBox runat="server" ID="txtVideoDateEnd" onchange="CheckDateFormat(this, '錄影日期迄日');" Width="90px"></asp:TextBox>
                    <img id="imgVideoDateEnd" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <th style="text-align: right; width: 10%; white-space: nowrap;">
                    音樂長度：
                </th>
                <td style="width: 20%;">
                    <asp:TextBox runat="server" ID="tbxMusicLengthMin_Start" Width="50px"></asp:TextBox> 分
                    <asp:TextBox runat="server" ID="tbxMusicLengthSec_Start" Width="30px" MaxLength="2"></asp:TextBox> 秒 ～
                    <asp:TextBox runat="server" ID="tbxMusicLengthMin_End" Width="50px"></asp:TextBox> 分
                    <asp:TextBox runat="server" ID="tbxMusicLengthSec_End" Width="30px" MaxLength="2"></asp:TextBox> 秒
                </td>
                <th style="text-align: right; width: 10%;">
                    速度：
                </th>
                <td style="width: 10%;">
                    <asp:DropDownList ID="tbxSpeed_Start" runat="server" Width="50px">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1"></asp:ListItem>
                        <asp:ListItem Value="2"></asp:ListItem>
                        <asp:ListItem Value="3"></asp:ListItem>
                        <asp:ListItem Value="4"></asp:ListItem>
                        <asp:ListItem Value="5"></asp:ListItem>
                    </asp:DropDownList> ～
                    <asp:DropDownList ID="tbxSpeed_End" runat="server" Width="50px">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1"></asp:ListItem>
                        <asp:ListItem Value="2"></asp:ListItem>
                        <asp:ListItem Value="3"></asp:ListItem>
                        <asp:ListItem Value="4"></asp:ListItem>
                        <asp:ListItem Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th style="text-align: right; width: 10%;">
                    評比：
                </th>
                <td style="width: 15%; white-space: nowrap;">
                    <asp:DropDownList ID="tbxRating_Start" runat="server" Width="50px">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1"></asp:ListItem>
                        <asp:ListItem Value="2"></asp:ListItem>
                        <asp:ListItem Value="3"></asp:ListItem>
                        <asp:ListItem Value="4"></asp:ListItem>
                        <asp:ListItem Value="5"></asp:ListItem>
                    </asp:DropDownList> ～
                    <asp:DropDownList ID="tbxRating_End" runat="server" Width="50px">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1"></asp:ListItem>
                        <asp:ListItem Value="2"></asp:ListItem>
                        <asp:ListItem Value="3"></asp:ListItem>
                        <asp:ListItem Value="4"></asp:ListItem>
                        <asp:ListItem Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="text-align: right; width: 25%; white-space: nowrap;">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click" style="cursor: pointer;"/>
                    <asp:Button ID="btnAdd" runat="server" Width="80px" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <span style="color: blue;">全文檢索的查詢欄位：節目, 類別, 演出者, 曲目名稱, 敘述, SN編號, Take, 作詞, 作曲, 編曲/改編, 審核, 曲風, 樂器, 語言, 腳本/集數, 節期, 備註等欄位。</span>
                </td>
            </tr>
            <tr>
                <td colspan="7">資料查詢結果筆數：<asp:Label ID="count" runat="server"></asp:Label></td>
                <td style="text-align: right;">
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="7">
                     <asp:Label ID="lblGridList" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="7" style="text-align: right; width: 25%; white-space: nowrap;">
                    <span style="font-weight: bold; color: #006;">匯出類別：</span>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnExportExcel" runat="server" Width="100px" Text="匯出全部" OnClick="btnExportExcel_Click" OnClientClick="return CheckCategoryToExcel();" style="cursor: pointer;"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnExportQueryExcel" runat="server" Width="130px" Text="匯出搜尋結果" OnClick="btnExportQueryExcel_Click" OnClientClick="return confirm('您是否確定要匯出查詢結果？');" style="cursor: pointer;"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
