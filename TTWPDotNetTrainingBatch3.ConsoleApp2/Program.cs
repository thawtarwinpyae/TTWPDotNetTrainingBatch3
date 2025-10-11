// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("Hello, World!");

SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

sqlConnectionStringBuilder.DataSource="."; // server name
sqlConnectionStringBuilder.InitialCatalog = "MiniPOS";//database name
sqlConnectionStringBuilder.UserID = "sa";
sqlConnectionStringBuilder.Password = "sasa@123";
sqlConnectionStringBuilder.TrustServerCertificate = true;

SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

connection.Open();

Console.WriteLine("hello");

string query = @"SELECT [ProductId]
      ,[ProductName]
      ,[Quantity]
      ,[Price]
      ,[DeleteFlag]
  FROM [dbo].[Tbl_Product]";

SqlCommand cmd = new SqlCommand(query, connection);
SqlDataAdapter adapter = new SqlDataAdapter(cmd);
DataTable dt = new DataTable();
adapter.Fill(dt);

connection.Close();

Console.WriteLine("---Product List---");

for (int i = 0; i<dt.Rows.Count; i++)
{
    DataRow row = dt.Rows[i];

    int rowNo = i+1;
    decimal price =Convert.ToDecimal(row["Price"]);

    /*Console.WriteLine(row["ProductId"]);
    Console.WriteLine(row["ProductName"]);
    Console.WriteLine(row["Quantity"]);
    Console.WriteLine(row["Price"]);*/
    Console.WriteLine(rowNo.ToString() + ". " +row["ProductName"].ToString()+"("+price.ToString("n0") +")");

}

Console.ReadLine();