using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTWPDotNetTrainingBatch3.ConsoleApp3.Database.AppDbContextModels;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class SaleEFCoreService
    {
        public void Read() 
        {
            AppDbContext db = new AppDbContext();

            var sale = db.TblSales.ToList();

            Console.WriteLine("---Sale List---");
            for (int i = 0; i< sale.Count; i++)
            {
                int rowNo = i+1;

                Console.WriteLine("Sale No " + rowNo.ToString() + ". Product Id: "+ sale[i].ProductId +" - Qty: " + sale[i].Quantity);
            }
        }
        public void Create(int id, int qty) 
        {
            AppDbContext db = new AppDbContext();

            var product = db.TblProducts.Where(x => x.ProductId==id && x.Quantity>=qty).FirstOrDefault();

            if (product is null)
            {
                return;
            }

            var sale = new TblSale()
            {
                ProductId = product.ProductId,
                Quantity = qty,
                Price = product.Price,
                DeleteFlag = false,
                CreatedDateTime = DateTime.Now
            };

            db.TblSales.Add(sale);
            int result = db.SaveChanges();
            string message = result>0 ? "Successfully Saved!" : "Not Saved!";
            Console.WriteLine(message);           

        }
    }
}

