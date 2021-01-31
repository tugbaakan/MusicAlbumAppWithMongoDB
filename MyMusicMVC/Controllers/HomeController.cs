using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Config;
        private string URLBase
        {
            get
            {
                return _Config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration Config)
        {
            _logger = logger;
            _Config = Config;
        }

        public async Task<IActionResult> Index()
        {
            var listMusic = new ListMusicViewModel();
            var  musicList = new List<Music>();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(URLBase + "Music"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    musicList = JsonConvert.DeserializeObject<List<Music>>(apiResponse);
                }
            }
            listMusic.ListMusic = musicList;
            return View(listMusic);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> AddMusic()
        {
            var musicViewModel = new MusicViewModel();
            var listArtrist = new List<Artist>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(URLBase + "Artist"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listArtrist = JsonConvert.DeserializeObject<List<Artist>>(apiResponse);
                }

            }
            musicViewModel.ArtistList = new SelectList(listArtrist, "Id", "Name");
            return View(musicViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddMusic(MusicViewModel musicModelView)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var music = new Music() { ArtistId = int.Parse(musicModelView.AristId), Name = musicModelView.Music.Name };

                    //Get token 
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "You must be authenticate";
                        return View(musicModelView);
                    }
                    string stringData = JsonConvert.SerializeObject(music);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PostAsync(URLBase + "Music", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(musicModelView);
                }
            }
            return View(musicModelView);

        }
    }
}
