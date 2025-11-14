using LaundryServiceConsoleApp.Database.AppDbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp
{
    public class CustomerEFCoreService
    {
        private readonly AppDbContext db = new AppDbContext();

        public void Read() 
        {
            var customers = db.TblCustomers
                            .Where(c => !c.DeleteFlag)   // ✅ exclude deleted
                            .ToList();
            Console.WriteLine("-----Customer List-----");
            foreach (var c in customers)
            {
                Console.WriteLine($"ID: {c.CustomerId} | Name: {c.CustomerName} | Phone: {c.Phone} | Address: {c.Address}");
            }
        }
        public void Create() 
        {
            Console.Write("Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();

            var customer = new TblCustomer { CustomerName = name, Phone = phone, Address = address, DeleteFlag=false, CreatedDateTime=DateTime.Now };
            db.TblCustomers.Add(customer);
            db.SaveChanges();
            Console.WriteLine($"Customer {customer.CustomerId} added!");
        }
        public void Update()
        {
            Console.Write("Enter CustomerId to update: ");
            int id = int.Parse(Console.ReadLine());

            var customer = db.TblCustomers.FirstOrDefault(c => c.CustomerId == id && !c.DeleteFlag);
            if (customer != null)
            {
                Console.Write("New Customer Name: ");
                customer.CustomerName = Console.ReadLine();
                Console.Write("New Phone: ");
                customer.Phone = Console.ReadLine();
                Console.Write("New Address: ");
                customer.Address = Console.ReadLine();
                customer.ModifiedDateTime=DateTime.Now;

                db.SaveChanges();
                Console.WriteLine("Customer updated!");

            }
            else {
                Console.WriteLine("Customer not found or has been deleted");
            }
        }
        public void Delete() 
        {
            Console.Write("Enter CustomerId to delete: ");
            int id = int.Parse(Console.ReadLine());

            var customer = db.TblCustomers.FirstOrDefault(c => c.CustomerId == id && !c.DeleteFlag);
            if (customer != null)
            {
                customer.DeleteFlag = true;
                customer.ModifiedDateTime=DateTime.Now;
                db.SaveChanges();
                Console.WriteLine("Customer deleted!");
            }
            else
            {
                Console.WriteLine("Customer not found or has been deleted");
            }
        }

    }
}
