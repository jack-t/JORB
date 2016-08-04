using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;

namespace Demo.Services
{
    public interface IRepository<T, in TParam> where T : class 
    {
        IQueryable<T> GetAll();
        T GetFor(TParam param);
        void Add(T t);
        void Remove(T t);
    }
}
