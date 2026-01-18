using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaGestionInventario.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }
    [Required(ErrorMessage ="La fecha es obligatoria")]
    public DateTime Fecha {  get; set; } = DateTime.Now;
    [Required(ErrorMessage = "El concepto es obligatorio")]
    public string Concepto { get; set; }
    public double Total { get; set; }

    [ForeignKey(nameof(EntradaId))]
    public virtual ICollection<EntradasDetalles> EntradasDetalles { get; set; } = new List<EntradasDetalles>();
}
