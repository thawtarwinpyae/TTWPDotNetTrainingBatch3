using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTWPDotNetTrainingBatch3.ConsoleApp3.Database.AppDbContextModels;

namespace TTWPDotNetTrainingBatch3.ConsoleApp3
{
    public class ProductEFCoreService
    {
        public void Read() 
        {
            AppDbContext db = new AppDbContext();
            var product = db.TblProducts.ToList();

            for (int i = 0; i<product.Count; i++)
            {
                int rowNo = i+1;

                Console.WriteLine(rowNo.ToString() + ". "+product[i].ProductName +"("+ product[i].Price+")");
            }


        }
        public void Create() 
        {
            AppDbContext db = new AppDbContext();
            var product = new TblProduct() {
                ProductName = "Guava",
                Quantity=40,
                Price=1000,
                DeleteFlag=false,
                CreatedDateTime=DateTime.Now

            };

            db.TblProducts.Add(product);
            int result = db.SaveChanges();
            string message = result>0 ? "Successfully Saved!" : "Not Saved!";
            Console.WriteLine(message);

        }
        public void Update() 
        {
            AppDbContext db = new AppDbContext();

            var product = db.TblProducts.Where(x => x.ProductId==10).FirstOrDefault();

            if (product is null)
            {
                return;
            }

            product.ProductName="Red Guava";
            product.ModifiedDateTime=DateTime.Now;

            int result = db.SaveChanges();
            string message = result>0 ? "Successfully Saved!" : "Not Saved!";
            Console.WriteLine(message);
        }
        public void Delete() 
        {
            AppDbContext db = new AppDbContext();

            var product = db.TblProducts.Where(x => x.ProductId==10).FirstOrDefault();

            if(product is null)
            {
                return;
            }

            product.DeleteFlag = true;
            product.ModifiedDateTime=DateTime.Now;

            int result=db.SaveChanges();
            string message = result>0 ? "Successfully deleted!" : "Not Deleted!";
            Console.WriteLine(message);
        }

    }
}
