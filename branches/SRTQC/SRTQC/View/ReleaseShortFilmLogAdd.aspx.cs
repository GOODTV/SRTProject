using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseShortFilmLogAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            lblShortFilmName.Font.Size = 11;
            lblShortFilmName.ForeColor = System.Drawing.Color.Blue; ;
            lblShortFilmName.Font.Bold = true;
            if (Session["customerID"] != null)
            {
                txtCustomerID.Text = Session["customerID"].ToString();
            }
            if (Session["supplyDate"] != null)
            {
                txtSupplyDate.Text = Session["supplyDate"].ToString();
            }
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        //檢核是否有此客戶
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = "select customerName from ReleaseMaster where customerID='" + txtCustomerID.Text.Trim() + "'";

        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            this.Page.RegisterStartupScript("s", "<script>alert('查無此客戶！');</script>");
            return;
        }
        try
        {
            ReleaseShortFilmLog_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('短片交檔記錄新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseShortFilmLogAdd.aspx?Key=" + HFD_Key.Value + "');</Script>");
            Response.End();
        }
    }
    public void ReleaseShortFilmLog_AddNew()
    {
        string strSql = @"INSERT INTO dbo.ReleaseShortFilmLog
                        (filmID,customerID,supplyDate,filename,CreateUser,CreateDatetime)
                        VALUES (@filmID,@customerID,@supplyDate,@filename,@CreateUser,@CreateDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("filmID", txtFilmID.Text.Trim());
        dict.Add("customerID", txtCustomerID.Text.Trim());
        dict.Add("supplyDate", txtSupplyDate.Text.Trim());
        dict.Add("filename", txtFilename.Text.Trim());

        dict.Add("CreateUser", "media");
        dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        Session["customerID"] = txtCustomerID.Text.Trim();
        Session["supplyDate"] = txtSupplyDate.Text.Trim();
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Session["customerID"] = null;
        Session["supplyDate"] = null;
        Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogQuery.aspx", "Key=" + HFD_Key.Value));
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strSql = @"select [dbo].[getProgramName]('" + txtFilmID.Text + "') as CFTitle";
        String ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["CFTitle"].ToString();
        }
        else
        {
            ProgramName = "查無短片名稱!";
        }
        lblShortFilmName.Text = ProgramName;
        lblShortFilmName.Font.Size = 11;
        lblShortFilmName.ForeColor = System.Drawing.Color.Blue; ;
        lblShortFilmName.Font.Bold = true;
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