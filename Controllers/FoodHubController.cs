using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
        HttpClient hc = new HttpClient();
        private FoodHubEntities foodHubEntities;
        private object varhashedBytes;
        public static int UserId;
        public List<GetCategory_Result> Categorylist;
        public FoodHubController()
        {
            this.foodHubEntities = new FoodHubEntities();
            this.Categorylist = new List<GetCategory_Result>();

        }
        public ActionResult Index()
        {
            if (Session["Idss"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "FoodHub");
            }
            
        }
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

        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignIn(AddUser Userdata)
        {
            string result = "fail";
            using (var sha512 = SHA512.Create())
            {
                varhashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(Userdata.Password));
                 var _currentUser = foodHubEntities.Users.FirstOrDefault(user => user.Email == Userdata.Email && user.Password == varhashedBytes);
                if (_currentUser != null)
                {
                    HttpContext.Session["Id"]= Convert.ToInt32(_currentUser.Id.ToString());
                    HttpContext.Session["UserName"] = _currentUser.FirstName.ToString();
                    HttpContext.Session["IsAdmin"] = _currentUser.IsAdmin.ToString();
                    //Session["Id"] = Convert.ToInt32(_currentUser.Id.ToString());
                    //Session["UserName"] = _currentUser.FirstName.ToString();
                    //Session["IsAdmin"] = _currentUser.IsAdmin.ToString();
                    result = "success";
                }
               
            }
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


            hc.BaseAddress = new Uri("https://localhost:44348/");

            HttpResponseMessage message = await hc.GetAsync("api/values/getItem/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<GetItems_Result>>();
                list = display.Result;

            }
            return Json(list.ToDataSourceResult(request));
            
        }
        public async Task<ActionResult> GetAuditDetail([DataSourceRequest] DataSourceRequest request)
        {
            List<Auditing_Result> Auditlist = new List<Auditing_Result>();


            hc.BaseAddress = new Uri("https://localhost:44348/");

            HttpResponseMessage message = await hc.GetAsync("api/values/GetAuditDetail/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<Auditing_Result>>();
                Auditlist = display.Result;

            }
            return Json(Auditlist.ToDataSourceResult(request));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> AddItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            var categoryName= foodHubEntities.Categories.FirstOrDefault(p=>p.Id==item.CategoryId);
            item.CategoryName = categoryName.CategoryName;
            if (item != null && ModelState.IsValid)
            {
                hc.BaseAddress = new Uri("https://localhost:44348/");
                HttpResponseMessage message = await hc.PostAsJsonAsync("api/values/AddItem/", item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            if (item != null && ModelState.IsValid)
            {
                hc.BaseAddress = new Uri("https://localhost:44348/");
                HttpResponseMessage message = await hc.PostAsJsonAsync("api/values/UpdateItem/", item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> DeleteItem([DataSourceRequest] DataSourceRequest request, GetItems item)
        {
            if (item != null)
            {
                hc.BaseAddress = new Uri("https://localhost:44348/");
                HttpResponseMessage message = await hc.PostAsJsonAsync("api/values/DeleteItem/", item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> GetCategory()
        {
            List<GetCategory_Result> Categorylist = new List<GetCategory_Result>();


            hc.BaseAddress = new Uri("https://localhost:44348/");

            HttpResponseMessage message = await hc.GetAsync("api/values/GetCategory/");

            if (message.IsSuccessStatusCode)
            {
                var display = message.Content.ReadAsAsync<List<GetCategory_Result>>();
                Categorylist = display.Result;
            }
            return Json(Categorylist, JsonRequestBehavior.AllowGet);
        }
    }
}