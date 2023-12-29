using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes
{
    public class IndexModel : PageModel
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitofwork;
        public IEnumerable<FoodType> _FoodTypes { get; set; }

        //public IndexModel(ApplicationDbContext db)
        public IndexModel(IUnitOfWork unitofwork)
        {
            // _db = db;
            _unitofwork = unitofwork;
        }


        public void OnGet()
        {

            //_FoodTypes = _db.FoodType;
            _FoodTypes = _unitofwork.FoodType.GetAll();

        }
    }
}
