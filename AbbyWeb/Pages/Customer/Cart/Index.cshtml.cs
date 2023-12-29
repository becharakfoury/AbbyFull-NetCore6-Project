using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {

        public IEnumerable<ShoppingCart> _shoppingCartList { get; set; }

        private readonly IUnitOfWork _unitofwork;
        public double _CardTotal;

        public IndexModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
            _CardTotal = 0;
        }


   

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim!=null)
            {
                _shoppingCartList = _unitofwork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

                foreach (var carditem in _shoppingCartList)
                {
                    _CardTotal += (carditem.MenuItem.Price * carditem.Count);
                }
            }         

        }

        public IActionResult OnPostPlus(int cartId)
        {
            var _cart = _unitofwork.ShoppingCart.GetFirstOrDefault(u=>u.Id == cartId);
            _unitofwork.ShoppingCart.IncrementCount(_cart, 1);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var _cart = _unitofwork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);            
            
            if (_cart.Count==1) {
                var count = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == _cart.ApplicationUserId).ToList().Count - 1;

                _unitofwork.ShoppingCart.Remove(_cart);
                _unitofwork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitofwork.ShoppingCart.DecrementCount(_cart, 1);
            }
            return RedirectToPage("/Customer/Cart/Index");


        }

        public IActionResult OnPostRemove(int cartId)
        {
            var _cart = _unitofwork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            var count = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == _cart.ApplicationUserId).ToList().Count - 1;

            _unitofwork.ShoppingCart.Remove(_cart);
            _unitofwork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToPage("/Customer/Cart/Index");
        }

    }
}
