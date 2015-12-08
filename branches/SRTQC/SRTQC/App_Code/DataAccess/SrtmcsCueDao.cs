using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DataAccess
{
    public class SrtmcsCueDao
    {
        
        #region Select
        /// <summary>
        /// Get Cue查詢結果清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/10/11, Create
        /// </history>
        public String GetCueoList(string startDate, string endDate, string channel, string _TM001, string episodeNumber, string houseNo, string premier, string planningName, string calendarTime, string calendarTime2)
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT CUE.* ");
            strSQL.Append("       ,CASE WHEN EPG.[Premier] = 1 THEN '首' WHEN EPG.[Repeat] = 1 THEN '重' ELSE '' END PremierRepeat ");
            strSQL.Append("       ,SRT.[_MTIME],SRT.[_ST041],SRT.[_ST043] ");
            strSQL.Append("       ,[dbo].[getProgramName](CUE.[PresentationTitleID]) [PresentationTitleName] ");
            strSQL.Append(" FROM ");
            strSQL.Append(" (SELECT [Channel] ");
            strSQL.Append("         ,[Date] ");
            strSQL.Append("         ,CONVERT(varchar(12), [Date], 111) AS [CalendarDate] ");
            strSQL.Append("         ,[Time] ");
            strSQL.Append("         ,[HouseNo] ");
            strSQL.Append("         ,CASE WHEN ISNUMERIC(SUBSTRING([PresentationTitle],2,1)) = 1  THEN SUBSTRING([PresentationTitle],0,8) ");
            strSQL.Append("               ELSE SUBSTRING([PresentationTitle],0,9) END  [PresentationTitleID] ");           
            strSQL.Append("         ,[PartNo] ");
            strSQL.Append("         ,[EpisodeNumber] ");
            strSQL.Append("         ,[Duration] ");
            strSQL.Append("         ,Case When Len(IsNull([Comment],''))> 0 ");
            strSQL.Append("               Then (Case When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                          When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='N' Then 'N' ");
            strSQL.Append("                          When Substring([Type],1,1)='N' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                          When Substring([Type],1,1)='N' And Substring([Comment],1,1)='N' Then 'N' ");
            strSQL.Append("                          When IsNull(EpisodeNumber,'') <> '' Then '資料錯誤!!!' Else '' End) ");
            strSQL.Append("          Else (Case When Substring([Type],1,1)='Y' Then 'Y' ");
            strSQL.Append("                     When Substring([Type],1,1)='N' Then 'N' ");
            strSQL.Append("                     When IsNull(EpisodeNumber,'') <> '' Then '資料錯誤!!!' Else '' End) End [TypeCommnet] ");
            strSQL.Append("         ,[PresentationSequenceNo] ");
            strSQL.Append("  FROM [pms].[dbo].[srtmcs_cue] ) AS CUE ");
            strSQL.Append(" LEFT JOIN [pms].[dbo].[srtmcs_epg] AS EPG ");
            strSQL.Append(" ON CUE.[HouseNo] =  EPG.[HouseNo] AND CAST(CUE.[Date] AS date) = CAST(EPG.[CalendarDate] AS date) ");
            strSQL.Append("    AND CUE.[Channel] = EPG.[Channel] AND CAST(Substring(CUE.[Time],1,2) AS int) = DATEPART(HOUR, EPG.[CalendarDate]) ");
            strSQL.Append("    AND CAST(Substring(CUE.[Time],4,2) AS int) = DATEPART(MINUTE, EPG.[CalendarDate]) ");
            strSQL.Append(" LEFT JOIN [pms].[dbo].[_ST03P1] AS SRT ");
            strSQL.Append(" ON SRT.[_ST030] = CUE.PresentationTitleID AND SRT.[_ST031] = CUE.EpisodeNumber ");
            strSQL.Append(" WHERE 1 = 1");

            // 開始日期
            if (!string.IsNullOrEmpty(startDate) && startDate != "")
                strSQL.Append(" AND CUE.[Date] >= @startDate ");
            // 結束日期
            if (!string.IsNullOrEmpty(endDate) && endDate != "")
                strSQL.Append(" AND CUE.[Date] <= @endDate ");
            // 頻道別
            if (!string.IsNullOrEmpty(channel) && channel != "")
                strSQL.Append(" AND CUE.[Channel] LIKE '%' + @channel + '%' ");
            // 節目/短片代碼
            if (!string.IsNullOrEmpty(_TM001) && _TM001 != "")
                strSQL.Append(" AND CUE.[PresentationTitleID] LIKE '%' + @presentationTitle + '%' ");
            // 集數
            //20140113 Modify by Tanya:修改集數為精準查詢
            if (!string.IsNullOrEmpty(episodeNumber) && episodeNumber != "")
                strSQL.Append(" AND CUE.[EpisodeNumber] = @episodeNumber ");
            // HouseNumber
            if (!string.IsNullOrEmpty(houseNo) && houseNo != "")
                strSQL.Append(" AND CUE.[HouseNo]  LIKE '%' + @houseNo + '%' ");
            // Premier
            if (!string.IsNullOrEmpty(premier) && premier != "")
                strSQL.Append(" AND EPG.[Premier] = @premier ");
            // 2015/5/19 節目名稱、播出時間查詢條件
            //節目名稱
            if (!string.IsNullOrEmpty(planningName) && planningName != "")
                strSQL.Append(" AND EPG.[PlanningTitle] LIKE '%' + @PlanningName + '%' ");
            //播出時間
            if (!string.IsNullOrEmpty(calendarTime) && calendarTime != "")
                strSQL.Append(" AND SUBSTRING(CUE.[Time],1,5) >= @CalendarTime ");
            //結束時間
            if (!string.IsNullOrEmpty(calendarTime2) && calendarTime2 != "")
                strSQL.Append(" AND SUBSTRING(CUE.[Time],1,5) <= @CalendarTime2 ");
            strSQL.Append(" ORDER BY CUE.[Channel],CUE.[CalendarDate],CUE.[Time] ");

            return strSQL.ToString();
        }

        /// <summary>
        /// Get SRT匯出清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/10/31, Create
        /// </history>
        public String GetSrtExportList()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT CUE.HouseNo ,ST._ST043,TypeCommnet ");
            strSQL.Append(" FROM dbo._ST03P1 ST ");
            strSQL.Append(" INNER JOIN ");
            strSQL.Append("     (SELECT CASE WHEN ISNUMERIC(SUBSTRING(PresentationTitle,2,1))=1 THEN SUBSTRING(PresentationTitle,1,7) ");
            strSQL.Append("                  WHEN PresentationTitle IS NULL THEN NULL ");
            strSQL.Append("                  ELSE SUBSTRING(PresentationTitle,1,8) END PresentationTitleID ");
            strSQL.Append("             ,PresentationTitle,EpisodeNumber,HouseNo, ");
            strSQL.Append("         Case When Len(IsNull([Comment],''))> 0 ");
            strSQL.Append("               Then (Case When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                          When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='N' Then 'N' ");
            strSQL.Append("                          When Substring([Type],1,1)='N' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                          When Substring([Type],1,1)='N' And Substring([Comment],1,1)='N' Then 'N' ");
            strSQL.Append("                          When IsNull(EpisodeNumber,'') <> '' Then '資料錯誤!!!' Else '' End) ");
            strSQL.Append("          Else (Case When Substring([Type],1,1)='Y' Then 'Y' ");
            strSQL.Append("                     When Substring([Type],1,1)='N' Then 'N' ");
            strSQL.Append("                     When IsNull(EpisodeNumber,'') <> '' Then '資料錯誤!!!' Else '' End) End [TypeCommnet] ");
            strSQL.Append("      FROM dbo.srtmcs_cueTemp ");
            strSQL.Append("      WHERE IsNull(PresentationTitle,'') <> '' ");
            strSQL.Append("        AND IsNull(EpisodeNumber,'') <> '' ) CUE ");
            strSQL.Append(" ON ST._ST030 = CUE.PresentationTitleID AND ST._ST031 = CUE.EpisodeNumber ");
            strSQL.Append(" WHERE ST._ST043 IS NOT NULL and [TypeCommnet] = 'N' ");
            strSQL.Append(" GROUP BY CUE.HouseNo ,ST._ST043, [TypeCommnet] ");

            return strSQL.ToString();
        }        
        #endregion

        #region Insert
        /// <summary>
        /// Insert SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public String InsertCueTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" INSERT INTO [pms].[dbo].[srtmcs_cueTemp] ");
            strSQL.Append("             ([Time] ");
            strSQL.Append("             ,[FixTime] ");
            strSQL.Append("             ,[PresentationTitle] ");
            strSQL.Append("             ,[EpisodeNumber] ");
            strSQL.Append("             ,[PartNo] ");
            strSQL.Append("             ,[Duration] ");
            strSQL.Append("             ,[EpisodeTitle] ");
            strSQL.Append("             ,[HouseNo] ");
            strSQL.Append("             ,[MediaNo] ");
            strSQL.Append("             ,[ProgramTitle] ");
            strSQL.Append("             ,[PromotionTitle] ");
            strSQL.Append("             ,[Channel] ");
            strSQL.Append("             ,[Date] ");
            strSQL.Append("             ,[Comment] ");
            strSQL.Append("             ,[Type] ");
            strSQL.Append("             ,[Repeat] ");
            strSQL.Append("             ,[SOM] ");
            strSQL.Append("             ,[EOM] ");
            strSQL.Append("             ,[Classification] ");
            strSQL.Append("             ,[PresentationSequenceNo]) ");                     
            strSQL.Append(" VALUES ");
            strSQL.Append("             (@Time ");
            strSQL.Append("             ,@FixTime ");
            strSQL.Append("             ,@PresentationTitle ");
            strSQL.Append("             ,@EpisodeNumber ");
            strSQL.Append("             ,@PartNo ");
            strSQL.Append("             ,@Duration ");
            strSQL.Append("             ,@EpisodeTitle ");
            strSQL.Append("             ,@HouseNo ");
            strSQL.Append("             ,@MediaNo ");
            strSQL.Append("             ,@ProgramTitle ");
            strSQL.Append("             ,@PromotionTitle ");
            strSQL.Append("             ,@Channel ");
            strSQL.Append("             ,@Date ");
            strSQL.Append("             ,@Comment ");
            strSQL.Append("             ,@Type ");
            strSQL.Append("             ,@Repeat ");
            strSQL.Append("             ,@SOM ");
            strSQL.Append("             ,@EOM ");
            strSQL.Append("             ,@Classification ");
            strSQL.Append("             ,@PresentationSequenceNo) ");                       

            return strSQL.ToString();
        }

        /// <summary>
        /// Insert SrtEPG From SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/10/31, Create
        /// </history>
        public String InsertCueFromTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" INSERT INTO [pms].[dbo].[srtmcs_cue] ");
            strSQL.Append("             ([Time] ");
            strSQL.Append("             ,[FixTime] ");
            strSQL.Append("             ,[PresentationTitle] ");
            strSQL.Append("             ,[EpisodeNumber] ");
            strSQL.Append("             ,[PartNo] ");
            strSQL.Append("             ,[Duration] ");
            strSQL.Append("             ,[EpisodeTitle] ");
            strSQL.Append("             ,[HouseNo] ");
            strSQL.Append("             ,[MediaNo] ");
            strSQL.Append("             ,[ProgramTitle] ");
            strSQL.Append("             ,[PromotionTitle] ");
            strSQL.Append("             ,[Channel] ");
            strSQL.Append("             ,[Date] ");
            strSQL.Append("             ,[Comment] ");
            strSQL.Append("             ,[Type] ");
            strSQL.Append("             ,[Repeat] ");
            strSQL.Append("             ,[SOM] ");
            strSQL.Append("             ,[EOM] ");
            strSQL.Append("             ,[Classification] ");
            strSQL.Append("             ,[PresentationSequenceNo]) ");           
            strSQL.Append(" SELECT ");
            strSQL.Append("             [Time] ");
            strSQL.Append("             ,Case When [FixTime] = 'True' OR [FixTime] = 'TRUE' Then 1 ");
            strSQL.Append("                   When [FixTime] = 'False' OR [FixTime] = 'FALSE' Then 0 ");
            strSQL.Append("                   Else NULL End ");
            strSQL.Append("             ,[PresentationTitle] ");
            strSQL.Append("             ,Case When [EpisodeNumber] = '' Then NULL Else CONVERT(int,[EpisodeNumber]) End ");
            strSQL.Append("             ,Case When [PartNo] = '' Then NULL Else CONVERT(int,[PartNo]) End ");
            strSQL.Append("             ,[Duration] ");
            strSQL.Append("             ,[EpisodeTitle] ");
            strSQL.Append("             ,[HouseNo] ");
            strSQL.Append("             ,[MediaNo] ");
            strSQL.Append("             ,[ProgramTitle] ");
            strSQL.Append("             ,[PromotionTitle] ");
            strSQL.Append("             ,[Channel] ");
            strSQL.Append("             ,[Date] ");
            strSQL.Append("             ,[Comment] ");
            strSQL.Append("             ,[Type] ");
            strSQL.Append("             ,Case When [Repeat] = 'True' OR [Repeat] = 'TRUE' Then 1 ");
            strSQL.Append("                   When [Repeat] = 'False' OR [Repeat] = 'FALSE' Then 0 ");
            strSQL.Append("                   Else NULL End ");
            strSQL.Append("             ,[SOM] ");
            strSQL.Append("             ,[EOM] ");
            strSQL.Append("             ,[Classification] ");
            strSQL.Append("             ,Case When [PresentationSequenceNo] = '' Then NULL Else CONVERT(int,[PresentationSequenceNo]) End ");                                                                   
            strSQL.Append(" FROM dbo.srtmcs_cueTemp ");
            strSQL.Append(" WHERE ISNULL([HouseNo],'') <> '' "); 

            return strSQL.ToString();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update SrtCUETemp Error Date.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// 2.Tanya Wu, 2014/03/03, Modify 判斷隔日改以Time時間為主
        /// </history>
        public String UpdateCueTempErrorDate()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" UPDATE [pms].[dbo].[srtmcs_cueTemp] ");
            strSQL.Append("    SET [Date] = DATEADD(D,1,[Date]) ");
            strSQL.Append(" WHERE PresentationSequenceNo > ");
            strSQL.Append("       (SELECT Max(Convert(Int,PresentationSequenceNo)) ");
            strSQL.Append("        FROM [pms].[dbo].[srtmcs_cueTemp] ");
            strSQL.Append("        WHERE Convert(Int,Substring(Time,1,2)) >= 5 ");
            strSQL.Append("          AND IsNull([HouseNo],'') <> '') ");

            return strSQL.ToString();

        }
        
        #endregion

        #region Delete
        /// <summary>
        /// Delete SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create        
        /// </history>
        public String DelCueTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" Delete dbo.srtmcs_cueTemp ");

            return strSQL.ToString();
        }

        /// <summary>
        /// Delete SrtCUE 預重新匯入的舊資料.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// 2.Tanya Wu, 2014/01/06, Modify:刪除條件改為Channel,Date,Time
        /// </history>
        public String DelCueOldData()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" DELETE dbo.srtmcs_cue ");
            strSQL.Append(" WHERE Channel =  (SELECT Channel FROM dbo.srtmcs_cueTemp GROUP BY Channel) AND ");
            strSQL.Append("       CONVERT(varchar(12), [Date], 111) + ' ' + SUBSTRING(Time,1,2) BETWEEN ");
            strSQL.Append("       (SELECT CONVERT(varchar(12), [Date], 111)  + ' ' + SUBSTRING(Time,1,2) ");
            strSQL.Append("        FROM dbo.srtmcs_cueTemp ");
            strSQL.Append("        WHERE PresentationSequenceNo = ");
            strSQL.Append("              (SELECT Min(Convert(Int,PresentationSequenceNo)) FROM dbo.srtmcs_cueTemp WHERE IsNull(HouseNo,'') <> '' )) ");
            strSQL.Append("       AND ");
            strSQL.Append("       (SELECT CONVERT(varchar(12), [Date], 111)  + ' ' + SUBSTRING(Time,1,2) ");
            strSQL.Append("        FROM dbo.srtmcs_cueTemp ");
            strSQL.Append("        WHERE PresentationSequenceNo = ");
            strSQL.Append("              (SELECT Max(Convert(Int,PresentationSequenceNo)) FROM dbo.srtmcs_cueTemp WHERE IsNull(HouseNo,'') <> '' )) ");                        

            return strSQL.ToString();
        }
        #endregion
    }
}
