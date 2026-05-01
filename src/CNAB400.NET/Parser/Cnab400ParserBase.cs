using System.Globalization;

namespace CNAB400.NET.Parser;

public abstract class Cnab400ParserBase
{
    protected IReadOnlyList<string> ReadLines(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("O caminho do arquivo deve ser informado.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Arquivo CNAB 400 não encontrado: {filePath}", filePath);
        }

        var lines = File.ReadAllLines(filePath);
        return ValidateAndMaterializeLines(lines);
    }

    protected IReadOnlyList<string> ValidateAndMaterializeLines(IEnumerable<string> lines)
    {
        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines), "As linhas do CNAB 400 não podem ser nulas.");
        }

        var materializedLines = lines.ToList();

        if (materializedLines.Count == 0)
        {
            throw new InvalidDataException("O arquivo CNAB 400 está vazio.");
        }

        for (var i = 0; i < materializedLines.Count; i++)
        {
            var line = materializedLines[i];
            if (line.Length != 400)
            {
                throw new InvalidDataException(
                    $"A linha {i + 1} possui {line.Length} caracteres. Cada linha deve ter exatamente 400 caracteres.");
            }
        }

        return materializedLines;
    }

    protected string ExtractString(string line, int start, int length)
    {
        ValidateRange(line, start, length);
        return line.Substring(start - 1, length).Trim();
    }

    protected decimal ExtractDecimal(string line, int start, int length, int decimalPlaces)
    {
        var raw = ExtractString(line, start, length);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return 0m;
        }

        if (!long.TryParse(raw, NumberStyles.None, CultureInfo.InvariantCulture, out var value))
        {
            throw new FormatException(
                $"Não foi possível converter o valor '{raw}' em decimal na posição {start} com tamanho {length}.");
        }

        var divisor = (decimal)Math.Pow(10, decimalPlaces);
        return value / divisor;
    }

    protected int ExtractInt(string line, int start, int length)
    {
        var raw = ExtractString(line, start, length);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return 0;
        }

        if (!int.TryParse(raw, NumberStyles.None, CultureInfo.InvariantCulture, out var value))
        {
            throw new FormatException(
                $"Não foi possível converter o valor '{raw}' em inteiro na posição {start} com tamanho {length}.");
        }

        return value;
    }

    protected DateTime ExtractDate(string line, int start)
    {
        var raw = ExtractString(line, start, 6);
        if (!DateTime.TryParseExact(raw, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            throw new FormatException($"Não foi possível converter a data '{raw}' no formato DDMMAA.");
        }

        return date;
    }

    private static void ValidateRange(string line, int start, int length)
    {
        if (line is null)
        {
            throw new ArgumentNullException(nameof(line));
        }

        if (start <= 0 || length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), "Start e length devem ser maiores que zero.");
        }

        if ((start - 1) + length > line.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(length),
                $"Faixa inválida: start={start}, length={length} para linha com {line.Length} caracteres.");
        }
    }
}
