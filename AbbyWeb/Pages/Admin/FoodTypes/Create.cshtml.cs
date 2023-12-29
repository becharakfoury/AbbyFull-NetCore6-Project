using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AbbyWeb.Pages.Admin.FoodTypes
{

    [BindProperties] // will be used to bind all properties / on class level
    public class CreateModel : PageModel
    {

        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitofwork;

        public FoodType _FoodType { get; set; }


        //public CreateModel(ApplicationDbContext db)
        public CreateModel(IUnitOfWork unitofwork)
        {
            // _db = db;
            _unitofwork = unitofwork;
        }

        public void OnGet()
        {
        }

        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost() 
        {

      
            if(ModelState.IsValid)
            {
                //await _db.FoodType.AddAsync(_FoodType);
                //await _db.SaveChangesAsync();
                _unitofwork.FoodType.Add(_FoodType);
                _unitofwork.Save();


                TempData["success"] = "Food Type created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }            

            


    }
}
