using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName",model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.Email);
                p.Add("@CellphoneNumber", model.cellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spPersons_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                var p = new DynamicParameters();
                p.Add("@placeNumber", model.PlaceNumber);
                p.Add("@placeName",model.PlaceName);
                p.Add("@prizeAmount",model.PrizeAmount);
                p.Add("@prizePercentage",model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                var p = new DynamicParameters();
                p.Add("@teamName",model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                foreach(PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@teamId", model.Id);
                    p.Add("@personId", tm.Id);

                    connection.Execute("spTeamMembers_Insert", p, commandType:CommandType.StoredProcedure);
                }
                return model;
            }
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                SaveTournament(connection, model);

                SaveTournamentPrizes(connection, model);

                SaveTournamentEntries(connection, model);

                SaveTournamentRounds(connection, model);

                return model;
            }
        }
        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            // Loop through the rounds
            // Loop through the matchups
            // save matchups
            // Loop through matchup entries
            // save matchup entries
            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("spMatchups_Insert", p, commandType: CommandType.StoredProcedure);

                    matchup.Id = p.Get<int>("@id");


                    foreach(MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);
                        if (entry.ParentMatchup.Id != 0)
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        if (entry.TeamCompeting.Id != 0)
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        p.Add("@id",0, dbType:DbType.Int32,direction: ParameterDirection.Output);

                        connection.Execute("spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                        entry.Id = p.Get<int>("@id");
                    }
                }
            }
        }
        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            model.Id = p.Get<int>("@id");
        }
        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel pz in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pz.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel tm in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                output = connection.Query<PersonModel>("spPeople_GetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                output = connection.Query<TeamModel>("spTeam_GetAll").ToList();
                foreach(TeamModel tm in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", tm.Id);
                    tm.TeamMembers = connection.Query<PersonModel>("spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        public List<TournamentModel> GetTournaments_All()
        {
            List<TournamentModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                output = connection.Query<TournamentModel>("spTournaments_GetAll").ToList();
                foreach(TournamentModel t in output)
                {
                    
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    // populate prizes
                    t.Prizes = connection.Query<PrizeModel>("spPrizes_GetByTournament",p, commandType: CommandType.StoredProcedure).ToList() ;
                    // populate Teams
                    t.EnteredTeams = connection.Query<TeamModel>("spTeams_GetByTournament",p, commandType:CommandType.StoredProcedure).ToList();
                    foreach(TeamModel tm in t.EnteredTeams)
                    {
                        p = new DynamicParameters();
                        p.Add("@TeamId", tm.Id);
                        tm.TeamMembers = connection.Query<PersonModel>("spTeamMembers_GetByTeam",p, commandType:CommandType.StoredProcedure).ToList();
                    }
                    // populate Rounds
                    p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    List<MatchupModel> matchups = connection.Query<MatchupModel>("spMatchups_GetByTournament",p,commandType:CommandType.StoredProcedure).ToList();
                    // populate all the matchupEntries under each matchup
                    foreach(MatchupModel m in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", m.Id);
                        m.Entries = connection.Query<MatchupEntryModel>("spMatchupEntries_GetByMatchup",p, commandType:CommandType.StoredProcedure).ToList();
                        // populate each entry (2 models: teamCompeting, parentMatchup)
                        List<TeamModel> allTeams = GetTeam_All();
                        foreach(MatchupEntryModel entry in m.Entries)
                        {
                            if(entry.TeamCompetingId > 0)
                            {
                                entry.TeamCompeting = allTeams.Where(x=>x.Id == entry.TeamCompetingId).First();
                            }
                            if(entry.ParentMatchupId > 0)
                            {
                                entry.ParentMatchup = matchups.Where(x=> x.Id == entry.ParentMatchupId).First();
                            }
                        }
                        // populate each matchup (1 model: winner)
                        if(m.WinnerId > 0)
                        {
                            m.Winner = allTeams.Where(x=>x.Id == m.WinnerId).First();
                        }
                    }
                    // List<List<MatchupModel>>
                    List<MatchupModel> currRow = new List<MatchupModel>();
                    int currRound = 1;
                    foreach(MatchupModel matchup in matchups)
                    {
                        // check if this matchup round number is beyond the current round
                        // if yes, we need to add the current round (currRow) into rounds
                        // reset the current round
                        // increase currRound by 1

                        if(matchup.MatchupRound > currRound)
                        {
                            t.Rounds.Add(currRow);
                            currRow = new List<MatchupModel>();
                            currRound++;
                        }
                        currRow.Add(matchup);
                    }
                    // add the last round into t.Rounds
                    t.Rounds.Add(currRow);
                }
            }
            return output;
        }

        public void UpdateMatchup(MatchupModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentsCSC460")))
            {
                var p = new DynamicParameters();
                p.Add("@id", model.Id);
                if (model.Winner.Id != 0)
                {
                    p.Add("@WinderId", model.Winner.Id);
                }
                else
                {
                    p.Add("@WinderId", null);
                }

                connection.Execute("spMatchups_Update", p, commandType: CommandType.StoredProcedure);
                foreach(MatchupEntryModel me in model.Entries)
                {
                    if(me.TeamCompeting.Id != 0)
                    {
                        p = new DynamicParameters();
                        p.Add("@id", me.Id);
                        p.Add("@TeamCompetingId",me.TeamCompeting.Id);
                        p.Add("@Score", me.Score);

                        connection.Execute("spMatchupEntries_Update", p, commandType:CommandType.StoredProcedure);
                    }
                }
            }
        }
    }
}
