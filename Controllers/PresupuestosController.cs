using System.Diagnostics;
using Models;
using Microsoft.AspNetCore.Mvc;
using tp6.Models;
using System.Net.Cache;

namespace mvc.Controllers;

public class PresupuestosController : Controller
{

    private readonly ILogger<PresupuestosController> _logger;
    private List<Presupuesto>? presupuestos;
    private PresupuestoRepository presRep;


    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        presRep = new PresupuestoRepository();
    }

    public IActionResult Index()
    {
        presupuestos = presRep.ListarPresupuesto();
        return View(presupuestos);
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



    public IActionResult Detalle(int id)
    {
        List<PresupuestoDetalle> detalles = presRep.ObtenerPresupuestoDetalle(id);

        if (detalles == null)
        {
            detalles = new List<PresupuestoDetalle>();
        }

        return View(detalles);
    }

    public IActionResult Agregar()
    {
        return View(); 
    }

    [HttpPost("api/GuardarPresupuesto")]
    public IActionResult GuardarPresupuesto(Presupuesto presupuesto)
    {
        presRep.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            presRep.EliminarPresupuesto(id);
            return RedirectToAction("Index");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"No se pudo eliminar el presupuesto: {ex.Message}");
        }

    }
    public IActionResult Modificar(int id)
    {
        var presupuesto = presRep.BuscarPresupuestoPorID(id);
        return View(presupuesto); 
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest();

            Presupuesto entidad = presRep.BuscarPresupuestoPorID(presupuesto.IdPresupuesto);
            entidad.FechaCreacion = presupuesto.FechaCreacion;
            presRep.ModificarPresupuesto(entidad.IdPresupuesto, entidad);

            return RedirectToAction("Index");
        }
        catch(Exception)
        {
            return StatusCode(500, "No se pudo modificar el presupuesto");
        }
    }


}
