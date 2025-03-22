using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IToDoRepository
    {
        Task<List<TaskList>> GetAllAsync(TaskQueryObject queryObject);
        Task<TaskList?> GetByIdAsync(int id);
        Task<List<TaskList>> GetByUserIdAsync(int userId, TaskQueryObject queryObject);
        Task<TaskList> CreateAsync(TaskList taskModel);
        Task<TaskList?> UpdateAsync(int id, UpdateTaskDto taskDto);
        Task<TaskList?> DeleteAsync(int id);
    }
}