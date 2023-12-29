using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories
{

    [BindProperties] // will be used to bind all properties / on class level
    public class DeleteModel : PageModel
    {

        private readonly IUnitOfWork _unitofwork;


        public Category _Category { get; set; }


        public DeleteModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public void OnGet(int Id)
        {            
            _Category = _unitofwork.Category.GetFirstOrDefault(u=>u.Id==Id);
            //_Category = _db.Category.Find(Id);
            //_Category = _db.Category.Single(u => u.Id == Id);
            //_Category = _db.Category.SingleOrDefault(u => u.Id == Id);            
            //_Category = _db.Category.Where(u => u.Id == Id).FirstOrDefault();

        }


        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost() 
        {               
                        
            var categoryfromdb = _unitofwork.Category.GetFirstOrDefault(u => u.Id == _Category.Id);
            if (categoryfromdb != null)
            {
                _unitofwork.Category.Remove(categoryfromdb);
                _unitofwork.Save();

                //_db.Category.Remove(categoryfromdb);
                //await _db.SaveChangesAsync();


                TempData["success"] = "Category deleted successfully";

                return RedirectToPage("Index");
            }

            return Page();


        }




    }
}
