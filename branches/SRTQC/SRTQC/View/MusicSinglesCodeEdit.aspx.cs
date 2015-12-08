using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_MusicSinglesCodeEdit : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            HFD_id.Value = Util.GetQueryString("id");
            string strSql = @"select distinct * from ( select distinct [CodeType] from [dbo].[MusicSinglesCode] union
                select distinct [CodeName] as [CodeType] from [dbo].[MusicSinglesCode] where [CodeType] = '音樂單曲類別'
                union select distinct [CodeName] as [CodeType] from [dbo].[MusicSinglesCodeBase] ) as msc order by [CodeType]  ; ";
            Util.FillDropDownList(ddlCodeType, strSql, "CodeType", "CodeType", false);
            LoadFormData();
        }
    }

    //----------------------------------------------------------------------
    public void LoadFormData()
    {

        string strSql = " select * from [MusicSinglesCode] where [CodeID] = @id ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("id", HFD_id.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count <= 0)
        {
            Response.Redirect("MusicSinglesCode.aspx");
        }

        DataRow dr = dt.Rows[0];

        //類別名稱
        ddlCodeType.SelectedValue = dr["CodeType"].ToString();
        //排序
        txtCodeNo.Text = dr["CodeNo"].ToString();
        //項目名稱
        txtCodeName.Text = dr["CodeName"].ToString();

    }

    //----------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        txtCodeNo.Text = txtCodeNo.Text.Trim();
        txtCodeName.Text = txtCodeName.Text.Trim();
        if (String.IsNullOrEmpty(txtCodeNo.Text) || String.IsNullOrEmpty(txtCodeName.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('未輸入資料');</Script>");
            return;
        }
        
        try
        {
            string strSql = " update [MusicSinglesCode] set ";
            strSql += "  [CodeType] = @CodeType ";
            strSql += ", [CodeNo] = @CodeNo ";
            strSql += ", [CodeName] = @CodeName ";
            strSql += " where [CodeID] = @CodeID ";

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("CodeID", HFD_id.Value);
            dict.Add("CodeType", ddlCodeType.SelectedValue);
            dict.Add("CodeNo", txtCodeNo.Text.Trim());
            dict.Add("CodeName", txtCodeName.Text.Trim());

            NpoDB.ExecuteSQLS(strSql, dict);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('修改成功！');</Script>");
        }
        catch (Exception ex)
        {
            System.Console.Out.WriteLine(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('修改失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
        }

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("MusicSinglesCode.aspx");
    }

    //----------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string strSql = "delete from  [MusicSinglesCode] where [CodeID] = @id ";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("id", HFD_id.Value);
            NpoDB.ExecuteSQLS(strSql, dict);
            Response.Write("<Script language='JavaScript'>alert('刪除成功！');location.href=('MusicSinglesCode.aspx');</Script>");
        }
        catch (Exception ex)
        {
            //resolt = false
            System.Console.Out.WriteLine(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('刪除失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
        }

    }

}
