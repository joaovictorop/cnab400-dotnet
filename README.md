# 📄 CNAB400.NET

> Parser e gerador de arquivos CNAB 400 para .NET 8 — suporte a Itaú e Bradesco.

[![NuGet](https://img.shields.io/nuget/v/CNAB400.NET?style=flat-square)](https://www.nuget.org/packages/CNAB400.NET)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square)](https://dotnet.microsoft.com)

---

## 🇧🇷 O que é o CNAB 400?

CNAB 400 é o padrão de troca de arquivos bancários definido pela FEBRABAN, amplamente utilizado em sistemas financeiros brasileiros para registro e retorno de boletos, cobranças e pagamentos.

---

## ✨ Funcionalidades

- ✅ **Parse** de arquivos CNAB 400 → objetos .NET (remessa e retorno)
- ✅ **Geração** de arquivos CNAB 400 a partir de objetos .NET
- ✅ Suporte a **Itaú (341)** e **Bradesco (237)**
- ✅ Layouts: **Cobrança**, **Pagamento** e **Retorno Bancário**
- ✅ Validação de campos obrigatórios e tamanho de linha
- ✅ Cobertura de testes com **xUnit**

---

## 📦 Instalação

```bash
dotnet add package CNAB400.NET
```

---

## 🚀 Como usar

### Parse (Arquivo → Objeto)

```csharp
using CNAB400.NET;

var parser = new Cnab400Parser(BancoEnum.Itau);
var remessa = parser.Parse("caminho/para/arquivo.rem");

foreach (var detalhe in remessa.Detalhes)
{
    Console.WriteLine($"Nosso Número: {detalhe.NossoNumero}");
    Console.WriteLine($"Valor: {detalhe.Valor}");
    Console.WriteLine($"Vencimento: {detalhe.DataVencimento}");
}
```

### Geração (Objeto → Arquivo)

```csharp
using CNAB400.NET;

var remessa = new RemessaCnab400
{
    Header = new HeaderRemessa
    {
        CodigoBanco = "341",
        NomeEmpresa = "MINHA EMPRESA LTDA",
        DataGravacao = DateTime.Today
    },
    Detalhes = new List<DetalheRemessa>
    {
        new DetalheRemessa
        {
            NossoNumero = "000001",
            Valor = 150.00m,
            DataVencimento = DateTime.Today.AddDays(30),
            NomeSacado = "JOÃO DA SILVA",
            CpfCnpjSacado = "12345678901"
        }
    }
};

var gerador = new Cnab400Generator(BancoEnum.Itau);
gerador.GerarArquivo(remessa, "saida/remessa.rem");
```

---

## 🏦 Bancos suportados

| Banco | Código | Cobrança | Pagamento | Retorno |
|-------|--------|----------|-----------|---------|
| Itaú  | 341    | ✅       | ✅        | ✅      |
| Bradesco | 237 | ✅       | ✅        | ✅      |

---

## 🗂️ Estrutura do projeto

```
CNAB400.NET/
├── src/
│   └── CNAB400.NET/
│       ├── Bancos/
│       │   ├── Itau/
│       │   └── Bradesco/
│       ├── Models/
│       │   ├── Header.cs
│       │   ├── Detalhe.cs
│       │   └── Trailer.cs
│       ├── Parser/
│       │   └── Cnab400Parser.cs
│       └── Generator/
│           └── Cnab400Generator.cs
├── tests/
│   └── CNAB400.NET.Tests/
│       ├── ParserTests.cs
│       └── GeneratorTests.cs
├── samples/
│   └── CNAB400.NET.Sample/
│       └── Program.cs
└── README.md
```

---

## 🧪 Testes

```bash
dotnet test
```

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Abra uma issue ou envie um pull request.

1. Fork o projeto
2. Crie sua branch: `git checkout -b feature/novo-banco`
3. Commit: `git commit -m 'feat: adiciona suporte ao Banco do Brasil'`
4. Push: `git push origin feature/novo-banco`
5. Abra um Pull Request

---

## 📄 Licença

MIT © [João Victor Oliveira](https://www.linkedin.com/in/joao-vop)

---

## 👨‍💻 Autor

Desenvolvido por **João Victor Oliveira** — Desenvolvedor Full Stack especializado em sistemas financeiros e integrações bancárias.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-joao--vop-blue?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/joao-vop)
[![GitHub](https://img.shields.io/badge/GitHub-joaovop14-black?style=flat-square&logo=github)](https://github.com/joaovop14)
