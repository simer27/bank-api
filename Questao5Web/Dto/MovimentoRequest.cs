using System.ComponentModel.DataAnnotations;

namespace Questao5Web.Dto
{
    public class MovimentoRequest
    {
        [Required]
        public string ChaveIdempotencia { get; set; }

        [Required]
        public string IdContaCorrente { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public string TipoMovimento { get; set; }
    }

}
