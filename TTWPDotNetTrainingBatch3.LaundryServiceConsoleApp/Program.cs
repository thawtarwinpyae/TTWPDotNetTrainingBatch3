// See https://aka.ms/new-console-template for more information
using TTWPDotNetTrainingBatch3.LaundryServiceConsoleApp;


CustomerService customerService = new CustomerService();
LaundryService laundryservice = new LaundryService();
OrderService orderService = new OrderService();
//customerService.Delete();

orderService.Update();