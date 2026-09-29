using Microsoft.AspNetCore.Mvc;
using TaskTrack.Repo.Models;
using TaskTrack.Service.DTOs;
using TaskTrack.Service.Services;

namespace TaskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>GET /api/projects — List all active projects (includes DepartmentName)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>GET /api/projects/search?name=&amp;status=&amp;departmentId= — Filter (all params optional)</summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] ProjectStatus? status,
            [FromQuery] int? departmentId)
        {
            var result = await _service.SearchAsync(name, status, departmentId);
            return Ok(result);
        }

        /// <summary>GET /api/projects/department/{departmentId} — Projects by department</summary>
        [HttpGet("department/{departmentId:int}")]
        public async Task<IActionResult> GetByDepartment(int departmentId)
        {
            var result = await _service.GetByDepartmentAsync(departmentId);
            return Ok(result);
        }

        /// <summary>GET /api/projects/{id} — Get project with its tasks</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Project with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>POST /api/projects — Create new project</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.ProjectID }, result);
        }

        /// <summary>PUT /api/projects/{id} — Update a project</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = $"Project with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>DELETE /api/projects/{id} — Delete (blocked if has linked tasks)</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Project with ID {id} not found." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
