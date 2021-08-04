using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFoodTypeListForDropDown()
        {
            return _db.MenuItem.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
        }

        public void Update(MenuItem menuItem)
        {
            var menuItemFormDb = _db.MenuItem.FirstOrDefault(s => s.Id == menuItem.Id);
            menuItemFormDb.Name = menuItem.Name;
            menuItemFormDb.CategoryId = menuItem.CategoryId;
            menuItemFormDb.Description = menuItem.Description;
            menuItemFormDb.FoodTypeId = menuItem.FoodTypeId;
            menuItemFormDb.Price = menuItem.Price;
            if(menuItem.Image != null) menuItemFormDb.Image = menuItem.Image;

            _db.SaveChanges();
        }
    }
}
