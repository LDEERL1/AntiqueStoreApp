using AntiqueStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AntiqueStoreApp
{
    /// <summary>
    /// Логика взаимодействия для AddEditSupplierWindow.xaml
    /// </summary>
    public partial class AddEditSupplierWindow : Window
    {
        private DataAccess dataAccess = new DataAccess();
        private Supplier currentSupplier;

        public AddEditSupplierWindow(Supplier supplier = null)
        {
            InitializeComponent();
            if (supplier != null)
            {
                currentSupplier = supplier;
                NameTextBox.Text = supplier.Name;
                ContactDetailsTextBox.Text = supplier.ContactDetails;
            }
        }

        private void SaveSupplier(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(ContactDetailsTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentSupplier == null)
            {
                dataAccess.AddSupplier(NameTextBox.Text, ContactDetailsTextBox.Text);
            }
            else
            {
               
                currentSupplier.Name = NameTextBox.Text;
                currentSupplier.ContactDetails = ContactDetailsTextBox.Text;
                dataAccess.UpdateSupplier(currentSupplier);
            }

            this.DialogResult = true; 
            this.Close();
        }
        

    }
}
