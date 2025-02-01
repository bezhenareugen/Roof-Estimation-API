using RoofEstimation.BLL.Services.TearOffService;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Entities.TearOff;
using RoofEstimation.Models.Constants;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.Api.UnitTests;

public class Tests
{
    [TestFixture]
    public class TearOffServiceTests
    {
        private TearOffService _tearOffService;

        [SetUp]
        public void Setup()
        {
            _tearOffService = new TearOffService();
        }

        [Test]
        public void GetCalculatedTearOffs()
        {
            // Arrange
            var tearOffs = TearOffsRealData.TearOffs;
            var roofInfo = RoofInfosRealData.RoofInfos.FirstOrDefault();

            // Act
            var result = _tearOffService.GetCalculatedTearOffs(tearOffs, roofInfo);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(tearOffs, Has.Count.EqualTo(result.TearOffWithPrices.Count));
                Assert.That(result.Total, Is.EqualTo(2346));
            });
        }
    }
}