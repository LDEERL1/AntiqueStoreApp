
AntiqueStoreApp

Educational desktop application for managing an antique store inventory.
The project demonstrates working with a relational database, SQL Server, and a WPF client application.

Technologies

C#
WPF
SQL Server
T-SQL
ADO.NET

Database setup
1. Create the database

Open SQL Server Management Studio (SSMS).

Run the SQL script from the folder /Database:

AntiqueStore_CreateSchema.sql

AntiqueStore_TestData.sql

These scripts will:

create the AntiqueStore database

create all required tables and relationships

insert test data

Test data is for demonstration purposes only.

2. Update connection string

The application uses a local SQL Server instance and Windows Authentication.

Open the file:

DataAccess.cs


Find the connection string:

private string connectionString =
    "Server=DESKTOP-F9V8BJ6;Database=AntiqueStore;Integrated Security=True;";


Change Server value to match your SQL Server instance name, for example:

Server=localhost

or

Server=YOUR_PC_NAME\SQLEXPRESS

Running the application

Open the solution in Visual Studio.

Make sure SQL Server is running.

Build the solution.

Run the project.

Project purpose

This project was created for educational purposes and portfolio demonstration.
It is not intended for production use.

Project status

The project is under development.
Further improvements may include UI enhancements and additional business logic.
