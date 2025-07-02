using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PathbitChallenge.Application.Interfaces;

namespace PathbitChallenge.Infrastructure.Services;

public class CepRapidService : ICepService
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly ILogger<CepRapidService> _logger;

  public CepRapidService(IHttpClientFactory httpClientFactory, ILogger<CepRapidService> logger)
  {
    _httpClientFactory = httpClientFactory;
    _logger = logger;
  }

  public async Task<CepApiResponse?> GetAddressByCepAsync(string cep)
  {
    var client = _httpClientFactory.CreateClient("CepRapid");
    try
    {
      // A API ceprapido.com retorna um objeto JSON com as propriedades que precisamos
      var response = await client.GetFromJsonAsync<CepApiResponse>($"cep/{cep}");
      return response;
    }
    catch (HttpRequestException ex)
    {
      // Captura erros de rede ou status codes como 404 (Não Encontrado)
      _logger.LogWarning(ex, "Falha ao buscar CEP {Cep}. Status Code: {StatusCode}", cep, ex.StatusCode);
      return null;
    }
  }
}
