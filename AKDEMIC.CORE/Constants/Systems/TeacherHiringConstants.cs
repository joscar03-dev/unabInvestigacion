using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants.Systems
{
    public static class TeacherHiringConstants
    {
        public static class ApplicantTeacher
        {
            public static class Status
            {
                public const byte PENDING = 1;
                public const byte QUALIFIED = 2;
                public const byte APPROVED = 3;
                public const byte REJECTED = 4;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {PENDING, "Pendiente" },
                    {QUALIFIED, "Calificado" },
                    {APPROVED, "Aprobado" },
                    {REJECTED, "Rechazado" }
                };
            }

            public static class Interview
            {
                public static class Type
                {
                    public const byte PERSONAL_INTERVIEW = 1;
                    public const byte MASTER_CLASS_EVALUATION = 2;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {PERSONAL_INTERVIEW, "Entrevista Personal" },
                        {MASTER_CLASS_EVALUATION, "Evaluación de Clase Magistral" }
                    };
                }
            }
        }

        public static class Convocation
        {
            public static class Type
            {
                public const byte CONTRACT = 1;
                public const int NOMINATION = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {CONTRACT,"Contrato" },
                    {NOMINATION,"Nombramiento" },
                };
            }

            public static class Document
            {
                public static class Type
                {
                    public const byte TO_UPLOAD = 1;
                    public const byte RULES = 2;
                }
            }

            public static class Rubric_Section
            {
                public static class Type
                {
                    public const byte EXTERNAL_EVALUATION = 1;
                    public const byte PERSONAL_INTERVIEW = 2;
                    public const byte MASTER_CLASS_EVALUATION = 3;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {EXTERNAL_EVALUATION,"Evaluación Externa" },
                        {PERSONAL_INTERVIEW, "Entrevista Personal" },
                        {MASTER_CLASS_EVALUATION, "Evaluación de Clase Magistral" }
                    };
                }
            }

            public static class Question
            {
                public static class Type
                {
                    public const byte TEXT_QUESTION = 1;
                    public const int MULTIPLE_SELECTION_QUESTION = 2;
                    public const byte UNIQUE_SELECTION_QUESTION = 3;
                    public const byte ESCALE_SELECTION_QUESTION = 4;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {TEXT_QUESTION,"Pregunta de Texto" },
                        {MULTIPLE_SELECTION_QUESTION,"Pregunta de selección múltiple" },
                        {UNIQUE_SELECTION_QUESTION,"Pregunta de selección única" },
                        {ESCALE_SELECTION_QUESTION,"Pregunta de selección de escala" }
                    };
                }

                public static class StaticType
                {
                    public const byte NONE = 1;
                    public const byte FULLNAME = 2;
                    public const byte DNI = 3;
                    public const byte BIRTH_DATE = 4;
                    public const byte EMAIL = 5;
                    public const byte MOBILE_PHONE = 6;
                    public const byte LANDLINE = 7;
                    public const byte ADDRESS = 8;

                    public static Dictionary<byte, string> VALUES_FORMAT1 = new Dictionary<byte, string>()
                    {
                        {FULLNAME,"Apellidos y Nombres" },
                        {DNI,"DNI N°" },
                        {BIRTH_DATE,"Fecha de Nacimiento" },
                        {EMAIL,"Correo Electrónico" },
                        {MOBILE_PHONE,"Teléfono Móvil" },
                        {LANDLINE,"Teléfono Fijo" },
                        {ADDRESS,"Dirección Domiciliaria" },
                    };
                }
            }
        }
    }
}
