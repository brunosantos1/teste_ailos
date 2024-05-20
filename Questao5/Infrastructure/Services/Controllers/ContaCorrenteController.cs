using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Sqlite;
using System.Collections.Generic;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        
        private readonly IMediator mediator;


        public ContaCorrenteController(IMediator _mediator)
        {
            this.mediator = _mediator;
        }

        
        [HttpGet("{id}/Saldo")]
        public async Task<IActionResult> GetSaldo([FromRoute] string id)
        {
            var query = new ObterSaldoContaCorrenteRequest();
            query.IdContaCorrente = id;

            
            try
            {
                var queryResponse = await this.mediator.Send(query);
                return Ok(queryResponse);
            } catch(InvalidAccount)
            {
                return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta não localizada" });
            } catch(InactiveAccount)
            {
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Esta conta está inativa" });
            }

            
        }
    }

    
}