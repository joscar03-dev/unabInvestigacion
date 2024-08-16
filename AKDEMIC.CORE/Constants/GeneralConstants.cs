using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants
{
    public class GeneralConstants
    {
        public static class Database
        {
            public const int DATABASE = DataBaseConstants.MYSQL;
            public const bool FULLTEXT_ENABLED = false;
            public static class CONNECTION_STRINGS
            {
                public readonly static int CONNECTION_STRING = DataBaseConstants.ConnectionString.MySql.DEFAULT;
            }

            public static class VERSIONS
            {
                public readonly static int VERSION = DataBaseConstants.Versions.MySql.V8021;
            }
        }

        public static class Institution
        {
            public static int VALUE = InstitutionConstants.UNAB;
        }

        public static class Themes
        {
            public static int VALUE = Institution.VALUE;
        }

        public static class FileStorage
        {
            public static int STORAGE_MODE = InstitutionConstants.StorageMode[Institution.VALUE];
            public static string PATH = InstitutionConstants.Path[Institution.VALUE];
        }

        public static class Authentication
        {
            public static bool SSO_ENABLED = false;

            public static class SingleSignOn
            {
                public const string LOCALHOST_AUTHORITY = "https://localhost:44376";
                public static Dictionary<int, string> Authorities = new Dictionary<int, string>()
                {
                    { InstitutionConstants.UNAB, "micampussite" },
                };
            }
        }

        public static string GetAuthority(bool isDevelopment = false)
        {
            try
            {
                if (isDevelopment) return Authentication.SingleSignOn.LOCALHOST_AUTHORITY;

                return Authentication.SingleSignOn.Authorities[GeneralConstants.Institution.VALUE];
            }
            catch (Exception)
            {
                return Authentication.SingleSignOn.LOCALHOST_AUTHORITY;
            }
        }

        public static string GetInstitutionName()
        {
            return InstitutionConstants.Names[Institution.VALUE];
        }
        public static string GetInstitutionLocation()
        {
            return InstitutionConstants.Locations[Institution.VALUE];
        }

        public static string GetInstitutionAbbreviation()
        {
            return InstitutionConstants.Abbreviations[Institution.VALUE];
        }

        public static string GetTheme()
        {
            return InstitutionConstants.Values[Themes.VALUE];
        }

        public static class ROLES
        {
            #region  ADMINISTRADORES
            public const string TEACHERINVESTIGATION_ADMIN = "Administrador de Investigación Docente";
            public const string SUPERADMIN = "Superadmin";
            #endregion

            #region GENERALS
            public const string TEACHERS = "Docentes";
            public const string STUDENTS = "Alumnos";
            #endregion
            public const string DOCENTE_ASESOR = "Docente Asesor";

            #region TEACHER INVESTIGATION
            public const string REPORT_QUERIES = "Consulta Reportes";
            public const string OTI_SUPPORT = "Apoyo de Tecnología";
            public const string ACOMPANIANTE = "Acompañante";

            public const string RESEARCHERS = "Investigador";
            public const string TECHNICAL_COMMITTEE = "Comité técnico";
            public const string EVALUATOR_COMMITTEE = "Comité evaluador";
            public const string INVESTIGATIONCONVOCATION_MONITOR = "Monitor de convocatoria de investigación";
            public const string INVESTIGATIONCONVOCATION_COORDINATORMONITOR = "Coordinador monitor de convocatoria de investigación";
            public const string INCUBATORCONVOCATION_MONITOR = "Monitor de convocatoria de incubadora";
            public const string INCUBATORCONVOCATION_COORDINATORMONITOR = "Coordinador monitor de convocatoria de incubadora";

            public const string TECHNICAL_COLLABORATOR = "Colaborador Técnico";


            public const string BUSINESS_INCUBATOR_UNIT = "Jefe de Unidad de Incubadoras de Empresas";
            public const string RESEARCH_PROMOTION_UNIT = "Jefe de Unidad de Fomento a la Investigación";
            public const string PUBLICATION_UNIT = "Jefe de Unidad de Publicaciones";
            public const string INNOVATION_TECHNOLOGY_TRANSFER_UNIT = "Jefe de Unidad de Innovación y Transferencia Tecnológica";

            #endregion

            public const string EXTERNAL_EVALUATOR = "Evaluador Externo";

            #region TEACHER HIRING
            public const string APPLICANT_TEACHER = "Docente Postulante";
            public const string CONVOCATION_PRESIDENT = "Presidente de la Convocatoria";
            public const string CONVOCATION_MEMBER = "Miembro de la Convocatoria";
            public const string EXTERNAL_JURY = "Jurado Externo";
            public const string TEACHING_SECRETARY = "Secretario Docente";

            #endregion

        }

        public static class Solution
        {
            public const int TeacherInvestigation = 1;
            public const int TeacherHiring = 2;
            public const int WebApi = 3;
            public static Dictionary<int, string> Values = new Dictionary<int, string>()
            {
                { TeacherInvestigation, "Sistema de Investigación Docente" },
                { TeacherHiring, "Contratación Docente" },
                { WebApi, "Web Api"}
            };
            public static Dictionary<int, Dictionary<int, string>> Institution = new Dictionary<int, Dictionary<int, string>>()
            {
                {
                    InstitutionConstants.UNAB,
                    new Dictionary<int, string>()
                    {
                        { TeacherInvestigation, Values[TeacherInvestigation] },
                        { TeacherHiring, Values[TeacherHiring] },
                    }
                }
            };
            public static Dictionary<int, Dictionary<int, string>> Routes = new Dictionary<int, Dictionary<int, string>>
            {
                { 0, new Dictionary<int, string>{
                    { TeacherInvestigation, "https://localhost:5005/" },
                    { TeacherHiring, "http://localhost:5000/" },
                    { WebApi, "http://apiweb.unab.edu.pe/" }
                } },
                { InstitutionConstants.UNAB, new Dictionary<int, string> {
                    { TeacherInvestigation, "http://investigacion.unab.edu.pe/" },
                    { TeacherHiring, "" },
                    { WebApi, "http://apiweb.unab.edu.pe/"}
                } },
            };
        }

        public static class DATATABLE
        {
            public static class SERVER_SIDE
            {
                public static class DEFAULT
                {
                    public const string ORDER_DIRECTION = "DESC";
                }

                public static class SENT_PARAMETERS
                {
                    public const string DRAW_COUNTER = "draw";
                    public const string PAGING_FIRST_RECORD = "start";
                    public const string RECORDS_PER_DRAW = "length";
                    public const string SEARCH_VALUE = "search[value]";
                    public const string SEARCH_REGEX = "search[regex]";
                    public const string ORDER_COLUMN = "order[0][column]";
                    public const string ORDER_DIRECTION = "order[0][dir]";
                }
            }
        }

        public static class SELECT2
        {
            public static class DEFAULT
            {
                public const int PAGE_SIZE = 10;
            }

            public static class SERVER_SIDE
            {
                public static class REQUEST_PARAMETERS
                {
                    public const string CURRENT_PAGE = "page";
                    public const string QUERY = "q";
                    public const string REQUEST_TYPE = "_type";
                    public const string SEARCH_TERM = "term";
                }

                public static class REQUEST_TYPE
                {
                    public const string QUERY = "query";
                    public const string QUERY_APPEND = "query_append";
                }
            }
        }

        public static class ENTITY_ENTRIES
        {
            public static class PROPERTY_NAME
            {
                public const string CODE_NUMBER = "CodeNumber";
                public const string CODE_TEXT = "CodeText";
                public const string CREATED_AT = "CreatedAt";
                public const string DELETED_AT = "DeletedAt";
                public const string UPDATED_AT = "UpdatedAt";

                public const string CREATED_BY = "CreatedBy";
                public const string DELETED_BY = "DeletedBy";
                public const string UPDATED_BY = "UpdatedBy";
            }
        }

        public static class FORMATS
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
            public const string DATETIME_CUSTOM = "dd/MM/yyyy HH:mm";
        }

        public static class TimeZoneInfo
        {
            public const bool DisableDaylightSavingTime = true;
            public const int Gmt = -5;
        }

        public const string LINUX_TIMEZONE_ID = "America/Bogota";
        public const string OSX_TIMEZONE_ID = "America/Cayman";
        public const string WINDOWS_TIMEZONE_ID = "SA Pacific Standard Time";

        public static class PermissionHelpers
        {
            public enum Permission
            {
                [Description("Eliminar usuarios")]
                DeleteUser,
                [Description("Agregar usuarios")]
                AddUser,
                [Description("Administrar roles")]
                ManageRoles
            }
        }

        public static class INVESTIGATIONCONVOCATION
        {
            public static class STATES
            {
                public const byte OPEN = 0;
                public const byte CLOSED = 1;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
                {
                    { OPEN, "Abierta" },
                    { CLOSED, "Cerrada" },
                };
            }
        }

        public static class USERREQUEST_STATES
        {
            public const int PENDING = 0;
            public const int ACCEPTED = 1;
            public const int REJECTED = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { PENDING, "Pendiente" },
                { ACCEPTED, "Aceptado" },
                { REJECTED, "Rechazado" }
            };
        }

        public static class USERREQUEST_TYPE
        {
            public const int CONTEST_EVALUATOR = 1;
            public const int TECHNICAL_COLLABORATOR = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { CONTEST_EVALUATOR, "Evaluador de Concurso" },
                { TECHNICAL_COLLABORATOR, "Colaborador Técnico" },
            };
        }

        public static class USER_TYPES
        {
            public const int NOT_ASIGNED = 0;
            public const int TEACHER = 1;
            public const int ADMINISTRATIVE = 2;
            public const int STUDENT = 3;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NOT_ASIGNED, "No Asignado" },
                { TEACHER, "Docente" },
                { ADMINISTRATIVE, "Administrativo" },
                { STUDENT, "Alumno" }
            };
        }

        public static string GetApplicationRoute(int application, bool isDevelopment = false)
        {
            try
            {
                if (isDevelopment) return GeneralConstants.Solution.Routes[0][application];

                return GeneralConstants.Solution.Routes[GeneralConstants.Institution.VALUE][application];
            }
            catch (Exception)
            {
                return GeneralConstants.Solution.Routes[0][application];
            }
        }
    }
}