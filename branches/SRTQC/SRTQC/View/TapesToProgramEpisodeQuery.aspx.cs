using Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_TapesToProgramEpisodeQuery : System.Web.UI.Page
{

    //int m_iRowIdx;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridDataBind();
    }

    protected void btnImprot_Click(object sender, EventArgs e)
    {

        string strProgramCode = txtProgramCode.Text.Trim();
        if (strProgramCode == "") return;
        int intEpisode = txtEpisode.Text.Trim() == "" ? 0 : Convert.ToInt32(txtEpisode.Text.Trim());
        int dbCount = 0;

        for (int i = 0; i < this.gridEPGList.Rows.Count; i++)
        {
            if (((CheckBox)gridEPGList.Rows[i].FindControl("cbDel")).Checked)
            {

                Label VideoTapeBarcode = (Label)gridEPGList.Rows[i].FindControl("VideoTapeBarcode");
                Label Original_Episode = (Label)gridEPGList.Rows[i].FindControl("Original_Episode");

                DBProvider dbProvider = new DBProvider();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sp_VideoTapeBarcode = new SqlParameter("@_TM02P1_TM020", SqlDbType.NVarChar);
                sp_VideoTapeBarcode.Value = VideoTapeBarcode.Text;
                cmd.Parameters.Add(sp_VideoTapeBarcode);
                SqlParameter sp_Original_Episode = new SqlParameter("@_TM02P1_TM030", SqlDbType.Int);
                sp_Original_Episode.Value = Convert.ToInt32(Original_Episode.Text);
                cmd.Parameters.Add(sp_Original_Episode);
                SqlParameter sp_ProgramCode = new SqlParameter("@PE_ProgramCode", SqlDbType.NVarChar);
                sp_ProgramCode.Value = txtProgramCode.Text.Trim();
                cmd.Parameters.Add(sp_ProgramCode);
                SqlParameter sp_Episode = new SqlParameter("@PE_Episode", SqlDbType.Int);
                sp_Episode.Value = (intEpisode == 0 ? intEpisode : intEpisode++);
                cmd.Parameters.Add(sp_Episode);

                SqlParameter retID = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                retID.Direction = ParameterDirection.ReturnValue;

                try
                {
                    dbProvider.ExecuteNonQuery("TapesToProgramEpisode", cmd);
                    dbCount += (Convert.ToInt32(retID.Value) + 1);
                }
                catch (Exception ex)
                {
                    System.Console.Out.WriteLine(ex.Message.ToString());
                    throw ex.GetBaseException();
                }

            }
        }

        if (dbCount > 0)
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "checkClear();alert('已完成 " + dbCount + " 筆匯入或更新！');", true);
        else
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('未完成匯入！');", true);


    }

    private void GridDataBind()
    {

        //原節目代號
        string strOriginal_ProgramCode = this.txtOriginal_ProgramCode.Text.Trim();
        //原集數
        string strOriginal_Episode1 = this.txtOriginal_Episode1.Text.Trim();
        string strOriginal_Episode2 = this.txtOriginal_Episode2.Text.Trim();
        //影帶條碼
        string strVideoTapeBarcode = this.txtVideoTapeBarcode.Text.Trim();
        //系列名稱
        //分集名稱
        //分集大綱
        //主持人/講員
        //來賓姓名
        //總長度
        //分段長度
        //首播日期
        //提醒記要

        // Grid顯示待辦維護清單

        DBProvider dbProvider = new DBProvider();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        string strSQL = @"
            SELECT a.*,b.[_TM003] as ProgramName,cast(0 as bit) as CheckFlag,e.TapesClassName
                ,ROW_NUMBER() OVER(ORDER BY Original_ProgramCode,Original_Episode,VideoTapeBarcode) as ROWNO
            FROM (
            SELECT distinct
                   [_TM020] as VideoTapeBarcode
	              ,[_TM001] as Original_ProgramCode
                  ,[_TM030] as Original_Episode
                  ,[_TM031] as SubSeries
                  ,[_TM032] as EpisodeName
                  ,[_TM033] as PremiereDate
                  ,[_TM036] as TotalLength
                  ,RTRIM(REPLACE(REPLACE([_TM037],';','; '),'@','@ ')) as PartLength
                  ,[_TM038] as EpisodeSynopsisOriginal
                  ,[_TM039] as PresenterSpeaker
                  ,[_TM040] as GuestName
                  ,[_TM041] as ReminderNotes
              FROM [_TM02P1] as a1 ) as a
              LEFT JOIN [_TM01P0] as b
              on a.Original_ProgramCode = b.[_TM001]
              LEFT JOIN [_TM02P0] as c
              on a.VideoTapeBarcode = c.[_TM020]
			  LEFT JOIN [TapesClass] as e 
			  ON c.[_TM021] = e.TapesClassCode
            ";
        string strSQLWhere = "";
        if (!string.IsNullOrEmpty(strOriginal_ProgramCode) && strOriginal_ProgramCode != "")
            strSQLWhere += (strSQLWhere == "" ? " where " : " and ") + "isnull(Original_ProgramCode,'') = '" + strOriginal_ProgramCode + "' ";
        if (!string.IsNullOrEmpty(strOriginal_Episode1) && strOriginal_Episode1 != "")
            strSQLWhere += (strSQLWhere == "" ? " where " : " and ") + "Original_Episode >= " + strOriginal_Episode1 + " ";
        if (!string.IsNullOrEmpty(strOriginal_Episode2) && strOriginal_Episode2 != "")
            strSQLWhere += (strSQLWhere == "" ? " where " : " and ") + "Original_Episode <= " + strOriginal_Episode2 + " ";
        if (!string.IsNullOrEmpty(strVideoTapeBarcode) && strVideoTapeBarcode != "")
            strSQLWhere += (strSQLWhere == "" ? " where " : " and ") + "VideoTapeBarcode like '" + strVideoTapeBarcode + "%' ";

        DataTable dt = null;

        try
        {
            dt = dbProvider.OpenDataTable(strSQL + strSQLWhere, cmd);
        }
        catch (Exception ex)
        {
            System.Console.Out.WriteLine(ex.Message.ToString());
        }

        this.InfoPanel.Visible = true;
        if (dt.Rows.Count > 0)
        {
            this.lblGridCount.Text = "共 " + dt.Rows.Count + " 筆";
            //btnImprot.Visible = true;
            divInput.Visible = true;
        }
        else
        {
            this.lblGridCount.Text = "共 0 筆";
            //btnImprot.Visible = false;
            divInput.Visible = false;
        }

        this.gridEPGList.DataSource = dt;
        this.gridEPGList.DataBind();

    }

    protected void gridEPGList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //光棒效果:滑鼠經過變色
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("OnMouseover", "this.style.backgroundColor='#009999';this.style.color='White'");
            if (e.Row.RowIndex % 2 == 0)
            {
                //單數列
                e.Row.Attributes.Add("OnMouseout", "this.style.backgroundColor='#EEEEEE';this.style.color='Black'");
            }
            else
            {
                //雙數列
                e.Row.Attributes.Add("OnMouseout", "this.style.backgroundColor='#DCDCDC';this.style.color='Black'");
            }
        }            

    }

    protected void gridEPGList_PageIndexChanged(object sender, EventArgs e)
    {
        this.GridDataBind();
    }

    protected void gridEPGList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.gridEPGList.PageIndex = e.NewPageIndex;
    }

}
