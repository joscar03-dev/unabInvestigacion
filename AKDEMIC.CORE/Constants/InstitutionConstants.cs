using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants
{
    public static class InstitutionConstants
    {
        public const int UNAB = 13;


        public static Dictionary<int, string> Abbreviations = new Dictionary<int, string>()
            {
                { UNAB, "UNAB" },
            };

        public static Dictionary<int, string> Locations = new Dictionary<int, string>()
            {
                { UNAB, "Barranca" },
            };

        public static Dictionary<int, string> Codes = new Dictionary<int, string>()
            {
                { UNAB, "" },
            };

        public static Dictionary<int, string> Names = new Dictionary<int, string>()
            {
                { UNAB, "Universidad Nacional de Barranca" },
            };
      

        public static Dictionary<int, string> SupportEmail = new Dictionary<int, string>()
            {
                { UNAB, "test@testtest.pe" },

            };

        public static Dictionary<int, string> SupportEmailName = new Dictionary<int, string>()
            {
                { UNAB, "test" },
            };

        public static Dictionary<int, string> SupportEmailPassword = new Dictionary<int, string>()
            {
                { UNAB, "Test.2020" },
            };

        public static Dictionary<int, int> StorageMode = new Dictionary<int, int>()
            {
                { UNAB, FileStorageConstants.Mode.SERVER_STORAGE_MODE },

            };


        public static Dictionary<int, string> Path = new Dictionary<int, string>()
            {
                { UNAB, "/usr/share/nginx/html/common/sigau" },
            };

        public static Dictionary<int, string> Values = new Dictionary<int, string>()
            {
                { UNAB, "unab"},
            };
    }
}
