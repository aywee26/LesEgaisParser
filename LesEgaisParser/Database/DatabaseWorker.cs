using System;
using System.Data.SqlClient;

namespace LesEgaisParser.Database
{
    public class DatabaseWorker
    {
        private readonly string _connectionString;

        public DatabaseWorker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void OutputWoodDealsToConsole()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM WoodDeals";
                var command = new SqlCommand(sqlExpression, connection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string dealNumber = reader.GetString(0);
                            string sellerName = reader.GetString(1);
                            string sellerInn = reader.GetString(2);
                            string buyerName = reader.GetString(3);
                            string buyerInn = reader.GetString(4);
                            DateTime dealDate = reader.GetDateTime(5);
                            decimal woodVolumeBuyer = reader.GetDecimal(6);
                            decimal woodVolumeSeller = reader.GetDecimal(7);

                            Console.WriteLine($"{dealNumber}\t{sellerName}\t{sellerInn}\t{buyerName}\t{buyerInn}\t{dealDate}\t{woodVolumeBuyer}\t{woodVolumeSeller}");
                        }
                    }
                }
                
            }
        }
    }
}
