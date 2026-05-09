using CNAB400.NET.Bancos.Itau;
using CNAB400.NET.Enums;

namespace CNAB400.NET.Parser;

public static class Cnab400ParserFactory
{
    public static ICnab400Parser Create(BancoEnum banco) => banco switch
    {
        BancoEnum.Itau => new ItauCnab400Parser(),
        BancoEnum.Bradesco => throw new NotSupportedException($"Banco {banco} não suportado ainda."),
        _ => throw new NotSupportedException($"Banco {banco} não suportado ainda.")
    };
}
