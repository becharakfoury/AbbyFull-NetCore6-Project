using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {


        private readonly ApplicationDbContext _db;
        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItem Obj)
        {
            var objFromDb = _db.MenuItem.FirstOrDefault(u => u.Id == Obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = Obj.Name;
                objFromDb.Description = Obj.Description;
                objFromDb.Price = Obj.Price; 

                objFromDb.FoodTypeId = Obj.FoodTypeId;
                objFromDb.CategoryId = Obj.CategoryId;

                if (Obj.Image !=null) {
                    objFromDb.Image = Obj.Image;
                }                

            }
        }
    }
}
