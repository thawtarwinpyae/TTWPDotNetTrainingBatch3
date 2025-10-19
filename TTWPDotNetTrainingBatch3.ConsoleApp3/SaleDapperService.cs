using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class SaleDapperService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog= "MiniPOS",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate=true,
        };

        public void Read()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();

                string query = @"SELECT [SaleId]
      ,[ProductId]
      ,[Quantity]
      ,[Price]
      ,[DeleteFlag]
      ,[CreatedDateTime]
  FROM [dbo].[Tbl_Sale]";

                List<SaleDto> lst = db.Query<SaleDto>(query).ToList();

                Console.WriteLine("---Sale List---");
                for(int i=0; i<lst.Count; i++) 
                {
                    int rowNo = i+1;
                    Console.WriteLine(rowNo.ToString() + ". " +"ProductId: "+ lst[i].ProductId +" , Qty: " + lst[i].Quantity);
                }

            }
        }
        public void Create()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString)) 
            {
                db.Open();             

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

                int result=db.Execute(query, new {qty=26,productId=8});
                string message = result>0 ? "Successfully Saved!" : "Not Saved!";

                Console.WriteLine(message);

            }

        }
    

    }

    public class SaleDto 
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime CreatedDateTime { get; set; }


    }
}
