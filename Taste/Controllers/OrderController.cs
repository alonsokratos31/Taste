using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get(string status = null )
        {
            List<OrderDetailsViewModel> orderListVM = new List<OrderDetailsViewModel>();

            IEnumerable<OrderHeader> orderHeadersList;

            if (User.IsInRole(SD.CustomerRole))
            {
                var claimsdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(u => u.UserId == claim.Value, null, "ApplicationUser");
            }
            else            
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(null, null, "ApplicationUser");
            

            if (status == "cancelled")
                orderHeadersList = orderHeadersList.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded || o.Status == SD.PaymentStatusRejected);
            
            else
            {
                if (status == "completed")
                    orderHeadersList = orderHeadersList.Where(o => o.Status == SD.StatusCompleted);
                
                else
                    orderHeadersList = orderHeadersList.Where(o => o.Status == SD.StatusReady || o.Status == SD.StatusInProcess || o.Status == SD.StatusSubmitted || o.Status == SD.PaymentStatusPending);
                
            }    

            foreach (OrderHeader item in orderHeadersList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(m => m.OrderId == item.Id).ToList()
                };

            orderListVM.Add(individual);
            }

            return Json(new { data = orderListVM });
        }
    }
}
