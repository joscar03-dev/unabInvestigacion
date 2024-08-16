using AKDEMIC.CORE.Interfaces;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AKDEMIC.INFRASTRUCTURE.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class , IAggregateRoot
    {
        protected readonly AkdemicContext _context;

        public EfRepository(AkdemicContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<int> CountAsync(ISpecification<T> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).CountAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _context.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _context.Set<T>().FindAsync(keyValues, cancellationToken);
        }
        public async Task<T> GetByIdAsync(params object[] keyValues)
        {
            return await _context.Set<T>().FindAsync(keyValues);
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _context.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            if (ignoreQueryFilter)
            {
                return await _context.Set<T>().IgnoreQueryFilters().ToListAsync(cancellationToken);
            }
            else
            {
                return await _context.Set<T>().ToListAsync(cancellationToken);
            }

        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).ToListAsync(cancellationToken);
        }

        public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec, bool ignoreQueryFilter = false, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec, ignoreQueryFilter).ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec, bool ignoreQueryFilter = false)
        {
            if (ignoreQueryFilter)
            {
                return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().IgnoreQueryFilters().AsQueryable(), spec);
            }
            else
            {
                return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), spec);
            }
        }

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec, bool ignoreQueryFilter = false)
        {
            if (spec is null) throw new ArgumentNullException("Specification is required");
            if (spec.Selector is null) throw new SelectorNotFoundException();

            if (ignoreQueryFilter)
            {
                return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().IgnoreQueryFilters().AsQueryable(), spec);
            }
            else
            {
                return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), spec);
            }
        }

        public async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync(cancellationToken);
            return entities;
        }
    }
}
