using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5Web.Models
{

    public class Movimento
    {
        [Key]
        public string IdMovimento { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string IdContaCorrente { get; set; }

        [Required]
        public DateTime DataMovimento { get; set; } = DateTime.UtcNow;

        [Required]
        [RegularExpression("[CD]")]
        public string TipoMovimento { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue)]
        public double Valor { get; set; }

        [ForeignKey("IdContaCorrente")]
        public ContaCorrente ContaCorrente { get; set; }
    }

}
