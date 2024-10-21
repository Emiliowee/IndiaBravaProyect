using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AppStore.Services
{
    public interface IImagenService
    {
        Task<string> GuardarImagen(IFormFile? imagen);
        Task EliminarImagen(string? rutaImagen);
    }
}
