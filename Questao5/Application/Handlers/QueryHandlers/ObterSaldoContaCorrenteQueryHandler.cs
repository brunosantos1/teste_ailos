using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers.QueryHandlers
{
    public class ObterSaldoContaCorrenteQueryHandler : IRequestHandler<ObterSaldoContaCorrenteRequest, ObterSaldoContaCorrenteResponse>
    {
        private readonly DatabaseConfig databaseConfig;

        public ObterSaldoContaCorrenteQueryHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public Task<ObterSaldoContaCorrenteResponse> Handle(ObterSaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string sql_conta = "SELECT * from contacorrente WHERE idcontacorrente = @id;";
            var parameters_conta = new { id = request.IdContaCorrente };


            var contas = connection.Query<ContaCorrente>(sql_conta, parameters_conta).ToList();
            if(contas.Count<1)
            {
                throw new InvalidAccount();
            } else if(contas.FirstOrDefault().ativo!=true)
            {
                throw new InactiveAccount();
            }

            string sql_saldo = @"
                SELECT 
                    SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END) - 
                    SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END) AS Saldo
                FROM movimento
                WHERE idcontacorrente = @idConta;";
            var parameters_saldo = new { idConta = request.IdContaCorrente };

            var saldo = connection.QueryFirstOrDefault<double>(sql_saldo, parameters_saldo);
            var hora_atual = DateTime.Now;
            var conta = contas.FirstOrDefault();
            ObterSaldoContaCorrenteResponse response = new ObterSaldoContaCorrenteResponse();
            response.Saldo = saldo;
            response.DataHoraResposta = hora_atual;
            response.NomeTitular = conta.nome;
            response.NumeroContaCorrente = conta.numero;

            return Task.FromResult(response);
        }
    }
}