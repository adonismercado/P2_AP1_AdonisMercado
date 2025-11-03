using Microsoft.EntityFrameworkCore;
using P2_AP1_AdonisMercado.DAL;
using P2_AP1_AdonisMercado.Models;
using System.Linq.Expressions;

namespace P2_AP1_AdonisMercado.Services;
public class PedidosService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Pedidos pedido)
    {
        if (!await Existe(pedido.PedidoId))
        {
            return await Insertar(pedido);
        }
        else
        {
            return await Modificar(pedido);
        }
    }

    public async Task<bool> Existe(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos 
            .AnyAsync(r => r.PedidoId == pedidoId);
    }
    public async Task<bool> Insertar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Pedidos.Add(pedido);

        foreach (var detalle in pedido.PedidosDetalles)
        {
            var componente = await contexto.Componentes.FirstAsync(c => c.ComponenteId == detalle.ComponenteId);
            if (componente != null)
            {
                componente.Existencia += detalle.Cantidad;
            }
        }

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var anterior = await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.PedidoId == pedido.PedidoId);

        if (anterior == null)
        {
            return false;
        }

        foreach (var comp in await ListarComponentes())
        {

            int antes = anterior.PedidosDetalles
                .Where(p => p.ComponenteId == comp.ComponenteId)
                .Sum(p => p.Cantidad);

            int ahora = pedido.PedidosDetalles
                .Where(p => p.ComponenteId == comp.ComponenteId)
                .Sum(p => p.Cantidad);

            comp.Existencia += (ahora - antes);
        }

        contexto.Pedidos.Update(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Pedidos>> Listar(Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Where(criterio)
            .Include(d => d.PedidosDetalles)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pedidos?> Buscar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
    }
    public async Task<bool> Eliminar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var pedido = await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

        if (pedido == null)
        {
            return false;
        }

        foreach (var detalle in pedido.PedidosDetalles)
        {
            var componente = await contexto.Componentes.FindAsync(detalle.ComponenteId);
            if (componente != null)
            {
                componente.Existencia -= detalle.Cantidad;
            }
        }

        contexto.PedidosDetalles.RemoveRange(pedido.PedidosDetalles);
        contexto.Pedidos.Remove(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Componente>> ListarComponentes()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Componentes
            .AsNoTracking()
            .ToListAsync();
    }
}
