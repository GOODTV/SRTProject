using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_MusicSinglesQueryOnly : System.Web.UI.Page
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
        HFD_CurrentQuerye.ID = "HFD_CurrentQuerye";
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
            //string strQueryOnlyKey = Util.GetQueryString("QueryKey");
            string strQueryOnlyKey = Session["QueryOnlyKey"] != null ? Session["QueryOnlyKey"].ToString() : "";
            //Session["QueryOnlyKey"] = null;
            string[] arrayQueryOnlyKey = strQueryOnlyKey.Split('|');
            if (arrayQueryOnlyKey.Length == 9)
            {
                tbFullTextSearch.Text = arrayQueryOnlyKey[0];
                tbxMusicLengthMin_Start.Text = arrayQueryOnlyKey[1] == "" ? "" : Convert.ToInt32(arrayQueryOnlyKey[1]) / 60 == 0 ? "" : (Convert.ToInt32(arrayQueryOnlyKey[1]) / 60).ToString();
                tbxMusicLengthSec_Start.Text = arrayQueryOnlyKey[1] == "" ? "" : Convert.ToInt32(arrayQueryOnlyKey[1]) % 60 == 0 ? "" : (Convert.ToInt32(arrayQueryOnlyKey[1]) % 60).ToString();
                tbxMusicLengthMin_End.Text = arrayQueryOnlyKey[2] == "" ? "" : Convert.ToInt32(arrayQueryOnlyKey[2]) / 60 == 0 ? "" : (Convert.ToInt32(arrayQueryOnlyKey[2]) / 60).ToString();
                tbxMusicLengthSec_End.Text = arrayQueryOnlyKey[2] == "" ? "" : Convert.ToInt32(arrayQueryOnlyKey[2]) % 60 == 0 ? "" : (Convert.ToInt32(arrayQueryOnlyKey[2]) % 60).ToString();
                tbxSpeed_Start.Text = arrayQueryOnlyKey[3];
                tbxSpeed_End.Text = arrayQueryOnlyKey[4];
                tbxRating_Start.Text = arrayQueryOnlyKey[5];
                tbxRating_End.Text = arrayQueryOnlyKey[6];
                txtVideoDateBegin.Text = arrayQueryOnlyKey[7];
                txtVideoDateEnd.Text = arrayQueryOnlyKey[8];
                LoadFormData();
            }
        }

    }

    public void LoadFormData()
    {
        string strSql;
        string strSqlWhere = "";
        DataTable dt;
        strSql = @"SELECT [UniqueNumber] ,[Category] as [節目] ,[Item] as [類別] ,Convert(char(10),[VideoDate],111) as [錄影日期] 
                    ,[Performer_Singer] as [演出者] ,[Tracks] as [曲目名稱] 
                    ,rtrim(cast([MusicLength]/60 as char(5))) + '分' + rtrim(cast([MusicLength]%60 as char(5))) + '秒' as [長度] 
                    ,[Described] as [敘述] ,[SN1Number] as [SN編號] ,[Take] ,[Authors] as [作詞] 
                    ,[Compose] as [作曲] ,[Arranger_Adaptation] as [編曲/改編] ,[Audit] as [審核] 
                    ,[MusicalStyles] as [曲風] ,[MusicalInstruments] as [樂器] ,[Language] as [語言] 
                    ,[ScriptMark] as [腳本/集數] ,[Speed] as [速度] ,[Rating] as [評比] ,[Festivals] as [節期] 
                    ,[Remark] as [備註]
                   FROM [dbo].[MusicSingles]  where 1=1 ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //全文檢索
        /*
        唯一編號	UniqueNumber int
        節目	Category
        類別	Item
        演出者/演唱者	Performer_Singer
        曲目	Tracks
        敘述	Described
        SN1編號	SN1Number
        Take
        作詞	Authors
        作曲	Compose
        編曲/改編	Arranger_Adaptation
        審核	Audit
        曲風	MusicalStyles
        樂器	MusicalInstruments
        語言	Language
        標記	ScriptMark
        節期	Festivals
        備註	Remark
        */
        if (tbFullTextSearch.Text.Trim() != "")
        {
            string[] aText = tbFullTextSearch.Text.Trim().Split(' ');
            foreach (string strTextSearch in aText)
            {
                strSqlWhere += " AND (ISNULL([Category],'')+ISNULL([Item],'')+ISNULL([Performer_Singer],'')+ISNULL([Take],'')+" +
                "ISNULL([Tracks],'')+ISNULL([Described],'')+ISNULL([SN1Number],'')+ISNULL([Authors],'')+ISNULL([Compose],'')+" +
                "ISNULL([Arranger_Adaptation],'')+ISNULL([Audit],'')+ISNULL([MusicalStyles],'')+ISNULL([MusicalInstruments],'')+" +
                "ISNULL([Language],'')+ISNULL([ScriptMark],'')+ISNULL([Festivals],'')+ISNULL([Remark],'') like '%" + strTextSearch + "%') ";
            }
        }
        //錄影日期	VideoDate
        if (!String.IsNullOrEmpty(txtVideoDateBegin.Text))
        {
            strSqlWhere += " AND (CAST([VideoDate] as Date) >= CAST('" + txtVideoDateBegin.Text + "' as Date) ) ";
        }
        if (!String.IsNullOrEmpty(txtVideoDateEnd.Text))
        {
            strSqlWhere += " AND (CAST([VideoDate] as Date) <= CAST('" + txtVideoDateEnd.Text + "' as Date) ) ";
        }
        //音樂長度	MusicLength
        int intMusicLength_Start = (String.IsNullOrEmpty(tbxMusicLengthMin_Start.Text) ? 0 : Convert.ToInt32(tbxMusicLengthMin_Start.Text) * 60)
            + (String.IsNullOrEmpty(tbxMusicLengthSec_Start.Text) ? 0 : Convert.ToInt32(tbxMusicLengthSec_Start.Text));
        int intMusicLength_End = (String.IsNullOrEmpty(tbxMusicLengthMin_End.Text) ? 0 : Convert.ToInt32(tbxMusicLengthMin_End.Text) * 60)
            + (String.IsNullOrEmpty(tbxMusicLengthSec_End.Text) ? 0 : Convert.ToInt32(tbxMusicLengthSec_End.Text));

        if (intMusicLength_Start > 0)
        {
            strSqlWhere += " AND ([MusicLength] >= " + intMusicLength_Start.ToString() + ") ";
        }
        if (intMusicLength_End > 0)
        {
            strSqlWhere += " AND ([MusicLength] <= " + intMusicLength_End.ToString() + ") ";
        }
        //速度	Speed
        if (tbxSpeed_Start.Text != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],0) > 0 AND [Speed] >= " + tbxSpeed_Start.Text + " ) ";
        }
        if (tbxSpeed_End.Text != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],0) > 0 AND [Speed] <= " + tbxSpeed_End.Text + " ) ";
        }
        //評比	Rating
        if (tbxRating_Start.Text != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],0) > 0 AND [Rating] >= " + tbxRating_Start.Text + " ) ";
        }
        if (tbxRating_End.Text != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],0) > 0 AND [Rating] <= " + tbxRating_End.Text + " ) ";
        }

        /*
        //速度	Speed
        if (tbxSpeed_Start.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],'') <> '' AND CAST([Speed] as int) >= CAST('" + tbxSpeed_Start.Text.Trim() + "' as int) ) ";
        }
        if (tbxSpeed_End.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],'') <> '' AND CAST([Speed] as int) <= CAST('" + tbxSpeed_End.Text.Trim() + "' as int) ) ";
        }
        //評比	Rating
        if (tbxRating_Start.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],'') <> '' AND CAST([Rating] as int) >= CAST('" + tbxRating_Start.Text.Trim() + "' as int) ) ";
        }
        if (tbxRating_End.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],'') <> '' AND CAST([Rating] as int) <= CAST('" + tbxRating_End.Text.Trim() + "' as int) ) ";
        }
        */

        strSql += strSqlWhere + " order by Category,Item,Performer_Singer,Tracks ";
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            lblGridList.ForeColor = System.Drawing.Color.Red;
            Session["QueryOnlyKey"] = null;
        }
        else
        {
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("UniqueNumber");
            npoGridView.DisableColumn.Add("UniqueNumber");
            npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = true; //換頁
            npoGridView.EditLink = Util.RedirectByTime("MusicSinglesQueryOnlyDetail.aspx", "UniqueNumber=");
            //npoGridView.EditLinkTarget = "_blank";  //開啟新視窗
            npoGridView.EditLinkDoubleClick = true;     // double-click
            //npoGridView.ColumnByRow = 12; //設定每行12個欄位
            npoGridView.PageSize = 10;
            //npoGridView.CssClass = "table_line";
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();

            string strQueryOnlyKey = tbFullTextSearch.Text + "|";
            strQueryOnlyKey += (intMusicLength_Start == 0 ? "" : intMusicLength_Start.ToString()) + "|";
            strQueryOnlyKey += (intMusicLength_End == 0 ? "" : intMusicLength_End.ToString()) + "|";
            strQueryOnlyKey += tbxSpeed_Start.Text + "|";
            strQueryOnlyKey += tbxSpeed_End.Text + "|";
            strQueryOnlyKey += tbxRating_Start.Text + "|";
            strQueryOnlyKey += tbxRating_End.Text + "|";
            strQueryOnlyKey += txtVideoDateBegin.Text + "|";
            strQueryOnlyKey += txtVideoDateEnd.Text;
            Session["QueryOnlyKey"] = strQueryOnlyKey;
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
    }

    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

}
