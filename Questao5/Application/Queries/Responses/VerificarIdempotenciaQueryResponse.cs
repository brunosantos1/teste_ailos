namespace Questao5.Application.Queries.Responses
{
    public class VerificarIdempotenciaQueryResponse
    {
        public string chave_idempotencia { get; set; }
        public string requisicao { get; set; }
        public string resultado { get; set; }
    }
}