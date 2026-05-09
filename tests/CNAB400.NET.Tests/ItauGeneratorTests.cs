using CNAB400.NET.Bancos.Itau;
using CNAB400.NET.Models;
using FluentAssertions;

namespace CNAB400.NET.Tests;

public class ItauGeneratorTests
{
    [Fact]
    public void Generate_HeaderCorreto_PosicaoNomeEmpresa()
    {
        var generator = new ItauCnab400Generator();
        var remessa = BuildRemessa();

        var lines = generator.GenerateLines(remessa).ToList();
        var header = lines[0];

        header.Substring(26, 20).Should().Be("EMPRESA TESTE LTDA  ");
    }

    [Fact]
    public void Generate_DetalheCorreto_PosicaoValor()
    {
        var generator = new ItauCnab400Generator();
        var remessa = BuildRemessa();

        var lines = generator.GenerateLines(remessa).ToList();
        var detalhe = lines[1];

        detalhe.Substring(126, 13).Should().Be("0000000123456");
    }

    [Fact]
    public void Generate_DetalheCorreto_PosicaoDataVencimento()
    {
        var generator = new ItauCnab400Generator();
        var remessa = BuildRemessa();

        var lines = generator.GenerateLines(remessa).ToList();
        var detalhe = lines[1];

        detalhe.Substring(120, 6).Should().Be("300426");
    }

    [Fact]
    public void Generate_LinhasGeradas_TodaTemExatamente400Chars()
    {
        var generator = new ItauCnab400Generator();
        var remessa = BuildRemessa();

        var lines = generator.GenerateLines(remessa).ToList();

        lines.Should().NotBeEmpty();
        lines.Should().OnlyContain(line => line.Length == 400);
    }

    [Fact]
    public void Generate_ParseRoundTrip_RemessaIgualAposGerarEParsear()
    {
        var generator = new ItauCnab400Generator();
        var parser = new ItauCnab400Parser();
        var remessa = BuildRemessa();
        var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.rem");

        try
        {
            generator.GenerateFile(remessa, tempFilePath);
            var parsed = parser.Parse(tempFilePath);

            parsed.Header.CodigoBanco.Should().Be(remessa.Header.CodigoBanco);
            parsed.Header.NomeEmpresa.Should().Be(remessa.Header.NomeEmpresa);
            parsed.Header.DataGravacao.Should().Be(remessa.Header.DataGravacao);
            parsed.Header.NumeroSequencial.Should().Be(remessa.Header.NumeroSequencial);

            parsed.Detalhes.Should().HaveCount(remessa.Detalhes.Count);
            parsed.Detalhes[0].NossoNumero.Should().Be(remessa.Detalhes[0].NossoNumero);
            parsed.Detalhes[0].Valor.Should().Be(remessa.Detalhes[0].Valor);
            parsed.Detalhes[0].DataVencimento.Should().Be(remessa.Detalhes[0].DataVencimento);
            parsed.Detalhes[0].NomeSacado.Should().Be(remessa.Detalhes[0].NomeSacado);
            parsed.Detalhes[0].CpfCnpjSacado.Should().Be(remessa.Detalhes[0].CpfCnpjSacado);
            parsed.Detalhes[0].Instrucao1.Should().Be(remessa.Detalhes[0].Instrucao1);
            parsed.Detalhes[0].Instrucao2.Should().Be(remessa.Detalhes[0].Instrucao2);
            parsed.Detalhes[0].NumeroSequencial.Should().Be(remessa.Detalhes[0].NumeroSequencial);

            parsed.Trailer.QuantidadeTitulos.Should().Be(remessa.Trailer.QuantidadeTitulos);
            parsed.Trailer.ValorTotal.Should().Be(remessa.Trailer.ValorTotal);
            parsed.Trailer.NumeroSequencial.Should().Be(remessa.Trailer.NumeroSequencial);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    private static RemessaCnab400 BuildRemessa()
    {
        return new RemessaCnab400
        {
            Header = new HeaderRemessa
            {
                CodigoBanco = 341,
                NomeEmpresa = "EMPRESA TESTE LTDA",
                DataGravacao = new DateTime(2026, 4, 30),
                NumeroSequencial = 1,
            },
            Detalhes =
            [
                new DetalheRemessa
                {
                    NossoNumero = "123456789012",
                    Valor = 1234.56m,
                    DataVencimento = new DateTime(2026, 4, 30),
                    NomeSacado = "CLIENTE TESTE",
                    CpfCnpjSacado = "123456789012345",
                    Instrucao1 = "INST01",
                    Instrucao2 = "INST02",
                    NumeroSequencial = 2,
                },
            ],
            Trailer = new TrailerRemessa
            {
                QuantidadeTitulos = 1,
                ValorTotal = 1234.56m,
                NumeroSequencial = 3,
            },
        };
    }
}
