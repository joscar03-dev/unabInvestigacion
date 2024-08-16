using System;
using System.Collections.Generic;

namespace AKDEMIC.CORE.Constants
{
    public class ConfigurationConstants
    {
        public static class TEACHERINVESTIGATION
        {
            public const string RULES_ARTICLESCIENTIFIC = "LCEN_Rules_ArticleScientific";//Documento Reglamento Interno
            public const string HAS_SINGLEPOSTULANT_RESTRICTION = "TI_Has_SinglePostulant_Restriction";
            public const string ALLOW_REGISTRATION_REQUEST = "TI_Allow_Registration_Request";
            public const string PUBLICATION_TERMS_AND_CONDITION = "TI_Publication_Terms_And_Condition"; //Texto de terminos y condiciones para publicaciones

            public static Dictionary<string, string> DEFAULT_VALUES = new Dictionary<string, string>()
            {
                { RULES_ARTICLESCIENTIFIC, ""},
                { HAS_SINGLEPOSTULANT_RESTRICTION, "false"},
                { ALLOW_REGISTRATION_REQUEST , "false"},
                { PUBLICATION_TERMS_AND_CONDITION , "Acepto que la información proporcionada es verídica."}
            };
        }
    }
}
