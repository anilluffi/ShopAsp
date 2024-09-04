using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login()
    {

        return View();
    }

    public IActionResult Register()
    {
        return View();  
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var userFromDb = await _userService.Login(user.Email, user.Password);
        if (userFromDb == null)
        {
            return NotFound();
        }
        // save user
        HttpContext.Session.SetString("user", userFromDb.Name);
        HttpContext.Session.SetInt32("role", (int)userFromDb.Role); 
        HttpContext.Session.SetInt32("id", userFromDb.Id);
        return RedirectToAction("Products", "Product");
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        user.Role = Role.Client;
        user.CreatedAt = DateTime.UtcNow;
        await _userService.Register(user);
        HttpContext.Session.SetString("user", user.Name);
        HttpContext.Session.SetInt32("role", (int)user.Role);
        HttpContext.Session.SetInt32("id", user.Id);
        return RedirectToAction("Products", "Product");
    }
}
