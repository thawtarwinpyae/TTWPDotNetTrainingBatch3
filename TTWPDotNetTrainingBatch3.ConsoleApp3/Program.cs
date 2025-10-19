// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using TTWPDotNetTrainingBatch3.ConsoleApp3;


Console.WriteLine("Hello, World!");

ProductService productService = new ProductService();

//productService.Read();
//productService.Create();
//productService.Update();
//productService.Delete();

SaleService saleService = new SaleService();
//saleService.Read();
//saleService.Create();

ProductDapperService productDapperService = new ProductDapperService();
//productDapperService.Read();
//productDapperService.Create();
//productDapperService.Update();
//productDapperService.Delete();

SaleDapperService saleDapperService = new SaleDapperService();
//saleDapperService.Read();
//saleDapperService.Create();

ProductEFCoreService productEFCoreService = new ProductEFCoreService();
//productEFCoreService.Read();
//productEFCoreService.Create();
//productEFCoreService.Update();
//productEFCoreService.Delete();

SaleEFCoreService saleEFCoreService = new SaleEFCoreService();
//saleEFCoreService.Read();
saleEFCoreService.Create(10, 2);