using System.Windows;
using AntiqueStoreApp.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Linq;
using OfficeOpenXml;
using System.Windows.Data;

namespace AntiqueStoreApp
{
    public partial class MainWindow : Window
    {
        public DataAccess dataAccess = new DataAccess();

        public MainWindow()
        {
            InitializeComponent();
            LoadCustomers();
            LoadSuppliers();
            LoadProducts();
            LoadOrders();
            LoadAllOrderDetails();

            LoadWarehouse();
            InitializeComponent();

            LoadCategoriesForFilter();  
            LoadSuppliersForFilter();  
            LoadConditionsForFilter();  
        }

        private void LoadCustomers()
        {
            CustomersGrid.ItemsSource = dataAccess.GetCustomers();
        }


        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditCustomerWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void EditCustomer(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem is Customer selectedCustomer)
            {
                var editWindow = new AddEditCustomerWindow(selectedCustomer);
                editWindow.ShowDialog();
                LoadCustomers(); 


            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите покупателя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCustomer(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomersGrid.SelectedItem;
            if (selectedCustomer != null)
            {
                dataAccess.DeleteCustomer(selectedCustomer.ID);
                LoadCustomers(); 
            }
        }





        private void LoadSuppliers()
        {
            SuppliersGrid.ItemsSource = null; 
            SuppliersGrid.ItemsSource = dataAccess.GetSuppliers();
        }


        private void LoadProducts()
        {
            ProductsGrid.ItemsSource = dataAccess.GetProducts();
        }

        public void LoadOrders()
        {
            OrdersGrid.ItemsSource = null;
            OrdersGrid.ItemsSource = dataAccess.GetOrders();
        }



        private void LoadAllOrderDetails()
        {
            var orderDetails = dataAccess.GetAllOrderDetails();
            OrderDetailsGrid.ItemsSource = orderDetails;
        }


        private void LoadWarehouse()
        {
            WarehouseGrid.ItemsSource = dataAccess.GetWarehouse();
        }
        private void AddSupplier(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditSupplierWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadSuppliers();
            }
        }



        private void EditSupplier(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selectedSupplier)
            {
                var editWindow = new AddEditSupplierWindow(selectedSupplier);
                editWindow.ShowDialog();
                LoadSuppliers(); 
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставщика для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteSupplier(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selectedSupplier)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить поставщика {selectedSupplier.Name}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dataAccess.DeleteSupplier(selectedSupplier.ID);
                    MessageBox.Show("Поставщик успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadSuppliers();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставщика для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }




        private void AddProduct(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditProductWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadProducts();

                LoadWarehouse();
            }
        }

        private void EditProduct(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                var editWindow = new AddEditProductWindow(selectedProduct);
                editWindow.ShowDialog();
                LoadProducts();

                LoadWarehouse();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)ProductsGrid.SelectedItem;
            if (selectedProduct != null)
            {
                dataAccess.DeleteProduct(selectedProduct.ID);

                dataAccess.DeleteWarehouseByProductId(selectedProduct.ID);

                LoadProducts(); 
                LoadWarehouse(); 
            }
        }
        private void LoadCategoriesForFilter()
        {
            List<string> categories = new List<string>
    {
        "Мебель", "Искусство", "Посуда", "Декор", "Одежда"
    };
            CategoryFilter.ItemsSource = categories;
            CategoryFilter.SelectedIndex = -1;  
        }

        private void LoadSuppliersForFilter()
        {
            var suppliers = dataAccess.GetSuppliers();
            SupplierFilter.ItemsSource = suppliers;
            SupplierFilter.DisplayMemberPath = "Name"; 
            SupplierFilter.SelectedValuePath = "ID";  
            SupplierFilter.SelectedIndex = -1; 
        }


        private void LoadConditionsForFilter()
        {
            List<string> conditions = new List<string>
    {
        "Отличное", "Хорошее", "Удовлетворительное", "Устаревшее"
    };
            ConditionFilter.ItemsSource = conditions;
            ConditionFilter.SelectedIndex = -1; 
        }

        private void FilterProducts(object sender, RoutedEventArgs e)
        {
            string selectedCategory = CategoryFilter.SelectedItem?.ToString();
            string selectedCondition = ConditionFilter.SelectedItem?.ToString();
            Supplier selectedSupplier = (Supplier)SupplierFilter.SelectedItem;

            var products = dataAccess.GetProducts();

            var filteredProducts = products.Where(p =>
                (string.IsNullOrEmpty(selectedCategory) || p.Category == selectedCategory) &&
                (string.IsNullOrEmpty(selectedCondition) || p.Condition == selectedCondition) &&
                (selectedSupplier == null || p.Supplier.ID == selectedSupplier.ID)
            ).ToList();

            ProductsGrid.ItemsSource = filteredProducts;
        }


      
        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProducts(sender, e);  
        }

        private void SupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProducts(sender, e);  
        }

        private void ConditionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProducts(sender, e);  
        }




        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            CategoryFilter.SelectedIndex = -1;
            SupplierFilter.SelectedIndex = -1;
            ConditionFilter.SelectedIndex = -1;

  
            ProductsGrid.ItemsSource = dataAccess.GetProducts();
        }

        private void AddWarehouse(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWarehouseWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadWarehouse(); 
            }
        }

        private void EditWarehouse(object sender, RoutedEventArgs e)
        {
            if (WarehouseGrid.SelectedItem is Warehouse selectedWarehouse)
            {
                var editWindow = new AddEditWarehouseWindow(selectedWarehouse); 
                if (editWindow.ShowDialog() == true)
                {
                    LoadWarehouse(); 
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите склад для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteWarehouse(object sender, RoutedEventArgs e)
        {
            Warehouse selectedWarehouse = (Warehouse)WarehouseGrid.SelectedItem;
            if (selectedWarehouse != null)
            {
                dataAccess.DeleteWarehouse(selectedWarehouse.ID);
                LoadWarehouse(); 
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTab)
            {
                Color targetColor = Colors.White; 

               
                switch (selectedTab.Header.ToString())
                {
                    case "Поставщики":
                        targetColor = Color.FromArgb(255, 0, 191, 255); 
                        break;

                    case "Товары":
                        targetColor = Color.FromArgb(255, 216, 191, 216); 
                        break;

                    case "Склад":
                        targetColor = Color.FromArgb(255, 144, 238, 144); 
                        break;

                    case "Покупатели":
                        targetColor = Color.FromArgb(255, 255, 223, 186); 
                        break;

                    case "Заказы":
                        targetColor = Color.FromArgb(255, 173, 216, 230); 
                        break;

                    case "Детали заказов":
                        targetColor = Color.FromArgb(255, 240, 128, 128); 
                        break;

                    default:
                        targetColor = Colors.White; 
                        break;
                }

                
                var colorAnimation = new System.Windows.Media.Animation.ColorAnimation
                {
                    From = ((SolidColorBrush)this.Background).Color,
                    To = targetColor,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500))
                };

               
                var brush = new SolidColorBrush(((SolidColorBrush)this.Background).Color);
                brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                this.Background = brush;
            }
        }


        private void FilterOrdersByDate(object sender, RoutedEventArgs e)
        {
            var dateRangeWindow = new DateRangeWindow();
            if (dateRangeWindow.ShowDialog() == true)
            {
                DateTime startDate = dateRangeWindow.StartDate;
                DateTime endDate = dateRangeWindow.EndDate;

                var filteredOrders = dataAccess.GetOrders()
                    .Where(order => order.OrderDate >= startDate && order.OrderDate <= endDate)
                    .ToList();

                OrdersGrid.ItemsSource = filteredOrders; 
            }
        }



        private void ResetOrdersFilter(object sender, RoutedEventArgs e)
        {
            LoadOrders(); 
        }



        private void ExportFilteredOrderDetailsToExcel(object sender, RoutedEventArgs e)
        {
            int.TryParse(OrderFilterTextBox.Text, out int filteredOrderId);

            var orderDetails = dataAccess.GetAllOrderDetails();
            var orders = dataAccess.GetOrders();

            if (filteredOrderId > 0)
            {
                orderDetails = orderDetails.Where(detail => detail.OrderID == filteredOrderId).ToList();
                orders = orders.Where(order => order.OrderID == filteredOrderId).ToList();
            }

            if (orderDetails == null || !orderDetails.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal totalOrderPrice = orderDetails.Sum(detail => detail.Price);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Детали заказов");

                worksheet.Cells[1, 1].Value = "ID Заказа:";
                worksheet.Cells[1, 2].Value = filteredOrderId > 0 ? filteredOrderId.ToString() : "Все заказы";

                worksheet.Cells[2, 1].Value = "Дата заказа:";
                worksheet.Cells[2, 2].Value = filteredOrderId > 0
                    ? orders.FirstOrDefault()?.OrderDate.ToString("yyyy-MM-dd") ?? "—"
                    : "Разные";

                worksheet.Cells[3, 1].Value = "Покупатель:";
                worksheet.Cells[3, 2].Value = filteredOrderId > 0
                    ? orders.FirstOrDefault()?.CustomerFullName ?? "—"
                    : "Разные";

                worksheet.Cells[5, 1].Value = "Товар";
                worksheet.Cells[5, 2].Value = "Количество";
                worksheet.Cells[5, 3].Value = "Стоимость";

                int row = 6;
                foreach (var detail in orderDetails)
                {
                    worksheet.Cells[row, 1].Value = detail.ProductName;
                    worksheet.Cells[row, 2].Value = detail.Quantity;
                    worksheet.Cells[row, 3].Value = detail.Price; 
                    row++;
                }

                worksheet.Cells[row, 2].Value = "Общая стоимость:";
                worksheet.Cells[row, 3].Value = totalOrderPrice;
                worksheet.Cells[row, 2, row, 3].Style.Font.Bold = true;

                worksheet.Cells.AutoFitColumns();

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = filteredOrderId > 0
                        ? $"Детали заказа_{filteredOrderId}.xlsx"
                        : "Детали заказов.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    package.SaveAs(new System.IO.FileInfo(saveFileDialog.FileName));
                    MessageBox.Show("Данные успешно экспортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }








        private void SalesGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                var column = (DataGridTextColumn)e.Column;
                column.Binding = new Binding(e.PropertyName) { StringFormat = "yyyy-MM-dd" };
            }
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            var addOrderWindow = new AddEditOrderWindow();
            if (addOrderWindow.ShowDialog() == true)
            {
                LoadOrders(); 
                LoadWarehouse();
                LoadAllOrderDetails();

            }
        }
        private void UpdateOrder(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Order selectedOrder)
            {
                var editOrderWindow = new AddEditOrderWindow(selectedOrder);

                if (editOrderWindow.ShowDialog() == true)
                {
                    LoadOrders();
                    LoadAllOrderDetails();
                    LoadWarehouse();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Order selectedOrder)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var orderDetails = dataAccess.GetOrderDetails(selectedOrder.OrderID);
                    foreach (var detail in orderDetails)
                    {
                        dataAccess.UpdateWarehouseQuantity(detail.ProductID, detail.Quantity); 
                    }
                    dataAccess.DeleteOrder(selectedOrder.OrderID);
                    LoadOrders(); 
                    LoadWarehouse();
                    LoadAllOrderDetails();
                }
            }
        }
        private void FilterOrderDetails(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(OrderFilterTextBox.Text, out int orderId))
            {
                var filteredDetails = dataAccess.GetAllOrderDetails().Where(detail => detail.OrderID == orderId).ToList();
                OrderDetailsGrid.ItemsSource = filteredDetails;
            }
            else
            {
                MessageBox.Show("Введите корректный номер заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetOrderFilter(object sender, RoutedEventArgs e)
        {
            OrderFilterTextBox.Text = string.Empty;
            LoadAllOrderDetails(); 
        }








    }
}
