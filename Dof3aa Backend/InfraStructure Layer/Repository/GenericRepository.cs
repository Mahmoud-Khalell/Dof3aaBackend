using Core.Context;
using Core.entities;
using InfraStructure_Layer.baseSpecification;
using InfraStructure_Layer.Interfaces;
using InfraStructure_Layer.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly Connector connector;

        public GenericRepository(Connector connector)
        {
            this.connector = connector;
        }
        public async Task AddAsync(T entity)=>await connector.Set<T>().AddAsync(entity);
        

        public void Delete(T entity)=> connector.Set<T>().Remove(entity);


        public async Task<IReadOnlyList<T>> GetAllAsync()=> await connector.Set<T>().ToListAsync();
        

        public async Task<T> GetByIdAsync(int id)=>await connector.Set<T>().FindAsync(id);

        public void Update(T entity) => connector.Set<T>().Update(entity);  

        public async Task<T> Find(IBaseSpecification<T> spec)
        {
            var query=SpecificationEvaluator.GetQuery(connector.Set<T>().AsQueryable(), spec);
            return await query.FirstOrDefaultAsync();

        }
        public async Task<IReadOnlyList<T>> FindAll(IBaseSpecification<T> spec)
        {
            var query = SpecificationEvaluator.GetQuery(connector.Set<T>().AsQueryable(), spec);
            return await query.ToListAsync();
        }
    }
}
