using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectCreateTeamMembersViewModel
    {
        public Guid InvestigationProjectId { get; set; }
        public string UserId { get; set; }
        public Guid TeamMemberRoleId { get; set; }
        public IFormFile File { get; set; }
        public string CvFilePath { get; set; }
        public string Objectives { get; set; }
    }
}
