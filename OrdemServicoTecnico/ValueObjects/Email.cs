using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.ValueObjects
{
    public record Email
    {
        public string Valor { get; init; }

        public Email(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || !valor.Contains("@"))
                throw new InvalidEntityException("Email inválido");

            Valor = valor;
        }
    }
}