using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItem.GetAll(null,null,"Category,FoodType") });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            try
            {
                var objFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (objFromDb == null)
                    return Json(new { success = false, message = "Error while deleting" });

                
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

                _unitOfWork.MenuItem.Remove(objFromDb);
                _unitOfWork.Save();
              
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error while deleting {ex.Message}" });
            }

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
