using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SendInBlue.Models;
using SendInBlue.Tests.Properties;

namespace SendInBlue.Tests
{
    public class UserClientTests
    {
        [Test]
        public async Task UpsertUserTest()
        {
            var userClient = new SendInBlueClient(Settings.Default.ApiKey, Settings.Default.TrackingIdKey);
            var user = UserModel.Create("jappleseed@apple.com", new Dictionary<string, string>
            {
                {"FirstName", "Johnny" },
                {"LastName", "Appleseed" }
            });

            var returnVal = await userClient.UpsertUserAsync(user);

            Assert.AreEqual("success", returnVal.Code);
        }

        [Test]
        public async Task TrackEventTest()
        {
            var userClient = new SendInBlueClient(Settings.Default.ApiKey, Settings.Default.TrackingIdKey);

            var result = await userClient.SendTrackEventAsync("jappleseed@apple.com", "testEvent");

            Assert.AreEqual("NoContent", result.Code);
        }
    }
}
