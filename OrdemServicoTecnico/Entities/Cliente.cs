using System.Collections.Generic;
using System.Linq;
using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.Entities
{
    public class Cliente
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public List<OrdemDeServico> Ordens { get; private set; } = new List<OrdemDeServico>();

        public Cliente(int id, string nome, Email email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new InvalidEntityException("Nome inválido");

            Id = id;
            Nome = nome;
            Email = email;
        }

        public void AdicionarOrdem(OrdemDeServico ordem)
        {
            if (ordem == null)
                throw new InvalidEntityException("Ordem inválida");

            if (Ordens.Any(o => o.Id == ordem.Id))
                return; // evitar duplicatas silently

            Ordens.Add(ordem);
        }
    }
}