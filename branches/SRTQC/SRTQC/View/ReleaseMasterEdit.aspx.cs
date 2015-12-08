using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseMasterEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("customerID");
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
        strSql = @" select * From ReleaseMaster
                    where customerID='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ReleaseMasterQuery.aspx");

        DataRow dr = dt.Rows[0];

        //客戶代號
        txtCustomerID.Text = dr["customerID"].ToString().Trim();
        //客戶名稱
        txtCustomerName.Text = dr["customerName"].ToString().Trim();
        //聯絡人姓名1
        txtName_1.Text = dr["name_1"].ToString().Trim();
        //聯絡人電話1
        txtPhone_1.Text = dr["phone_1"].ToString().Trim();
        //聯絡人手機1
        txtMobile_1.Text = dr["mobile_1"].ToString().Trim();
        //聯絡人電郵1
        txtEmail_1.Text = dr["email_1"].ToString().Trim();
        //聯絡人姓名2
        txtName_2.Text = dr["name_2"].ToString();
        //聯絡人電話2
        txtPhone_2.Text = dr["phone_2"].ToString().Trim();
        //聯絡人手機2
        txtMobile_2.Text = dr["mobile_2"].ToString().Trim();
        //聯絡人電郵2
        txtEmail_2.Text = dr["email_2"].ToString().Trim();
        //供片間隔
        if (dr["interval"].ToString().Trim() == "月")
        {
            rblInterval.SelectedValue = "1";
        }
        else if (dr["interval"].ToString().Trim() == "季")
        {
            rblInterval.SelectedValue = "2";
        }
        else 
        {
            rblInterval.SelectedValue = "3";
        }
        //備註
        txtRemark.Text = dr["remark"].ToString().Trim();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            ReleaseMaster_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('交檔客戶修改成功！');</Script>");
            //Response.Redirect(Util.RedirectByTime("ReleaseMasterQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseMasterQuery.aspx');</Script>");
            Response.End();
        }
    }
    public void ReleaseMaster_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update ReleaseMaster set ";

        //strSql += "  customerID = @customerID";
        strSql += "  customerName = @customerName";
        strSql += ", name_1 = @name_1";
        strSql += ", phone_1 = @phone_1";
        strSql += ", mobile_1 = @mobile_1";
        strSql += ", email_1 = @email_1";
        strSql += ", name_2 = @name_2";
        strSql += ", phone_2 = @phone_2";
        strSql += ", mobile_2 = @mobile_2";
        strSql += ", email_2 = @email_2";
        strSql += ", interval = @interval";
        strSql += ", remark = @remark";

        strSql += ", ModifyUser= @ModifyUser";
        strSql += ", ModifyDatetime = @ModifyDatetime";
        strSql += " where customerID = @customerID";

        //dict.Add("customerID", txtCustomerID.Text.Trim());
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

        dict.Add("ModifyUser", "media");
        dict.Add("ModifyDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("customerID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql1 = "update ReleaseMaster set DeleteDatetime =@DeleteDatetime where customerID='" + HFD_Uid.Value + "'";
        Dictionary<string, object> dict1 = new Dictionary<string, object>();
        dict1.Add("DeleteDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql1, dict1);
        /*string strSql2 = "update ReleaseProgramLog set DeleteDatetime =@DeleteDatetime where customerID='" + HFD_Uid.Value + "'";
        Dictionary<string, object> dict2 = new Dictionary<string, object>();
        dict2.Add("DeleteDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql2, dict2);
        string strSql3 = "update ReleaseShortFilmLog set DeleteDatetime =@DeleteDatetime where customerID='" + HFD_Uid.Value + "'";
        Dictionary<string, object> dict3 = new Dictionary<string, object>();
        dict3.Add("DeleteDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql3, dict3);
        string strSql = "delete from ReleaseMaster where customerID='" + HFD_Uid.Value + "'";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        NpoDB.ExecuteSQLS(strSql, dict);*/

        Response.Write("<Script language='JavaScript'>alert('交檔客戶資料刪除成功！');</Script>");
        //Response.Redirect(Util.RedirectByTime("ReleaseMasterQuery.aspx"));
        Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseMasterQuery.aspx');</Script>");
        Response.End();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseMasterQuery.aspx"));
    }
}