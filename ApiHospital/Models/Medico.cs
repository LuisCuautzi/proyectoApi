using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace ApiHospital.Models
{
    public class Medico
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El ID es Requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El Nombre es Requerido")]
        public string Apellido { get; set; }
        public string Celular { get; set; }
        public string Especialidad { get; set; }
        public Estatus Status { get; set; }
    }
}
