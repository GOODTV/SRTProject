using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseProgramLogEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("SerNo");
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
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
        strSql = @" select * From ReleaseProgramLog
                    where SerNo='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ReleaseProgramLogQuery.aspx");

        DataRow dr = dt.Rows[0];

        //節目代號
        txtProgramID.Text = dr["programID"].ToString().Trim();
        //節目名稱
        lblProgramName.Text = ProgramName();
        lblProgramName.Font.Size = 11;
        lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        lblProgramName.Font.Bold = true;
        //集數
        txtEpisode.Text = dr["episode"].ToString().Trim();
        //客戶代號
        txtCustomerID.Text = dr["customerID"].ToString().Trim();
        //客戶名稱
        lblCustomerName.Text = CustomerName();
        lblCustomerName.Font.Size = 11;
        lblCustomerName.ForeColor = System.Drawing.Color.Blue; ;
        lblCustomerName.Font.Bold = true;
        //供片時間
        txtSupplyDate.Text = DateTime.Parse(dr["supplyDate"].ToString()).ToString("yyyy/MM/dd");
        //檔案名稱
        txtFilename.Text = dr["filename"].ToString().Trim();
        //Logo
        if (dr["logo"].ToString().Trim() == "1")
        {
            cbxLogo.Checked = true;
        }
        else
        {
            cbxLogo.Checked = false;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string strSql = "";
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
        try
        {
            ReleaseProgramLog_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('節目交檔記錄修改成功！');</Script>");
            //Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseProgramLogQuery.aspx?Key=" + HFD_Key.Value + "');</Script>");
            Response.End();
        }
    }
    public void ReleaseProgramLog_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update ReleaseProgramLog set ";

        strSql += "  programID = @programID";
        strSql += ", episode = @episode";
        strSql += ", customerID = @customerID";
        strSql += ", supplyDate = @supplyDate";
        strSql += ", filename = @filename";
        strSql += ", logo = @logo";

        strSql += ", ModifyUser= @ModifyUser";
        strSql += ", ModifyDatetime = @ModifyDatetime";
        strSql += " where SerNo = @SerNo";

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

        dict.Add("ModifyUser", "media");
        dict.Add("ModifyDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("SerNo", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from ReleaseProgramLog where SerNo=@SerNo";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SerNo", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        Response.Write("<Script language='JavaScript'>alert('節目交檔記錄刪除成功！');</Script>");
        //Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx"));
        Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseProgramLogQuery.aspx?Key="+ HFD_Key.Value +"');</Script>");
        Response.End();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx", "Key=" + HFD_Key.Value));
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
    private string CustomerName()
    {
        string strSql = @"select customerName  from [dbo].[ReleaseMaster] WHERE customerID = '" + txtCustomerID.Text + "'";
        string CustomerName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            CustomerName = dr["customerName"].ToString();
        }
        return CustomerName;
    }
}