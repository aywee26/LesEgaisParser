using LesEgaisParser.Database;
using LesEgaisParser.Mapping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace LesEgaisParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var numbersOfDeals = int.Parse(ConfigurationManager.AppSettings["NumberOfDealsPerRequest"]);
            var requestDelaySeconds = int.Parse(ConfigurationManager.AppSettings["DelayBetweenRequestsSeconds"]);
            var requestDelay = TimeSpan.FromSeconds(requestDelaySeconds);

            Console.WriteLine("Testing connection to DB...");
            var dbWorker = new DatabaseWorker(connectionString);
            dbWorker.TestConnectionToDb();

            Console.WriteLine("Starting HttpClient...");
            var httpClient = new HttpClientAdapter(numbersOfDeals, requestDelay);

            Console.WriteLine("Requesting number of deals to process...");
            var dealsTotal = httpClient.RequestTotalNumberOfDeals();
            var pages = (int)Math.Ceiling((decimal)dealsTotal / numbersOfDeals);
            Console.WriteLine("Deals to process: " + dealsTotal);
            Console.WriteLine("Pages to process: " + pages);
            Console.WriteLine();

            var mapper = new WoodDealMapper();

            var skippedPages = new List<int>();

            for (int i = 0; i < pages; i++)
            {
                Thread.Sleep(requestDelay);
                Console.WriteLine($"Processing page {i}...");
                var pageContent = httpClient.RequestSpecialReportWoodDeal(i);

                if (pageContent == null)
                {
                    Console.WriteLine("Nothing to deserialize!");
                    Console.WriteLine("Page skipped!");
                    skippedPages.Add(i);
                    continue;
                }

                Console.WriteLine("Deserializing data...");
                var deserializedContent = mapper.GetWoodDeals(pageContent);
                
                Console.WriteLine("Attempting to upsert data...");
                dbWorker.UpsertWoodDeals(deserializedContent);

                Console.WriteLine("Success!");
            }

            Console.WriteLine("Parsing is done!");
            Console.WriteLine("Skipped pages: " + string.Join(", ", skippedPages));
            Console.ReadLine();
        }
    }
}
