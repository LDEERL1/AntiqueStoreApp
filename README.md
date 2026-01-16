# AntiqueStoreApp

Educational desktop application for managing an antique store inventory.

The project demonstrates working with a relational database, Microsoft SQL Server, and a WPF client application.  
It includes basic CRUD operations, database relationships, and interaction with the database using ADO.NET.

---

## Technologies

- C#
- WPF
- Microsoft SQL Server
- T-SQL
- ADO.NET

---

## Database Setup

### 1. Create the database

Open **SQL Server Management Studio (SSMS)** and execute the SQL scripts located in the `/Database` folder in the following order:

1. `AntiqueStore_CreateSchema.sql`
2. `AntiqueStore_TestData.sql`

These scripts will:
- create the `AntiqueStore` database
- create all required tables and relationships
- insert test data for demonstration purposes

> Test data is used only for educational and demonstration purposes.

---

### 2. Update the connection string

The application uses a local SQL Server instance with Windows Authentication.

Open the file:

Find the connection string:

private string connectionString =
    "Server=DESKTOP-F9V8BJ6;Database=AntiqueStore;Integrated Security=True;";

Replace the Server value with your SQL Server instance name, for example:

Server=localhost

or

Server=YOUR_PC_NAME\SQLEXPRESS

Running the Application

    Open the solution in Visual Studio.

    Make sure SQL Server is running.

    Build the solution.

    Run the project.

Project Purpose

This project was created for educational purposes and portfolio demonstration.
It is not intended for production use.
Project Status

The project is under development.
Possible future improvements include UI enhancements and additional business logic.
