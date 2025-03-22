using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class ToDoMappers
    {
        public static TaskDto GetTaskDto(this TaskList taskModel)
        {
            return new TaskDto
            {
                TaskListId = taskModel.TaskListId,
                UserId = taskModel.UserId,
                Title = taskModel.Title,
                Desc = taskModel.Desc,
                Priority = taskModel.Priority,
                Status = taskModel.Status,
                CreatedAt = taskModel.CreatedAt,
                StartDate = taskModel.StartDate,
                EndDate = taskModel.EndDate
            };
        }

        public static TaskList CreateTaskDto(this CreateTasksDto taskModel)
        {
            return new TaskList
            {
                UserId = taskModel.UserId,
                Title = taskModel.Title,
                Desc = taskModel.Desc,
                Priority = taskModel.Priority,
                Status = taskModel.Status,
                CreatedAt = taskModel.CreatedAt,
                StartDate = taskModel.StartDate,
                EndDate = taskModel.EndDate
            };
        }
    }
}