using ApiHospital.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ApiHospital.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiHospital.Controllers
{
    [ApiController]
    [Route("api/paciente/{pacienteId:int}/citas")]
    public class CitaController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public CitaController(AplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet("/api/citas")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetListCita()
        {
            var listaCitas = await _context.citas.Include(a => a.paciente).ToListAsync();

            return listaCitas;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetListCitaByPaciente(int pacienteId)
        {
            var existe = await _context.pacientes.AnyAsync(a => a.Id == pacienteId);
            if (!existe)
            {
                return NotFound($"No existe ninguna paciente con el id {pacienteId}");
            }

            var listaCitas = await _context.citas.Include(a => a.paciente).Where(a => a.PacienteID == pacienteId).ToListAsync();

            return listaCitas;
        }

       
        [HttpGet("/api/citas/{id:int}", Name = "GetCitaById")]
        public async Task<ActionResult<Cita>> GetCitaById(int id)
        {
            var cita = await _context.citas.Include(a => a.paciente).FirstOrDefaultAsync(a => a.Id == id);
            if (cita == null)
            {
                return NotFound($"No existe ninguna cita con el id {id}");
            }
            return cita;
        }

        [HttpPost]
        public async Task<ActionResult> RegistarCita([FromBody] Cita cita, int pacienteId)
        {
            var existe = await _context.pacientes.AnyAsync(a => a.Id == pacienteId);
            if (!existe)
            {
                return NotFound($"No existe ninguna cita con el id{pacienteId}");
            }

            existe = await _context.citas.Include(a => a.paciente).AnyAsync(a => a.Fecha == cita.Fecha);
            if (existe)
            {
                return BadRequest($"Ya existe una cita con la fecha{cita.Fecha}");
            }

            _context.citas.Add(cita);
            await _context.SaveChangesAsync();

            cita.paciente = _context.pacientes.Find(pacienteId);

            return CreatedAtRoute("GetCitaById", new { id = cita.Id }, cita);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCita([FromBody] Cita cita, int pacienteId, int id)
        {
            var existe = await _context.citas.AnyAsync(a => a.Id == pacienteId);
            if (!existe)
            {
                return NotFound($"No existe ningun paciente con el id {pacienteId}");
            }

            if (cita.Id != id)
            {
                return BadRequest("El id de la cita no coincide");
            }

            if (cita.PacienteID != pacienteId)
            {
                return BadRequest($"El paciente de la cita no coincide");
            }
            existe = await _context.citas.AnyAsync(a => a.Id == id);
            if (!existe)
            {
                return NotFound($"No existe ninguna cita con el id {id}");
            }

            existe = await _context.citas.AnyAsync(a => a.Id == id && a.PacienteID == pacienteId);
            if (!existe)
            {
                return NotFound("no se encontro la cita solicitada");
            }

            existe = await _context.citas.AnyAsync(a => a.Fecha == cita.Fecha && a.Id != id);
            if (existe)
            {
                return BadRequest("la fecha de la cita ya fue utilizada");
            }

            _context.citas.Update(cita);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("/api/citas/{id:int}")]
        public async Task<ActionResult> DeleteCita(int id)
        {
            var cita = await _context.citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound($"No existe ninguna cita con el id {id}");
            }

            _context.citas.Remove(cita);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[HttpGet("/api/citas/search/{prefixText}")]
        //public async Task<ActionResult<IEnumerable<Cita>>> busqueda(string prefixText)
        //{
        //    var cita = await _context.citas.Include(a => a.paciente).Where(a => a.PacienteID.Contains(prefixText)).ToListAsync();

        //    return cita;
        //}


    }
}
