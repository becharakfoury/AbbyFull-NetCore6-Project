using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    //base class: Repository<Category> must be before any interface: ICategoryRepository
    //Such way we implemnt only the 2 custom functions (Save / update)
    //Quick Fix: Click to implement the interface and create ApplicationDbContext
    {

        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category category)
        {
            //Method #1: this method updated the entire entity (all fields),
            //_db.Category.Update(category);

            //Method #2: case we need to update just specific Field
            //Retrieve the category record from Database
            //EF once we retrive an entity it will track its changes
            var objFromDb = _db.Category.FirstOrDefault(u => u.Id == category.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                objFromDb.DisplayOrder  = category.DisplayOrder;
            }
        }
    }
}
