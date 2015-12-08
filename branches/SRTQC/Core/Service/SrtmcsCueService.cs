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
    public class SrtmcsCueService
    {
        DBProvider dbProvider = new DBProvider();
        SqlConnection conn = new SqlConnection();
        SrtmcsCueDao objCueDao = new SrtmcsCueDao();                      

        #region Select
        /// <summary>
        /// Get Cue查詢結果清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/10/11, Create
        /// 2.Tanya Wu, 2013/11/29, Modify:查詢資料來源修改
        /// </history>
        public DataTable GetCueList(string startDate, string endDate, string channel, string presentationTitle, string episodeNumber, string houseNo, string premier)
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
            if (!string.IsNullOrEmpty(presentationTitle) && presentationTitle != "")
                cmd.Parameters.Add("@presentationTitle", SqlDbType.NVarChar).Value = presentationTitle;
            if (!string.IsNullOrEmpty(episodeNumber) && episodeNumber != "")
                cmd.Parameters.Add("@episodeNumber", SqlDbType.NVarChar).Value = episodeNumber;
            if (!string.IsNullOrEmpty(houseNo) && houseNo != "")
                cmd.Parameters.Add("@houseNo", SqlDbType.NVarChar).Value = houseNo;
            if (!string.IsNullOrEmpty(premier) && premier != "")
                cmd.Parameters.Add("@premier", SqlDbType.NVarChar).Value = premier;

            return dbProvider.OpenDataTable(objCueDao.GetCueoList(startDate, endDate, channel, presentationTitle, episodeNumber, houseNo, premier), cmd);
        }

        /// <summary>
        /// Get SRT匯出清單.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public DataTable GetSrtExportList()
        {
            return dbProvider.OpenDataTableNonParameter(objCueDao.GetSrtExportList());
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public void InsertCueTemp(SrtmcsCUETemp parameters)
        {
            SqlCommand cmd = new SqlCommand();

            // Add the parameter to the Parameters collection.            
            cmd.Parameters.Add("@Time", SqlDbType.NVarChar).Value = parameters.Time;
            cmd.Parameters.Add("@FixTime", SqlDbType.NVarChar).Value = parameters.FixTime;
            cmd.Parameters.Add("@PresentationTitle", SqlDbType.NVarChar).Value = parameters.PresentationTitle;
            cmd.Parameters.Add("@EpisodeNumber", SqlDbType.NVarChar).Value = parameters.EpisodeNumber;
            cmd.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = parameters.PartNo;
            cmd.Parameters.Add("@Duration", SqlDbType.NVarChar).Value = parameters.Duration;
            cmd.Parameters.Add("@EpisodeTitle", SqlDbType.NVarChar).Value = parameters.EpisodeTitle;
            cmd.Parameters.Add("@HouseNo", SqlDbType.NVarChar).Value = parameters.HouseNo;
            cmd.Parameters.Add("@MediaNo", SqlDbType.NVarChar).Value = parameters.MediaNo;
            cmd.Parameters.Add("@ProgramTitle", SqlDbType.NVarChar).Value = parameters.ProgramTitle;
            cmd.Parameters.Add("@PromotionTitle", SqlDbType.NVarChar).Value = parameters.PromotionTitle;
            cmd.Parameters.Add("@Channel", SqlDbType.NVarChar).Value = parameters.Channel;
            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = parameters.Date;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = parameters.Comment;
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar).Value = parameters.Type;
            cmd.Parameters.Add("@Repeat", SqlDbType.NVarChar).Value = parameters.Repeat;
            cmd.Parameters.Add("@SOM", SqlDbType.NVarChar).Value = parameters.SOM;
            cmd.Parameters.Add("@EOM", SqlDbType.NVarChar).Value = parameters.EOM;
            cmd.Parameters.Add("@Classification", SqlDbType.NVarChar).Value = parameters.Classification;
            cmd.Parameters.Add("@PresentationSequenceNo", SqlDbType.NVarChar).Value = parameters.PresentationSequenceNo;

            //cmd.Parameters.Add("@Region", SqlDbType.VarChar).Value = parameters.Region;                                                            
            //cmd.Parameters.Add("@Episode", SqlDbType.VarChar).Value = parameters.Episode;                                    
            //cmd.Parameters.Add("@ScheduleType", SqlDbType.VarChar).Value = parameters.ScheduleType;            
            //cmd.Parameters.Add("@AlternativeTitle", SqlDbType.VarChar).Value = parameters.AlternativeTitle;            
            //cmd.Parameters.Add("@Planning", SqlDbType.VarChar).Value = parameters.Planning;
            //cmd.Parameters.Add("@Programme", SqlDbType.VarChar).Value = parameters.Programme;
            //cmd.Parameters.Add("@Source", SqlDbType.VarChar).Value = parameters.Source;
            //cmd.Parameters.Add("@Availability", SqlDbType.VarChar).Value = parameters.Availability;            
            //cmd.Parameters.Add("@Segment", SqlDbType.VarChar).Value = parameters.Segment;
            //cmd.Parameters.Add("@BreakType", SqlDbType.VarChar).Value = parameters.BreakType;
            //cmd.Parameters.Add("@Component", SqlDbType.VarChar).Value = parameters.Component;
            //cmd.Parameters.Add("@Promotion", SqlDbType.VarChar).Value = parameters.Promotion;
            //cmd.Parameters.Add("@MediaPattern", SqlDbType.VarChar).Value = parameters.MediaPattern;            
            //cmd.Parameters.Add("@Exhibition", SqlDbType.VarChar).Value = parameters.Exhibition;
            //cmd.Parameters.Add("@ViaSource", SqlDbType.VarChar).Value = parameters.ViaSource;
            //cmd.Parameters.Add("@Transition", SqlDbType.VarChar).Value = parameters.Transition;
            //cmd.Parameters.Add("@Effect", SqlDbType.VarChar).Value = parameters.Effect;
            //cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = parameters.Category;
            //cmd.Parameters.Add("@TargetAudience", SqlDbType.VarChar).Value = parameters.TargetAudience;
            //cmd.Parameters.Add("@Censor", SqlDbType.VarChar).Value = parameters.Censor;
            //cmd.Parameters.Add("@Rights", SqlDbType.VarChar).Value = parameters.Rights;
            //cmd.Parameters.Add("@BreakOverBooked", SqlDbType.VarChar).Value = parameters.BreakOverBooked;
            //cmd.Parameters.Add("@StandardItem", SqlDbType.VarChar).Value = parameters.StandardItem;
            //cmd.Parameters.Add("@ProductionNo", SqlDbType.VarChar).Value = parameters.ProductionNo;
            //cmd.Parameters.Add("@UmbrellaTitle", SqlDbType.VarChar).Value = parameters.UmbrellaTitle;
            //cmd.Parameters.Add("@PlannerOriginalDuration", SqlDbType.VarChar).Value = parameters.PlannerOriginalDuration;
            //cmd.Parameters.Add("@PlannerOriginaTime", SqlDbType.VarChar).Value = parameters.PlannerOriginaTime;
            //cmd.Parameters.Add("@PresentationDurationManualAdjustment", SqlDbType.VarChar).Value = parameters.PresentationDurationManualAdjustment;
            //cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = parameters.Status;
            //cmd.Parameters.Add("@TransitionSpeed", SqlDbType.VarChar).Value = parameters.TransitionSpeed;
            //cmd.Parameters.Add("@ScheduleRemark", SqlDbType.VarChar).Value = parameters.ScheduleRemark;
            //cmd.Parameters.Add("@AutomationTrigger", SqlDbType.VarChar).Value = parameters.AutomationTrigger;
            //cmd.Parameters.Add("@StandardListID", SqlDbType.VarChar).Value = parameters.StandardListID;
            //cmd.Parameters.Add("@ProductionSite", SqlDbType.VarChar).Value = parameters.ProductionSite;
            //cmd.Parameters.Add("@Genre", SqlDbType.VarChar).Value = parameters.Genre;
            //cmd.Parameters.Add("@GuaranteedBreak", SqlDbType.VarChar).Value = parameters.GuaranteedBreak;
            //cmd.Parameters.Add("@AdditionalInstructions", SqlDbType.VarChar).Value = parameters.AdditionalInstructions;
            //cmd.Parameters.Add("@MediaSegmentPatternID", SqlDbType.VarChar).Value = parameters.MediaSegmentPatternID;
            //cmd.Parameters.Add("@Origin", SqlDbType.VarChar).Value = parameters.Origin;            
            //cmd.Parameters.Add("@BreakFormat", SqlDbType.VarChar).Value = parameters.BreakFormat;
            //cmd.Parameters.Add("@AudioCharacteristic", SqlDbType.VarChar).Value = parameters.AudioCharacteristic;
            //cmd.Parameters.Add("@HDSDFormat", SqlDbType.VarChar).Value = parameters.HDSDFormat;
            //cmd.Parameters.Add("@AspectRatio", SqlDbType.VarChar).Value = parameters.AspectRatio;
            //cmd.Parameters.Add("@StartTime", SqlDbType.VarChar).Value = parameters.StartTime;
            //cmd.Parameters.Add("@EndTime", SqlDbType.VarChar).Value = parameters.EndTime;
            //cmd.Parameters.Add("@CalendarDate", SqlDbType.DateTime).Value = parameters.CalendarDate;

            dbProvider.ExecuteNonQuery(objCueDao.InsertCueTemp(), cmd);
        }

        /// <summary>
        /// Insert SrtCUE From SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public void InsertCueFromTemp()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objCueDao.InsertCueFromTemp(), cmd);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update SrtCUETemp Error Date.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public void UpdateCueTempErrorDate()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objCueDao.UpdateCueTempErrorDate(), cmd);
        }
        #endregion

        #region Delete
        // <summary>
        /// Delete SrtCUETemp.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public void DelCueTemp()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objCueDao.DelCueTemp(), cmd);
        }

        /// <summary>
        /// Delete SrtCUE 預重新匯入的舊資料.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/27, Create
        /// </history>
        public void DelCueOldData()
        {
            SqlCommand cmd = new SqlCommand();

            dbProvider.ExecuteNonQuery(objCueDao.DelCueOldData(), cmd);
        }
        #endregion
        
    }
}
