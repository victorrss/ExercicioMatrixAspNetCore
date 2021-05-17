using System.Threading.Tasks;
using AutoMapper;
using ExercicioMatrix.DAL.Usuarios;
using ExercicioMatrix.DAL.Usuarios.Interfaces;
using ExercicioMatrix.DAL.Usuarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExercicioMatrix.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UserController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = Roles.ROLE_ADMIN + "," + Roles.ROLE_READ)]
        public async Task<IActionResult> ListarTodos()
        {
            var users = await _usuarioService.ListarTodosAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.ROLE_ADMIN + "," + Roles.ROLE_READ)]
        public async Task<IActionResult> ListarUsuario(string id)
        {
            var req = new UsuarioRemoveGetByIdRequest { Id = id };
            var model = await _usuarioService.ListarUsuarioPorIdAsync(req);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UsuarioApiResponse>(model));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ROLE_ADMIN + "," + Roles.ROLE_CREATE)]
        public async Task<IActionResult> Adicionar([FromBody] UsuarioAddUpdateRequest req)
        {
            if (ModelState.IsValid)
            {
                var id = await _usuarioService.AdicionarAsync(req);
                if (!string.IsNullOrEmpty(id))
                    return Created($"/api/user/{id}", req);
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = Roles.ROLE_ADMIN + "," + Roles.ROLE_UPDATE)]
        public async Task<IActionResult> Editar([FromBody] UsuarioAddUpdateRequest req)
        {
            if (ModelState.IsValid)
            {
                await _usuarioService.EditarAsync(req);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.ROLE_ADMIN + "," + Roles.ROLE_DELETE)]
        public async Task<IActionResult> Remover(string id)
        {
            var req = new UsuarioRemoveGetByIdRequest { Id = id };
            await _usuarioService.RemoverAsync(req);
            return NoContent();
        }


    }
}
