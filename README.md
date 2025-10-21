# atividade-ordem-servico

Uma atividade de Programação Orientada a Objetos (POO) que implementa um sistema de Ordem de Serviço utilizando a abordagem TDD (Test-Driven Development).

Este repositório contém uma solução em .NET com domínio, value objects, enums e testes unitários que exemplificam princípios de modelagem orientada a objetos.

## Principais pontos

- Linguagem: C# (.NET)
- Estilo: Programação Orientada a Objetos, Domain-Driven Design simples (Entities, Value Objects, Enums)
- Abordagem: Desenvolvimento guiado por testes (TDD)

Rodando os testes (PowerShell / Windows)

Abra o PowerShell e execute:

```powershell
cd C:\Repositorios\DotNet\atividade-ordem-servico\OrdemServicoTecnico
dotnet restore
dotnet build
dotnet test
```

Se preferir executar apenas os testes sem recompilar (quando já tiver build recente):

```powershell
cd C:\Repositorios\DotNet\atividade-ordem-servico\OrdemServicoTecnico
dotnet test --no-build
```

## Estrutura do projeto

- `OrdemServicoTecnico/` - projeto principal
	- `Entities/` - Entidades do domínio (Cliente, OrdemDeServico, ItemDeServico)
	- `ValueObjects/` - Objetos de valor (Email, Money)
	- `Enums/` - Enums do domínio (StatusOS)
	- `Tests/` - Testes unitários (TDD)

### Vitrine / Como ler o projeto

- Comece por `Entities/OrdemDeServico.cs` para entender as operações de negócio.
- Veja `ValueObjects/Email.cs` e `ValueObjects/Money.cs` para exemplos de validação e representação de valores.
- `Tests/OrdemServicoTests.cs` mostra exemplos de como o comportamento esperado foi especificado via testes.

## Reflexão sobre o domínio

### Reflexão sobre o projeto 'OrdemServicoTecnico'

1) Como records/structs melhoram a imutabilidade e clareza do código

Neste projeto há exemplos claros de Value Objects em `ValueObjects/Email.cs` e `ValueObjects/Money.cs` e entidades em `Entities/Cliente.cs`, `Entities/OrdemDeServico.cs` e `Entities/ItemDeServico.cs`.

Usar `record` (ou `readonly struct` para tipos primitivos de valor quando adequado) melhora a imutabilidade porque o compilador trata as instâncias como valores imutáveis por padrão, entregando várias vantagens:
- Segurança contra mutação acidental: propriedades não-mutable e cópia por valor reduzem efeitos colaterais entre camadas.
- Igualdade por valor automática: records implementam Equals/GetHashCode com base nas propriedades, facilitando comparações em testes e coleções.
- Sintaxe concisa e expressiva: declarações curtas tornam as intenções de design mais claras (ex.: "isso é um Value Object").

No contexto deste repositório, transformar `Email` e `Money` em `record` (se ainda não forem) reforça a intenção de imutabilidade e melhora a leitura dos testes em `Tests/OrdemServicoTests.cs`.

2) Vantagens dos enums para representar estados finitos

O enum `Enums/StatusOS.cs` é uma escolha apropriada para representar os estados finitos de uma ordem de serviço (ex.: Aberto, EmAndamento, Concluido). Vantagens:
- Clareza semântica: expressa os estados possíveis de forma legível ao invés de usar strings ou números mágicos.
- Segurança de tipo: evita valores inválidos em tempo de compilação; facilita switch/case exaustivo.
- Facilita serialização/visualização: ao mapear para UI ou logs, os nomes dos enums são explícitos.

Recomendação prática: quando as transições de estado têm regras de negócio (por exemplo: só é possível ir de Aberto -> EmAndamento, mas não de Concluido -> EmAndamento), centralize essa lógica na entidade `OrdemDeServico` como métodos (Ex.: `Iniciar()`, `Concluir()`) que validem o `StatusOS` antes de aplicar mudanças.

3) Impacto na testabilidade e manutenção

- Testabilidade: Imutabilidade e Value Objects tornam os testes determinísticos: criar instâncias conhecidas e compará-las por valor é direto (menor necessidade de builders complexos).
- Isolamento: enums e records evitam mocks complexos — você pode instanciar valores concretos em testes sem setar comportamento interno.
- Manutenção: mudanças locais em um record/ValueObject têm menos impacto colateral. Além disso, usar enums para estados melhora a legibilidade ao revisar histórico (git) e reduz bugs relacionados a estados inválidos.

No repositório, `Tests/OrdemServicoTests.cs` pode se beneficiar de asserts que comparam objetos por valor (se `Email`/`Money` forem records), reduzindo verificação manual de campos.

4) Importância no design do domínio

Modelar o domínio com Value Objects, Entities e Enums melhora a Ubiquitous Language do sistema. Exemplos concretos no projeto:
- `Entities/OrdemDeServico.cs` deve encapsular regras de negócio e expor operações de domínio (não apenas setters públicos). Isso garante invariantes (por exemplo: total da ordem calculado a partir dos `ItemDeServico`).
- ValueObjects (`Email`, `Money`) devem validar e representar corretamente as regras (formato de email, unidades monetárias). Torná-los imutáveis evita estados inválidos.

Ao tratar o design do domínio como primeiro cidadão, o código se torna mais expressivo, fácil de testar e menos propenso a bugs oriundos de lógica espalhada por camadas de aplicação.

Pequenas observações e recomendações (baixo risco):
- Transformar objetos simples que representam valores (como `Email`, `Money`) em `record` se ainda não forem. Use `readonly record struct` para otimização se for crítico em desempenho e se as semantics permitirem.
- Tornar propriedades apenas leitura (init-only ou get-only) em entidades quando possível e controlar mutações por métodos de domínio.
- Implementar métodos de transição de estado em `OrdemDeServico` (ex.: `Iniciar`, `Concluir`) que encapsulem validações em vez de permitir atribuição direta de `StatusOS`.
- Adicionar mais asserts por valor nos testes (ex.: Assert.Equal(expectedRecord, actualRecord)).

## Conclusão

O projeto já demonstra a separação entre domínio e testes. Aplicando pequenas melhorias de imutabilidade e encapsulamento de regras (uso adequado de records/enums e métodos de domínio) a testabilidade, clareza e manutenção irão melhorar ainda mais.

Quality gates

- Build: PASS (compilou com warnings)
- Tests: PASS (nenhum teste executado com falha durante esta verificação)

Próximos passos sugeridos

- Opcional: aplicar as mudanças sugeridas nos ValueObjects e adicionar 1-2 testes que demonstrem comparação por valor.
- Documentar regras de transição de `StatusOS` dentro de `OrdemDeServico`.