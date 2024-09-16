using MediatR;
using Questao5Web.DataContext;
using Questao5Web.Application.Commands;

namespace Questao5Web.Application.Handler
{

    public class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateMovimentoCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            var idempotencia = await _context.Idempotencias.FindAsync(request.RequestId);
            if (idempotencia != null)
            {
                return new CreateMovimentoResponse { Success = false, Message = "Requisição já processada." };
            }

            var conta = await _context.ContaCorrentes.FindAsync(request.IdContaCorrente);
            if (conta == null)
            {
                return new CreateMovimentoResponse { Success = false, Message = "INVALID_ACCOUNT" };
            }

            if (!conta.Ativo)
            {
                return new CreateMovimentoResponse { Success = false, Message = "INACTIVE_ACCOUNT" };
            }

            if (request.Valor <= 0)
            {
                return new CreateMovimentoResponse { Success = false, Message = "INVALID_VALUE" };
            }

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            {
                return new CreateMovimentoResponse { Success = false, Message = "INVALID_TYPE" };
            }

            var movimento = new Movimento
            {
                IdMovimento = request.IdMovimento,
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.UtcNow,
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            };

            _context.Movimentos.Add(movimento);
            await _context.SaveChangesAsync();

            var idempotenciaEntry = new Idempotencia
            {
                ChaveIdempotencia = request.RequestId,
                Requisicao = request.ToString(),
                Resultado = movimento.IdMovimento
            };
            _context.Idempotencias.Add(idempotenciaEntry);
            await _context.SaveChangesAsync();

            return new CreateMovimentoResponse { Success = true, IdMovimento = movimento.IdMovimento };
        }
    }
}
