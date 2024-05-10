namespace MasterDetail.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public required string Name { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public bool? Activo { get; set; } = false;

    }
}
