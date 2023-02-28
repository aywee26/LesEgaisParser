using System;

namespace LesEgaisParser.DataTransferObjects
{
    public class SearchReportWoodDeal
    {
        public Data data { get; set; }

        public class Data
        {
            public Searchreportwooddeal searchReportWoodDeal { get; set; }
        }

        public class Searchreportwooddeal
        {
            public Content[] content { get; set; }
            public string __typename { get; set; }
        }

        public class Content
        {
            public string sellerName { get; set; }
            public string sellerInn { get; set; }
            public string buyerName { get; set; }
            public string buyerInn { get; set; }
            public decimal woodVolumeBuyer { get; set; }
            public decimal woodVolumeSeller { get; set; }
            public DateTime dealDate { get; set; }
            public string dealNumber { get; set; }
            public string __typename { get; set; }
        }

    }
}
