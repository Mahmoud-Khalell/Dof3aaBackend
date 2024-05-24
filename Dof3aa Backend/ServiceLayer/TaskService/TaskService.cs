using Core.entities;
using InfraStructure_Layer.baseSpecification;
using InfraStructure_Layer.Interfaces;
using ServiceLayer.DocumentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unit;

        public TaskService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<int> CreateTask(task task)
        {
            await unit.Repository<task>().AddAsync(task);
            return await unit.Complete();


        }

        public async Task<int> DeleteTask(int id)
        {
            var task =await unit.Repository<task>().GetByIdAsync(id);
            if(task.SaurceUrl != null)
            {
                DocumentService.DeleteFile(task.SaurceUrl);

            }

            unit.Repository<task>().Delete(task);
            return await unit.Complete();
        }

        public async Task<IReadOnlyList<task>> GetAllCourceTask(int CourceId)
        {
            var spec = new TaskSpecification(e=>e.CourceId==CourceId);
            return await unit.Repository<task>().FindAll(spec);
        }

        public async Task<IReadOnlyList<task>> GetAllTasks()
        {
            var spec = new TaskSpecification();
            return await unit.Repository<task>().FindAll(spec);
        }

        public async Task<task> GetTask(int id)
        {
            var spec= new TaskSpecification(e => e.Id == id);
            return await unit.Repository<task>().Find(spec);
        }

        public async Task<bool> IsExist(int id)
        {
            var task =await unit.Repository<task>().GetByIdAsync(id);
            return task != null;
        }

        public async Task<int> UpdateTask(task task)
        {
            unit.Repository<task>().Update(task);
            return await unit.Complete();
        }
    }
}
