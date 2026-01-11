using Microsoft.EntityFrameworkCore;
using PracticaGestionInventario.Data;
using PracticaGestionInventario.Models;
using System.ComponentModel;
using System.Linq.Expressions;

namespace PracticaGestionInventario.Services;

public class EntradasService(IDbContextFactory <ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas.AnyAsync(e => e.EntradaId == entradaId);
    }
    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
    public async Task AfectarExistencia(EntradasDetalles[] entradaDetalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach(var item  in entradaDetalle)
        {
            var producto = await contexto.Productos.SingleAsync(p => p.ProductoId == item.ProductoId);
            if(tipoOperacion == TipoOperacion.Suma)
            {
                producto.Existencias += item.Cantidad;
            }
            else
            {
                producto.Existencias -= item.Cantidad;
            }
            await contexto.SaveChangesAsync();
        }
    }
    public async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Resta);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var original = await contexto.Entradas.Include(e => e.EntradasDetalles).AsNoTracking().SingleOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);
        if (original == null) return false;
        await AfectarExistencia(original.EntradasDetalles.ToArray(), TipoOperacion.Suma);
        contexto.EntradasDetalles.RemoveRange(original.EntradasDetalles);
        contexto.Update(entrada);
        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Resta);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Guardar(Entradas entrada)
    {
        if (!await Existe(entrada.EntradaId))
            return await Insertar(entrada);
        else
            return await Modificar(entrada);
    }
    public async Task<bool> Eliminar(int entradaid)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var entrada = await Buscar(entradaid);

        await AfectarExistencia(entrada.EntradasDetalles.ToArray(), TipoOperacion.Resta);
        contexto.EntradasDetalles.RemoveRange(entrada.EntradasDetalles);
        contexto.Entradas.Remove(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<Entradas?> Buscar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas.Include(e => e.EntradasDetalles).FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas.Include(e => e.EntradasDetalles).Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<List<Productos>> ListarProductos()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos.Where(p => p.ProductoId > 0).AsNoTracking().ToListAsync();
    }
}
