﻿using LesEgaisParser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace LesEgaisParser.Database
{
    public class DatabaseWorker
    {
        private readonly string _connectionString;

        public DatabaseWorker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void TestConnectionToDb()
        {
            const string sqlExpression = "select * from WoodDeals";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not connect to DB!");
                    Console.WriteLine(ex.Message);
                    throw;
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
                        InsertParameters(command, deal);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    throw;
                }                
            }
        }

        private void InsertParameters(SqlCommand command, WoodDeal deal)
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
                Value = deal.SellerInn ?? SqlString.Null
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
                Value = deal.BuyerInn ?? SqlString.Null
            };
            command.Parameters.Add(buyerInnParam);

            var dealDateParam = new SqlParameter
            {
                ParameterName = "@dealDate",
                SqlDbType = SqlDbType.Date,
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
