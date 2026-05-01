namespace CNAB400.NET.Models;

public class RemessaCnab400
{
    public HeaderRemessa Header { get; set; } = new();

    public List<DetalheRemessa> Detalhes { get; set; } = [];

    public TrailerRemessa Trailer { get; set; } = new();
}
