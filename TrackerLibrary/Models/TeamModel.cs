using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// Represents the unique identifier of team
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents all the team member in the same team
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
        /// <summary>
        /// Used to distinguish this team from others
        /// </summary>
        public string TeamName { get; set; }
    }
}
