using Core.Context;
using Core.entities;
using InfraStructure_Layer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure_Layer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Connector connector;
        private Hashtable repositories;
        public UnitOfWork(Connector connector)
        {
            this.connector = connector;
        }
        public async Task<int> Complete()=> await connector.SaveChangesAsync();


        public void Dispose()=>connector.Dispose();


        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null) repositories = new Hashtable();
            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), connector);
                repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<T>)repositories[type];
        }
    }
}
