using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.DTOs;
using TaskTrack.Service.Services;

namespace TaskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>GET /api/accounts — List all accounts (Admin only)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        /// <summary>GET /api/accounts/{id} — Get account by ID (Admin only)</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }
            return Ok(account);
        }

        /// <summary>PUT /api/accounts/{id} — Update account name or role (Admin only)</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _accountService.UpdateAsync(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }

            return Ok(updated);
        }

        /// <summary>DELETE /api/accounts/{id} — Delete account (Admin only, rejected if tasks exist)</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _accountService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new { message = $"Account with ID {id} not found." });
                }

                return Ok(new { message = $"Account {id} deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Rejection due to linked tasks — HTTP 400
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
