using Microsoft.EntityFrameworkCore;
using P2_AP1_AdonisMercado.DAL;
using P2_AP1_AdonisMercado.Models;
using System.Linq.Expressions;

namespace P2_AP1_AdonisMercado.Services;
public class RegistrosService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Registros registro)
    {
        if (!await Existe(registro.RegistroId))
        {
            return await Insertar(registro);
        }
        else
        {
            return await Modificar(registro);
        }
    }

    public async Task<bool> Existe(int registroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registros
            .AnyAsync(r => r.RegistroId == registroId);
    }
    public async Task<bool> Insertar(Registros registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Registros.Add(registro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Registros registro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Registros.Update(registro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Registros>> Listar(Expression<Func<Registros, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
