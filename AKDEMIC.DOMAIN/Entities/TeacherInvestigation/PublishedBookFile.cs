﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PublishedBookFile : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public Guid PublishedBookId { get; set; }
        public PublishedBook PublishedBook { get; set; }
    }
}
