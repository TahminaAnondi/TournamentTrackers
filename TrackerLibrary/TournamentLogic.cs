using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order our list randomly of teams
        // Check if it is big enough - if not, add in byes - 2^n
        // Create our first round of matchups
        // Create every round after that - 8 matchups - 4 matchups - 2 matchups - 1 matchup
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);

            int rounds = FindNumberOfRound(randomizedTeams.Count);

            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
        }
        private static void CreateOtherRounds(TournamentModel model,int rounds) 
        {
            // we create new rounds from 2nd round
            int round = 2;
            // create list of MatchupModel to hold the previous round, the info of new round is 
            // getting from the previous round
            List<MatchupModel> previousRound = model.Rounds[0];
            // create list of MatchupModel to hold the current round
            List<MatchupModel> currRound = new List<MatchupModel>();
            // create a temp Matchup to hold the newly created matchup
            MatchupModel currMatchup = new MatchupModel();

            while(round <= rounds)
            {
                foreach(MatchupModel matchup in previousRound)
                {
                    // add a new matchup entry to currMatchup, whose parent is matchup
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = matchup });
                    if(currMatchup.Entries.Count > 1)
                    {
                        // add current round number to currMatchup
                        currMatchup.MatchupRound = round;
                        // add currMatchup into currRound
                        currRound.Add(currMatchup);
                        // reset currMatchup to new
                        currMatchup = new MatchupModel();
                    }
                }
                // add currRound into tournament rounds
                model.Rounds.Add(currRound);
                // update previous round with current round
                previousRound = currRound;
                // reset currRound to new
                currRound = new List<MatchupModel>();
                // update round to avoid the infinite loop
                round++;
            }
        }
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            // create a temp holder to hold a matchup
            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                // add an matchup entry into curr
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
                // if we still have bye or we already add two team entries into the matchup
                // we set the matchupRound as 1 since we are creating the first round
                // we add the current matchup into output
                // refresh the curr as new matchup
                // further check if the number of bye is not 0
                // if yes, we need to decrease bye by 1
                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();
                    if(byes > 0)
                    {
                        byes--;
                    }

                }
            }
            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = (int)Math.Pow(2, rounds);
            output = totalTeams - numberOfTeams;
            return output;
        }
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // Guid.NewGuid() will assign a new identifier to each item in the container of teams
            // each identifier is unique which is generated randomly to make the reordered list 
            // random enough
            return teams.OrderBy(x=> Guid.NewGuid()).ToList();
        }
        private static int FindNumberOfRound(int teamCount)
        {
            int output = 1;
            int val = 2;
            while(val < teamCount)
            {
                output++;
                val *= 2;
            }
            return output;
        }
    }
}
