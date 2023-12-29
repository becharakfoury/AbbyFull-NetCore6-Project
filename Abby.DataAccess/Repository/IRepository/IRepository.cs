using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        //GET ALL, GET By ID FIRST OR DEFAULT, ADD, REMOVE, REMOVERANGE
        void Add(T entity); // will return nothing
        void Remove(T entity); // will return nothing
        void RemoveRange(IEnumerable<T> entity); // will return nothing

		// similar to define a select + where to query a database, return type will be: IEnumerable<T> or list of the items
		// For the momeent we will keep it simple and get all records from database, we will set a Filter in the future
		// ? ==> Optional
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
			  Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
			  string? includeProperties = null);

		//Function to get first or default by ID, return only One Record. So return type will be one class: T
		//Expression can be ommited, check the below syntax used before
		//_Category = _db.Category.SingleOrDefault(u => u.Id == Id);  
		// ? ==> Optional
		T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);


    }
}
