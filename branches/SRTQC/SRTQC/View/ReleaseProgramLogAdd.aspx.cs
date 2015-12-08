using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseProgramLogAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            if (Session["programID"] != null)
            {
                txtProgramID.Text = Session["programID"].ToString();
            }
            if (Session["customerID"] != null)
            {
                txtCustomerID.Text = Session["customerID"].ToString();
            }
            if (Session["supplyDate"] != null)
            {
                txtSupplyDate.Text = Session["supplyDate"].ToString();
            }
            //節目名稱
            lblProgramName.Text = ProgramName();
            lblProgramName.Font.Size = 11;
            lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
            lblProgramName.Font.Bold = true;
        }
    }
    private string ProgramName()
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(_TM002,'') <> '' THEN _TM002 
									WHEN ISNULL(_TM004,'') <> '' THEN _TM004
									WHEN ISNULL(_TM003,'') <> '' THEN _TM003 ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE _TM001 = '" + txtProgramID.Text + "'";
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        //依節目代號及集數檢核分集資料庫中不播出、不重播及不供片標記 2015/1/30增加
        string strSql = "";
        /*DataTable dt = null;
        Dictionary<string, object> dict_check = new Dictionary<string, object>();
        strSql = "select ISNULL(NoBroadcastNote,'') as NoBroadcastNote,ISNULL(NoReplayNote,'') as NoReplayNote,ISNULL(NoProvidevideoNote,'') as NoProvidevideoNote from [dbo].[ProgramEpisode] where ProgramCode='" + txtProgramID.Text.Trim() + "' and Episode='" + txtEpisode.Text.Trim() + "'";

        dt = NpoDB.GetDataTableS(strSql, dict_check);
        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];
            string msg = "";
            if (dr["NoBroadcastNote"].ToString().Trim() == "1")
            {
                msg = " 不播出";
            }
            if (dr["NoReplayNote"].ToString().Trim() == "1")
            {
                msg += " 不重播";
            }
            if (dr["NoProvidevideoNote"].ToString().Trim() == "1")
            {
                msg += " 不供片";
            }
            if (dr["NoBroadcastNote"].ToString().Trim() == "1" || dr["NoReplayNote"].ToString().Trim() == "1" || dr["NoProvidevideoNote"].ToString().Trim() == "1")
            {
                this.Page.RegisterStartupScript("s", "<script>confirm('此集" + msg + "！');</script>");
            }
        }*/
        //檢核是否有此客戶
        DataTable dt2 = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "select customerName from ReleaseMaster where customerID='" + txtCustomerID.Text.Trim() + "'";

        dt2 = NpoDB.GetDataTableS(strSql, dict);
        if (dt2.Rows.Count == 0)
        {
            this.Page.RegisterStartupScript("s", "<script>alert('查無此客戶！');</script>");
            return;
        }
        //檢核同一客戶是否己供過同一節目同一集數 2015/1/30增加
        /*DataTable dt3 = null;
        Dictionary<string, object> dict3 = new Dictionary<string, object>();
        strSql = "select CONVERT(VARCHAR, supplyDate, 111) AS supplyDate from dbo.ReleaseProgramLog where customerID='" + txtCustomerID.Text.Trim() + "' and programID='" + txtProgramID.Text.Trim() + "' and episode='" + txtEpisode.Text.Trim() + "'";

        dt3 = NpoDB.GetDataTableS(strSql, dict3);
        if (dt3.Rows.Count !=0)
        {
            DataRow dr = dt3.Rows[0];
            this.Page.RegisterStartupScript("s", "<script>alert('該節目集數已於" + dr["supplyDate"].ToString() + "供片過！');</script>");
            return;
        }*/
        try
        {
            ReleaseProgramLog_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('節目交檔記錄新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseProgramLogAdd.aspx?Key=" + HFD_Key.Value + "');</Script>");
            Response.End();
        }
    }
    public void ReleaseProgramLog_AddNew()
    {
        string strSql = @"INSERT INTO dbo.ReleaseProgramLog
                        (programID,episode,customerID,supplyDate,filename,logo,CreateUser,CreateDatetime)
                        VALUES (@programID,@episode,@customerID,@supplyDate,@filename,@logo,@CreateUser,@CreateDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("programID", txtProgramID.Text.Trim());
        dict.Add("episode", txtEpisode.Text.Trim());
        dict.Add("customerID", txtCustomerID.Text.Trim());
        dict.Add("supplyDate", txtSupplyDate.Text.Trim());
        dict.Add("filename", txtFilename.Text.Trim());
        if (cbxLogo.Checked == true)
        {
            dict.Add("logo", "1");
        }
        else
        {
            dict.Add("logo", "0");
        }
        dict.Add("CreateUser", "media");
        dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        Session["programID"] = txtProgramID.Text.Trim();
        Session["customerID"] = txtCustomerID.Text.Trim();
        Session["supplyDate"] = txtSupplyDate.Text.Trim();
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Session["programID"] = null;
        Session["customerID"] = null;
        Session["supplyDate"] = null;
        Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx", "Key=" + HFD_Key.Value));
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(_TM002,'') <> '' THEN _TM002 
									WHEN ISNULL(_TM004,'') <> '' THEN _TM004
									WHEN ISNULL(_TM003,'') <> '' THEN _TM003 ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE _TM001 = '" + txtProgramID.Text + "'";
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
    protected void btnQueryCustomer_Click(object sender, EventArgs e)
    {
        string strSql = "select customerName  from [dbo].[ReleaseMaster] WHERE customerID = '" + txtCustomerID.Text + "'";
        String CustomerName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            CustomerName = dr["customerName"].ToString();
        }
        else
        {
            CustomerName = "查無客戶名稱!";
        }
        lblCustomerName.Text = CustomerName;
        lblCustomerName.Font.Size = 11;
        lblCustomerName.ForeColor = System.Drawing.Color.Blue; ;
        lblCustomerName.Font.Bold = true;
    }
}