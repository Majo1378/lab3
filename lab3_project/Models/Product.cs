using System.ComponentModel.DataAnnotations;
public class Product
{
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    // Función ejemplo: calcular valor total de este producto (precio * stock)
    public decimal ValorTotal() => Precio * Stock;
}
