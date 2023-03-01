using LesEgaisParser.Database;
using LesEgaisParser.Mapping;
using System;
using System.Configuration;

namespace LesEgaisParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var numbersOfDeals = int.Parse(ConfigurationManager.AppSettings["NumberOfDealsPerRequest"]);
            var requestDelay = int.Parse(ConfigurationManager.AppSettings["RequestDelay"]);

            Console.WriteLine("Starting HttpClient...");
            var httpClient = new HttpClientAdapter(numbersOfDeals, requestDelay);
            var dealsTotal = httpClient.RequestTotalNumberOfDeals();
            var pages = (int)Math.Ceiling((decimal)dealsTotal / numbersOfDeals);
            Console.WriteLine("Deals to process: " + dealsTotal);
            Console.WriteLine("Pages to process: " + pages);

            Console.WriteLine("Processing page 0...");
            var page0content = httpClient.RequestSpecialReportWoodDeal(0);
            var mapper = new WoodDealMapper();
            var content = mapper.GetWoodDeals(page0content);
            Console.WriteLine("Data serialized...");

            Console.WriteLine("Connecting to db...");
            var dbWorker = new DatabaseWorker(connectionString);
            Console.WriteLine("Attempting to upsert data...");
            dbWorker.UpsertWoodDeals(content);
            Console.WriteLine("Success!");
            Console.ReadLine();
        }
    }
}
