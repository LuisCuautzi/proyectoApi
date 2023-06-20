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
    [Route("api/historial")]
    public class HistorialController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public HistorialController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historial>>> GetListHistorial()
        {
            var listaHistorial = await _context.hitorial.ToArrayAsync();
            return listaHistorial;
        }

        [HttpGet("{id:int}", Name = "GetHistorialById")]
        public async Task<ActionResult<Historial>> GetHistorialById(int id)
        {
            var historial = await _context.hitorial.FirstOrDefaultAsync(a => a.Id == id);
            if (historial == null)
            {
                return NotFound("Historial no encontrado");
            }

            return historial;
        }

        [HttpPost]
        public async Task<ActionResult> RegristrarHistorial([FromBody] Historial historial)
        {
            var existe = await _context.hitorial.AnyAsync(a => a.Fecha == historial.Fecha);
            if (existe)
            {
                return BadRequest($"Ya hay un Historial com esa feha: {historial.Fecha}");
            }
            _context.hitorial.Add(historial);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetHistorialById", new { id = historial.Id }, historial);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateHistorial([FromBody] Historial historial, int id)
        {
            if (historial.Id != id)
            {
                return BadRequest("No se encuentra es ID");
            }
            var existe = await _context.hitorial.AnyAsync(a => a.Fecha == historial.Fecha && a.Id != id);

            if (existe)
            {
                return BadRequest("El historial ya fue utilizado");
            }
            _context.hitorial.Update(historial);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteHistorial(int id)
        {
            var historial = await _context.hitorial.FindAsync(id);
            if (historial == null)
            {
                return NotFound("Ese historial no esta registrado");
            }
            _context.hitorial.Remove(historial);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpGet("{prefixText}")]
        public async Task<ActionResult<IEnumerable<Historial>>> BuscarHistorial(string prefixText)
        {
            var historial = await _context.hitorial.Where(a => a.Diagnostico.Contains(prefixText) ||
                                                               a.Tratamiento.Contains(prefixText)).ToListAsync();

            return historial;
        }

    }
}
