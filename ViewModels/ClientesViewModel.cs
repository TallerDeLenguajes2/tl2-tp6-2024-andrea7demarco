using System.ComponentModel.DataAnnotations;

namespace tp6.ViewModels;

public class ClientesViewModel
{
    private string _nombre; // Es obligatorio
    private string _mail; // Validar el tipo mail
    private string _telefono; // Validar el tipo teléfono

    public ClientesViewModel()
    {
        _nombre = string.Empty;
        _mail = string.Empty;
        _telefono = string.Empty;
    }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Nombre
    {
        get => _nombre;
        set => _nombre = value;
    }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido.")]
    public string Mail
    {
        get => _mail;
        set => _mail = value;
    }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El formato del teléfono es inválido.")]
    public string Telefono
    {
        get => _telefono;
        set => _telefono = value;
    }
}


/*Atributos de validación
Estos son algunos de los atributos de validación integrados:
[ValidateNever]: ValidateNeverAttribute indica que una propiedad o parámetro debe excluirse de la
validación.
[Compare]: valida que dos propiedades de un modelo coinciden.
[EmailAddress]: valida que la propiedad tiene un formato de correo electrónico.
[Phone]: valida que la propiedad tiene un formato de número de teléfono.
[Range]: valida que el valor de propiedad se encuentra dentro de un intervalo especificado.
[RegularExpression]: valida que el valor de propiedad coincide con una expresión regular especificada.
[Required]: valida que el campo no es NULL.
[StringLength]: valida que un valor de propiedad de cadena no supera un límite de longitud especificado.
[Url]: valida que la propiedad tiene un formato de dirección URL.*/