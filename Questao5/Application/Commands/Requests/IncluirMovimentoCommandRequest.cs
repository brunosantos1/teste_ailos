using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class IncluirMovimentoCommandRequest: IRequest<IncluirMovimentoCommandResponse>
    {
        public string IdContaCorrente { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }
        public string IdRequisicao { get; set; }
    }
}
