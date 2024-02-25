using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// Represents the unique identifier of Matchup
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the two teams that will compete with each other
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// The ID from the database that will be used to identify the winner.
        /// </summary>
        public int WinnerId { get; set; }
        /// <summary>
        /// Represents the winner team or winner player
        /// </summary>
        public TeamModel Winner { get; set; } = new TeamModel();
        /// <summary>
        /// Represnts the current round number
        /// </summary>
        public int MatchupRound { get; set; }
        public string DisplayName {
            get
            {
                if (Entries.Any(me => me.TeamCompeting == null))
                {
                    return "Matchup Not Yet Determined";
                }

                string output = string.Join(" vs. ", Entries.Select(me => me.TeamCompeting.TeamName ?? "Not Yet Set"));
                return output;
            }

        }
    }
}
