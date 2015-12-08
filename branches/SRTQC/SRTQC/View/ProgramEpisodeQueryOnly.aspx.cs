using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ProgramEpisodeQueryOnly : System.Web.UI.Page
{
    #region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage, HFD_CurrentQuerye;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }
    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);

        HFD_CurrentQuerye = new HiddenField();
        HFD_CurrentQuerye.Value = "Query";
        HFD_CurrentQuerye.ID = "HFHFD_CurrentQuerye";
        Form1.Controls.Add(HFD_CurrentQuerye);
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼

    protected void Page_Load(object sender, EventArgs e)
    {
        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            hfExportExcelVisible.Value = Request.QueryString["ExportExcel"];
            if (hfExportExcelVisible.Value == "0") btnExportExcel.Visible = false;
            else btnExportExcel.Enabled = false;
            //hfExportExcelVisible
            Session["strSql"] = "";
            Session["strSqlWhere"] = "";
        }

    }

    public void LoadFormData()
    {
        string strSql;
        string strSqlWhere = "";
        DataTable dt;
        strSql = @"select ProgramID ,ProgramCode as [節目代號], Episode as [集數], [ProgramName] as [節目名稱] ,SubSeries as [系列名稱] ,PresenterSpeaker as [主持人/講員] ,
                    TotalLength as [長度] ,Case when CopyrightNote = '1' then 'V' else '' end as [版權註記] , Case when NoBroadcastNote = '1' then 'V' else '' end as [不播出註記] ,
                    Case when NoReplayNote = '1' then 'V' else '' end as [不重播註記] , Case when NoProvidevideoNote = '1' then 'V' else '' end as [不供片註記] ,
                    Case when Lock = 'Y' then 'V' else '' end as [更新鎖定], Case when ISNULL(ReminderNotes,'') <> '' then 'V' else '' end as [提醒紀要] ,
                    Original_ProgramCode as [原節目代號] ,Original_Episode as [原集數], MainSeries as [大系列名稱], EpisodeName as [分集名稱], GuestName as [來賓] ,
                    PackagerReplaySuggest as [製評],EditorReplaySuggest as [編評],PartLength ,CONVERT(VarChar,[PremiereDate],111) as [首播日期]
                    from (select *,[dbo].[getProgramName]([ProgramCode]) as [ProgramName] from dbo.ProgramEpisode) as pe where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        //全文檢索
        if (tbFullTextSearch.Text.Trim() != "")
        {
            string[] aText = tbFullTextSearch.Text.Trim().Split(' ');
            foreach (string strTextSearch in aText)
            {
                strSqlWhere += " AND ([ProgramCode]+ISNULL([Original_ProgramCode],'')+ISNULL([ProgramName],'')+ISNULL([MainSeries],'')+"+
                    "ISNULL([SubSeries],'')+ISNULL([EpisodeName],'')+ISNULL([EpisodeSynopsisOriginal],'')+ISNULL([EpisodeSynopsis500],'')+"+
                    "ISNULL([EpisodeSynopsis300],'')+ISNULL([EpisodeSynopsis120],'')+ISNULL([EpisodeSynopsisWEB],'')+"+
                    "ISNULL([PresenterSpeaker],'')+ISNULL([GuestName],'')+ISNULL([ReminderNotes],'') like '%" + strTextSearch + "%') ";
            }
        }
        //集數或原集數
        if (tbxEpisodeNo_Start.Text.Trim() != "" && tbxEpisodeNo_End.Text.Trim() != "")
        {
            strSqlWhere += " AND (([Episode] >= " + tbxEpisodeNo_Start.Text.Trim() + " AND [Episode] <= " + tbxEpisodeNo_End.Text.Trim()
            + ") OR (ISNULL([Original_Episode],0) >= " + tbxEpisodeNo_Start.Text.Trim() + " AND ISNULL([Original_Episode],0) <= " + tbxEpisodeNo_End.Text.Trim() + ")) ";
        }
        else if (tbxEpisodeNo_Start.Text.Trim() != "")
        {
            strSqlWhere += " AND ([Episode] >= " + tbxEpisodeNo_Start.Text.Trim() + " OR ISNULL([Original_Episode],0) >= " + tbxEpisodeNo_Start.Text.Trim() + ") ";
        }
        else if (tbxEpisodeNo_End.Text.Trim() != "")
        {
            //原集數為零等於沒有集數，所以不列入查詢範圍內。
            strSqlWhere += " AND ([Episode] <= " + tbxEpisodeNo_End.Text.Trim() + " OR (ISNULL([Original_Episode],0) >= 1 AND ISNULL([Original_Episode],0) <= " + tbxEpisodeNo_End.Text.Trim() + ")) ";
        }
        //總長度
        if (txtTotalLength1.Text.Trim() != "" || txtTotalLength2.Text.Trim() != "")
        {
            strSqlWhere += " AND LEN([TotalLength]) = 11 AND ISNUMERIC(SUBSTRING([TotalLength],1,2)) = 1 AND ISNUMERIC(SUBSTRING([TotalLength],4,2)) = 1 ";
        }
        if (txtTotalLength1.Text.Trim() != "")
        {
            strSqlWhere += " AND SUBSTRING([TotalLength],1,2)*60+SUBSTRING([TotalLength],4,2) >= " + txtTotalLength1.Text.Trim();
        }
        if (txtTotalLength2.Text.Trim() != "")
        {
            strSqlWhere += " AND SUBSTRING([TotalLength],1,2)*60+SUBSTRING([TotalLength],4,2) <= " + txtTotalLength2.Text.Trim();
        }
        //首播日期
        if (txtPremiereDate1.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST([PremiereDate] AS date) >= CAST('" + txtPremiereDate1.Text.Trim() + "' AS date) ";
        }
        if (txtPremiereDate2.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST([PremiereDate] AS date) <= CAST('" + txtPremiereDate2.Text.Trim() + "' AS date) ";
        }
        //版權註記
        if (cbxCopyrightNote.SelectedValue == "1")
        {
            strSqlWhere += " AND ISNULL([CopyrightNote],'0') = '1' ";
        }
        else if (cbxCopyrightNote.SelectedValue == "0")
        {
            strSqlWhere += " AND ISNULL([CopyrightNote],'0') <> '1' ";
        }
        //不播出註記
        if (cbxNoBroadcastNote.SelectedValue == "1")
        {
            strSqlWhere += " AND ISNULL([NoBroadcastNote],'0') = '1' ";
        }
        else if (cbxNoBroadcastNote.SelectedValue == "0")
        {
            strSqlWhere += " AND ISNULL([NoBroadcastNote],'0') <> '1' ";
        }
        //不重播註記
        if (cbxNoReplayNote.SelectedValue == "1")
        {
            strSqlWhere += " AND ISNULL([NoReplayNote],'0') = '1' ";
        }
        else if (cbxNoReplayNote.SelectedValue == "0")
        {
            strSqlWhere += " AND ISNULL([NoReplayNote],'0') <> '1' ";
        }
        //不供片註記
        if (cbxNoProvidevideoNote.SelectedValue == "1")
        {
            strSqlWhere += " AND ISNULL([NoProvidevideoNote],'0') = '1' ";
        }
        else if (cbxNoProvidevideoNote.SelectedValue == "0")
        {
            strSqlWhere += " AND ISNULL([NoProvidevideoNote],'0') <> '1' ";
        }
        //更新鎖定
        if (cbxIsLock.SelectedValue == "1")
        {
            strSqlWhere += " AND ISNULL([Lock],'N') = 'Y' ";
        }
        else if (cbxIsLock.SelectedValue == "0")
        {
            strSqlWhere += " AND (ISNULL([Lock],'N') <> 'Y' OR [Lock] = '' ) ";
        }
        //製評(製作人重播建議)
        if (txtPackagerReplaySuggest1.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST(ISNULL([PackagerReplaySuggest],0) as int) >= CAST('" + txtPackagerReplaySuggest1.Text.Trim() + "' as int) ";
        }
        if (txtPackagerReplaySuggest2.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST(ISNULL([PackagerReplaySuggest],0) as int) <= CAST('" + txtPackagerReplaySuggest2.Text.Trim() + "' as int) ";
        }
        //編評(編審重播建議)
        if (txtEditorReplaySuggest1.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST(ISNULL([EditorReplaySuggest],0) as int) >= CAST('" + txtEditorReplaySuggest1.Text.Trim() + "' as int) ";
        }
        if (txtEditorReplaySuggest2.Text.Trim() != "")
        {
            strSqlWhere += " AND CAST(ISNULL([EditorReplaySuggest],0) as int) <= CAST('" + txtEditorReplaySuggest2.Text.Trim() + "' as int) ";
        }
        strSql += strSqlWhere + " order by ProgramCode,Episode ";
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
            btnExportExcel.Enabled = false;
            Session["strSql"] = "";
            Session["strSqlWhere"] = "";
        }
        else
        {
            //換掉原先的 Html Table
//            string strBody = "<table class='table_h' width='100%'>";

//            foreach (DataRow dr in dt.Rows)
//            {
//                strBody += @"<TR><TH noWrap>節目代號</SPAN></TH>
//                            <TH noWrap><SPAN>集數</SPAN></TH>
//                            <TH noWrap><SPAN>節目名稱</SPAN></TH>
//                            <TH noWrap><SPAN>系列名稱</SPAN></TH>
//                            <TH noWrap><SPAN>主持人/講員</SPAN></TH>
//                            <TH noWrap><SPAN>長度</SPAN></TH>
//                            <TH noWrap><SPAN>版權註記</SPAN></TH>
//                            <TH noWrap><SPAN>不播出註記</SPAN></TH>
//                            <TH noWrap><SPAN>不重播註記</SPAN></TH>
//                            <TH noWrap><SPAN>不供片註記</SPAN></TH>
//                            <TH noWrap><SPAN>更新鎖定</SPAN></TH>
//                            <TH noWrap><SPAN>提醒紀要</SPAN></TH></TR>";
//                strBody += "<TR onclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("ProgramEpisodeDetail.aspx", "ProgramID=" + dr["ProgramID"].ToString()) + "\',\'_self\',\'\')\">";
//                strBody += "<TD noWrap><SPAN>" + dr["節目代號"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["集數"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["節目名稱"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["系列名稱"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["主持人or講員"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["總長度"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["版權註記"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["不播出註記"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["不重播註記"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["不供片註記"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["更新鎖定"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["提醒紀要"].ToString() + "</SPAN></TD></TR>";
//                strBody += @"<TR><TH noWrap>舊代號</SPAN></TH>
//                            <TH noWrap><SPAN>集數</SPAN></TH>
//                            <TH noWrap><SPAN>大系列名稱</SPAN></TH>
//                            <TH noWrap><SPAN>分集名稱</SPAN></TH>
//                            <TH noWrap><SPAN>來賓</SPAN></TH>
//                            <TH noWrap><SPAN>段數</SPAN></TH>
//                            <TH colspan='3'><SPAN>製評</SPAN></TH>
//                            <TH colspan='3'><SPAN>編評</SPAN></TH></TR>";
//                strBody += "<TR onclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("ProgramEpisodeDetail.aspx", "ProgramID=" + dr["ProgramID"].ToString()) + "\',\'_self\',\'\')\">";
//                strBody += "<TD noWrap><SPAN>" + dr["原節目代號"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["原集數"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["大系列名稱"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["分集名稱"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center'><SPAN>" + dr["來賓"].ToString() + "</SPAN></TD>";
//                int PartLength = dr["PartLength"].ToString().Split('@').Length - 1;
//                strBody += "<TD align='center'><SPAN>" + PartLength + "</SPAN></TD>";
//                strBody += "<TD align='center' colspan='3'><SPAN>" + dr["製評"].ToString() + "</SPAN></TD>";
//                strBody += "<TD align='center' colspan='3'><SPAN>" + dr["編評"].ToString() + "</SPAN></TD>";
//            }

//            strBody += "</table>";
//            lblGridList.Text = strBody;
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("ProgramID");
            npoGridView.DisableColumn.Add("ProgramID");
            npoGridView.DisableColumn.Add("PartLength");
            npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = true; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ProgramEpisodeQueryOnlyDetail.aspx", "ExportExcel=" + hfExportExcelVisible.Value + "&ProgramID=");
            npoGridView.EditLinkTarget = "_blank";  //開啟新視窗
            npoGridView.EditLinkDoubleClick = true;     // double-click
            npoGridView.ColumnByRow = 12; //設定每行12個欄位
            npoGridView.PageSize = 10;
            npoGridView.CssClass = "table_line2";
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;

            btnExportExcel.Enabled = true;
            Session["strSql"] = strSql;
            Session["strSqlWhere"] = strSqlWhere;
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

        if (Session["strSql"] == null || Session["strSql"].ToString() == "")
        {
            return;
        }
        string strSql = Session["strSql"].ToString();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0) return;

        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.DisableColumn.Add("ProgramID");
        npoGridView.DisableColumn.Add("PartLength");
        npoGridView.ShowPage = false;
        npoGridView.CssClass = "table_line2";
        string ExcelBody = npoGridView.Render();

        //int intIndex = ExcelBody.IndexOf("</table>");
        //ExcelBody = ExcelBody.Substring(intIndex + 9);
        string[] arryExcelBody = ExcelBody.Split(new string[] { "<tr   >" }, System.StringSplitOptions.RemoveEmptyEntries);
        ExcelBody = arryExcelBody[0];
        
        for (int i = 1; i < arryExcelBody.Length; i++)
        {
            if (i % 2 == 0)
            {
                ExcelBody += "<tr class='table_tr_background'>" + arryExcelBody[i];
            }
            else
            {
                ExcelBody += "<tr>" + arryExcelBody[i];
            }
        }
        
        string strExcel = @"
<style type=text/css>

		table {
				font-size: 16px;
				font-family: 標楷體;
		}

		#Title {
				font-size: 21px;
				font-weight: bold;
		}
				
        .table_line2 {
            border-collapse: collapse;
            empty-cells: show;
        }

        .table_line2 tr th {
            padding: 2px;
            border: #000 solid 0.5pt;
            background: #FEFE98;
        }

        .table_line2 tr td {
            padding: 2px;
            border: #000 solid 0.5pt;
        }

        .table_tr_background {
            background: #E6F8FF;
        }
 
</style>
";
        strExcel += "<table><tr><td style='text-align: center;'><span id='Title'>分集基本資料查詢結果</span></td><tr><td style='text-align: center;'>" + ExcelBody + "</td></tr></table>";
        Util.OutputTxt(strExcel, "1", "ProgramEpisode" + DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

}
