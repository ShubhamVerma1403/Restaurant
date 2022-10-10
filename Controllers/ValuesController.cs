using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NLog;
using Restaurant.Models;
using Restaurant.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Restaurant.Controllers
{
    public class ValuesController : ApiController
    {

        public readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private FoodHubEntities foodHubEntities;

        public ValuesController()
        {
            this.foodHubEntities = new FoodHubEntities();
        }

        [HttpGet]
        public IHttpActionResult SignIn()
        {
            List<GetUserInfo_Result> userList = foodHubEntities.GetUserInfo().ToList();
            return Json(userList);
        }

        [Route("api/Values/SignUp")]
        [HttpPost]
        public IHttpActionResult SignUp(AddUser Userdata)
        {
            var user = foodHubEntities.GetUserInfo().FirstOrDefault(x => x.Email == Userdata.Email);
            if (user != null)
            {
                return BadRequest(user.Email + " already exist");
            }
            else
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));

                foodHubEntities.AddUser(Userdata.LastName, Userdata.FirstName, Userdata.Email, Userdata.Password, Userdata.PhoneNo_, responseMessage);
                foodHubEntities.SaveChanges();
                return Json("Account created successfully");
            }

        }

        public IHttpActionResult GetItem()
        {
            logger.Info("Entering the GetItem method.  ");
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            List<GetItems_Result> result = foodHubEntities.GetItems(responseMessage).ToList();
            logger.Info("Exit the GetItem method.    Item Fetched successfully");
            return Json(result);
        }

        public HttpResponseMessage AddItem(GetItems item)
        {
            logger.Info("Entering in the AddItem method");
            try
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                foodHubEntities.AddItems(item.ItemName, item.Price, item.CategoryId, item.Description, responseMessage);
                foodHubEntities.SaveChanges();
                logger.Info("Exit the Add method.    Item Added successfully");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                logger.Error(" Exception during Adding item ");
                logger.Error(e);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, e);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateItem(GetItems item)
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            var result = foodHubEntities.UpdateItem(item.Id, item.ItemName, item.Price, item.CategoryId, item.Description, responseMessage);
            return Json(result);
        }

        [HttpPost]
        public HttpResponseMessage DeleteItem(GetItems deleteitem)
        {
            logger.Info("Entering in the Delete method");
            try
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                foodHubEntities.DeleteItem(deleteitem.Id, responseMessage);
                foodHubEntities.SaveChanges();
                logger.Info("Exit the Delete method.    Item Deleted successfully");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.Error(" Deletion failed ");
                logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        public IHttpActionResult GetCategory()
        {
            logger.Info("Entering the GetCategory method.  ");
            List<GetCategory_Result> result = foodHubEntities.GetCategory().ToList();
            logger.Info("Exit the GetCategory method.     Category Fetched successfully");
            return Json(result);
        }

        [Route("api/Values/order")]
        [HttpPost]
        public IHttpActionResult Order(OrderItem orderItem)
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));

            foodHubEntities.OrderItem(Convert.ToInt32(orderItem.UserId), orderItem.ItemId,orderItem.Price,orderItem.Quantity, responseMessage);
            
            foodHubEntities.SaveChanges();
            return Ok();
        }

        [Route("api/Values/GetAuditDetail")]
        public DataSourceResult GetAuditDetail([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request)
        {
            logger.Info("Entering the GetAuditDetail method.  ");

            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            List<Auditing_Result> result = foodHubEntities.Auditing(responseMessage).ToList();

            logger.Info("Exit the GetAuditDetail method.     GetAuditDetail successfully");

            return result.ToDataSourceResult(request);
        }




















    }
}
