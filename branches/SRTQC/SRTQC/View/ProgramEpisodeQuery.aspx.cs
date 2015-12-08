using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ProgramEpisodeQuery : BasePage
{
    /*#region NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage, HFD_CurrentQuerye;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }
    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);

        HFD_CurrentQuerye = new HiddenField();
        HFD_CurrentQuerye.Value = "Query";
        HFD_CurrentQuerye.ID = "HFHFD_CurrentQuerye";
        Form1.Controls.Add(HFD_CurrentQuerye);
    }
    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }
    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    #endregion NpoGridView 處理換頁相關程式碼*/
    protected void Page_Load(object sender, EventArgs e)
    {
        //有 npoGridView 時才需要
        //Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            if (Session["ProgramCode"] != null)
            {
                tbxProgramCode.Text = Session["ProgramCode"].ToString();
            }
            if (Session["EpisodeNo_Start"] != null)
            {
                tbxEpisodeNo_Start.Text = Session["EpisodeNo_Start"].ToString();
            }
            if (Session["EpisodeNo_End"] != null)
            {
                tbxEpisodeNo_End.Text = Session["EpisodeNo_End"].ToString();
            }
            if (tbxProgramCode.Text != "" || tbxEpisodeNo_Start.Text != "" || tbxEpisodeNo_End.Text != "")
            {
                LoadFormData();
            }
        }
    }
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"select ProgramID ,ProgramCode as 節目代號, [dbo].[getProgramName]([ProgramCode]) as 節目名稱, Episode as 集數 ,SubSeries as 系列名稱 ,EpisodeName as 分集名稱 ,PresenterSpeaker as 主持人or講員 ,
                    TotalLength as 總長度 ,Case when CopyrightNote = '1' then 'V' else '' end as 版權註記 , Case when NoBroadcastNote = '1' then 'V' else '' end as 不播出註記 ,
                    Case when NoReplayNote = '1' then 'V' else '' end as 不重播註記 , Case when NoProvidevideoNote = '1' then 'V' else '' end as 不供片註記 ,
                    Case when Lock = 'Y' then 'V' else '' end as 更新鎖定, Case when ISNULL(ReminderNotes,'') <> '' then 'V' else '' end as 提醒紀要
                    from dbo.ProgramEpisode where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxProgramCode.Text.Trim() != "")
        {
            strSql += " and ProgramCode like N'%" + tbxProgramCode.Text.Trim() + "%'";
        }
        if (tbxEpisodeNo_Start.Text.Trim() != "")
        {
            strSql += " and Episode >= '" + tbxEpisodeNo_Start.Text.Trim() + "' ";
        }
        if (tbxEpisodeNo_End.Text.Trim() != "")
        {
            strSql += " and Episode <= '" + tbxEpisodeNo_End.Text.Trim() + "' ";
        }
        strSql += " order by ProgramCode,Episode ";
        Session["ProgramCode"] = tbxProgramCode.Text.Trim();
        Session["EpisodeNo_Start"] = tbxEpisodeNo_Start.Text.Trim();
        Session["EpisodeNo_End"] = tbxEpisodeNo_End.Text.Trim();
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("ProgramID");
            npoGridView.DisableColumn.Add("ProgramID");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ProgramEpisodeEdit.aspx", "ProgramID=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramEpisodeAdd.aspx", "ProgramCode=" + tbxProgramCode.Text));
    }
    //刪除
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql = "delete from ProgramEpisode where 1=1 ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        
        if (tbxProgramCode.Text.Trim() != "")
        {
            strSql += " and ProgramCode like N'%" + tbxProgramCode.Text.Trim() + "%'";
        }
        if (tbxEpisodeNo_Start.Text.Trim() != "")
        {
            strSql += " and Episode >= '" + tbxEpisodeNo_Start.Text.Trim() + "' ";
        }
        if (tbxEpisodeNo_End.Text.Trim() != "")
        {
            strSql += " and Episode <= '" + tbxEpisodeNo_End.Text.Trim() + "' ";
        }
        NpoDB.ExecuteSQLS(strSql, dict);

        Response.Write("<Script language='JavaScript'>alert('分集資料刪除成功！');</Script>");
        Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramEpisodeQuery.aspx');</Script>");
        Response.End();
    }
}