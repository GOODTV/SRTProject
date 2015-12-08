<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MusicSinglesEdit.aspx.cs" Inherits="View_MusicSinglesEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>音樂單曲主檔修改</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <link href="../include/calendar-win2k-cold-1.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            initCalendar();

            initSpeed();
            initRating();

            //排除按Enter會誤動作
            $("body").bind("keypress", function (e) {

                if (e.which == 13) {
                    if (e.target.id == "txtDescribed") {
                    }
                    else if (e.target.id == "txtRemark") {
                        //$("#tbxRequester").blur();
                    }
                    else {
                        return false;
                    }
                }

            });

            //速度
            $('img[id^=Speed]').click(function () {

                var img_id = this.id;
                //var img_src = this.src;
                //img_src = img_src.match(/_tick/) ? img_src.replace(/_tick/, "_untick") : img_src.replace(/_untick/, "_tick");
                var img_no = parseInt(img_id.replace('Speed', ''));
                if (parseInt($('#txtSpeed').val()) == img_no) {
                    $('#txtSpeed').val(0);
                    $('img[id^=Speed]').each(function () {

                        $(this).attr('src', $(this).attr('src').replace('_tick', '_untick'));

                    });
                }
                else {
                    $('#txtSpeed').val(img_no);
                    $('img[id^=Speed]').each(function () {

                        if (parseInt(this.id.replace('Speed', '')) <= img_no) {
                            $(this).attr('src', $(this).attr('src').replace('_untick', '_tick'));
                        }
                        else {
                            $(this).attr('src', $(this).attr('src').replace('_tick', '_untick'));
                        }

                    });
                }

                /*
                //❶❷❸❹❺①②③④⑤
                var speed_no_old = $('#txtSpeed').val();
                var speed_no = parseInt(this.id.replace('Speed', ''));

                $('#Speed1').text('①');
                $('#Speed2').text('②');
                $('#Speed3').text('③');
                $('#Speed4').text('④');
                $('#Speed5').text('⑤');

                if (speed_no_old != speed_no) {

                    switch (speed_no) {
                        case 1:
                            $('#Speed1').text('❶');
                            break;
                        case 2:
                            $('#Speed2').text('❷');
                            break;
                        case 3:
                            $('#Speed3').text('❸');
                            break;
                        case 4:
                            $('#Speed4').text('❹');
                            break;
                        case 5:
                            $('#Speed5').text('❺');
                            break;

                    }
                    $('#txtSpeed').val(speed_no);
                }
                else {
                    $('#txtSpeed').val('');
                }
                */

            });

            //評比
            $('img[id^=Rating]').click(function () {

                var img_id = this.id;
                //var img_src = this.src;
                //img_src = img_src.match(/_tick/) ? img_src.replace(/_tick/, "_untick") : img_src.replace(/_untick/, "_tick");
                var img_no = parseInt(img_id.replace('Rating', ''));
                if (parseInt($('#txtRating').val()) == img_no) {
                    $('#txtRating').val(0);
                    $('img[id^=Rating]').each(function () {

                        $(this).attr('src', $(this).attr('src').replace('_tick', '_untick'));

                    });
                }
                else {
                    $('#txtRating').val(img_no);
                    $('img[id^=Rating]').each(function () {

                        if (parseInt(this.id.replace('Rating', '')) <= img_no) {
                            $(this).attr('src', $(this).attr('src').replace('_untick', '_tick'));
                        }
                        else {
                            $(this).attr('src', $(this).attr('src').replace('_tick', '_untick'));
                        }

                    });
                }

            });

        });

        function initCalendar() {
            Calendar.setup({
                inputField: "txtVideoDate",   // id of the input field
                button: "imgVideoDate"     // 與觸發動作的物件ID相同
            });
        }

        function initSpeed() {

            var speed_no = parseInt($('#txtSpeed').val());

            $('img[id^=Speed]').each(function () {

                if (parseInt(this.id.replace('Speed', '')) <= speed_no) {
                    $(this).attr('src', $(this).attr('src').replace('_untick', '_tick'));
                }

            });

            /*
            switch (speed_no) {
                case 1:
                    $('#Speed1').text('❶');
                    break;
                case 2:
                    $('#Speed2').text('❷');
                    break;
                case 3:
                    $('#Speed3').text('❸');
                    break;
                case 4:
                    $('#Speed4').text('❹');
                    break;
                case 5:
                    $('#Speed5').text('❺');
                    break;
            }
            */
        }

        function initRating() {

            var rating_no = parseInt($('#txtRating').val());

            $('img[id^=Rating]').each(function () {

                if (parseInt(this.id.replace('Rating', '')) <= rating_no) {
                    $(this).attr('src', $(this).attr('src').replace('_untick', '_tick'));
                }

            });

        }

        function CheckFieldMustFillBasic() {

            /*
            var strRet = "";
            var txtProgramCode = document.getElementById('txtProgramCode');
            var txtEpisode = document.getElementById('txtEpisode');
            var txtOriginal_ProgramCode = document.getElementById('txtOriginal_ProgramCode');
            var txtOriginal_Episode = document.getElementById('txtOriginal_Episode');

            var txtEpisodeSynopsisOriginal = document.getElementById('txtEpisodeSynopsisOriginal');
            var txtEpisodeSynopsis500 = document.getElementById('txtEpisodeSynopsis500');
            var txtEpisodeSynopsis300 = document.getElementById('txtEpisodeSynopsis300');
            var txtEpisodeSynopsis120 = document.getElementById('txtEpisodeSynopsis120');
            var txtEpisodeSynopsisWEB = document.getElementById('txtEpisodeSynopsisWEB');

            if (txtProgramCode.value == "") {
                strRet += "節目代號 ";
            }
            if (txtEpisode.value == "") {
                strRet += "集數 ";
            }
            if (strRet != "") {
                strRet += "欄位不可為空白！";
                alert(strRet);
                return false;
            }
            if (isNaN(Number(txtEpisode.value)) == true) {
                alert('集數 欄位必須為數字！');
                txtEpisode.focus();
                return false;
            }
            if (isNaN(Number(txtOriginal_Episode.value)) == true) {
                alert('原集數 欄位必須為數字！');
                txtOriginal_Episode.focus();
                return false;
            }
            cnt = 0;
            sName = txtProgramCode.value;
            for (var i = 0; i < sName.length; i++) {
                if (escape(sName.charAt(i)).length >= 4) cnt += 2;
                else cnt++;
            }
            if (cnt > 8) {
                alert('節目代號 欄位長度超過限制！');
                return false;
            }
            cnt = 0;
            sName = txtOriginal_ProgramCode.value;
            for (var i = 0; i < sName.length; i++) {
                if (escape(sName.charAt(i)).length >= 4) cnt += 2;
                else cnt++;
            }
            if (cnt > 8) {
                alert('原節目代號 欄位長度超過限制！');
                return false;
            }

            cnt = 0;
            cnt = txtEpisodeSynopsisOriginal.value.length;
            if (cnt > 3000) {
                alert('分集大綱（原版） 欄位長度超過限制！');
                return false;
            }
            cnt = 0;
            cnt = txtEpisodeSynopsis500.value.length;
            if (cnt > 500) {
                alert('分集大綱（500字版） 欄位長度超過限制！');
                return false;
            }
            cnt = 0;
            cnt = txtEpisodeSynopsis300.value.length;
            if (cnt > 300) {
                alert('分集大綱（300字版） 欄位長度超過限制！');
                return false;
            }
            cnt = 0;
            cnt = txtEpisodeSynopsis120.value.length;
            if (cnt > 120) {
                alert('分集大綱（120字版） 欄位長度超過限制！');
                return false;
            }
            cnt = 0;
            cnt = txtEpisodeSynopsisWEB.value.length;
            if (cnt > 1000) {
                alert('分集大綱（WEB版） 欄位長度超過限制！');
                return false;
            }
            */
            return true;
        }

    </script>
    <style type="text/css">

        .table_v TH {
            line-height: 35px;
        }

    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v"> 
            <tr>
                <th align="right" style="width: 10%;">
                    錄影日期：
                </th>
                <td align="left" style="width: 40%;">
                    <asp:TextBox runat="server" ID="txtVideoDate" onchange="CheckDateFormat(this, '錄影日期');" Width="200px"></asp:TextBox>
                    <img id="imgVideoDate" alt="" src="../images/date.gif" />
                </td>
                <th align="right" style="width: 10%;">
                    節目：
                </th>
                <td align="left" style="width: 40%">
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="white-space: nowrap; text-align: right;">
                    演出者：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPerformer_Singer" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    類別：
                </th>
                <td style="text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"><ContentTemplate>
                    <asp:DropDownList ID="ddlItem" runat="server" Width="200px"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    曲目名稱：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTracks" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    速度：
                </th>
                <td style="line-height: 30px;">
                    <asp:Image ID="Speed1" runat="server" ImageUrl="~/images/circle_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Speed2" runat="server" ImageUrl="~/images/circle_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Speed3" runat="server" ImageUrl="~/images/circle_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Speed4" runat="server" ImageUrl="~/images/circle_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Speed5" runat="server" ImageUrl="~/images/circle_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:TextBox runat="server" ID="txtSpeed" style="display: none;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    長度：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtMusicLengthMin" Width="50px"></asp:TextBox> 分
                    <asp:TextBox runat="server" ID="txtMusicLengthSec" Width="30px"></asp:TextBox> 秒
                </td>
                <th style="text-align: right;">
                    評比：
                </th>
                <td>
                    <asp:Image ID="Rating1" runat="server" ImageUrl="~/images/star_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Rating2" runat="server" ImageUrl="~/images/star_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Rating3" runat="server" ImageUrl="~/images/star_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Rating4" runat="server" ImageUrl="~/images/star_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:Image ID="Rating5" runat="server" ImageUrl="~/images/star_untick.png" Width="28px" ImageAlign="AbsMiddle" style="cursor: pointer;" />
                    <asp:TextBox runat="server" ID="txtRating" style="display: none;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    SN編號：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSN1Number" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    曲風：
                </th>
                <td>
                    <asp:Label ID="lblMusicalStyles" runat="server" Width="450px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    Take：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTake" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    樂器：
                </th>
                <td>
                    <asp:Label ID="lblMusicalInstruments" runat="server" Width="450px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    審核：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAudit" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    語言：
                </th>
                <td>
                    <asp:Label ID="lblLanguage" runat="server" Width="450px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    作詞：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAuthors" Width="450px"></asp:TextBox>
                </td>
                <th style="text-align: right;">
                    節期：
                </th>
                <td>
                    <asp:Label ID="lblFestivals" runat="server" Width="450px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    作曲：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtCompose" Width="450px"></asp:TextBox>
                </td>
                <th style="white-space: nowrap; text-align: right;">
                    腳本/集數：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtScriptMark" Width="450px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" style="white-space: nowrap;">
                    編曲/改編：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtArranger_Adaptation" Width="450px"></asp:TextBox>
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
                    <asp:Textbox runat="server" ID="txtDescribed" TextMode="MultiLine" Width="450px" Height="100px"></asp:Textbox>
                </td>
                <th style="text-align: right;">
                    備註：
                </th>
                <td>
                    <asp:Textbox runat="server" ID="txtRemark" TextMode="MultiLine" Width="450px" Height="100px"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnDel" runat="server" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick= "return confirm('您是否確定要刪除？'); "/>
                </td>
                <td align="right" colspan="3">
                <asp:Button ID="btnEdit" runat="server" Text="儲存" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckFieldMustFillBasic(); "/>
                <asp:Button ID="btnExit" runat="server" Text="離開" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
