using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_MusicSinglesQuery : System.Web.UI.Page
{
    #region NpoGridView 處理換頁相關程式碼
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
        HFD_CurrentQuerye.ID = "HFD_CurrentQuerye";
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
    #endregion NpoGridView 處理換頁相關程式碼

    protected void Page_Load(object sender, EventArgs e)
    {
        //有 npoGridView 時才需要
        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            //string strQueryKey = Util.GetQueryString("QueryKey");
            string strQueryKey = Session["QueryKey"] != null ? Session["QueryKey"].ToString() : "";
            //Session["QueryKey"] = null;
            string[] arrayQueryKey = strQueryKey.Split('|');
            btnExportQueryExcel.Enabled = true;
            if (arrayQueryKey.Length == 9)
            {
                tbFullTextSearch.Text = arrayQueryKey[0];
                tbxMusicLengthMin_Start.Text = arrayQueryKey[1] == "" ? "" : Convert.ToInt32(arrayQueryKey[1]) / 60 == 0 ? "" : (Convert.ToInt32(arrayQueryKey[1]) / 60).ToString();
                tbxMusicLengthSec_Start.Text = arrayQueryKey[1] == "" ? "" : Convert.ToInt32(arrayQueryKey[1]) % 60 == 0 ? "" : (Convert.ToInt32(arrayQueryKey[1]) % 60).ToString();
                tbxMusicLengthMin_End.Text = arrayQueryKey[2] == "" ? "" : Convert.ToInt32(arrayQueryKey[2]) / 60 == 0 ? "" : (Convert.ToInt32(arrayQueryKey[2]) / 60).ToString();
                tbxMusicLengthSec_End.Text = arrayQueryKey[2] == "" ? "" : Convert.ToInt32(arrayQueryKey[2]) % 60 == 0 ? "" : (Convert.ToInt32(arrayQueryKey[2]) % 60).ToString();
                tbxSpeed_Start.Text = arrayQueryKey[3];
                tbxSpeed_End.Text = arrayQueryKey[4];
                tbxRating_Start.Text = arrayQueryKey[5];
                tbxRating_End.Text = arrayQueryKey[6];
                txtVideoDateBegin.Text = arrayQueryKey[7];
                txtVideoDateEnd.Text = arrayQueryKey[8];
                LoadFormData();
            }
            else
            {
                btnExportQueryExcel.Enabled = false;
            }
            loadDropDownList(ddlCategory, "音樂單曲類別");

        }

    }

    public void LoadFormData()
    {
        string strSql;
        string strSqlWhere = "";
        DataTable dt;
        strSql = @"SELECT [UniqueNumber] ,[Category] as [節目] ,[Item] as [類別] ,Convert(char(10),[VideoDate],111) as [錄影日期] 
                    ,[Performer_Singer] as [演出者] ,[Tracks] as [曲目名稱] 
                    ,rtrim(cast([MusicLength]/60 as char(5))) + '分' + rtrim(cast([MusicLength]%60 as char(5))) + '秒' as [長度] 
                    ,[Described] as [敘述] ,[SN1Number] as [SN編號] ,[Take] ,[Authors] as [作詞] 
                    ,[Compose] as [作曲] ,[Arranger_Adaptation] as [編曲/改編] ,[Audit] as [審核] 
                    ,[MusicalStyles] as [曲風] ,[MusicalInstruments] as [樂器] ,[Language] as [語言] 
                    ,[ScriptMark] as [腳本/集數] ,[Speed] as [速度] ,[Rating] as [評比] ,[Festivals] as [節期] 
                    ,[Remark] as [備註]
                   FROM [dbo].[MusicSingles]  where 1=1 ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        //全文檢索
        /*
        唯一編號	UniqueNumber int
        節目	Category
        類別	Item
        演出者/演唱者	Performer_Singer
        曲目	Tracks
        敘述	Described
        SN1編號	SN1Number
        Take
        作詞	Authors
        作曲	Compose
        編曲/改編	Arranger_Adaptation
        審核	Audit
        曲風	MusicalStyles
        樂器	MusicalInstruments
        語言	Language
        標記	ScriptMark
        節期	Festivals
        備註	Remark
        */
        if (tbFullTextSearch.Text.Trim() != "")
        {
            string[] aText = tbFullTextSearch.Text.Trim().Split(' ');
            foreach (string strTextSearch in aText)
            {
                if (!String.IsNullOrEmpty(strTextSearch))
                {

                    strSqlWhere += " AND (ISNULL([Category],'') like '%" + strTextSearch + "%' or ISNULL([Item],'') like '%" +
                        strTextSearch + "%' or ISNULL([Performer_Singer],'') like '%" + strTextSearch + "%' or ISNULL([Take],'') like '%" +
                        strTextSearch + "%' or ISNULL([Tracks],'') like '%" + strTextSearch + "%' or ISNULL([Described],'') like '%" +
                        strTextSearch + "%' or ISNULL([SN1Number],'') like '%" + strTextSearch + "%' or ISNULL([Authors],'') like '%" +
                        strTextSearch + "%' or ISNULL([Compose],'') like '%" + strTextSearch + "%' or ISNULL([Arranger_Adaptation],'') like '%" +
                        strTextSearch + "%' or ISNULL([Audit],'') like '%" + strTextSearch + "%' or ISNULL([MusicalStyles],'') like '%" +
                        strTextSearch + "%' or ISNULL([MusicalInstruments],'') like '%" + strTextSearch + "%' or ISNULL([Language],'') like '%" +
                        strTextSearch + "%' or ISNULL([ScriptMark],'') like '%" + strTextSearch + "%' or ISNULL([Festivals],'') like '%" +
                        strTextSearch + "%' or ISNULL([Remark],'') like '%" + strTextSearch + "%') ";
                }
            }
        }
        //錄影日期	VideoDate
        if (!String.IsNullOrEmpty(txtVideoDateBegin.Text))
        {
            strSqlWhere += " AND (CAST([VideoDate] as Date) >= CAST('" + txtVideoDateBegin.Text + "' as Date) ) ";
        }
        if (!String.IsNullOrEmpty(txtVideoDateEnd.Text))
        {
            strSqlWhere += " AND (CAST([VideoDate] as Date) <= CAST('" + txtVideoDateEnd.Text + "' as Date) ) ";
        }
        //音樂長度	MusicLength
        int intMusicLength_Start = (String.IsNullOrEmpty(tbxMusicLengthMin_Start.Text) ? 0 : Convert.ToInt32(tbxMusicLengthMin_Start.Text) * 60)
            + (String.IsNullOrEmpty(tbxMusicLengthSec_Start.Text) ? 0 : Convert.ToInt32(tbxMusicLengthSec_Start.Text));
        int intMusicLength_End = (String.IsNullOrEmpty(tbxMusicLengthMin_End.Text) ? 0 : Convert.ToInt32(tbxMusicLengthMin_End.Text) * 60)
            + (String.IsNullOrEmpty(tbxMusicLengthSec_End.Text) ? 0 : Convert.ToInt32(tbxMusicLengthSec_End.Text));

        if (intMusicLength_Start > 0)
        {
            strSqlWhere += " AND ([MusicLength] >= " + intMusicLength_Start.ToString() + ") ";
        }
        if (intMusicLength_End > 0)
        {
            strSqlWhere += " AND ([MusicLength] <= " + intMusicLength_End.ToString() + ") ";
        }
        //速度	Speed
        if (tbxSpeed_Start.SelectedValue != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],0) > 0 AND [Speed] >= " + tbxSpeed_Start.SelectedValue + " ) ";
        }
        if (tbxSpeed_End.SelectedValue != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],0) > 0 AND [Speed] <= " + tbxSpeed_End.SelectedValue + " ) ";
        }
        //評比	Rating
        if (tbxRating_Start.SelectedValue != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],0) > 0 AND [Rating] >= " + tbxRating_Start.SelectedValue + " ) ";
        }
        if (tbxRating_End.SelectedValue != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],0) > 0 AND [Rating] <= " + tbxRating_End.SelectedValue + " ) ";
        }
        /* 保留數字
        //速度	Speed
        if (tbxSpeed_Start.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],'') <> '' AND CAST([Speed] as int) >= CAST('" + tbxSpeed_Start.Text.Trim() + "' as int) ) ";
        }
        if (tbxSpeed_End.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Speed],'') <> '' AND CAST([Speed] as int) <= CAST('" + tbxSpeed_End.Text.Trim() + "' as int) ) ";
        }
        //評比	Rating
        if (tbxRating_Start.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],'') <> '' AND CAST([Rating] as int) >= CAST('" + tbxRating_Start.Text.Trim() + "' as int) ) ";
        }
        if (tbxRating_End.Text.Trim() != "")
        {
            strSqlWhere += " AND (ISNULL([Rating],'') <> '' AND CAST([Rating] as int) <= CAST('" + tbxRating_End.Text.Trim() + "' as int) ) ";
        }
        */

        strSql += strSqlWhere + " order by Category,Item,Performer_Singer,Tracks ";
        dt = NpoDB.GetDataTableS(strSql, dict);
        Session["strSql"] = strSql;

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            lblGridList.ForeColor = System.Drawing.Color.Red;
            btnExportQueryExcel.Enabled = false;
            Session["QueryKey"] = null;
        }
        else
        {
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("UniqueNumber");
            npoGridView.DisableColumn.Add("UniqueNumber");
            npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = true; //換頁
            npoGridView.EditLink = Util.RedirectByTime("MusicSinglesEdit.aspx", "UniqueNumber=");
            //npoGridView.EditLinkTarget = "_blank";  //開啟新視窗
            npoGridView.EditLinkDoubleClick = true;     // double-click
            //npoGridView.ColumnByRow = 12; //設定每行12個欄位
            npoGridView.PageSize = 10;
            //npoGridView.CssClass = "table-list";
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;

            btnExportQueryExcel.Enabled = true;

            string strQueryKey = tbFullTextSearch.Text + "|";
            strQueryKey += (intMusicLength_Start == 0 ? "" : intMusicLength_Start.ToString()) + "|";
            strQueryKey += (intMusicLength_End == 0 ? "" : intMusicLength_End.ToString()) + "|";
            strQueryKey += tbxSpeed_Start.Text + "|";
            strQueryKey += tbxSpeed_End.Text + "|";
            strQueryKey += tbxRating_Start.Text + "|";
            strQueryKey += tbxRating_End.Text + "|";
            strQueryKey += txtVideoDateBegin.Text + "|";
            strQueryKey += txtVideoDateEnd.Text;
            Session["QueryKey"] = strQueryKey;
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
    }

    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

    private void loadDropDownList(DropDownList DropDownListName, string Category)
    {

        string strSql = "select distinct [CodeNo],[CodeName] from [dbo].[MusicSinglesCode] " +
                        " where [CodeType] = '" + Category + "' order by [CodeNo] ; ";
        Util.FillDropDownList(DropDownListName, strSql, "CodeName", "CodeName", true, "", "請選擇");

    }

    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("MusicSinglesAdd.aspx"));
    }

    protected void btnExportQueryExcel_Click(object sender, EventArgs e)
    {

        if (Session["strSql"] == null || Session["strSql"].ToString() == "")
        {

            return;
        }
        string strSql = Session["strSql"].ToString();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0) return;

        string strExcel = @"<?xml version='1.0'?>
<?mso-application progid='Excel.Sheet'?> 
<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
    xmlns:o='urn:schemas-microsoft-com:office:office'
    xmlns:x='urn:schemas-microsoft-com:office:excel'
    xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
    xmlns:html='http://www.w3.org/TR/REC-html40'>
 <ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>
  <WindowHeight>10275</WindowHeight>
  <WindowWidth>21600</WindowWidth>
  <WindowTopX>0</WindowTopX>
  <WindowTopY>0</WindowTopY>
  <TabRatio>718</TabRatio>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
<Styles>
    <Style ss:ID='Default' ss:Name='Normal'>
        <Alignment ss:Vertical='Bottom' />
        <Borders />
        <Font ss:FontName='新細明體' ss:Size='10'/>
        <Interior />
        <NumberFormat />
        <Protection />
    </Style>
  <Style ss:ID='s62'>
   <Alignment ss:Horizontal='Center' ss:Vertical='Center'/>
   <Borders>
    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
   </Borders>
   <Font ss:FontName='新細明體' ss:Bold='1'/>
   <Interior ss:Color='#C0C0C0' ss:Pattern='Solid'/>
  </Style>
  <Style ss:ID='s63'>
   <Alignment ss:Horizontal='Center' ss:Vertical='Center'/>
   <Borders>
    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
   </Borders>
   <Interior/>
  </Style>
</Styles>

<Worksheet ss:Name='Sheet1'>
<Table>
   <Column ss:Width='69'/>
   <Column ss:AutoFitWidth='0' ss:Width='90'/>
   <Column ss:AutoFitWidth='0' ss:Width='102'/>
   <Column ss:Width='69.75'/>
   <Column ss:Width='156.75'/>
   <Column ss:Width='69.75'/>
   <Column ss:Width='56.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='78.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='74.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='95.25'/>
   <Column ss:Width='50.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='63'/>
   <Column ss:AutoFitWidth='0' ss:Width='129.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='56.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='63.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='44.25'/>
   <Column ss:Index='18' ss:AutoFitWidth='0' ss:Width='87.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='122.25'/>
   <Row>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>錄影日期</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>演出者</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>曲目名稱</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>長度</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>敘述</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>SN編號</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>Take</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>作詞</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>作曲</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>編曲/改編</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>審核</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>曲風</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>樂器</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>語言</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>腳本/集數</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>速度</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>評比</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>節期</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>備註</Data></Cell>
   </Row>
";

            foreach (DataRow drSheet in dt.Rows)
            {

                strExcel += "<Row>";
                if (String.IsNullOrEmpty(drSheet["錄影日期"].ToString()))
                {
                    strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'></Data></Cell>";
                }
                else
                {
                    strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + DateTime.Parse(drSheet["錄影日期"].ToString()).ToString("yyyy/MM/dd") + "</Data></Cell>";
                }
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["演出者"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["曲目名稱"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["長度"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["敘述"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["SN編號"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Take"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["作詞"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["作曲"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["編曲/改編"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["審核"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["曲風"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["樂器"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["語言"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["腳本/集數"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='Number'>" + drSheet["速度"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='Number'>" + drSheet["評比"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["節期"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["備註"].ToString() + "</Data></Cell>";
                strExcel += "</Row>";

            }

            strExcel += @"
</Table>
<WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>
    <FrozenNoSplit/>
    <SplitHorizontal>1</SplitHorizontal>
    <TopRowBottomPane>1</TopRowBottomPane>
    <ProtectObjects>False</ProtectObjects>
    <ProtectScenarios>False</ProtectScenarios>
</WorksheetOptions>
</Worksheet>
<Worksheet ss:Name='Sheet2'>
</Worksheet>
<Worksheet ss:Name='Sheet3'>
</Worksheet>
</Workbook>";

            Util.OutputTxt(strExcel, "1", "MusicSingles_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
    
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

        string strSql = @"SELECT [CodeName] FROM [dbo].[MusicSinglesCode] where [CodeType] = @CodeType order by [CodeNo] ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CodeType", ddlCategory.SelectedValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0) return;

        string strExcel = @"<?xml version='1.0'?>
<?mso-application progid='Excel.Sheet'?> 
<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
    xmlns:o='urn:schemas-microsoft-com:office:office'
    xmlns:x='urn:schemas-microsoft-com:office:excel'
    xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
    xmlns:html='http://www.w3.org/TR/REC-html40'>
 <ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>
  <WindowHeight>10275</WindowHeight>
  <WindowWidth>21600</WindowWidth>
  <WindowTopX>0</WindowTopX>
  <WindowTopY>0</WindowTopY>
  <TabRatio>718</TabRatio>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
<Styles>
    <Style ss:ID='Default' ss:Name='Normal'>
        <Alignment ss:Vertical='Bottom' />
        <Borders />
        <Font ss:FontName='新細明體' ss:Size='10'/>
        <Interior />
        <NumberFormat />
        <Protection />
    </Style>
  <Style ss:ID='s62'>
   <Alignment ss:Horizontal='Center' ss:Vertical='Center'/>
   <Borders>
    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
   </Borders>
   <Font ss:FontName='新細明體' ss:Bold='1'/>
   <Interior ss:Color='#C0C0C0' ss:Pattern='Solid'/>
  </Style>
  <Style ss:ID='s63'>
   <Alignment ss:Horizontal='Center' ss:Vertical='Center'/>
   <Borders>
    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'
     ss:Color='#000000'/>
   </Borders>
   <Interior/>
  </Style>
</Styles>
";

        int i = 1;
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            string strSqlSheet = @"SELECT * FROM [dbo].[MusicSingles] where [Category] = @Category and [Item] = @Item
                                    order by [UniqueNumber] ";
            Dictionary<string, object> dictSheet = new Dictionary<string, object>();
            dictSheet.Add("Category", ddlCategory.SelectedValue);
            dictSheet.Add("Item", dr["CodeName"].ToString());
            DataTable dtSheet = NpoDB.GetDataTableS(strSqlSheet, dictSheet);

            // 0 = sheetName 演奏曲
            strExcel += "<Worksheet ss:Name='" + dr["CodeName"].ToString() + @"'>
<Table>
   <Column ss:Width='69'/>
   <Column ss:AutoFitWidth='0' ss:Width='90'/>
   <Column ss:AutoFitWidth='0' ss:Width='102'/>
   <Column ss:Width='69.75'/>
   <Column ss:Width='156.75'/>
   <Column ss:Width='69.75'/>
   <Column ss:Width='56.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='78.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='74.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='95.25'/>
   <Column ss:Width='50.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='63'/>
   <Column ss:AutoFitWidth='0' ss:Width='129.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='56.25'/>
   <Column ss:AutoFitWidth='0' ss:Width='63.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='44.25'/>
   <Column ss:Index='18' ss:AutoFitWidth='0' ss:Width='87.75'/>
   <Column ss:AutoFitWidth='0' ss:Width='122.25'/>
   <Row>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>錄影日期</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>演出者</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>曲目名稱</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>長度</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>敘述</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>SN編號</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>Take</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>作詞</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>作曲</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>編曲/改編</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>審核</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>曲風</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>樂器</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>語言</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>腳本/集數</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>速度</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>評比</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>節期</Data></Cell>
    <Cell ss:StyleID='s62'><Data ss:Type='String'>備註</Data></Cell>
   </Row>
";

            foreach (DataRow drSheet in dtSheet.Rows)
            {

                strExcel += "<Row>";
                if (String.IsNullOrEmpty(drSheet["VideoDate"].ToString()))
                {
                    strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'></Data></Cell>";
                }
                else
                {
                    strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + DateTime.Parse(drSheet["VideoDate"].ToString()).ToString("yyyy/MM/dd") + "</Data></Cell>";
                }
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Performer_Singer"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Tracks"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + String.Format("{0}分{1}秒", 
                    (Convert.ToInt32(drSheet["MusicLength"].ToString()) / 60).ToString(), 
                    (Convert.ToInt32(drSheet["MusicLength"].ToString()) % 60).ToString()) + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Described"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["SN1Number"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Take"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Authors"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Compose"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Arranger_Adaptation"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Audit"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["MusicalStyles"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["MusicalInstruments"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Language"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["ScriptMark"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='Number'>" + drSheet["Speed"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='Number'>" + drSheet["Rating"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Festivals"].ToString() + "</Data></Cell>";
                strExcel += "<Cell ss:StyleID='s63'><Data ss:Type='String'>" + drSheet["Remark"].ToString() + "</Data></Cell>";
                strExcel += "</Row>";

            }

            strExcel += String.Format(@"
</Table>
<WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>
   <TabColorIndex>{0}</TabColorIndex>
    <FrozenNoSplit/>
    <SplitHorizontal>1</SplitHorizontal>
    <TopRowBottomPane>1</TopRowBottomPane>
    <ProtectObjects>False</ProtectObjects>
    <ProtectScenarios>False</ProtectScenarios>
</WorksheetOptions>
</Worksheet>  ", i);

        }
        strExcel += "</Workbook>";

        Util.OutputTxt(strExcel, "1", ddlCategory.SelectedValue + "總表_" + DateTime.Now.ToString("yyyyMMddHHmmss"));

    }

}
