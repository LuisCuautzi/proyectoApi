using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiHospital.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El ID es Requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Nombre es Requerido")]
        public string Apellido { get; set; }
        public DateTime FecNac { get; set; }
        public string Genero { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public List<Cita> citas { get; set; }
    }
}
