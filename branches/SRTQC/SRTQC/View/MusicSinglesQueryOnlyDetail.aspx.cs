using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_MusicSinglesQueryOnlyDetail : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("UniqueNumber");
            Form_DataBind();
        }

    }

    //帶入資料
    public void Form_DataBind()
    {
        //****變數宣告****//
        string strSql, uid;
        DataTable dt;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select * From [MusicSingles]  where [UniqueNumber] = " + uid ;
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);

        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("MusicSinglesQueryOnly.aspx");

        DataRow dr = dt.Rows[0];

        //類別	Category
        txtCategory.Text = dr["Category"].ToString();
        //項目	Item
        txtItem.Text = dr["Item"].ToString();
        //錄影日期	VideoDate
        if (dr["VideoDate"].ToString() != "")
        {
            txtVideoDate.Text = DateTime.Parse(dr["VideoDate"].ToString()).ToString("yyyy/MM/dd");
        }
        //演出者/演唱者	Performer_Singer
        txtPerformer_Singer.Text = dr["Performer_Singer"].ToString().Trim();
        //曲目	Tracks
        txtTracks.Text = dr["Tracks"].ToString().Trim();
        //敘述	Described
        txtDescribed.Text = dr["Described"].ToString().Trim().Replace("\n","<br />");
        //SN1編號	SN1Number
        txtSN1Number.Text = dr["SN1Number"].ToString().Trim();
        //Take
        txtTake.Text = dr["Take"].ToString().Trim();
        //作詞	Authors
        txtAuthors.Text = dr["Authors"].ToString().Trim();
        //作曲	Compose
        txtCompose.Text = dr["Compose"].ToString().Trim();
        //編曲/改編	Arranger_Adaptation
        txtArranger_Adaptation.Text = dr["Arranger_Adaptation"].ToString().Trim();
        //審核	Audit
        txtAudit.Text = dr["Audit"].ToString().Trim();
        //曲風	MusicalStyles
        txtMusicalStyles.Text = dr["MusicalStyles"].ToString().Trim();
        //樂器	MusicalInstruments
        txtMusicalInstruments.Text = dr["MusicalInstruments"].ToString().Trim();
        //語言	Language
        txtLanguage.Text = dr["Language"].ToString().Trim();
        //標記	ScriptMark
        txtScriptMark.Text = dr["ScriptMark"].ToString().Trim();
        //節期	Festivals
        txtFestivals.Text = dr["Festivals"].ToString().Trim();
        //備註	Remark
        txtRemark.Text = dr["Remark"].ToString().Trim().Replace("\n", "<br />");
        //音樂長度	MusicLength
        txtMusicLengthMin.Text = (Convert.ToInt32(dr["MusicLength"].ToString()) / 60).ToString();
        txtMusicLengthSec.Text = (Convert.ToInt32(dr["MusicLength"].ToString()) % 60).ToString();
        //速度	Speed
        /*
        string Speed = dr["Speed"].ToString();
        switch (Speed)
        {
            case "1":
                txtSpeed.Text = "<SPAN style='FONT-SIZE: 27px; COLOR: #0066ff'>❶</SPAN>";
                break;
            case "2":
                txtSpeed.Text = "<SPAN style='FONT-SIZE: 27px; COLOR: #0066ff'>❷</SPAN>";
                break;
            case "3":
                txtSpeed.Text = "<SPAN style='FONT-SIZE: 27px; COLOR: #0066ff'>❸</SPAN>";
                break;
            case "4":
                txtSpeed.Text = "<SPAN style='FONT-SIZE: 27px; COLOR: #0066ff'>❹</SPAN>";
                break;
            case "5":
                txtSpeed.Text = "<SPAN style='FONT-SIZE: 27px; COLOR: #0066ff'>❺</SPAN>";
                break;

        }
        */
        int intSpeed = String.IsNullOrEmpty(dr["Speed"].ToString()) ? 0 : Convert.ToInt32(dr["Speed"]);
        for (int i = 1; i <= intSpeed; i++)
        {
            txtSpeed.Text += "<IMG style='WIDTH: 28px;' src='../images/circle_tick.png'>";
        }

        //評比	Rating
        int intRating = String.IsNullOrEmpty(dr["Rating"].ToString()) ? 0 : Convert.ToInt32(dr["Rating"]);
        for (int i = 1; i <= intRating; i++)
        {
            txtRating.Text += "<IMG style='WIDTH: 28px;' src='../images/star_tick.png'>";
        }

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("MusicSinglesQueryOnly.aspx");
    }

}
