using System.ComponentModel.DataAnnotations;

namespace PracticaGestionInventario.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }
    [Required(ErrorMessage = "La descripcion es obligatoria")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage ="El costo es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
    public double Costo { get; set; }
    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; }
    [Required(ErrorMessage = "Las Existencias deben ser obligatorias")]
    public int Existencias { get; set; } = 0;
}
