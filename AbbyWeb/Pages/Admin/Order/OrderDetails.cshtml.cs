using Abby.DataAccess.Repository.IRepository;
using Abby.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Abby.Utility;
using Abby.Models;
using Stripe;
using Abby.DataAccess.Repository;

namespace AbbyWeb.Pages.Admin.Order
{
    public class OrderDetailsModel : PageModel
    {

        private readonly IUnitOfWork _UnitOfWork;
        public OrderDetailVM OrderDetailVM { get; set; }
        public OrderDetailsModel(IUnitOfWork unitofwork)
        {
            _UnitOfWork = unitofwork;
        }

        public void OnGet(int Id)
        {
            OrderDetailVM = new()
            {
                //OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser"),
                OrderHeader = _UnitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == Id),
                OrderDetails = _UnitOfWork.OrderDetail.GetAll(u => u.OrderId == Id).ToList()
             };
        }

        public IActionResult OnPostOrderCompleted(int orderId)
        {
            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusCompleted);
            _UnitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderRefund(int orderId)
        {

            OrderHeader orderHeader = _UnitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId
            };

            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusRefunded);
            var service = new RefundService();
            Refund refund = service.Create(options);

            _UnitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusCancelled);
            _UnitOfWork.Save();
            return RedirectToPage("OrderList");
        }



    }
}
