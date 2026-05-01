using CNAB400.NET.Models;

namespace CNAB400.NET.Parser;

public interface ICnab400Parser
{
    RemessaCnab400 Parse(string filePath);

    RemessaCnab400 ParseFromLines(IEnumerable<string> lines);
}
