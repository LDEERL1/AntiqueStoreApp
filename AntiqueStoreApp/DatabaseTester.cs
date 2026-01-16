using System;
using System.Data.SqlClient;


namespace AntiqueStoreApp
{
    public class DatabaseTester
    {
        public static void TestConnection()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AntiqueStoreDB"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Соединение с базой данных установлено успешно!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }
        }
    }
}
