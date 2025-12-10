using System.Collections.Generic;
public class EstadisticasVM
{
    public List<Product> ProductosOrdenados { get; set; }
    public decimal PromedioPrecio { get; set; }
    public decimal ValorInventario { get; set; }
    public List<Product> StockCritico { get; set; }
}
