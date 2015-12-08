using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_MusicSinglesCode : BasePage
{
    //NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

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

    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");
        if (!IsPostBack)
        {
            LoadFormData();
        }
    }

    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = @"select [CodeID],[CodeType] as [類別名稱],[CodeNo] as [排序],[CodeName] as [項目名稱] 
                            from [MusicSinglesCode] where 1=1 ";
        if (txtCodeType.Text != "")
        {
            strSql += " and CodeType like @CodeType ";
        }
        if (txtCodeName.Text != "")
        {
            strSql += " and CodeName like @CodeName ";
        }
        strSql += " order by CodeType,CodeNo ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CodeType", "%" + txtCodeType.Text.Trim() + "%");
        dict.Add("CodeName", "%" + txtCodeName.Text.Trim() + "%");
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("CodeID");
        npoGridView.DisableColumn.Add("CodeID");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = "MusicSinglesCodeEdit.aspx?id=";
        npoGridView.PageSize = 20;
        lblGridList.Text = npoGridView.Render();
    }

    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("MusicSinglesCodeAdd.aspx", ""));
    }

    //----------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

}
