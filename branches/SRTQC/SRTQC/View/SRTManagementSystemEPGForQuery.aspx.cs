using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Core.Service;

public partial class SRTManagementSystemEPG : BasePage
{
    SrtmcsEpgService objEpgService = new SrtmcsEpgService();

    /// <summary>
    /// Page_Load
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/11, Create
    /// </history>
    protected void Page_Load(object sender, EventArgs e)
    {
        /* 2014/01/09 匯入後導業查詢全部決定由使用者輸入資料再查詢
        string quary  = Request.Params["QUARY"];

        if("QUARY".Equals(quary))
        {
            this.GridDataBind();
        }
         */
    }

    #region Button
    /// <summary>
    /// 查詢按鈕
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/15, Create
    /// </history>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        // Grid顯示待辦維護清單
        this.GridDataBind();
    }
    #endregion

    #region Grid
    /// <summary>
    /// Grid RowDataBound
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">GridViewRowEventArgs.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/11, Create
    /// </history>
    protected void gridEPGList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //光棒效果:滑鼠經過變色
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("OnMouseover", "this.style.backgroundColor='#008A8C';this.style.color='White'");
            if (e.Row.RowIndex % 2 == 0)
            {
                //單數列
                e.Row.Attributes.Add("OnMouseout", "this.style.backgroundColor='#EEEEEE';this.style.color='Black'");
            }
            else
            {
                //雙數列
                e.Row.Attributes.Add("OnMouseout", "this.style.backgroundColor='#DCDCDC';this.style.color='Black'");
            }
        }            
    }

    /// <summary>
    /// Grid PageIndexChanging
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">GridViewPageEventArgs.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/11, Create
    /// </history>
    protected void gridEPGList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.gridEPGList.PageIndex = e.NewPageIndex;        
    }

    /// <summary>
    /// Grid PageIndexChanged
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">EventArgs.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/11, Create
    /// </history>
    protected void gridEPGList_PageIndexChanged(object sender, EventArgs e)
    {
        this.GridDataBind();
    }

    /// <summary>
    /// Grid 資料繫結
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/10/11, Create
    /// </history>
    protected void GridDataBind()
    {
        string msg = "";

        bool haveError = false;
        bool CtimeError = false;
        // 開始日期
        string startDate = this.tbStartDate.Text;
        // 結束日期
        string endDate = this.tbEndDate.Text;
        // 頻道別
        string channel = this.Channel_box.SelectedValue; ;
        // 節目/短片代碼
        string planningTitle = this.PlanningTitle_box.Text;
        // 集數
        string episodeNumber = this.EpisodeNumber_box.Text;
        // HouseNumber
        string houseNo = this.HouseNo_box.Text;
        // 首/重播
        string premier = this.Premier_box.Text;
        // 節目名稱
        string planningName = this.PlanningName_box.Text;
        // 播出時間
        string calendarTime = this.CalendarTime_box.Text;
        // 結束時間
        string calendarTime2 = this.CalendarTime2_box.Text;

        // 2015/5/19 已在前端javascript檢查，但還是留下第二道檢查，把日期範圍放大備用。
        // 2014/9/24 增加檢查查詢條件
        if (startDate == "") msg += "未輸入開始日期！\\n";
        if (endDate == "") msg += "未輸入結束日期！\\n";
        if (startDate != "" && (startDate.Length != 10 || !IsDate(startDate))) msg += "開始日期錯誤！\\n";
        if (endDate != "" && (endDate.Length != 10 || !IsDate(endDate))) msg += "結束日期錯誤！\\n";
        if (startDate != "" && endDate != "" && msg == "")
        {
            if (Convert.ToDateTime(startDate) > Convert.ToDateTime(endDate)) msg += "開始日期至結束日期的範圍有誤！\\n";
            TimeSpan ts = Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate);
            if (ts.Days > 92) msg += "開始日期至結束日期的範圍太大！請選擇三個月內範圍。\\n";
        }
        if (msg != "")
        {
            InfoPanel.Visible = false;
            this.gridEPGList.Visible = false;
            this.gridEPGList2.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + msg + "');", true);
            return;
        }

        // Grid顯示待辦維護清單
        DataTable dt = objEpgService.GetEpgList(startDate, endDate, channel, planningTitle, episodeNumber, houseNo, premier, planningName, calendarTime, calendarTime2);

        if (dt.Rows.Count > 0)
        {
            //20131118 Add by Tanya:增加顯示資料筆數
            this.InfoPanel.Visible = true;
            this.lblGridCount.Text = "共" + dt.Rows.Count + "筆";

            //系列名稱
            DataColumn dtCol1 = dt.Columns.Add("EpisodeTitle1", typeof(string));
            dtCol1.AllowDBNull = true;
            dtCol1.Unique = false;
            //分集名稱
            DataColumn dtCol2 = dt.Columns.Add("EpisodeTitle2", typeof(string));
            dtCol2.AllowDBNull = true;
            dtCol2.Unique = false;
            //講員姓名
            DataColumn dtCol3 = dt.Columns.Add("EpisodeTitle3", typeof(string));
            dtCol3.AllowDBNull = true;
            dtCol3.Unique = false;
            //字幕上傳記錄
            DataColumn dtCol4 = dt.Columns.Add("UploadRecord", typeof(string));
            dtCol4.AllowDBNull = true;
            dtCol4.Unique = false;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["EpisodeTitle"].ToString().IndexOf('_') > -1)
                {
                    //系列名稱_分集名稱_講員姓名
                    string[] EpisodeTitle = dt.Rows[i]["EpisodeTitle"].ToString().Split('_');

                    if (EpisodeTitle.Length == 3)
                    {                        
                        dt.Rows[i]["EpisodeTitle1"] = EpisodeTitle[0];
                        dt.Rows[i]["EpisodeTitle2"] = EpisodeTitle[1];
                        dt.Rows[i]["EpisodeTitle3"] = EpisodeTitle[2];
                    }
                    else
                    {
                        dt.Rows[i]["EpisodeTitle1"] = "「" + dt.Rows[i]["EpisodeTitle"].ToString() + "」==> 資料格式錯誤!!!";
                        haveError = true;
                    }
                }
                else
                {
                    if (dt.Rows[i]["PlanningTitleName"].ToString() == dt.Rows[i]["EpisodeTitle"].ToString())
                    {
                        dt.Rows[i]["EpisodeTitle1"] = string.Empty;
                        dt.Rows[i]["EpisodeTitle2"] = string.Empty;
                        dt.Rows[i]["EpisodeTitle3"] = string.Empty;
                    }
                    else
                    {
                        dt.Rows[i]["EpisodeTitle1"] = dt.Rows[i]["EpisodeTitle"].ToString();
                        dt.Rows[i]["EpisodeTitle2"] = dt.Rows[i]["EpisodeTitle"].ToString();
                        dt.Rows[i]["EpisodeTitle3"] = string.Empty;
                    }
                }

                //字幕上傳記錄
                if (dt.Rows[i]["TypeCommnet"].ToString() == "Y" || dt.Rows[i]["TypeCommnet"].ToString().IndexOf("資料錯誤") > -1)
                {
                    dt.Rows[i]["UploadRecord"] = "";
                }
                else if (dt.Rows[i]["TypeCommnet"].ToString() == "N")
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["_ST041"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["_ST043"].ToString()))
                    {
                        //TXT 有 SRT 有
                        //修改為24小時制
                        dt.Rows[i]["UploadRecord"] = ((System.DateTime)dt.Rows[i]["_MTIME"]).ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else if (string.IsNullOrEmpty(dt.Rows[i]["_ST041"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["_ST043"].ToString()))
                    {
                        //TXT 沒有 SRT 有
                        dt.Rows[i]["UploadRecord"] = "警示:無TXT字幕檔案";
                    }
                    else if (!string.IsNullOrEmpty(dt.Rows[i]["_ST041"].ToString()) && string.IsNullOrEmpty(dt.Rows[i]["_ST043"].ToString()))
                    {
                        //TXT 有 SRT 沒有 
                        dt.Rows[i]["UploadRecord"] = "警示:無SRT字幕檔案";
                        CtimeError = true;
                    }
                    else
                    {
                        //TXT 沒有 SRT 沒有
                        dt.Rows[i]["UploadRecord"] = "警示:無TXT及SRT字幕檔案上傳";
                        CtimeError = true;
                    }
                }
            }          

            if (planningTitle.Length != 0 || episodeNumber.Length != 0)
            {
                this.gridEPGList2.DataSource = dt;
            }
            else
            {
                this.gridEPGList.DataSource = dt;
            }            
        }
        else
        {
            //20131118 Add by Tanya:增加顯示資料筆數
            this.InfoPanel.Visible = true;
            this.lblGridCount.Text = "共0筆";

            this.gridEPGList.DataSource = null;
        }

        if (planningTitle.Length != 0 || episodeNumber.Length != 0)
        {
            this.gridEPGList2.DataBind();
            this.gridEPGList2.Visible = true;
            this.gridEPGList.Visible = false;

            for (int index = 0; index < this.gridEPGList2.Rows.Count; index++)
            {
                //設定「資料格式錯誤」資料格式
                if (haveError)
                {                
                    if (this.gridEPGList2.Rows[index].Cells[9].Text.ToString().IndexOf("資料格式錯誤") > -1)
                    {
                        this.gridEPGList2.Rows[index].Cells[9].ForeColor = System.Drawing.Color.Red;
                        this.gridEPGList2.Rows[index].BackColor = System.Drawing.Color.Yellow;
                        this.gridEPGList2.Rows[index].Attributes.Add("OnMouseout", "this.style.backgroundColor='#FFFF00';this.style.color='Black'");
                    }                                   
                }

                //設定每日早上5點為第一筆資料格式
                 if (this.gridEPGList2.Rows[index].Cells[6].Text.ToString() == "05:00:00:000")
                {
                    this.gridEPGList2.Rows[index].BackColor = System.Drawing.Color.DarkTurquoise;
                    this.gridEPGList2.Rows[index].Attributes.Add("OnMouseout", "this.style.backgroundColor='#00CED1';this.style.color='Black'");
                }

                 //設定「字幕已疊映」格式                
                if (this.gridEPGList2.Rows[index].Cells[12].Text.ToString().IndexOf("資料錯誤") > -1)
                {
                    this.gridEPGList2.Rows[index].Cells[12].ForeColor = System.Drawing.Color.Red;
                }                 

                //設定「字幕上傳記錄」格式
                if (CtimeError)
                {
                    if (this.gridEPGList2.Rows[index].Cells[13].Text.ToString().IndexOf("警示") > -1)
                    {
                        this.gridEPGList2.Rows[index].Cells[13].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }                    
        }
        else
        {
            this.gridEPGList.DataBind();
            this.gridEPGList.Visible = true;
            this.gridEPGList2.Visible = false;

            for (int index = 0; index < this.gridEPGList.Rows.Count; index++)
            {
                //設定「資料格式錯誤」資料格式
                if (haveError)
                {
                    if (this.gridEPGList.Rows[index].Cells[9].Text.ToString().IndexOf("資料格式錯誤") > -1)
                    {
                        this.gridEPGList.Rows[index].Cells[9].ForeColor = System.Drawing.Color.Red;
                        this.gridEPGList.Rows[index].Cells[9].BackColor = System.Drawing.Color.Yellow;
                        //this.gridEPGList.Rows[index].Attributes.Add("OnMouseout", "this.style.backgroundColor='#FFFF00';this.style.color='Black'");
                    } 
                }

                //設定每日早上5點為第一筆資料格式
                if (this.gridEPGList.Rows[index].Cells[2].Text.ToString() == "05:00:00:000")
                {
                    this.gridEPGList.Rows[index].BackColor = System.Drawing.Color.DarkTurquoise;
                    this.gridEPGList.Rows[index].Attributes.Add("OnMouseout", "this.style.backgroundColor='#00CED1';this.style.color='Black'");
                }

                //設定「字幕已疊映」格式                
                if (this.gridEPGList.Rows[index].Cells[12].Text.ToString().IndexOf("資料錯誤") > -1)
                {
                    this.gridEPGList.Rows[index].Cells[12].ForeColor = System.Drawing.Color.Red;
                }                

                //設定「字幕上傳記錄」格式
                if (CtimeError)
                {
                    if (this.gridEPGList.Rows[index].Cells[13].Text.ToString().IndexOf("警示") > -1)
                    {
                        this.gridEPGList.Rows[index].Cells[13].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
       
    }
    #endregion

    public bool IsDate(string DateString)
    {
        try
        {
            Convert.ToDateTime(DateString);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

}