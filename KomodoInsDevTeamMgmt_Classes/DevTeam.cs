using System;
using System.Collections.Generic;
using System.Text;

namespace KomodoInsDevTeamMgmtApp.Classes
{
    public class DevTeam
    {
        public int TeamID { get; }
        public string TeamName { get; set; }
        public List<Developer> TeamMembers { get; set; }

        public DevTeam()
        {
            TeamID = GenerateDevTeamID();
        }

        public DevTeam(string teamName)
        {
            TeamName = teamName;
            TeamID = GenerateDevTeamID();
        }

        public DevTeam(string teamName, List<Developer> teamMembers)
        {
            TeamName = teamName;
            TeamMembers = teamMembers;
            TeamID = GenerateDevTeamID();
        }

        private int GenerateDevTeamID()
        {
            Random rnd = new Random();
            return rnd.Next(100, 999);
        }
    }
}
