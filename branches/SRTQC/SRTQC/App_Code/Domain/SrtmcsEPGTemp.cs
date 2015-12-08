using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain
{
    /// <summary>
    /// mapping到的Table：[dbo].[srtmcs_eqgTemp]
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/11/26, Create
    /// </history>
    public class SrtmcsEPGTemp
    {
        /// <summary>
        /// Gets or sets the Channel.
        /// </summary>
        /// <value>The Channel.</value>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the CalendarDate.
        /// </summary>
        /// <value>The Calendar Date.</value>
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        /// <value>The Time.</value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the HouseNo.
        /// </summary>
        /// <value>The House No.</value>
        public string HouseNo { get; set; }

        /// <summary>
        /// Gets or sets the Planning Title.
        /// </summary>
        /// <value>The Planning Title.</value>
        public string PlanningTitle { get; set; }

        /// <summary>
        /// Gets or sets the EpisodeNumber.
        /// </summary>
        /// <value>The Episode Number.</value>
        public string EpisodeNumber { get; set; }

        /// <summary>
        /// Gets or sets the EpisodeTitle.
        /// </summary>
        /// <value>The Episode Title.</value>
        public string EpisodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the Premier.
        /// </summary>
        /// <value>The Premier.</value>
        public string Premier { get; set; }

        /// <summary>
        /// Gets or sets the Repeat.
        /// </summary>
        /// <value>The Repeat.</value>
        public string Repeat { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The Type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        /// <value>The Comment.</value>
        public string Comment { get; set; }
    }
}
