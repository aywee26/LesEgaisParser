using LesEgaisParser.DataTransferObjects;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LesEgaisParser
{
    public class HttpClientDemo
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string _requestUrl = @"https://www.lesegais.ru/open-area/graphql";
        private readonly int _numberOfDealsPerRequest;
        private readonly int _requestDelay;
        
        public HttpClientDemo(int numberOfDealsPerRequest, int requestDelay)
        {
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.Add("Referer", @"https://www.lesegais.ru/open-area/deal");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Framework Application");

            _numberOfDealsPerRequest = numberOfDealsPerRequest;
            _requestDelay = requestDelay;
        }

        public int RequestTotalNumberOfDeals()
        {
            const string requestNumberOfDealsBody = @"{""query"":""query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    total\n    number\n    size\n    overallBuyerVolume\n    overallSellerVolume\n    __typename\n  }\n}\n"",""variables"":{""size"":20,""number"":0,""filter"":null},""operationName"":""SearchReportWoodDealCount""}";
            var json = PerformPostRequest(_requestUrl, requestNumberOfDealsBody);
            var jsonAsObject = JsonSerializer.Deserialize<SearchReportWoodDealCount>(json);
            return jsonAsObject.data.searchReportWoodDeal.total;
        }

        public SearchReportWoodDeal RequestSpecialReportWoodDeal(int page)
        {
            var body = GetBodyForSearchReportWoodDeal(_numberOfDealsPerRequest, page);
            var json = PerformPostRequest(_requestUrl, body);
            var jsonAsObject = JsonSerializer.Deserialize<SearchReportWoodDeal>(json);
            return jsonAsObject;
        }

        private string PerformPostRequest(string requestUrl, string bodyString)
        {
            using (var body = new StringContent(bodyString, Encoding.UTF8, "application/json"))
            {
                using (var response = _httpClient.PostAsync(requestUrl, body).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return jsonResponse;
                }
            }
        }

        private string GetBodyForSearchReportWoodDeal(int size, int page)
        {
            var builder = new StringBuilder();
            builder.Append(@"{""query"":""query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    content {\n      sellerName\n      sellerInn\n      buyerName\n      buyerInn\n      woodVolumeBuyer\n      woodVolumeSeller\n      dealDate\n      dealNumber\n      __typename\n    }\n    __typename\n  }\n}\n"",");
            builder.Append(@"""variables"":{""size"":");
            builder.Append(size);
            builder.Append(@",""number"":");
            builder.Append(page);
            builder.Append(@",""filter"":null,""orders"":null},""operationName"":""SearchReportWoodDeal""}");
            return builder.ToString();
        }
    }
}
