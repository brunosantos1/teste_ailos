﻿using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class VerificarIdempotenciaQueryRequest : IRequest<IEnumerable<VerificarIdempotenciaQueryResponse>>
    {
        public string ChaveIdempotencia { get; set; }
    }
}
