using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.DataAccess.Base.Enum;
using Product.DataAccess.Enum;
using Product.DataAccess.Models;
using Product.DataAccess.Repositories;
using Product.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductsApp.Controllers
{
    public class SalesOrdersController : Controller
    {
        private readonly ISalesOrdersRepository _salesOrdersRepository;
        private readonly ISOLinesRepository _soLinesRepository;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;

        public SalesOrdersController(ISalesOrdersRepository salesOrdersRepository, ISOLinesRepository soLinesRepository,IHttpClientFactory httpClientFactory)
        {
            _salesOrdersRepository = salesOrdersRepository;
            _soLinesRepository = soLinesRepository;
            _client = CreateHttpClientWithNTLM();
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> IndexAjax([FromBody] SalesOrdersVM model)
        {
            var resultt = await _client.GetAsync($"{_client.BaseAddress}/SalesOrder?$filter=No eq '{model.SearchNo}'");
            var SalesOrderList = JsonConvert.DeserializeObject<Product.DataAccess.Models.SlaesOrderHeaderAndLines.Rootobject>(await resultt.Content.ReadAsStringAsync());
            if (string.IsNullOrEmpty(model.SearchNo))
            {
                var data = _salesOrdersRepository.GetAllAsyncPage(model.PageNo, model.PageSize,null);
                var SO = data.Item1.Select(v => new SlaesOrderHeaderAndLines.Value()
                {
                    No = v.No,
                    Sell_to_Customer_Name = v.CustomerName,
                    Posting_Description = v.Discription ,
                    SO_Status = v.SOStatus,
                    Order_Date = v.OrderDate.ToString(),
                    Shipment_Date = v.ShipmentDate.ToString(),
                    Phone_No  = v.PhoneNo,
                    CRM_Sales_Amount =Convert.ToInt32(v.Amount)
                }).ToList();
                return Json(new
                {
                    TotalItems = data.Item2,
                    Data = SO
                });
            }
           
            return Json(new
            {
                TotalItems = SalesOrderList.value.Count(),
                Data = SalesOrderList.value
            });
        }
        public IActionResult _Details(string No)
        {
           
         return PartialView(new SalesOrdersVM { No=No });
        }
       
        public async Task<JsonResult> DetailsAjax([FromBody] SalesOrdersVM model)
        {
            var resultt = await _client.GetAsync($"{_client.BaseAddress}/SalesOrder?$filter=No eq '{model.No}'&$expand=SalesOrderSalesLines");
            var SalesOrderList = JsonConvert.DeserializeObject<Product.DataAccess.Models.SlaesOrderHeaderAndLines.Rootobject>(await resultt.Content.ReadAsStringAsync());
            var count = SalesOrderList.value[0].SalesOrderSalesLines.Count();
            return Json(new
            {
                TotalItems = count,
                Data = SalesOrderList.value[0].SalesOrderSalesLines
            });
           /* var LineFromDB = _soLinesRepository.GetAllQuerable().Include(c => c.SalesOrderId).Select(v => new {
                PartNumber = v.PartNumber.ToString(),
                Description = v.Description,
                Price = v.Price,
                Quantity = v.Quantity,
            }).ToList();
            //  var modelVm = _mapper.Map<List<ProductVM>>(data0.Item1);

            return Json(new
            {
                TotalItems = LineFromDB.Count(),
                Data = LineFromDB
            });*/
        }
        [HttpPost]
        public async Task<IActionResult> Save(string No)
        {
            var resultt = await _client.GetAsync($"{_client.BaseAddress}/SalesOrder?$filter=No eq '{No}'&$expand=SalesOrderSalesLines");
            var SalesOrderList = JsonConvert.DeserializeObject<SlaesOrderHeaderAndLines.Rootobject>(await resultt.Content.ReadAsStringAsync());
            var data = SalesOrderList.value[0];
            // var SOLine = NotofocationList.value[0].SalesOrderSalesLines;
            var SOLine = SalesOrderList.value[0].SalesOrderSalesLines.ToList();

            var isExist = (await _salesOrdersRepository.GetAsync(c => c.No == data.No ).ConfigureAwait(false)).Any();
            if (isExist)
            {
                // return View(category);
                return Json(new
                {
                    status = JsonStatus.Exist,
                    link = "",
                    color = NotificationColor.Error.ToColorName(),
                    management = "SO Management",
                    msg = "SO is exist."
                });
            }
            else
            {
                var result = await _salesOrdersRepository.AddAsync(new SalesOrders()
                {
                    No = data.No,
                    CustomerName = data.Sell_to_Customer_Name,
                    Discription = data.Posting_Description,
                    SOStatus = data.SO_Status,
                    OrderDate = Convert.ToDateTime(data.Order_Date),
                    ShipmentDate = Convert.ToDateTime(data.Shipment_Date),
                    PhoneNo = data.Phone_No,
                    Amount = data.CRM_Sales_Amount

                });

                if (result > 0)
                {
                    foreach (var line in SOLine)
                    {
                        await _soLinesRepository.AddAsync(new SOLines()
                        {
                            PartNumber = line.Line_No.ToString(),
                            Description = line.Description,
                            Price = line.Unit_Price,
                            Quantity = line.Quantity,
                            SalesOrders = (await _salesOrdersRepository.GetAsync(x => x.No == data.No).ConfigureAwait(false)).FirstOrDefault()
                        });
                    }
                    return Json(new
                    {
                        status = JsonStatus.Success,
                        link = "",
                        color = NotificationColor.Success.ToColorName(),
                        management = "SO Management",
                        msg = "SO was updated successfully into the database.",
                    });
                }
                else
                    return Json(new
                    {
                        status = JsonStatus.Error,
                        link = "",
                        color = NotificationColor.Error.ToColorName(),
                        management = "SO Management",
                        msg = "Error.",
                    });
            }
        }
        public HttpClient CreateHttpClientWithNTLM()
        {
            var url = "http://ns-hou-navdev01.netsync.com:9837/DEVNETSYNC2018/ODataV4/Company(";
         
            var uri = new Uri($"{url}'NetSync%20Network%20-%20Live')");
            var credentialsCache = new CredentialCache { { uri, "NTLM", new NetworkCredential("sales", "Netsync01") } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var httpClient = new HttpClient(handler) { BaseAddress = uri };
            httpClient.DefaultRequestHeaders.ConnectionClose = false;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ServicePointManager.FindServicePoint(uri).ConnectionLeaseTimeout = 120 * 1000;  // Close connection after two minutes

            return httpClient;
        }
    }
}

