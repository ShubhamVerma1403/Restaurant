using Restaurant.Models;
using Restaurant.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class ValuesController : ApiController
    {
        private FoodHubEntities foodHubEntities;
        public ValuesController()
        {
            this.foodHubEntities = new FoodHubEntities();
        }

        [Route("api/Values/order")]
        [HttpPost]
        public IHttpActionResult Order(Order orderItem)
        {
            
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));

                foodHubEntities.OrderItem(Convert.ToInt32(orderItem.UserId), orderItem.ItemId,orderItem.Price,orderItem.Quantity, responseMessage);

                foodHubEntities.SaveChanges();
                return Ok();

            

        }
        [Route("api/Values/SignUp")]
        [HttpPost]
        public IHttpActionResult SignUp(AddUser Userdata)
        {
            var user = foodHubEntities.GetUserInfo().FirstOrDefault(x => x.Email == Userdata.Email);
            if (user!=null)
            {
                return BadRequest(user.Email + " already exist");
            }
            else
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));

                foodHubEntities.AddUser(Userdata.LastName, Userdata.FirstName, Userdata.Email, Userdata.Password, Userdata.PhoneNo_, responseMessage);

                foodHubEntities.SaveChanges();
                return Ok("Account created successfully");

            }

        }
        public IHttpActionResult GetItem()
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            List<GetItems_Result> result = foodHubEntities.GetItems(responseMessage).ToList();
            return Json(result);
        }
        public IHttpActionResult AddItem(GetItems item)
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            var result = foodHubEntities.AddItems(item.ItemName, item.Price, item.CategoryId, item.Description, responseMessage);
            return Json(result);
        }

        [HttpPost]
        public IHttpActionResult UpdateItem(GetItems item)
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            var result = foodHubEntities.UpdateItem(item.Id,item.ItemName, item.Price, item.CategoryId, item.Description, responseMessage);
            return Json(result);
        }

        [HttpPost]
        public IHttpActionResult DeleteItem(GetItems deleteitem)
        {
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            var result = foodHubEntities.DeleteItem(deleteitem.Id, responseMessage);
            return Json(result);
        }
        public IHttpActionResult GetCategory()
        {
            List<GetCategory_Result> result = foodHubEntities.GetCategory().ToList();
            return Json(result);
        }

        public IHttpActionResult GetAuditDetail()
        {
            List<Auditing_Result> result = foodHubEntities.Auditing().ToList();
            return Json(result);
        }
    }
}
