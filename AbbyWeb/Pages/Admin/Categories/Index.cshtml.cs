using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitofwork;

        public IEnumerable<Category> Categories { get; set; }

        public IndexModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public void OnGet()
        {
            //working using applicationdbcontext directly
            //_Categories = _db.Category;

            //working using regository
            //Categories = _dbCategory.GetAll();
            Categories = _unitofwork.Category.GetAll();

        }
    }
}
