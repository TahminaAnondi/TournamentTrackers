using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// Represents the unique identifier of the tournament
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Used to distinguish this tournament from others.
        /// such as ping pong tournament, basketball tournament.
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Represents the entry fee if the tournament requires it.
        /// </summary>
        public decimal EntryFee { get; set; }
        /// <summary>
        /// Represents all the teams which join in this tournament
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        /// Represents the prizes for different places.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        /// Represents all the rounds happend within this tournament
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}
