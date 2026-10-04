using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FUNewsManagement.Repositories.Data;
using FUNewsManagement.Repositories.Interfaces;

namespace FUNewsManagement.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FUNewsManagementDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(FUNewsManagementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }
    }
}
