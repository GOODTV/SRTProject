<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramEpisodeQueryOnly.aspx.cs" Inherits="View_ProgramEpisodeQueryOnly" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>分集主檔查詢</title>
    <link type="text/css"href="../include/calendar-win2k-cold-1.css" rel="stylesheet" />
    <link type="text/css" href="../include/main.css" rel="stylesheet" />
    <link type="text/css" href="../include/table.css" rel="stylesheet" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function(){

            initCalendar();

            $('#ClearChecked').click(function () {
                $("#checktable :input").each(function () {
                    $(this).attr("checked", false);
                });
            });

            $('.table_line2 tr:nth-child(4n+1)').addClass('table_background');
            $('.table_line2 tr:nth-child(4n+2)').addClass('table_background');

            $('.table_line2 tr').hover(
                function () {
                    var tr_prev = $(this).prev();
                    if (tr_prev.hasClass('table_background') == $(this).hasClass('table_background')) {
                        tr_prev.addClass('table_hover');
                    }
                    var tr_next = $(this).next();
                    if (tr_next.hasClass('table_background') == $(this).hasClass('table_background')) {
                        tr_next.addClass('table_hover');
                    }
                    $(this).addClass('table_hover');

                },
                function () {
                    var tr_prev = $(this).prev();
                    if (tr_prev.hasClass('table_background') == $(this).hasClass('table_background')) {
                        tr_prev.removeClass('table_hover');
                    }
                    var tr_next = $(this).next();
                    if (tr_next.hasClass('table_background') == $(this).hasClass('table_background')) {
                        tr_next.removeClass('table_hover');
                    }
                    $(this).removeClass('table_hover');
                }
            );

        });


        function initCalendar() {
            Calendar.setup({
                inputField: "txtPremiereDate1",   // id of the input field
                button: "imgPremiereDate1"     // 與觸發動作的物件ID相同
            });
            Calendar.setup({
                inputField: "txtPremiereDate2",   // id of the input field
                button: "imgPremiereDate2"     // 與觸發動作的物件ID相同
            });
        }

        function CheckFieldMustFillBasic() {
            //全文檢索
            //2015/9/17 可只輸入首播日期
            if ($('#txtPremiereDate1').val() == "" && $('#txtPremiereDate2').val() == "") {
                if ($('#tbFullTextSearch').val() == "") {
                    alert('全文檢索 欄位必填！');
                    $('#tbFullTextSearch').focus();
                    return false;
                }
            }
            //集數
            if (isNaN(Number($('#tbxEpisodeNo_Start').val())) == true) {
                alert('集數的起集 必須為數字！');
                $('#tbxEpisodeNo_Start').focus();
                return false;
            }
            if (isNaN(Number($('#tbxEpisodeNo_End').val())) == true) {
                alert('集數的迄集 必須為數字！');
                $('#tbxEpisodeNo_End').focus();
                return false;
            }
            if ($('#tbxEpisodeNo_Start').val() != '' && $('#tbxEpisodeNo_End').val() != '') {
                if (Number($('#tbxEpisodeNo_Start').val()) > Number($('#tbxEpisodeNo_End').val())) {
                    alert('集數的查詢範圍有誤！');
                    $('#tbxEpisodeNo_Start').focus();
                    return false;
                }
            }
            //總長度
            if (isNaN(Number($('#txtTotalLength1').val())) == true) {
                alert('總長度的起數 必須為數字！');
                $('#txtTotalLength1').focus();
                return false;
            }
            if (isNaN(Number($('#txtTotalLength2').val())) == true) {
                alert('總長度的迄數 必須為數字！');
                $('#txtTotalLength2').focus();
                return false;
            }
            if ($('#txtTotalLength1').val() != '' && $('#txtTotalLength2').val() != '') {
                if (Number($('#txtTotalLength1').val()) > Number($('#txtTotalLength2').val())) {
                    alert('總長度的查詢範圍有誤！');
                    $('#txtTotalLength1').focus();
                    return false;
                }
            }
            //首播日期
            var strDate1 = $('#txtPremiereDate1').val();
            if (strDate1 != '') {
                if (!ValidateDate(strDate1) || strDate1.length != 10) {
                    alert('首播日期的查詢起日錯誤！');
                    return false;
                }
            }
            var strDate2 = $('#txtPremiereDate2').val();
            if (strDate2 != '') {
                if (!ValidateDate(strDate2) || strDate2.length != 10) {
                    alert('首播日期的查詢迄日錯誤！');
                    return false;
                }
            }
            if (strDate1 != '' && strDate2 != '') {
                var date1 = new Date(parseInt(strDate1.substring(0, 4), 10), (parseInt(strDate1.substring(5, 7), 10)) - 1, parseInt(strDate1.substring(8, 10), 10), 12, 0, 0, 0);
                var date2 = new Date(parseInt(strDate2.substring(0, 4), 10), (parseInt(strDate2.substring(5, 7), 10)) - 1, parseInt(strDate2.substring(8, 10), 10), 12, 0, 0, 0);
                if (date1 > date2) {
                    alert('首播日期的查詢範圍有誤！');
                    $('#txtPremiereDate1').focus();
                    return false;
                }
            }
            //製評
            if (isNaN(Number($('#txtPackagerReplaySuggest1').val())) == true) {
                alert('製評的起數 必須為數字！');
                $('#txtPackagerReplaySuggest1').focus();
                return false;
            }
            if (isNaN(Number($('#txtPackagerReplaySuggest2').val())) == true) {
                alert('製評的迄數 必須為數字！');
                $('#txtPackagerReplaySuggest2').focus();
                return false;
            }
            if ($('#txtPackagerReplaySuggest1').val() != '' && $('#txtPackagerReplaySuggest2').val() != '') {
                if (Number($('#txtPackagerReplaySuggest1').val()) > Number($('#txtPackagerReplaySuggest2').val())) {
                    alert('製評的查詢範圍有誤！');
                    $('#txtPackagerReplaySuggest1').focus();
                    return false;
                }
            }
            //編評
            if (isNaN(Number($('#txtEditorReplaySuggest1').val())) == true) {
                alert('編評的起數 必須為數字！');
                $('#txtEditorReplaySuggest1').focus();
                return false;
            }
            if (isNaN(Number($('#txtEditorReplaySuggest2').val())) == true) {
                alert('編評的迄數 必須為數字！');
                $('#txtEditorReplaySuggest2').focus();
                return false;
            }
            if ($('#txtEditorReplaySuggest1').val() != '' && $('#txtEditorReplaySuggest2').val() != '') {
                if (Number($('#txtEditorReplaySuggest1').val()) > Number($('#txtEditorReplaySuggest2').val())) {
                    alert('編評的查詢範圍有誤！');
                    $('#txtEditorReplaySuggest1').focus();
                    return false;
                }
            }

            return true;
        }

        function ValidateDate(dtValue) {

            var yyyy = parseInt(dtValue.substring(0, 4), 10);
            var mm = parseInt(dtValue.substring(5, 7), 10);
            var dd = parseInt(dtValue.substring(8, 10), 10);
            var date = new Date(yyyy, mm - 1, dd, 12, 0, 0, 0);
            return mm === (date.getMonth() + 1) &&
                dd === date.getDate() &&
                yyyy === date.getFullYear();

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
        
        /* 水平資料表格 每筆二行 */
        .table_line2 {
	        border: #6CC solid 1px;
            border-collapse: collapse;
            border-spacing: 0px; /*cellspacing */
            margin: 2px 0px 2px 0px;
        }
        .table_line2 th {
	        border: #fff dotted 1px;
            padding: 3px 0px 3px 0px;
        }
        .table_line2 td {
	        border: #ccc dotted 1px;
            padding: 3px 0px 3px 0px;
	        /*line-height:18px;*/
        }

    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:HiddenField ID="hfExportExcelVisible" runat="server" />
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right">
                全文檢索：
                </th>
                <td align="left">
                    <asp:TextBox ID="tbFullTextSearch" CssClass="font9" runat="server" Width="200px" />
                </td>
                <th align="right">
                集數或原集數：
                </th>
                <td align="left">
                    <asp:TextBox ID="tbxEpisodeNo_Start" CssClass="font9" runat="server" Width="50px" MaxLength="6" /> ～
                    <asp:TextBox ID="tbxEpisodeNo_End" CssClass="font9" runat="server" Width="50px" MaxLength="6" />
                </td>
                <th align="right">
                    總長度：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtTotalLength1" CssClass="font9" Width="30px" MaxLength="3"></asp:TextBox> ～
                    <asp:TextBox runat="server" ID="txtTotalLength2" CssClass="font9" Width="30px" MaxLength="3"></asp:TextBox> 分鐘
                </td>
                <th align="right">
                    首播日期：
                </th>
                <td align="left" colspan="2" nowrap="nowrap">
                    <asp:TextBox ID="txtPremiereDate1" runat="server" onchange="CheckDateFormat(this, '首播日期的起日');" Width="80px"></asp:TextBox>
                    <img id="imgPremiereDate1" alt="" src="../images/date.gif" /> ～
                    <asp:TextBox ID="txtPremiereDate2" runat="server" onchange="CheckDateFormat(this, '首播日期的迄日');" Width="80px"></asp:TextBox>
                    <img id="imgPremiereDate2" alt="" src="../images/date.gif" /> (yyyy/mm/dd)
                </td>
            </tr>
            <tr>
                <th align="right">
                    註記：
                </th>
                <td align="left" colspan="3">
                    <table id="checktable">
                        <tr>
                            <td align="center" style="border-right: 1px solid #C0C0C0;"><span>版權註記</span></td>
                            <td align="center" style="border-right: 1px solid #C0C0C0;"><span>不播出註記</span></td>
                            <td align="center" style="border-right: 1px solid #C0C0C0;"><span>不重播註記</span></td>
                            <td align="center" style="border-right: 1px solid #C0C0C0;"><span>不供片註記</span></td>
                            <td align="center"><span>更新鎖定</span></td>
                        </tr>
                        <tr>
                            <td style="border-right: 1px solid #C0C0C0;">
                                <asp:RadioButtonList ID="cbxCopyrightNote" runat="server" TextAlign="Left" ToolTip="版權註記" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="border-right: 1px solid #C0C0C0;">
                                <asp:RadioButtonList ID="cbxNoBroadcastNote" runat="server" TextAlign="Left" ToolTip="不播出註記" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="border-right: 1px solid #C0C0C0;">
                                <asp:RadioButtonList ID="cbxNoReplayNote" runat="server" TextAlign="Left" ToolTip="不重播註記" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="border-right: 1px solid #C0C0C0;">
                                <asp:RadioButtonList ID="cbxNoProvidevideoNote" runat="server" TextAlign="Left" ToolTip="不供片註記" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="cbxIsLock" runat="server" TextAlign="Left" ToolTip="更新鎖定" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <input id="ClearChecked" type="button" value="清除所有註記" style="cursor: pointer; width: 100px;" /></td>
                        </tr>
                    </table>
                </td>
                <th align="right">
                    製評：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtPackagerReplaySuggest1" CssClass="font9" Width="20px" MaxLength="1"></asp:TextBox> ～
                    <asp:TextBox runat="server" ID="txtPackagerReplaySuggest2" CssClass="font9" Width="20px" MaxLength="1"></asp:TextBox> 顆星
                </td>
                <th align="right">
                    編評：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtEditorReplaySuggest1" CssClass="font9" Width="20px" MaxLength="1"></asp:TextBox> ～
                    <asp:TextBox runat="server" ID="txtEditorReplaySuggest2" CssClass="font9" Width="20px" MaxLength="1"></asp:TextBox> 顆星
                </td>
                <td align="right">
                    <asp:Button ID="btnQuery" runat="server"   Width="80px" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click" style="cursor: pointer;"/>
                    <asp:Button ID="btnExportExcel" runat="server"  Width="80px" Text="匯出" OnClick="btnExportExcel_Click" OnClientClick=" return confirm('您是否確定要將查詢結果匯出？');" style="cursor: pointer;"/>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <span style="color: blue;">全文檢索的查詢欄位：節目代號、節目名稱、原節目代號、分集大綱（原版）、分集大綱（500字版）、分集大綱（300字版）、分集大綱（120字版）、分集大綱（WEB版）、提醒記要、系列名稱、主持人/講員、大系列名稱、分集名稱、來賓等欄位。</span>
                </td>
            </tr>
            <tr>
                <td colspan="9">資料查詢結果筆數：<asp:Label ID="count" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="9">
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
