using Microsoft.AspNetCore.Mvc;
using TaskTrack.Repo.Models;
using TaskTrack.Service.DTOs;
using TaskTrack.Service.Services;

namespace TaskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        /// <summary>GET /api/tasks — List all active tasks</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllActiveAsync();
            return Ok(result);
        }

        /// <summary>GET /api/tasks/search?title=&amp;status=&amp;priority=&amp;projectId=&amp;tagId= — Multi-filter</summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? title,
            [FromQuery] TaskStatusEnum? status,
            [FromQuery] TaskPriority? priority,
            [FromQuery] int? projectId,
            [FromQuery] int? tagId)
        {
            var result = await _service.SearchAsync(title, status, priority, projectId, tagId);
            return Ok(result);
        }

        /// <summary>GET /api/tasks/project/{projectId} — Tasks belonging to a project</summary>
        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var result = await _service.GetByProjectAsync(projectId);
            return Ok(result);
        }

        /// <summary>GET /api/tasks/{id} — Get task with its tags</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Task with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>POST /api/tasks — Create task (optional TagIDs array)</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.TaskID }, result);
        }

        /// <summary>PUT /api/tasks/{id} — Update task; replaces all tags; sets ModifiedDate</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = $"Task with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>DELETE /api/tasks/{id} — Soft-delete (IsActive = false, never hard-deletes)</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Task with ID {id} not found." });

            return NoContent();
        }
    }
}
