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
using Core.Domain;
using Core.Service;
//引用Microsoft Excel相關參考
//using Microsoft.Office.Interop;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using Core.Utility;
using System.Data.SqlClient;
using System.Globalization;

public partial class UploadCUE : BasePage
{
    SrtmcsCueService objCueService = new SrtmcsCueService(); 
    //string exportDir = "\\Files\\srt\\";    
    string exportDir = ConfigurationManager.AppSettings["srtPath"].ToString();
    /*** Excel Interop reference ***/
    //Excel.Application xlApp = null;
    //Excel.Workbook wb = null;
    //Excel.Worksheet ws = null;
    //Excel.Range aRange = null;
    //*******************************/

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
    /// 1.Tanya Wu, 2013/10/30, Create    
    /// 2.Tanya Wu, 2013/11/27, Modify:匯入檔修改
    /// 3.Tanya Wu, 2014/01/06, Modify:匯入時，先update err date再del old cue data
    /// </history>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string ImportFileName = string.Empty;
        string ImportSource = string.Empty;
        string appPath = Request.PhysicalApplicationPath;
        string saveDir = "\\Files\\Upload\\";
        bool InsertCue_ok = false;

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

                    //2.讀取Excel檔&Insert into srtqc_epgTemp
                    //if (this.xlApp == null)
                    //{
                    //    this.xlApp = new Excel.Application();
                    //}

                    //2.1 打開Server上的Excel檔案
                    //this.xlApp.Workbooks.Open(ImportSource, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //this.wb = xlApp.Workbooks[1];//第一個Workbook 
                    //this.wb.Save();

                    ////2.讀取Excel檔&Insert into epgTemp  
                    //InsertToTempTable(ImportSource)
                    //2.2 從第一個Worksheet讀資料
                    //if (InsertToTempTable(ImportSource, (Excel.Worksheet)xlApp.Worksheets[1]))
                    if (InsertToTempTable(ImportSource))
                    {

                        //3.transfer to epg from epgTemp
                        //3.1 Update cueTemp Error Date
                        objCueService.UpdateCueTempErrorDate();                        

                        //3.2 Delete cue Data
                        objCueService.DelCueOldData();

                        //3.3 Insert epg from epgTemp
                        objCueService.InsertCueFromTemp();

                        //4.已上傳字幕(PlanningTitle&集數)轉HourseNo.srt
                        //4.1 取得本次匯入資料且srt資料已存在清單
                        DataTable dt = objCueService.GetSrtExportList();

                        if (dt.Rows.Count > 0)
                        {
                            //4.2 匯出srt                            
                            for (int rowsIndex = 0; rowsIndex < dt.Rows.Count; rowsIndex++)
                            {
                                if (CreateFile(dt.Rows[rowsIndex]["HouseNo"].ToString()))
                                {
                                    StreamWriter sw = new StreamWriter(exportDir + "\\" + dt.Rows[rowsIndex]["HouseNo"].ToString() + ".srt", true, System.Text.Encoding.UTF8);

                                    sw.Write(dt.Rows[rowsIndex]["_ST043"].ToString());

                                    //關閉StreamWriter
                                    sw.Close();
                                    sw.Dispose();
                                }
                            }
                        }

                        InsertCue_ok = true;

                    }
                    else
                    {
                        if (strErrList != "")
                            this.AlertMessage("匯入檔案內容有誤！\n請參考以下時段作檢查：\n" + strErrList);
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
            this.AlertMessage("CUE檔匯入失敗!");
            this.AlertMessage(ex.Message.ToString());

            this.logger.Error(ex.Message, ex);
        }
        //finally
        //{
            //if (this.xlApp != null)
            //{
            //    //釋放所有記憶體                                      
            //    this.aRange = null;
            //    //this.ws = null;            
            //    this.wb = null;
            //
            //    this.xlApp.Quit();
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            //
            //    this.xlApp = null;
            //}            

            //GC.Collect();           
        //}
        if (InsertCue_ok)
        {

            //try
            //{

                // 2014/9/2 匯入節目分集基本檔 ProgramEpisode
                InsertProgramEpisodeFromCUETemp();

                Response.Write("<script>confirm('CUE檔匯入完成! 是否要導向查詢頁面?')? window.location.replace('SRTManagementSystemCUE.aspx'):document.write('')</script>");

            //}
            //catch (Exception ex)
            //{
            //    this.AlertMessage("CUE檔匯入成功！\n但更新節目分集失敗！");
                //this.AlertMessage(ex.Message.ToString());

            //    this.logger.Error(ex.Message, ex);
            //}
        }

    }

    private void InsertProgramEpisodeFromCUETemp()
    {

        DBProvider dbProvider = new DBProvider();
        SqlCommand cmdMaxPart = new SqlCommand();
        cmdMaxPart.CommandType = CommandType.Text;
        //cmd.CommandType = CommandType.StoredProcedure;
        DataTable dtMaxPart = null;

        string StrSQLMaxPart = @"
                SELECT MAX(cast([PartNo] as int)) as [MaxPart] FROM [srtmcs_cueTemp]
                WHERE ISNULL([HouseNo],'') <> '' AND ISNULL([ProgramTitle],'') <> ''
                AND ISNULL([SOM],'') <> '' AND ISNULL([EOM],'') <> ''
                AND CAST([PartNo] as int) > 0
                ";
        try
        {
            dtMaxPart = dbProvider.OpenDataTable(StrSQLMaxPart, cmdMaxPart);
        }
        catch (Exception ex)
        {
            //this.AlertMessage(ex.Message.ToString());
            this.logger.Error(ex.Message, ex);
        }

        // 來源沒有資料時，不作處理
        if (dtMaxPart.Rows.Count == 0) { return; }
        // 取得來源資料最大的段數
        int intMaxPart = (int)dtMaxPart.Rows[0]["MaxPart"];

        for (int k = 0; k < intMaxPart; k++)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaxPart", SqlDbType.Int).Value = k + 1;
            DataTable dtcue = null;
            string StrSQL = @"
                SELECT DISTINCT p.PartLength,c.*
                FROM [ProgramEpisode] as p
                JOIN (SELECT * FROM (SELECT 
                       rtrim(substring([ProgramTitle],1,7)) as ProgramCode
                      ,cast([EpisodeNumber] as int) as Episode
                      ,cast([PartNo] as int) as PartNo
                      ,[Duration]
	                  ,[SOM]+';'+[EOM]+';'+[Duration] as NewPartLength
	                  ,ROW_NUMBER() OVER(PARTITION BY rtrim(substring([ProgramTitle],1,7)),cast([EpisodeNumber] as int),cast([PartNo] as int) 
					   ORDER BY [PresentationSequenceNo] desc) as ROWNO
                  FROM [pms].[dbo].[srtmcs_cueTemp]
                  WHERE ISNULL([HouseNo],'') <> ''
                  AND ISNULL([ProgramTitle],'') <> ''
                  AND ISNULL([SOM],'') <> '' AND ISNULL([EOM],'') <> ''
			      AND cast([PartNo] as int) = @MaxPart
                ) AS c WHERE ROWNO = 1 ) as c
                on p.ProgramCode = c.ProgramCode
                and p.Episode = c.Episode
                ORDER BY c.ProgramCode,c.Episode ;
                ";

            try
            {
                //dbProvider.ExecuteNonQuery("InsertProgramEpisodeFromCUETemp", cmd);
                dtcue = dbProvider.OpenDataTable(StrSQL, cmd);
            }
            catch (Exception ex)
            {
                //this.AlertMessage("CUE檔匯入成功！\n但讀取節目分集資料失敗！");
                this.logger.Error(ex.Message, ex);
                //this.AlertMessage(ex.Message.ToString());
            }

            for (int i = 0; i < dtcue.Rows.Count; i++)
            {

                SqlCommand cmdcue = new SqlCommand();
                cmdcue.CommandType = CommandType.Text;
                string StrUpdateSQL = @"
                UPDATE [ProgramEpisode]
                SET ModifyUser='web', ModifyDatetime=getdate()
                    ,TotalLength=@TotalLength, PartLength=@PartLength
                WHERE ProgramCode=@ProgramCode and Episode=@Episode ;
            ";
                string strPartLength = dtcue.Rows[i]["PartLength"].ToString();
                string strProgramCode = dtcue.Rows[i]["ProgramCode"].ToString();
                int intEpisode = (int)dtcue.Rows[i]["Episode"];
                int intPartNo = (int)dtcue.Rows[i]["PartNo"];
                string strDuration = dtcue.Rows[i]["Duration"].ToString();
                string strNewPartLength = dtcue.Rows[i]["NewPartLength"].ToString();
                int section_cut = strPartLength.Length == 0 ? 0 : strPartLength.Split('@').Length - 1;
                String[] sections = strPartLength.Split('@');
                String[] newSections = new String[section_cut > intPartNo ? section_cut : intPartNo];
                string strsections = "";

                try
                {

                    if ((intPartNo - 1) > section_cut)
                    {
                        // 目的的分段長度小於來源的分段長度時，不作update
                    }
                    else if ((intPartNo == 1) && (section_cut == 0 || section_cut == 1))
                    {
                        // 目的資料的長度無資料，直接update，但限於PartNo=1，如果大於1 就不update。
                        cmdcue.Parameters.Add("@TotalLength", SqlDbType.NVarChar).Value = strDuration;
                        cmdcue.Parameters.Add("@PartLength", SqlDbType.NVarChar).Value = strNewPartLength + '@';
                        cmdcue.Parameters.Add("@ProgramCode", SqlDbType.NVarChar).Value = strProgramCode;
                        cmdcue.Parameters.Add("@Episode", SqlDbType.Int).Value = intEpisode;
                        dbProvider.ExecuteNonQuery(StrUpdateSQL, cmdcue);

                    }
                    else
                    {

                        for (int j = 0; j < newSections.Length; j++)
                        {
                            if (intPartNo == j + 1)
                            {
                                newSections[j] = strNewPartLength;
                                strsections += strNewPartLength + '@';
                            }
                            else
                            {
                                newSections[j] = sections[j];
                                strsections += sections[j] + '@';
                            }
                        }

                        // update目的資料
                        cmdcue.Parameters.Add("@TotalLength", SqlDbType.NVarChar).Value = summaryTotalBySections(newSections);
                        cmdcue.Parameters.Add("@PartLength", SqlDbType.NVarChar).Value = strsections;
                        cmdcue.Parameters.Add("@ProgramCode", SqlDbType.NVarChar).Value = strProgramCode;
                        cmdcue.Parameters.Add("@Episode", SqlDbType.Int).Value = intEpisode;
                        dbProvider.ExecuteNonQuery(StrUpdateSQL, cmdcue);

                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex.Message, ex);
                    //this.AlertMessage(ex.Message.ToString());
                }

            } // Update DB for loop


        } // MaxPart for loop

    }

    public static String summaryTotalBySections(String[] sections)
    {

        if (null == sections || sections.Length == 0)
        {
            return "";
        }

        long totalSeconds = 0;
        String totalTime = "";

        foreach (String section in sections)
        {
            String[] timeStr = section.Split(';');
            if (timeStr.Length != 3) return "";
            String length = timeStr[2];
            long seconds = getSeconds(length);

            totalSeconds += seconds;
        }

        totalTime = getStringFromSeconds(totalSeconds);

        return totalTime;

    }

    private static long getSeconds(String timeStr)
    {

        if (timeStr == "") {
            return 0;
        }

        String[] timeFields = timeStr.Split(':');
        if (timeFields.Length != 4) return 0;
        String hour = timeFields[0];
        String minute = timeFields[1];
        String second = timeFields[2];

        long seconds = Convert.ToInt64(second) + 60 * Convert.ToInt64(minute) + 3600 * Convert.ToInt64(hour);

        return seconds;

    }

    private static String getStringFromSeconds(long seconds)
    {

        int hour = 0;
        int minute = 0;
        int second = 0;

        while (60 <= seconds)
        {
            if (3600 <= seconds)
            {
                seconds -= (3600);
                hour++;
            }
            else if (60 <= seconds)
            {
                seconds -= (60);
                minute++;
            }
        }

        second = Convert.ToUInt16(seconds);
        String timeStr = String.Format("{0:00}:{1:00}:{2:00}:00", hour, minute, second);

        return timeStr;

    }

    protected void btnQueryCUE_Click(object sender, EventArgs e)
    {
        // 導向CUE查詢頁面
        Response.Redirect("SRTManagementSystemCUE.aspx");
    }
    #endregion

    #region Other Method
    /// <summary>
    /// 把Excel資料Insert into Table
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/10/30, Create
    /// 2.Tanya Wu, 2013/11/27, Modify:匯入檔欄位變更
    /// </history>
    //private bool InsertToTempTable(string excel_filename, Excel.Worksheet ws)
    private bool InsertToTempTable(string excel_filename)
    {
        SrtmcsCUETemp objSrtCUETemp = new SrtmcsCUETemp();
        //**********************************************************************//        
        string ProviderName = "Microsoft.Jet.OLEDB.4.0;";
        string ExtendedString = "'Excel 8.0;";
        string Hdr = "NO;";
        string IMEX = "1';";
        //**********************************************************************//        

        string cs = "Data Source=" + excel_filename + ";" +
            "Provider=" + ProviderName +
            "Extended Properties=" + ExtendedString +
            "HDR=" + Hdr +
            "IMEX=" + IMEX;

        //要開始讀取的起始列(微軟Worksheet是從1開始算) - 無標題
        //int rowIndex = 1;
        OleDbConnection cn = new OleDbConnection(cs);

        try
        {
     
            cn.Open();
            string qs = "Select * From [Sheet1$]";

            //Delete SrtCUETemp Data
            objCueService.DelCueTemp();

            //取得一列的範圍
            //this.aRange = ws.get_Range("A" + rowIndex.ToString(), "T" + rowIndex.ToString());

            OleDbCommand cmd = new OleDbCommand(qs, cn);
            OleDbDataReader dr = cmd.ExecuteReader();

            //if (ws.UsedRange.Rows.Count > 0)
            //{
            int i = 0;
                //for (int i = 1; i <= ws.UsedRange.Rows.Count; i++)
                while (dr.Read())
                {
                    i++;

                    //判斷每列Row第1格有值的話，才Insert
                    //if (((object[,])this.aRange.Value2)[1, 1] != null)

                    //2014/9/30 增加匯入前檢查錯誤
                    //判斷每列Row第1格(A欄位)有值與第8格(H欄位)有值，才Insert
                    if (dr[0].ToString().Trim() != "" && dr[7].ToString().Trim() != "")
                    {
                        // 2014/9/30 增加匯入前檢查錯誤
                        string strErrLine = "";
                        //Time A欄位,0
                        if (IsTime(dr[0].ToString().Trim()))
                        {
                            objSrtCUETemp.Time = dr[0].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.Time = "";
                            strErrLine += "(A" + i + "儲存格)時間格式有誤，";
                        }
                        //Fix Time B欄位,1
                        if (dr[1].ToString().Trim() != "")
                        {
                            if (dr[1].ToString().Trim() == "真" || dr[1].ToString().Trim().ToUpper() == "TRUE")
                                objSrtCUETemp.FixTime = "True";
                            else if (dr[1].ToString().Trim() == "假" || dr[1].ToString().Trim().ToUpper() == "FALSE")
                                objSrtCUETemp.FixTime = "False";
                            else
                                objSrtCUETemp.FixTime = dr[1].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.FixTime = "";
                        }
                        //Presentation Title C欄位,2
                        if (dr[2].ToString().Trim() != "")
                        {
                            objSrtCUETemp.PresentationTitle = dr[2].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.PresentationTitle = "";
                            strErrLine += "(C" + i + "儲存格)節目(短片)代號與名稱不能為空白，";
                        }
                        //Episode Number D欄位,3
                        if (dr[3].ToString().Trim() != "")
                        {
                            objSrtCUETemp.EpisodeNumber = dr[3].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.EpisodeNumber = "";
                            if (dr[7].ToString().Trim().Length == 11)
                                strErrLine += "(D" + i + "儲存格)集數不能為空白，";
                        }
                        //Part No E欄位,4
                        if (dr[4].ToString().Trim() != "")
                        {
                            objSrtCUETemp.PartNo = dr[4].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.PartNo = "";
                            if (dr[7].ToString().Trim().Length == 11)
                                strErrLine += "(E" + i + "儲存格)段數不能為空白，";
                        }
                        //Duration F欄位,5
                        if (dr[5].ToString().Trim() != "")
                        {
                            if (IsTime(dr[5].ToString().Trim()))
                            {
                                objSrtCUETemp.Duration = dr[5].ToString().Trim();
                            }
                            else
                            {
                                objSrtCUETemp.Duration = "";
                                strErrLine += "(F" + i + "儲存格)播映長度格式有誤，";
                            }
                        }
                        else
                        {
                            objSrtCUETemp.Duration = "";
                            strErrLine += "(F" + i + "儲存格)長度不能為空白，";
                        }
                        //Episode Title G欄位,6
                        objSrtCUETemp.EpisodeTitle = dr[6].ToString().Trim() != "" ? dr[6].ToString().Trim() : "";
                        //House No H欄位,7
                        objSrtCUETemp.HouseNo = dr[7].ToString().Trim() != "" ? dr[7].ToString().Trim() : "";
                        //Media No I欄位,8	
                        objSrtCUETemp.MediaNo = dr[8].ToString().Trim() != "" ? dr[8].ToString().Trim() : "";
                        //Program Title	J欄位,9
                        if (dr[9].ToString().Trim() != "")
                        {
                            objSrtCUETemp.ProgramTitle = dr[9].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.ProgramTitle = "";
                            if (dr[7].ToString().Trim().Length == 11)
                                strErrLine += "(J" + i + "儲存格)ProgramTitle不能為空白，";
                        }
                        //Promotion Title K欄位,10
                        if (dr[10].ToString().Trim() != "")
                        {
                            objSrtCUETemp.PromotionTitle = dr[10].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.PromotionTitle = "";
                            if (dr[7].ToString().Trim().Length == 6)
                                strErrLine += "(K" + i + "儲存格)PromotionTitle不能為空白，";
                        }

                        //Channel L欄位,11
                        if (dr[11].ToString().Trim() != "")
                        {
                            objSrtCUETemp.Channel = dr[11].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.Channel = "";
                            strErrLine += "(L" + i + "儲存格)Channel不能為空白，";
                        }
                        //Date M欄位,12
                        //objSrtCUETemp.Date = DateTime.FromOADate(double.Parse(dr[12].ToString()));
                        //objSrtCUETemp.Date = Convert.ToDateTime(dr[12].ToString());

                        string[] formats = { "d/M/yyyy", "d/M/yyyy HH:mm", "dd/MM/yyyy", "dd/MM/yyyy HH:mm" ,
                                                "yyyy/M/d", "yyyy/M/d HH:mm", "yyyy/MM/dd", "yyyy/MM/dd HH:mm" ,
                                                "yyyy-M-d", "yyyy-M-d HH:mm", "yyyy-MM-dd", "yyyy-MM-dd HH:mm"
                                            };
                        DateTime dtDate;
                        if (DateTime.TryParseExact(dr[12].ToString(), formats, null, DateTimeStyles.None, out dtDate))
                        {
                            objSrtCUETemp.Date = dtDate;
                        }
                        else
                        {
                            objSrtCUETemp.Date = DateTime.Today;
                            strErrList += "(M" + i + "儲存格)日期有誤，";
                        }
                        //objSrtCUETemp.Date = Convert.ToDateTime(dr[12].ToString());
                        //Comment N欄位,13
                        objSrtCUETemp.Comment = dr[13].ToString().Trim() != "" ? dr[13].ToString().Trim() : "";
                        //Type O欄位,14
                        if (dr[14].ToString().Trim() != "")
                        {
                            objSrtCUETemp.Type = dr[14].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.Type = "";
                            if (dr[7].ToString().Trim().Length == 11)
                                strErrLine += "(O" + i + "儲存格)字幕註記不能為空白，";
                        }
                        //Repeat P欄位,15
                        if (dr[15].ToString().Trim() != "")
                        {
                            if (dr[15].ToString().Trim() == "真" || dr[15].ToString().Trim().ToUpper() == "TRUE")
                                objSrtCUETemp.Repeat = "True";
                            else if (dr[15].ToString().Trim() == "假" || dr[15].ToString().Trim().ToUpper() == "FALSE")
                                objSrtCUETemp.Repeat = "False";
                            else
                            {
                                objSrtCUETemp.Repeat = dr[15].ToString().Trim();
                                if (dr[7].ToString().Trim().Length == 11)
                                    strErrLine += "(P" + i + "儲存格)首播/重播需為是否值，";
                            }
                        }
                        else
                        {
                            objSrtCUETemp.Repeat = "";
                            if (dr[7].ToString().Trim().Length == 11)
                                strErrLine += "(P" + i + "儲存格)首播/重播不能為空白，";
                        }
                        //SOM Q欄位,16
                        if (dr[16].ToString().Trim() != "")
                        {
                            if (IsTime(dr[16].ToString().Trim()))
                            {
                                objSrtCUETemp.SOM = dr[16].ToString().Trim();
                            }
                            else
                            {
                                objSrtCUETemp.SOM = "";
                                strErrList += "(Q" + i + "儲存格)SOM格式有誤，";
                            }
                        }
                        else
                        {
                            objSrtCUETemp.SOM = "";
                            if (dr[7].ToString().Trim() != "")
                                strErrList += "(Q" + i + "儲存格)SOM不能為空白，";
                        }
                        //EOM R欄位,17
                        if (dr[17].ToString().Trim() != "")
                        {
                            if (IsTime(dr[17].ToString().Trim()))
                            {
                                objSrtCUETemp.EOM = dr[17].ToString().Trim();
                            }
                            else
                            {
                                objSrtCUETemp.EOM = "";
                                if (dr[7].ToString().Trim() != "")
                                    strErrList += "(R" + i + "儲存格)EOM格式有誤，";
                            }
                        }
                        else
                        {
                            objSrtCUETemp.EOM = "";
                            if (dr[7].ToString().Trim() != "")
                                strErrList += "(R" + i + "儲存格)EOM不能為空白，";
                        }
                        //Classification S欄位,18
                        if (dr[18].ToString().Trim() != "")
                        {
                            objSrtCUETemp.Classification = dr[18].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.Classification = "";
                            strErrLine += "(S" + i + "儲存格)Classification不能為空白，";
                        }
                        //Presentation Sequence No T欄位,19
                        if (dr[19].ToString().Trim() != "")
                        {
                            objSrtCUETemp.PresentationSequenceNo = dr[19].ToString().Trim();
                        }
                        else
                        {
                            objSrtCUETemp.PresentationSequenceNo = "";
                            strErrLine += "(T" + i + "儲存格)Excel檔內排序編號不能為空白，";
                        }

                        /*
                        //Time
                        objSrtCUETemp.Time = ((object[,])this.aRange.Value2)[1, 1] != null ? ((object[,])this.aRange.Value2)[1, 1].ToString() : "";
                        //Fix Time
                        objSrtCUETemp.FixTime = ((object[,])this.aRange.Value2)[1, 2] != null ? ((object[,])this.aRange.Value2)[1, 2].ToString() : "";
                        //Presentation Title
                        objSrtCUETemp.PresentationTitle = ((object[,])this.aRange.Value2)[1, 3] != null ? ((object[,])this.aRange.Value2)[1, 3].ToString() : "";
                        //Episode Number
                        objSrtCUETemp.EpisodeNumber = ((object[,])this.aRange.Value2)[1, 4] != null ? ((object[,])this.aRange.Value2)[1, 4].ToString() : "";
                        //Part No
                        objSrtCUETemp.PartNo = ((object[,])this.aRange.Value2)[1, 5] != null ? ((object[,])this.aRange.Value2)[1, 5].ToString() : "";
                        //Duration
                        objSrtCUETemp.Duration = ((object[,])this.aRange.Value2)[1, 6] != null ? ((object[,])this.aRange.Value2)[1, 6].ToString() : "";
                        //Episode Title
                        objSrtCUETemp.EpisodeTitle = ((object[,])this.aRange.Value2)[1, 7] != null ? ((object[,])this.aRange.Value2)[1, 7].ToString() : "";
                        //House No
                        objSrtCUETemp.HouseNo = ((object[,])this.aRange.Value2)[1, 8] != null ? ((object[,])this.aRange.Value2)[1, 8].ToString() : "";
                        //Media No	
                        objSrtCUETemp.MediaNo = ((object[,])this.aRange.Value2)[1, 9] != null ? ((object[,])this.aRange.Value2)[1, 9].ToString() : "";
                        //Program Title	
                        objSrtCUETemp.ProgramTitle = ((object[,])this.aRange.Value2)[1, 10] != null ? ((object[,])this.aRange.Value2)[1, 10].ToString() : "";
                        //Promotion Title
                        objSrtCUETemp.PromotionTitle = ((object[,])this.aRange.Value2)[1, 11] != null ? ((object[,])this.aRange.Value2)[1, 11].ToString() : "";
                        //Channel
                        objSrtCUETemp.Channel = ((object[,])this.aRange.Value2)[1, 12] != null ? ((object[,])this.aRange.Value2)[1, 12].ToString() : "";
                        //Date
                        objSrtCUETemp.Date = DateTime.FromOADate(double.Parse(((object[,])this.aRange.Value2)[1, 13].ToString()));
                        //Comment
                        objSrtCUETemp.Comment = ((object[,])this.aRange.Value2)[1, 14] != null ? ((object[,])this.aRange.Value2)[1, 14].ToString() : "";
                        //Type
                        objSrtCUETemp.Type = ((object[,])this.aRange.Value2)[1, 15] != null ? ((object[,])this.aRange.Value2)[1, 15].ToString() : "";
                        //Repeat
                        objSrtCUETemp.Repeat = ((object[,])this.aRange.Value2)[1, 16] != null ? ((object[,])this.aRange.Value2)[1, 16].ToString() : "";
                        //SOM
                        objSrtCUETemp.SOM = ((object[,])this.aRange.Value2)[1, 17] != null ? ((object[,])this.aRange.Value2)[1, 17].ToString() : "";
                        //EOM
                        objSrtCUETemp.EOM = ((object[,])this.aRange.Value2)[1, 18] != null ? ((object[,])this.aRange.Value2)[1, 18].ToString() : "";
                        //Classification
                        objSrtCUETemp.Classification = ((object[,])this.aRange.Value2)[1, 19] != null ? ((object[,])this.aRange.Value2)[1, 19].ToString() : "";
                        //Presentation Sequence No
                        objSrtCUETemp.PresentationSequenceNo = ((object[,])this.aRange.Value2)[1, 20] != null ? ((object[,])this.aRange.Value2)[1, 20].ToString() : "";
                        */

                        // 2014/8/5 檢查錯誤與無效資料
                        /* 修改為匯入前檢查每個欄位
                        if (!IsTime(objSrtCUETemp.Time))
                        {
                            strErrList += "第 " + i + " 筆時間(A" + i + "儲存格)有誤\n";
                        }
                        else if (objSrtCUETemp.PresentationTitle == "" || objSrtCUETemp.Channel == "" || !IsTime(objSrtCUETemp.Duration) ||
                            objSrtCUETemp.PresentationSequenceNo == "")
                        {

                            strErrList += objSrtCUETemp.Time + "\n";

                        }
                        else if (objSrtCUETemp.HouseNo.Trim().Length == 11)
                        {
                            if (!IsTime(objSrtCUETemp.SOM) || !IsTime(objSrtCUETemp.EOM) ||
                            objSrtCUETemp.Classification == "" || objSrtCUETemp.EpisodeNumber == "" || objSrtCUETemp.PartNo == "" || 
                            objSrtCUETemp.EpisodeTitle == "" || objSrtCUETemp.ProgramTitle == "" || objSrtCUETemp.Type == "" || 
                            objSrtCUETemp.Repeat == "")
                            {
                                strErrList += objSrtCUETemp.Time + "\n";
                            }
                        }
                        else if (objSrtCUETemp.HouseNo.Trim().Length == 6)
                        {
                            if (!IsTime(objSrtCUETemp.SOM) || !IsTime(objSrtCUETemp.EOM) ||
                            objSrtCUETemp.Classification == "" || objSrtCUETemp.PromotionTitle == "")
                            {
                                strErrList += objSrtCUETemp.Time + "\n";
                            }
                        }
                        */
                        if (strErrLine!="")
                            strErrList += "第 " + i + " 筆的"+ strErrLine+ "\n";

                        //Insert into Table srtqc_cueTemp
                        objCueService.InsertCueTemp(objSrtCUETemp);
                    }

                    //往下抓一列Excel範圍
                    //rowIndex++;
                    //this.aRange = ws.get_Range("A" + rowIndex.ToString(), "T" + rowIndex.ToString());
                }

                // 2014/8/5 檢查完後判斷尚有缺少資料欄位既中斷執行
                if (strErrList != "") return false;
                //return false;
                return true;
            //}
            //else
            //{
            //    this.AlertMessage("匯入檔案無資料!");
            //    return false;
            //}

        }
        catch (Exception ex)
        {

            cn.Close();
            //this.AlertMessage("Excel日期欄位有誤!\n請檢查整排M欄位的日期是否有誤！");
            //this.AlertMessage("CUE檔匯入失敗!");
            this.AlertMessage(ex.Message.ToString());

            this.logger.Error(ex.Message, ex);

            return false;
        }
        finally
        {
            cn.Close();
            //關閉Server上的Excel檔案
            //this.wb.Close(false, Type.Missing, Type.Missing);
            //this.xlApp.Workbooks.Close();
        }
    }

    /// <summary>
    /// 判斷是否為時間
    /// </summary>
    /// <param name="pTime">傳入字串。</param>
    public static bool IsTime(string pTime)
    {
        return Regex.IsMatch(pTime, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d:[0-5]?\d)$");
    }

    /// <summary>
    /// 建立指定資料夾與檔案
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/9/20, Create
    /// </history>
    private bool CreateFile(string FileName)
    {
        string appPath = Request.PhysicalApplicationPath;
        //string strExportFullDir = appPath + exportDir;
        StreamWriter sw;

        try
        {
            //檢查匯出檔是否存在
            string ExportFile = exportDir + "\\" + FileName + ".srt";
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
