using Microsoft.EntityFrameworkCore;
using SampleStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public void Add(T item)
        {
            dbSet.Add(item);
        }

        private IQueryable _IncludeProperties(IQueryable<T> query, string? includeProperties = null)
        {
            foreach (string property in includeProperties.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }

            return query;
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (includeProperties != null) query = _IncludeProperties(query, includeProperties) as IQueryable<T>;

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            if (includeProperties != null) query = _IncludeProperties(query, includeProperties) as IQueryable<T>;

            return query.FirstOrDefault();
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
        }
    }
}
