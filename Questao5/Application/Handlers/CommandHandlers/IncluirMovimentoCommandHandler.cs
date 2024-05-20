using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers.CommandHandlers
{
    public class IncluirMovimentoCommandHandler : IRequestHandler<IncluirMovimentoCommandRequest, IncluirMovimentoCommandResponse>
    {
        private readonly DatabaseConfig databaseConfig;

        public IncluirMovimentoCommandHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public Task<IncluirMovimentoCommandResponse> Handle(IncluirMovimentoCommandRequest request, CancellationToken cancellationToken)
        {
            // Por questáo de desempenho validamos primeiro operaçoes que nao batem no banco de dados. Dessa forma se esta parte já estiver inválida, descartamos a necessidade de perder tempo validando outra parte no banco de dados que seria uma operaçao mais pesada.
            request.Tipo = request.Tipo.ToUpper();
            if(request.Tipo!="C" && request.Tipo!="D")
            {
                throw new InvalidType();
            } else if(request.Valor<=0)
            {
                throw new InvalidValue();
            }

            using (var connection = new SqliteConnection(databaseConfig.Name))
            {
                string sql_conta = "SELECT * from contacorrente WHERE idcontacorrente = @id;";
                var parameters_conta = new { id = request.IdContaCorrente };


                var contas = connection.Query<ContaCorrente>(sql_conta, parameters_conta).ToList();
                if (contas.Count < 1)
                {
                    throw new InvalidAccount();
                }
                else if (contas.FirstOrDefault().ativo != true)
                {
                    throw new InactiveAccount();
                }
                Guid newGuid = Guid.NewGuid();
                var parameters_movimento = new
                {
                    idmovimento = newGuid,
                    idcontacorrente = request.IdContaCorrente,
                    datamovimento = DateTime.Now,
                    tipomovimento = request.Tipo,
                    valor = request.Valor
                };
                var sqlmovimento = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor);";
                connection.Execute(sqlmovimento, parameters_movimento);

                IncluirMovimentoCommandResponse result = new IncluirMovimentoCommandResponse();
                result.IdMovimento = newGuid.ToString();

                string json_req = JsonConvert.SerializeObject(request);
                string json_resp = JsonConvert.SerializeObject(result);

                var parameters_idem = new
                {
                    chave_idempotencia = request.IdRequisicao,
                    requisicao = json_req,
                    resultado = json_resp
                };
                var sqlidem = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@chave_idempotencia, @requisicao, @resultado);";
                connection.Execute(sqlidem, parameters_idem);

                
                return Task.FromResult(result);
            }

        }
    }
}
