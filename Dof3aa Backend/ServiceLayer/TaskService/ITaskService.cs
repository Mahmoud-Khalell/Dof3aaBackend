using Core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.TaskService
{
    public interface ITaskService
    {
        Task<int> CreateTask(task task);
        Task<int> DeleteTask(int id);
        Task<int> UpdateTask(task task);
        Task<task> GetTask(int id);
        Task<IReadOnlyList<task>> GetAllTasks();
        Task<bool> IsExist(int id);
        Task<IReadOnlyList<task>> GetAllCourceTask(int CourceId);


    }
}
