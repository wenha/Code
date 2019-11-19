using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Application.Core.Entity;

namespace Application.Core.Interface
{
    public interface IRepository<T> where T:BaseEntity
    {
        T GetById(int id);

        IEnumerable<T> GetList();

        IEnumerable<T> GetList(Expression<Func<T, bool>> predict);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);
    }
}
