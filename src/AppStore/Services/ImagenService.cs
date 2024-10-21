using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using AppStore.Constants;

namespace AppStore.Services
{
    public class ImagenService : IImagenService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagenService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> GuardarImagen(IFormFile? imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                return ProductoConstants.RutaImagenPorDefecto;
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, ProductoConstants.CarpetaImagenes);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(fileStream);
            }

            return "/" + ProductoConstants.CarpetaImagenes + "/" + fileName;
        }

        public async Task EliminarImagen(string? rutaImagen)
        {
            if (string.IsNullOrEmpty(rutaImagen) || rutaImagen == ProductoConstants.RutaImagenPorDefecto)
            {
                return;
            }

            var rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath, rutaImagen.TrimStart('/'));

            if (File.Exists(rutaCompleta))
            {
                await Task.Run(() => File.Delete(rutaCompleta));
            }
        }
    }
}
