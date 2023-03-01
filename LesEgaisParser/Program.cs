using LesEgaisParser.Database;
using System.Configuration;

namespace LesEgaisParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var dbWorker = new DatabaseWorker(connectionString);
            dbWorker.OutputWoodDealsToConsole();


            //var numbersOfDeals = int.Parse(ConfigurationManager.AppSettings["NumberOfDealsPerRequest"]);
            //var requestDelay = int.Parse(ConfigurationManager.AppSettings["RequestDelay"]);

            //var httpClient = new HttpClientDemo(numbersOfDeals, requestDelay);

            //Console.WriteLine($"Total number of deals: {httpClient.RequestTotalNumberOfDeals()}");

            //var dealsDto = httpClient.RequestSpecialReportWoodDeal(page: 1);
            //var mapper = new WoodDealMapper();
            //var deals = mapper.GetWoodDeals(dealsDto);
            //Console.WriteLine("Example of deserialized wood deal:");
            //Console.WriteLine("Deal number: " + deals[0].DealNumber);
            //Console.WriteLine("Seller name: " + deals[0].SellerName);
            //Console.WriteLine("Seller INN: " + deals[0].SellerInn);
            //Console.WriteLine("How much was sold: " + deals[0].WoodVolumeSeller);
            //Console.ReadLine();
        }
    }
}
