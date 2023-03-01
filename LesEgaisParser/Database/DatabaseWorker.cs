using LesEgaisParser.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

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
                const string sqlExpression = "sp_GetDeals";
                var command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

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


        public void InsertWoodDeal(WoodDeal deal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                const string sqlExpression = "sp_InsertDeal";
                var command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    InsertParameters(ref command, deal);

                    var result = command.ExecuteScalar();
                    transaction.Commit();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Insertion failed!");
                    Console.WriteLine(ex.Message);
                }


            }
        }

        public void InsertWoodDeals(List<WoodDeal> woodDeals)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                const string sqlExpression = "sp_InsertDeal";

                try
                {
                    foreach (var deal in woodDeals)
                    {
                        var command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;
                        InsertParameters(ref command, deal);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Insertion failed!");
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public void UpsertWoodDeals(List<WoodDeal> woodDeals)
        {
            const string sqlExpression = "sp_UpsertDeal";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    foreach (var deal in woodDeals)
                    {
                        var command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;
                        InsertParameters(ref command, deal);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }
        }

        private void InsertParameters(ref SqlCommand command, WoodDeal deal)
        {
            var dealNumberParam = new SqlParameter
            {
                ParameterName = "@dealNumber",
                Value = deal.DealNumber
            };
            command.Parameters.Add(dealNumberParam);

            var sellerNameParam = new SqlParameter
            {
                ParameterName = "@sellerName",
                Value = deal.SellerName
            };
            command.Parameters.Add(sellerNameParam);

            var sellerInnParam = new SqlParameter
            {
                ParameterName = "@sellerInn",
                Value = deal.SellerInn
            };
            command.Parameters.Add(sellerInnParam);

            var buyerNameParam = new SqlParameter
            {
                ParameterName = "@buyerName",
                Value = deal.BuyerName
            };
            command.Parameters.Add(buyerNameParam);

            var buyerInnParam = new SqlParameter
            {
                ParameterName = "@buyerInn",
                Value = deal.BuyerInn
            };
            command.Parameters.Add(buyerInnParam);

            var dealDateParam = new SqlParameter
            {
                ParameterName = "@dealDate",
                Value = deal.DealDate
            };
            command.Parameters.Add(dealDateParam);

            var woodVolumeBuyerParam = new SqlParameter
            {
                ParameterName = "@woodVolumeBuyer",
                Value = deal.WoodVolumeBuyer
            };
            command.Parameters.Add(woodVolumeBuyerParam);

            var woodVolumeSellerParam = new SqlParameter
            {
                ParameterName = "@woodVolumeSeller",
                Value = deal.WoodVolumeSeller

            };
            command.Parameters.Add(woodVolumeSellerParam);
        }
    }
}
