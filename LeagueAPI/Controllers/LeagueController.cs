using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LeagueAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeagueAPI.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : Controller
    {
        string host = "api.riotgames.com";
        string api_key = "RGAPI-f7c19f80-59fe-48f9-8266-24addc02f001";
        //// GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public LeagueController() {
            
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "league1", "league2" };
        }
        [HttpGet("summoner")]
        public async Task<IActionResult> Get(string server, string name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://{server}.{host}");
                    var response = await client.GetAsync($"/lol/summoner/v3/summoners/by-name/{name}?api_key={api_key}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<ISummoner>(stringResult);
                    return Json(new
                    {
                        profileIconId = rawData.profileIconId,
                        name = rawData.name,
                        summonerLevel = rawData.summonerLevel,
                        accountId = rawData.accountId,
                        id = rawData.id,
                        revisionDate = rawData.revisionDate
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting summoner data from RiotGamesApi: {httpRequestException.Message}");
                }
            }
        }

        [HttpGet("matches")]
        public async Task<IActionResult> Get(string server, int accountId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://{server}.{host}");
                    var response = await client.GetAsync($"/lol/match/v3/matchlists/by-account/{accountId}?api_key={api_key}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<IMatch>(stringResult);
                    return Json(new
                    {
                        matches = rawData.matches
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting summoner data from RiotGamesApi: {httpRequestException.Message}");
                }
            }
        }

        [HttpGet("image")]
        public async Task<IActionResult> Get(int id, string server)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://{server}.{host}");
                    var response = await client.GetAsync($"/lol/static-data/v3/champions/{id}?locale=en_US&api_key={api_key}");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<IChampion>(stringResult);
                    return Json(new
                    {
                        title = rawData.title,
                        name = rawData.name,
                        key =  rawData.key,
                        id = rawData.id
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting summoner data from RiotGamesApi: {httpRequestException.Message}");
                }
            }
        }
        [HttpGet("champs")]
        public async Task<IActionResult> GetChamps(string server)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://{server}.{host}");
                    var response = await client.GetAsync($"/lol/static-data/v3/champions/?locale=en_US&dataById=false&api_key={api_key}");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<ChampsList>(stringResult);
                    return Json(new
                    {
                        champs = rawData.data
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting summoner data from RiotGamesApi: {httpRequestException.Message}");
                }
            }
        }
    }
}
