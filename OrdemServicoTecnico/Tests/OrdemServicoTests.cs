using System;
using Xunit;
using OrdemServicoTecnico.Entities;
using OrdemServicoTecnico.ValueObjects;
using OrdemServicoTecnico.Enums;
using OrdemServicoTecnico.Exceptions;

namespace OrdemServicoTecnico.Tests
{
    public class OrdemServicoTests
    {
        [Fact]
        public void Deve_Criar_Ordem_Com_Status_Aberta()
        {
            var cliente = new Cliente(1, "João", new Email("joao@email.com"));
            var ordem = new OrdemDeServico(1, cliente);

            Assert.Equal(StatusOS.Aberta, ordem.Status);
            Assert.Contains(ordem, cliente.Ordens);
        }

        [Fact]
        public void Deve_Calcular_Total_Corretamente()
        {
            var ordem = new OrdemDeServico(1, new Cliente(1, "Maria", new Email("maria@email.com")));
            ordem.AdicionarItem(new ItemDeServico("Reparo", new Money(100), 2));

            Assert.Equal(200, ordem.CalcularTotal().Valor);
        }

        [Fact]
        public void Deve_Iniciar_E_Concluir_Ordem_Corretamente()
        {
            var ordem = new OrdemDeServico(1, new Cliente(1, "Ana", new Email("ana@email.com")));
            ordem.Iniciar();
            Assert.Equal(StatusOS.EmAndamento, ordem.Status);

            ordem.Concluir();
            Assert.Equal(StatusOS.Concluida, ordem.Status);
            Assert.True(ordem.DataFechamento.HasValue);
        }

        [Fact]
        public void Nao_Pode_Alterar_Status_Apos_Finalizar()
        {
            var ordem = new OrdemDeServico(1, new Cliente(1, "Paulo", new Email("paulo@email.com")));
            ordem.Iniciar();
            ordem.Concluir();

            var ex = Assert.Throws<BusinessRuleException>(() => ordem.Iniciar());
            Assert.Contains("finalizada", ex.Message);
        }

        [Fact]
        public void Nao_Pode_Adicionar_Item_Apos_Ordem_Finalizada()
        {
            var ordem = new OrdemDeServico(1, new Cliente(1, "Lucas", new Email("lucas@email.com")));
            ordem.Iniciar();
            ordem.Concluir();

            var ex = Assert.Throws<BusinessRuleException>(() => ordem.AdicionarItem(new ItemDeServico("Peça", new Money(10), 1)));
            Assert.Contains("finalizada", ex.Message);
        }

        [Fact]
        public void Cancelar_Define_DataFechamento()
        {
            var ordem = new OrdemDeServico(1, new Cliente(1, "Rita", new Email("rita@email.com")));
            ordem.Cancelar();

            Assert.Equal(StatusOS.Cancelada, ordem.Status);
            Assert.True(ordem.DataFechamento.HasValue);
        }
    }
}