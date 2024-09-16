using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Questao5Web.DataContext;

namespace Questao5Web.Controllers
{   

    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContaCorrenteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contas = await _context.ContaCorrentes.ToListAsync();
            return Ok(contas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var conta = await _context.ContaCorrentes.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }
            return Ok(conta);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContaCorrente contaCorrente)
        {
            if (ModelState.IsValid)
            {
                _context.ContaCorrentes.Add(contaCorrente);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = contaCorrente.IdContaCorrente }, contaCorrente);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ContaCorrente contaCorrente)
        {
            if (id != contaCorrente.IdContaCorrente)
            {
                return BadRequest();
            }

            _context.Entry(contaCorrente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ContaCorrentes.Any(e => e.IdContaCorrente == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var conta = await _context.ContaCorrentes.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }

            _context.ContaCorrentes.Remove(conta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
