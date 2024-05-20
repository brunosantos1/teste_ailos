using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ObterSaldoContaCorrenteRequest : IRequest<ObterSaldoContaCorrenteResponse>
    {
        public string IdContaCorrente { get; set; }
    }
}
