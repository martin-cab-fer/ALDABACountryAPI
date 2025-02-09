using ALDABACountryAPI.Controllers;
using ALDABACountryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    [TestFixture]
    public class APIIntegrationTests
    {
        private CountryApiController controller;

        [SetUp]
        public void Setup()
        {
            controller = new CountryApiController();
        }

        [Test]
        public void GetByName_ValidCountry()
        {
            string countryName = "Spain";
            var actionResult = controller.GetByName(countryName);

            var result = actionResult.Result as OkObjectResult;

            Assert.That(result != null);

            string data = result.Value as string;

            Assert.That(!string.IsNullOrEmpty(data));
            Assert.That(data.Contains("Nombre oficial: España (Kingdom of Spain)"));
            Assert.That(data.Contains("Capital: Madrid"));
            Assert.That(data.Contains("Idiomas oficiales: Spanish"));
            Assert.That(data.Contains("Región: Europe"));
        }

        [Test]
        public void GetByName_InvalidCountry()
        {
            string countryName = "TestCountry";
            var actionResult = controller.GetByName(countryName);

            var result = actionResult.Result as NotFoundObjectResult;

            Assert.That(result != null);
        }

        [Test]
        public void GetAdjacentByName_ValidCountry()
        {
            string countryName = "Spain";
            var actionResult = controller.GetAdjacentByName(countryName);

            var result = actionResult.Result as OkObjectResult;

            Assert.That(result != null);

            string data = result.Value as string;

            Assert.That(!string.IsNullOrEmpty(data));
            Assert.That(data.Contains("Nombre: Portugal (Portuguese Republic)"));
            Assert.That(data.Contains("Capital: Lisbon"));
        }

        [Test]
        public void GetAdjacentByName_InvalidCountry()
        {
            string countryName = "TestCountry";
            var actionResult = controller.GetAdjacentByName(countryName);

            var result = actionResult.Result as NotFoundObjectResult;

            Assert.That(result != null);
        }

        [Test]
        public void GetAdjacentByName_CountryWithoutBorders()
        {
            string countryName = "Australia";
            var actionResult = controller.GetAdjacentByName(countryName);

            var result = actionResult.Result as NotFoundObjectResult;

            Assert.That(result != null);
            Assert.That(result.Value != null);
        }
    }
}
