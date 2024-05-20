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
    public class VerificarIdempotenciaQueryHandler : IRequestHandler<VerificarIdempotenciaQueryRequest, IEnumerable<VerificarIdempotenciaQueryResponse>>
    {
        private readonly DatabaseConfig databaseConfig;

        public VerificarIdempotenciaQueryHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public Task<IEnumerable<VerificarIdempotenciaQueryResponse>> Handle(VerificarIdempotenciaQueryRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string sql = "SELECT chave_idempotencia,requisicao,resultado from idempotencia WHERE chave_idempotencia = @id;";
            var parameters = new { id = request.ChaveIdempotencia };


            var lista = connection.Query<VerificarIdempotenciaQueryResponse>(sql, parameters);
            

            return Task.FromResult(lista);
        }
    }
}