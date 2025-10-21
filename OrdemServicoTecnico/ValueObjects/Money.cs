using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.ValueObjects
{
    public readonly record struct Money
    {
        public decimal Valor { get; init; }

        public Money(decimal valor)
        {
            if (valor < 0)
                throw new InvalidEntityException("Valor não pode ser negativo");

            Valor = valor;
        }
    }
}