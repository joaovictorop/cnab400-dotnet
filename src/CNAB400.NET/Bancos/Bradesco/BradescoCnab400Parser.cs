using CNAB400.NET.Models;
using CNAB400.NET.Parser;

namespace CNAB400.NET.Bancos.Bradesco;

public class BradescoCnab400Parser : Cnab400ParserBase, ICnab400Parser
{
    public RemessaCnab400 Parse(string filePath)
    {
        var lines = ReadLines(filePath);
        return ParseFromLines(lines);
    }

    public RemessaCnab400 ParseFromLines(IEnumerable<string> lines)
    {
        var validatedLines = ValidateAndMaterializeLines(lines);
        var remessa = new RemessaCnab400();
        var headerEncontrado = false;
        var trailerEncontrado = false;

        foreach (var line in validatedLines)
        {
            var tipoRegistro = ExtractString(line, 1, 1);

            switch (tipoRegistro)
            {
                case "0":
                    remessa.Header = ParseHeader(line);
                    headerEncontrado = true;
                    break;
                case "1":
                    remessa.Detalhes.Add(ParseDetalhe(line));
                    break;
                case "9":
                    remessa.Trailer = ParseTrailer(line);
                    trailerEncontrado = true;
                    break;
            }
        }

        if (!headerEncontrado)
        {
            throw new InvalidDataException("Arquivo CNAB 400 inválido: header (tipo 0) não encontrado.");
        }

        if (!trailerEncontrado)
        {
            throw new InvalidDataException("Arquivo CNAB 400 inválido: trailer (tipo 9) não encontrado.");
        }

        return remessa;
    }

    private HeaderRemessa ParseHeader(string line)
    {
        return new HeaderRemessa
        {
            CodigoBanco = 237,
            NomeEmpresa = ExtractString(line, 77, 18),
            DataGravacao = ExtractDate(line, 95),
            NumeroSequencial = ExtractInt(line, 395, 6),
        };
    }

    private DetalheRemessa ParseDetalhe(string line)
    {
        return new DetalheRemessa
        {
            NossoNumero = ExtractString(line, 58, 15),
            DataVencimento = ExtractDate(line, 121),
            Valor = ExtractDecimal(line, 127, 13, 2),
            NomeSacado = ExtractString(line, 150, 30),
            CpfCnpjSacado = ExtractString(line, 180, 15),
            Instrucao1 = ExtractString(line, 370, 6),
            Instrucao2 = ExtractString(line, 376, 6),
            NumeroSequencial = ExtractInt(line, 395, 6),
        };
    }

    private TrailerRemessa ParseTrailer(string line)
    {
        return new TrailerRemessa
        {
            QuantidadeTitulos = ExtractInt(line, 18, 12),
            ValorTotal = ExtractDecimal(line, 30, 17, 2),
            NumeroSequencial = ExtractInt(line, 395, 6),
        };
    }
}
