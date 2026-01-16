using AntiqueStoreApp.Models;
using System.Collections.Generic;

public class Product
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string Category { get; set; }
    public string Condition { get; set; }

    public Supplier Supplier { get; set; }  
    public List<Product> Products { get; private set; }

}
