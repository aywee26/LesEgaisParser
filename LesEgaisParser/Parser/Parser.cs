using LesEgaisParser.Database;
using LesEgaisParser.Mapping;
using System;
using System.Threading;

namespace LesEgaisParser.Parser
{
    public class ParserClass
    {
        private readonly string _connectionString;
        private readonly int _numberOfDealsPerPage;
        private readonly TimeSpan _requestDelay;
        private readonly TimeSpan _delayBetweenParsing;
        private readonly DatabaseWorker _dbWorker;
        private readonly EgaisHttpRequester _httpClient;
        private readonly WoodDealMapper _mapper;

        public ParserClass(string connectionString, int numberOfDealsPerPage, TimeSpan requestDelay, TimeSpan delayBetweenParsing)
        {
            _connectionString = connectionString;
            _numberOfDealsPerPage = numberOfDealsPerPage;
            _requestDelay = requestDelay;
            _delayBetweenParsing = delayBetweenParsing;

            _dbWorker = new DatabaseWorker(_connectionString);
            _httpClient = new EgaisHttpRequester(_numberOfDealsPerPage, _requestDelay);
            _mapper = new WoodDealMapper();
        }

        public void LaunchParsing()
        {
            do
            {
                PerformParsing();
                Thread.Sleep(_delayBetweenParsing);
            }
            while (true);
        }

        private void PerformParsing()
        {
            Console.WriteLine("Testing connection to DB...");
            _dbWorker.TestConnectionToDb();

            Console.WriteLine("Requesting number of deals to process...");
            var dealsTotal = _httpClient.RequestTotalNumberOfDeals();
            var pages = (int)Math.Ceiling((decimal)dealsTotal / _numberOfDealsPerPage);
            Console.WriteLine("Deals to process: " + dealsTotal);
            Console.WriteLine("Pages to process: " + pages);
            Console.WriteLine();

            for (int i = 0; i < pages; i++)
            {
                Console.WriteLine($"Processing page {i}...");
                var pageContent = _httpClient.RequestSpecialReportWoodDeal(i);

                if (pageContent == null)
                {
                    Console.WriteLine("Nothing to deserialize!");
                    Console.WriteLine($"Page {i} skipped!");
                    continue;
                }

                Console.WriteLine("Deserializing data...");
                var deserializedContent = _mapper.GetWoodDeals(pageContent);

                Console.WriteLine("Attempting to upsert data...");
                _dbWorker.UpsertWoodDeals(deserializedContent);

                Console.WriteLine("Success!\n");
            }
        }


    }
}
