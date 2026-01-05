using System.ComponentModel.DataAnnotations;

namespace PracticaGestionInventario.Models
{
    public class EntradasDetalles
    {
        [Key]
        public int Id { get; set; }
        public int EntradaId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
    }
}
