using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_MusicSinglesEdit : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("UniqueNumber");
            Form_DataBind();
        }

    }

    private void loadDropDownList(DropDownList DropDownListName, string Category)
    {

        string strSql = "select distinct [CodeNo],[CodeName] from [dbo].[MusicSinglesCode] " +
                        " where [CodeType] = '" + Category + "' order by [CodeNo] ; ";
        Util.FillDropDownList(DropDownListName, strSql, "CodeName", "CodeName", true, "", "請選擇");

    }

    public string LoadCheckBoxListData(string Category, string CategoryName, string DonorTypeValues)
    {
        string strSql = "select distinct [CodeNo],[CodeName] from [dbo].[MusicSinglesCode] " +
                        " where [CodeType] = '" + Category + "' order by [CodeNo] ; ";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        List<ControlData> list = new List<ControlData>();
        list.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            string CodeName = dr["CodeName"].ToString().Trim();
            bool ShowBR = false;
            list.Add(new ControlData("Checkbox", CategoryName, CategoryName, CodeName, CodeName, ShowBR, DonorTypeValues));
        }
        return HtmlUtil.RenderControl(list);
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadDropDownList(ddlItem, ddlCategory.SelectedValue);

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
            Response.Redirect("MusicSinglesQuery.aspx");

        DataRow dr = dt.Rows[0];

        //類別	Category
        loadDropDownList(ddlCategory, "音樂單曲類別");
        ddlCategory.SelectedValue = dr["Category"].ToString();
        //項目	Item
        loadDropDownList(ddlItem, dr["Category"].ToString());
        ddlItem.SelectedValue = dr["Item"].ToString();
        //錄影日期	VideoDate
        if (dr["VideoDate"].ToString() != "")
        {
            txtVideoDate.Text = DateTime.Parse(dr["VideoDate"].ToString()).ToString("yyyy/MM/dd");
        }
        //演出者/演唱者	Performer_Singer
        txtPerformer_Singer.Text = dr["Performer_Singer"].ToString();
        //曲目	Tracks
        txtTracks.Text = dr["Tracks"].ToString();
        //敘述	Described
        txtDescribed.Text = dr["Described"].ToString();
        //SN1編號	SN1Number
        txtSN1Number.Text = dr["SN1Number"].ToString();
        //Take
        txtTake.Text = dr["Take"].ToString();
        //作詞	Authors
        txtAuthors.Text = dr["Authors"].ToString();
        //作曲	Compose
        txtCompose.Text = dr["Compose"].ToString();
        //編曲/改編	Arranger_Adaptation
        txtArranger_Adaptation.Text = dr["Arranger_Adaptation"].ToString();
        //審核	Audit
        txtAudit.Text = dr["Audit"].ToString();
        //曲風	MusicalStyles
        lblMusicalStyles.Text = LoadCheckBoxListData("曲風", "cblMusicalStyles", dr["MusicalStyles"].ToString());
        //樂器	MusicalInstruments
        lblMusicalInstruments.Text = LoadCheckBoxListData("樂器", "cblMusicalInstruments", dr["MusicalInstruments"].ToString());
        //語言	Language
        lblLanguage.Text = LoadCheckBoxListData("語言", "cblLanguage", dr["Language"].ToString());
        //標記	ScriptMark
        txtScriptMark.Text = dr["ScriptMark"].ToString();
        //節期	Festivals
        lblFestivals.Text = LoadCheckBoxListData("節期", "cblFestivals", dr["Festivals"].ToString());
        //備註	Remark
        txtRemark.Text = dr["Remark"].ToString().Trim();
        //音樂長度	MusicLength
        txtMusicLengthMin.Text = (Convert.ToInt32(dr["MusicLength"].ToString()) / 60).ToString();
        txtMusicLengthSec.Text = (Convert.ToInt32(dr["MusicLength"].ToString()) % 60).ToString();
        //速度	Speed
        txtSpeed.Text = dr["Speed"].ToString().Trim();
        /*
        int speed_no = Convert.ToInt32(txtSpeed.Text);
        switch (speed_no)
        {
            case 1:
                Speed1.Text = "❶";
                break;
            case 2:
                Speed2.Text = "❷";
                break;
            case 3:
                Speed3.Text = "❸";
                break;
            case 4:
                Speed4.Text = "❹";
                break;
            case 5:
                Speed5.Text = "❺";
                break;

        }
        */
        //評比	Rating
        txtRating.Text = dr["Rating"].ToString().Trim();
        /*
        int img_no = Convert.ToInt32(txtRating.Text);
        switch (img_no)
        {
            case 1:
                Rating1.ImageUrl = "~/images/star_tick.png";
                break;
            case 2:
                Rating1.ImageUrl = "~/images/star_tick.png";
                Rating2.ImageUrl = "~/images/star_tick.png";
                break;
            case 3:
                Rating1.ImageUrl = "~/images/star_tick.png";
                Rating2.ImageUrl = "~/images/star_tick.png";
                Rating3.ImageUrl = "~/images/star_tick.png";
                break;
            case 4:
                Rating1.ImageUrl = "~/images/star_tick.png";
                Rating2.ImageUrl = "~/images/star_tick.png";
                Rating3.ImageUrl = "~/images/star_tick.png";
                Rating4.ImageUrl = "~/images/star_tick.png";
                break;
            case 5:
                Rating1.ImageUrl = "~/images/star_tick.png";
                Rating2.ImageUrl = "~/images/star_tick.png";
                Rating3.ImageUrl = "~/images/star_tick.png";
                Rating4.ImageUrl = "~/images/star_tick.png";
                Rating5.ImageUrl = "~/images/star_tick.png";
                break;

        }
        */
    }

    //----------------------------------------------------------------------
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        try
        {

            string strSql = @"UPDATE [dbo].[MusicSingles]
                                SET  [Category]            = @Category
                                    ,[Item]                = @Item
                                    ,[VideoDate]           = @VideoDate
                                    ,[Performer_Singer]    = @Performer_Singer
                                    ,[Tracks]              = @Tracks
                                    ,[MusicLength]         = @MusicLength
                                    ,[Described]           = @Described
                                    ,[SN1Number]           = @SN1Number
                                    ,[Take]                = @Take
                                    ,[Authors]             = @Authors
                                    ,[Compose]             = @Compose
                                    ,[Arranger_Adaptation] = @Arranger_Adaptation
                                    ,[Audit]               = @Audit
                                    ,[MusicalStyles]       = @MusicalStyles
                                    ,[MusicalInstruments]  = @MusicalInstruments
                                    ,[Language]            = @Language
                                    ,[ScriptMark]          = @ScriptMark
                                    ,[Speed]               = @Speed
                                    ,[Rating]              = @Rating
                                    ,[Festivals]           = @Festivals
                                    ,[Remark]              = @Remark
                                    ,[ModifyUser]          = @ModifyUser
                                    ,[ModifyDatetime]      = @ModifyDatetime
                            WHERE [UniqueNumber] = @UniqueNumber
                             ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("UniqueNumber", HFD_Uid.Value);
            dict.Add("Category", ddlCategory.SelectedValue);
            dict.Add("Item", ddlItem.SelectedValue);
            if (String.IsNullOrEmpty(txtVideoDate.Text.Trim()))
            {
                dict.Add("VideoDate", null);
            }
            else
            {
                dict.Add("VideoDate", Convert.ToDateTime(txtVideoDate.Text));
            }
            dict.Add("Performer_Singer", txtPerformer_Singer.Text.Trim());
            dict.Add("Tracks", txtTracks.Text.Trim());
            int intMusicLength = (txtMusicLengthMin.Text.Trim() == "" ? 0 : Convert.ToInt32(txtMusicLengthMin.Text.Trim())) * 60
                + (txtMusicLengthSec.Text.Trim() == "" ? 0 : Convert.ToInt32(txtMusicLengthSec.Text.Trim()));
            dict.Add("MusicLength", intMusicLength);
            dict.Add("Described", txtDescribed.Text.Trim());
            dict.Add("SN1Number", txtSN1Number.Text.Trim());
            dict.Add("Take", txtTake.Text.Trim());
            dict.Add("Authors", txtAuthors.Text.Trim());
            dict.Add("Compose", txtCompose.Text.Trim());
            dict.Add("Arranger_Adaptation", txtArranger_Adaptation.Text.Trim());
            dict.Add("Audit", txtAudit.Text.Trim());
            dict.Add("MusicalStyles", Util.GetControlValue("cblMusicalStyles"));
            dict.Add("MusicalInstruments", Util.GetControlValue("cblMusicalInstruments"));
            dict.Add("Language", Util.GetControlValue("cblLanguage"));
            dict.Add("ScriptMark", txtScriptMark.Text.Trim());
            dict.Add("Festivals", Util.GetControlValue("cblFestivals"));
            dict.Add("Speed", txtSpeed.Text);
            dict.Add("Rating", txtRating.Text);
            dict.Add("Remark", txtRemark.Text.Trim());
            //dict.Add("CreateUser", "");
            //dict.Add("CreateDatetime", DateTime.Now);
            //dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            dict.Add("ModifyUser", "");
            dict.Add("ModifyDatetime", DateTime.Now);
            //dict.Add("ModifyDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            NpoDB.ExecuteSQLS(strSql, dict);
            lblMusicalStyles.Text = LoadCheckBoxListData("曲風", "cblMusicalStyles", Util.GetControlValue("cblMusicalStyles"));
            lblMusicalInstruments.Text = LoadCheckBoxListData("樂器", "cblMusicalInstruments", Util.GetControlValue("cblMusicalInstruments"));
            lblLanguage.Text = LoadCheckBoxListData("語言", "cblLanguage", Util.GetControlValue("cblLanguage"));
            lblFestivals.Text = LoadCheckBoxListData("節期", "cblFestivals", Util.GetControlValue("cblFestivals"));
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>initSpeed();initRating();alert('修改成功！');</Script>");
        }
        catch (Exception ex) {
            //resolt = false
            System.Console.Out.WriteLine(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('修改失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
        }

    }

    //----------------------------------------------------------------------
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
            string strSql = "delete from [dbo].[MusicSingles] where [UniqueNumber] = @UniqueNumber";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("UniqueNumber", HFD_Uid.Value);
            NpoDB.ExecuteSQLS(strSql, dict);
            //ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('刪除成功！');location.href=('MusicSinglesQuery.aspx');</Script>");
            Response.Write("<Script language='JavaScript'>alert('刪除成功！');location.href=('MusicSinglesQuery.aspx');</Script>");
        }
        catch (Exception ex)
        {
            //resolt = false
            System.Console.Out.WriteLine(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('刪除失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
        }
    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("MusicSinglesQuery.aspx");
    }

}
