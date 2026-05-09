using CNAB400.NET.Bancos.Itau;
using CNAB400.NET.Enums;

namespace CNAB400.NET.Generator;

public static class Cnab400GeneratorFactory
{
    public static ICnab400Generator Create(BancoEnum banco) => banco switch
    {
        BancoEnum.Itau => new ItauCnab400Generator(),
        _ => throw new NotSupportedException($"Banco {banco} não suportado ainda.")
    };
}
