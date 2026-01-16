using System;
using System.Windows;

namespace AntiqueStoreApp
{
    public partial class DateRangeWindow : Window
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateRangeWindow()
        {
            InitializeComponent();
        }

        private void ApplyDateFilter(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                StartDate = StartDatePicker.SelectedDate.Value;
                EndDate = EndDatePicker.SelectedDate.Value;

                if (StartDate > EndDate)
                {
                    MessageBox.Show("Начальная дата не может быть больше конечной.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите обе даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
