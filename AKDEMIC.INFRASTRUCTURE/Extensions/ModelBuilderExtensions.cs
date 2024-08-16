using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.INFRASTRUCTURE.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Invoke(this ModelBuilder modelBuilder, MethodInfo methodInfo, Type type)
        {
            if (methodInfo != null)
            {
                var methodInfoGeneric = methodInfo.MakeGenericMethod(type);

                methodInfoGeneric.Invoke(modelBuilder, new object[] { modelBuilder });
            }
        }
    }
}
