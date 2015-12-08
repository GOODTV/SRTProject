<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseShortfilmLogQuery.aspx.cs" Inherits="View_ReleaseShortfilmLogQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>短片交檔記錄</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../Script/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#GridView1 tr td").click(function () {

                window.open("ReleaseShortFilmLogEdit.aspx?SerNo=" + $(this).parent().children().eq(0).text() + "&Key=" + $("#HFD_Key").val(), "_self");
            });

        });
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var tbxCustomerID = document.getElementById('tbxCustomerID');
            var tbxFilmID = document.getElementById('tbxFilmID');
            var tbxEpisodeNo_Start = document.getElementById('tbxEpisodeNo_Start');
            var tbxEpisodeNo_End = document.getElementById('tbxEpisodeNo_End');

            var ddlYearS = document.getElementById('ddlYearS');
            var ddlMonthS = document.getElementById('ddlMonthS');
            var ddlYearE = document.getElementById('ddlYearE');
            var ddlMonthE = document.getElementById('ddlMonthE');
            if (ddlYearE.value < ddlYearS.value) {
                strRet += "供片年月起始年度不能小於結束年度！";
                alert(strRet);
                return false;
            }
            //if (tbxCustomerID.value == "" && (tbxFilmID.value == "" || tbxEpisodeNo_Start.value == "")) {
            if (tbxCustomerID.value == "" && tbxFilmID.value == "") {
                strRet += "至少需輸入一個查詢條件：客戶或短片代號";
                alert(strRet);
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">

        .table_h th:first-child {

            display: none;

        }

        .table_h td {

            cursor: pointer;

        }

        .table_h td:first-child {

            display: none;

        }

    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td align="left">
                    客戶代號：<asp:TextBox ID="tbxCustomerID" CssClass="font9" runat="server" Width="100px" />
                </td>
                <td align="left">
                    短片代號：<asp:TextBox ID="tbxFilmID" CssClass="font9" runat="server" Width="100px" />
                </td>
                <td align="left">
                    集數：<asp:TextBox ID="tbxEpisodeNo_Start" CssClass="font9" runat="server" Width="50px" />～
                    <asp:TextBox ID="tbxEpisodeNo_End" CssClass="font9" runat="server" Width="50px" />
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="btnQuery" runat="server"   Width="20mm" Text="查詢" OnClick="btnQuery_Click" OnClientClick= "return CheckFieldMustFillBasic();"/>
                    <asp:Button ID="btnToxls" runat="server"  Width="20mm"
                    Text="匯出" OnClick="btnToxls_Click" OnClientClick=" return confirm('您是否確定要將查詢結果匯出？');"/>
                    <asp:Button ID="btnAdd" runat="server"  Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="5" class="style1">
                    <font color="red">*</font>供片年月：　年度<asp:DropDownList ID="ddlYearS" runat="server" CssClass="font9">
                    </asp:DropDownList>
                    月份<asp:DropDownList ID="ddlMonthS" runat="server" CssClass="font9">
                        <asp:ListItem Text=" " Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="1月" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2月" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3月" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4月" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5月" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6月" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7月" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8月" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9月" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10月" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11月" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12月" Value="12"></asp:ListItem>
                    </asp:DropDownList>～
                    年度<asp:DropDownList ID="ddlYearE" runat="server" CssClass="font9">
                    </asp:DropDownList>
                    月份<asp:DropDownList ID="ddlMonthE" runat="server" CssClass="font9">
                        <asp:ListItem Text=" " Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="1月" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2月" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3月" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4月" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5月" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6月" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7月" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8月" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9月" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10月" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11月" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12月" Value="12"></asp:ListItem>
                    </asp:DropDownList>　資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="9">
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                    <asp:GridView ID="GridView1" runat="server" Width="100%"></asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
