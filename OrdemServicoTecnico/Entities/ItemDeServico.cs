using OrdemServicoTecnico.ValueObjects;
using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.Entities
{
    public class ItemDeServico
    {
        public string Descricao { get; private set; }
        public Money PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }

        public ItemDeServico(string descricao, Money precoUnitario, int quantidade)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new InvalidEntityException("Descrição inválida");

            if (quantidade <= 0)
                throw new InvalidEntityException("Quantidade deve ser > 0");

            Descricao = descricao;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }

        public Money Subtotal()
        {
            return new Money(PrecoUnitario.Valor * Quantidade);
        }
    }
}