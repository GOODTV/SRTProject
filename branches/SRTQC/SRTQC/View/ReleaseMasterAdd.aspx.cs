using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseMasterAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string strSql = "";

        DataTable dt = null;
        Dictionary<string, object> dict_check = new Dictionary<string, object>();
        strSql = "select * from [dbo].[ReleaseMaster] where customerID='" + txtCustomerID.Text.Trim() + "' ";

        dt = NpoDB.GetDataTableS(strSql, dict_check);
        if (dt.Rows.Count != 0)
        {
            this.Page.RegisterStartupScript("s", "<script>alert('已有相同的客戶代號！');</script>");
            return;
        }
        try
        {
            ReleaseMaster_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('交檔客戶新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect(Util.RedirectByTime("ReleaseMasterQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseMasterQuery.aspx');</Script>");
            Response.End();
        }
    }
    public void ReleaseMaster_AddNew()
    {
        string strSql = @"INSERT INTO dbo.ReleaseMaster
                           (customerID,customerName,name_1,phone_1,mobile_1,email_1,name_2,phone_2
                            ,mobile_2,email_2,interval,remark,CreateUser,CreateDatetime)
                            VALUES (@customerID,@customerName,@name_1,@phone_1,@mobile_1,@email_1,@name_2,@phone_2
                            ,@mobile_2,@email_2,@interval,@remark,@CreateUser,@CreateDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("customerID", txtCustomerID.Text.Trim());
        dict.Add("customerName", txtCustomerName.Text.Trim());
        dict.Add("name_1", txtName_1.Text.Trim());
        dict.Add("phone_1", txtPhone_1.Text.Trim());
        dict.Add("mobile_1", txtMobile_1.Text.Trim());
        dict.Add("email_1", txtEmail_1.Text.Trim());
        dict.Add("name_2", txtName_2.Text.Trim());
        dict.Add("phone_2", txtPhone_2.Text.Trim());
        dict.Add("mobile_2", txtMobile_2.Text.Trim());
        dict.Add("email_2", txtEmail_2.Text.Trim());
        if (rblInterval.SelectedValue == "1")
        {
            dict.Add("interval", "月");
        }
        else if (rblInterval.SelectedValue == "2")
        {
            dict.Add("interval", "季");
        }
        else
        {
            dict.Add("interval", "年");
        }

        dict.Add("remark", txtRemark.Text.Trim());
        dict.Add("CreateUser", "media");
        dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseMasterQuery.aspx"));
    }
}