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
    public class OrderDetailRepository : Repository<OrderDetails>, IOrderDetailRepository
    //base class: Repository<Category> must be before any interface: ICategoryRepository
    //Such way we implemnt only the 2 custom functions (Save / update)
    //Quick Fix: Click to implement the interface and create ApplicationDbContext
    {

        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderDetails obj)
        {
          
            //var objFromDb = _db.OrderDetails.FirstOrDefault(u => u.Id == obj.Id);
            //if (objFromDb != null)
            ///{
                //objFromDb.Name = category.Name;
                //objFromDb.DisplayOrder  = category.DisplayOrder;
            //}


            //We will adopt a way that will update the entire object in one line
            _db.OrderDetails.Update(obj);
        }
    }
}
