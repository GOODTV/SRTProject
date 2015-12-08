using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Core.Utility;
using Core.DataAccess;
using Core.Domain;


namespace Core.Service
{
    public class SrtmcsEpgService
    {
        DBProvider dbProvider = new DBProvider();
        SqlConnection conn = new SqlConnection();
        SrtmcsEpgDao objEpgDao = new SrtmcsEpgDao();       

        #region Select
        /// <summary>
        /// Get EPG查詢結果清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/10/11, Create
        /// </history>
        public DataTable GetEpgList(string startDate, string endDate, string channel, string planningTitle, string episodeNumber, string houseNo, string premier, string planningName, string calendarTime, string calendarTime2)
        {
            SqlCommand cmd = new SqlCommand();

            // Add the parameter to the Parameters collection.
            if (!string.IsNullOrEmpty(startDate) && startDate != "")
            {
                DateTime sdate = new DateTime(Int32.Parse(startDate.Substring(0, 4)), Int32.Parse(startDate.Substring(5, 2)), Int32.Parse(startDate.Substring(8, 2)));
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = sdate;
            }
            if (!string.IsNullOrEmpty(endDate) && endDate != "")
            {
                DateTime edate = new DateTime(Int32.Parse(endDate.Substring(0, 4)), Int32.Parse(endDate.Substring(5, 2)), Int32.Parse(endDate.Substring(8, 2))).AddDays(1).AddMinutes(-1);
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = edate;
            }
            if (!string.IsNullOrEmpty(channel) && channel != "")
                cmd.Parameters.Add("@channel", SqlDbType.NVarChar).Value = channel;
            if (!string.IsNullOrEmpty(planningTitle) && planningTitle != "")
                cmd.Parameters.Add("@planningTitle", SqlDbType.NVarChar).Value = planningTitle;
            if (!string.IsNullOrEmpty(episodeNumber) && episodeNumber != "")
                cmd.Parameters.Add("@episodeNumber", SqlDbType.NVarChar).Value = episodeNumber;
            if (!string.IsNullOrEmpty(houseNo) && houseNo != "")
                cmd.Parameters.Add("@houseNo", SqlDbType.NVarChar).Value = houseNo;
            if (!string.IsNullOrEmpty(premier) && premier != "")
                cmd.Parameters.Add("@premier", SqlDbType.NVarChar).Value = premier;
            if (!string.IsNullOrEmpty(planningName) && planningName != "")
                cmd.Parameters.Add("@PlanningName", SqlDbType.NVarChar).Value = planningName;
            if (!string.IsNullOrEmpty(calendarTime) && calendarTime != "")
                cmd.Parameters.Add("@CalendarTime", SqlDbType.NVarChar).Value = calendarTime.Substring(0, 2) + ":" + calendarTime.Substring(2, 2); ;
            if (!string.IsNullOrEmpty(calendarTime2) && calendarTime2 != "")
                cmd.Parameters.Add("@CalendarTime2", SqlDbType.NVarChar).Value = calendarTime2.Substring(0, 2) + ":" + calendarTime2.Substring(2, 2); ;

            return dbProvider.OpenDataTable(objEpgDao.GetEpgoList(startDate, endDate, channel, planningTitle, episodeNumber, houseNo, premier, planningName, calendarTime, calendarTime2), cmd);
        }

        //原程式
        public DataTable GetEpgList(string startDate, string endDate, string channel, string planningTitle, string episodeNumber, string houseNo, string premier)
        {
            SqlCommand cmd = new SqlCommand();

            // Add the parameter to the Parameters collection.
            if (!string.IsNullOrEmpty(startDate) && startDate != "")
            {
                DateTime sdate = new DateTime(Int32.Parse(startDate.Substring(0, 4)), Int32.Parse(startDate.Substring(5, 2)), Int32.Parse(startDate.Substring(8, 2)));
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = sdate;
            }
            if (!string.IsNullOrEmpty(endDate) && endDate != "")
            {
                DateTime edate = new DateTime(Int32.Parse(endDate.Substring(0, 4)), Int32.Parse(endDate.Substring(5, 2)), Int32.Parse(endDate.Substring(8, 2))).AddDays(1).AddMinutes(-1);
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = edate;
            }
            if (!string.IsNullOrEmpty(channel) && channel != "")
                cmd.Parameters.Add("@channel", SqlDbType.NVarChar).Value = channel;
            if (!string.IsNullOrEmpty(planningTitle) && planningTitle != "")
                cmd.Parameters.Add("@planningTitle", SqlDbType.NVarChar).Value = planningTitle;
            if (!string.IsNullOrEmpty(episodeNumber) && episodeNumber != "")
                cmd.Parameters.Add("@episodeNumber", SqlDbType.NVarChar).Value = episodeNumber;
            if (!string.IsNullOrEmpty(houseNo) && houseNo != "")
                cmd.Parameters.Add("@houseNo", SqlDbType.NVarChar).Value = houseNo;
            if (!string.IsNullOrEmpty(premier) && premier != "")
                cmd.Parameters.Add("@premier", SqlDbType.NVarChar).Value = premier;

            return dbProvider.OpenDataTable(objEpgDao.GetEpgoList(startDate, endDate, channel, planningTitle, episodeNumber, houseNo, premier, null, null, null), cmd);
        }

        /// <summary>
        /// Get EPG Temp 播映日期時間清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/28, Create        
        /// </history>
        public DataTable GetEpgTempCalendarDateList()
        {
            return dbProvider.OpenDataTableNonParameter(objEpgDao.GetEpgTempCalendarDateList());
        }

        /// <summary>
        /// Get SRT匯出清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public DataTable GetSrtExportList()
        {
            return dbProvider.OpenDataTableNonParameter(objEpgDao.GetSrtExportList());
        }

        /// <summary>
        /// 取得EPG節目名稱.
        /// </summary>
        /// <history>
        /// 1.Samson Hsu, 2015/07/27, Create
        /// </history>
        public DataTable GetEpgProgramName(string ProgramID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            return dbProvider.OpenDataTable(objEpgDao.GetEpgProgramName(), cmd);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public void InsertEpgTemp(SrtmcsEPGTemp parameters)
        {
            SqlCommand cmd = new SqlCommand();

            // Add the parameter to the Parameters collection. 
            cmd.Parameters.Add("@Channel", SqlDbType.NVarChar).Value = parameters.Channel;
            cmd.Parameters.Add("@CalendarDate", SqlDbType.DateTime).Value = parameters.CalendarDate;
            cmd.Parameters.Add("@Time", SqlDbType.NVarChar).Value = parameters.Time;
            cmd.Parameters.Add("@HouseNo", SqlDbType.NVarChar).Value = parameters.HouseNo;
            cmd.Parameters.Add("@PlanningTitle", SqlDbType.NVarChar).Value = parameters.PlanningTitle;
            cmd.Parameters.Add("@EpisodeNumber", SqlDbType.NVarChar).Value = parameters.EpisodeNumber;
            cmd.Parameters.Add("@EpisodeTitle", SqlDbType.NVarChar).Value = parameters.EpisodeTitle;
            cmd.Parameters.Add("@Premier", SqlDbType.NVarChar).Value = parameters.Premier;
            cmd.Parameters.Add("@Repeat", SqlDbType.NVarChar).Value = parameters.Repeat;
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar).Value = parameters.Type;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = parameters.Comment;

            //cmd.Parameters.Add("@EventDuration", SqlDbType.VarChar).Value = parameters.EventDuration;
            //cmd.Parameters.Add("@Media", SqlDbType.VarChar).Value = parameters.Media;            
            //cmd.Parameters.Add("@CalendarEndDate", SqlDbType.DateTime).Value = parameters.CalendarEndDate;            
            //cmd.Parameters.Add("@Region", SqlDbType.VarChar).Value = parameters.Region;
            //cmd.Parameters.Add("@ScheduleType", SqlDbType.VarChar).Value = parameters.ScheduleType;            
            //cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = parameters.Category;
            //cmd.Parameters.Add("@Source", SqlDbType.VarChar).Value = parameters.Source;
            //cmd.Parameters.Add("@FixedTime", SqlDbType.VarChar).Value = parameters.FixedTime;            
            //cmd.Parameters.Add("@Exhibition", SqlDbType.VarChar).Value = parameters.Exhibition;            
            //cmd.Parameters.Add("@Cost", SqlDbType.VarChar).Value = parameters.Cost;
            //cmd.Parameters.Add("@Certificate", SqlDbType.VarChar).Value = parameters.Certificate;
            //cmd.Parameters.Add("@EstimatedRating", SqlDbType.VarChar).Value = parameters.EstimatedRating;
            //cmd.Parameters.Add("@ActualRating", SqlDbType.VarChar).Value = parameters.ActualRating;
            //cmd.Parameters.Add("@AltTitle", SqlDbType.VarChar).Value = parameters.AltTitle;
            //cmd.Parameters.Add("@UmbrellaTitle", SqlDbType.VarChar).Value = parameters.UmbrellaTitle;            
            //cmd.Parameters.Add("@TargetAudience", SqlDbType.VarChar).Value = parameters.TargetAudience;
            //cmd.Parameters.Add("@PlanningBlockContained", SqlDbType.VarChar).Value = parameters.PlanningBlockContained;
            //cmd.Parameters.Add("@ProductionNo", SqlDbType.VarChar).Value = parameters.ProductionNo;
            //cmd.Parameters.Add("@ScheduleRemark", SqlDbType.VarChar).Value = parameters.ScheduleRemark;            
            //cmd.Parameters.Add("@PlanningStaggerCasted", SqlDbType.VarChar).Value = parameters.PlanningStaggerCasted;
            //cmd.Parameters.Add("@PlanningMasterPlanningID", SqlDbType.VarChar).Value = parameters.PlanningMasterPlanningID;
            //cmd.Parameters.Add("@PlanningStaggerCastDate", SqlDbType.VarChar).Value = parameters.PlanningStaggerCastDate;
            //cmd.Parameters.Add("@PlanningStaggerCastEndDate", SqlDbType.VarChar).Value = parameters.PlanningStaggerCastEndDate;
            //cmd.Parameters.Add("@PlanningMasterChannelID", SqlDbType.VarChar).Value = parameters.PlanningMasterChannelID;
            //cmd.Parameters.Add("@BroadcastDate", SqlDbType.DateTime).Value = parameters.BroadcastDate;
            //cmd.Parameters.Add("@EventID", SqlDbType.VarChar).Value = parameters.EventID;
            //cmd.Parameters.Add("@PlanningExtProgID", SqlDbType.VarChar).Value = parameters.PlanningExtProgID;
            //cmd.Parameters.Add("@PlanningExtEpisodeID", SqlDbType.VarChar).Value = parameters.PlanningExtEpisodeID;
            //cmd.Parameters.Add("@PlanningExtTimeStamp", SqlDbType.VarChar).Value = parameters.PlanningExtTimeStamp;
            //cmd.Parameters.Add("@PlanningExtEventID", SqlDbType.VarChar).Value = parameters.PlanningExtEventID;            
            //cmd.Parameters.Add("@VariantCode", SqlDbType.VarChar).Value = parameters.VariantCode;
            //cmd.Parameters.Add("@PlanningProgrammePatternID", SqlDbType.VarChar).Value = parameters.PlanningProgrammePatternID;            
            //cmd.Parameters.Add("@MediaSegmentPatternID", SqlDbType.VarChar).Value = parameters.MediaSegmentPatternID;
            //cmd.Parameters.Add("@ProductionSite", SqlDbType.VarChar).Value = parameters.ProductionSite;
            //cmd.Parameters.Add("@BreakDescription", SqlDbType.VarChar).Value = parameters.BreakDescription;
            //cmd.Parameters.Add("@SeriesLink", SqlDbType.VarChar).Value = parameters.SeriesLink;

            dbProvider.ExecuteNonQuery(objEpgDao.InsertEpgTemp(), cmd);
        }   

        /// <summary>
        /// Insert SrtEPG From SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public void InsertEpgFromTemp()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objEpgDao.InsertEpgFromTemp(), cmd);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update SrtEPGTemp Duration.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/28, Create
        /// </history>
        public void UpdateDurationWithHouseNo(string HouseNo, string Duration)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Add("@Duration", SqlDbType.NVarChar).Value = Duration;
            cmd.Parameters.Add("@HouseNo", SqlDbType.NVarChar).Value = HouseNo;            

            dbProvider.ExecuteNonQuery(objEpgDao.UpdateDurationWithHouseNo(), cmd);
        }
        #endregion

        #region Delete
        // <summary>
        /// Delete SrtEPGTemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public void DelEpgTemp()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objEpgDao.DelEpgTemp(), cmd);
        }

        /// <summary>
        /// Delete SrtEPG 預重新匯入的舊資料.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public void DelEpgOldData()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objEpgDao.DelEpgOldData(), cmd);
        }
        #endregion


    }
}
