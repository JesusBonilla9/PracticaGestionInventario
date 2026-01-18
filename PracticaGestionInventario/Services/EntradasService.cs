using Microsoft.EntityFrameworkCore;
using PracticaGestionInventario.Data;
using PracticaGestionInventario.Models;
using System.Linq.Expressions;

namespace PracticaGestionInventario.Services;

public class EntradasService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
    public async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        return await contexto.Entradas.AnyAsync(e => e.EntradaId == entradaId);
    }

    public async Task AfectarExistencia(EntradasDetalles[] detalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        foreach (var item in detalle)
        {
            var producto = await contexto.Productos
                .SingleAsync(p => p.ProductoId == item.ProductoId);

            if (tipoOperacion == TipoOperacion.Suma)
                producto.Existencias += item.Cantidad;
            else
                producto.Existencias -= item.Cantidad;

            await contexto.SaveChangesAsync();
        }
    }

    public async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        var original = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (original == null)
            return false;

        await AfectarExistencia(original.EntradasDetalles.ToArray(), TipoOperacion.Resta);
        contexto.EntradasDetalles.RemoveRange(original.EntradasDetalles);
        contexto.Update(entrada);

        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Suma);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Entradas entrada)
    {
        if (!await Existe(entrada.EntradaId))
            return await Insertar(entrada);
        else
            return await Modificar(entrada);
    }

    public async Task<Entradas?> Buscar(int entradaId)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        var entrada = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .SingleOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entrada == null)
            return false;

        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Resta);
        contexto.EntradasDetalles.RemoveRange(entrada.EntradasDetalles);
        contexto.Entradas.Remove(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}



