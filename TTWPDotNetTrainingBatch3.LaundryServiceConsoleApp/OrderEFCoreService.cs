using LaundryServiceConsoleApp.Database.AppDbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class OrderEFCoreService
    {
        AppDbContext db = new AppDbContext();

        public void Read()
        {
            var orderDetails = from od in db.TblOrderDetails
                               join o in db.TblOrders on od.OrderId equals o.OrderId
                               join c in db.TblCustomers on o.CustomerId equals c.CustomerId
                               join s in db.TblServices on od.ServiceId equals s.ServiceId
                               where !od.DeleteFlag && !c.DeleteFlag && !s.DeleteFlag   // ✅ exclude soft-deleted
                               select new
                               {
                                   od.OrderId,
                                   CustomerName = c.CustomerName,
                                   ServiceName = s.ServiceName,
                                   od.Weight,
                                   o.OrderDate,
                                   o.Status
                               };

            Console.WriteLine("OrderId | CustomerName | ServiceName | Weight | OrderDate | Status");
            Console.WriteLine("---------------------------------------------------------------");

            foreach (var od in orderDetails)
            {
                Console.WriteLine($"{od.OrderId} | {od.CustomerName} | {od.ServiceName} | {od.Weight}kg | {od.OrderDate} | {od.Status}");

            }
        }
        public void Create()
        {
            Console.Write("Enter customer id: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            // ✅ Check if customer exists and is not deleted
            var customer = db.TblCustomers.FirstOrDefault(c => c.CustomerId == customerId && !c.DeleteFlag);
            if (customer == null)
            {
                Console.WriteLine("Customer does not exist!");
                return;
            }

            Console.Write("Choose Laundry Service Type: 1=Washing, 2=Ironing, 3=Dry Cleaning: ");
            int serviceType = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter weight in Kg: ");
            decimal weight = Convert.ToDecimal(Console.ReadLine());

            // ✅ Create new order
            var order = new TblOrder
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending.ToString(), // store enum as string or int depending on schema
                CreatedDateTime = DateTime.Now,
            };

            db.TblOrders.Add(order);
            db.SaveChanges();   // saves and generates OrderId

            // ✅ Create order detail
            var service = db.TblServices.FirstOrDefault(s => s.ServiceId == serviceType && !s.DeleteFlag);
            if (service == null)
            {
                Console.WriteLine("Service not found!");
                return;
            }

            var orderDetail = new TblOrderDetail
            {
                OrderId = order.OrderId,
                ServiceId = serviceType,
                Weight = weight,
                SubTotal = weight * service.PricePerKg,
                DeleteFlag = false,
                CreatedDateTime = DateTime.Now
            };

            db.TblOrderDetails.Add(orderDetail);
            db.SaveChanges();

            Console.WriteLine("Order added successfully!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your Order Number is " + order.OrderId);
            Console.ResetColor();
        }
        public void Update()
        {
            Console.Write("Enter Order ID to update: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter new Status (1=Pending, 2=Processing, 3=Completed, 4=Canceled): ");
            int statusChoice = int.Parse(Console.ReadLine());
            OrderStatus newOrderStatus = (OrderStatus)statusChoice;
            string newStatus = newOrderStatus.ToString();

            Console.Write("Enter new Weight (kg): ");
            decimal newWeight = Convert.ToDecimal(Console.ReadLine());

            // ✅ LINQ join to check if order exists and fetch details + service
            var query = (from o in db.TblOrders
                         join od in db.TblOrderDetails on o.OrderId equals od.OrderId
                         join s in db.TblServices on od.ServiceId equals s.ServiceId
                         where o.OrderId == id && !od.DeleteFlag && !s.DeleteFlag
                         select new { Order = o, Detail = od, Service = s })
                         .ToList();

            if (!query.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Order Id does not exist!");
                Console.ResetColor();
                return;
            }

            // ✅ Update order status
            var orderEntity = query.First().Order;
            orderEntity.Status = newStatus;
            orderEntity.ModifiedDateTime = DateTime.Now;

            // ✅ Update order details (weight + subtotal)
            foreach (var row in query)
            {
                row.Detail.Weight = newWeight;
                row.Detail.SubTotal = newWeight * row.Service.PricePerKg;
                row.Detail.ModifiedDateTime = DateTime.Now;

                // If canceled → soft delete
                if (newOrderStatus == OrderStatus.Canceled)
                {
                    row.Detail.DeleteFlag = true;
                }
            }

            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Order Id \"{id}\" is Updated!");
            Console.ResetColor();
        }
       

    }
}
