using CNAB400.NET.Generator;
using CNAB400.NET.Models;

namespace CNAB400.NET.Bancos.Itau;

public class ItauCnab400Generator : Cnab400GeneratorBase, ICnab400Generator
{
    public void GenerateFile(RemessaCnab400 remessa, string outputPath)
    {
        var lines = GenerateLines(remessa);
        WriteFile(lines, outputPath);
    }

    public IEnumerable<string> GenerateLines(RemessaCnab400 remessa)
    {
        if (remessa is null)
        {
            throw new ArgumentNullException(nameof(remessa));
        }

        var lines = new List<string>
        {
            BuildHeader(remessa.Header),
        };

        lines.AddRange(remessa.Detalhes.Select(BuildDetalhe));
        lines.Add(BuildTrailer(remessa.Trailer));

        return lines;
    }

    private string BuildHeader(HeaderRemessa header)
    {
        var chars = CreateLine('0');
        SetField(chars, 2, PadInt(header.CodigoBanco, 3));
        SetField(chars, 27, PadString(header.NomeEmpresa, 20));
        SetField(chars, 95, PadDate(header.DataGravacao));
        SetField(chars, 395, PadInt(header.NumeroSequencial, 6));
        return new string(chars);
    }

    private string BuildDetalhe(DetalheRemessa detalhe)
    {
        var chars = CreateLine('1');
        SetField(chars, 70, PadNumericString(detalhe.NossoNumero, 12));
        SetField(chars, 121, PadDate(detalhe.DataVencimento));
        SetField(chars, 127, PadDecimal(detalhe.Valor, 13, 2));
        SetField(chars, 150, PadString(detalhe.NomeSacado, 30));
        SetField(chars, 180, PadNumericString(detalhe.CpfCnpjSacado, 15));
        SetField(chars, 370, PadString(detalhe.Instrucao1, 6));
        SetField(chars, 376, PadString(detalhe.Instrucao2, 6));
        SetField(chars, 395, PadInt(detalhe.NumeroSequencial, 6));
        return new string(chars);
    }

    private string BuildTrailer(TrailerRemessa trailer)
    {
        var chars = CreateLine('9');
        SetField(chars, 18, PadInt(trailer.QuantidadeTitulos, 12));
        SetField(chars, 30, PadDecimal(trailer.ValorTotal, 17, 2));
        SetField(chars, 395, PadInt(trailer.NumeroSequencial, 6));
        return new string(chars);
    }

    private static char[] CreateLine(char tipoRegistro)
    {
        var chars = Enumerable.Repeat(' ', 400).ToArray();
        chars[0] = tipoRegistro;
        return chars;
    }

    private static void SetField(char[] chars, int start, string value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            chars[start - 1 + i] = value[i];
        }
    }

    private static string PadNumericString(string value, int length)
    {
        value ??= string.Empty;
        var digits = new string(value.Where(char.IsDigit).ToArray());

        if (digits.Length > length)
        {
            digits = digits[^length..];
        }

        return digits.PadLeft(length, '0');
    }
}
