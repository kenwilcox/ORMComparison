using ORMComparison.DataContracts;
using System;

namespace ORMComparison
{
    interface IAccessor
    {
        Athlete FindAthlete(long athleteId);
        Athlete[] FindByPosition(string position);
        Athlete SaveAthlete(ORMComparison.DataContracts.Athlete athlete);
        TeamAthletes FindTeamWithAthletes(long teamId);
    }
}
