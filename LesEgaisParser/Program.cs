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

            Console.WriteLine("Starting HttpClient...");
            var httpClient = new HttpClientAdapter(numbersOfDeals);

            Console.WriteLine("Requesting number of deals to process...");
            var dealsTotal = httpClient.RequestTotalNumberOfDeals();
            var pages = (int)Math.Ceiling((decimal)dealsTotal / numbersOfDeals);
            Console.WriteLine("Deals to process: " + dealsTotal);
            Console.WriteLine("Pages to process: " + pages);
            Console.WriteLine();

            var mapper = new WoodDealMapper();
            var dbWorker = new DatabaseWorker(connectionString);

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
                    Console.WriteLine("Delay between requests increased!\n");
                    requestDelay.Add(TimeSpan.FromSeconds(5));
                    skippedPages.Add(i);
                    continue;
                }

                Console.WriteLine("Deserializing data...");
                var deserializedContent = mapper.GetWoodDeals(pageContent);
                
                Console.WriteLine("Attempting to upsert data...");
                dbWorker.UpsertWoodDeals(deserializedContent);

                Console.WriteLine("Success!");
                Console.WriteLine("Current delay between requests: " + requestDelay + Environment.NewLine);
            }

            Console.WriteLine("Parsing is done!");
            Console.WriteLine("Skipped pages: " + string.Join(", ", skippedPages));
            Console.WriteLine("Delay between requests in seconds: " + requestDelay.TotalSeconds);
            Console.ReadLine();
        }
    }
}
