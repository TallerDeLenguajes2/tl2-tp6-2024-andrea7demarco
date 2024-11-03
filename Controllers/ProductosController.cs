using System.Diagnostics;
using Models;
using Microsoft.AspNetCore.Mvc;
using tp6.Models;
using System.Net.Cache;

namespace mvc.Controllers;

public class ProductosController : Controller
{

    private readonly ILogger<ProductosController> _logger;
    private List<Producto> ?productos;
    private ProductosRepository prodRep;
    

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        prodRep = new ProductosRepository();
    }

    public IActionResult Index()
    {
        productos = prodRep.ListarProducto();
        return View(productos);
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

    [HttpPost("api/GuardarProducto")]
    public IActionResult GuardarProducto(Producto producto)
    {
        prodRep.CrearProducto(producto);
        return RedirectToAction("Index");
    }

    public IActionResult Modificar(int id)
    {
        var producto = prodRep.BuscaProductoPorID(id);
        return View(producto); 
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest();

            Producto entidad = prodRep.BuscaProductoPorID(producto.IdProducto);
            entidad.Descripcion = producto.Descripcion;
            entidad.Precio = producto.Precio;
            prodRep.ModificarProducto(entidad.IdProducto, entidad);

            return RedirectToAction("Index");
        }
        catch(Exception)
        {
            return StatusCode(500, "No se pudo modificar el nombre del producto");
        }
    }



    







}
