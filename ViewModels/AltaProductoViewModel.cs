using System.ComponentModel.DataAnnotations;
namespace tp6.ViewModels;

public class AltaProductoViewModel
{
    private string _descripcion;
    private int _precio;

    public AltaProductoViewModel()
    {
        _descripcion = string.Empty;
        _precio = 0;
    }

    [StringLength(250, ErrorMessage = "El mínimo de longitud de caracteres es 1")]
    public string Descripcion {get => _descripcion; set => _descripcion = value; }


    [Required(ErrorMessage = "El campo precio es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public int Precio {get => _precio ; set => _precio = value;}
    
}