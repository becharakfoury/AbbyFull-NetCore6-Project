using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    //must be defined using a Generic type: T
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; //defiend Dbset to access Dbset declared at: ApplicationDbContext

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            
            this.dbSet = db.Set<T>(); 
            //BIND all dbset declared in ApplicationDbContext into the local variable
			//in such method we can use DBset same as we used it in entity framework to do: Add, Edit, Delete operaiotn 
			// into the database
			//_db.MenuItem.OrderBy(u => u.Name);
		}

		//Applying the interface functions
		public void Add(T entity)
        {
            dbSet.Add(entity);            
        }

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
		   Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null, string? includeProperties = null)
		{
            IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
            {
                //Case we are using DbContext:  FoodType, Cagtegory
                //_db.MenuItem.Include(u => u.FoodType).Include(u => u.Category);
                //abc,,xyz -> abc xyz
                foreach (var includeProperty in includeProperties.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
			if (orderby != null)
			{
				return orderby(query).ToList();
			}

			return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
			if (includeProperties != null)
			{
				//Case we are using DbContext:  FoodType, Cagtegory
				//_db.MenuItem.Include(u => u.FoodType).Include(u => u.Category);
				//abc,,xyz -> abc xyz
				foreach (var includeProperty in includeProperties.Split(
					new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProperty);
				}
			}

			return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
