using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories
{

    [BindProperties] // will be used to bind all properties / on class level
    public class EditModel : PageModel
    {

        private readonly IUnitOfWork _unitofwork;

        public Category _Category { get; set; }

        public EditModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public void OnGet(int Id)
        {
            _Category = _unitofwork.Category.GetFirstOrDefault(u => u.Id == Id);

            //_Category = _db.Category.FirstOrDefault(u=>u.Id==Id);
            //_Category = _db.Category.Find(Id);
            //_Category = _db.Category.Single(u => u.Id == Id);
            //_Category = _db.Category.SingleOrDefault(u => u.Id == Id);            
            //_Category = _db.Category.Where(u => u.Id == Id).FirstOrDefault();
        }


        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost() 
        {

            if (_Category.Name == _Category.DisplayOrder.ToString()) {
                 //The below work also   
                //ModelState.AddModelError(string.Empty, "Name and Display Order cannot be the same!");
                ModelState.AddModelError("_Category.Name", "Name and Display Order cannot be the same!");
            }

            if(ModelState.IsValid)
            {
                _unitofwork.Category.Update(_Category);
                _unitofwork.Save();

                //_db.Category.Update(_Category);
                //await _db.SaveChangesAsync();

                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }            

            


    }
}
