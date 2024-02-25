using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represents the unique identifier of MatchupEntry
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the unique identifier for the Team
        /// </summary>
        public int TeamCompetingId { get; set; }
        /// <summary>
        /// Represents one team in the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; } = new TeamModel();
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represents the unique identifier for the parent matchup.
        /// </summary>
        public int ParentMatchupId { get; set; }
        /// <summary>
        /// Represents the matchup that this team came from as a winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; } = new MatchupModel();
    }
}
