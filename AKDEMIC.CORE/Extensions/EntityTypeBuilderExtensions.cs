using AKDEMIC.CORE.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ToDatabaseTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, int databaseType, string name, string schema = null) where TEntity : class
        {
            switch (databaseType)
            {
                case DataBaseConstants.MYSQL:
                    entityTypeBuilder.ToTable($"{schema ?? "dbo"}_{name}");

                    break;
                case DataBaseConstants.SQL:
                    entityTypeBuilder.ToTable(name, schema);

                    break;
            }

            return entityTypeBuilder;
        }
    }
}
