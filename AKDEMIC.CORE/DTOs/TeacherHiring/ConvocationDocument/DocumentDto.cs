using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationDocument
{
    public class DocumentDto
    {
        public Guid ConvocationId { get; set; }
        public string CreatedAt { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool HasApplicantTeacherDocuments { get; set; }
        public byte Type { get; set; }
    }
}
