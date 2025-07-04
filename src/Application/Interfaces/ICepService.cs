namespace PathbitChallenge.Application.Interfaces;

// Usamos record para respostas imutáveis simples
public record CepApiResponse(string Endereco, string Bairro, string Cidade, string Estado);

public interface ICepService
{
  Task<CepApiResponse?> GetAddressByCepAsync(string cep);
}
