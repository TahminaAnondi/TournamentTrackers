using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);

        PersonModel CreatePerson(PersonModel model);

        List<PersonModel> GetAllPersons();

        TeamModel CreateTeam(TeamModel model);

        List<TeamModel> GetTeam_All();

        TournamentModel CreateTournament(TournamentModel model);

        List<TournamentModel> GetTournaments_All();

        void UpdateMatchup(MatchupModel model);
    }
}
