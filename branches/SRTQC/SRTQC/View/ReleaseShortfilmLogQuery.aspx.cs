using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class View_ReleaseShortfilmLogQuery : BasePage
{
    string DateS = "";
    string DateE = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            //取得DropDownList資料
            string Sql1,Sql2;
            Sql1 = "select distinct Year(supplyDate) as Year from [dbo].[ReleaseShortFilmLog] order by Year";
            SqlDataAdapter da1 = new SqlDataAdapter(Sql1, NpoDB.GetSqlConnection());

            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "ReleaseShortFilmLog");     //第二、執行SQL指令，取出資料
            ddlYearS.DataValueField = "Year";     //在此輸入的是資料表的欄位名稱
            ddlYearS.DataTextField = "Year";      //在此輸入的是資料表的欄位名稱
            ddlYearS.DataSource = ds1.Tables["ReleaseShortFilmLog"].DefaultView;
            ddlYearS.DataBind();
            Sql2 = "select distinct Year(supplyDate) as Year from [dbo].[ReleaseShortFilmLog] order by Year desc";
            SqlDataAdapter da2 = new SqlDataAdapter(Sql2, NpoDB.GetSqlConnection());
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "ReleaseShortFilmLog");     //第二、執行SQL指令，取出資料
            ddlYearE.DataValueField = "Year";     //在此輸入的是資料表的欄位名稱
            ddlYearE.DataTextField = "Year";      //在此輸入的是資料表的欄位名稱
            ddlYearE.DataSource = ds2.Tables["ReleaseShortFilmLog"].DefaultView;
            ddlYearE.DataBind();
            //---------DropDownList End-------------
            string[] strArray;
            strArray = HFD_Key.Value.Split(';');
            if (strArray.Length > 1)
            {
                //customerID
                if (strArray[0] != "")
                {
                    tbxCustomerID.Text = strArray[0];
                }
                //FilmID
                if (strArray[1] != "")
                {
                    tbxFilmID.Text = strArray[1];
                }
                //EpisodeNo_Start
                if (strArray[2] != "")
                {
                    tbxEpisodeNo_Start.Text = strArray[2];
                }
                //EpisodeNo_End
                if (strArray[3] != "")
                {
                    tbxEpisodeNo_End.Text = strArray[3];
                }
                //YearS
                if (strArray[4] != "")
                {
                    ddlYearS.Text = strArray[4];
                }
                //MonthS
                if (strArray[5] != "")
                {
                    ddlMonthS.Text = strArray[5];
                }
                //YearE
                if (strArray[6] != "")
                {
                    ddlYearE.Text = strArray[6];
                }
                //MonthE
                if (strArray[7] != "")
                {
                    ddlMonthE.Text = strArray[7];
                }
            }
            if (tbxCustomerID.Text != "")
            {
                LoadFormData1();
            }
            if (tbxFilmID.Text.Trim() != "")
            {
                LoadFormData2();
            }
            if (tbxCustomerID.Text.Trim() != "" && tbxFilmID.Text.Trim() != "")
            {
                LoadFormData2();
            }
        }
    }
    //客戶代號　＋　供片年月區間
    public void LoadFormData1()
    {
        string strSql;
        DataTable dt;
        strSql = @"select R.SerNo,R.customerID as '客戶代號' , M.customerName as '客戶名稱' ,CONVERT(VarChar,[supplyDate],111) as '供片年月'
                    , R.filmID as '短片代號', [dbo].[getProgramName](R.filmID) as '短片名稱', CF._CF024 as '集數', filename as '檔名'
                    from dbo.ReleaseShortFilmLog R
                    left join [dbo].[_CF02P0] CF on CF._CF010=R.filmID
                    left join [dbo].[ReleaseMaster] M on M.customerID=R.customerID
                    where R.DeleteDatetime is Null ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCustomerID.Text.Trim() != "")
        {
            strSql += " and R.customerID like N'%" + tbxCustomerID.Text.Trim() + "%'";
        }
        if (tbxFilmID.Text.Trim() != "")
        {
            strSql += " and R.filmID like N'%" + tbxFilmID.Text.Trim() + "%'";
        }
        if (tbxEpisodeNo_Start.Text.Trim() != "")
        {
            strSql += " and CF._CF024 >= " + tbxEpisodeNo_Start.Text.Trim() + " ";
        }
        if (tbxEpisodeNo_End.Text.Trim() != "")
        {
            strSql += " and CF._CF024 <= " + tbxEpisodeNo_End.Text.Trim() + " ";
        }
        if (ddlYearS.SelectedValue != "")
        {
            strSql += " and DATEPART(Year,supplyDate) >= '" + ddlYearS.SelectedValue + "'";
        }
        if (ddlYearS.SelectedValue != "" && ddlMonthS.SelectedValue != "")
        {
            DateS = ddlYearS.SelectedValue + "/" + ddlMonthS.SelectedValue + "/1";
            strSql += " and CONVERT(varchar(10) , supplyDate, 111) >= Cast('" + DateS + "' as date)";
        }
        if (ddlYearE.SelectedValue != "")
        {
            strSql += " and DATEPART(Year,supplyDate) <= '" + ddlYearE.SelectedValue + "'";
        }
        if (ddlYearE.SelectedValue != "" && ddlMonthE.SelectedValue != "")
        {
            int days = DateTime.DaysInMonth(Convert.ToInt32(ddlYearE.SelectedValue), Convert.ToInt32(ddlMonthE.SelectedValue));
            DateE = ddlYearE.SelectedValue + "/" + ddlMonthE.SelectedValue + "/" + Convert.ToString(days);
            strSql += " and CONVERT(varchar(10) , supplyDate, 111) <= Cast('" + DateE + "' as date) ";
        }
        strSql += " order by R.customerID,R.filmID,cast(CF._CF024 as integer),R.SerNo ";
        HFD_Key.Value = tbxCustomerID.Text + ";" + tbxFilmID.Text + ";" + tbxEpisodeNo_Start.Text + ";" + tbxEpisodeNo_End.Text + ";" + ddlYearS.SelectedValue + ";" + ddlMonthS.SelectedValue + ";" + ddlYearE.SelectedValue + ";" + ddlMonthE.SelectedValue;
        dt = NpoDB.GetDataTableS(strSql, dict);
        GridView1.CssClass = "table_h";
        GridView1.DataSource = dt;
        GridView1.DataBind();

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            /*
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("SerNo");
            npoGridView.DisableColumn.Add("SerNo");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ReleaseShortFilmLogEdit.aspx", "SerNo=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
            lblGridList.Text = "";
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }
    //短片代號　＋　供片年月區間
    public void LoadFormData2()
    {
        string strSql;
        DataTable dt;
        strSql = @"select R.SerNo,R.filmID as '短片代號', [dbo].[getProgramName](R.filmID) as '短片名稱', CF._CF024 as '集數', R.customerID as '客戶代號' , M.customerName as '客戶名稱' 
                    ,CONVERT(VarChar,[supplyDate],111) as '供片年月' ,filename as '檔名' 
                    from dbo.ReleaseShortFilmLog R
                    left join [dbo].[_CF02P0] CF on CF._CF010=R.filmID
                    left join [dbo].[ReleaseMaster] M on M.customerID=R.customerID
                    where R.DeleteDatetime is Null ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCustomerID.Text.Trim() != "")
        {
            strSql += " and R.customerID like N'%" + tbxCustomerID.Text.Trim() + "%'";
        }
        if (tbxFilmID.Text.Trim() != "")
        {
            strSql += " and R.filmID like N'%" + tbxFilmID.Text.Trim() + "%'";
        }
        if (tbxEpisodeNo_Start.Text.Trim() != "")
        {
            strSql += " and CF._CF024 >= " + tbxEpisodeNo_Start.Text.Trim() + " ";
        }
        if (tbxEpisodeNo_End.Text.Trim() != "")
        {
            strSql += " and CF._CF024 <= " + tbxEpisodeNo_End.Text.Trim() + " ";
        }
        if (ddlYearS.SelectedValue != "")
        {
            strSql += " and DATEPART(Year,supplyDate) >= '" + ddlYearS.SelectedValue + "'";
        }
        if (ddlYearS.SelectedValue != "" && ddlMonthS.SelectedValue != "")
        {
            DateS = ddlYearS.SelectedValue + "/" + ddlMonthS.SelectedValue + "/1";
            strSql += " and CONVERT(varchar(10) , supplyDate, 111) >= Cast('" + DateS + "' as date)";
        }
        if (ddlYearE.SelectedValue != "")
        {
            strSql += " and DATEPART(Year,supplyDate) <= '" + ddlYearE.SelectedValue + "'";
        }
        if (ddlYearE.SelectedValue != "" && ddlMonthE.SelectedValue != "")
        {
            int days = DateTime.DaysInMonth(Convert.ToInt32(ddlYearE.SelectedValue), Convert.ToInt32(ddlMonthE.SelectedValue));
            DateE = ddlYearE.SelectedValue + "/" + ddlMonthE.SelectedValue + "/" + Convert.ToString(days);
            strSql += " and CONVERT(varchar(10) , supplyDate, 111) <= Cast('" + DateE + "' as date) ";
        }
        strSql += " order by R.filmID,cast(CF._CF024 as integer),R.customerID,R.SerNo ";
        HFD_Key.Value = tbxCustomerID.Text + ";" + tbxFilmID.Text + ";" + tbxEpisodeNo_Start.Text + ";" + tbxEpisodeNo_End.Text + ";" + ddlYearS.SelectedValue + ";" + ddlMonthS.SelectedValue + ";" + ddlYearE.SelectedValue + ";" + ddlMonthE.SelectedValue;
        dt = NpoDB.GetDataTableS(strSql, dict);
        GridView1.CssClass = "table_h";
        GridView1.DataSource = dt;
        GridView1.DataBind();

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            /*
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("SerNo");
            npoGridView.DisableColumn.Add("SerNo");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ReleaseShortFilmLogEdit.aspx", "SerNo=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
            lblGridList.Text = "";
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (ddlYearS.SelectedItem == null && ddlYearE.SelectedItem == null)
        {
            Session["strSql"] = "";
            Response.Write("<Script language='JavaScript'>alert('目前尚無資料，請先新增資料！');</Script>");
            return;
        }
        if (tbxFilmID.Text.Trim() != "")
        {
            LoadFormData2();
        }
        else
        {
            LoadFormData1();
        }
    }
    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLogAdd.aspx", "Key=" + HFD_Key.Value));
    }
    public void btnToxls_Click(object sender, EventArgs e)
    {
        if (Session["strSql"] != "")
        {
            Response.Redirect(Util.RedirectByTime("ReleaseShortFilmLog_Print_Excel.aspx"));
        }
        else
        {
            Response.Write("<Script language='JavaScript'>alert('目前無資料可匯出！');</Script>");
            return;
        }
    }
}