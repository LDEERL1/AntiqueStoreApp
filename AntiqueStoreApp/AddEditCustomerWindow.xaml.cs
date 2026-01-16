using System;
using System.Windows;
using AntiqueStoreApp.Models;

namespace AntiqueStoreApp
{
    public partial class AddEditCustomerWindow : Window
    {
        private DataAccess dataAccess = new DataAccess();
        private Customer currentCustomer;

        public AddEditCustomerWindow(Customer customer = null)
        {
            InitializeComponent();
            if (customer != null)
            {
                currentCustomer = customer;
                FullNameTextBox.Text = customer.FullName;
                ContactDetailsTextBox.Text = customer.ContactDetails;
            }
        }

        private void SaveCustomer(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) || string.IsNullOrWhiteSpace(ContactDetailsTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentCustomer == null)
            {
                dataAccess.AddCustomer(FullNameTextBox.Text, ContactDetailsTextBox.Text);
            }
            else
            {
                currentCustomer.FullName = FullNameTextBox.Text;
                currentCustomer.ContactDetails = ContactDetailsTextBox.Text;
                dataAccess.UpdateCustomer(currentCustomer);
            }

            this.DialogResult = true; 
            this.Close();
        }

    }
}
