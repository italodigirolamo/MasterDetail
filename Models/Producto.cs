using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MasterDetail.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        [Precision(16, 2)]
        public decimal Precio { get; set; }
    }
}
