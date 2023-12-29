using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories
{

    [BindProperties] // will be used to bind all properties / on class level
    public class CreateModel : PageModel
    {

        private readonly IUnitOfWork _unitofwork;

        public Category Category { get; set; }

        public CreateModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public void OnGet()
        {
        }

        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost() 
        {

            if (Category.Name == Category.DisplayOrder.ToString()) {
                 //The below work also   
                //ModelState.AddModelError(string.Empty, "Name and Display Order cannot be the same!");
                ModelState.AddModelError("Category.Name", "Name and Display Order cannot be the same!");
            }

            if(ModelState.IsValid)
            {
                _unitofwork.Category.Add(Category);
                _unitofwork.Save();

                //method working when using DB Context only
                //await _db.Category.AddAsync(_Category);
                //await _db.SaveChangesAsync();

                //method using irepository
                //_dbCategory.Add(Category);
                //_dbCategory.Save();


                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }            

            


    }
}
