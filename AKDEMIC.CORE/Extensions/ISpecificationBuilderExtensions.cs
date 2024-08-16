using AKDEMIC.CORE.Constants;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class ISpecificationBuilderExtensions
    {
        public static IOrderedSpecificationBuilder<T> OrderByCondition<T>(this ISpecificationBuilder<T> specificationBuilder, string condition, Expression<Func<T, object?>> orderExpression)
        {
            //if (orderExpression == null)
            //{
                return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
            //}


            //if (condition == GeneralConstants.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            //{
            //    ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
            //        .Add((orderExpression, OrderTypeEnum.OrderByDescending));
            //}
            //else
            //{
            //    ((List<(Expression<Func<T, object?>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
            //        .Add((orderExpression, OrderTypeEnum.OrderBy));
            //}

            //var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

            //return orderedSpecificationBuilder;
        }
    }
}
