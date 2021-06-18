using KomodoInsDevTeamMgmtApp.Classes;
using KomodoInsDevTeamMgmtApp.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KomodoInsDevTeamMgmtApp_UnitTests
{
    [TestClass]
    public class DeveloperRepoTests
    {
        private Developer _developer;
        private DeveloperRepo _developerRepo;

        [TestInitialize]
        public void Arrange()
        {
            _developer = new Developer();
            _developer.FirstName = "Test";
            _developer.LastName = "Dev";
            _developerRepo = new DeveloperRepo();
        }

        // Create
        [TestMethod]
        public void DeveloperAddedToList_ShouldGetNotNull()
        {
            // Arrange @ test initialize

            // Act
            _developerRepo.AddDeveloperToRepo(_developer);
            Developer developerFromRepo = _developerRepo.GetDeveloperByID(_developer.DevID);

            // Assert
            Assert.IsNotNull(_developer);
        }

        // Delete
        [TestMethod]
        public void DeveloperDeletedFromList_ShouldBeTrue()
        {
            // Arrange @ test initialize and...
            _developerRepo.AddDeveloperToRepo(_developer);

            // Act
            bool deleteResult = _developerRepo.DeleteDeveloperByID(_developer.DevID);

            // Assert
            Assert.IsTrue(deleteResult);
        }
    }
}
