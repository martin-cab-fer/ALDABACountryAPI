using ALDABACountryAPI;
using ALDABACountryAPI.Controllers;
using ALDABACountryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace APITests
{
    [TestFixture]
    public class APIUnitTests
    {
        private CountryApiController controller;
        private DtoCountryData testDto;

        [SetUp]
        public void Setup()
        {
            controller = new CountryApiController();
            testDto = new DtoCountryData()
            {
                name = new DtoCountryNames()
                {
                    common = "Spain",
                    official = "Kingdom of Spain",
                },
                capital = new List<string>() { "Madrid" },
                borders = new List<string>() { "FRA" },
                languages = new Dictionary<string, string>(){ { "spa", "Spanish" } },
                population = 8000000,
                region = "Europe",
                translations = new Dictionary<string, DtoCountryNames>() { { "spa", new DtoCountryNames()
                        {
                            common = "Espa�a",
                            official = "Reino de Espa�a",
                        }
                    }
                },
                flags = new DtoCountryFlags()
                {
                    png = "test.png",
                    svg = "test.svg"
                }
            };
        }

        [Test]
        public void GetByName_WithEmptyName()
        {
            string testName = "";
            var actionResult = controller.GetByName(testName);

            var result = actionResult.Result as BadRequestObjectResult;

            Assert.That(result != null);
        }

        [Test]
        public void GetAdjacentByName_WithEmptyName()
        {
            string testName = "";
            var actionResult = controller.GetAdjacentByName(testName);

            var result = actionResult.Result as BadRequestObjectResult;

            Assert.That(result != null);
        }

        [Test]
        public void CountryDataCreation()
        {
            CountryData data = new CountryData(testDto);

            Assert.That(data.OfficialName == "Espa�a (Kingdom of Spain)");
            Assert.That(data.CapitalName == "Madrid");
            Assert.That(data.OfficialLanguages.Contains("Spanish"));
            Assert.That(data.Population == 8000000);
            Assert.That(data.RegionName == "Europe");
        }

        [Test]
        public void AdjacentCountryDataCreation()
        {
            AdjacentCountryData data = new AdjacentCountryData(testDto);

            Assert.That(data.CountryName == "Espa�a (Kingdom of Spain)");
            Assert.That(data.CapitalName == "Madrid");
            Assert.That(data.Population == 8000000);
        }
    }
}