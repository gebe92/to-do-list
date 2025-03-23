using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _todoRepo;
        private readonly UserManager<AppUser> userManager;

        public ToDoController(IToDoRepository todoRepo, UserManager<AppUser> userManager)
        {
            _todoRepo = todoRepo;
            this.userManager = userManager;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTasksDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskModel = taskDto.CreateTaskDto();
            await _todoRepo.CreateAsync(taskModel);
            return CreatedAtAction(nameof(GetById), new { id = taskModel.TaskListId }, taskModel.GetTaskDto());
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] TaskQueryObject query)
        {
            var task = await _todoRepo.GetAllAsync(query);
            var taskDto = task.Select(t => t.GetTaskDto());

            return Ok(taskDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _todoRepo.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task.GetTaskDto());
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId, [FromQuery] TaskQueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _todoRepo.GetByUserIdAsync(userId, query);

            if (task == null)
            {
                return NotFound();
            }

            var taskDto = task.Select(t => t.GetTaskDto());

            return Ok(taskDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var task = await _todoRepo.UpdateAsync(id, taskDto);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task.GetTaskDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var task = await _todoRepo.DeleteAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}