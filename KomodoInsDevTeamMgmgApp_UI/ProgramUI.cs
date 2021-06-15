using System;
using System.Collections.Generic;
using System.Text;
using KomodoInsDevTeamMgmtApp.Repos;
using KomodoInsDevTeamMgmtApp.Classes;

namespace KomodoInsDevTeamMgmgApp_UI
{
    class ProgramUI
    {
        private DeveloperRepo _developerRepo = new DeveloperRepo();
        private DevTeamRepo _devTeamRepo = new DevTeamRepo();

        public void Run()
        {
            SeedRepos();
            Console.WriteLine("Komodo Insurance Developer Team Management Application v1.00\n");
            MainMenu();
        }

        public void MainMenu()
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                // Display menu options
                Console.WriteLine("_____________Main Menu_____________\n" +
                    "  1. Add a new developer team\n" +
                    "  2. Add a new developer\n" +
                    "  3. View/update/delete developer teams\n" +
                    "  4. View/update/delete developers\n" +
                    "  5. Add multiple developers to a developer team\n" +
                    "  6. Display developers without Pluralsight access.\n" +
                    "  0. Exit\n\n" +
                    "Please select a menu option:");

                // Get input
                ConsoleKey input = Console.ReadKey().Key;
                string selection = ValidateMenuInput(input, false, false);
                Console.Clear();

                // Display selected menu
                switch (selection)
                {
                    case "1":
                        MenuCreateDevTeam();
                        break;
                    case "2":
                        MenuCreateDeveloper();
                        break;
                    case "3":
                        MenuDisplayDevTeams();
                        break;
                    case "4":
                        MenuDisplayDevelopers();
                        break;
                    case "5":
                        MenuAddMultipleDevsToDevTeam();
                        break;
                    case "6":
                        ReportNoPluralsightAccess();
                        break;
                    case "0":
                        // Exit
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }
        }

        public void MenuCreateDevTeam()
        {
            // Get developer team information
            Console.WriteLine("______________Add Team______________\n" +
                "Developer team name:");
            string teamName = Console.ReadLine();

            // Check to make sure team name is not empty.
            // If empty, exit method without creating a new developer team.
            if (teamName == "")
            {
                Console.WriteLine("Team name cannot be blank.\n" +
                    "Returning to main menu without adding a new developer team.\n");
                Console.ReadKey();
                return;
            }

            // Create new developer team.
            DevTeam devTeam = new DevTeam(teamName);

            // Add developer team to repo
            _devTeamRepo.AddDevTeamToRepo(devTeam);

            Console.Clear();
            Console.WriteLine($"{devTeam.TeamName} (Developer Team ID #{devTeam.TeamID}) has been added to the system.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void MenuCreateDeveloper()
        {
            // Get required developer information
            Console.WriteLine("___________Add Developer___________");
            Developer developer = SetDeveloperName();
            // Check if valid developer was returned.
            if (developer == null) { return; }
            developer = SetDeveloperPluralsightAccess(developer);

            // Add developer to repo
            _developerRepo.AddDeveloperToRepo(developer);
            Console.Clear();
            Console.WriteLine($"{developer.FullName} (Developer ID #{String.Format("{0:000-0000}", developer.DevID)}) has been added to the system.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // SetDeveloperName method overload for creating a new developer.
        public Developer SetDeveloperName()
        {
            return SetDeveloperName(null);
        }

        public Developer SetDeveloperName(Developer developer)
        {
            Console.WriteLine("Developer's first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Developer's last name:");
            string lastName = Console.ReadLine();

            // Test to make sure first and last names are not empty.
            // If empty, exit method without creating a new developer.
            if (firstName == "" || lastName == "")
            {
                Console.WriteLine("Developers must have a first and last name.\n" +
                    "Developer will not be added/updated. Returning to previous menu...\n");
                Console.ReadKey();
                return null;
            }

            // Create new developer if we are not updating an existing developer.
            if (developer == null) { developer = new Developer(); }
            developer.FirstName = firstName;
            developer.LastName = lastName;

            return developer;
        }

        public Developer SetDeveloperPluralsightAccess(Developer developer)
        {
            ConsoleKey pluralsightResponse = default;
            while (pluralsightResponse != ConsoleKey.Y && pluralsightResponse != ConsoleKey.N)
            {
                Console.WriteLine("Does this developer currently have access to Pluralsight? [Y/N]");
                pluralsightResponse = Console.ReadKey().Key;
            }
            developer.CanAccessPluralsight = (pluralsightResponse == ConsoleKey.Y ? true : false);

            return developer;
        }

        public void DisplayDevTeams(List<DevTeam> listOfDevTeams, int displayIndex)
        {
            int pageCount;
            int menuIndex;
            int teamIndex;

            // Reset display variables for .
            // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
            pageCount = listOfDevTeams.Count / 10 + 1;
            if (displayIndex > pageCount - 1) { displayIndex = pageCount - 1; }

            // Display developers, 10 at a time
            Console.WriteLine($"_List of Developer Teams: Page {displayIndex + 1} of {pageCount}___");
            if (listOfDevTeams.Count == 0)
            {
                Console.WriteLine("(no developer teams to display)");
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    teamIndex = displayIndex * 10 + i;
                    if (listOfDevTeams.Count > teamIndex)
                    {
                        // listOfDevelopers uses zero-based index and menu uses options 1, 2, ..., 0.
                        menuIndex = i + 1 < 10 ? i + 1 : 0;
                        Console.WriteLine($"{menuIndex}. {listOfDevTeams[teamIndex].TeamName} (Developer Team ID #{listOfDevTeams[teamIndex].TeamID})");
                    }
                }
            }
        }

        public void DisplayDevelopers(List<Developer> listOfDevelopers, int displayIndex)
        {
            int pageCount;
            int menuIndex;
            int devIndex;

            // Reset display variables for .
            // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
            pageCount = listOfDevelopers.Count / 10 + 1;
            if (displayIndex > pageCount - 1) { displayIndex = pageCount - 1; }

            // Display developers, 10 at a time
            Console.WriteLine($"_List of Developers: Page {displayIndex + 1} of {pageCount}___");
            if (listOfDevelopers.Count == 0)
            {
                Console.WriteLine("(no developers to display)");
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    devIndex = displayIndex * 10 + i;
                    if (listOfDevelopers.Count > devIndex)
                    {
                        // listOfDevelopers uses zero-based index and menu uses options 1, 2, ..., 0.
                        menuIndex = i + 1 < 10 ? i + 1 : 0;
                        Console.WriteLine($"{menuIndex}. {listOfDevelopers[devIndex].FullName} (Developer ID #{String.Format("{0:000-0000}", listOfDevelopers[devIndex].DevID)})");
                    }
                }
            }
        }

        public void MenuDisplayDevTeams()
        {
            List<DevTeam> listOfDevTeams;

            bool continueLoop = true;
            int displayIndex = 0;

            while (continueLoop)
            {
                listOfDevTeams = _devTeamRepo.GetListOfDevTeams();

                DisplayDevTeams(listOfDevTeams, displayIndex);
                Console.WriteLine("\nSelect a developer team [1-0] to view, add, or delete team/members\n" +
                    "or press [N]ext, [P]revious, or [E]xit:");

                // Get input
                ConsoleKey input = Console.ReadKey().Key;
                string selection = ValidateMenuInput(input, true, false);
                Console.Clear();

                switch (selection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "0":
                        // Display developer detail menu.
                        // displayIndex uses zero-based indexing and menu uses one-based indexing.
                        int teamIndex = displayIndex * 10 + (selection == "0" ? 9 : Int32.Parse(selection) - 1);
                        if (teamIndex <= listOfDevTeams.Count - 1)
                        {
                            SubmenuDevTeamDetails(listOfDevTeams[teamIndex]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            Console.ReadKey();
                        }
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfDevTeams.Count / 10 ? displayIndex + 1 : listOfDevTeams.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                        break;
                    case "e":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }
        }

        public void SubmenuDevTeamDetails(DevTeam devTeam)
        {
            bool continueLoop = true;
            int displayIndex = 0;

            List<Developer> listOfTeamMembers;
            if (devTeam.TeamMembers == null)
            {
                listOfTeamMembers = new List<Developer>();
            }
            else
            {
                listOfTeamMembers = devTeam.TeamMembers;
            }

            while (continueLoop)
            {
                Console.WriteLine("_Developer Team Details__________\n" +
                $"Developer team name: {devTeam.TeamName}\n" +
                $"Developer team ID: {devTeam.TeamID}\n" +
                $"Number of team members: {listOfTeamMembers.Count}\n");
                DisplayDevelopers(listOfTeamMembers, displayIndex);
                Console.WriteLine("\nWhat would you like to do?\n");
                Console.WriteLine("Select a developer [1-0] to remove from the team, [D]elete this team, [A]dd a team member not on the list to the team,\n" +
                    "or press [N]ext, [P]revious, or [E]xit to navigate through existing team members:");

                ConsoleKey submenuInput = Console.ReadKey().Key;
                string submenuSelection = ValidateMenuInput(submenuInput, true, true);
                Console.Clear();

                switch (submenuSelection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "0":
                        // displayIndex uses zero-based indexing and menu uses one-based indexing.
                        int devIndex = displayIndex * 10 + (submenuSelection == "0" ? 9 : Int32.Parse(submenuSelection) - 1);
                        if (devIndex <= listOfTeamMembers.Count - 1)
                        {
                            Developer developerToRemove = listOfTeamMembers[devIndex];
                            SubmenuDevTeamRemoveDev(developerToRemove, devTeam, listOfTeamMembers);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            Console.ReadKey();
                        }
                        break;
                    case "a":
                        SubmenuDevTeamAddDev(devTeam, listOfTeamMembers);
                        break;
                    case "d":
                        if (listOfTeamMembers.Count > 0)
                        {
                            Console.WriteLine("You cannot delete a developer team that has members.  Please remove all team members before selecting this option.");
                            Console.ReadKey();
                        }
                        else
                        {
                            bool updateSuccessful = _devTeamRepo.DeleteDevTeamByID(devTeam.TeamID);
                            if (updateSuccessful)
                            {
                                Console.WriteLine("Developer team successfully deleted.");
                            }
                            else
                            {
                                Console.WriteLine("Unable to delete developer team.");
                            }
                            Console.ReadKey();
                            continueLoop = false;
                        }
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfTeamMembers.Count / 10 ? displayIndex + 1 : listOfTeamMembers.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                        break;
                    case "e":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }
        }

        public void SubmenuDevTeamRemoveDev(Developer developerToRemove, DevTeam devTeam, List<Developer> listOfTeamMembers)
        {
            bool updateSuccessful = false;

            listOfTeamMembers.Remove(developerToRemove);
            devTeam.TeamMembers = listOfTeamMembers;

            updateSuccessful = _devTeamRepo.UpdateDevTeamByID(devTeam.TeamID, devTeam);
            // Update developer ID if successfully removed and display message
            if (updateSuccessful)
            {
                developerToRemove.DevID = developerToRemove.DevID % 10000;
                Console.WriteLine($"{developerToRemove.FullName} (Developer ID #{String.Format("{0:000-0000}", developerToRemove.DevID)}) has been successfully removed from {devTeam.TeamName}.");
            }
            else
            {
                Console.WriteLine("Unable to remove developer from team.");
            }
            Console.ReadKey();
        }

        public void SubmenuDevTeamAddDev(DevTeam devTeam, List<Developer>listOfTeamMembers)
        {
            bool continueLoop = true;
            bool updateSuccessful = false;
            int displayIndex = 0;

            while (continueLoop)
            {
                List<Developer> listOfDevelopers = _developerRepo.GetListOfDevelopersWithoutTeam();

                if (listOfDevelopers.Count == 0)
                {
                    Console.WriteLine("All developers have been placed on a team.\n");
                }

                DisplayDevelopers(listOfDevelopers, displayIndex);
                Console.WriteLine($"\nSelect a developer [1-0] to add to {devTeam.TeamName} (Developer Team # {devTeam.TeamID}),\n" +
                    "or press [N]ext, [P]revious, or [E]xit:");

                // Get input
                ConsoleKey input = Console.ReadKey().Key;
                string selection = ValidateMenuInput(input, true, false);
                Console.Clear();

                switch (selection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "0":
                        // Add developer to team.
                        // displayIndex uses zero-based indexing and menu uses one-based indexing.
                        int devIndex = displayIndex * 10 + (selection == "0" ? 9 : Int32.Parse(selection) - 1);
                        if (devIndex <= listOfDevelopers.Count - 1)
                        {
                            Developer developerToAdd = listOfDevelopers[devIndex];
                            listOfTeamMembers.Add(developerToAdd);
                            devTeam.TeamMembers = listOfTeamMembers;

                            // If successful, update developer ID to add team prefix.
                            updateSuccessful = _devTeamRepo.UpdateDevTeamByID(devTeam.TeamID, devTeam);
                            if (updateSuccessful)
                            {
                                developerToAdd.DevID = developerToAdd.DevID + 10000 * devTeam.TeamID;
                                Console.WriteLine($"{developerToAdd.FullName} (Developer ID #{String.Format("{0:000-0000}", developerToAdd.DevID)}) has been successfully added to {devTeam.TeamName}.");
                            }
                            else
                            {
                                Console.WriteLine("Unable to add developer to team.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                        Console.ReadKey();
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfDevelopers.Count / 10 ? displayIndex + 1 : listOfDevelopers.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                        break;
                    case "e":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }
        }

        public void MenuDisplayDevelopers()
        {
            ConsoleKey filterResponse = default;
            List<Developer> listOfDevelopers;

            while (filterResponse != ConsoleKey.Y && filterResponse != ConsoleKey.N)
            {
                Console.Clear();
                Console.WriteLine("Would you like to only display developers that are not on a developer team? [Y/N]");
                filterResponse = Console.ReadKey().Key;
            }
            Console.Clear();

            bool continueLoop = true;
            int displayIndex = 0;

            while (continueLoop)
            {
                if (filterResponse == ConsoleKey.Y)
                {
                    listOfDevelopers = _developerRepo.GetListOfDevelopersWithoutTeam();
                    if (listOfDevelopers.Count == 0)
                    {
                        Console.WriteLine("All developers have been placed on a team.");
                    }
                }
                else
                {
                    listOfDevelopers = _developerRepo.GetListOfDevelopers();
                }

                DisplayDevelopers(listOfDevelopers, displayIndex);
                Console.WriteLine("\nSelect a developer [1-0] for additional details\n" +
                    "or press [N]ext, [P]revious, or [E]xit:");

                // Get input
                ConsoleKey input = Console.ReadKey().Key;
                string selection = ValidateMenuInput(input, true, false);
                Console.Clear();

                switch (selection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "0":
                        // Display developer detail menu.
                        // displayIndex uses zero-based indexing and menu uses one-based indexing.
                        int devIndex = displayIndex * 10 + (selection == "0" ? 9 : Int32.Parse(selection) - 1);
                        if (devIndex <= listOfDevelopers.Count - 1)
                        {
                            SubmenuDeveloperDetails(listOfDevelopers[devIndex]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            Console.ReadKey();
                        }
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfDevelopers.Count / 10 ? displayIndex + 1 : listOfDevelopers.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1 ;
                        break;
                    case "e":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }
        }

        public void SubmenuDeveloperDetails(Developer developer)
        {
            bool updateSuccessful = false;
            // Developers who are on a team will 7-digit developer ID.  Otherwise, they will have a 4-digit ID.
            bool developerOnTeam = developer.DevID > 10000;

            Console.WriteLine("_Developer Details__________\n" +
                $"Developer name: {developer.FullName}\n" +
                $"Developer ID: {String.Format("{0:000-0000}", developer.DevID)}\n" +
                $"Can access Pluralsight?: {developer.CanAccessPluralsight}\n");

            Console.WriteLine("What would you like to do?\n");
            // Display different option depending on whether or not the developer is on a team.
            if (developerOnTeam)
            {
                Console.WriteLine("Update developer [N]ame, update developer's [P]luralsight access,\n" +
                    "remove developer from current [T]eam, or [D]elete developer.");
            }
            else
            {
                Console.WriteLine("Update developer [N]ame, update developer's [P]luralsight access,\n" +
                    "    add developer to a [T]eam, or [D]elete developer.");
            }
            Console.WriteLine("Any other key will return to the developer list...");

            ConsoleKey submenuSelection = Console.ReadKey().Key;
            Console.Clear();

            switch (submenuSelection)
            {
                case ConsoleKey.N:
                    developer = SetDeveloperName(developer);
                    // SetDeveloperName returns null if invalid input was given.
                    if (developer != null)
                    {
                        updateSuccessful = _developerRepo.UpdateDeveloperByID(developer.DevID, developer);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case ConsoleKey.P:
                    developer = SetDeveloperPluralsightAccess(developer);
                    updateSuccessful = _developerRepo.UpdateDeveloperByID(developer.DevID, developer);
                    break;
                case ConsoleKey.T:
                    // If developer is on a team, remove them from that team, else display menu to add them to a team.
                    if (developerOnTeam)
                    {
                        int teamID = developer.DevID / 10000;
                        DevTeam devTeam = _devTeamRepo.GetDevTeamByID(teamID);
                        List<Developer> teamMembers = devTeam.TeamMembers;
                        teamMembers.Remove(developer);
                        devTeam.TeamMembers = teamMembers;
                        
                        // Reset the developer's ID to the last 4 digits of that ID and update.
                        developer.DevID = developer.DevID % 10000;
                        updateSuccessful = _developerRepo.UpdateDeveloperByID(developer.DevID, developer);
                    }
                    else
                    {
                        bool continueLoop = true;
                        int displayIndex = 0;
                        List<DevTeam> listOfDevTeams = _devTeamRepo.GetListOfDevTeams();

                        while (continueLoop)
                        {
                            DisplayDevTeams(listOfDevTeams, 0);
                            Console.WriteLine($"\nSelect a developer team [1-0] to which you would like to add {developer.DevID} (Developer ID #{String.Format("{0:000-0000}", developer.DevID)})\n" +
                                "or press [N]ext, [P]revious, or [E]xit:");

                            // Get input
                            ConsoleKey teamMenuKey = Console.ReadKey().Key;
                            string teamMenuSelection = ValidateMenuInput(teamMenuKey, true, false);
                            Console.Clear();

                            switch (teamMenuSelection)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                case "0":
                                    // Display developer team member menu.
                                    // displayIndex uses zero-based indexing and menu uses one-based indexing.
                                    int teamIndex = displayIndex * 10 + (teamMenuSelection == "0" ? 9 : Int32.Parse(teamMenuSelection) - 1);
                                    if (teamIndex <= listOfDevTeams.Count - 1)
                                    {
                                        // Add selected developer to team members property of selected DevTeam object
                                        List<Developer> teamMembers;
                                        if (listOfDevTeams[teamIndex].TeamMembers == null)
                                        {
                                            teamMembers = new List<Developer>();
                                        }
                                        else
                                        {
                                            teamMembers = listOfDevTeams[teamIndex].TeamMembers;
                                        }
                                        teamMembers.Add(developer);
                                        listOfDevTeams[teamIndex].TeamMembers = teamMembers;

                                        // Update developer ID prefix and update developer team repo
                                        updateSuccessful = _devTeamRepo.UpdateDevTeamByID(listOfDevTeams[teamIndex].TeamID, listOfDevTeams[teamIndex]);
                                        if (updateSuccessful) { developer.DevID = listOfDevTeams[teamIndex].TeamID * 10000 + developer.DevID; }
                                        continueLoop = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input.");
                                    }
                                    break;
                                case "n":
                                    // Go to next page, max at page count.
                                    // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                                    displayIndex = displayIndex < listOfDevTeams.Count / 10 ? displayIndex + 1 : listOfDevTeams.Count / 10;
                                    break;
                                case "p":
                                    // Go to previous page, min at page 1.
                                    displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                                    break;
                                case "e":
                                    // Exit menu.
                                    continueLoop = false;
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid menu selection.");
                                    break;
                            }
                            Console.Clear();
                        }
                    }
                    break;
                case ConsoleKey.D:
                    ConsoleKey confirm = default;
                    while (confirm != ConsoleKey.N && confirm != ConsoleKey.Y)
                    {
                        Console.WriteLine($"Are you sure you want to delete {developer.FullName} (Developer ID #{String.Format("{0:000-0000}", developer.DevID)})? [Y/N]");
                        confirm = Console.ReadKey().Key;
                        Console.Clear();
                    }

                    switch (confirm)
                    {
                        case ConsoleKey.Y:
                            // Check to see if developer is on a team; if so, remove them from that team.
                            if (developer.DevID > 9999)
                            {
                                int teamID = developer.DevID / 10000;
                                DevTeam devTeam = _devTeamRepo.GetDevTeamByID(teamID);
                                List<Developer> teamMembers = devTeam.TeamMembers;
                                teamMembers.Remove(developer);
                                devTeam.TeamMembers = teamMembers;
                            }
                            updateSuccessful = _developerRepo.DeleteDeveloperByID(developer.DevID);
                            break;
                        case ConsoleKey.N:
                            Console.WriteLine("Developer was not deleted.");
                            return;
                        default:
                            return;
                    }
                    break;
                default:
                    return;
            }

            Console.Clear();
            if (updateSuccessful)
            {
                Console.WriteLine($"{developer.FullName} (Developer ID #{String.Format("{0:000-0000}", developer.DevID)}) has been successfully {(submenuSelection == ConsoleKey.D ? "deleted" : "updated")}.");
            }
            else
            {
                Console.WriteLine("No changes were made.");
            }
            Console.ReadKey();
        }

        public void MenuAddMultipleDevsToDevTeam()
        {
            List<DevTeam> listOfDevTeams;

            bool continueLoop = true;
            int displayIndex = 0;

            while (continueLoop)
            {
                listOfDevTeams = _devTeamRepo.GetListOfDevTeams();

                DisplayDevTeams(listOfDevTeams, displayIndex);
                Console.WriteLine("\nSelect a developer team [1-0],\n" +
                    "or press [N]ext, [P]revious, or [E]xit:");

                // Get input
                ConsoleKey input = Console.ReadKey().Key;
                string selection = ValidateMenuInput(input, true, false);
                Console.Clear();

                switch (selection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "0":
                        // Display developer detail menu.
                        // displayIndex uses zero-based indexing and menu uses one-based indexing.
                        int teamIndex = displayIndex * 10 + (selection == "0" ? 9 : Int32.Parse(selection) - 1);
                        if (teamIndex <= listOfDevTeams.Count - 1)
                        {
                            SubmenuAddMultipleDevsToDevTeam(listOfDevTeams[teamIndex]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            Console.ReadKey();
                        }
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfDevTeams.Count / 10 ? displayIndex + 1 : listOfDevTeams.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                        break;
                    case "e":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu selection.");
                        break;
                }
                Console.Clear();
            }

        }

        public void SubmenuAddMultipleDevsToDevTeam(DevTeam devTeam)
        {
            List<Developer> listOfDevelopers;

            List<Developer> listOfTeamMembers;
            if (devTeam.TeamMembers == null)
            {
                listOfTeamMembers = new List<Developer>();
            }
            else
            {
                listOfTeamMembers = devTeam.TeamMembers;
            }

            bool continueLoop = true;
            int displayIndex = 0;

            while (continueLoop)
            {
                listOfDevelopers = _developerRepo.GetListOfDevelopersWithoutTeam();
                if (listOfDevelopers.Count == 0)
                {
                    Console.WriteLine("All developers have been placed on a team.");
                }

                DisplayDevelopers(listOfDevelopers, displayIndex);
                Console.WriteLine("\nSelect a developers [1-0] to add to the developer team (separated by a comma, e.g. \"1,2,4,5\")\n" +
                    "or press [N]ext, [P]revious, or [E]xit:");

                // Get input and check if string is numeric (with commas removed) or equal to one of the other menu options
                string selection = Console.ReadLine().ToLower();
                Console.Clear();
                if (Int32.TryParse(selection.Replace(",",""), out int i) == false  && selection != "n" && selection != "p" && selection != "e" && selection != "x")
                {
                    selection = "error";
                }

                switch (selection)
                {
                    case "error":
                        Console.WriteLine("Incorrect input detected.");
                        Console.ReadKey();
                        break;
                    case "n":
                        // Go to next page, max at page count.
                        // displayIndex uses zero-based indexing and pageCount uses one-based indexing.
                        displayIndex = displayIndex < listOfDevelopers.Count / 10 ? displayIndex + 1 : listOfDevelopers.Count / 10;
                        break;
                    case "p":
                        // Go to previous page, min at page 1.
                        displayIndex = displayIndex == 0 ? 0 : displayIndex - 1;
                        break;
                    case "e":
                    case "x":
                        // Exit menu.
                        continueLoop = false;
                        break;
                    default:
                        List<int> developersAdded = new List<int>();
                        foreach (char c in selection.Replace(",", ""))
                        {
                            Int32.TryParse(c.ToString(), out i);
                            // listOfDevelopers uses zero-based index and menu uses options 1, 2, ..., 0.
                            int devIndex = displayIndex * 10 + (i == 0 ? 9 : i - 1);
                            if (devIndex <= listOfDevelopers.Count - 1 && !developersAdded.Contains(devIndex))
                            {
                                int initialTeamCount = listOfTeamMembers.Count;

                                Developer developerToAdd = listOfDevelopers[devIndex];
                                listOfTeamMembers.Add(developerToAdd);

                                // Check to make sure developer was successfully added to list
                                if (listOfTeamMembers.Count == initialTeamCount + 1)
                                {
                                    devTeam.TeamMembers = listOfTeamMembers;
                                    developersAdded.Add(devIndex);

                                    // Update developer ID with team ID prefix
                                    developerToAdd.DevID = developerToAdd.DevID + 10000 * devTeam.TeamID;
                                    Console.WriteLine($"{developerToAdd.FullName} (Developer ID #{String.Format("{0:000-0000}", developerToAdd.DevID)}) queued to be added to {devTeam.TeamName}.");
                                }
                            }
                        }
                        // If successful, update developer ID to add team prefix.
                        bool updateSuccessful = _devTeamRepo.UpdateDevTeamByID(devTeam.TeamID, devTeam);
                        if (updateSuccessful)
                        {
                            Console.WriteLine($"\nThe requested developers have been successfully added to {devTeam.TeamName} (Team ID #{devTeam.TeamID}).");
                        }
                        else
                        {
                            // Reset prefix for all queued developers if team update was unsuccessful
                            foreach (int j in developersAdded)
                            {
                                listOfDevelopers[j].DevID = listOfDevelopers[j].DevID % 10000;
                            }
                            Console.WriteLine("\nUnable to add the requested developers to the requested team.");
                        }
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
            }
        }

        public void ReportNoPluralsightAccess()
        {
            List<Developer> developersWithoutPluraslight = _developerRepo.GetListOfDevelopersWithoutPluralsight();

            if (developersWithoutPluraslight.Count == 0)
            {
                Console.WriteLine("All developers have access to Pluralsight.");
            }
            else
            {
                Console.WriteLine($"As of {DateTime.Today.ToString("dd/MM/yyyy")}, the following {developersWithoutPluraslight.Count} developers do not have access to Pluralsight:\n");
                foreach (Developer dev in developersWithoutPluraslight)
                {
                    Console.WriteLine($"{dev.FullName} (Developer ID #{String.Format("{0:000-0000}", dev.DevID)})");
                }
                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            }
        }

        public string ValidateMenuInput(ConsoleKey input, bool nextPrevExit, bool addDelete)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return "1";
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return "2";
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return "3";
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    return "4";
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    return "5";
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    return "6";
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    return "7";
                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    return "8";
                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    return "9";
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return "0";
                case ConsoleKey.N:
                case ConsoleKey.RightArrow:
                case ConsoleKey.PageDown:
                case ConsoleKey.Enter:
                    return nextPrevExit? "n" : "";
                case ConsoleKey.P:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.PageUp:
                    return nextPrevExit ? "p" : "";
                case ConsoleKey.X:
                case ConsoleKey.E:
                case ConsoleKey.Escape:
                    return nextPrevExit ? "e" : "";
                case ConsoleKey.A:
                    return addDelete ? "a" : "";
                case ConsoleKey.D:
                    return addDelete ? "d" : "";
                default:
                    return "";
            }
        }

        public void SeedRepos()
        {
            Developer dev1 = new Developer("Gertrudis", "Waterman", false);
            Developer dev2 = new Developer("Jun-Ho", "Lamarre", false);
            Developer dev3 = new Developer("Peter", "Muller", true);
            Developer dev4 = new Developer("Arif", "Waldvogel", false);
            Developer dev5 = new Developer("Jad", "Kellogg", false);
            Developer dev6 = new Developer("Drusa", "Reed", true);
            Developer dev7 = new Developer("Ekaitz", "Sampson", false);
            Developer dev8 = new Developer("Marcio", "Rudawski", true);
            Developer dev9 = new Developer("Windsor", "Rapp", false);
            Developer dev10 = new Developer("Borivoj", "Outterridge", false);
            Developer dev11 = new Developer("Stepan", "Botha", false);
            Developer dev12 = new Developer("Hesekiel", "Solyom", true);
            Developer dev13 = new Developer("Thankful", "Vega", false);
            Developer dev14 = new Developer("Amin", "Steinmann", true);
            Developer dev15 = new Developer("Cacilia", "Miyajima", true);

            DevTeam devTeam1 = new DevTeam("Red");
            DevTeam devTeam2 = new DevTeam("Blue");
            DevTeam devTeam3 = new DevTeam("Yellow");
            DevTeam devTeam4 = new DevTeam("Gold");
            DevTeam devTeam5 = new DevTeam("Purple");

            List<Developer> team1Members = new List<Developer>();
            team1Members.Add(dev1);
            dev1.DevID = dev1.DevID + devTeam1.TeamID * 10000;
            team1Members.Add(dev2);
            dev2.DevID = dev2.DevID + devTeam1.TeamID * 10000;
            team1Members.Add(dev3);
            dev3.DevID = dev3.DevID + devTeam1.TeamID * 10000;
            devTeam1.TeamMembers = team1Members;

            List<Developer> team2Members = new List<Developer>();
            team2Members.Add(dev11);
            dev11.DevID = dev11.DevID + devTeam2.TeamID * 10000;
            team2Members.Add(dev12);
            dev12.DevID = dev12.DevID + devTeam2.TeamID * 10000;
            team2Members.Add(dev13);
            dev13.DevID = dev13.DevID + devTeam2.TeamID * 10000;
            devTeam2.TeamMembers = team2Members;

            _developerRepo.AddDeveloperToRepo(dev1);
            _developerRepo.AddDeveloperToRepo(dev2);
            _developerRepo.AddDeveloperToRepo(dev3);
            _developerRepo.AddDeveloperToRepo(dev4);
            _developerRepo.AddDeveloperToRepo(dev5);
            _developerRepo.AddDeveloperToRepo(dev6);
            _developerRepo.AddDeveloperToRepo(dev7);
            _developerRepo.AddDeveloperToRepo(dev8);
            _developerRepo.AddDeveloperToRepo(dev9);
            _developerRepo.AddDeveloperToRepo(dev10);
            _developerRepo.AddDeveloperToRepo(dev11);
            _developerRepo.AddDeveloperToRepo(dev12);
            _developerRepo.AddDeveloperToRepo(dev13);
            _developerRepo.AddDeveloperToRepo(dev14);
            _developerRepo.AddDeveloperToRepo(dev15);

            _devTeamRepo.AddDevTeamToRepo(devTeam1);
            _devTeamRepo.AddDevTeamToRepo(devTeam2);
            _devTeamRepo.AddDevTeamToRepo(devTeam3);
            _devTeamRepo.AddDevTeamToRepo(devTeam4);
            _devTeamRepo.AddDevTeamToRepo(devTeam5);
        }
    }
}
