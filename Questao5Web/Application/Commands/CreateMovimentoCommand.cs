using MediatR;

namespace Questao5Web.Application.Commands
{

    public class CreateMovimentoCommand : IRequest<CreateMovimentoResponse>
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
        public string RequestId { get; set; }
    }

}
