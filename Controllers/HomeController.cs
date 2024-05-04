using CRMSMVCAPP.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;


namespace CRMSMVCAPP.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region LoginPage
        [HttpGet]
        public IActionResult Index()
        {
            UserRequest userRequest = new UserRequest();
            return View(userRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserRequest userRequest)
        {


            UserResponse userResponse = new UserResponse();
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(userRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("https://localhost:44389/api/User/GetUserLogin", data);

                if (res.IsSuccessStatusCode)
                {
                    var user1 = res.Content.ReadAsStringAsync().Result;
                    userResponse = JsonConvert.DeserializeObject<UserResponse>(user1);
                }

            }
            if (userResponse.USERNAME != null && userResponse.PASSWORD != null)
            {


                var user = JsonConvert.SerializeObject(userResponse);
                HttpContext.Session.SetString("user", user);
                return RedirectToAction("Index", "MainPage");
            }
            else
            {
                ViewBag.Messeage = string.Format("KULLANICI ADI Veya SIFRE HATALI!");
                return View();
            }

        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}