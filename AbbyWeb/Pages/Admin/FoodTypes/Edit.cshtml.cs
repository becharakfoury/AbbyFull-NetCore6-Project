using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes
{

    [BindProperties] // will be used to bind all properties / on class level
    public class EditModel : PageModel
    {

        //private readonly ApplicationDbContext _db;

        private readonly IUnitOfWork _unitofwork;

        public FoodType _FoodType { get; set; }


        //public EditModel(ApplicationDbContext db)
        public EditModel(IUnitOfWork unitofwork)
        {
            // _db = db;
            _unitofwork = unitofwork;
        }

        public void OnGet(int Id)
        {
            //_FoodType = _db.FoodType.FirstOrDefault(u=>u.Id==Id);
            _FoodType = _unitofwork.FoodType.GetFirstOrDefault(u => u.Id == Id);
            //_Category = _db.Category.Find(Id);
            //_Category = _db.Category.Single(u => u.Id == Id);
            //_Category = _db.Category.SingleOrDefault(u => u.Id == Id);            
            //_Category = _db.Category.Where(u => u.Id == Id).FirstOrDefault();

        }


        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost() 
        {

      

            if(ModelState.IsValid)
            {
                // _db.FoodType.Update(_FoodType);
                //await _db.SaveChangesAsync();
                _unitofwork.FoodType.Update(_FoodType);
                _unitofwork.Save();

                 TempData["success"] = "Food Type updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }            

            


    }
}
