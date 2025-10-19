using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.ConsoleApp2
{
    public class ProductService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource=".",
            InitialCatalog="MiniPOS",
            UserID="sa",
            Password="sasa@123",
            TrustServerCertificate=true
        };


        /*sqlConnectionStringBuilder.DataSource="."; // server name
            sqlConnectionStringBuilder.InitialCatalog = "MiniPOS";//database name
            sqlConnectionStringBuilder.UserID = "sa";
            sqlConnectionStringBuilder.Password = "sasa@123";
            sqlConnectionStringBuilder.TrustServerCertificate = true;
        */

        public void Read() 
        { 
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
                decimal price = Convert.ToDecimal(row["Price"]);

                /*Console.WriteLine(row["ProductId"]);
                Console.WriteLine(row["ProductName"]);
                Console.WriteLine(row["Quantity"]);
                Console.WriteLine(row["Price"]);*/
                Console.WriteLine(rowNo.ToString() + ". " +row["ProductName"].ToString()+"("+price.ToString("n0") +")");

            }
        }

        public void Create()
        {
            string query = @"INSERT INTO [dbo].[Tbl_Product]
           ([ProductName]
           ,[Quantity]
           ,[Price]
           ,[DeleteFlag])
     VALUES
           ('Avocado1'
           ,100
           ,1000
           ,0)";

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Successfully Saved!" : "Saving Failed!";
            Console.WriteLine(message);

        }

        public void Update() 
        {
            string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductName] = 'Coconut'
      ,[Quantity] = 50
      ,[Price] = 4500
      ,[DeleteFlag] = 1
 WHERE ProductId =6";

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Updated!" : "Not Updated!";
            Console.WriteLine(message);

        }

        public void Delete() 
        {
            string query = @"DELETE FROM Tbl_Product WHERE ProductId=7";

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Deleted!" : "Not Deleted!";
            Console.WriteLine(message);
        }
    }
}
