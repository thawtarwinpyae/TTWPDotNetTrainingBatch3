using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class SaleService
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

            string query = @"SELECT [SaleId]
      ,[ProductId]
      ,[Quantity]
      ,[Price]
      ,[DeleteFlag]
      ,[CreatedDateTime]
  FROM [dbo].[Tbl_Sale]";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            for (int i = 0; i<dt.Rows.Count; i++)
            {
                int saleNo = i+1;
                DataRow row = dt.Rows[i];
                int qty = Convert.ToInt32(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["Price"]);

                Console.WriteLine("Sale N0. " + saleNo.ToString()+"\n Product Id: "+ row["ProductId"].ToString() + "\n Quantity: " + qty.ToString() + "\n Price: " + price.ToString());
            }
            
            
        }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            connection.Open();

            int qty = 30;
            int productId = 7;

            string query = @"INSERT INTO [dbo].[Tbl_Sale]
           ([ProductId]
           ,[Quantity]
           ,[Price]
           ,[DeleteFlag]
           ,[CreatedDateTime])     
(SELECT product.ProductId, @qty,product.Price,0,getDate()
FROM Tbl_Product product
WHERE product.ProductId = @productId
and product.Quantity >= @qty)";


            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@productId", productId);

            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result>0 ? "Successfully Saved!" : "Not Saved!";

            Console.WriteLine(message);
        }


    }
}
