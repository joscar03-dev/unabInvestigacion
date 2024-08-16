using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Constants
{
    public static class FileStorageConstants
    {
        public static class Mode
        {
            public const int BLOB_STORAGE_MODE = 1;
            public const int SERVER_STORAGE_MODE = 2;
        }

        public static class SystemFolder
        {
            //GENERAL
            public const string GENERAL = "general";

            //TEACHER_HIRING
            public const string TEACHER_HIRING = "teacherhiring";

            //TEACHER_INVESTIGATION
            public const string TEACHER_INVESTIGATION = "teacherinvestigation";
            public const string INVESTIGACION_FORMATIVA = "investigacionformativa";
            public const string INVESTIGACION_FOMENTO = "investigacionfomento";
            public const string ASESORIA = "asesoria";

        }

        public static class CONTAINER_NAMES
        {

            //GENERAL
            public const string CONVOCATORIA = "convocatoria";
            public const string CARTAPRESENTACION = "cartapresentacion";
            public const string PROYECTOACTIVIDAD = "proyectoactividad";

            public const string ANEXOCONVOCATORIA = "anexosconvocatoria";
            public const string USER_CURRICULUM_VITAE = "usercurriculumvitae";
            public const string USERPICTURE = "userpicture";

            //TEACHER_INVESTIGATION

            public const string EVENTS = "eventos";
            public const string INVESTIGATIONCONVOCATION_DOCUMENTS = "investigationconvocationdocuments";
            public const string PLANTRABAJO = "plantrabajo";
            public const string ASESORIAALUMNO = "asesoriaalumnos";

            public const string INVESTIGATIONCONVOCATIONPOSTULANT_DOCUMENTS = "investigationconvocationpostulantdocuments";
            public const string INVESTIGATIONCONVOCATION_RESOLUTIONS = "investigationconvocationresolutions";
            public const string PUBLICATION_DOCUMENTS = "publicationdocuments";

            public const string INVESTIGATIONCONVOCATION_PHOTOS = "investigationconvocationphotos";
            public const string CONVOCATION_DOCUMENTS = "convocationdocuments";


            public const string INCUBATORCONVOCATION_PHOTOS = "incubatorconvocationphotos";
            public const string INCUBATORCONVOCATION_DOCUMENTS = "incubatorconvocationdocuments";
            public const string INCUBATORPOSTULATION_ANNEXES = "incubatorpostulationannexes";


            public const string APPLICANT_TEACHER_DOCUMENTS = "applicantteacherdocuments";
            public const string OPERATIVE_PLAN = "operativeplan";

            //TEACHER_INVESTIGATION

        
            public const string CONFERENCE_DOCUMENTS = "conferencedocuments";
            public const string PUBLISHEDBOOK_DOCUMENTS = "publishedbookdocuments";
            public const string PUBLISHEDCHAPTERBOOK_DOCUMENTS = "publishedchapterbookdocuments";

        



        }

    }
}
