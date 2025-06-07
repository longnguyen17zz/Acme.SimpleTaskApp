using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Orders;
using Acme.SimpleTaskApp.Web.Models.InfoOrder;
using Acme.SimpleTaskApp.Web.Models.OrderSuccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Acme.SimpleTaskApp.Web.Models.InfoOrder.InfoOrderDetailViewModel;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class OrdersController : SimpleTaskAppControllerBase
    {
        private readonly IOrderAppService _orderAppService;


        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
          
        }

        public async Task<ActionResult> GetOrderList()
        {

            var orderList = await _orderAppService.GetOrderListUser();

            var viewModel = new InfoOrderListViewModel
            {
                OrderItems = orderList
            };
            return View(viewModel);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var orderDetail = await _orderAppService.GetOrderDetail(id);
            var model = new InfoOrderDetailViewModel
            {
                OrderId = id,
                PaymentMethod = orderDetail.PaymentMethod,
                TotalAmount = orderDetail.TotalAmount,
                Status = orderDetail.Status,
                OrderDate = orderDetail.OrderDate,
                FullName = orderDetail.FullName,
                Address = orderDetail.Address,
                PhoneNumber = orderDetail.PhoneNumber,
                Items = orderDetail.Items.Select(i => new OrderDetailUserDto
                {
                    ProductName = i.ProductName,
                    Image = i.Image,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()


            };
            return View(model); 
        }

        public async Task<ActionResult> OrderSuccess(int id)
        {
            var dto = await _orderAppService.GetAsync(new EntityDto<int>(id));

            var viewModel = new OrderSuccessViewModel
            {
                OrderCode = dto.OrderCode,
                OrderDate = dto.OrderDate,
                PaymentMethod = dto.PaymentMethod,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status.ToString(),
                EstimatedDeliveryDate = dto.EstimatedDeliveryDate
            };

            return View(viewModel);
        }

       
    }
}
