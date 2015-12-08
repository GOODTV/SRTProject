using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ProgramEpisodeEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("ProgramID");
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
        strSql = @" select * From ProgramEpisode
                    where ProgramID='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ProgramEpisodeQuery.aspx");

        DataRow dr = dt.Rows[0];

        //節目代號
        txtProgramCode.Text = dr["ProgramCode"].ToString().Trim();
        //節目名稱
        lblProgramName.Text = ProgramName();
        lblProgramName.Font.Size = 11;
        lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        lblProgramName.Font.Bold = true;
        //集數
        txtEpisode.Text = dr["Episode"].ToString().Trim();
        //原節目代號
        txtOriginal_ProgramCode.Text = dr["Original_ProgramCode"].ToString().Trim();
        //原集數
        txtOriginal_Episode.Text = dr["Original_Episode"].ToString().Trim();
        //大系列名稱
        txtMainSeries.Text = dr["MainSeries"].ToString().Trim();
        //系列名稱
        txtSubSeries.Text = dr["SubSeries"].ToString();
        //分集名稱
        txtEpisodeName.Text = dr["EpisodeName"].ToString().Trim();
        //分集大綱(原版)
        txtEpisodeSynopsisOriginal.Text = dr["EpisodeSynopsisOriginal"].ToString().Trim();
        //分集大綱(500字版)
        txtEpisodeSynopsis500.Text = dr["EpisodeSynopsis500"].ToString().Trim();
        //分集大綱(300字版)
        txtEpisodeSynopsis300.Text = dr["EpisodeSynopsis300"].ToString().Trim();
        //分集大綱(120字版)
        txtEpisodeSynopsis120.Text = dr["EpisodeSynopsis120"].ToString().Trim();
        //分集大綱(WEB版)
        txtEpisodeSynopsisWEB.Text = dr["EpisodeSynopsisWEB"].ToString().Trim();
        //主持人/講員
        txtPresenterSpeaker.Text = dr["PresenterSpeaker"].ToString().Trim();
        //來賓姓名
        txtGuestName.Text = dr["GuestName"].ToString();
        //總長度
        txtTotalLength.Text = dr["TotalLength"].ToString().Trim();
        //分段長度
        txtPartLength.Text = dr["PartLength"].ToString().Trim();
        //段數
        int count = dr["PartLength"].ToString().Split('@').Length - 1;
        txtPartNo.Text = count.ToString();
        //首播日期
        if (dr["PremiereDate"].ToString() != "")
        { 
            txtPremiereDate.Text = DateTime.Parse(dr["PremiereDate"].ToString()).ToString("yyyy/MM/dd");
        }
        //版權註記
        if (dr["CopyrightNote"].ToString().Trim() == "1")
        {
            cbxCopyrightNote.Checked = true;
        }
        //不播出註記
        if (dr["NoBroadcastNote"].ToString().Trim() == "1")
        {
            cbxNoBroadcastNote.Checked = true;
        }
        //不重播註記
        if (dr["NoReplayNote"].ToString().Trim() == "1")
        {
            cbxNoReplayNote.Checked = true;
        }
        //不供片註記
        if (dr["NoProvidevideoNote"].ToString().Trim() == "1")
        {
            cbxNoProvidevideoNote.Checked = true;
        }
        //更新鎖定
        if (dr["Lock"].ToString().Trim() == "Y")
        {
            cbxIsLock.Checked = true;
        }
        //提醒記要
        txtReminderNotes.Text = dr["ReminderNotes"].ToString().Trim();
        //製作人重播建議
        txtPackagerReplaySuggest.Text = dr["PackagerReplaySuggest"].ToString().Trim();
        //編審重播建議
        txtEditorReplaySuggest.Text = dr["EditorReplaySuggest"].ToString().Trim();
    }
    private string ProgramName()
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(_TM002,'') <> '' THEN _TM002 
									WHEN ISNULL(_TM004,'') <> '' THEN _TM004
									WHEN ISNULL(_TM003,'') <> '' THEN _TM003 ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE _TM001 = '" + txtProgramCode.Text + "'";
        string ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["ProgramName"].ToString();
        }
        return ProgramName;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        bool check_ok = false;

        try
        {
            check_ok = ProgramEpisode_CheckEdit();
        }
        catch (Exception ex)
        {
            //throw ex;
            this.AlertMessage("分段長度欄位資料有誤！");
        }

        if (check_ok)
        {
            try
            {
                ProgramEpisode_Edit();
                flag = true;
            }
            catch (Exception ex)
            {
                this.AlertMessage("分集資料修改失敗！");
                throw ex;
            }

        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('分集資料修改成功！');</Script>");
            //Response.Redirect(Util.RedirectByTime("ProgramEpisodeQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramEpisodeQuery.aspx');</Script>");
            Response.End();
        }

    }

    public bool ProgramEpisode_CheckEdit()
    {

        String strPart = txtPartLength.Text.Trim();
        if (String.IsNullOrEmpty(strPart))
        {
            txtTotalLength.Text = "";
            return true;
        }
        if (strPart.Substring(strPart.Length - 1, 1) == "@")
        {
            strPart = strPart.Substring(0, strPart.Length - 1);
        }
        String[] sections = strPart.Split('@');
        txtTotalLength.Text = summaryTotalBySections(sections);
        return true;
    }

    public void ProgramEpisode_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update ProgramEpisode set ";

        strSql += "  ProgramCode = @ProgramCode";
        strSql += ", Episode = @Episode";
        strSql += ", Original_ProgramCode = @Original_ProgramCode";
        strSql += ", Original_Episode = @Original_Episode";
        strSql += ", MainSeries = @MainSeries";
        strSql += ", SubSeries = @SubSeries";
        strSql += ", EpisodeName = @EpisodeName";

        strSql += ", EpisodeSynopsisOriginal = @EpisodeSynopsisOriginal";
        strSql += ", EpisodeSynopsis500 = @EpisodeSynopsis500";
        strSql += ", EpisodeSynopsis300 = @EpisodeSynopsis300";
        strSql += ", EpisodeSynopsis120 = @EpisodeSynopsis120";
        strSql += ", EpisodeSynopsisWEB = @EpisodeSynopsisWEB";
        strSql += ", PresenterSpeaker = @PresenterSpeaker";
        strSql += ", GuestName = @GuestName";
        strSql += ", TotalLength = @TotalLength";
        strSql += ", PartLength = @PartLength";
        strSql += ", PremiereDate = @PremiereDate";
        strSql += ", CopyrightNote = @CopyrightNote";
        strSql += ", NoBroadcastNote = @NoBroadcastNote";
        strSql += ", NoReplayNote = @NoReplayNote";
        strSql += ", NoProvidevideoNote = @NoProvidevideoNote";
        strSql += ", Lock = @Lock";
        strSql += ", ReminderNotes = @ReminderNotes";
        strSql += ", PackagerReplaySuggest = @PackagerReplaySuggest";
        strSql += ", EditorReplaySuggest = @EditorReplaySuggest";

        strSql += ", ModifyUser= @ModifyUser";
        strSql += ", ModifyDatetime = @ModifyDatetime";
        strSql += " where ProgramID = @ProgramID";

        dict.Add("ProgramCode", txtProgramCode.Text.Trim());
        dict.Add("Episode", txtEpisode.Text.Trim());
        dict.Add("Original_ProgramCode", txtOriginal_ProgramCode.Text.Trim());
        dict.Add("Original_Episode", txtOriginal_Episode.Text.Trim());
        dict.Add("MainSeries", txtMainSeries.Text.Trim());
        dict.Add("SubSeries", txtSubSeries.Text.Trim());
        dict.Add("EpisodeName", txtEpisodeName.Text.Trim());

        dict.Add("EpisodeSynopsisOriginal", txtEpisodeSynopsisOriginal.Text.Trim());
        dict.Add("EpisodeSynopsis500", txtEpisodeSynopsis500.Text.Trim());
        dict.Add("EpisodeSynopsis300", txtEpisodeSynopsis300.Text.Trim());
        dict.Add("EpisodeSynopsis120", txtEpisodeSynopsis120.Text.Trim());
        dict.Add("EpisodeSynopsisWEB", txtEpisodeSynopsisWEB.Text.Trim());
        dict.Add("PresenterSpeaker", txtPresenterSpeaker.Text.Trim());
        dict.Add("GuestName", txtGuestName.Text.Trim());
        dict.Add("TotalLength", txtTotalLength.Text.Trim());
        dict.Add("PartLength", txtPartLength.Text.Trim());
        dict.Add("PremiereDate", txtPremiereDate.Text.Trim());
        if (cbxCopyrightNote.Checked)
        {
            dict.Add("CopyrightNote", "1");
        }
        else
        {
            dict.Add("CopyrightNote", "0");
        }
        if (cbxNoBroadcastNote.Checked)
        {
            dict.Add("NoBroadcastNote", "1");
        }
        else
        {
            dict.Add("NoBroadcastNote", "0");
        }
        if (cbxNoReplayNote.Checked)
        {
            dict.Add("NoReplayNote", "1");
        }
        else
        {
            dict.Add("NoReplayNote", "0");
        }
        if (cbxNoProvidevideoNote.Checked)
        {
            dict.Add("NoProvidevideoNote", "1");
        }
        else
        {
            dict.Add("NoProvidevideoNote", "0");
        }
        if (cbxIsLock.Checked)
        {
            dict.Add("Lock", "Y");
        }
        else
        {
            dict.Add("Lock", "");
        }
        dict.Add("ReminderNotes", txtReminderNotes.Text.Trim());
        dict.Add("PackagerReplaySuggest", txtPackagerReplaySuggest.Text.Trim());
        dict.Add("EditorReplaySuggest", txtEditorReplaySuggest.Text.Trim());

        dict.Add("ModifyUser", "media");
        dict.Add("ModifyDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("ProgramID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from ProgramEpisode where ProgramID=@ProgramID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ProgramID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        Response.Write("<Script language='JavaScript'>alert('分集資料刪除成功！');</Script>");
        //Response.Redirect(Util.RedirectByTime("ProgramEpisodeQuery.aspx"));
        Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramEpisodeQuery.aspx');</Script>");
        Response.End();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramEpisodeQuery.aspx"));
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(_TM002,'') <> '' THEN _TM002 
									WHEN ISNULL(_TM004,'') <> '' THEN _TM004
									WHEN ISNULL(_TM003,'') <> '' THEN _TM003 ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE _TM001 = '" + txtProgramCode.Text + "'";
        String ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["ProgramName"].ToString();
        }
        else
        {
            ProgramName = "查無節目名稱!";
        }
        lblProgramName.Text = ProgramName;
        lblProgramName.Font.Size = 11;
        lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        lblProgramName.Font.Bold = true;
    }

    public static String summaryTotalBySections(String[] sections)
    {

        if (null == sections || sections.Length == 0)
        {
            return "";
        }

        long totalSeconds = 0;
        String totalTime = "";

        foreach (String section in sections)
        {
            String[] timeStr = section.Split(';');
            String length = timeStr[2];
            long seconds = getSeconds(length);

            totalSeconds += seconds;
        }

        totalTime = getStringFromSeconds(totalSeconds);

        return totalTime;

    }

    private static long getSeconds(String timeStr)
    {

        if (timeStr == "")
        {
            return 0;
        }

        String[] timeFields = timeStr.Split(':');
        String hour = timeFields[0];
        String minute = timeFields[1];
        String second = timeFields[2];

        long seconds = Convert.ToInt64(second) + 60 * Convert.ToInt64(minute) + 3600 * Convert.ToInt64(hour);

        return seconds;

    }

    private static String getStringFromSeconds(long seconds)
    {

        int hour = 0;
        int minute = 0;
        int second = 0;

        while (60 <= seconds)
        {
            if (3600 <= seconds)
            {
                seconds -= (3600);
                hour++;
            }
            else if (60 <= seconds)
            {
                seconds -= (60);
                minute++;
            }
        }

        second = Convert.ToUInt16(seconds);
        String timeStr = String.Format("{0:00}:{1:00}:{2:00}:00", hour, minute, second);

        return timeStr;

    }

}
