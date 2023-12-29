using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Abby.Utility;

namespace AbbyWeb.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		
		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;		
		}


        [Authorize]
        [HttpGet]
		public IActionResult Get(string? status=null)
		{
					

            //var _oederheaderlist = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            var _oederheaderlist = _unitOfWork.OrderHeader.GetAll();

			if (status == "cancelled")
			{
				_oederheaderlist = _oederheaderlist.Where(u => u.Status == SD.StatusCancelled || u.Status == SD.StatusRejected);
			}
			else
			{
				if (status == "completed")
				{
					_oederheaderlist = _oederheaderlist.Where(u => u.Status == SD.StatusCompleted);
				}
				else
				{
					if (status == "ready")
					{
						_oederheaderlist = _oederheaderlist.Where(u => u.Status == SD.StatusReady);
					}
					else
					{
						_oederheaderlist = _oederheaderlist.Where(u => u.Status == SD.StatusSubmitted || u.Status == SD.StatusInProcess);
					}
				}
			}

			return Json(new { data = _oederheaderlist });
		}
	}
}
