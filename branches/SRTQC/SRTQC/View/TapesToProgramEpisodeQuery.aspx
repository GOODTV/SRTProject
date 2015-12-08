<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TapesToProgramEpisodeQuery.aspx.cs" Inherits="View_TapesToProgramEpisodeQuery" MasterPageFile="~/Master/MasterPage.master"%>

    <asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
        <script type="text/javascript">

            $(document).ready(function () {
                
            });

            function checkClear() {

                elm = document.forms[0];

                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type == "checkbox") {
                        elm.elements[i].checked = false;
                    }
                }

            }

            function checkAll(spanChk) {

                elm = document.forms[0];

                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type == "checkbox" && elm[i].id != spanChk.id) {
                        //if (elm.elements[i].checked != spanChk.checked) {
                        if (spanChk.checked) {
                            elm.elements[i].checked = true;
                            //elm.elements[i].click();
                        }
                        else {
                            elm.elements[i].checked = false;
                        }
                    }
                }

                checkOne();
            }

            function checkOne() {

                elm = document.forms[0];
                var Order_cnt = 0;
                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type == "checkbox" && elm[i].id != "ContentPlaceHolder1_gridEPGList_cbHeader") {
                        if (elm.elements[i].checked) {
                            Order_cnt++;
                        }
                    }
                }

                $("#ContentPlaceHolder1_lblSelectCount").text("已點選 " + thousandComma(Order_cnt) + " 筆");

            }

            function thousandComma(number) {
                var num = number.toString();
                var pattern = /(-?\d+)(\d{3})/;

                while (pattern.test(num)) {
                    num = num.replace(pattern, "$1,$2");

                }
                return num;

            }

            function checkquery() {

                // && $("#ContentPlaceHolder1_txtVideoTapeBarcode").val() == ""
                if ($("#ContentPlaceHolder1_txtOriginal_ProgramCode").val() == "") {
                    alert("未輸入原節目代碼！");
                    return false;
                }
                else {

                    if ($("#ContentPlaceHolder1_txtOriginal_Episode1").val() != "") {
                        var ex = /^\d+$/;
                        if (!ex.test($("#ContentPlaceHolder1_txtOriginal_Episode1").val())) {
                            alert("輸入的原集數之起集數不為整數！");
                            return false;
                        }
                    }
                    if ($("#ContentPlaceHolder1_txtOriginal_Episode2").val() != "") {
                        var ex = /^\d+$/;
                        if (!ex.test($("#ContentPlaceHolder1_txtOriginal_Episode2").val())) {
                            alert("輸入的原集數之訖集數不為整數！");
                            return false;
                        }
                    }

                    document.body.style.cursor = 'wait';
                    return true;
                }

            }

            function checkinput() {


                if ($("#ContentPlaceHolder1_txtProgramCode").val() == "") {
                    alert("未輸入新節目代碼！");
                    return false;
                }
                
                if ($("#ContentPlaceHolder1_txtEpisode").val() != "") {
                    var ex = /^\d+$/;
                    if (!ex.test($("#ContentPlaceHolder1_txtEpisode").val())) {
                        alert("輸入的新集數不為整數！");
                        return false;
                    }
                }

                elm = document.forms[0];

                var Episodes = "";
                var j = 0;
                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type == "checkbox" && elm[i].id != "ContentPlaceHolder1_gridEPGList_cbHeader") {
                        if (elm.elements[i].checked) {
                            j++;
                            var str = elm[i].id;
                            var intIndex = str.replace("ContentPlaceHolder1_gridEPGList_cbDel_", "");
                            var strValue = document.getElementById("ContentPlaceHolder1_gridEPGList_Original_Episode_" + intIndex).innerText;
                            if (Episodes == "")
                                Episodes += strValue;
                            else
                                Episodes += "," + strValue;
                        }
                    }
                }
                if (j == 0) {
                    alert("未勾選欲匯入的影帶！");
                    return false;
                }
                else if (j > 1 && $("#ContentPlaceHolder1_txtEpisode").val().trim() != "" && $("#ContentPlaceHolder1_txtEpisode").val().trim() != "0") {
                    if (confirm("確認多筆匯入，從輸入新集數起依序修改原集數嗎？")) {
                        var intEpisode = $("#ContentPlaceHolder1_txtEpisode").val();
                        Episodes = "";
                        for (k = 0; k < j; k++) {
                            Episodes += (k == 0 ? intEpisode : "," + intEpisode);
                            intEpisode++;
                        }
                    }
                    else
                        return false;
                }

                //document.body.style.cursor = 'wait';

                var PE_result = "";
                PE_result = ProgramEpisodeExist($("#ContentPlaceHolder1_txtProgramCode").val(), Episodes);
                //document.body.style.cursor =  'none';
                if (PE_result != "") {
                    if (!confirm("將匯入節目中已存在如下：\n" + PE_result + "確認要更新嗎？")) return false;
                }
                else
                    if (!confirm("確認要匯入嗎？")) return false;


                document.body.style.cursor = 'wait';

                return true;
                //alert("匯入 "+j+" 筆，完成！(for Test)");
                //return false;

            }

            //檢查新節目與及數是否已存在
            function ProgramEpisodeExist(NewPE, Episodes) {
                var result;
                $.ajax({
                    url: "/View/Ajax.aspx",
                    type: "POST",
                    data: { Type: 1, NewPE: NewPE, Episodes: Episodes },
                    async: false, //同步
                    success: function (response) {
                        result = response;
                    },
                    error: function () { alert('ajax failed'); }
                });

                return result;
            }

        </script>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True" />

        <table><tr><td align="left" valign="top">
        <asp:Panel ID="divQuery" runat="server">
            <table style="background-color: #FFFFCC">
                <tr align="left">
                    <td width="90">
                        <asp:Label ID="lblOriginal_ProgramCode" runat="server" Text="原節目代號" Width="80px"></asp:Label></td>
                    <td width="90">
                        <asp:TextBox ID="txtOriginal_ProgramCode" runat="server" Width="70px" /></td>
                    <td>&nbsp;</td>
                    <td width="60">
                        <asp:Label ID="lblOriginal_Episode" runat="server" Text="原集數"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtOriginal_Episode1" runat="server" Width="40px" /> ~ <asp:TextBox ID="txtOriginal_Episode2" runat="server" Width="40px" /></td>
                    <td>&nbsp;</td>
                    <td width="70">
                        <asp:Label ID="lblVideoTapeBarcode" runat="server" Text="影帶條碼"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtVideoTapeBarcode" runat="server" Width="80px" /></td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查詢影帶" OnClick="btnQuery_Click" OnClientClick="return checkquery();" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="divInput" runat="server" Visible="False">
            <table width="500px" style="background-color: #CCFFFF">
                <tr align="left">
                    <td width="90">
                        <asp:Label ID="lblProgramCode" runat="server" Text="新節目代號" Width="80px"></asp:Label></td>
                    <td width="90">
                        <asp:TextBox ID="txtProgramCode" runat="server" Width="70px" /></td>
                    <td>&nbsp;</td>
                    <td width="60">
                        <asp:Label ID="lblEpisode" runat="server" Text="新集數"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtEpisode" runat="server" Width="40px" /></td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnImprot" runat="server" Text="匯入分集資料維護" OnClick="btnImprot_Click" OnClientClick="return checkinput();" Width="150px" /></td>
                </tr>
            </table>
        </asp:Panel>
       </td>
       <td>&nbsp;</td>
       <td>
           <!--<span style="color: #0000FF">1.請在查詢結果勾選欲匯入的影帶<br />
                2.單筆匯入時，填入<font color="red">新節目代號</font>(必填)及<font color="red">新集數</font>(非必填)<br />
                3.多筆匯入時，將以填入的<font color="red">新節目代號</font>及<font color="red">新集數</font>依序替換原集數。
        </span>-->
        </td></tr></table>
        <br />
        <asp:Panel ID="InfoPanel" runat="server" Visible="False">
            <asp:Label ID="lblGrid" runat="server" Text="查詢結果：" CssClass="gridTitle"/>
            <asp:Label ID="lblGridCount" runat="server" ForeColor="Red" Font-Size="Small" />
            <asp:Label ID="lblSelectCount" runat="server" Text="已點選 0 筆" ForeColor="Blue" Font-Size="Small" />
        </asp:Panel>
                <!--<asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="24%" HeaderText="節目名稱" DataField="PlanningTitleName" />-->
                <!--<td width="24%" class="gridHead">節目名稱</td>-->

        <asp:Panel ID="GridPanel" runat="server">

            <asp:GridView ID="gridEPGList" runat="server" Width="100%"
                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                CellPadding="3" GridLines="Vertical" Font-Size="13px"
                OnRowDataBound="gridEPGList_RowDataBound"
                OnPageIndexChanging="gridEPGList_PageIndexChanging"
                OnPageIndexChanged="gridEPGList_PageIndexChanged" 
                AutoGenerateColumns="False">

                <AlternatingRowStyle BackColor="#DCDCDC" />

                <Columns>
                    <asp:TemplateField HeaderText="選取">
                    <HeaderTemplate>
                        <asp:CheckBox ID="cbHeader" runat="server" onclick="checkAll(this);" Text="" ToolTip="按一次全選，再按一次取消全選" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbDel" runat="server" onclick="checkOne();"/>
                    </ItemTemplate>
                   </asp:TemplateField>
                    <asp:BoundField HeaderText="編號" DataField="ROWNO" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="影帶條碼" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="False" SortExpression="VideoTapeBarcode">
                        <ItemTemplate>
                            <asp:Label ID="VideoTapeBarcode" runat="server" Text='<%# Bind("VideoTapeBarcode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProgramName" HeaderText="節目名稱" ItemStyle-Width="150px">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="原集數" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:Label ID="Original_Episode" runat="server" Text='<%# Bind("Original_Episode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TapesClassName" HeaderText="影帶類別" ItemStyle-Wrap="False">
                    </asp:BoundField>
                    <asp:BoundField DataField="SubSeries" HeaderText="系列名稱" ItemStyle-Width="150px">
                    </asp:BoundField>
                    <asp:BoundField DataField="EpisodeName" HeaderText="分集名稱" ItemStyle-Width="150px">
                    </asp:BoundField>
                    <asp:BoundField DataField="EpisodeSynopsisOriginal" HeaderText="分集大綱" ItemStyle-Width="150px">
                    </asp:BoundField>
                    <asp:BoundField DataField="PresenterSpeaker" HeaderText="主持人/講員" ItemStyle-Width="100px">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="來賓姓名" DataField="GuestName" ItemStyle-Width="100px">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="總長度" DataField="TotalLength" ItemStyle-Width="100px">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="分段長度" DataField="PartLength">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="首播日期" DataField="PremiereDate">
                    </asp:BoundField>
                    <asp:BoundField HeaderText="提醒記要" DataField="ReminderNotes">
                    </asp:BoundField>
                </Columns>

                <EmptyDataTemplate>
                    <table style="border: 1px none #999999; width: 100%; background-color: white; border-collapse: collapse;" rules="cols" cellspacing="0" cellpadding="3" >
                        <tr style="color: white; font-weight: bold; background-color:#000084; text-align: center; white-space: nowrap;">
                            <td>&nbsp;&nbsp;</td>
                            <td>編號</td>
                            <td>影帶條碼</td>
                            <td>原節目代號</td>
                            <td>節目名稱</td>
                            <td>原集數</td>
                            <td>系列名稱</td>
                            <td>分集名稱</td>
                            <td>分集大綱</td>
                            <td>主持人/講員</td>
                            <td>來賓姓名</td>
                            <td>總長度</td>
                            <td>分段長度</td>
                            <td>首播日期</td>
                            <td>提醒記要</td>
                        </tr>
                        <tr>
                            <td colspan="15" align="center">無符合條件的資料</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>

                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
                <PagerSettings FirstPageImageUrl="~/App_Themes/image/arrow_top.gif"
                    LastPageImageUrl="~/App_Themes/image/arrow_down.gif"
                    Mode="NextPreviousFirstLast"
                    NextPageImageUrl="~/App_Themes/image/arrow_next.gif"
                    PreviousPageImageUrl="~/App_Themes/image/arrow_back.gif" />
                <PagerStyle HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />

            </asp:GridView>

        </asp:Panel>
    </asp:Content>
