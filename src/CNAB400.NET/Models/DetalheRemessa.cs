namespace CNAB400.NET.Models;

public class DetalheRemessa
{
    public string NossoNumero { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public DateTime DataVencimento { get; set; }

    public string NomeSacado { get; set; } = string.Empty;

    public string CpfCnpjSacado { get; set; } = string.Empty;

    public string Instrucao1 { get; set; } = string.Empty;

    public string Instrucao2 { get; set; } = string.Empty;

    public int NumeroSequencial { get; set; }
}
