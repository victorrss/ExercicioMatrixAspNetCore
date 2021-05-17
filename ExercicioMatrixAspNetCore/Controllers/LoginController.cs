using ExercicioMatrix.DAL.Usuarios.Interfaces;
using ExercicioMatrix.DAL.Usuarios.Models;
using ExercicioMatrix.DAL.Usuarios.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExercicioMatrix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        { 
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = await _authService.LoginAsync(model);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}