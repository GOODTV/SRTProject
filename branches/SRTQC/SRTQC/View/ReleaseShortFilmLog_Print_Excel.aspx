<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseShortFilmLog_Print_Excel.aspx.cs" Inherits="View_ReleaseShortFilmLog_Print_Excel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <title>短片交檔記錄查詢</title>
</head>
<body>
    <script type="text/javascript">
        function SelfClose() {
            window.opener = null;
            window.open('', '_self', '');
            window.close();
        }
    </script>
    <form id="Form1" runat="server">
        <div>
            <asp:Literal ID="GridList" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
