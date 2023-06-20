using System.ComponentModel.DataAnnotations;

namespace ApiHospital.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El ID es Requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Nombre es Requerido")]
        public string Direccion { get; set; }
        public string Telefono { get; set; }

    }
}
