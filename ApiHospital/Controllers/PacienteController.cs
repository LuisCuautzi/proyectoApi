using ApiHospital.Data;
using ApiHospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHospital.Controllers
{
    [ApiController]
    [Route("api/paciente")]
    public class PacienteController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public PacienteController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetListPaciente()
        {
            var listaPaciente = await _context.pacientes.Include(a => a.citas).ToArrayAsync();
            return listaPaciente;
        }

        [HttpGet("{id:int}", Name = "GetPacienteById")]
        public async Task<ActionResult<Paciente>> GetPacienteById(int id)
        {
            var paciente = await _context.pacientes.Include(a => a.citas).FirstOrDefaultAsync(a => a.Id == id);
            if (paciente == null)
            {
                return NotFound("Paciente no encontrado");
            }

            return paciente;
        }

        [HttpPost]
        public async Task<ActionResult> RegristrarPaciente([FromBody] Paciente paciente)
        {
            var existe = await _context.pacientes.AnyAsync(a => a.Nombre == paciente.Nombre);
            if (existe)
            {
                return BadRequest($"Ya hay un paciente com ese Nombre: {paciente.Nombre}");
            }
            _context.pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetPacienteById", new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePaciente([FromBody] Paciente paciente, int id)
        {
            if (paciente.Id != id)
            {
                return BadRequest("No se encuentra es ID");
            }
            var existe = await _context.pacientes.AnyAsync(a => a.Nombre == paciente.Nombre && a.Id != id);

            if (existe)
            {
                return BadRequest("El paciente ya fue utilizado");
            }
            _context.pacientes.Update(paciente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound("Ese paciente no esta registrado");
            }
            _context.pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{prefixText}")]
        public async Task<ActionResult<IEnumerable<Paciente>>> BuscarPaciente(string prefixText)
        {
            var paciente = await _context.pacientes.Include(a => a.citas).Where(a => a.Nombre.Contains(prefixText) ||
                                                               a.Direccion.Contains(prefixText) ||
                                                               a.Apellido.Contains(prefixText) ||
                                                               a.Genero.Contains(prefixText) ||
                                                               a.Direccion.Contains(prefixText)).ToListAsync();

            return paciente;
        }
    }
}
