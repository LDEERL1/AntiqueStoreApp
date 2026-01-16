using System;
using System.Collections.Generic;
using System.Windows;
using AntiqueStoreApp.Models;

namespace AntiqueStoreApp
{
    public partial class AddEditWarehouseWindow : Window
    {
        private DataAccess dataAccess = new DataAccess();
        private Warehouse currentWarehouse;

        // Конструктор для добавления или редактирования склада
        // Конструктор для добавления или редактирования склада
        public AddEditWarehouseWindow(Warehouse warehouse = null)
        {
            InitializeComponent();

            // Загружаем список продуктов
            var products = dataAccess.GetProducts();
            ProductComboBox.ItemsSource = products;
            ProductComboBox.DisplayMemberPath = "Name";  // Название продукта будет отображаться
            ProductComboBox.SelectedValuePath = "ID";  // Используем ID продукта

            // Загружаем список мест
            var locations = new List<string> { "Склад 1", "Склад 2", "Склад 3" }; // Пример, замените на ваши реальные данные
            LocationComboBox.ItemsSource = locations;

            // Загружаем статусы
            var statuses = new List<string> { "В наличии", "Заказан", "Продан" }; // Пример статусов
            StatusComboBox.ItemsSource = statuses;

            // Если передан объект склада, заполняем поля для редактирования
            if (warehouse != null)
            {
                currentWarehouse = warehouse;
                ProductComboBox.SelectedValue = warehouse.ProductID;
                QuantityTextBox.Text = warehouse.Quantity.ToString();
                LocationComboBox.SelectedItem = warehouse.Location;
                ArrivalDatePicker.SelectedDate = warehouse.ArrivalDate;
                StatusComboBox.SelectedItem = warehouse.Status;
                OrderedDatePicker.SelectedDate = warehouse.OrderedDate;
            }
            else
            {
                currentWarehouse = new Warehouse(); // Для нового объекта
            }
        }



        // Сохранение изменений
        private void SaveWarehouse(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedValue == null || string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                LocationComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(StatusComboBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Количество должно быть неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            currentWarehouse.ProductID = (int)ProductComboBox.SelectedValue;
            currentWarehouse.Quantity = quantity;
            currentWarehouse.Location = LocationComboBox.SelectedItem.ToString();
            currentWarehouse.ArrivalDate = ArrivalDatePicker.SelectedDate;
            currentWarehouse.Status = StatusComboBox.SelectedItem.ToString();
            currentWarehouse.OrderedDate = OrderedDatePicker.SelectedDate;

            if (currentWarehouse.ID == 0)
            {
                dataAccess.AddWarehouse(currentWarehouse.ProductID, currentWarehouse.Quantity, currentWarehouse.Location, currentWarehouse.ArrivalDate, currentWarehouse.Status, currentWarehouse.OrderedDate);
            }
            else
            {
                dataAccess.UpdateWarehouse(currentWarehouse);
            }

            this.DialogResult = true;
            this.Close();
        }


    }
}
