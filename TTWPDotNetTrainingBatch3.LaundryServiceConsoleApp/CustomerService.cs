using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class CustomerService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "LaundryService",
            UserID="sa",
            Password="sasa@123",
            TrustServerCertificate = true,
        };

        public void Read()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            
            connection.Open();

            string query = @"SELECT [CustomerId]
      ,[CustomerName]
      ,[Phone]
      ,[Address]
  FROM [dbo].[Tbl_Customer]";


            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            Console.WriteLine("-----Customer List-----");


            for (int i = 0; i<dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];


                Console.WriteLine($"ID: {row["CustomerID"]}, Name: {row["CustomerName"]}, Phone: {row["Phone"]}, Address: {row["Address"]}");
            
            }

        
        }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter custome name: ");
            string name = Console.ReadLine();
            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            string query = @"INSERT INTO [dbo].[Tbl_Customer]
           ([CustomerName]
           ,[Phone]
           ,[Address])
     VALUES
           (@customerName
           ,@phone
           ,@address
           ,0
           ,GETDATE())";

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@customerName", name);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@address", address);

            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string msg = result > 0 ? "Customer added successfully!" : "Customer not added!";
            Console.WriteLine(msg);

        }
        public void Update()
        {

            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter CustomerID to update: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter New Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter New Address: ");
            string address = Console.ReadLine();

            connection.Open();

            string query = "UPDATE [dbo].[Tbl_Customer] SET CustomerName=@name, Phone=@phone, Address=@address,ModifiedDateTime=GETDATE() WHERE CustomerID=@id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name",name);
            cmd.Parameters.AddWithValue("@phone",phone);
            cmd.Parameters.AddWithValue("@address",address);
            cmd.Parameters.AddWithValue("@id",id);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string msg = result>0 ? "Customer Id \""+id+"\"  is Updated!": "Update Failed!";
            Console.WriteLine(msg);
        
        }
        public void Delete()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter customer ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            string query = @"UPDATE [dbo].[Tbl_Customer]
   SET [DeleteFlag] = 1,
	   [ModifiedDateTime] = GETDATE()
 WHERE CustomerId = @id";

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string msg = result>0 ? "Deleted Customer Id "+id+"!" : "Unable to delete!";

            Console.WriteLine(msg);
        }


    }
}
