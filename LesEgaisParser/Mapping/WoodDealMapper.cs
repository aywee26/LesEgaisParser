using LesEgaisParser.DataTransferObjects;
using LesEgaisParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LesEgaisParser.Mapping
{
    public class WoodDealMapper
    {
        public List<WoodDeal> GetWoodDeals(SearchReportWoodDeal dealsDto)
        {
            var dealsContent = dealsDto.data.searchReportWoodDeal.content;
            var woodDeals = new List<WoodDeal>(dealsContent.Length);
            foreach (var deal in dealsContent)
            {
                if (IsNameCorrect(deal.sellerName)
                    && IsNameCorrect(deal.buyerName)
                    && IsDateCorrect(deal.dealDate)
                    && IsDealNumberCorrect(deal.dealNumber))
                {
                    var woodDeal = new WoodDeal()
                    {
                        DealNumber = deal.dealNumber,
                        SellerName = deal.sellerName,
                        BuyerName = deal.buyerName,
                        DealDate = (DateTime)deal.dealDate,
                        WoodVolumeBuyer = deal.woodVolumeBuyer,
                        WoodVolumeSeller = deal.woodVolumeSeller
                    };

                    if (IsINNCorrect(deal.buyerInn))
                    {
                        woodDeal.BuyerInn = deal.buyerInn;
                    }
                    else
                    {
                        woodDeal.BuyerInn = null;
                    }

                    if (IsINNCorrect(deal.sellerInn))
                    {
                        woodDeal.SellerInn = deal.sellerInn;
                    }
                    else
                    {
                        woodDeal.SellerInn = null;
                    }

                    woodDeals.Add(woodDeal);
                }
            }

            return woodDeals;            
        }

        private bool IsINNCorrect(string inn)
        {
            if (inn == null)
            {
                return false;
            }

            if (inn == string.Empty)
            {
                return false;
            }

            return IsINNControlSumCorrect(inn);
        }

        private bool IsDealNumberCorrect(string dealNumber)
        {
            if (string.IsNullOrEmpty(dealNumber))
                return false;

            if (dealNumber.Length != 28)
                return false;

            if (!dealNumber.All(char.IsDigit))
                return false;

            return true;
        }

        private bool IsNameCorrect(string name)
        {
            return (!string.IsNullOrWhiteSpace(name));
        }

        private bool IsINNControlSumCorrect(string inn)
        {
            if (inn.Length == 10)
            {
                var controlDigits = new int[] { 2, 4, 10, 3, 5, 9, 4, 6, 8, 0 };
                int sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    var digitFromInn = (int)(char.GetNumericValue(inn[i]));
                    sum += digitFromInn * controlDigits[i];
                }

                var result = sum % 11 % 10;
                var check = (int)(char.GetNumericValue(inn[inn.Length - 1]));
                return result == check;
            }

            if (inn.Length == 12)
            {
                var controlDigits = new int[] { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8, 0 };
                int sum1 = 0;
                int sum2 = 0;
                for (int i = 0; i < 11; i++)
                {
                    var digitFromInn = (int)(char.GetNumericValue(inn[i]));
                    sum1 += digitFromInn * controlDigits[i + 1];
                    sum2 += digitFromInn * controlDigits[i];
                }

                var result1 = sum1 % 11 % 10;
                var check1 = (int)(char.GetNumericValue(inn[inn.Length - 2]));
                var result2 = sum2 % 11 % 10;
                var check2 = (int)(char.GetNumericValue(inn[inn.Length - 1]));

                return (result1 == check1 && result2 == check2);
            }

            return false;
        }

        private bool IsDateCorrect(DateTime? date)
        {
            if (date == null)
            {
                return false;
            }

            return true;
        }
    }
}
