using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Producto
{
    [Key]
    public int IDProducto { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    public string? Nombre { get; set; }
    
    public string? UnidadMedida { get; set; }
    
    [Required(ErrorMessage = "El contenido neto es requerido")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContenidoNeto { get; set; }
    
    [Required(ErrorMessage = "El precio es requerido")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }
    
    [Required(ErrorMessage = "El stock es requerido")]
    public int Stock { get; set; }
    
    public string? Imagen { get; set; }
}
