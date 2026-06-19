// Services/ProductoService.cs
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApiBlazor.Models;


namespace WebApiBlazor.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        // Lista en memoria, podemos ver los cambios incluso en una api fake como es esta
        public List<Producto> ProductosTraidos { get; private set; } = new();

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        // GET  (con los limites por cantidad agregados para el paginado)
        public async Task<List<Producto>> GetProductosAsync(int offset = 0, int limit = 10)
        {
            var resultado = await _http.GetFromJsonAsync<List<Producto>>(
                $"products?offset={offset}&limit={limit}"
            ) ?? new List<Producto>();

            ProductosTraidos = resultado;
            return ProductosTraidos;
        }

        // GET 
        public async Task<Producto?> GetProductoPorIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<Producto>($"products/{id}");
            }
            catch
            {
                return null;
            }
        }

        // POST 
        public async Task<Producto?> CrearProductoAsync(Producto nuevo)
        {
            var response = await _http.PostAsJsonAsync("products", new
            {
                title = nuevo.Title,
                price = nuevo.Price,
                description = nuevo.Description,
                categoryId = nuevo.Category?.Id ?? 1,
                images = nuevo.Images ?? new List<string> { "https://placehold.co/600x400" }
            });

            if (response.IsSuccessStatusCode)
            {
                var creado = await response.Content.ReadFromJsonAsync<Producto>();
                if (creado != null)
                    ProductosTraidos.Insert(0, creado); // Arriba de todo para que se note el cambio
                return creado;
            }
            return null;
        }

        // PUT
        public async Task<Producto?> EditarProductoAsync(Producto editado)
        {
            var response = await _http.PutAsJsonAsync($"products/{editado.Id}", new
            {
                title = editado.Title,
                price = editado.Price,
                description = editado.Description,
                images = editado.Images ?? new List<string> { "https://placehold.co/600x400" },
                categoryId = editado.Category?.Id ?? 1
            });

            if (response.IsSuccessStatusCode)
            {
                var idx = ProductosTraidos.FindIndex(p => p.Id == editado.Id);
                if (idx >= 0)
                    ProductosTraidos[idx] = editado;
                return editado;
            }
            return null;
        }

        // DELETE
        public async Task<bool> EliminarProductoAsync(int id)
        {
            var response = await _http.DeleteAsync($"products/{id}");
            if (response.IsSuccessStatusCode)
            {
                ProductosTraidos.RemoveAll(p => p.Id == id);
                return true;
            }
            return false;
        }
    }
}