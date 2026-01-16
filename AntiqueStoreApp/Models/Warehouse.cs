using System;

public class Warehouse
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public string Status { get; set; }
    public DateTime? OrderedDate { get; set; }
    public string ProductName { get; set; }
}
