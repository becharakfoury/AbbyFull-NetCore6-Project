using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AbbyWeb.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitofwork;
        public IEnumerable<MenuItem> _MenuItemList { get; set; }
		public IEnumerable<Category> _CategoryList { get; set; }

		public IndexModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public void OnGet()
        {
            _MenuItemList = _unitofwork.MenuItem.GetAll(includeProperties: "Category,FoodType");
			//includeProperties: "Category,FoodType"  ==> Case sensitive
			_CategoryList = _unitofwork.Category.GetAll(orderby: u => u.OrderBy(c => c.DisplayOrder));
		}
    }
}
