using LesEgaisParser.DataTransferObjects;
using LesEgaisParser.Models;
using System.Collections.Generic;
using System.Linq;

namespace LesEgaisParser.Mapping
{
    public class WoodDealMapper
    {
        public List<WoodDeal> GetWoodDeals(SearchReportWoodDeal dealsDto)
        {
            var woodDeals = new List<WoodDeal>();
            var dealsContent = dealsDto.data.searchReportWoodDeal.content;
            foreach (var deal in dealsContent)
            {
                if (IsDealNumberCorrect(deal.dealNumber)
                    && IsNameCorrect(deal.sellerName)
                    && IsNameCorrect(deal.buyerName)
                    && IsINNCorrect(deal.sellerInn)
                    && IsINNCorrect(deal.buyerInn))
                {
                    var woodDeal = new WoodDeal()
                    {
                        DealNumber = deal.dealNumber,
                        SellerName = deal.sellerName,
                        SellerInn = deal.sellerInn,
                        BuyerName = deal.buyerName,
                        BuyerInn = deal.buyerInn,
                        DealDate = deal.dealDate,
                        WoodVolumeBuyer = deal.woodVolumeBuyer,
                        WoodVolumeSeller = deal.woodVolumeSeller
                    };

                    woodDeals.Add(woodDeal);
                }

            }

            return woodDeals;            
        }

        private bool IsINNCorrect(string inn)
        {
            return (!string.IsNullOrWhiteSpace(inn)) &&(inn.Length == 12) && (inn.All(char.IsDigit));
        }

        private bool IsDealNumberCorrect(string dealNumber)
        {
            return (!string.IsNullOrWhiteSpace(dealNumber)) && (dealNumber.Length == 28) && (dealNumber.All(char.IsDigit));
        }

        private bool IsNameCorrect(string name)
        {
            return (!string.IsNullOrWhiteSpace(name));
        }
    }
}
