using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterDetail.Models
{
    public class Configuracion
    {
        public int Id { get; set; }
        public required string Identificador { get; set; }

        [Required, MaxLength(100)]
        [DisplayName("Nombre Empresa")]
        public required string NombreEmpresa { get; set; }
        public required string Direccion { get; set;}
        public required string Telefonos { get; set;}
        public required string Correo { get; set; }
        public required string Logo { get; set; }

    }
}
