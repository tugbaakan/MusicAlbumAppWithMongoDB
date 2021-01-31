using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;
using System.IdentityModel.Tokens;

namespace MyMusicMVC.Controllers
{
    public class AccountController : Controller
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
        public AccountController(ILogger<HomeController> logger, IConfiguration Config)
        {
            _logger = logger;
            _Config = Config;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(URLBase + "User/authenticate", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        string stringJWT = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var jwtString = jwt["token"].ToString();
                        HttpContext.Session.SetString("token", jwtString);
                        HttpContext.Session.SetString("username", jwt["username"].ToString());
                        ViewBag.Message = "User logged in successfully!  " + jwt["username"].ToString();
                    }
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(register);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(URLBase + "User/register", contentData);
                    var result = response.IsSuccessStatusCode;
                    if(result)
                    {
                        string stringJWT = response.Content.ReadAsStringAsync().Result;

                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var jwtString = jwt["token"].ToString();
                        HttpContext.Session.SetString("token", jwtString);//username

                        HttpContext.Session.SetString("username", jwt["username"].ToString());
                        return RedirectToAction("Index", "Home");
                    }
                }
                }
                return View(register);
        }
    }
}