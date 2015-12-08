using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class SysMgr_MusicSinglesCodeAdd : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strSql = @"select * from ( select distinct [CodeType] from [dbo].[MusicSinglesCode] union
                select distinct [CodeName] as [CodeType] from [dbo].[MusicSinglesCode] where [CodeType] = '音樂單曲類別'
                union select distinct [CodeName] as [CodeType] from [dbo].[MusicSinglesCodeBase] ) as msc order by [CodeType]  ; ";
            Util.FillDropDownList(ddlCodeType, strSql, "CodeType", "CodeType", true, "", "請選擇");
        }
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        txtCodeName.Text = txtCodeName.Text.Trim();
        if (String.IsNullOrEmpty(ddlCodeType.SelectedValue) || String.IsNullOrEmpty(txtCodeName.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('未輸入資料');</Script>");
            return;
        }
        string strSql = @"select [CodeID] from [dbo].[MusicSinglesCode]
                          where [CodeType] = @CodeType and [CodeName] = @CodeName ; ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CodeType", ddlCodeType.SelectedValue);
        dict.Add("CodeName", txtCodeName.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('已存在！');</Script>");
        }
        else
        {

            try
            {
                strSql = @"insert into  [MusicSinglesCode]  ( [CodeType], [CodeName], [CodeNo]) 
                    values(@CodeType,@CodeName,case when ISNULL((SELECT MAX([CodeNo]) FROM [dbo].[MusicSinglesCode] 
                    where [CodeType] = @CodeType),'') = '' then 1 else (SELECT MAX([CodeNo]) 
                    FROM [dbo].[MusicSinglesCode] where [CodeType] = @CodeType) + 1 end) ; ";
                NpoDB.ExecuteSQLS(strSql, dict);
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('新增成功！');location.href=('MusicSinglesCode.aspx');</Script>");
            }
            catch (Exception ex)
            {
                //resolt = false
                System.Console.Out.WriteLine(ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('新增失敗！\\n失敗原因：" + ex.Message.Replace("\r\n", "\\n") + "');</Script>");
            }

        }

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("MusicSinglesCode.aspx");
    }

}
