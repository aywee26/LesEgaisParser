using LesEgaisParser.Parser;
using System;
using System.Configuration;

namespace LesEgaisParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var numberOfDeals = int.Parse(ConfigurationManager.AppSettings["NumberOfDealsPerRequest"]);
            var requestDelaySeconds = int.Parse(ConfigurationManager.AppSettings["DelayBetweenRequestsSeconds"]);
            var requestDelay = TimeSpan.FromSeconds(requestDelaySeconds);
            var delayBetweenParsingMinutes = int.Parse(ConfigurationManager.AppSettings["DelayBetweenParsingMinutes"]);
            var delay = TimeSpan.FromMinutes(delayBetweenParsingMinutes);

            var parser = new ParserClass(connectionString, numberOfDeals, requestDelay, delay);
            parser.LaunchParsing();
        }
    }
}
