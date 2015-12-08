using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Text;
using System.Collections;
using Core.Domain;
using Core.Service;
using Core.Utility;
using System.Data.SqlClient;

public partial class UploadEPG : BasePage
{    
    SrtmcsEpgService objEpgService = new SrtmcsEpgService();   
    //string exportDir = "\\Files\\srt\\";    
    string exportDir = ConfigurationManager.AppSettings["srtPath"].ToString();

    string strErrList = "";

    /// <summary>
    /// Page_Load
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/15, Create    
    /// </history>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Button
    /// <summary>
    /// 匯入按鈕
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2013/10/15, Create   
    /// 2.Tanya Wu, 2013/11/23, 增加計算「播映長度」
    /// </history>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string ImportFileName = string.Empty;
        string ImportSource = string.Empty;
        string appPath = Request.PhysicalApplicationPath;
        string saveDir = "\\Files\\Upload\\";

        try
        {
            //檢查匯入檔案是否已選取
            if (fuFile.HasFile)
            {
                ImportFileName = this.fuFile.FileName;

                if (Path.GetExtension(ImportFileName) == ".xls")
                {
                    //Modify by Tanya:改為存檔為原始上傳檔名
                    ImportSource = appPath + saveDir + ImportFileName;

                    //1.文件存檔上傳至Server
                    fuFile.SaveAs(ImportSource);

                    //2.讀取Excel檔&Insert into epgTemp                    
                    if (InsertToTempTable(ImportSource))
                    {
                        //3.transfer to epg from epgTemp
                        //3.1 Delete old epg Data
                        objEpgService.DelEpgOldData();

                        //3.2 Update Duration
                        UpdateDuration();

                        //3.3 Insert epg from epgTemp
                        objEpgService.InsertEpgFromTemp();                        

                        //Mark by Tanya:EPG不匯出srt
                        //4.已上傳字幕(PlanningTitle&集數)轉HourseNo.srt
                        //4.1 取得本次匯入資料且srt資料已存在清單
                        //DataTable dt = objEpgService.GetSrtExportList();

                        //if (dt.Rows.Count > 0)
                        //{
                            ////4.2 匯出srt                            
                            //for (int rowsIndex = 0; rowsIndex < dt.Rows.Count; rowsIndex++)
                            //{
                                //if (CreateFile(dt.Rows[rowsIndex]["HouseNo"].ToString()))
                                //{
                                //    StreamWriter sw = new StreamWriter(exportDir + "\\" + dt.Rows[rowsIndex]["HouseNo"].ToString() + ".srt", true, System.Text.Encoding.UTF8);

                                //    sw.Write(dt.Rows[rowsIndex]["_ST043"].ToString());

                                //    //關閉StreamWriter
                                //    sw.Close();
                                //    sw.Dispose();
                                //}
                            //}
                        //}

                        // 2014/9/2 匯入節目分集基本檔 ProgramEpisode
                        InsertProgramEpisodeFromEPGTemp();

                        Response.Write("<script>confirm('EPG檔匯入完成! 是否要導向查詢頁面?')? window.location.replace('SRTManagementSystemEPG.aspx'):document.write('')</script>");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strErrList))
                        {
                            this.AlertMessage("匯入檔案有誤！請參考以下時段：\n" + strErrList);
                        }
                    }

                }
                else
                {
                    this.AlertMessage("必需為Excel檔(*.xls)");
                }
            }
            else
            {
                this.AlertMessage("請選取檔案!");
            }
        }
        catch (Exception ex)
        {
            this.AlertMessage("EPG檔匯入失敗!");
            this.AlertMessage(ex.Message.ToString());

            this.logger.Error(ex.Message, ex);
        }        

    }

    private void InsertProgramEpisodeFromEPGTemp()
    {

        DBProvider dbProvider = new DBProvider();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        try
        {
            dbProvider.ExecuteNonQuery("InsertProgramEpisodeFromEPGTemp", cmd);
        }
        catch (Exception ex)
        {
            this.AlertMessage(ex.Message.ToString());
        }

    }

    protected void btnQueryEPG_Click(object sender, EventArgs e)
    {
        // 導向EPG查詢頁面
        Response.Redirect("SRTManagementSystemEPG.aspx");
    }
    #endregion

    #region Other Method
    /// <summary>
    /// 把Excel資料Insert into Table
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/10/29, Create
    /// 2.Tanya Wu, 2013/11/26, Modify:匯入檔欄位更動
    /// </history>
    private bool InsertToTempTable(string excel_filename)
    {        
        SrtmcsEPGTemp objSrtEPGTemp = new SrtmcsEPGTemp();
        string ProviderName = "Microsoft.Jet.OLEDB.4.0;";
        string ExtendedString = "'Excel 8.0;";
        string Hdr = "Yes;";
        string IMEX = "1';";

        //Delete SrtEPGTemp Data
        objEpgService.DelEpgTemp();

        string cs = "Data Source=" + excel_filename + ";" +
                    "Provider=" + ProviderName +
                    "Extended Properties=" + ExtendedString +
                    "HDR=" + Hdr +
                    "IMEX=" + IMEX;

        // 用來存放上傳MOD檔案所需要的資料內容
        ArrayList ModDataList = new ArrayList();
        // 用來存放上傳大陸檔案所需要的資料內容
        ArrayList ChinaChannel1DataList = new ArrayList();
        ArrayList ChinaChannel2DataList = new ArrayList();

        // 用來檢核Planning Title 第二碼是英文或數字  如果是數字抓七碼 如果不是數字抓八碼 (節目名稱)
        string strValue = @"^\d+(\.)?\d*$"; //數字
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(strValue);

        using (OleDbConnection cn = new OleDbConnection(cs))
        {
            cn.Open();
            string qs = "Select * From [Sheet1$]";

            try
            {
                using (OleDbCommand cmd = new OleDbCommand(qs, cn))
                {
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (dr.Read())
                        {
                            //int Col = dr.FieldCount;
                            i++;

                            //Channel
                            objSrtEPGTemp.Channel = dr["Channel"] != null ? dr["Channel"].ToString() : "";
                            //CalendarDate	
                            if (dr["Calendar Date"].ToString() != null && dr["Calendar Date"].ToString() != "")
                                objSrtEPGTemp.CalendarDate = Convert.ToDateTime(dr["Calendar Date"].ToString());
                            else
                                objSrtEPGTemp.CalendarDate = DateTime.Today;

                            //Time
                            objSrtEPGTemp.Time = dr["Time"] != null ? dr["Time"].ToString() : "";
                            //HouseNo	
                            objSrtEPGTemp.HouseNo = dr["House No"] != null ? dr["House No"].ToString() : "";
                            //PlanningTitle
                            objSrtEPGTemp.PlanningTitle = dr["Planning Title"] != null ? dr["Planning Title"].ToString() : "";
                            //EpisodeNumber	
                            objSrtEPGTemp.EpisodeNumber = dr["Episode Number"] != null ? dr["Episode Number"].ToString() : "";
                            //EpisodeTitle	
                            objSrtEPGTemp.EpisodeTitle = dr["Episode Title"] != null ? dr["Episode Title"].ToString() : "";
                            //Premier	
                            objSrtEPGTemp.Premier = dr["Premier"] != null ? dr["Premier"].ToString() : "";
                            //Repeat	
                            objSrtEPGTemp.Repeat = dr["Repeat"] != null ? dr["Repeat"].ToString() : "";
                            //Type	
                            objSrtEPGTemp.Type = dr["Type"] != null ? dr["Type"].ToString() : "";
                            //Comment
                            objSrtEPGTemp.Comment = dr["Comment"] != null ? dr["Comment"].ToString() : "";

                            //EventDuration 
                            //objSrtEPGTemp.EventDuration = dr["Event Duration"] != null ? dr["Event Duration"].ToString() : "";
                            ////Media
                            //objSrtEPGTemp.Media = dr["Media"] != null ? dr["Media"].ToString() : "";                            
                            ////CalendarEndDate	
                            //objSrtEPGTemp.CalendarEndDate = Convert.ToDateTime(dr["Calendar End Date"].ToString());
                            ////Channel	
                            //objSrtEPGTemp.Channel = dr["Channel"] != null ? dr["Channel"].ToString() : "";
                            ////Region	
                            //objSrtEPGTemp.Region = dr["Region"] != null ? dr["Region"].ToString() : "";
                            ////ScheduleType	
                            //objSrtEPGTemp.ScheduleType = dr["Schedule Type"] != null ? dr["Schedule Type"].ToString() : "";                            
                            ////Category
                            //objSrtEPGTemp.Category = dr["Category"] != null ? dr["Category"].ToString() : "";
                            ////Source	
                            //objSrtEPGTemp.Source = dr["Source"] != null ? dr["Source"].ToString() : "";
                            ////FixedTime	
                            //objSrtEPGTemp.FixedTime = dr["Fixed Time"] != null ? dr["Fixed Time"].ToString() : "";                            
                            ////Exhibition	
                            //objSrtEPGTemp.Exhibition = dr["Exhibition"] != null ? dr["Exhibition"].ToString() : "";                            
                            ////Cost
                            //objSrtEPGTemp.Cost = dr["Cost"] != null ? dr["Cost"].ToString() : "";
                            ////Certificate	
                            //objSrtEPGTemp.Certificate = dr["Certificate"] != null ? dr["Certificate"].ToString() : "";
                            ////EstimatedRating
                            //objSrtEPGTemp.EstimatedRating = dr["Estimated Rating"] != null ? dr["Estimated Rating"].ToString() : "";
                            ////ActualRating	
                            //objSrtEPGTemp.ActualRating = dr["Actual Rating"] != null ? dr["Actual Rating"].ToString() : "";
                            ////AltTitle	
                            //objSrtEPGTemp.AltTitle = dr["Alt' Title"] != null ? dr["Alt' Title"].ToString() : "";
                            ////UmbrellaTitle	
                            //objSrtEPGTemp.UmbrellaTitle = dr["Umbrella Title"] != null ? dr["Umbrella Title"].ToString() : "";                            
                            ////TargetAudience	
                            //objSrtEPGTemp.TargetAudience = dr["Target Audience"] != null ? dr["Target Audience"].ToString() : "";
                            ////PlanningBlockContained	
                            //objSrtEPGTemp.PlanningBlockContained = dr["PlanningBlockContained"] != null ? dr["PlanningBlockContained"].ToString() : "";
                            ////ProductionNo	
                            //objSrtEPGTemp.ProductionNo = dr["Production No"] != null ? dr["Production No"].ToString() : "";
                            ////ScheduleRemark
                            //objSrtEPGTemp.ScheduleRemark = dr["Schedule Remark"] != null ? dr["Schedule Remark"].ToString() : "";                            
                            ////PlanningStaggerCasted	
                            //objSrtEPGTemp.PlanningStaggerCasted = dr["PlanningStaggerCasted"] != null ? dr["PlanningStaggerCasted"].ToString() : "";
                            ////PlanningMasterPlanningID	
                            //objSrtEPGTemp.PlanningMasterPlanningID = dr["PlanningMasterPlanningID"] != null ? dr["PlanningMasterPlanningID"].ToString() : "";
                            ////PlanningStaggerCastDate	
                            //objSrtEPGTemp.PlanningStaggerCastDate = dr["PlanningStaggerCastDate"] != null ? dr["PlanningStaggerCastDate"].ToString() : "";
                            ////PlanningStaggerCastEndDate	
                            //objSrtEPGTemp.PlanningStaggerCastEndDate = dr["PlanningStaggerCastEndDate"] != null ? dr["PlanningStaggerCastEndDate"].ToString() : "";
                            ////PlanningMasterChannelID	
                            //objSrtEPGTemp.PlanningMasterChannelID = dr["PlanningMasterChannelID"] != null ? dr["PlanningMasterChannelID"].ToString() : "";
                            ////BroadcastDate
                            //objSrtEPGTemp.BroadcastDate = Convert.ToDateTime(dr["Broadcast Date"].ToString());
                            ////EventID	
                            //objSrtEPGTemp.EventID = dr["Event ID"] != null ? dr["Event ID"].ToString() : "";
                            ////PlanningExtProgID	
                            //objSrtEPGTemp.PlanningExtProgID = dr["PlanningExtProgID"] != null ? dr["PlanningExtProgID"].ToString() : "";
                            ////PlanningExtEpisodeID	
                            //objSrtEPGTemp.PlanningExtEpisodeID = dr["PlanningExtEpisodeID"] != null ? dr["PlanningExtEpisodeID"].ToString() : "";
                            ////PlanningExtTimeStamp	
                            //objSrtEPGTemp.PlanningExtTimeStamp = dr["PlanningExtTimeStamp"] != null ? dr["PlanningExtTimeStamp"].ToString() : "";
                            ////PlanningExtEventID	
                            //objSrtEPGTemp.PlanningExtEventID = dr["PlanningExtEventID"] != null ? dr["PlanningExtEventID"].ToString() : "";                            
                            ////VariantCode	
                            //objSrtEPGTemp.VariantCode = dr["Variant Code"] != null ? dr["Variant Code"].ToString() : "";
                            ////PlanningProgrammePatternID	
                            //objSrtEPGTemp.PlanningProgrammePatternID = dr["PlanningProgrammePatternID"] != null ? dr["PlanningProgrammePatternID"].ToString() : "";                            
                            ////MediaSegmentPatternID	
                            //objSrtEPGTemp.MediaSegmentPatternID = dr["MediaSegmentPatternID"] != null ? dr["MediaSegmentPatternID"].ToString() : "";
                            ////Production Site	
                            //objSrtEPGTemp.ProductionSite = dr["Production Site"] != null ? dr["Production Site"].ToString() : "";
                            ////BreakDescription	
                            //objSrtEPGTemp.BreakDescription = dr["Break Description"] != null ? dr["Break Description"].ToString() : "";
                            ////SeriesLink
                            //objSrtEPGTemp.SeriesLink = dr["Series Link"] != null ? dr["Series Link"].ToString() : "";

                            // 有對應節目名稱為false 反之為true.
                            bool ProgramErr = true;

                            try
                            {
                                DataTable dt = objEpgService.GetEpgProgramName(objSrtEPGTemp.PlanningTitle.Substring(0, 7));
                                if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString().Trim()))
                                {
                                    ProgramErr = false;
                                }
                                dt.Dispose();
                            }
                            catch (Exception ex)
                            {
                                this.logger.Error(ex.Message, ex);
                            }

                            if (dr["Calendar Date"].ToString() == "")
                            {
                                strErrList += "第 " + i + " 筆時間(Calendar Date)有誤\n";
                            }
                            else if (objSrtEPGTemp.Channel == "" || objSrtEPGTemp.Time == "" || objSrtEPGTemp.HouseNo == "" ||
                                objSrtEPGTemp.PlanningTitle == "" || objSrtEPGTemp.EpisodeNumber == "" || objSrtEPGTemp.Premier == "" ||
                                 objSrtEPGTemp.Repeat == "" || objSrtEPGTemp.Type == "")
                            {
                                strErrList += dr["Calendar Date"].ToString() + "\n";
                            }
                            else if (ProgramErr)
                            {
                                strErrList += dr["Calendar Date"].ToString() + " 節目代碼對應節目名稱有誤\n";
                            }

                            //Insert into Table srtqc_epgTemp
                            objEpgService.InsertEpgTemp(objSrtEPGTemp);

                            // 節目名稱
                            string Program = "";

                            // 判斷第二個字串是否為數字,是的話抓前面七碼不是的話抓八碼
                            if (r.IsMatch(objSrtEPGTemp.PlanningTitle.Substring(1, 2)))
                            {
                                Program = objSrtEPGTemp.PlanningTitle.Substring(7).Trim();
                            }
                            else
                            {
                                Program = objSrtEPGTemp.PlanningTitle.Substring(8).Trim();
                            }

                            // 如果是一台資料的話
                            if ("GOODTV1".Equals(objSrtEPGTemp.Channel))
                            {
                                // MOD資料所需日期格式
                                string ModDateValue = objSrtEPGTemp.CalendarDate.ToString("yyyy-MM-dd HH:mm:ss.0");
                                // 抓取MOD所需資料放入LIST (節目名稱|播放時間) 
                                ModDataList.Add(Program + "|" + ModDateValue);

                                // 大陸一台資料所需日期格式
                                string ChinaChannelDateValue = objSrtEPGTemp.CalendarDate.ToString("yyyy/MM/dd HH:mm");
                                // 抓取大陸一台所需資料放入LIST (節目名稱|播放時間) 
                                ChinaChannel1DataList.Add(Program + "|" + ChinaChannelDateValue);
                            }

                            // 如果是二台資料的話
                            if ("GOODTV2".Equals(objSrtEPGTemp.Channel))
                            {
                                // 大陸二台資料所需日期格式
                                string ChinaChannelDateValue = objSrtEPGTemp.CalendarDate.ToString("yyyy/MM/dd HH:mm");
                                // 抓取大陸二台所需資料放入LIST (節目名稱|播放時間) 
                                ChinaChannel2DataList.Add(Program + "|" + ChinaChannelDateValue);
                            }

                        }
                     }
                }

                // 2014/7/21 檢查完後判斷尚有缺少資料欄位既中斷執行
                if (strErrList != "") return false;

                /* 2014/7/18 改由DTS轉檔產出
                StringBuilder ModSb = new StringBuilder("");
                if (ModDataList.Count != 0)
                {
                    if (ModDataList.Count > 1)
                    {
                        // 組成要寫入MOD檔案的資料內容
                        for (int i = 0; i < ModDataList.Count - 1; i++)
                        {
                            string FirstData = (string)ModDataList[i];
                            string ScendData = (string)ModDataList[i + 1];

                            string Program = FirstData.Split('|')[0];
                            string StartTime = FirstData.Split('|')[1].Substring(0, FirstData.Split('|')[1].Length);
                            string EndTime = ScendData.Split('|')[1].Substring(0, ScendData.Split('|')[1].Length);

                            ModSb.Append(Program + "|" + StartTime + "|" + EndTime + "|\n");
                        }
                    }

                    // 最後一筆資料結束時間為五點
                    string LastData = (string)ModDataList[ModDataList.Count - 1];
                    string LastDataProgram = LastData.Split('|')[0];
                    string LastDataStartTime = LastData.Split('|')[1].Substring(0, LastData.Split('|')[1].Length);
                    ModSb.Append(LastDataProgram + "|" + LastDataStartTime + "|" + LastDataStartTime.Substring(0, 10) + " 05:00:00.0" + "|\n");

                    // Mod檔案匯出
                    CreateModFile(ModSb.ToString());
                }
                */

                StringBuilder ChinaSb1 = new StringBuilder("");
                StringBuilder ChinaSb2 = new StringBuilder("");

                // 組成要寫入大陸一台檔案的資料內容
                if (ChinaChannel1DataList.Count != 0)
                {
                    // 檔案內容的日期
                    String DataDate = "";

                    // 組成要寫入MOD檔案的資料內容
                    for (int i = 0; i < ChinaChannel1DataList.Count; i++)
                    {
                        string Data = (string)ChinaChannel1DataList[i];

                        string Program = Data.Split('|')[0];
                        string Time = Data.Split('|')[1].Substring(0, Data.Split('|')[1].Length); // EX: 2014/01/06 00:00

                        if (i == 0)
                        {
                            DataDate = Time.Substring(0, 10);

                            ChinaSb1.Append(DataDate + "\n\n");
                            ChinaSb1.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                        }
                        else
                        {
                            if (!DataDate.Equals(Time.Substring(0, 10)))
                            {
                                DataDate = Time.Substring(0, 10);

                                ChinaSb1.Append("\n" + DataDate + "\n\n");
                                ChinaSb1.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                            }
                            else
                            {
                                ChinaSb1.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                            }
                        }
                           
                    }

                    // 大陸一台匯出
                    if (ChinaSb1.ToString() != "")
                    {
                        CreateChinaFile(ChinaSb1.ToString() ,"1");
                    }                    
                }

                // 組成要寫入大陸二台檔案的資料內容
                if (ChinaChannel2DataList.Count != 0)
                {
                    // 檔案內容的日期
                    String DataDate = "";

                    // 組成要寫入MOD檔案的資料內容
                    for (int i = 0; i < ChinaChannel2DataList.Count; i++)
                    {
                        string Data = (string)ChinaChannel2DataList[i];

                        string Program = Data.Split('|')[0];
                        string Time = Data.Split('|')[1].Substring(0, Data.Split('|')[1].Length); // EX: 2014/01/06 00:00

                        if (i == 0)
                        {
                            DataDate = Time.Substring(0, 10);

                            ChinaSb2.Append(DataDate + "\n\n");
                            ChinaSb2.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                        }
                        else
                        {
                            if (!DataDate.Equals(Time.Substring(0, 10)))
                            {
                                DataDate = Time.Substring(0, 10);

                                ChinaSb2.Append("\n" + DataDate + "\n\n");
                                ChinaSb2.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                            }
                            else
                            {
                                ChinaSb2.Append(Time.Substring(10).Trim() + "\t" + Program + "\n");
                            }
                        }

                    }

                    // 大陸二台匯出
                    if (ChinaSb2.ToString() != "")
                    {
                        CreateChinaFile(ChinaSb2.ToString(), "2");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                this.AlertMessage("EPG檔匯入失敗!");
                this.AlertMessage(ex.Message.ToString());

                this.logger.Error(ex.Message, ex);

                return false;
            }
        }               
        
    }

    /// <summary>
    /// 計算Duration並更新DB
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/11/28, Create
    /// </history>
    private void UpdateDuration()
    {
        try
        {
            DataTable dt = objEpgService.GetEpgTempCalendarDateList();

            if (dt.Rows.Count > 0)
            {
                double ts;
                string Duration = string.Empty;
                string HouseNo = string.Empty;
                DateTime CalendarDateNow;
                DateTime CalendarDateNext;

                for (int rowsIndex = 0; rowsIndex < dt.Rows.Count; rowsIndex++)
                {
                    HouseNo = dt.Rows[rowsIndex]["HouseNo"].ToString();
                    CalendarDateNow = Convert.ToDateTime(dt.Rows[rowsIndex]["CalendarDate"].ToString());

                    if (rowsIndex == dt.Rows.Count - 1)
                    {
                        CalendarDateNext = Convert.ToDateTime(dt.Rows[0]["CalendarDate"].ToString()).AddDays(1);
                    }
                    else
                        CalendarDateNext = Convert.ToDateTime(dt.Rows[rowsIndex+1]["CalendarDate"].ToString());

                    ts = CalendarDateNext.Subtract(CalendarDateNow).TotalMinutes;
                    //20131225 Modify by Tanya以分鐘計
                    Duration = ts.ToString();
                    //if (ts >= 60)
                    //{
                    //    int hour = Convert.ToInt16(Math.Floor(ts / 60));
                    //    double min = ts % 60;
                    //    Duration = hour.ToString().PadLeft(2, '0') + ':' + min.ToString().PadLeft(2, '0');
                    //}
                    //else
                    //    Duration = "00:" + ts.ToString().PadLeft(2, '0');

                    //Update Duration To Database
                    objEpgService.UpdateDurationWithHouseNo(HouseNo, Duration);                  
                }
            }
            
        }
        catch (Exception ex)
        {
            this.AlertMessage("EPG播映時間計算發生錯誤!");
            this.AlertMessage(ex.Message.ToString());

            this.logger.Error(ex.Message, ex);            
        }
    }

    ///// <summary>
    ///// 建立指定資料夾與檔案
    ///// </summary>
    ///// <history>
    ///// 1.Tanya Wu, 2012/9/20, Create
    ///// </history>
    //private bool CreateFile(string FileName)
    //{
    //    string appPath = Request.PhysicalApplicationPath;
    //    //string strExportFullDir = appPath + exportDir;
    //    StreamWriter sw;

    //    try
    //    {
    //        //檢查匯出檔是否存在
    //        string ExportFile = exportDir + "\\" + FileName + ".srt";
    //        if (!File.Exists(ExportFile))
    //        {
    //            //建立匯出檔
    //            sw = File.CreateText(ExportFile);
    //        }
    //        else
    //        {
    //            //先刪除再重新建立匯出檔
    //            File.Delete(ExportFile);
    //            sw = File.CreateText(ExportFile);
    //        }

    //        //關閉StreamWriter
    //        sw.Close();
    //        sw.Dispose();

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {           
    //        this.logger.Error(ex.Message, ex);
    //        return false;
    //    }
    //}

    private bool CreateModFile(string txtValue)
    {
        string modDir = "D:\\MOD-Watch\\";
        StreamWriter sw;

        try
        {
            //檢查匯出檔是否存在
            string ExportFile = modDir + "3.txt";
            if (!File.Exists(ExportFile))
            {
                //建立匯出檔
                sw = File.CreateText(ExportFile);
            }
            else
            {
                //先刪除再重新建立匯出檔
                File.Delete(ExportFile);
                sw = File.CreateText(ExportFile);
            }

            sw.WriteLine(txtValue); 
            //關閉StreamWriter
            sw.Close();
            sw.Dispose();

            return true;
        }
        catch (Exception ex)
        {           
            this.logger.Error(ex.Message, ex);
            return false;
        }
    }

    private bool CreateChinaFile(string txtValue, string channel)
    {
        string dir = "D:\\MOD-Watch\\";
        StreamWriter sw;

        try
        {
            //檢查匯出檔是否存在
            string ExportFile = dir + "goodtv" + channel + ".txt";
            if (!File.Exists(ExportFile))
            {
                //建立匯出檔
                sw = File.CreateText(ExportFile);
            }
            else
            {
                //先刪除再重新建立匯出檔
                File.Delete(ExportFile);
                sw = File.CreateText(ExportFile);
            }

            sw.WriteLine(txtValue);
            //關閉StreamWriter
            sw.Close();
            sw.Dispose();

            return true;
        }
        catch (Exception ex)
        {
            this.logger.Error(ex.Message, ex);
            return false;
        }
    }
    #endregion
}