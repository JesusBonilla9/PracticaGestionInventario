using System.ComponentModel.DataAnnotations;

namespace PracticaGestionInventario.Models
{
    public class EntradasDetalles
    {
        [Key]
        public int Id { get; set; }
        public int EntradaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public double Costo { get; set; }
    }
}
