using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace AbbyWeb.Pages.Customer.Cart
{
	[Authorize]
	public class OrderConfirmationModel : PageModel
    {
	
		private readonly IUnitOfWork _unitofwork;
		public int OrderId { get; set; }

		public OrderConfirmationModel(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;		
		}


		public void OnGet(int Id)
        {
			OrderHeader _orderheader = _unitofwork.OrderHeader.GetFirstOrDefault (u  => u.Id == Id);
			if (_orderheader.SessionId != null)
			{
				var service = new SessionService();
				Session session = service.Get(_orderheader.SessionId);
				if (session.PaymentStatus.ToLower()=="paid")
				{
					_orderheader.Status = SD.StatusSubmitted;
					_unitofwork.Save();
				}

				//Clear the shopping Cart
				List<ShoppingCart> _shoppingcarts = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == _orderheader.UserId).ToList();
				_unitofwork.ShoppingCart.RemoveRange(_shoppingcarts);
				_unitofwork.Save();
				OrderId = Id;

			}

        }
    }
}
