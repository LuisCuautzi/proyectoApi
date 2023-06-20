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
    [Route("api/hospital")]
    public class HospitalController: ControllerBase
    {
        private readonly AplicationDbContext _context;

        public HospitalController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetListHospital()
        {
            var listaHospital = await _context.hospital.ToArrayAsync();
            return listaHospital;
        }

        [HttpGet("{id:int}", Name = "GetHopitalById")]
        public async Task<ActionResult<Hospital>> GetHospitalById(int id)
        {
            var hospital = await _context.hospital.FirstOrDefaultAsync(a => a.Id == id);
            if (hospital == null)
            {
                return NotFound("Hospital no encontrado");
            }

            return hospital;
        }

        [HttpPost]
        public async Task<ActionResult> RegristrarHospital([FromBody] Hospital hospital)
        {
            var existe = await _context.hospital.AnyAsync(a => a.Nombre == hospital.Nombre);
            if (existe)
            {
                return BadRequest($"Ya hay un Hospital com ese Nombre: {hospital.Nombre}");
            }
            _context.hospital.Add(hospital);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetHospitalById", new { id = hospital.Id }, hospital);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateHospital([FromBody] Hospital hospital, int id)
        {
            if (hospital.Id != id)
            {
                return BadRequest("No se encuentra es ID");
            }
            var existe = await _context.hospital.AnyAsync(a => a.Nombre == hospital.Nombre && a.Id != id);

            if (existe)
            {
                return BadRequest("El hospital ya fue utilizado");
            }
            _context.hospital.Update(hospital);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteHospital(int id)
        {
            var hospital = await _context.hospital.FindAsync(id);
            if (hospital == null)
            {
                return NotFound("Ese hospital no esta registrado");
            }
            _context.hospital.Remove(hospital);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{prefixText}")]
        public async Task<ActionResult<IEnumerable<Hospital>>> BuscarHospital(string prefixText)
        {
            var hospital = await _context.hospital.Where(a => a.Nombre.Contains(prefixText) ||
                                                               a.Direccion.Contains(prefixText) ||
                                                               a.Telefono.Contains(prefixText)).ToListAsync();

            return hospital;
        }
    }
}
