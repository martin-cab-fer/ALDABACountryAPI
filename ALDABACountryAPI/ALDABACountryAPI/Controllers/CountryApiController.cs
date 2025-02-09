using ALDABACountryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ALDABACountryAPI.Controllers
{
    [ApiController]
    [Route("country")]
    [ResponseCache(Duration = 120 )]
    public class CountryApiController : ControllerBase
    {
        public readonly string TargetAPIUrl = "https://restcountries.com/v3.1/";

        public CountryApiController()
        {
        }

        /// <summary>
        /// Devuelve información detallada de un país en base a un nombre o código ISO
        /// </summary>
        /// <param name="nameOrCode">El nombre o código ISO del país</param>
        /// <returns>Información detallada del país encontrado</returns>
        /// <response code="200">Devuelve la información detallada del país</response>
        /// <response code="400">En caso de un nombre/ISO inválido</response>
        /// <response code="404">En caso de no encontrar ningún país</response>
        /// <response code="500">En caso de surgir un error inesperado</response>
        /// <response code="503">En caso de que restcountries.com no esté disponible</response>
        [HttpGet]
        [Route("{nameOrCode?}")]
        public async Task<ActionResult> GetByName(string nameOrCode)
        {
            try
            {
                if (string.IsNullOrEmpty(nameOrCode)) {
                    return BadRequest("El nombre o código ISO no puede ser vacío.");
                }

                bool isISOName = nameOrCode.Length <= 3;

                string apiRouteEnd = isISOName ? "alpha/" : "name/";

                CountryData result;

                string finalURL = TargetAPIUrl + apiRouteEnd + nameOrCode;
                Task<HttpResponseMessage> apiResult = RequestCountryData(finalURL);
                apiResult.Wait();

                if (apiResult.Result == null || !apiResult.Result.IsSuccessStatusCode)
                {
                    if (apiResult.Result != null && apiResult.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return NotFound("No se ha encontrado ningún país con el nombre o código " + nameOrCode);
                    else
                        return StatusCode(503, "restcountries.com no está disponible.");
                }
                else
                {
                    string parsedContent = await apiResult.Result.Content.ReadAsStringAsync();
                    List<DtoCountryData> parsedDto = JsonSerializer.Deserialize<List<DtoCountryData>>(parsedContent);
                    DtoCountryData firstMatch = parsedDto.FirstOrDefault();
                    if (firstMatch == null)
                        return NotFound("No se ha encontrado ningún país con el nombre o código " + nameOrCode);

                    result = new CountryData(firstMatch);
                }

                return Ok(result.DataToText);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ha ocurrido un error inesperado.");
            }
        }

        /// <summary>
        /// Devuelve una lista con información básica de los país adyacentes al en base a un nombre o código ISO
        /// </summary>
        /// <param name="nameOrCode">El nombre o código ISO del país</param>
        /// <returns>Lista de información básica de todos los países encontrados</returns>
        /// <response code="200">Devuelve una lista de información básica de los países encontrados</response>
        /// <response code="400">En caso de un nombre/ISO inválido</response>
        /// <response code="404">En caso de no encontrar ningún país o que el encontrado no tenga vecinos</response>
        /// <response code="500">En caso de surgir un error inesperado</response>
        /// <response code="503">En caso de que restcountries.com no esté disponible</response>
        [HttpGet]
        [Route("{nameOrCode?}/neigbors")]
        public async Task<ActionResult> GetAdjacentByName(string nameOrCode)
        {
            List<string> adjacentCodes = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(nameOrCode))
                {
                    return BadRequest("El nombre o código ISO no puede ser vacío.");
                }
                bool isISOName = nameOrCode.Length <= 3;

                string apiRouteEnd = isISOName ? "alpha/" : "name/";

                List<AdjacentCountryData> result;

                string finalURL = TargetAPIUrl + apiRouteEnd + nameOrCode;
                Task<HttpResponseMessage> apiResult = RequestCountryData(finalURL);
                apiResult.Wait();

                if (apiResult.Result == null || !apiResult.Result.IsSuccessStatusCode)
                {
                    if (apiResult.Result != null && apiResult.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return NotFound("No se ha encontrado ningún país con el nombre o código " + nameOrCode);
                    else
                        return StatusCode(503, "restcountries.com no está disponible.");
                }
                else
                {
                    string parsedContent = await apiResult.Result.Content.ReadAsStringAsync();
                    List<DtoCountryData> parsedDto = JsonSerializer.Deserialize<List<DtoCountryData>>(parsedContent);
                    DtoCountryData firstMatch = parsedDto.FirstOrDefault();
                    if (firstMatch == null)
                        return NotFound("No se ha encontrado ningún país con el nombre o código " + nameOrCode);

                    if (firstMatch.borders == null || !firstMatch.borders.Any())
                        return NotFound("El país con el nombre o código " + nameOrCode + " no tiene países vecinos.");

                    finalURL = TargetAPIUrl + "alpha?codes=" + string.Join(",", firstMatch.borders);
                    Task<HttpResponseMessage> apiResult2 = RequestCountryData(finalURL);
                    apiResult2.Wait();

                    if (apiResult.Result.IsSuccessStatusCode)
                    {
                        parsedContent = await apiResult2.Result.Content.ReadAsStringAsync();
                        List<DtoCountryData> parsedDto2 = JsonSerializer.Deserialize<List<DtoCountryData>>(parsedContent);
                        result = parsedDto2.Select(a => new AdjacentCountryData(a)).ToList();
                    }
                    else
                        return StatusCode(503, "restcountries.com no está disponible.");
                }

                List<string> resultStrings = new List<string>();
                for (int i = 0; i < result.Count; i++)
                {
                    resultStrings.Add((i + 1) + ". " + result[i].DataToText);
                }

                return Ok(string.Join(Environment.NewLine, resultStrings));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ha ocurrido un error inesperado.");
            }
        }

        private async Task<HttpResponseMessage> RequestCountryData(string url)
        {
            HttpClient client = new HttpClient();
            Uri path = new Uri(url);

            HttpResponseMessage response = await client.GetAsync(path);

            return response;
        }
    }
}
