namespace ShopProjectMVC.Core.Models;

public class Order
{
    public int Id { get; set; }
    public virtual User User { get; set; }
    public virtual Product Product { get; set; }
    public DateTime CreatedAt { get; set; }
}

/*
 - buy product (create order)
 - view products (get products)
 - login/register (get/create user)
 - sorting/filter (products)
 - view orders (get orders)
 - view orders with limits
 - add product
 - remove product
 - update product
 */