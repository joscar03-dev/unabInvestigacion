using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Extensions.MutableRelationalNameExtensions;

namespace AKDEMIC.CORE.Extensions
{
    public static class IMutableIndexExtensions
    {
        public static IMutableIndex NormalizeRelationalName(this IMutableIndex mutableIndex, int length = -1)
        {
            var mutableIndexRelational = mutableIndex;
            MutableRelationalName mutableKeyRelationalName = mutableIndexRelational.GetDefaultDatabaseName();
            var normalizedRelationalName = mutableKeyRelationalName.NormalizeRelationalName();

            if (length >= 0 && normalizedRelationalName.Length >= length)
            {
                normalizedRelationalName = normalizedRelationalName.Substring(0, length);
            }

            mutableIndexRelational.SetDatabaseName(normalizedRelationalName);

            return mutableIndex;
        }
    }
}
