using CRMSMVCAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using X.PagedList;

namespace CRMSMVCAPP.Controllers
{
    [AllowAnonymous]
    public class MainPageController : Controller
    {

        #region MainPage
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1)
        {
            List<Product> products = new List<Product>();
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage result = await client.GetAsync("https://localhost:44389/api/Product/GetAllProduct");
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<Product>>(res);
                }
            }
            return View(products.ToPagedList(page, 9));
        }
        #endregion

        #region GetProductsByCategory
        [HttpGet]
        public async Task<ActionResult> GetProductsByCategory(int id, int page = 1)
        {
            List<Product> product = new List<Product>();
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage result = await client.GetAsync(" https://localhost:44389/api/Product/GetAllProduct/" + id);
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<List<Product>>(res);
                }
            }


            return View(product.ToPagedList(page, 9));
        }

        #endregion

        #region FilterProducts
        [HttpGet]
        public async Task<ActionResult> FilterProducts(int id, string filterType /*,int page = 1*/)
        {
            List<Product> product = new List<Product>();
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage result = await client.GetAsync("https://localhost:44389/api/Product/FilterProduct/" + filterType + "/" + id);
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<List<Product>>(res);
                }
            }


            return View(product/*.ToPagedList(page, 9)*/);
        }


        #endregion

        #region GetProductDetailsById
        [HttpGet]
        public async Task<ActionResult> GetProductById(int Id)
        {
            Product product = new Product();
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage result = await client.GetAsync("https://localhost:44389/api/Product/GetProductById/" + Id);
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(res);
                }
            }
            return View(product);
        }

        #endregion

    }
}
