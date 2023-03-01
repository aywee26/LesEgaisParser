using LesEgaisParser.Database;
using LesEgaisParser.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace LesEgaisParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var dbWorker = new DatabaseWorker(connectionString);
            Console.WriteLine("Before UpSert attempt:");
            dbWorker.OutputWoodDealsToConsole();

            var woodDealToInsert1 = new WoodDeal
            {
                DealNumber = "0453000000000000000107017409",
                BuyerName = "Псевдо Псевдо Физическое лицо",
                BuyerInn = "",
                SellerName = "ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"ИНТЕРСТРОЙ\"",
                SellerInn = "0107017409",
                DealDate = DateTime.Parse("01.03.2023"),
                WoodVolumeBuyer = 0m,
                WoodVolumeSeller = 0m
            };
            var woodDealToInsert2 = new WoodDeal
            {
                DealNumber = "0029002310013042000107017409",
                BuyerName = "МУНИЦИПАЛЬНОЕ УНИТАРНОЕ ПРЕДПРИЯТИЕ \"КРАСНОДАРСКОЕ ТРАМВАЙНО-ТРОЛЛЕЙБУСНОЕ УПРАВЛЕНИЕ\" МУНИЦИПАЛЬНОГО ОБРАЗОВАНИЯ ГОРОД КРАСНОДАР",
                BuyerInn = "2310013042",
                SellerName = "ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"ИНТЕРСТРОЙ\"",
                SellerInn = "0107017409",
                DealDate = DateTime.Parse("01.03.2023"),
                WoodVolumeBuyer = 0m,
                WoodVolumeSeller = 0m
            };
            var woodDealToInsert3 = new WoodDeal
            {
                DealNumber = "0004024004008184000240804862",
                BuyerName = "ИП Хажипов Евгений Маратович",
                BuyerInn = "024004008184",
                SellerName = "ООО \"Газэнерго ОЙл\"",
                SellerInn = "0240804862",
                DealDate = DateTime.Parse("01.03.2023"),
                WoodVolumeBuyer = 0m,
                WoodVolumeSeller = 0m
            };

            dbWorker.UpsertWoodDeals(new List<WoodDeal> { woodDealToInsert1, woodDealToInsert2, woodDealToInsert3 });

            Console.WriteLine("After UpSert attempt:");
            dbWorker.OutputWoodDealsToConsole();
            Console.ReadLine();




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
