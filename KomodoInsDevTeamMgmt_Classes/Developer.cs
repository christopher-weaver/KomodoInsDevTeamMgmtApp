using System;
using System.Collections.Generic;
using System.Text;

namespace KomodoInsDevTeamMgmtApp.Classes
{
    public class Developer
    {
        private int _devID;

        // 000-#### if not assigned to a team;
        // %%%-#### if assigned to team with ID %%%.
        public int DevID
        {
            get
            {
                return _devID;
            }

            // The last 4 digits of a DevID are essentially read-only.
            // Once generated when a Developer object is constructed,
            // only the %%% 3-digit pre-fix can be altered.  This is
            // used when a developer moves to a different team.
            set
            {
                if ((value % 10000 == _devID || _devID % 10000 == value) && value <= 9999999)
                {
                    _devID = value;
                }
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public bool CanAccessPluralsight { get; set; }

        public Developer()
        {
            _devID = GenerateDevID();
        }

        public Developer(string firstName, string lastName, bool canAccessPluralsight)
        {
            FirstName = firstName;
            LastName = lastName;
            CanAccessPluralsight = canAccessPluralsight;
            _devID = GenerateDevID();
        }

        private int GenerateDevID()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 9999);
        }
    }
}
