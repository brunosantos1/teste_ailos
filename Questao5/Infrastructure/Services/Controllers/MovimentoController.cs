using Dapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Services.Message;
using Questao5.Infrastructure.Sqlite;
using System.Collections.Generic;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentoController : ControllerBase
    {
        
        private readonly IMediator mediator;


        public MovimentoController(IMediator _mediator)
        {
            this.mediator = _mediator;
        }

        
        [HttpPost()]
        public async Task<IActionResult> AdicionarMovimento([FromBody] IncluirMovimentoRequest request, [FromHeader(Name = "Idempotency-key")] string idempotencyKey)
        {
            if (string.IsNullOrEmpty(idempotencyKey))
            {
                return BadRequest("Idempotency-key is required");
            }

            // Verifica se requisição já foi feita
            var queryidem = new VerificarIdempotenciaQueryRequest();
            queryidem.ChaveIdempotencia = idempotencyKey;
            var result = await this.mediator.Send(queryidem);
            if(result.ToList().Count>0)
            {
                var linha = result.FirstOrDefault();
                return Ok(JsonConvert.DeserializeObject<IncluirMovimentoCommandResponse>(linha.resultado));
            }

            // Segue o fluxo caso nao tenha sido feita ainda esta requisição

            IncluirMovimentoCommandRequest command = new IncluirMovimentoCommandRequest();
            command.IdContaCorrente = request.IdContaCorrente;
            command.Valor = request.Valor;
            command.Tipo = request.Tipo;
            command.IdRequisicao = idempotencyKey;

            try
            {
                var commandResponse = await this.mediator.Send(command);
                return Ok(commandResponse);
            } catch(InvalidAccount)
            {
                return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta não localizada" });
            } catch(InactiveAccount)
            {
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Esta conta está inativa" });
            }
            catch (InvalidType)
            {
                return BadRequest(new { Tipo = "INACTIVE_TYPE", Mensagem = "Tipo deve ser C ou D "});
            }
            catch (InvalidValue)
            {
                return BadRequest(new { Tipo = "INACTIVE_VALUE", Mensagem = "Valor deve ser positivo" });
            }


        }
    }

    
}