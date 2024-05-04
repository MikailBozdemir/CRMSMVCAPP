using CRMSMVCAPP.Models;
using CRMSMVCAPP.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace CRMSMVCAPP.Controllers
{
    [Authentication]
    public class CartController : Controller
    {

        #region GetCartDetails
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Cart> cart = new List<Cart>();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
                UserResponse user = new UserResponse();

                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);



                var result = await client.GetAsync(" https://localhost:44389/api/Cart/GetCartById/" + user.USERID);

                if (result.IsSuccessStatusCode)
                {
                    var cart1 = result.Content.ReadAsStringAsync().Result;

                    cart = JsonConvert.DeserializeObject<List<Cart>>(cart1);

                }
                if (cart == null)
                {
                    return RedirectToAction("EmptyCart");
                }

            }
            return View(cart);
        }
        #endregion

        #region AddToCart
        [HttpPost]
        public async Task<ActionResult> AddToCart(int Id)
        {
            using (var client = new HttpClient())
            {


                UserResponse user = new UserResponse();

                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);

                CartRequest cartRequest = new CartRequest() { ProductId = Id, CutomerId = user.USERID, Amount = 1 };
                var json = JsonConvert.SerializeObject(cartRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("https://localhost:44389/api/Cart/AddToCart", data);

                if (res.IsSuccessStatusCode)
                {
                    var cart1 = res.Content.ReadAsStringAsync().Result;

                }


            }
            return RedirectToAction("Index", "Cart");
        }
        #endregion

        #region RemoveFromCart
        [HttpGet]
        public async Task<ActionResult> RemoveFromCart(int Id)
        {
            using (var client = new HttpClient())
            {


                UserResponse user = new UserResponse();

                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);

                CartRequest cartRequest = new CartRequest() { ProductId = Id, CutomerId = user.USERID, Amount = 1 };
                var json = JsonConvert.SerializeObject(cartRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("https://localhost:44389/api/Cart/RemoveFromCart", data);






                if (res.IsSuccessStatusCode)
                {
                    var cart1 = res.Content.ReadAsStringAsync().Result;



                }


            }
            return RedirectToAction("Index", "Cart");
        }
        #endregion

        public ActionResult EmptyCart()
        {
            return View();
        }


    }
}
