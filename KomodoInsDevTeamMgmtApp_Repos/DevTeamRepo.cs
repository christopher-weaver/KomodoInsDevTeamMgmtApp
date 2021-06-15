using System;
using System.Collections.Generic;
using System.Text;
using KomodoInsDevTeamMgmtApp.Classes;

namespace KomodoInsDevTeamMgmtApp.Repos
{
    public class DevTeamRepo
    {
        private List<DevTeam> _listOfDevTeams = new List<DevTeam>();

        // Create
        public void AddDevTeamToRepo(DevTeam devTeam)
        {
            _listOfDevTeams.Add(devTeam);
        }

        // Read
        public List<DevTeam> GetListOfDevTeams()
        {
            return _listOfDevTeams;
        }

        // Update
        public bool UpdateDevTeamByName(string teamName, DevTeam newTeam)
        {
            DevTeam oldTeam = GetDevTeamByName(teamName);

            return UpdateDevTeam(oldTeam, newTeam);
        }

        public bool UpdateDevTeamByID(int teamID, DevTeam newTeam)
        {
            DevTeam oldTeam = GetDevTeamByID(teamID);

            return UpdateDevTeam(oldTeam, newTeam);
        }

        private bool UpdateDevTeam(DevTeam oldTeam, DevTeam newTeam)
        {
            if (oldTeam == null)
            {
                return false;
            }
            else
            {
                oldTeam.TeamName = newTeam.TeamName;
                oldTeam.TeamMembers = newTeam.TeamMembers;
                return true;
            }
        }

        // Delete
        public bool DeleteDevTeamByName(string devTeamName)
        {
            DevTeam devTeam = GetDevTeamByName(devTeamName);

            return DeleteDevTeam(devTeam);
        }

        public bool DeleteDevTeamByID(int devTeamID)
        {
            DevTeam devTeam = GetDevTeamByID(devTeamID);

            return DeleteDevTeam(devTeam);
        }

        private bool DeleteDevTeam(DevTeam devTeam)
        {
            if (devTeam == null)
            {
                return false;
            }

            int initialCount = _listOfDevTeams.Count;
            _listOfDevTeams.Remove(devTeam);

            return _listOfDevTeams.Count < initialCount;
        }

        // Helper functions for update and delete
        public DevTeam GetDevTeamByName(string teamName)
        {
            foreach (DevTeam team in _listOfDevTeams)
            {
                if (teamName.ToLower() == team.TeamName.ToLower())
                {
                    return team;
                }
            }

            return null;
        }

        public DevTeam GetDevTeamByID(int teamID)
        {
            foreach (DevTeam team in _listOfDevTeams)
            {
                if (teamID == team.TeamID)
                {
                    return team;
                }
            }

            return null;
        }
    }
}
