using System.ComponentModel.DataAnnotations;

namespace P2_AP1_AdonisMercado.Models;

public class PedidosDetalle
{
    [Key]
    public int DetalleId { get; set; }
    public int PedidoId { get; set; }
    public int ComponenteId { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}
