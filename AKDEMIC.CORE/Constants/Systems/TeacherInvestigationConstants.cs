using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants.Systems
{
    public static class TeacherInvestigationConstants
    {
        public static class InscriptionForm
        {
            public static class QuestionType
            {
                public const int TEXT_QUESTION = 1;
                public const int MULTIPLE_SELECTION_QUESTION = 2;
                public const int UNIQUE_SELECTION_QUESTION = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { TEXT_QUESTION , "Pregunta de texto" },
                    { MULTIPLE_SELECTION_QUESTION , "Pregunta de selección múltiple" },
                    { UNIQUE_SELECTION_QUESTION , "Pregunta de selección única" },
                };
            }
        }
        public static class Publication
        {
            public static class WorkCategory
            {
                public const int PUBLICATION = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PUBLICATION , "Publicación" },
                };
            }
        }

        public static class Conference
        {
            public static class Type
            {
                public const int NATIONAL = 1;
                public const int INTERNATIONAL = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NATIONAL , "Nacional" },
                    { INTERNATIONAL , "Internacional" },
                };
            }
        }

        public static class Unit
        {
            public const string BUSINESS_INCUBATOR_UNIT = "Incubadoras de Empresas";
            public const string RESEARCH_PROMOTION_UNIT = "Fomento a la Investigación";
            public const string PUBLICATION_UNIT = "Publicaciones";
            public const string INNOVATION_TECHNOLOGY_TRANSFER_UNIT = "Innovación y Transferencia Tecnológica";

            public static Dictionary<string, string> BOSSROL = new Dictionary<string, string>()
            {
                { BUSINESS_INCUBATOR_UNIT , GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT },
                { RESEARCH_PROMOTION_UNIT , GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT },
                { PUBLICATION_UNIT , GeneralConstants.ROLES.PUBLICATION_UNIT },
                { INNOVATION_TECHNOLOGY_TRANSFER_UNIT , GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT },
            };
        }

        public static class OperativePlan
        {
            public static class State
            {
                public const int PENDING = 0;
                public const int APPROVED = 1;
                public const int OBSERVED = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING , "Pendiente" },
                    { APPROVED , "Aprobado" },
                    { OBSERVED , "Observado" },
                };
            }
        }

        public static class ConvocationPostulant //deInvestigacion
        {
            public static class ProjectState
            {
                public const int INPROCESS = 0;
                public const int REQUESTDOCUMENT = 1;
                public const int DECLINED = 2;
                public const int ACCEPTED = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { INPROCESS , "En proceso" },
                    { REQUESTDOCUMENT , "Solicitud de sinceración de documentación" },
                    { DECLINED , "Rechazado" },
                    { ACCEPTED , "Aceptado" }
                };
            }

            public static class ReviewState
            {
                public const int PENDING = 0;
                public const int OBSERVED = 1;
                public const int DECLINED = 2;
                public const int ADMITTED = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING , "Pendiente" },
                    { OBSERVED , "Observado" },
                    { DECLINED , "Rechazado" },
                    { ADMITTED , "Admitido" }
                };
            }

            public static class ProgressState
            {
                public const int INPROCESS = 0;
                public const int FINISHED = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { INPROCESS , "En Proceso" },
                    { FINISHED , "Finalizado" },
                };
            }
        }

        public static class PostulantObservation
        {
            public static class State
            {
                public const int PENDINGCORRECTION = 0;
                public const int PENDINGREVIEW = 1;
                public const int NOTCORRECTED = 2;
                public const int CORRECTED = 3;



                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDINGCORRECTION , "Pendiente de corrección" },
                    { PENDINGREVIEW , "Pendiente de revisión" },
                    { NOTCORRECTED , "No subsanado" },
                    { CORRECTED , "Subsanado" }

                };
            }
        }

        public static class IncubatorPostulation
        {
            public static class ReviewState
            {
                public const int PENDING = 0;
                public const int APPROVED = 1;
                public const int DECLINED = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING , "Pendiente" },
                    { APPROVED , "Aprobado" },
                    { DECLINED , "Rechazado" },
                };
            }
        }
    }
}
