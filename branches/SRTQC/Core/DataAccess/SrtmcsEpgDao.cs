using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain;

namespace Core.DataAccess
{
    public class SrtmcsEpgDao
    {
        
        #region Select
        /// <summary>
        /// Get EPG查詢結果清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/28, Create        
        /// </history>
        public String GetEpgoList(string startDate, string endDate, string channel, string planningTitle, string episodeNumber, string houseNo, string premier)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT EPG.* ,SRT.[_MTIME],SRT.[_ST041],SRT.[_ST043] ");
            strSQL.Append("         ,[dbo].[getProgramName](EPG.[PlanningTitleID]) [PlanningTitleName] ");
            strSQL.Append(" FROM ");
            strSQL.Append(" (SELECT");
            strSQL.Append("       [Channel] ");
            strSQL.Append("       ,CONVERT(varchar(10), [CalendarDate], 111) AS CalendarDate ");
            strSQL.Append("       ,Convert(varchar(12), [CalendarDate], 114) AS CalendarTime ");
            strSQL.Append("       ,[HouseNo] ");
            strSQL.Append("       ,CASE WHEN ISNUMERIC(SUBSTRING([PlanningTitle],2,1)) = 1  THEN SUBSTRING([PlanningTitle],0,8) ");
            strSQL.Append("        ELSE SUBSTRING([PlanningTitle],0,9) END PlanningTitleID ");
            strSQL.Append("       ,[EpisodeNumber] ");
            strSQL.Append("       ,CASE WHEN [Premier] = 1 THEN '首' WHEN [Repeat] = 1 THEN '重' ELSE '' END PremierRepeat ");
            strSQL.Append("       ,[Duration] ");
            strSQL.Append("       ,[EpisodeTitle] ");
            strSQL.Append("       ,[PlanningTitle],[Premier] ");
            strSQL.Append("       ,Case When Len(IsNull([Comment],''))> 0 ");
            strSQL.Append("             Then (Case When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                        When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='N' Then 'N' ");
            strSQL.Append("                        When Substring([Type],1,1)='N' And Substring([Comment],1,1)='Y' Then 'Y' ");
            strSQL.Append("                        When Substring([Type],1,1)='N' And Substring([Comment],1,1)='N' Then 'N'  Else '資料錯誤!!!' End) ");            
            strSQL.Append("        Else (Case When Substring([Type],1,1)='Y' Then 'Y' ");
            strSQL.Append("                   When Substring([Type],1,1)='N' Then 'N' Else '資料錯誤!!!' End) End [TypeCommnet] ");
            strSQL.Append(" FROM [pms].[dbo].[srtmcs_epg] ) EPG ");
            strSQL.Append(" LEFT JOIN [pms].[dbo].[_ST03P1] AS SRT ");
            strSQL.Append(" ON SRT.[_ST030] = EPG.PlanningTitleID AND SRT.[_ST031] = EPG.EpisodeNumber ");
            strSQL.Append(" WHERE 1 = 1");

            // 開始日期
            if (!string.IsNullOrEmpty(startDate) && startDate != "")
                strSQL.Append(" AND EPG.[CalendarDate] >= @startDate ");
            // 結束日期
            if (!string.IsNullOrEmpty(endDate) && endDate != "")
                strSQL.Append(" AND EPG.[CalendarDate] <= @endDate ");
            // 頻道別
            if (!string.IsNullOrEmpty(channel) && channel != "")
                strSQL.Append(" AND EPG.[Channel] LIKE '%' + @channel + '%' ");
            // 節目/短片代碼
            if (!string.IsNullOrEmpty(planningTitle) && planningTitle != "")
                strSQL.Append(" AND EPG.[PlanningTitle] LIKE '%' + @planningTitle + '%' ");
            // 集數
            //20140113 Modify by Tanya:修改集數為精準查詢
            if (!string.IsNullOrEmpty(episodeNumber) && episodeNumber != "")
                strSQL.Append(" AND EPG.[EpisodeNumber] = @episodeNumber ");
            // HouseNumber
            if (!string.IsNullOrEmpty(houseNo) && houseNo != "")
                strSQL.Append(" AND EPG.[HouseNo]  LIKE '%' + @houseNo + '%' ");
            // Premier
            if (!string.IsNullOrEmpty(premier) && premier != "")
                strSQL.Append(" AND EPG.[Premier] = @premier ");

            strSQL.Append(" ORDER BY EPG.[Channel],EPG.[CalendarDate],EPG.[CalendarTime] ");

            return strSQL.ToString();
        }

        /// <summary>
        /// Get EPG Temp 播映日期時間清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/28, Create        
        /// </history>
        public String GetEpgTempCalendarDateList()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT CalendarDate,HouseNo ");
            strSQL.Append(" FROM dbo.srtmcs_epgTemp ");
            strSQL.Append(" ORDER BY CalendarDate ");

            return strSQL.ToString();
        }
        
        /// <summary>
        /// Get SRT匯出清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String GetSrtExportList()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT EPG.HouseNo ,ST._ST043 ");
            strSQL.Append(" FROM dbo._ST03P1 ST ");
            strSQL.Append(" INNER JOIN ");
            strSQL.Append("     (SELECT CASE WHEN ISNUMERIC(SUBSTRING(PlanningTitle,2,1))=1 THEN SUBSTRING(PlanningTitle,1,7) ");
            strSQL.Append("                  WHEN PlanningTitle IS NULL THEN NULL ");
            strSQL.Append("                  ELSE SUBSTRING(PlanningTitle,1,8) END PlanningTitle ");
            strSQL.Append("             ,EpisodeNumber,HouseNo ");
            strSQL.Append("      FROM dbo.srtmcs_epg ");
            strSQL.Append("      WHERE CalendarDate >= (SELECT MIN(CalendarDate) FROM dbo.srtmcs_epgTemp) ");
            strSQL.Append("        AND CalendarDate <= (SELECT MAX(CalendarDate) FROM dbo.srtmcs_epgTemp) ) EPG ");
            strSQL.Append(" ON ST._ST030 = EPG.PlanningTitle AND ST._ST031 = EPG.EpisodeNumber ");
            strSQL.Append(" WHERE ST._ST043 IS NOT NULL ");

            return strSQL.ToString();
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String InsertEpgTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" INSERT INTO [pms].[dbo].[srtmcs_epgTemp] ");
            strSQL.Append("             ([Channel] ");
            strSQL.Append("             ,[CalendarDate] ");            
            strSQL.Append("             ,[Time] ");
            strSQL.Append("             ,[HouseNo] ");
            strSQL.Append("             ,[PlanningTitle] ");
            strSQL.Append("             ,[EpisodeNumber] ");
            strSQL.Append("             ,[EpisodeTitle] ");
            strSQL.Append("             ,[Premier] ");
            strSQL.Append("             ,[Repeat] ");
            strSQL.Append("             ,[Type] ");
            strSQL.Append("             ,[Comment]) ");           
            strSQL.Append(" VALUES ");
            strSQL.Append("             (@Channel ");
            strSQL.Append("             ,@CalendarDate ");
            strSQL.Append("             ,@Time ");
            strSQL.Append("             ,@HouseNo ");
            strSQL.Append("             ,@PlanningTitle ");
            strSQL.Append("             ,@EpisodeNumber ");
            strSQL.Append("             ,@EpisodeTitle ");
            strSQL.Append("             ,@Premier ");
            strSQL.Append("             ,@Repeat ");
            strSQL.Append("             ,@Type ");
            strSQL.Append("             ,@Comment) ");                     

            return strSQL.ToString();
        }

        /// <summary>
        /// Insert SrtEPG From SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String InsertEpgFromTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" INSERT INTO [pms].[dbo].[srtmcs_epg] ");
            strSQL.Append("             ([Channel] ");
            strSQL.Append("             ,[CalendarDate] ");
            strSQL.Append("             ,[Time] ");
            strSQL.Append("             ,[HouseNo] ");
            strSQL.Append("             ,[PlanningTitle] ");
            strSQL.Append("             ,[EpisodeNumber] ");
            strSQL.Append("             ,[EpisodeTitle] ");
            strSQL.Append("             ,[Premier] ");
            strSQL.Append("             ,[Repeat] ");
            strSQL.Append("             ,[Type] ");
            strSQL.Append("             ,[Comment] ");
            strSQL.Append("             ,[Duration]) ");          
            strSQL.Append(" SELECT ");
            strSQL.Append("        [Channel] ");
            strSQL.Append("        ,[CalendarDate] ");
            strSQL.Append("        ,[Time] ");
            strSQL.Append("        ,[HouseNo] ");
            strSQL.Append("        ,[PlanningTitle] ");
            strSQL.Append("        ,Case When [EpisodeNumber]='' Then NULL Else CONVERT(int,[EpisodeNumber]) End ");
            strSQL.Append("        ,[EpisodeTitle] ");
            strSQL.Append("        ,Case When [Premier] = 'True' OR [Premier] = 'TRUE' Then 1 ");
            strSQL.Append("              When [Premier] = 'False' OR [Premier] = 'FALSE' Then 0 ");
            strSQL.Append("         Else NULL End ");
            strSQL.Append("        ,Case When [Repeat] = 'True' OR [Repeat] = 'TRUE' Then 1 ");
            strSQL.Append("              When [Repeat] = 'False' OR [Repeat] = 'FALSE' Then 0 ");
            strSQL.Append("         Else NULL End ");
            strSQL.Append("        ,[Type] ");
            strSQL.Append("        ,[Comment] ");
            strSQL.Append("        ,[Duration] ");         
            strSQL.Append(" FROM dbo.srtmcs_epgTemp ");
            return strSQL.ToString();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update SrtEPGTemp Duration.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String UpdateDurationWithHouseNo()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" Update dbo.srtmcs_epgTemp ");
            strSQL.Append("    Set Duration=@Duration ");
            strSQL.Append(" Where HouseNo=@HouseNo ");

            return strSQL.ToString();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String DelEpgTemp()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" Delete dbo.srtmcs_epgTemp ");

            return strSQL.ToString();
        }

        /// <summary>
        /// Delete SrtEPG 預重新匯入的舊資料.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String DelEpgOldData()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" Delete dbo.srtmcs_epg ");
            strSQL.Append(" WHERE CalendarDate >= (SELECT MIN(CalendarDate) FROM dbo.srtmcs_epgTemp) ");
            strSQL.Append("   AND CalendarDate <= (SELECT MAX(CalendarDate) FROM dbo.srtmcs_epgTemp) ");
            strSQL.Append("   AND Channel = (SELECT Channel FROM dbo.srtmcs_epgTemp Group by Channel) ");

            return strSQL.ToString();
        }
        #endregion

    }
}
