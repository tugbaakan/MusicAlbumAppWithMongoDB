using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class ComposerController : Controller
    {
        private readonly ILogger<ComposerController> _logger;
        private readonly IConfiguration _Config;
        private string URLBase
        {
            get
            {
                return _Config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public ComposerController(ILogger<ComposerController> logger, IConfiguration Config)
        {
            _logger = logger;
            _Config = Config;
        }
        public async Task<IActionResult> Index()
        {
            var listComposerViewModel = new ListComposerViewModel();
            var listComposer = new List<Composer>();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(URLBase + "Composer"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listComposer = JsonConvert.DeserializeObject<List<Composer>>(apiResponse);
                }
                listComposerViewModel.Composers = listComposer;
                return View(listComposerViewModel);
            }
        }

        public IActionResult AddComposer()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AddComposer(ComposerViewModel composerViewModel)
        {
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //Get token 
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "You must be authenticate";
                        return View(composerViewModel);
                    }
                    string stringData = JsonConvert.SerializeObject(composerViewModel);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PostAsync(URLBase + "Composer", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(composerViewModel);
                }
            }
            return View(composerViewModel);


        }
    }
    
}