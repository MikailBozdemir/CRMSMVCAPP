using ClosedXML.Excel;
using CRMSMVCAPP.Models;
using CRMSMVCAPP.Utilities;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CRMSMVCAPP.Controllers
{
    [Authentication]
    public class InvoiceController : Controller
    {

        #region GetInvoiceDetailsByUserId

       
        [HttpGet]
        public  async Task <IActionResult> Index()
        {
            List<List<Invoice>> invoice2 = new List<List<Invoice>>();
            List<Invoice> invoice = new List<Invoice>();
            List<Invoice> invoice1 = new List<Invoice>();

            using (var client = new HttpClient())
            {


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
                UserResponse user = new UserResponse();

                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);



                var result = await client.GetAsync(" https://localhost:44389/GetInvoiceById/" + user.USERID);


                if (result.IsSuccessStatusCode)
                {
                    var invo = result.Content.ReadAsStringAsync().Result;

                    invoice = JsonConvert.DeserializeObject<List<Invoice>>(invo);

                    if (invoice!=null)
                    {

                 

                    for (int i = 0; i < invoice.Count; i++)
                    {
                        if (i == 0)
                        {
                            invoice1.Add(invoice[0]);
                        }
                        else if (i != 0)
                        {
                            if (invoice[i].InvoiceId == invoice[i - 1].InvoiceId)
                            {
                                invoice1.Add(invoice[i]);
                            }
                            else if (invoice[i].InvoiceId != invoice[i - 1].InvoiceId)
                            {
                                if (invoice1.Count != 0)
                                {
                                    invoice2.Add(invoice1);
                                }
                                invoice1 = new List<Invoice>();
                                invoice1.Add(invoice[i]);

                            }
                        }
                    }
                    invoice2.Add(invoice1);
                    }
                    else if (invoice == null)
                    {
                        return RedirectToAction("EmptyInvoice");
                    }

                }


            }
            return View(invoice2);

        }

        #endregion


        #region Getİnvoiced
        [HttpPost]
        public async Task<ActionResult> GetInvoice()
        {
            using (var client = new HttpClient())
            {

                UserResponse user = new UserResponse();
                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);

                var json = JsonConvert.SerializeObject(user.USERID);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync("https://localhost:44389/GetInvoice", data);

                if (res.IsSuccessStatusCode)
                {
                    var cart1 = res.Content.ReadAsStringAsync().Result;

                }

                ViewBag.Messeage = "Satın Alma Islemi Basarili Oldu Faturanizi Satin Alma Gecmisinden Indirebilirsiniz.";
            }
            return RedirectToAction("Index", "Cart");
        }
        #endregion


        #region ExportToExcel
        public async Task<IActionResult> ExportInvoiceToExcel()
        {

            List<List<Invoice>> invoice2 = new List<List<Invoice>>();
            List<Invoice> invoice = new List<Invoice>();
            List<Invoice> invoice1 = new List<Invoice>();
            using (var client = new HttpClient())
            {


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
                UserResponse user = new UserResponse();

                var _user = HttpContext.Session.GetString("user");
                user = JsonConvert.DeserializeObject<UserResponse>(_user);



                var result = await client.GetAsync(" https://localhost:44389/GetInvoiceById/" + user.USERID);


                if (result.IsSuccessStatusCode)
                {
                    var invo = result.Content.ReadAsStringAsync().Result;

                    invoice = JsonConvert.DeserializeObject<List<Invoice>>(invo);

                    for (int i = 0; i < invoice.Count; i++)
                    {
                        if (i == 0)
                        {
                            invoice1.Add(invoice[0]);
                        }
                        else if (i != 0)
                        {
                            if (invoice[i].InvoiceId == invoice[i - 1].InvoiceId)
                            {
                                invoice1.Add(invoice[i]);
                            }
                            else if (invoice[i].InvoiceId != invoice[i - 1].InvoiceId)
                            {
                                if (invoice1.Count != 0)
                                {
                                    invoice2.Add(invoice1);
                                }
                                invoice1 = new List<Invoice>();
                                invoice1.Add(invoice[i]);

                            }
                        }
                    }
                    invoice2.Add(invoice1);


                }


            }

            using (var workbook = new XLWorkbook())
            {
                for (int i = 0; i < invoice2.Count; i++)
                {


                    var worksheet = workbook.Worksheets.Add(i + 1 + ". Fatura Bilgi");
                    worksheet.Cell(1, 1).Value = "İsim";
                    worksheet.Cell(3, 1).Value = "Adres";
                    worksheet.Cell(1, 5).Value = "Tarih";



                    worksheet.Cell(4, 1).Value = "#";
                    worksheet.Cell(4, 2).Value = "FaturaNo";
                    worksheet.Cell(4, 3).Value = "ÜrünAdı";
                    worksheet.Cell(4, 4).Value = "Adet";
                    worksheet.Cell(4, 5).Value = "Fiyat";


                    int rowCount = 5;

                    for (int j = 0; j < invoice2[i].Count; j++)
                    {
                        worksheet.Cell(rowCount, 1).Value = j + 1;
                        worksheet.Cell(rowCount, 2).Value = invoice2[i][j].InvoiceId;
                        worksheet.Cell(rowCount, 3).Value = invoice2[i][j].ProductName;
                        worksheet.Cell(rowCount, 4).Value = invoice2[i][j].Amount;
                        worksheet.Cell(rowCount, 5).Value = invoice2[i][j].TotalPriceByProduct;
                        rowCount++;

                    }
                    worksheet.Cell(rowCount, 1).Value = "Toplam";
                    worksheet.Cell(rowCount, 5).Value = invoice2[i][0].TotalPrice + " " + "TL";
                    worksheet.Cell(1, 2).Value = invoice2[i][0].UserName;
                    worksheet.Cell(3, 2).Value = invoice2[i][0].Adress;
                    worksheet.Cell(1, 6).Value = invoice2[i][0].InvoiceDate;

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SatınAlmaGecmisi.xlsx");
                }
            }
        }
        #endregion

        public IActionResult EmptyInvoice()
        {
            return View();
                
        }

    }
}
