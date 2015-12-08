<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadEPG.aspx.cs" Inherits="UploadEPG" MasterPageFile="~/Master/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function showBlockUI() {
            //顯示上傳中的圖片            
            $.blockUI({ message: '檔案匯入處理中,請勿關閉瀏覽器!!!<img src="../App_Themes/image/PageBlock.gif" />' });
            $("#ContentPlaceHolder1_btnImport").click(); //執行上傳            
            //$.unblockUI();/*因為會postback頁面刷新，所以有沒有unblockUI沒差*/
        }
    
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"><p />
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell Width="8%" HorizontalAlign = "Right">
                <asp:Label ID="lblImportFile" runat="server" CssClass="tableLabel" Text="匯入檔案：" />
            </asp:TableCell>
            <asp:TableCell Width="92%" HorizontalAlign = "Left">
                <asp:FileUpload ID="fuFile" runat="server" Width="500px" />&nbsp;
                <input type="button" value="匯入" onclick="showBlockUI();" class="Button" />                
                <div style="display:none;"> 
                    <asp:Button ID="btnImport" runat="server" CssClass="Button" Text="匯入" OnClick="btnImport_Click" />
                </div>
                <asp:Button ID="btnQueryEPG" runat="server" Text="EPG查詢" OnClick="btnQueryEPG_Click" CssClass="Button" />
            </asp:TableCell>            
        </asp:TableRow>
</asp:Table>
</asp:Content>
