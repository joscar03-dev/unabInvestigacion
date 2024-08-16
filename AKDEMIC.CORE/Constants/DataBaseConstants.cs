using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants
{
    public static class DataBaseConstants
    {
        public const int MYSQL = 1;
        public const int SQL = 2;

        public static class ConnectionString
        {
            public static Dictionary<Tuple<int, int>, string> VALUES = new()
            {
                { new Tuple<int, int>(DataBaseConstants.MYSQL, MySql.DEFAULT), MySql.VALUES[MySql.DEFAULT] },
                { new Tuple<int, int>(DataBaseConstants.SQL, Sql.DEFAULT), Sql.VALUES[Sql.DEFAULT] },
            };

            public static class MySql
            {
                public const int DEFAULT = 0;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "MySqlDefaultConnection" },
                    };
            }

            public static class Sql
            {
                public const int DEFAULT = 0;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "SqlDefaultConnection" },
                    };
            }
        }

        public static class Versions
        {
            public static class MySql
            {
                public const int VNULL = 0;
                public const int V5717 = 1;
                public const int V5723 = 2;
                public const int V5726 = 3;
                public const int V8021 = 4;

                public static Dictionary<int, Version> VALUES = new Dictionary<int, Version>()
                    {
                        { VNULL, null },
                        { V5717, new Version(5, 7, 17) },
                        { V5723, new Version(5, 7, 23) },
                        { V5726, new Version(5, 7, 26) },
                        { V8021, new Version(8, 0, 21) },
                    };
            }
        }
    }
}
