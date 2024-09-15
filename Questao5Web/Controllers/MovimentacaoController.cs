using Microsoft.AspNetCore.Mvc;

namespace Questao5Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        public MovimentacaoController()
        {
        }

        [HttpPost(Name = "UpdateSaldo")]
        public ContaSaldoResponse UpdateSaldo(int contaId, double saldo)
        {
            return new ContaSaldoResponse()
            {
                CPF = 1234 + contaId,
                Saldo = 3434.34
            };
        }

        [HttpGet(Name = "GetSaldo")]
        public ContaSaldoResponse GetSaldo(int contaId)
        {
            return new ContaSaldoResponse()
            {
                CPF = 1234 + contaId,
                Saldo = 3434.34
            };
        }
    }

    public class ContaSaldoResponse
    {
        public int CPF { get; set; }
        public double Saldo { get; set; }
    }
}
