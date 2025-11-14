using LaundryServiceConsoleApp.Database.AppDbContext.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class LaundryServiceEFCoreService
    {
        private readonly AppDbContext db = new AppDbContext();

        public void Read()
        {
            var services = db.TblServices
             .Where(s => !s.DeleteFlag)   // ✅ exclude deleted
             .ToList();

            Console.WriteLine("\n--- Active Service List ---");
            foreach (var s in services)
            {
                Console.WriteLine($"ID: {s.ServiceId} | Name: {s.ServiceName} | Price/kg: {s.PricePerKg}");
            }
        }
        public void Create()
        {
            Console.Write("Laundry Service Name: ");
            string name = Console.ReadLine();
            Console.Write("Price per Kg: ");
            decimal price = decimal.Parse(Console.ReadLine());

            var service = new TblService { ServiceName = name, PricePerKg = price, DeleteFlag = false, CreatedDateTime=DateTime.Now };
            db.TblServices.Add(service);
            db.SaveChanges();
            Console.WriteLine($"Service {service.ServiceId} added!");
        }
        public void Update()
        {
            Console.Write("Enter Service Id to update: ");
            int id = int.Parse(Console.ReadLine());

            var service = db.TblServices.FirstOrDefault(s => s.ServiceId == id && !s.DeleteFlag);
            if (service != null)
            {
                Console.Write("New Service Name: ");
                service.ServiceName = Console.ReadLine();
                Console.Write("New Price per Kg: ");
                service.PricePerKg = decimal.Parse(Console.ReadLine());
                service.ModifiedDateTime=DateTime.Now;

                db.SaveChanges();
                Console.WriteLine("Service updated!");
            }
            else
            {
                Console.WriteLine("Service not found or has been deleted.");
            }
        }
        public void Delete()
        {
            Console.Write("Enter ServiceId to delete: ");
            int id = int.Parse(Console.ReadLine());

            var service = db.TblServices.FirstOrDefault(s => s.ServiceId == id && !s.DeleteFlag);
            if (service != null)
            {
                service.DeleteFlag = true;  // ✅ soft delete
                service.ModifiedDateTime=DateTime.Now;
                db.SaveChanges();
                Console.WriteLine("Service deleted!");
            }
            else
            {
                Console.WriteLine("Service not found or already deleted.");
            }
        }

        
    }
}
