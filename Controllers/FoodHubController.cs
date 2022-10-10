using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NLog;
using Restaurant.Models;
using Restaurant.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    
    public class FoodHubController : Controller
    {
        string baseUrl = "https://localhost:44348/";
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        HttpClient httpClient = new HttpClient();
        private FoodHubEntities foodHubEntities;
        private object varhashedBytes;
        public List<GetCategory_Result> Categorylist;

        public FoodHubController()
        {
            this.foodHubEntities = new FoodHubEntities();
            this.Categorylist = new List<GetCategory_Result>();

        }
        
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        //MainPage
        public ActionResult Main()
        {
            if (Session["Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "FoodHub");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(AddUser Userdata)
        {
            Logger.Info("Entering the FoodHub Controller.    SignIn method");

            string result = "fail";
           
            List<GetUserInfo_Result> userList = new List<GetUserInfo_Result>();

            httpClient.BaseAddress = new Uri(baseUrl);
            HttpResponseMessage message = await httpClient.GetAsync("api/values/SignIn/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<GetUserInfo_Result>>();
                userList = display.Result;

                using (var sha512 = SHA512.Create())
                {
                    varhashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(Userdata.Password));
                    var _currentUser = userList.FirstOrDefault(user => user.Email == Userdata.Email &&  user.Password.ToString() == varhashedBytes.ToString());
                    
                    if (_currentUser != null)
                    {
                        HttpContext.Session["Id"] = Convert.ToInt32(_currentUser.Id.ToString());
                        HttpContext.Session["UserName"] = _currentUser.FirstName.ToString();
                        HttpContext.Session["IsAdmin"] = _currentUser.IsAdmin.ToString();

                        Logger.Info("Exit Sign In method.     SignIn success");

                        result = "success";

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            Logger.Info("Exit Sign In method.     SignIn fail");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "FoodHub");
        }

        public async Task<ActionResult> GetItem([DataSourceRequest] DataSourceRequest request)
        {
            List<GetItems_Result> list = new List<GetItems_Result>();

            httpClient.BaseAddress = new Uri(baseUrl);

            HttpResponseMessage message = await httpClient.GetAsync("api/values/getItem/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<GetItems_Result>>();
                list = display.Result;
            }
            return Json(list.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> AddItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            var categoryName = foodHubEntities.Categories.FirstOrDefault(p => p.Id == item.CategoryId);
            item.CategoryName = categoryName.CategoryName;

            if (item != null && ModelState.IsValid)
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                HttpResponseMessage message = await httpClient.PostAsJsonAsync("api/values/AddItem/", item);
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            if (item != null && ModelState.IsValid)
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                HttpResponseMessage message = await httpClient.PostAsJsonAsync("api/values/UpdateItem/", item);
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> DeleteItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            if (item != null)
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                HttpResponseMessage message = await httpClient.PostAsJsonAsync("api/values/DeleteItem/", item);
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> GetCategory()
        {
            List<GetCategory_Result> Categorylist = new List<GetCategory_Result>();

            httpClient.BaseAddress = new Uri(baseUrl);

            HttpResponseMessage message = await httpClient.GetAsync("api/values/GetCategory/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<GetCategory_Result>>();
                Categorylist = display.Result;
            }
            return Json(Categorylist, JsonRequestBehavior.AllowGet);
        }

        
    }
}