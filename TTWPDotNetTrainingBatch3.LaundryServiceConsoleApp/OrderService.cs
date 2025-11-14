using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class OrderService
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

            string query = @"SELECT od.OrderId, c.CustomerName AS CustomerName, s.ServiceName, od.Weight, o.OrderDate, o.Status
                         FROM [dbo].[Tbl_OrderDetail] od
                         INNER JOIN [dbo].[Tbl_Order] o ON od.OrderId = o.OrderId
                         INNER JOIN [dbo].[Tbl_Customer] c ON o.CustomerId=c.CustomerId
                         INNER JOIN [dbo].[Tbl_Service] s ON od.ServiceId = s.ServiceId;";


            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            Console.WriteLine("OrderId | CustomerName | ServiceName | Weight | OrderDate | Status");
            Console.WriteLine("---------------------------------------------------------------");



            for (int i = 0; i<dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                Console.WriteLine($"{row["OrderId"]} | {row["CustomerName"]} | {row["ServiceName"]} | {row["Weight"]}kg | {row["OrderDate"]} | {row["Status"]}");


            }

        }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter customer id: ");
            int customerId = Convert.ToInt32(Console.ReadLine());


            string checkCusQuery = "SELECT COUNT(*) FROM [dbo].[Tbl_Customer] WHERE CustomerId=@CustomerId";

            connection.Open();
            SqlCommand cmd = new SqlCommand(checkCusQuery, connection);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            int cusCount = (int)cmd.ExecuteScalar();
            connection.Close();

            if(cusCount<0)
            {
                Console.WriteLine("Customer does not exist!");
                return;
            }

            Console.Write("Choose Laundry Service Type:1 for Washing,2 for Ironing, 3 for Dry Cleaning : ");
            int serviceType = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter weight in Kg: ");
            int weight = Convert.ToInt32(Console.ReadLine());

            string query = @"INSERT INTO [dbo].[Tbl_Order]
           ([CustomerId]
           ,[OrderDate]
           ,[Status]
           ,[CreatedDateTime])
           OUTPUT INSERTED.OrderId
     VALUES
           (@CustomerId
           ,GETDATE()
           ,@status
           ,GETDATE());";

            string queryOrderDetail = @"INSERT INTO 
            [dbo].[Tbl_OrderDetail] (OrderId, ServiceId, Weight,SubTotal,DeleteFlag,CreatedDateTime)
VALUES (@orderId, @serviceId, @weight, @weight*(SELECT PricePerKg FROM [dbo].[Tbl_Service] WHERE [dbo].[Tbl_Service].ServiceId = @serviceId),0,getdate())";


            connection.Open();

            SqlCommand cmdOrder = new SqlCommand(query, connection);
            cmdOrder.Parameters.AddWithValue("@CustomerId", customerId);
            cmdOrder.Parameters.AddWithValue("@status", OrderStatus.Pending);
            int result = cmdOrder.ExecuteNonQuery();
            int newId = Convert.ToInt32(cmdOrder.ExecuteScalar());

            connection.Close();

            string msg = result > 0 ? "Order added successfully!" : "Order not added!";
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your Order Number is " + newId);
            Console.ResetColor();

            connection.Open();
            SqlCommand cmdOrderDetail = new SqlCommand(queryOrderDetail, connection);

            cmdOrderDetail.Parameters.AddWithValue("@orderId", newId);
            cmdOrderDetail.Parameters.AddWithValue("@serviceId", serviceType);
            cmdOrderDetail.Parameters.AddWithValue("@weight", weight);

            cmdOrderDetail.ExecuteNonQuery();
            connection.Close();


        }
        public void Update()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            Console.Write("Enter Order ID to update: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter new Status (1=Pending, 2=Processing, 3=Completed, 4=Canceled): ");
            int statusChoice = int.Parse(Console.ReadLine());
            OrderStatus newOrderStatus = (OrderStatus)statusChoice;
            string newStatus = newOrderStatus.ToString();

            Console.Write("Enter new Weight (kg): ");
            int newWeight = int.Parse(Console.ReadLine());

            string checkOrderQuery = "SELECT COUNT(*) FROM [dbo].[Tbl_Order] WHERE OrderId=@OrderId";

            connection.Open();
            SqlCommand cmdCheck = new SqlCommand(checkOrderQuery, connection);
            cmdCheck.Parameters.AddWithValue("@OrderId", id);

            int orderCount = (int)cmdCheck.ExecuteScalar();
            connection.Close();

            if (orderCount<=0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Order Id does not exist!");
                Console.ResetColor();
                return;
            }

            string queryServiceId = "SELECT ServiceId FROM [dbo].[Tbl_OrderDetail] WHERE OrderId=@OrderId";

            connection.Open();
            SqlCommand cmdServiceId = new SqlCommand(queryServiceId, connection);
            cmdServiceId.Parameters.AddWithValue("@OrderId", id);

            int serviceId = (int)cmdServiceId.ExecuteScalar();
            connection.Close();

            string query = @"UPDATE [dbo].[Tbl_Order]
   SET [Status] = @newStatus
      ,[ModifiedDateTime] = Getdate()
 WHERE OrderId = @id";

            string queryStatus = @"UPDATE [dbo].[Tbl_OrderDetail]
   SET [Weight] = @newWeight
        ,[SubTotal] = @newWeight*(SELECT PricePerKg FROM [dbo].[Tbl_Service] WHERE [dbo].[Tbl_Service].ServiceId = @serviceId)
      ,[ModifiedDateTime] = Getdate()
 WHERE OrderId = @id";

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@newStatus", newStatus);
            cmd.Parameters.AddWithValue("@id", id);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            connection.Open();
            SqlCommand cmdStatus = new SqlCommand(queryStatus, connection);
            cmdStatus.Parameters.AddWithValue("@newWeight", newWeight);
            cmdStatus.Parameters.AddWithValue("@id", id);
            cmdStatus.Parameters.AddWithValue("@serviceId", serviceId);

            int resultStatus = cmdStatus.ExecuteNonQuery();
            connection.Close();

            Console.ForegroundColor = ConsoleColor.Yellow;

            if (result >0 || resultStatus>0)
            {
                Console.WriteLine("Order Id \""+id+"\"  is Updated!");
            }
            else { Console.WriteLine("Update Failed!"); }
            Console.ResetColor();

            if(newStatus == OrderStatus.Canceled.ToString())
            {
                string flagQuery = "UPDATE [dbo].[Tbl_OrderDetail] SET DeleteFlag=1,[ModifiedDateTime] = Getdate() WHERE OrderId=@id";
                connection.Open();


                SqlCommand cmdFlagQuery = new SqlCommand(flagQuery, connection);
                cmdFlagQuery.Parameters.AddWithValue("@id", id);

                cmdFlagQuery.ExecuteNonQuery();
                connection.Close();

            }
        }

        public void CheckCancel() 
        { 

        }
        public void Delete()
        {
            
        }
    }

    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Canceled = 4
    }
}
