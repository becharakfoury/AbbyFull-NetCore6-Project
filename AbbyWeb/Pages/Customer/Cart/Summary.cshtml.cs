using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using Stripe.Identity;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    [BindProperties]
    public class SummaryModel : PageModel
    {
        public IEnumerable<ShoppingCart> _shoppingCartList { get; set; }
        public OrderHeader _orderheader { get; set; }


        private readonly IUnitOfWork _unitofwork;       

        public SummaryModel(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
            _orderheader = new OrderHeader();
        }

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                _shoppingCartList = _unitofwork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

                foreach (var carditem in _shoppingCartList)
                {
                    _orderheader.OrderTotal += (carditem.MenuItem.Price * carditem.Count);
                }

                ApplicationUser _ApplicationUser = _unitofwork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                if (_ApplicationUser != null)
                {
                    _orderheader.PickupName = _ApplicationUser.FirstName + " " + _ApplicationUser.LastName;
                    _orderheader.PhoneNumber = _ApplicationUser.PhoneNumber;

                }

            }

           

        }



		public IActionResult OnPost()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim != null)
			{
				_shoppingCartList = _unitofwork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claim.Value,
					includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

				foreach (var carditem in _shoppingCartList)
				{
					_orderheader.OrderTotal += (carditem.MenuItem.Price * carditem.Count);
				}

                _orderheader.Status = SD.StatusPending;
                _orderheader.OrderDate = System.DateTime.Now;
                _orderheader.UserId = claim.Value;
				_orderheader.PickUpTime = Convert.ToDateTime(_orderheader.PickUpDate.ToShortDateString() + " " + _orderheader.PickUpTime.ToShortTimeString());
                _unitofwork.OrderHeader.Add(_orderheader);
                _unitofwork.Save();

                foreach (var item in _shoppingCartList)
                {
                    OrderDetails orderDetails = new()
                    {
                        OrderId = _orderheader.Id,
                        MenuItemId = item.MenuItemId,
                        Count = item.Count,
                        Price = item.MenuItem.Price,
                        Name = item.MenuItem.Name

					};
					_unitofwork.OrderDetail.Add(orderDetails);
				}

				//clear the shopping cart
				//var quantity = _shoppingCartList.ToList().Count;				
				//_unitofwork.ShoppingCart.RemoveRange(_shoppingCartList);
				_unitofwork.Save();

				//Stipe Payment  xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
				var domain = "https://localhost:44358/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),                 
                    PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                    Mode = "payment",
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={_orderheader.Id}",
                    CancelUrl = domain + "Customer/Cart/index",
                };



				//Add Line items / to display list of Ordered Items in Stripe Page
                foreach (var item in _shoppingCartList) {
                    var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							//7.99->799
							UnitAmount = (long)(item.MenuItem.Price * 100),
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.MenuItem.Name
							},
						},
						Quantity = item.Count
					};

					options.LineItems.Add(sessionLineItem);
				}
				//Finish Line items / to display list of Ordered Items in Stripe Page


				var service = new Stripe.Checkout.SessionService();
				Session session = service.Create(options);

				_orderheader.SessionId = session.Id;
				_orderheader.PaymentIntentId = session.PaymentIntentId;
				_unitofwork.Save(); //OrderHEader is tracked by Entity Framework

				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303); // 303: it will redirect to the session

				// xxxxxxxxxxxxxxaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
			}
            return Page(); // Case the user was not Anthenticated
		}
}
}
