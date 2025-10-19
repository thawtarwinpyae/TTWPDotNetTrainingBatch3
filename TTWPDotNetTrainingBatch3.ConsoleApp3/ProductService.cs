using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class ProductService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "MiniPOS",
            UserID="sa",
            Password="sasa@123",
            TrustServerCertificate = true,
        };

        public void Read() 
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            string query = @"SELECT [ProductId]
      ,[ProductName]
      ,[Quantity]
      ,[Price]
      ,[DeleteFlag]
      ,[CreatedDateTime]
      ,[ModifiedDateTime]
  FROM [dbo].[Tbl_Product]";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            Console.WriteLine("---Product List---");

            for (int i=0; i<dt.Rows.Count; i++) 
            {
                DataRow row = dt.Rows[i];

                int rowNo = i+1;
                Console.WriteLine(rowNo.ToString() +"."+ row["ProductName"]+"("+ row["Price"] +")");
            }
        }
        public void Create() 
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            string query = @"INSERT INTO [dbo].[Tbl_Product]
           ([ProductName]
           ,[Quantity]
           ,[Price]
           ,[DeleteFlag]
           ,[CreatedDateTime]
           ,[ModifiedDateTime])
     VALUES
           ('Testing Product'
           , 20
           ,2000
           ,0
           ,getdate()
           ,null)";

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Successfully Saved!" : "NOT Saved!";

            Console.WriteLine(message);

        }
        public void Update() 
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductName] = 'Testing Product'
      ,[Quantity] =20
      ,[Price] = 10000
      ,[DeleteFlag] = 0
      ,[CreatedDateTime] = '2025-10-19 11:34:18.727'
      ,[ModifiedDateTime] = GETDATE()
 WHERE ProductName = 'Testing Product'";

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Updated!" : "Not Updated!";

            Console.WriteLine(message);

        }
        public void Delete() {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductName] = 'Deleted Testing Product'
	  ,[DeleteFlag] = 1
      ,[ModifiedDateTime] = GETDATE()
 WHERE ProductName = 'Testing Product'";

            SqlCommand cmd = new SqlCommand(query, connection);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Deleted!" : "Not Deleted!";

            Console.WriteLine(message);
        }

    }
}
