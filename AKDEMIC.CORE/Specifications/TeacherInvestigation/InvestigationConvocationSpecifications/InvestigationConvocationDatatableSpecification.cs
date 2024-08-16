using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationSpecifications
{
    public sealed class InvestigationConvocationDatatableSpecification : Specification<InvestigationConvocation, object>
    {
        public InvestigationConvocationDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<InvestigationConvocation, dynamic>> orderByPredicate = null, string searchValue=null)
        {
            if(!string.IsNullOrEmpty(searchValue))
            {
                Query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            Query
                .OrderByCondition(GeneralConstants.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    InscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    InscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    x.MinScore,
                    x.State,
                    x.AllowInquiries
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
