using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LesEgaisParser
{
    public class HttpClientDemo
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string _requestUrl = @"https://www.lesegais.ru/open-area/graphql";
        private const string _requestNumberOfDealsBody = @"{""query"":""query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    total\n    number\n    size\n    overallBuyerVolume\n    overallSellerVolume\n    __typename\n  }\n}\n"",""variables"":{""size"":20,""number"":0,""filter"":null},""operationName"":""SearchReportWoodDealCount""}";

        public HttpClientDemo()
        {
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.Add("Referer", @"https://www.lesegais.ru/open-area/deal");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Framework Application");
        }

        public int RequestTotalNumberOfDeals()
        {
            var json = PerformPostRequest(_requestUrl, _requestNumberOfDealsBody);
            var jsonAsObject = JsonSerializer.Deserialize<SearchReportWoodDealCount>(json);
            return jsonAsObject.data.searchReportWoodDeal.total;
        }

        private string PerformPostRequest(string requestUrl, string bodyString)
        {
            using (var body = new StringContent(bodyString, Encoding.UTF8, "application/json"))
            {
                using (var response = _httpClient.PostAsync(_requestUrl, body).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return jsonResponse;
                }
            }
        }
    }
}
