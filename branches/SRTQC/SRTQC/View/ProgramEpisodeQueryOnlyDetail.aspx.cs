using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ProgramEpisodeQueryOnlyDetail : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["strSqlWhere"] == null) Response.Redirect("ProgramEpisodeQueryOnly.aspx");

        if (!IsPostBack)
        {
            hfExportExcelVisible.Value = Request.QueryString["ExportExcel"];
            if (hfExportExcelVisible.Value == "0") btnExportExcel.Visible = false;
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
        string strSql2;
        DataTable dtQuery;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select [ProgramID] ,[ProgramCode] ,[Episode] ,[EpisodeName] ,[EpisodeSynopsisOriginal] ,[EpisodeSynopsis500] 
                    ,[EpisodeSynopsis300] ,[EpisodeSynopsis120] ,[EpisodeSynopsisWEB] ,[ReminderNotes] 
                    From ProgramEpisode  where ProgramID = " + uid + " ";
        strSql2 = @" select ROW_NUMBER() OVER(ORDER BY ProgramCode,Episode) AS [NO],ProgramID,ProgramName
            from (select *,[dbo].[getProgramName]([ProgramCode]) as [ProgramName] from dbo.ProgramEpisode) as pe 
            where 1=1 ";
        strSql2 += Session["strSqlWhere"].ToString() + " order by ProgramCode,Episode ";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);
        dtQuery = NpoDB.QueryGetTable(strSql2);

        DataRow[] foundRows = dtQuery.Select("ProgramID = " + uid);
        int uno = Convert.ToInt32(foundRows[0][0]);
        string ProgramName = foundRows[0][2].ToString();
        int dt2_cut = dtQuery.Rows.Count;

        if (uno == 1) btnPrevious.Enabled = false;
        else
        {
            DataRow[] Previous = dtQuery.Select("NO = " + (uno-1));
            HFD_PreviousUid.Value = Previous[0][1].ToString();
        }
        if (uno == dt2_cut) btnNext.Enabled = false;
        else
        {
            DataRow[] NextUid = dtQuery.Select("NO = " + (uno + 1));
            HFD_NextUid.Value = NextUid[0][1].ToString();
        }

        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ProgramEpisodeQueryOnly.aspx");

        DataRow dr = dt.Rows[0];

        //節目代號
        string strProgramCode = dr["ProgramCode"].ToString().Trim();
        //節目名稱
        //lblProgramName.Text = ProgramName;
        //lblProgramName.Font.Size = 11;
        //lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        //lblProgramName.Font.Bold = true;
        //集數
        string strEpisode = dr["Episode"].ToString().Trim();
        //分集名稱
        //txtEpisodeName.Text = dr["EpisodeName"].ToString().Trim();
        //分集大綱(原版)
        string strEpisodeSynopsisOriginal = dr["EpisodeSynopsisOriginal"].ToString().Trim();
        //分集大綱(500字版)
        string strEpisodeSynopsis500 = dr["EpisodeSynopsis500"].ToString().Trim();
        //分集大綱(300字版)
        string strEpisodeSynopsis300 = dr["EpisodeSynopsis300"].ToString().Trim();
        //分集大綱(120字版)
        string strEpisodeSynopsis120 = dr["EpisodeSynopsis120"].ToString().Trim();
        //分集大綱(WEB版)
        string strEpisodeSynopsisWEB = dr["EpisodeSynopsisWEB"].ToString().Trim();
        //提醒記要
        string strReminderNotes = dr["ReminderNotes"].ToString().Trim();

        string ExcelBody = @"
<table width='100%' border='0' align='center' cellpadding='0' cellspacing='1' class='table_v'> 
	<tr>
		<th align='center' nowrap='nowrap' width='10%'><span>節目代號</span></th>
		<td align='center' width='10%'><span>" + strProgramCode+ @"</span></td>
		<th align='center' nowrap='nowrap' width='10%'><span>原版分集大綱</span></th>
		<td style='line-height: 18px;'><span>" + strEpisodeSynopsisOriginal + @"</span></td>
	</tr>
	<tr>
		<th align='center'><span>集數</span></th>
		<td align='center'><span>" + strEpisode + @"</span></td>
		<th align='center' nowrap='nowrap'><span>500字分集大綱</span></th>
		<td style='line-height: 18px;'><span>" + strEpisodeSynopsis500 + @"</span></td>
	</tr>
	<tr>
		<th align='center'><span>名稱</span></th>
		<td align='center'><span>" + ProgramName + @"</span></td>
		<th align='center'><span>300字分集大綱</span></th>
		<td style='line-height: 18px;'><span>" + strEpisodeSynopsis300 + @"</span></td>
	</tr>
	<tr>
		<td colspan='2'><span></span></td>
		<th align='center'><span>120字分集大綱</span></th>
		<td style='line-height: 18px;'><span>" + strEpisodeSynopsis120 + @"</span></td>
	</tr>
	<tr>
		<td colspan='2'><span></span></td>
		<th align='center'><span>WEB分集大綱</span></th>
		<td style='line-height: 18px;'><span>" + strEpisodeSynopsisWEB + @"</span></td>
	</tr>
	<tr>
		<td colspan='2'><span></span></td>
		<th align='center'><span>提醒記要</span></th>
		<td style='line-height: 18px;'><span>" + strReminderNotes + @"</span></td>
	</tr>
</table>
";

        lblGridList.Text = ExcelBody;
    }

    //---------------------------------------------------------------------------

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramEpisodeQueryOnlyDetail.aspx", "ExportExcel=" + hfExportExcelVisible.Value + "&ProgramID=" + HFD_PreviousUid.Value));
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramEpisodeQueryOnlyDetail.aspx", "ExportExcel=" + hfExportExcelVisible.Value + "&ProgramID=" + HFD_NextUid.Value));
    }


    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

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
				
        .table_v {
            border-collapse: collapse;
            empty-cells: show;
        }

        .table_v tr th {
            padding: 2px;
            border: #000 solid 0.5pt;
            background: #FEFE98;
        }

        .table_v tr td {
            padding: 2px;
            border: #000 solid 0.5pt;
        }

</style>
";
        strExcel += "<table width='90%'><tr><td style='text-align: center;'><span id='Title'>分集基本資料明細查詢結果</span></td><tr><td style='text-align: center;'>" + lblGridList.Text + "</td></tr></table>";
        Util.OutputTxt(strExcel, "1", "ProgramEpisodeDetail" + DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

}
