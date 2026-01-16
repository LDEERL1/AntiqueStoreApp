using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AntiqueStoreApp.Models;

namespace AntiqueStoreApp
{
    public partial class AddEditOrderWindow : Window
    {
        private DataAccess dataAccess = new DataAccess();
        public Order CurrentOrder { get; private set; }
        public List<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
        public List<Product> Products { get; private set; }
        private bool isEditingCell = false;


        public AddEditOrderWindow(Order order = null)
        {
            InitializeComponent();
            LoadCustomers();
            LoadProducts();

            if (order != null)
            {
                CurrentOrder = order;
                CustomerComboBox.SelectedValue = order.CustomerID;
                OrderDatePicker.SelectedDate = order.OrderDate;
                OrderDetails = dataAccess.GetOrderDetails(order.OrderID);
            }

            OrderDetailsGrid.ItemsSource = OrderDetails; 
        }





        private void LoadCustomers()
        {
            var customers = dataAccess.GetCustomers();
            CustomerComboBox.ItemsSource = customers;
        }

        private void LoadProducts()
        {
            Products = dataAccess.GetProducts(); 
            OrderDetailsGrid.Columns.OfType<DataGridComboBoxColumn>().FirstOrDefault(column => column.Header.ToString() == "Товар")
                .ItemsSource = Products;
        }


        private void SaveOrder(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedValue == null || !OrderDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentOrder == null)
            {
                CurrentOrder = new Order
                {
                    CustomerID = (int)CustomerComboBox.SelectedValue,
                    OrderDate = OrderDatePicker.SelectedDate.Value
                };
                CurrentOrder.OrderID = dataAccess.AddOrder(CurrentOrder);
            }
            else
            {
                CurrentOrder.CustomerID = (int)CustomerComboBox.SelectedValue;
                CurrentOrder.OrderDate = OrderDatePicker.SelectedDate.Value;

                var oldDetails = dataAccess.GetOrderDetails(CurrentOrder.OrderID);
                foreach (var detail in oldDetails)
                {
                    dataAccess.ReturnStock(detail.ProductID, detail.Quantity);
                }

                dataAccess.UpdateOrder(CurrentOrder);
            }

            foreach (var detail in OrderDetails)
            {
                if (!ValidateStock(detail))
                {
                    return;
                }

                detail.Price = Products.FirstOrDefault(p => p.ID == detail.ProductID)?.Price * detail.Quantity ?? 0;

                if (detail.OrderDetailID == 0)
                {
                    detail.OrderID = CurrentOrder.OrderID;
                    dataAccess.AddOrderDetail(detail);
                }
                else
                {
                    dataAccess.UpdateOrderDetail(detail);
                }

                dataAccess.UpdateWarehouseQuantity(detail.ProductID, -detail.Quantity);
            }

            CurrentOrder.TotalPrice = OrderDetails.Sum(d => d.Price);

            ((MainWindow)Application.Current.MainWindow).LoadOrders();

            this.DialogResult = true;
            this.Close();
        }




        private void CancelOrder(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool ValidateStock(OrderDetail detail)
        {
            var stock = dataAccess.GetWarehouseByProductId(detail.ProductID);
            if (stock == null || stock.Quantity < detail.Quantity)
            {
                MessageBox.Show($"Недостаточно товара '{detail.ProductName}' на складе. Доступно: {stock?.Quantity ?? 0}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private void CheckProducts()
        {
            if (Products != null && Products.Any())
            {
                Console.WriteLine("Товары успешно загружены:");
                foreach (var product in Products)
                {
                    Console.WriteLine($"ID: {product.ID}, Name: {product.Name}");
                }
            }
            else
            {
                MessageBox.Show("Товары не загружены. Проверьте метод LoadProducts.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OrderDetailsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Количество" || e.Column.Header.ToString() == "Товар")
            {
                var oldDetail = e.Row.Item as OrderDetail;
                if (oldDetail != null)
                {
                    var previousDetail = new OrderDetail
                    {
                        ProductID = oldDetail.ProductID,
                        Quantity = oldDetail.Quantity
                    };

                    int newQuantity = int.TryParse((e.EditingElement as TextBox)?.Text, out int parsedQuantity) ? parsedQuantity : oldDetail.Quantity;
                    var newDetail = new OrderDetail
                    {
                        ProductID = oldDetail.ProductID,
                        Quantity = newQuantity
                    };

                    UpdateStockForQuantityChange(previousDetail, newDetail);

                    oldDetail.Quantity = newQuantity;
                    oldDetail.Price = Products.FirstOrDefault(p => p.ID == oldDetail.ProductID)?.Price * newQuantity ?? 0;

                    OrderDetailsGrid.ItemsSource = null;
                    OrderDetailsGrid.ItemsSource = OrderDetails;
                }
            }
        }





        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedValue is int selectedProductId)
            {
                var currentDetail = OrderDetailsGrid.SelectedItem as OrderDetail;
                if (currentDetail != null)
                {
                    var selectedProduct = Products.FirstOrDefault(p => p.ID == selectedProductId);
                    if (selectedProduct != null)
                    {
                        currentDetail.Price = selectedProduct.Price * currentDetail.Quantity; // Цена = цена товара * количество
                        currentDetail.ProductName = selectedProduct.Name;
                    }
                }
            }
        }




        private void ReturnStockForChangedItem(OrderDetail oldDetail, OrderDetail newDetail)
        {
            if (oldDetail != null && oldDetail.ProductID != newDetail.ProductID)
            {
                if (oldDetail.ProductID > 0 && oldDetail.Quantity > 0)
                {
                    dataAccess.UpdateWarehouseQuantity(oldDetail.ProductID, oldDetail.Quantity);
                }

                if (newDetail.ProductID > 0 && newDetail.Quantity > 0)
                {
                    dataAccess.UpdateWarehouseQuantity(newDetail.ProductID, -newDetail.Quantity);
                }
            }
        }

        private void UpdateStockForQuantityChange(OrderDetail oldDetail, OrderDetail newDetail)
        {
            if (oldDetail != null && newDetail != null)
            {
                int quantityChange = newDetail.Quantity - oldDetail.Quantity;

                if (quantityChange < 0)
                {
                    dataAccess.UpdateWarehouseQuantity(newDetail.ProductID, -quantityChange); 
                }
                else if (quantityChange > 0)
                {
                    dataAccess.UpdateWarehouseQuantity(newDetail.ProductID, -quantityChange); 
                }
            }
        }









    }


}
