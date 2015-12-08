<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramEpisodeEdit.aspx.cs" Inherits="View_ProgramEpisodeEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>分集內容編輯</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        window.onload = initCalendar; 
        function initCalendar() {
            Calendar.setup({
                inputField: "txtPremiereDate",   // id of the input field
                button: "imgPremiereDate"     // 與觸發動作的物件ID相同
            });
            ChkKeyIn_Fn(document.getElementById('txtEpisodeSynopsisOriginal'), 'Origin');
            ChkKeyIn_Fn(document.getElementById('txtEpisodeSynopsis500'), '500');
            ChkKeyIn_Fn(document.getElementById('txtEpisodeSynopsis300'), '300');
            ChkKeyIn_Fn(document.getElementById('txtEpisodeSynopsis120'), '120');
            ChkKeyIn_Fn(document.getElementById('txtEpisodeSynopsisWEB'), 'WEB');
        }
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var cnt = 0;
            var sName = '';
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
            /*if (txtOriginal_ProgramCode.value == "") {
                strRet += "原節目代號 ";
            }
            if (txtOriginal_Episode.value == "") {
                strRet += "原集數 ";
            }*/
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
            return true;
        }

        //檢查輸入字數功能函式
        function ChkKeyIn_Fn(objElement,val) {
            var intCount = 0;
            //取得指定標籤id物件。
            var strTextOriginal = document.getElementById("lblOriginal");
            var strText500 = document.getElementById("lbl500");
            var strText300 = document.getElementById("lbl300");
            var strText120 = document.getElementById("lbl120");
            var strTextWEB = document.getElementById("lblWEB");

            //取得字串值長度。
            intCount = objElement.value.length;
            //計算剩餘字串值長度。
            if (val =='Origin')
                strTextOriginal.value = "已輸入字數：" + (parseInt(intCount));
            if (val == '500')
                strText500.value = "已輸入字數：" + (parseInt(intCount));
            if (val == '300')
                strText300.value = "已輸入字數：" + (parseInt(intCount));
            if (val == '120')
                strText120.value = "已輸入字數：" + (parseInt(intCount));
            if (val == 'WEB')
                strTextWEB.value = "已輸入字數：" + (parseInt(intCount));
        }

    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
    <asp:HiddenField runat="server" ID="HFD_Uid" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v"> 
            <tr>
                <th align="right" colspan="1"><font color="red">*</font>
                    節目代號：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtProgramCode" CssClass="font9" Width="100px"></asp:TextBox> 
                    <asp:Button ID="btnQuery" runat="server" Width="15mm" Text="查詢" OnClick="btnQuery_Click"/> 
                    <asp:Label ID="lblProgramName" runat="server" Text=""></asp:Label> 
                </td>
                <th align="right" colspan="1"><font color="red">*</font>
                    集數：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtEpisode" CssClass="font9" Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    原節目代號：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtOriginal_ProgramCode" CssClass="font9" BackColor="#E6E6FA" ReadOnly="True" Width="100px"></asp:TextBox>
                </td>
                <th align="right" colspan="1">
                    原集數：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtOriginal_Episode" CssClass="font9" BackColor="#E6E6FA" ReadOnly="True" Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    大系列名稱：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtMainSeries" CssClass="font9" Width="200px"></asp:TextBox>
                </td>
                <th align="right" colspan="1">
                    系列名稱：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtSubSeries" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    分集名稱：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtEpisodeName" CssClass="font9" Width="600px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    分集大綱（原版）：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtEpisodeSynopsisOriginal" cssclass="font9"
                        TextMode="MultiLine" Width="600px" Height="100px" onblur="ChkKeyIn_Fn(this,'Origin');" onkeyup="ChkKeyIn_Fn(this,'Origin');"></asp:TextBox><br /><asp:Textbox ID="lblOriginal" runat="server" ReadOnly="True" BackColor="#E6E6FA"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th colspan="1" align="right">
                    分集大綱（500字版）：
                </th>
                <td colspan="5">
                    <asp:Textbox runat="server" ID="txtEpisodeSynopsis500" CssClass="font9" 
                        TextMode="MultiLine" Width="600px" Height="100px" onblur="ChkKeyIn_Fn(this,'500');" onkeyup="ChkKeyIn_Fn(this,'500');"></asp:Textbox><br /><asp:Textbox ID="lbl500" runat="server" ReadOnly="True" BackColor="#E6E6FA"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th colspan="1" align="right">
                    分集大綱（300字版）：
                </th>
                <td colspan="2">
                    <asp:Textbox runat="server" ID="txtEpisodeSynopsis300" CssClass="font9" 
                        TextMode="MultiLine" Width="350px" Height="100px" onblur="ChkKeyIn_Fn(this,'300');" onkeyup="ChkKeyIn_Fn(this,'300');"></asp:Textbox><br /><asp:Textbox ID="lbl300" runat="server" ReadOnly="True" BackColor="#E6E6FA"></asp:Textbox>
                </td>
                <th colspan="1" align="right">
                    分集大綱（120字版）：
                </th>
                <td colspan="2">
                    <asp:Textbox runat="server" ID="txtEpisodeSynopsis120" CssClass="font9" 
                        TextMode="MultiLine" Width="350px" Height="100px" onblur="ChkKeyIn_Fn(this,'120');" onkeyup="ChkKeyIn_Fn(this,'120');"></asp:Textbox><br /><asp:Textbox ID="lbl120" runat="server" ReadOnly="True" BackColor="#E6E6FA"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th colspan="1" align="right">
                    分集大綱（WEB版）：
                </th>
                <td colspan="5">
                    <asp:Textbox runat="server" ID="txtEpisodeSynopsisWEB" CssClass="font9" 
                        TextMode="MultiLine" Width="600px" Height="100px" onblur="ChkKeyIn_Fn(this,'WEB');" onkeyup="ChkKeyIn_Fn(this,'WEB');"></asp:Textbox><br /><asp:Textbox ID="lblWEB" runat="server" ReadOnly="True" BackColor="#E6E6FA"></asp:Textbox> 
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    主持人/講員：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtPresenterSpeaker" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
                <th align="right" colspan="1">
                    來賓姓名：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtGuestName" CssClass="font9" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    總長度：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtTotalLength" CssClass="font9" Width="150px" BackColor="#E6E6FA" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    分段長度：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtPartLength" CssClass="font9" Width="800px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    段數：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="font9" BackColor="#E6E6FA" ReadOnly="True" Width="70px"></asp:TextBox>
                </td>
                <th align="right" colspan="1">
                    首播日期：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox ID="txtPremiereDate" runat="server" onchange="CheckDateFormat(this, '首播日期');"></asp:TextBox>
                    <img id="imgPremiereDate" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    註記：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:checkbox runat="server" ID="cbxCopyrightNote" Text="版權註記" CssClass="font9"></asp:checkbox>
                    <asp:checkbox runat="server" ID="cbxNoBroadcastNote" Text="不播出註記" CssClass="font9"></asp:checkbox>
                    <asp:checkbox runat="server" ID="cbxNoReplayNote" Text="不重播註記" CssClass="font9"></asp:checkbox>
                    <asp:checkbox runat="server" ID="cbxNoProvidevideoNote" Text="不供片註記" CssClass="font9"></asp:checkbox>
                    <asp:checkbox runat="server" ID="cbxIsLock" Text="更新鎖定" CssClass="font9"></asp:checkbox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    提醒記要：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtReminderNotes" CssClass="font9" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    製作人重播建議：
                </th>
                <td align="left" colspan="2" class="style1">
                    <%--<asp:RadioButtonList ID="rblPackagerReplaySuggest" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>--%>
                    <asp:TextBox runat="server" ID="txtPackagerReplaySuggest" CssClass="font9" Width="15px"></asp:TextBox> 顆星
                </td>
                <th align="right" colspan="1">
                    編審重播建議：
                </th>
                <td align="left" colspan="2" class="style1">
                    <%-- <asp:RadioButtonList ID="rblEditorReplaySuggest" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>--%>
                    <asp:TextBox runat="server" ID="txtEditorReplaySuggest" CssClass="font9" Width="15px"></asp:TextBox> 顆星
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnDel" runat="server" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick= "return confirm('您是否確定要刪除？'); "/>
                </td>
                <td align="right" colspan="5">
                <asp:Button ID="btnEdit" runat="server" Text="修改" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckFieldMustFillBasic(); "/>
                <asp:Button ID="btnExit" runat="server" Text="取消" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
