using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models;
using AppStore.Constants;
using AppStore.Services;
using Microsoft.AspNetCore.Http;

namespace AppStore.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IndiaBravaContext _context;
        private readonly IImagenService _imagenService;

        public ProductosController(IndiaBravaContext context, IImagenService imagenService)
        {
            _context = context;
            _imagenService = imagenService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Producto producto, IFormFile? imagen)
        {
            if (!ModelState.IsValid || !EsProductoValido(producto))
            {
                return CrearRespuestaError(ProductoConstants.ErrorValoresInvalidos);
            }
            try
            {
                producto.Imagen = await _imagenService.GuardarImagen(imagen);
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return CrearRespuestaError(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Json(new
            {
                idProducto = producto.IDProducto,
                nombre = producto.Nombre,
                precio = producto.Precio,
                contenidoNeto = producto.ContenidoNeto,
                unidadMedida = producto.UnidadMedida,
                stock = producto.Stock,
                imagen = producto.Imagen
            });
        }

        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Producto producto, IFormFile? imagen)
        {
            if (!ModelState.IsValid || !EsProductoValido(producto))
            {
                return CrearRespuestaError(ProductoConstants.ErrorValoresInvalidos);
            }

            try
            {
                var productoExistente = await _context.Productos.FindAsync(producto.IDProducto);
                if (productoExistente == null)
                {
                    return CrearRespuestaError(ProductoConstants.ErrorProductoNoEncontrado);
                }

                if (imagen != null)
                {
                    await _imagenService.EliminarImagen(productoExistente.Imagen);
                    producto.Imagen = await _imagenService.GuardarImagen(imagen);
                }
                else
                {
                    producto.Imagen = productoExistente.Imagen;
                }

                _context.Entry(productoExistente).CurrentValues.SetValues(producto);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return CrearRespuestaError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return CrearRespuestaError(ProductoConstants.ErrorProductoNoEncontrado);
            }

            try
            {
                await _imagenService.EliminarImagen(producto.Imagen);
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return CrearRespuestaError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarStock(int productoId, int cantidadStock)
        {
            if (cantidadStock <= 0)
            {
                return Json(new { success = false, error = ProductoConstants.ErrorCantidadStockPositiva });
            }

            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null)
            {
                return Json(new { success = false, error = ProductoConstants.ErrorProductoNoEncontrado });
            }

            try
            {
                producto.Stock += cantidadStock;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerarReporte(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                var productos = await _context.Productos.ToListAsync();
                // Proximamenteeee
                return Json(new { success = true, message = "Reporte generado con éxito", cantidadProductos = productos.Count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private bool EsProductoValido(Producto producto)
        {
            return producto.Precio > 0 && producto.Stock >= 0 && producto.ContenidoNeto > 0;
        }
        private IActionResult CrearRespuestaError(string error = ProductoConstants.ErrorModeloInvalido)
        {
            var errores = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            if (!errores.Any())
            {
                errores.Add(error);
            }
            return Json(new { success = false, error, modelErrors = errores });
        }
    }
}
