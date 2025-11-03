using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace P2_AP1_AdonisMercado.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string NombreCliente { get; set; }
    public decimal Total { get; set; }

    [InverseProperty("Pedidos")]
    public virtual ICollection<PedidosDetalle> PedidosDetalles { get; set; } = new List<PedidosDetalle>();
}
