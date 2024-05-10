using System.ComponentModel.DataAnnotations;

namespace MasterDetail.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        [Required]
        public string Nombre { get; set; } = null!;
    }
}
