namespace CNAB400.NET.Models;

public class HeaderRemessa
{
    public int CodigoBanco { get; set; }

    public string NomeEmpresa { get; set; } = string.Empty;

    public DateTime DataGravacao { get; set; }

    public int NumeroSequencial { get; set; }
}
