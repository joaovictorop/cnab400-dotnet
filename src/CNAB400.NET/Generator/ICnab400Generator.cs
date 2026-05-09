using CNAB400.NET.Models;

namespace CNAB400.NET.Generator;

public interface ICnab400Generator
{
    void GenerateFile(RemessaCnab400 remessa, string outputPath);

    IEnumerable<string> GenerateLines(RemessaCnab400 remessa);
}
