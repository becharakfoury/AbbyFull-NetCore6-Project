using Abby.DataAccess.Data;
using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AbbyWeb.Pages.Admin.MenuItems
{

    [BindProperties] // will be used to bind all properties / on class level
    public class UpsertModel : PageModel
    {

        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public IEnumerable<SelectListItem> _CategoryList { get; set; }
        public IEnumerable<SelectListItem> _FoodTypeList { get; set; }

        public MenuItem _MenuItem { get; set; }
        

        //public CreateModel(ApplicationDbContext db)
        public UpsertModel(IUnitOfWork unitofwork, IWebHostEnvironment hostEnvironment)
        {
            // _db = db;
            _unitofwork = unitofwork;
            _hostEnvironment = hostEnvironment;
            _MenuItem = new(); // needed to load first time the page
        }

        public void OnGet(int? id)
        {

            if (id != null)
            {
                //Bind / bring MenuItem data from databse
                _MenuItem = _unitofwork.MenuItem.GetFirstOrDefault(u => u.Id == id);
            }

            //List of definition are needed in all cases: New / Edit
            _CategoryList = _unitofwork.Category.GetAll().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            _FoodTypeList = _unitofwork.FoodType.GetAll().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

        }

        //we can use OnPostCreate / OnPostEdit  to have multiple post handlers
        //public async Task<IActionResult> OnPost(Category _Category) 
        public async Task<IActionResult> OnPost()
        {

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (_MenuItem.Id == 0)
            {
                //create
                if (files.Count > 0)
                {
                    string fileName_new = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    _MenuItem.Image = @"\images\menuItems\" + fileName_new + extension;
                }else
                {
                    _MenuItem.Image = "";
                }
         

                _unitofwork.MenuItem.Add(_MenuItem);
                _unitofwork.Save();
            }
            else
            {
                //edit
                var objFromDb = _unitofwork.MenuItem.GetFirstOrDefault(u => u.Id == _MenuItem.Id);
                if (files.Count > 0)
                {
                    string fileName_new = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    //delete the old image
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));
                    // ==> \\ will be used to remove \ from the db record
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    _MenuItem.Image = @"\images\menuItems\" + fileName_new + extension;
                }
                else
                {
                    _MenuItem.Image = objFromDb.Image;
                }

                _unitofwork.MenuItem.Update(_MenuItem);
                _unitofwork.Save();

            }

            return RedirectToPage("./Index");

        }




    }
}
