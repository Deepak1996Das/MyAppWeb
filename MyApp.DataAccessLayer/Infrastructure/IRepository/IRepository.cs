using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.IRepository
{
   public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? inludeProperties=null);

        T GetById(Expression<Func<T,bool>> predicate, string? inludeProperties = null);
        void Delete(T entity);
        void Add(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
