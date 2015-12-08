using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_MusicSinglesAdd : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            loadDropDownList(ddlCategory, "音樂單曲類別");
            //loadCheckBoxList(cblMusicalStyles, "曲風");
            ddlItem.Enabled = false;
            lblMusicalStyles.Text = LoadCheckBoxListData("曲風", "cblMusicalStyles", "");
            lblMusicalInstruments.Text = LoadCheckBoxListData("樂器", "cblMusicalInstruments", "");
            lblLanguage.Text = LoadCheckBoxListData("語言", "cblLanguage", "");
            lblFestivals.Text = LoadCheckBoxListData("節期", "cblFestivals", "");
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

        if (String.IsNullOrEmpty(ddlCategory.SelectedValue))
        {
            ddlItem.SelectedValue = "";
            ddlItem.Enabled = false;
        }
        else
        {
            ddlItem.Enabled = true;
            loadDropDownList(ddlItem, ddlCategory.SelectedValue);
        }

    }


    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        try
        {

            string strSql = @"INSERT INTO [dbo].[MusicSingles] 
            ([Category] ,[Item] ,[VideoDate] ,[Performer_Singer] ,[Tracks] ,[MusicLength] ,[Described] ,[SN1Number] ,[Take] 
            ,[Authors] ,[Compose] ,[Arranger_Adaptation] ,[Audit] ,[MusicalStyles] ,[MusicalInstruments] ,[Language] ,[ScriptMark] 
            ,[Speed] ,[Rating] ,[Festivals] ,[Remark] ,[CreateUser] ,[CreateDatetime]) 
             VALUES (@Category ,@Item ,@VideoDate ,@Performer_Singer ,@Tracks ,@MusicLength ,@Described ,@SN1Number ,@Take 
            ,@Authors ,@Compose ,@Arranger_Adaptation ,@Audit ,@MusicalStyles ,@MusicalInstruments ,@Language ,@ScriptMark 
            ,@Speed ,@Rating ,@Festivals ,@Remark ,@CreateUser ,@CreateDatetime) ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
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
            dict.Add("CreateUser", "");
            dict.Add("CreateDatetime", DateTime.Now);
            NpoDB.ExecuteSQLS(strSql, dict);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('新增成功！');location.href=('MusicSinglesQuery.aspx');</Script>");
        }
        catch (Exception ex)
        {
            //resolt = false
            System.Console.Out.WriteLine(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('新增失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
        }

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("MusicSinglesQuery.aspx");
    }

}

