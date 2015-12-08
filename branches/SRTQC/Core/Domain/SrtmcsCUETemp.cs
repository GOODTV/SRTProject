using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain
{
    /// <summary>
    /// mapping到的Table：[dbo].[srtmcs_cueTemp]
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2013/11/27, Create
    /// </history>
    public class SrtmcsCUETemp
    {
        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        /// <value>The Time.</value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the FixedTime.
        /// </summary>
        /// <value>The Fixed Time.</value>
        public string FixTime { get; set; }

        /// <summary>
        /// Gets or sets the PresentationTitle.
        /// </summary>
        /// <value>The Presentation Title.</value>
        public string PresentationTitle { get; set; }

        /// <summary>
        /// Gets or sets the EpisodeNumber.
        /// </summary>
        /// <value>The Episode Number.</value>
        public string EpisodeNumber { get; set; }

        /// <summary>
        /// Gets or sets the PartNo.
        /// </summary>
        /// <value>The Part No.</value>
        public string PartNo { get; set; }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        /// <value>The Duration.</value>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the EpisodeTitle.
        /// </summary>
        /// <value>The Episode Title.</value>
        public string EpisodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the HouseNo.
        /// </summary>
        /// <value>The House No.</value>
        public string HouseNo { get; set; }
        
        /// <summary>
        /// Gets or sets the MediaNo.
        /// </summary>
        /// <value>The Media No.</value>
        public string MediaNo { get; set; }

        /// <summary>
        /// Gets or sets the ProgramTitle.
        /// </summary>
        /// <value>The Program Title.</value>
        public string ProgramTitle { get; set; }

        /// <summary>
        /// Gets or sets the PromotionTitle.
        /// </summary>
        /// <value>The Promotion Title.</value>
        public string PromotionTitle { get; set; }

        /// <summary>
        /// Gets or sets the Channel.
        /// </summary>
        /// <value>The Channel.</value>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        /// <value>The Event Date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        /// <value>The Comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The Type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Repeat.
        /// </summary>
        /// <value>The Repeat.</value>
        public string Repeat { get; set; }

        /// <summary>
        /// Gets or sets the SOM.
        /// </summary>
        /// <value>The SOM.</value>
        public string SOM { get; set; }

        /// <summary>
        /// Gets or sets the EOM.
        /// </summary>
        /// <value>The EOM.</value>
        public string EOM { get; set; }

        /// <summary>
        /// Gets or sets the Classification.
        /// </summary>
        /// <value>The Classification.</value>
        public string Classification { get; set; }      

        /// <summary>
        /// Gets or sets the PresentationSequenceNo.
        /// </summary>
        /// <value>The Presentation Sequence No.</value>
        public string PresentationSequenceNo { get; set; }        
    }
}
