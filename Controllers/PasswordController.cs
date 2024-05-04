using CRMSMVCAPP.Models;
using CRMSMVCAPP.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CRMSMVCAPP.Controllers
{
    [Authentication]
    public class PasswordController : Controller
    {
        #region ChangePassword
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(PasswordRequest passwordRequest)
        {

            if (passwordRequest.NewPassword != passwordRequest.ConfirmPassword)
            {
                ViewBag.Messeage = "Sifreler Ayni Degil Tekrar Deneyiniz";
                return View(passwordRequest);
            }
            UserResponse userResponse = new UserResponse();
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(passwordRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("https://localhost:44389/ChancePassword", data);

                if (res.IsSuccessStatusCode)
                {
                    ViewBag.Messeage = "Sifre Basarili Bir Sekilde Degisti.";
                }
                else
                {
                    ViewBag.Messeage = string.Format("KULLANICI ADI Veya SIFRE HATALI!");
                    return View(passwordRequest);
                }

            }

            return View();
        }
        #endregion


    }
}
