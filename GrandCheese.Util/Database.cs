using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    public class Database
    {
        public static IDbConnection Get()
        {
            var connection = new NpgsqlConnection("User ID=postgres;Password=root;Host=localhost;Port=5432;Database=gc;Pooling=true;");

            connection.Open();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return connection;
        }
    }
}
