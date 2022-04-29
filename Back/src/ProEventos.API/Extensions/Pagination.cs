using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ProEventos.API.models;

namespace ProEventos.API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, int paginaAtual, int totalDeItens, int itensPorPagina, int quantidadeDePaginas)
        {
            var pagination = new PaginationHeader(paginaAtual, itensPorPagina, totalDeItens, quantidadeDePaginas);
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));

            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}