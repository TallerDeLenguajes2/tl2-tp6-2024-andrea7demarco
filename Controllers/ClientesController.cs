using System.Diagnostics;
using Models;
using Microsoft.AspNetCore.Mvc;
using tp6.Models;
using System.Net.Cache;

namespace mvc.Controllers;

public class ClientesController : Controller
{

    private readonly ILogger<ClientesController> _logger;
    private List<Cliente> Clientes;
    private ClientesRepository cliRep;
    

    public ClientesController(ILogger<ClientesController> logger)
    {
        _logger = logger;
        cliRep = new ClientesRepository();
    }

    public IActionResult Index()
    {
        Clientes = cliRep.ListarCliente();
        return View(Clientes);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

   public IActionResult Agregar()
    {
        return View();
    }

    [HttpPost("api/GuardarCliente")]
    public IActionResult GuardarCliente(Cliente cliente)
    {
        cliRep.CrearCliente(cliente);
        return RedirectToAction("Index");
    }

    public IActionResult Modificar(int id)
    {
        var cliente = cliRep.BuscaClientePorID(id);
        return View(cliente); 
    }

    [HttpPost]
    public IActionResult ModificarCliente(Cliente cliente)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest();

            Cliente entidad = cliRep.BuscaClientePorID(cliente.IdCliente);
            entidad.Nombre = cliente.Nombre;
            entidad.Email = cliente.Email;
            entidad.Telefono = cliente.Telefono;
            cliRep.ModificarCliente(entidad.IdCliente, entidad);

            return RedirectToAction("Index");
        }
        catch(Exception)
        {
            return StatusCode(500, "No se pudo modificar el nombre del cliente");
        }
    }

/*    public IActionResult Eliminar(int id)
    {
        try
        {
            cliRep.EliminarClientePorID(id);
            return RedirectToAction("Index");
        }
        catch(Exception)
        {
            return StatusCode(500, "No se pudo eliminar el cliente");
        }

    }

*/

    







}
