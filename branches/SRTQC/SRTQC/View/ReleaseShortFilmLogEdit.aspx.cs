using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseShortFilmLogEdit : BasePage
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
        strSql = @" select * From ReleaseShortFilmLog
                    where SerNo='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ReleaseShortFilmLogQuery.aspx");

        DataRow dr = dt.Rows[0];

        //短片代號
        txtFilmID.Text = dr["filmID"].ToString().Trim();
        //短片名稱
        lblShortFilmName.Text = ShortFilmName();
        lblShortFilmName.Font.Size = 11;
        lblShortFilmName.ForeColor = System.Drawing.Color.Blue; ;
        lblShortFilmName.Font.Bold = true;
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
    }
    private string ShortFilmName()
    {
        string strSql = @"select [dbo].[getProgramName]('" + txtFilmID.Text + "') as CFTitle";
        string ShortFilmName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ShortFilmName = dr["CFTitle"].ToString();
        }
        return ShortFilmName;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
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
            ReleaseShortFilmLog_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('短片交檔記錄修改成功！');</Script>");
            //Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseShortFilmLogQuery.aspx?Key=" + HFD_Key.Value + "');</Script>");
            Response.End();
        }
    }
    public void ReleaseShortFilmLog_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update ReleaseShortFilmLog set ";

        strSql += "  filmID = @filmID";
        strSql += ", customerID = @customerID";
        strSql += ", supplyDate = @supplyDate";
        strSql += ", filename = @filename";

        strSql += ", ModifyUser= @ModifyUser";
        strSql += ", ModifyDatetime = @ModifyDatetime";
        strSql += " where SerNo = @SerNo";

        dict.Add("filmID", txtFilmID.Text.Trim());
        dict.Add("customerID", txtCustomerID.Text.Trim());
        dict.Add("supplyDate", txtSupplyDate.Text.Trim());
        dict.Add("filename", txtFilename.Text.Trim());

        dict.Add("ModifyUser", "media");
        dict.Add("ModifyDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("SerNo", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from ReleaseShortFilmLog where SerNo=@SerNo";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SerNo", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        Response.Write("<Script language='JavaScript'>alert('短片交檔記錄刪除成功！');</Script>");
        //Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogQuery.aspx"));
        Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseShortFilmLogQuery.aspx?Key=" + HFD_Key.Value + "');</Script>");
        Response.End();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogQuery.aspx", "Key=" + HFD_Key.Value));
    }
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