using Core.entities;
using InfraStructure_Layer.baseSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task AddAsync(T entity);

        void Delete(T entity);
        void Update(T entity);

        Task<T> Find(IBaseSpecification<T> spec);
        Task<IReadOnlyList<T>> FindAll(IBaseSpecification<T> spec);


    }
}
