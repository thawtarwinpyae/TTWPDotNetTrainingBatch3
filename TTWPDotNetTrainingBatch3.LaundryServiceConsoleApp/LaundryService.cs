using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class LaundryService
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

            string query = @"SELECT [ServiceId]
      ,[ServiceName]
      ,[PricePerKg]
      ,[DeleteFlag]
      ,[CreatedDateTime]
      ,[ModifiedDateTime]
  FROM [dbo].[Tbl_Service] where DeleteFlag=0";


            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            Console.WriteLine("-----Laundry Service List-----");


            for (int i = 0; i<dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];


                Console.WriteLine($"Service ID: {row["ServiceId"]}, Service Name: {row["ServiceName"]}, Price: {row["PricePerKg"]}");

            }

        }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter Service Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Price per Kg: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            string query = @"INSERT INTO [dbo].[Tbl_Service]
           ([ServiceName]
           ,[PricePerKg]
           ,[DeleteFlag]
           ,[CreatedDateTime]
           )
     VALUES
           (@name
           ,@price
           ,0
           ,GETDATE())";

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string msg = result>0 ? "New Service is added!" : "Unable to add new service!";

            Console.WriteLine(msg);
        }
        public void Update()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter Service Id to update: ");
            int id = int.Parse(Console.ReadLine());
            
            Console.Write("Enter New Service Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Price Per Kg: ");
            string price = Console.ReadLine();


            connection.Open();

            string query = "UPDATE [dbo].[Tbl_Service] SET ServiceName=@name, PricePerKg=@price, ModifiedDateTime=GETDATE() WHERE ServiceId=@id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@id", id);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string msg = result>0 ? "Laundry Service Id \""+id+"\"  is Updated!" : "Update Failed!";
            Console.WriteLine(msg);
        }
        public void Delete()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter service ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            string query = @"UPDATE [dbo].[Tbl_Service]
   SET [DeleteFlag] = 1,
	   [ModifiedDateTime] = GETDATE()
 WHERE ServiceId = @id";

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string msg = result>0 ? "Deleted Service Id "+id+"!" : "Unable to delete!";

            Console.WriteLine(msg);
        }

    }
}
