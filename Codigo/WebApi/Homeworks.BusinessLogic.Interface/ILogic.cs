using System;
using System.Collections.Generic;

namespace Homeworks.BusinessLogic.Interface
{
    public interface ILogic<T>
    {
        T Create(T entity);

        void Remove(Guid id);

        T Update(Guid id, T entity);

        T Get(Guid id);

        IEnumerable<T> GetAll();
    }

}
