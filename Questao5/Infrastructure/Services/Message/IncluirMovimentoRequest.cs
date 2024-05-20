namespace Questao5.Infrastructure.Services.Message
{
    public class IncluirMovimentoRequest
    {
        public string IdContaCorrente { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }
    }
}
