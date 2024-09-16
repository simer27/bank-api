using Microsoft.AspNetCore.Mvc;
using Questao5Web.DataContext;
using Questao5Web.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Questao5Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovimentoController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Realiza a movimentação na conta do cliente pelo seu identificador único.
        /// </summary>
        /// <param name="IdContaCorrente">Id do cliente que irá movimentar a conta</param>
        /// <remarks>
        /// Exemplo de Requisição:
        /// Post: <br />
        /// { <br />
        ///      "chaveIdempotencia": "258", <br />
        ///      "idContaCorrente": "382D323D-7067-ED11-8866-7C5DFA4A16C2", <br />
        ///      "valor": 57.00, <br />
        ///      "tipoMovimento": "D" <br />
        /// }
        /// </remarks>
        /// <returns>O identificador da movimentação gerada.</returns>
        /// <response code="200"> Retorno de exemplo: <br />
        /// "idMovimento": "e5554646-be39-4c89-b8d3-4a53d889528b"</response>
        /// <response code="400"> Retorno de exemplo: <br />
        ///     "tipo": "INVALID_ACCOUNT", "mensagem": "Conta corrente não encontrada."<br />
        ///     <br />
        ///  OU <br />
        ///     "tipo": "INACTIVE_ACCOUNT", "mensagem": "Conta corrente inativa."<br />
        ///     <br />
        ///  OU <br />
        ///     "tipo": "INVALID_VALUE", "mensagem": "O valor da movimentação deve ser positivo."<br />
        ///     <br />
        ///  OU <br />
        ///     "tipo": "INVALID_TYPE", "mensagem": "Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito."</response>
        [HttpPost]
        public async Task<IActionResult> Create(MovimentoRequest request)
        {            

            var conta = await _context.ContaCorrentes.FindAsync(request.IdContaCorrente);
            if (conta == null)
            {
                return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta corrente não encontrada." });
            }

            if (!conta.Ativo)
            {
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente inativa." });
            }

            if (request.Valor <= 0)
            {
                return BadRequest(new { Tipo = "INVALID_VALUE", Mensagem = "O valor da movimentação deve ser positivo." });
            }

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            {
                return BadRequest(new { Tipo = "INVALID_TYPE", Mensagem = "Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito." });
            }

            var idempotenciaExistente = await _context.Idempotencias.FindAsync(request.ChaveIdempotencia);
            if (idempotenciaExistente != null)
            {
                return Ok(new { IdMovimento = idempotenciaExistente.Resultado });
            }

            var movimento = new Movimento
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.UtcNow,
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            };

            _context.Movimentos.Add(movimento);

            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = request.ChaveIdempotencia,
                Requisicao = request.ToString(),
                Resultado = movimento.IdMovimento
            };
            _context.Idempotencias.Add(idempotencia);

            await _context.SaveChangesAsync();

            return Ok(new { IdMovimento = movimento.IdMovimento });
        }

        private object FormatValidationErrors(ModelStateDictionary modelState)
        {
            var errors = new List<object>();

            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = error.ErrorMessage;

                    if (state.Key == nameof(MovimentoRequest.TipoMovimento))
                    {
                        errorMessage = "Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.";
                    }

                    errors.Add(new { Campo = state.Key, Mensagem = errorMessage });
                }
            }

            return new { Tipo = "VALIDATION_ERROR", Erros = errors };
        }


        /// <summary>
        /// Realiza a consulta do saldo atual na conta do cliente pelo seu identificador único.
        /// </summary>
        /// <param name="IdContaCorrente">Id do cliente que irá consultar o saldo da conta</param>
        /// <remarks>
        /// Exemplo de Requisição:
        /// Get:         
        /// Parametro de entrada: <br />
        /// Id da Conta Corrente: "382D323D-7067-ED11-8866-7C5DFA4A16C2",
        /// </remarks>
        /// <returns>O identificador da movimentação gerada.</returns>
        /// <response code="200"> Retorno de exemplo: <br />
        /// {<br />
        /// "numeroConta": 456,<br />
        /// "nomeTitular": "Eva Woodward",<br />
        /// "dataConsulta": "2024-09-16T13:21:01.8111821-03:00",<br />
        /// "saldoAtual": "527,51" <br />
        ///}
        /// </response>

        /// <response code="400"> Retorno de exemplo: <br />
        ///     "tipo": "INVALID_ACCOUNT", "mensagem": "Conta corrente não encontrada."<br />
        ///     <br />
        ///  OU <br />
        ///     "tipo": "INACTIVE_ACCOUNT", "mensagem": "Conta corrente inativa." </response>
        [HttpGet("{id}/saldo")]
        public async Task<IActionResult> GetSaldo(string id)
        {
            var contaCorrente = await _context.ContaCorrentes.FindAsync(id);
            if (contaCorrente == null)
            {
                return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta corrente não encontrada." });
            }

            if (!contaCorrente.Ativo)
            {
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente está inativa." });
            }

            var creditos = await _context.Movimentos
                .Where(m => m.IdContaCorrente == id && m.TipoMovimento == "C")
                .SumAsync(m => (double?)m.Valor) ?? 0.0;

            var debitos = await _context.Movimentos
                .Where(m => m.IdContaCorrente == id && m.TipoMovimento == "D")
                .SumAsync(m => (double?)m.Valor) ?? 0.0;

            var saldo = creditos - debitos;

            var saldoArredondado = Math.Round(saldo, 2);

            var resultado = new
            {
                NumeroConta = contaCorrente.Numero,
                NomeTitular = contaCorrente.Nome,
                DataConsulta = DateTime.Now,
                SaldoAtual = saldoArredondado.ToString("F2") 
            };

            return Ok(resultado);
        }


    }
}
