using System;
using System.Windows;
using AntiqueStoreApp.Models;
using System.Collections.Generic;

namespace AntiqueStoreApp
{
    public partial class AddEditProductWindow : Window
    {
        private DataAccess dataAccess = new DataAccess();
        private Product currentProduct;

        public AddEditProductWindow(Product product = null)
        {
            InitializeComponent();
            LoadSuppliers(); 
            LoadYears(); 
            LoadCategories(); 
            LoadConditions(); 

            if (product != null)
            {
                currentProduct = product;
                NameTextBox.Text = product.Name;
                DescriptionTextBox.Text = product.Description;
                PriceTextBox.Text = product.Price.ToString();
                YearComboBox.SelectedItem = product.Year; 
                CategoryComboBox.SelectedItem = product.Category;
                ConditionComboBox.SelectedItem = product.Condition;
                SupplierComboBox.SelectedValue = product.Supplier.ID; 
            }
        }

        private void LoadSuppliers()
        {
            var suppliers = dataAccess.GetSuppliers();
            SupplierComboBox.ItemsSource = suppliers;
        }

        private void LoadYears()
        {
            
            int currentYear = DateTime.Now.Year;
            List<int> years = new List<int>();
            for (int year = 100; year <= currentYear; year++)
            {
                years.Add(year);
            }
            YearComboBox.ItemsSource = years;
        }

        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource = dataAccess.GetCategories();
        }


        private void LoadConditions()
        {
            List<string> conditions = new List<string>
            {
                "Отличное",
                "Хорошее",
                "Удовлетворительное",
                "Устаревшее"
            };
            ConditionComboBox.ItemsSource = conditions;
        }

        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) || YearComboBox.SelectedValue == null ||
                CategoryComboBox.SelectedItem == null || ConditionComboBox.SelectedItem == null || SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть числом больше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(YearComboBox.SelectedValue.ToString(), out int year) || year < 100 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Год должен быть в диапазоне от 100 до текущего года.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentProduct == null)
            {
                Supplier selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;
                int productId = dataAccess.AddProduct(NameTextBox.Text, DescriptionTextBox.Text, price, year, CategoryComboBox.Text.ToString(), ConditionComboBox.Text.ToString(), selectedSupplier);

                Warehouse newWarehouse = new Warehouse
                {
                    ProductID = productId
                };

                AddEditWarehouseWindow warehouseWindow = new AddEditWarehouseWindow(newWarehouse);

                if (warehouseWindow.ShowDialog() == false)
                {
                    dataAccess.DeleteProduct(productId);
                    MessageBox.Show("Запись о продукте была удалена, так как данные о складе не были заполнены.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                currentProduct.Name = NameTextBox.Text;
                currentProduct.Description = DescriptionTextBox.Text;
                currentProduct.Price = price;
                currentProduct.Year = year;
                currentProduct.Category = CategoryComboBox.Text.ToString();
                currentProduct.Condition = ConditionComboBox.Text.ToString();
                currentProduct.Supplier = (Supplier)SupplierComboBox.SelectedItem; 
                dataAccess.UpdateProduct(currentProduct);
            }

            this.DialogResult = true;
            this.Close();
        }

    }
}
