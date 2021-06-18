using KomodoInsDevTeamMgmtApp.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KomodoInsDevTeamMgmtApp_UnitTests
{
    [TestClass]
    public class DeveloperTests
    {
        [TestMethod]
        public void SetDevID_ShouldSetCorrectInt()
        {
            // Arrange
            Developer developer = new Developer();

            // Act
            int expected = developer.DevID + 9990000;
            developer.DevID = developer.DevID + 9990000;
            int actual = developer.DevID;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
