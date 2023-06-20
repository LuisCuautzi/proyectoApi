//using AutoMapper;
using ApiHospital.Data;
//using ApiHospital.DTOs;
using ApiHospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHospital.Controllers
{
    [ApiController]
    [Route("api/medico")]
    public class MedicoController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        //private readonly IMapper mapper;
        public MedicoController(AplicationDbContext context)
        {
            _context = context;
            //this.mapper = mapper;
        }

        //obtener la lista de aerolineas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetListMedico()
        {
            var listaMedico = await _context.medicos.ToListAsync();
            return listaMedico;
        }

        //ontener medico por id
        [HttpGet("{id:int}", Name = "GetMedicoById")]
        public async Task<ActionResult<Medico>> GetMedicoById(int id)
        {
            var medico = await _context.medicos.FirstOrDefaultAsync(a => a.Id == id);
            if(medico == null)
            {
                return NotFound("Medico no encontrado");
            }
            return medico;
        }

        //regristrar medico
        [HttpPost]
        public async Task<ActionResult> RegistrarMedico([FromBody] Medico medico)
        {
            var existe = await _context.medicos.AnyAsync(a => a.Name == medico.Name);
            if (existe)
            {
                return BadRequest($"Ya hay un aerolinea com ese nombre: {medico.Name}");
            }
            //Proceso para convertir el DTO en el modelo
            //var aerolineaObj = mapper.Map<Medico>(medico);


            _context.medicos.Add(medico);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetMedicoById", new { id = medico.Id }, medico);
        }


        //actualizar medico
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMedico([FromBody] Medico medico, int id)
        {
            if (medico.Id != id)
            {
                return BadRequest("No se encuentra es ID");
            }
            var existe = await _context.medicos.AnyAsync(a => a.Name == medico.Name && a.Id != id);

            if (existe)
            {
                return BadRequest("El nombre del medico ya fue utilizado");
            }
            _context.medicos.Update(medico);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Eliminar medico
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMedico(int id)
        {
            var medico = await _context.medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound("Esa aerolinea no esta registrada");
            }
            _context.medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Buscar medico po nombre, por apellido, por especialidad
        [HttpGet("{prefixText}")]
        public async Task<ActionResult<IEnumerable<Medico>>> BuscarAerolines(string prefixText)
        {
            var medico = await _context.medicos.Where(a => a.Name.Contains(prefixText) ||
                                                                  a.Apellido.Contains(prefixText) ||
                                                                  a.Especialidad.Contains(prefixText)).ToListAsync();

            return medico;
        }
    }
}
