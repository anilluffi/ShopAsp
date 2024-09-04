namespace ShopProjectMVC.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public virtual Category Category { get; set; }
    public int Count { get; set; }
}