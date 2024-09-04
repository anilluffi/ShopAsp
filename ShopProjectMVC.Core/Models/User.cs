namespace ShopProjectMVC.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public Role Role { get; set; }
}

public enum Role
{
    Client,
    Admin
}
