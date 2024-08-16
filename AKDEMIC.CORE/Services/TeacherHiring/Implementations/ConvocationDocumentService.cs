using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationDocument;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationDocumentSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationDocumentService : IConvocationDocumentService
    {
        private readonly IAsyncRepository<ConvocationDocument> _convocationDocumentRepository;

        public ConvocationDocumentService(
            IAsyncRepository<ConvocationDocument> convocationDocumentRepository
            )
        {
            _convocationDocumentRepository = convocationDocumentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, byte? type)
        {
            Expression<Func<ConvocationDocument, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var documentSpecificationData = new ConvocationDocumentDatatableSpecification(parameters, orderByPredicate, convocationId, type);
            var documentSpecificationCount = new ConvocationDocumentFilterSpecification(convocationId, type);

            var data = await _convocationDocumentRepository.ListAsync(documentSpecificationData);
            var recordsFiltered = await _convocationDocumentRepository.CountAsync(documentSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<DocumentDto>> GetDocuments(Guid convocationId, byte? type = null)
        {
            var documentSpecification = new ConvocationDocumentDataSpecification(convocationId, type);
            var data = await _convocationDocumentRepository.ListAsync(documentSpecification);
            return data;
        }
    }
}
