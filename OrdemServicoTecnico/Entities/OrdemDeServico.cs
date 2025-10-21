using System;
using System.Collections.Generic;
using System.Linq;
using OrdemServicoTecnico.Enums;
using OrdemServicoTecnico.ValueObjects;
using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.Entities
{
    public class OrdemDeServico
    {
        public int Id { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public DateTime? DataFechamento { get; private set; }
        public StatusOS Status { get; private set; }
        public Cliente Cliente { get; private set; }
        private List<ItemDeServico> _itens = new List<ItemDeServico>();
        public IReadOnlyCollection<ItemDeServico> Itens => _itens.AsReadOnly();

        public OrdemDeServico(int id, Cliente cliente)
        {
            if (cliente == null) 
                throw new InvalidEntityException("Cliente inválido");

            Id = id;
            Cliente = cliente;
            DataAbertura = DateTime.Now;
            Status = StatusOS.Aberta;
            cliente.AdicionarOrdem(this);
        }

        public void AdicionarItem(ItemDeServico item)
        {
            if (item == null)
                throw new InvalidEntityException("Item inválido");

            if (Status == StatusOS.Concluida || Status == StatusOS.Cancelada)
                throw new BusinessRuleException("Ordem finalizada não aceita novos itens");

            _itens.Add(item);
        }

        public Money CalcularTotal()
        {
            decimal total = _itens.Sum(item => item.Subtotal().Valor);
            return new Money(total);
        }

        // Métodos explícitos de transição de estado
        public void Iniciar()
        {
            EnsureNotFinalized();
            if (Status != StatusOS.Aberta)
                throw new BusinessRuleException("Só é possível iniciar uma ordem que esteja 'Aberta'");

            Status = StatusOS.EmAndamento;
        }

        public void Concluir()
        {
            EnsureNotFinalized();
            if (Status != StatusOS.EmAndamento)
                throw new BusinessRuleException("Só é possível concluir uma ordem que esteja 'EmAndamento'");

            Status = StatusOS.Concluida;
            DataFechamento = DateTime.Now;
        }

        public void Cancelar()
        {
            EnsureNotFinalized();
            // qualquer status exceto finalizados pode ser cancelado
            Status = StatusOS.Cancelada;
            DataFechamento = DateTime.Now;
        }

        private void EnsureNotFinalized()
        {
            if (Status == StatusOS.Concluida || Status == StatusOS.Cancelada)
                throw new BusinessRuleException("Ordem finalizada não pode ter status alterado");
        }

        // manter método privado caso seja necessário
        private void TransicionarStatus(StatusOS novoStatus)
        {
            if (Status == StatusOS.Concluida || Status == StatusOS.Cancelada)
                throw new BusinessRuleException("Ordem finalizada não pode ter status alterado");

            Status = novoStatus;
            if (novoStatus == StatusOS.Concluida || novoStatus == StatusOS.Cancelada)
                DataFechamento = DateTime.Now;
        }
    }
}