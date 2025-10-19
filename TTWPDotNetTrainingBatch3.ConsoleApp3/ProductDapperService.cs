using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class ProductDapperService
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
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString)) 
            {
                db.Open();

                string query = @"SELECT [ProductId]
      ,[ProductName]
      ,[Quantity]
      ,[Price]
      ,[DeleteFlag]
      ,[CreatedDateTime]
      ,[ModifiedDateTime]
  FROM [dbo].[Tbl_Product]";

            List<ProductDto> lst=db.Query<ProductDto>(query).ToList();

                Console.WriteLine("---Product List---");
            for(int i=0;i<lst.Count;i++)
                {
                    int rowNo = i+1;
                    Console.WriteLine(rowNo.ToString()+". "+ lst[i].ProductName +"(" +lst[i].Price+")");
                }
            }
        }
        public void Create()
        {

            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = @"INSERT INTO [dbo].[Tbl_Product]
           ([ProductName]
           ,[Quantity]
           ,[Price]
           ,[DeleteFlag]
           ,[CreatedDateTime]
           ,[ModifiedDateTime])
     VALUES
           ('Dapper Product'
           , 30
           ,3000
           ,0
           ,getdate()
           ,null)";

                int result=db.Execute(query);

                string message = result>0 ? "Successfully Saved!" : "NOT Saved!";

                Console.WriteLine(message);
            }
            
        }
        public void Update()
        { 
            using(IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString)) 
            {
                db.Open();

                string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductName] = 'Update Dapper Product'
      ,[Quantity] =30
      ,[Price] = 30000
      ,[DeleteFlag] = 0
      ,[CreatedDateTime] = '2025-10-19 16:25:33.300'
      ,[ModifiedDateTime] = GETDATE()
 WHERE ProductName = 'Dapper Product'";

                int result = db.Execute(query);
                string message = result>0 ? "Successfully Updated" : "Not Updated";

                Console.WriteLine(message);
            }
        
        }
        public void Delete()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();

                string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductName] = 'Deleted Dapper Product'
	  ,[DeleteFlag] = 1
      ,[ModifiedDateTime] = GETDATE()
 WHERE ProductName = 'Update Dapper Product'";

                int result = db.Execute(query);
                string message = result>0 ? "Deleted!" : "Not Deleted!";

                Console.WriteLine(message); 
            }
        
        }



    }

    public class ProductDto
    { 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

    }
}
