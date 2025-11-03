using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2_AP1_AdonisMercado.Models;

public class Componente
{
    [Key]
    public int ComponenteId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Existencia { get; set; }

    [InverseProperty("Componentes")]
    public virtual ICollection<PedidosDetalle> PedidosDetalles { get; set; } = new List<PedidosDetalle>();
}
