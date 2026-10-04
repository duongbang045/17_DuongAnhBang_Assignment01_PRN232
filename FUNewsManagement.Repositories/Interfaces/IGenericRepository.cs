using System.Linq.Expressions;

namespace FUNewsManagement.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Search(Expression<Func<T, bool>> predicate);
    }
}
