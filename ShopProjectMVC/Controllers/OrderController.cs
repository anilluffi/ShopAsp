using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Controllers;

public class OrderController : Controller
{
    
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
        
    }
    
    public IActionResult Orders()
    {
        if (HttpContext.Session.GetString("user") == null)
        {
            return RedirectToAction("Login", "User");
        }
        else
        {
            int userId = HttpContext.Session.GetInt32("id").Value;
            IEnumerable<Order> orders = _orderService.GetOrders(userId).ToList();
            return View(orders);
        }
        
    }
}