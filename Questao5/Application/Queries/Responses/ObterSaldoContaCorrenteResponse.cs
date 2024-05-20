namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoContaCorrenteResponse
    {
        public double Saldo { get; set; }
        public int NumeroContaCorrente { get; set; }
        public DateTime DataHoraResposta { get; set; } 
        public string NomeTitular { get; set; }
    }
}
