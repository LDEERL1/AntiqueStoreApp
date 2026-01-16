using AntiqueStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

public class DataAccess
{
    private string connectionString = "Server=DESKTOP-F9V8BJ6;Database=AntiqueStore;Integrated Security=True;";



    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT p.ID, p.Name, p.Description, p.Price, p.Year, p.Category, p.Condition, 
                   s.ID AS SupplierID, s.Name AS SupplierName
            FROM Products p
            LEFT JOIN Suppliers s ON p.SupplierID = s.ID";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    Year = (int)reader["Year"],
                    Category = (string)reader["Category"],
                    Condition = (string)reader["Condition"],
                    Supplier = new Supplier
                    {
                        ID = (int)reader["SupplierID"],
                        Name = (reader["SupplierName"] != DBNull.Value) ? (string)reader["SupplierName"] : "Неизвестный поставщик"
                    }
                });
            }
        }

        return products;
    }







    public int AddProduct(string name, string description, decimal price, int year, string category, string condition, Supplier supplier)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            INSERT INTO Products (Name, Description, Price, Year, Category, Condition, SupplierID)
            OUTPUT INSERTED.ID
            VALUES (@Name, @Description, @Price, @Year, @Category, @Condition, @SupplierID)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Price", price);
            command.Parameters.AddWithValue("@Year", year);
            command.Parameters.AddWithValue("@Category", category);
            command.Parameters.AddWithValue("@Condition", condition);
            command.Parameters.AddWithValue("@SupplierID", supplier.ID);  

            connection.Open();
            int insertedId = (int)command.ExecuteScalar(); 
            return insertedId;
        }
    }



    public void UpdateProduct(Product product)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Year = @Year, Category = @Category, Condition = @Condition, SupplierID = @SupplierID WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Year", product.Year);
            command.Parameters.AddWithValue("@Category", product.Category);
            command.Parameters.AddWithValue("@Condition", product.Condition);
            command.Parameters.AddWithValue("@SupplierID", product.Supplier.ID);  
            command.Parameters.AddWithValue("@ID", product.ID);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void DeleteProduct(int productId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Products WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", productId);
            command.ExecuteNonQuery();
        }
    }

    public bool ProductExists(string name, int? excludeId = null)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM Products WHERE Name = @Name";
            if (excludeId.HasValue)
            {
                query += " AND ID != @ExcludeID";
            }

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            if (excludeId.HasValue)
            {
                command.Parameters.AddWithValue("@ExcludeID", excludeId.Value);
            }

            connection.Open();
            int count = (int)command.ExecuteScalar();
            return count > 0; 
        }
    }






    public List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ID, FullName, ContactDetails FROM Customers";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    ID = (int)reader["ID"],
                    FullName = (string)reader["FullName"],
                    ContactDetails = (string)reader["ContactDetails"]
                });
            }
        }

        return customers;
    }
    public void AddCustomer(string fullName, string contactDetails)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Customers (FullName, ContactDetails) VALUES (@FullName, @ContactDetails)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@ContactDetails", contactDetails);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateCustomer(Customer customer)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE Customers SET FullName = @FullName, ContactDetails = @ContactDetails WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", customer.FullName);
            command.Parameters.AddWithValue("@ContactDetails", customer.ContactDetails);
            command.Parameters.AddWithValue("@ID", customer.ID);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void DeleteCustomer(int customerId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Customers WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", customerId);
            command.ExecuteNonQuery();
        }
    }


   
    public List<Supplier> GetSuppliers()
    {
        List<Supplier> suppliers = new List<Supplier>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Suppliers";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                suppliers.Add(new Supplier
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    ContactDetails = (string)reader["ContactDetails"]
                });
            }
        }

        return suppliers;
    }


    public void AddSupplier(string name, string contactDetails)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Suppliers (Name, ContactDetails) VALUES (@Name, @ContactDetails)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@ContactDetails", contactDetails);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void UpdateSupplier(Supplier supplier)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE Suppliers SET Name = @Name, ContactDetails = @ContactDetails WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", supplier.Name);
            command.Parameters.AddWithValue("@ContactDetails", supplier.ContactDetails);
            command.Parameters.AddWithValue("@ID", supplier.ID);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    // Удалить поставщика
    public void DeleteSupplier(int supplierId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Suppliers WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", supplierId);
            command.ExecuteNonQuery();
        }
    }


    public List<Product> GetProductsBySupplier(int supplierId)
    {
        List<Product> products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
            SELECT p.ID, p.Name, p.Description, p.Price, p.Year, p.Category, p.Condition, 
                   s.ID AS SupplierID, s.Name AS SupplierName
            FROM Products p
            LEFT JOIN Suppliers s ON p.SupplierID = s.ID
            WHERE p.SupplierID = @SupplierID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupplierID", supplierId);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Condition = (string)reader["Condition"],
                    Price = (decimal)reader["Price"],
                    Year = (int)reader["Year"],
                    Category = (string)reader["Category"],
                    Supplier = new Supplier 
                    {
                        ID = (int)reader["SupplierID"],
                        Name = (string)reader["SupplierName"]
                    }
                });
            }
        }

        return products;
    }




    public List<Warehouse> GetWarehouse()
    {
        List<Warehouse> warehouse = new List<Warehouse>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
        SELECT w.ID, w.ProductID, w.Quantity, w.Location, w.ArrivalDate, w.Status, w.OrderedDate,
               p.Name AS ProductName
        FROM Warehouse w
        LEFT JOIN Products p ON w.ProductID = p.ID";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                warehouse.Add(new Warehouse
                {
                    ID = (int)reader["ID"],
                    ProductID = (int)reader["ProductID"],
                    Quantity = (int)reader["Quantity"],
                    Location = (string)reader["Location"],
                    ArrivalDate = reader["ArrivalDate"] as DateTime?,
                    Status = (string)reader["Status"],
                    OrderedDate = reader["OrderedDate"] as DateTime?,
                    ProductName = (string)reader["ProductName"]
                });
            }
        }
        return warehouse;
    }



    public List<string> GetCategories()
    {
        return new List<string>
    {
        "Мебель",
        "Искусство",
        "Посуда",
        "Декор",
        "Одежда"
    };
    }


    public void AddWarehouse(int productId, int quantity, string location, DateTime? arrivalDate, string status, DateTime? orderedDate)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Warehouse (ProductID, Quantity, Location, ArrivalDate, Status, OrderedDate) VALUES (@ProductID, @Quantity, @Location, @ArrivalDate, @Status, @OrderedDate)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@Location", location);
            command.Parameters.AddWithValue("@ArrivalDate", (object)arrivalDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@OrderedDate", (object)orderedDate ?? DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateWarehouse(Warehouse warehouse)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            UPDATE Warehouse 
            SET ProductID = @ProductID, 
                Quantity = @Quantity, 
                Location = @Location, 
                ArrivalDate = @ArrivalDate, 
                Status = @Status, 
                OrderedDate = @OrderedDate 
            WHERE ID = @ID"; 

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", warehouse.ProductID);
            command.Parameters.AddWithValue("@Quantity", warehouse.Quantity);
            command.Parameters.AddWithValue("@Location", warehouse.Location);
            command.Parameters.AddWithValue("@ArrivalDate", (object)warehouse.ArrivalDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", warehouse.Status);
            command.Parameters.AddWithValue("@OrderedDate", (object)warehouse.OrderedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@ID", warehouse.ID); 

            connection.Open();
            command.ExecuteNonQuery();
        }
    }



    
    public void DeleteWarehouse(int warehouseId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Warehouse WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", warehouseId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }



    
    public Warehouse GetWarehouseByProductId(int productId)
    {
        Warehouse warehouse = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Warehouse WHERE ProductID = @ProductID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                warehouse = new Warehouse
                {
                    ID = (int)reader["ID"],
                    ProductID = (int)reader["ProductID"],
                    Quantity = (int)reader["Quantity"],
                    Location = (string)reader["Location"],
                    ArrivalDate = reader["ArrivalDate"] as DateTime?,
                    Status = (string)reader["Status"],
                    OrderedDate = reader["OrderedDate"] as DateTime?
                };
            }
        }

        return warehouse;
    }
    public Product GetProductById(int productId)
    {
        Product product = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT p.ID, p.Name, p.Description, p.Price, p.Year, p.Category, p.Condition, " +
                           "s.ID AS SupplierID, s.Name AS SupplierName " +
                           "FROM Products p " +
                           "LEFT JOIN Suppliers s ON p.SupplierID = s.ID " +
                           "WHERE p.ID = @ProductID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                product = new Product
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Price = (decimal)reader["Price"],
                    Year = (int)reader["Year"],
                    Category = (string)reader["Category"],
                    Condition = (string)reader["Condition"],
                    Supplier = new Supplier
                    {
                        ID = (int)reader["SupplierID"],
                        Name = (string)reader["SupplierName"]
                    }  
                };
            }
        }

        return product;
    }

    public void DeleteWarehouseByProductId(int productId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Warehouse WHERE ProductID = @ProductID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);
            command.ExecuteNonQuery();
        }
    }






    public List<OrderDetail> GetAllOrderDetails()
    {
        List<OrderDetail> orderDetails = new List<OrderDetail>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, od.Price, p.Name AS ProductName
            FROM OrderDetails od
            JOIN Products p ON od.ProductID = p.ID";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderDetailID = (int)reader["OrderDetailID"],
                    OrderID = (int)reader["OrderID"],
                    ProductID = (int)reader["ProductID"],
                    Quantity = (int)reader["Quantity"],
                    Price = (decimal)reader["Price"],
                    ProductName = (string)reader["ProductName"]
                });
            }
        }
        return orderDetails;
    }

    public List<OrderDetail> GetOrderDetails(int orderId)
    {
        List<OrderDetail> orderDetails = new List<OrderDetail>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, od.Price, p.Name AS ProductName
            FROM OrderDetails od
            JOIN Products p ON od.ProductID = p.ID
            WHERE od.OrderID = @OrderID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", orderId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderDetailID = (int)reader["OrderDetailID"],
                    OrderID = (int)reader["OrderID"],
                    ProductID = (int)reader["ProductID"],
                    Quantity = (int)reader["Quantity"],
                    Price = (decimal)reader["Price"],
                    ProductName = (string)reader["ProductName"] 
                });
            }
        }
        return orderDetails;
    }

    public List<Order> GetOrders()
    {
        List<Order> orders = new List<Order>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT o.OrderID, o.CustomerID, o.OrderDate, c.FullName AS CustomerFullName,
                   (SELECT SUM(od.Price) 
                    FROM OrderDetails od 
                    WHERE od.OrderID = o.OrderID) AS TotalPrice
            FROM Orders o
            JOIN Customers c ON o.CustomerID = c.ID";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = (int)reader["OrderID"],
                    CustomerID = (int)reader["CustomerID"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    CustomerFullName = (string)reader["CustomerFullName"],
                    TotalPrice = reader["TotalPrice"] != DBNull.Value ? (decimal)reader["TotalPrice"] : 0
                });
            }
        }
        return orders;
    }


    public int AddOrder(Order order)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            INSERT INTO Orders (CustomerID, OrderDate)
            OUTPUT INSERTED.OrderID
            VALUES (@CustomerID, @OrderDate)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);

            connection.Open();
            return (int)command.ExecuteScalar(); 
        }
    }


    public void UpdateOrder(Order order)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            UPDATE Orders 
            SET CustomerID = @CustomerID, OrderDate = @OrderDate
            WHERE OrderID = @OrderID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@OrderID", order.OrderID);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void DeleteOrder(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Orders WHERE OrderID = @OrderID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", orderId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void AddOrderDetail(OrderDetail detail)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price)
            VALUES (@OrderID, @ProductID, @Quantity, @Price)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", detail.OrderID);
            command.Parameters.AddWithValue("@ProductID", detail.ProductID);
            command.Parameters.AddWithValue("@Quantity", detail.Quantity);
            command.Parameters.AddWithValue("@Price", detail.Price);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void UpdateOrderDetail(OrderDetail detail)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            UPDATE OrderDetails 
            SET ProductID = @ProductID, Quantity = @Quantity, Price = @Price
            WHERE OrderDetailID = @OrderDetailID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderDetailID", detail.OrderDetailID);
            command.Parameters.AddWithValue("@ProductID", detail.ProductID);
            command.Parameters.AddWithValue("@Quantity", detail.Quantity);
            command.Parameters.AddWithValue("@Price", detail.Price);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void DeleteOrderDetail(int orderDetailId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderDetailID", orderDetailId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateWarehouseQuantity(int productId, int quantityChange)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            UPDATE Warehouse 
            SET Quantity = Quantity + @QuantityChange 
            WHERE ProductID = @ProductID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@QuantityChange", quantityChange);
            command.Parameters.AddWithValue("@ProductID", productId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void ReturnStock(int productId, int quantity)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"
            UPDATE Warehouse
            SET Quantity = Quantity + @Quantity
            WHERE ProductID = @ProductID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@ProductID", productId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }


}
