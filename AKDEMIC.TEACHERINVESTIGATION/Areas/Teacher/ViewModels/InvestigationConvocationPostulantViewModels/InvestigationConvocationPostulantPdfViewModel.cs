using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationPostulantViewModels
{
    public class InvestigationConvocationPostulantPdfViewModel
    {
        public string ShieldPath { get; set; }
        public string ImageLogoPath { get; set; }

        public string InstitutionName { get; set; }

        public string InstitutionLocation { get; set; }

        public string ProjectTitle { get; set; }

        public string UserFullName { get; set; }
        public string UserDni { get; set; }
        public string UserName { get; set; }

        public List<string> ResearchLines { get; set; }

        public string ResearchLineConcat { get; set; }
        public string InvestigationType { get; set; }

        public string ExecutionAddress { get; set; }

        public int Budget { get; set; }

        public string MainLocation { get; set; }

        public string ProblemDescription { get; set; }
        public string ProblemRecord { get; set; }
        public string ProblemFormulation { get; set; }
        public string Justification { get; set; }
        public string Hypothesis { get; set; }
        public string Variable { get; set; }
        public string SpecificGoal { get; set; }
        public string MethodologyDescription { get; set; }
        public string EthicalConsiderations { get; set; }

        public string ExpectedResults { get; set; }
        public string BibliographicReferences { get; set; }

        public List<UserPdfViewModel> UserPdfViewModel { get; set; }

        public double TotalDays { get; set; }

        public double TotalYears { get; set; }
        public double TotalMonths { get; set; }
        public double RemainingDays { get; set; }
    }

    public class UserPdfViewModel
    {
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string Speciality { get; set; }
        public string AcademicDegree { get; set; }
        public string MainFunction { get; set; }
    }

    public class UserApiViewModel
    {
        public string userName { get; set; }
        public string fullName { get; set; }
        public bool exist { get; set; }
        public UserGenericStudiesViewModel maxStudy { get; set; }
    }

    public class UserGenericStudiesViewModel
    {
        public string specialty { get; set; }
        public string institution { get; set; }
        public string institutionType { get; set; }
        public string academicDegree { get; set; }
    }
}
