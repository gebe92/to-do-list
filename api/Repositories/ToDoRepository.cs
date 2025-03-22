using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;

        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskList> CreateAsync(TaskList taskModel)
        {
            await _context.TaskLists.AddAsync(taskModel);
            await _context.SaveChangesAsync();
            return taskModel;
        }

        public async Task<TaskList?> DeleteAsync(int id)
        {
            var findTask = await _context.TaskLists.FirstOrDefaultAsync(x => x.TaskListId == id);

            if (findTask == null)
            {
                return null;
            }

            _context.TaskLists.Remove(findTask);
            await _context.SaveChangesAsync();

            return findTask;
        }

        public async Task<List<TaskList>> GetAllAsync(TaskQueryObject query)
        {
            var task = _context.TaskLists.AsQueryable();
            task = Filter(task, query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await task.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<TaskList?> GetByIdAsync(int id)
        {
            return await _context.TaskLists.FirstOrDefaultAsync(i => i.TaskListId == id);
        }

        public async Task<List<TaskList>> GetByUserIdAsync(int userId, TaskQueryObject query)
        {
            var trans = _context.TaskLists.Where(i => i.UserId == userId);
            trans = Filter(trans, query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await trans.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<TaskList?> UpdateAsync(int id, UpdateTaskDto taskDto)
        {
            var findTask = await _context.TaskLists.FirstOrDefaultAsync(x => x.TaskListId == id);

            if (findTask == null)
            {
                return null;
            }

            findTask.Title = taskDto.Title;
            findTask.Desc = taskDto.Desc;
            findTask.Priority = taskDto.Priority;
            findTask.Status = taskDto.Status;
            findTask.StartDate = taskDto.StartDate;
            findTask.EndDate = taskDto.EndDate;

            await _context.SaveChangesAsync();

            return findTask;
        }

        private IQueryable<TaskList> Filter(IQueryable<TaskList> task, TaskQueryObject query)
        {
            if (query != null && HasFilterValues(query))
            {
                if (!string.IsNullOrWhiteSpace(query.Priority))
                {
                    task = task.Where(t => t.Priority.Equals(query.Priority));
                }

                if (!string.IsNullOrWhiteSpace(query.Status))
                {
                    task = task.Where(t => t.Status.Equals(query.Status));
                }

                if (query.TaskDate != null)
                {
                    task = task.Where(t => t.StartDate.Date <= query.TaskDate.Value.Date && t.EndDate.Date >= query.TaskDate.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    switch (query.SortBy)
                    {
                        case "Title":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.Title) : task.OrderBy(t => t.Title);
                            break;
                        case "Desc":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.Desc) : task.OrderBy(t => t.Desc);
                            break;
                        case "Priority":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.Priority) : task.OrderBy(t => t.Priority);
                            break;
                        case "Status":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.Status) : task.OrderBy(t => t.Status);
                            break;
                        case "StartDate":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.StartDate) : task.OrderBy(t => t.StartDate);
                            break;
                        case "EndDate":
                            task = query.IsDecsending ? task.OrderByDescending(t => t.EndDate) : task.OrderBy(t => t.EndDate);
                            break;
                    }
                }
            }

            return task;
        }

        private bool HasFilterValues(TaskQueryObject query)
        {
            return !string.IsNullOrWhiteSpace(query.Priority) ||
                !string.IsNullOrWhiteSpace(query.Status) ||
                query.TaskDate != null ||
                !string.IsNullOrWhiteSpace(query.SortBy);
        }
    }
}