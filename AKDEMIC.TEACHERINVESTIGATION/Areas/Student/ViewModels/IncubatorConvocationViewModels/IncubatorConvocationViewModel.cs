using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.FacultyViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorConvocationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PicturePath { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AddressedTo { get; set; }
        public bool IsPostulant { get; set; }
    }

    public class IncubatorConvocationDetailViewModel: IncubatorConvocationViewModel
    {
        public string Requirements { get; set; }
        public string DocumentPath { get; set; }

        public string InscriptionStartDate { get; set; }
        public string InscriptionEndDate { get; set; }
        public List<FacultyViewModel> Faculties { get; set; }
    }


    public class IncubatorPostulationViewModel
    {
        public Guid IncubatorConvocationId { get; set; }
        public string Title { get; set; }//Nombre del Proyecto
        public string GeneralGoals { get; set; }//Objetivos Generales
        public int MonthDuration { get; set; }
        public decimal Budget { get; set; } //Presupuesto
        public Guid? DepartmentId { get; set; } //Informacion del Api
        public string DepartmentText { get; set; }

        public Guid? ProvinceId { get; set; } //Informacion del Api
        public string ProvinceText { get; set; }

        public Guid? DistrictId { get; set; } //Informacion del Api
        public string DistrictText { get; set; }
        public string AdviserId { get; set; } //Docente Asesor del Api Teacher.UserId
        public string CoAdviserId { get; set; } //Docente co Asesor del Api Teacher.UserId
        public List<IncubatorPostulationAnnexViewModel> IncubatorPostulationAnnexes { get; set; }
    }

    public class IncubatorPostulationAnnexViewModel
    {
        public IFormFile AnnexFile { get; set; }
        public Guid IncubatorConvocationAnnexId { get; set; }
    }
}
