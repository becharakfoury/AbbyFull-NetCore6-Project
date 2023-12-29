using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;



//using Abby.Utility;
//using Microsoft.AspNetCore.Http;
//using System.Linq;



namespace AbbyWeb.Pages.Customer.Home
{

    [Authorize]
    public class DetailsModel : PageModel
    {
		private readonly IUnitOfWork _unitofwork;
		

		public DetailsModel(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;
		}

		[BindProperty]
        public ShoppingCart _ShoppingCart { get; set; }

        //public MenuItem _MenuItem { get; set; }
        //count is needed and define in the code because we do not have in the database a field related to it in: Menu Item
        //[Range(1, 10, ErrorMessage ="Please select a count between 1 and 100")]
		//public int Count {  get; set; }

		public void OnGet(int Id)
        {

            //First Code version logic
            // _MenuItem = _unitofwork.MenuItem.GetFirstOrDefault(u => u.Id == Id, includeProperties: "Category,FoodType")


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            _ShoppingCart = new()
            {
                ApplicationUserId = claim.Value,
                MenuItem = _unitofwork.MenuItem.GetFirstOrDefault(u => u.Id == Id, includeProperties: "Category,FoodType"),
                MenuItemId = Id //we will map from recieved parameter ID
            };
            

		}


        public IActionResult OnPost()
        {

            if (ModelState.IsValid)
            {

                ShoppingCart _shoppingCartFromDb = _unitofwork.ShoppingCart.GetFirstOrDefault(
                    filter: u => u.ApplicationUserId == _ShoppingCart.ApplicationUserId &&
                    u.MenuItemId == _ShoppingCart.MenuItemId);

                if(_shoppingCartFromDb==null)
                {

                    _unitofwork.ShoppingCart.Add(_ShoppingCart);
                    _unitofwork.Save();

                    HttpContext.Session.SetInt32(SD.SessionCart,
                      _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == _ShoppingCart.ApplicationUserId).ToList().Count);
              
                }
                else
                {
                    _unitofwork.ShoppingCart.IncrementCount(_shoppingCartFromDb, _ShoppingCart.Count);
                }
              

                return RedirectToPage("Index");
            }
           else
            {
                return Page();
            }

        }

    }
}
