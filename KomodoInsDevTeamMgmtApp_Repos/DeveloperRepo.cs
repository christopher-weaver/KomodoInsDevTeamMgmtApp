using System;
using System.Collections.Generic;
using System.Text;
using KomodoInsDevTeamMgmtApp.Classes;

namespace KomodoInsDevTeamMgmtApp.Repos
{
    public class DeveloperRepo
    {
        private List<Developer> _listOfDevelopers = new List<Developer>();

        // Create
        public void AddDeveloperToRepo(Developer developer)
        {
            _listOfDevelopers.Add(developer);
        }

        // Read
        public List<Developer> GetListOfDevelopers()
        {
            return _listOfDevelopers;
        }

        public List<Developer> GetListOfDevelopersWithoutTeam()
        {
            List<Developer> listOfDevelopers = new List<Developer>();
            
            foreach (Developer developer in _listOfDevelopers)
            {
                // Developers without a team will have a developer ID less than 10000.
                if (developer.DevID < 10000)
                {
                    listOfDevelopers.Add(developer);
                }
            }

            return listOfDevelopers;
        }

        public List<Developer> GetListOfDevelopersWithoutPluralsight()
        {
            List<Developer> listOfDevelopers = new List<Developer>();

            foreach (Developer developer in _listOfDevelopers)
            {
                if (!developer.CanAccessPluralsight)
                {
                    listOfDevelopers.Add(developer);
                }
            }

            return listOfDevelopers;
        }

        // Update
        // This one is currently unused.
        private bool UpdateDeveloperByName(string developerName, Developer newDeveloper)
        {
            Developer oldDeveloper = GetDeveloperByFullName(developerName);

            return UpdateDeveloper(oldDeveloper, newDeveloper);
        }

        public bool UpdateDeveloperByID(int developerID, Developer newDeveloper)
        {
            Developer oldDeveloper = GetDeveloperByID(developerID);

            return UpdateDeveloper(oldDeveloper, newDeveloper);
        }

        private bool UpdateDeveloper(Developer oldDeveloper, Developer newDeveloper)
        {
            if (oldDeveloper == null)
            {
                return false;
            }
            else
            {
                oldDeveloper.DevID = newDeveloper.DevID;
                oldDeveloper.FirstName = newDeveloper.FirstName;
                oldDeveloper.LastName = newDeveloper.LastName;
                oldDeveloper.CanAccessPluralsight = newDeveloper.CanAccessPluralsight;
                return true;
            }
        }

        // Delete
        // This one is currently unused.
        public bool DeleteDeveloperByName(string developerName)
        {
            Developer developer = GetDeveloperByFullName(developerName);

            return DeleteDeveloper(developer);
        }

        public bool DeleteDeveloperByID(int developerID)
        {
            Developer developer = GetDeveloperByID(developerID);

            return DeleteDeveloper(developer);
        }

        private bool DeleteDeveloper(Developer developer)
        {
            if (developer == null)
            {
                return false;
            }

            int initialCount = _listOfDevelopers.Count;
            _listOfDevelopers.Remove(developer);

            return _listOfDevelopers.Count < initialCount;
        }


        // Helper functions for update and delete
        private Developer GetDeveloperByFullName(string developerName)
        {
            foreach (Developer developer in _listOfDevelopers)
            {
                if (developerName.ToLower() == developer.FullName.ToLower())
                {
                    return developer;
                }
            }

            return null;
        }

        public Developer GetDeveloperByID(int developerID)
        {
            // Get last 4 of Developer ID since this is the part of the ID
            // that is essentially read-only.
            developerID = developerID % 10000;

            foreach (Developer developer in _listOfDevelopers)
            {
                // Again, getting the last 4 digits of each developer ID to compare.
                if (developerID == (developer.DevID % 10000))
                {
                    return developer;
                }
            }

            return null;
        }
    }
}
