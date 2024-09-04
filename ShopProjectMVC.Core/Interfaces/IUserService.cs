using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Core.Interfaces;

public interface IUserService
{
    Task<User> Login(string email, string password);
    Task<User> Register(User user);
}
