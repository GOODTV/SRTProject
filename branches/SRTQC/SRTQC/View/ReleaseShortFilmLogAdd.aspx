<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseShortFilmLogAdd.aspx.cs" Inherits="View_ReleaseShortFilmLogAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>短片交檔記錄新增</title>
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
                inputField: "txtSupplyDate",   // id of the input field
                button: "imgSupplyDate"     // 與觸發動作的物件ID相同
            });
        }
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var cnt = 0;
            var sName = "";
            var txtFilmID = document.getElementById('txtFilmID');
            var txtCustomerID = document.getElementById('txtCustomerID');
            var txtSupplyDate = document.getElementById('txtSupplyDate');

            if (txtFilmID.value == "") {
                strRet += "短片代號不可空白";
                alert(strRet);
                return false;
            }
            if (txtCustomerID.value == "") {
                strRet += "客戶代號不可空白";
                alert(strRet);
                return false;
            }
            if (txtSupplyDate.value == "") {
                strRet += "供片時間不可空白";
                alert(strRet);
                return false;
            }
            cnt = 0;
            sName = txtCustomerID.value;
            for (var i = 0; i < sName.length; i++) {
                if (escape(sName.charAt(i)).length >= 4) cnt += 2;
                else cnt++;
            }
            if (cnt > 3) {
                alert('客戶代號 欄位長度超過限制！');
                return false;
            }
            return true;
        }
        function WindowsOpen() {
            window.open('CustomerShow.aspx?customerID=txtCustomerID&customerName=tbxCustomerName', 'NewWindows',
                        'status=no,scrollbars=yes,top=100,left=120,width=500,height=450;');
        }
        function WindowsOpenFilm() {
            window.open('ShortFilmShow.aspx?filmID=txtFilmID&filmName=lblShortFilmName&fileName=txtFilename', 'NewWindows',
                        'status=no,scrollbars=yes,top=100,left=120,width=500,height=450;');
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v"> 
            <tr>
                <asp:panel id="Panel_Query" runat="server" defaultbutton="btnQuery">
                    <th align="right" colspan="1"><font color="red">*</font>
                        短片代號：
                    </th>
                    <td align="left" colspan="2" class="style1">
                        <asp:TextBox runat="server" ID="txtFilmID" CssClass="font9" Width="100px"></asp:TextBox>&nbsp;<a href onclick="WindowsOpenFilm()" style="cursor:hand"><img border="0" src="../images/toolbar_search.gif" width="17"></a>
                        <asp:Button ID="btnQuery" runat="server" Width="15mm" Text="查詢" OnClick="btnQuery_Click"/>
                        <asp:TextBox ID="lblShortFilmName" runat="server" Text="" Width="150mm" ReadOnly="true" BorderStyle="None"></asp:TextBox>
                    </td>
                </asp:panel>
                <th align="right" colspan="1"><font color="red">*</font>
                    客戶代號：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtCustomerID" CssClass="font9" Width="50px" OnTextChanged="btnQueryCustomer_Click"></asp:TextBox>&nbsp;<!--a href onclick="WindowsOpen()" style="cursor:hand"><img border="0" src="../images/toolbar_search.gif" width="17"></a-->
                    <%-- <asp:Button ID="btnQueryCustomer" runat="server" Width="15mm" Text="查詢" OnClick="btnQueryCustomer_Click"/>--%>
                    <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                    <br /><div id="memo" style="color: red; font-size: medium;">輸入客戶代號，按「Enter」即可呈現客戶名稱。</div>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1"><font color="red">*</font>
                    供片時間：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtSupplyDate" CssClass="font9" Width="100px"></asp:TextBox>
                    <img id="imgSupplyDate" alt="" src="../images/date.gif" />
                </td>
                <th align="right" colspan="1">
                    檔案名稱：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtFilename" CssClass="font9" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                    <asp:Button ID="btnAdd" runat="server"  Width="20mm" Text="儲存" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnAdd_Click"/>
                    <asp:Button ID="btnExit" runat="server" Width="20mm" Text="離開" onclick="btnExit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
