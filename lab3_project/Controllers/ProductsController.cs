using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context) { _context = context; }

    // Index: búsqueda (uso de variable, condicional, lista, operador)
    public async Task<IActionResult> Index(string search)
    {
        var query = _context.Products.AsQueryable(); // variable y lista
        if (!string.IsNullOrEmpty(search)) // condicional
        {
            query = query.Where(p => p.Nombre.Contains(search)); // operador y lambda
        }
        var list = await query.ToListAsync(); // bucle implícito retornado por EF
        return View(list);
    }

    // Edit: GET
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id); // variable
        if (product == null) return NotFound(); // condicional
        return View(product);
    }

    // Edit: POST - ejemplo de uso de operadores y funciones auxiliares
    [HttpPost]
    public async Task<IActionResult> Edit(Product model)
    {
        if (!ModelState.IsValid) return View(model); // condicional
        // Operador: actualizar valores
        var prod = await _context.Products.FindAsync(model.Id);
        if (prod == null) return NotFound();
        prod.Precio = model.Precio;
        prod.Stock = model.Stock;
        // Función: recalcular o acciones adicionales
        var valor = prod.ValorTotal(); // llamada a función en modelo
        _context.Products.Update(prod);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Delete: GET
    public async Task<IActionResult> Delete(int id)
    {
        var prod = await _context.Products.FindAsync(id);
        if (prod == null) return NotFound();
        return View(prod);
    }

    // Delete Confirmed: POST
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prod = await _context.Products.FindAsync(id);
        if (prod != null)
        {
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Estadisticas: LINQ avanzado + bucles y funciones
    public IActionResult Estadisticas()
    {
        var productos = _context.Products.ToList(); // lista
        // Reporte de precios: ordenar (operador)
        var ordenados = productos.OrderByDescending(p => p.Precio).ToList();
        // Promedio de precios (uso de función de agregación)
        decimal promedio = 0;
        if (productos.Count > 0)
        {
            promedio = productos.Average(p => p.Precio);
        }
        // Valor total inventario (bucle y operador)
        decimal valorTotal = 0;
        foreach (var p in productos)
        {
            valorTotal += p.Precio * p.Stock;
        }
        // Stock critico (condicional y lista)
        var criticos = productos.Where(p => p.Stock < 5).ToList();

        var vm = new EstadisticasVM {
            ProductosOrdenados = ordenados,
            PromedioPrecio = promedio,
            ValorInventario = valorTotal,
            StockCritico = criticos
        };
        return View(vm);
    }
}
