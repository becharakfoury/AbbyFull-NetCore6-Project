using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Models.ViewModel;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Order
{
    [Authorize(Roles = $"{SD.ManagerRole},{SD.KitchenRole}")]
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _UnitOfWork;       
        public List<OrderDetailVM> _orderDetailVMs { get; set; } 
        public ManageOrderModel(IUnitOfWork unitofwork)
        {
            _UnitOfWork = unitofwork;
        }



        public void OnGet()
        {
            _orderDetailVMs = new();

            List<OrderHeader> _orderHeader = _UnitOfWork.OrderHeader.GetAll( 
                u => u.Status == SD.StatusSubmitted ||
                u.Status == SD.StatusInProcess).ToList();

            foreach(OrderHeader item in _orderHeader)
            {
                OrderDetailVM individual = new OrderDetailVM()
                {
                    OrderHeader = item,
                    OrderDetails = _UnitOfWork.OrderDetail.GetAll(u => u.OrderId == item.Id).ToList()
                };
                _orderDetailVMs.Add(individual);
            }

        }

        public IActionResult OnPostOrderInProcess(int orderId)
        {
            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusInProcess);
            _UnitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusReady);
            _UnitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            _UnitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusCancelled);
            _UnitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

    }
}
