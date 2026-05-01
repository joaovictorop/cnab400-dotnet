using CNAB400.NET.Bancos.Itau;
using FluentAssertions;

namespace CNAB400.NET.Tests;

public class ItauParserTests
{
    [Fact]
    public void Parse_HeaderValido_RetornaCodigoBancoCorreto()
    {
        var parser = new ItauCnab400Parser();
        var lines = BuildValidRemessaLines();

        var result = parser.ParseFromLines(lines);

        result.Header.CodigoBanco.Should().Be(341);
    }

    [Fact]
    public void Parse_DetalheValido_RetornaValorCorreto()
    {
        var parser = new ItauCnab400Parser();
        var lines = BuildValidRemessaLines();

        var result = parser.ParseFromLines(lines);

        result.Detalhes.Should().ContainSingle();
        result.Detalhes[0].Valor.Should().Be(1234.56m);
    }

    [Fact]
    public void Parse_DetalheValido_RetornaDataVencimentoCorreta()
    {
        var parser = new ItauCnab400Parser();
        var lines = BuildValidRemessaLines();

        var result = parser.ParseFromLines(lines);

        result.Detalhes.Should().ContainSingle();
        result.Detalhes[0].DataVencimento.Should().Be(new DateTime(2026, 4, 30));
    }

    [Fact]
    public void Parse_LinhaComTamanhoInvalido_LancaException()
    {
        var parser = new ItauCnab400Parser();
        var lines = new[] { new string('0', 399) };

        var act = () => parser.ParseFromLines(lines);

        act.Should().Throw<InvalidDataException>()
            .WithMessage("*exatamente 400 caracteres*");
    }

    [Fact]
    public void Parse_ArquivoInexistente_LancaException()
    {
        var parser = new ItauCnab400Parser();

        var act = () => parser.Parse("arquivo-inexistente.rem");

        act.Should().Throw<FileNotFoundException>();
    }

    private static IEnumerable<string> BuildValidRemessaLines()
    {
        yield return BuildHeaderLine();
        yield return BuildDetalheLine();
        yield return BuildTrailerLine();
    }

    private static string BuildHeaderLine()
    {
        var chars = CreateBaseLine('0');
        SetField(chars, 2, 3, "341");
        SetField(chars, 27, 20, "EMPRESA EXEMPLO");
        SetField(chars, 95, 6, "300426");
        SetField(chars, 395, 6, "000001");
        return new string(chars);
    }

    private static string BuildDetalheLine()
    {
        var chars = CreateBaseLine('1');
        SetField(chars, 70, 12, "123456789012");
        SetField(chars, 121, 6, "300426");
        SetField(chars, 127, 13, "0000000123456");
        SetField(chars, 150, 30, "SACADO TESTE LTDA");
        SetField(chars, 180, 15, "123456789012345");
        SetField(chars, 370, 6, "INST01");
        SetField(chars, 376, 6, "INST02");
        SetField(chars, 395, 6, "000002");
        return new string(chars);
    }

    private static string BuildTrailerLine()
    {
        var chars = CreateBaseLine('9');
        SetField(chars, 18, 12, "000000000001");
        SetField(chars, 30, 17, "00000000000123456");
        SetField(chars, 395, 6, "000003");
        return new string(chars);
    }

    private static char[] CreateBaseLine(char tipoRegistro)
    {
        var chars = Enumerable.Repeat(' ', 400).ToArray();
        chars[0] = tipoRegistro;
        return chars;
    }

    private static void SetField(char[] chars, int start, int length, string value)
    {
        var formattedValue = value.Length > length
            ? value[..length]
            : value.PadRight(length);

        for (var i = 0; i < length; i++)
        {
            chars[start - 1 + i] = formattedValue[i];
        }
    }
}
