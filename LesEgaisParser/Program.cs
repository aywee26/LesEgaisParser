using LesEgaisParser.Mapping;
using System;

namespace LesEgaisParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClientDemo();
            Console.WriteLine($"Total number of deals: {httpClient.RequestTotalNumberOfDeals()}");
            var dealsDto = httpClient.RequestSpecialReportWoodDeal(page: 1);
            var mapper = new WoodDealMapper();
            var deals = mapper.GetWoodDeals(dealsDto);
            Console.WriteLine("Example of deserialized wood deal:");
            Console.WriteLine("Deal number: " + deals[0].DealNumber);
            Console.WriteLine("Seller name: " + deals[0].SellerName);
            Console.WriteLine("Seller INN: " + deals[0].SellerInn);
            Console.WriteLine("How much was sold: " + deals[0].WoodVolumeSeller);
            Console.ReadLine();
        }
    }
}
