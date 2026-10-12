using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.DTOs;
using TaskTrack.Service.Services;

namespace TaskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentsController(IDepartmentService service)
        {
            _service = service;
        }

        /// <summary>GET /api/departments — List all active departments (Public)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllActiveAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }

        /// <summary>GET /api/departments/search?name= — Partial name search (Public)</summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search query 'name' is required." });

            var result = await _service.SearchByNameAsync(name);
            return Ok(result);
        }

        /// <summary>GET /api/departments/{id} — Get department with its projects (Public)</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Department with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>POST /api/departments — Create new department (Protected)</summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.DepartmentID }, result);
        }

        /// <summary>PUT /api/departments/{id} — Update a department (Protected)</summary>
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = $"Department with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>DELETE /api/departments/{id} — Delete department (Protected, blocked if has projects)</summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Department with ID {id} not found." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
