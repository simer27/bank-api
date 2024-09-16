using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ContaCorrente
{
    [Key]
    public string IdContaCorrente { get; set; }

    [Required]
    [Range(1, 9999999999)]
    public int Numero { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    public bool Ativo { get; set; }
}

public class Movimento
{
    [Key]
    public string IdMovimento { get; set; }

    [Required]
    public string IdContaCorrente { get; set; }

    [Required]
    public DateTime DataMovimento { get; set; }

    [Required]
    [RegularExpression("[CD]")]
    public string TipoMovimento { get; set; }

    [Required]
    [Range(0.01, Double.MaxValue)]
    public decimal Valor { get; set; }

    [ForeignKey("IdContaCorrente")]
    public ContaCorrente ContaCorrente { get; set; }
}

public class Idempotencia
{
    [Key]
    public string ChaveIdempotencia { get; set; }

    public string Requisicao { get; set; }

    public string Resultado { get; set; }
}
