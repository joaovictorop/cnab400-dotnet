using System.Globalization;

namespace CNAB400.NET.Generator;

public abstract class Cnab400GeneratorBase
{
    protected void WriteFile(IEnumerable<string> lines, string outputPath)
    {
        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines), "As linhas a serem escritas não podem ser nulas.");
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException("O caminho de saída deve ser informado.", nameof(outputPath));
        }

        var materializedLines = lines.ToList();
        for (var i = 0; i < materializedLines.Count; i++)
        {
            var line = materializedLines[i];
            if (line.Length != 400)
            {
                throw new InvalidDataException(
                    $"A linha {i + 1} gerada possui {line.Length} caracteres. Cada linha deve ter exatamente 400 caracteres.");
            }
        }

        var fullPath = Path.GetFullPath(outputPath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllLines(fullPath, materializedLines);
    }

    protected string PadString(string value, int length)
    {
        value ??= string.Empty;

        if (value.Length > length)
        {
            return value[..length];
        }

        return value.PadRight(length);
    }

    protected string PadInt(int value, int length)
    {
        return value.ToString(CultureInfo.InvariantCulture).PadLeft(length, '0');
    }

    protected string PadDecimal(decimal value, int length, int decimalPlaces)
    {
        var factor = (decimal)Math.Pow(10, decimalPlaces);
        var scaled = decimal.Round(value * factor, 0, MidpointRounding.AwayFromZero);
        return scaled.ToString("0", CultureInfo.InvariantCulture).PadLeft(length, '0');
    }

    protected string PadDate(DateTime date)
    {
        return date.ToString("ddMMyy", CultureInfo.InvariantCulture);
    }
}
