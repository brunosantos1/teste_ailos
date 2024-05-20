using Newtonsoft.Json;
using System.Runtime.CompilerServices;

class Partida
{
    public string competition { get; set; }
    public int year { get; set; }
    public string round { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public int team1goals { get; set; }
    public int team2goals { get; set; }
}

class Raiz
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Partida> data { get; set; }
}

public class Program
{
    public static void Main()
    {
        try
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;
            int totalGoals = getTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            teamName = "Chelsea";
            year = 2014;
            totalGoals = getTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            // Output expected:
            // Team Paris Saint - Germain scored 109 goals in 2013
            // Team Chelsea scored 92 goals in 2014
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorre uma Exceção:");
            Console.WriteLine(ex.Message);
        }
        
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        string baseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

        int totalGols = 0;

        // É feito um for loop de 1 até 2 por que a requisição deve ser feita para os 2 lados que o time pode estar, team1 ou team2.
        for(var indice = 1; indice<=2; indice++)
        {
            string urlComParametros = $"{baseUrl}?year={year}&team{indice}={team}";
            using (HttpClient client = new HttpClient())
            {
                var paginaAtual = 1;
                var totalPaginas = 1;
                // While para tratar a paginação, pois os resultados não são trazidos todos de uma vez.
                while (paginaAtual <= totalPaginas)
                {
                    string urlFinal = $"{urlComParametros}&page={paginaAtual}";

                    HttpResponseMessage response = client.GetAsync(urlFinal).Result;

                    response.EnsureSuccessStatusCode();

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    Raiz raiz = JsonConvert.DeserializeObject<Raiz>(responseBody);
                    totalPaginas = raiz.total_pages;

                    foreach (var partida in raiz.data)
                    {
                        if(indice == 1)
                        {
                            totalGols += partida.team1goals;
                        } else
                        {
                            totalGols += partida.team2goals;
                        }
                    }
                    paginaAtual++;
                }
            }
        }

        return totalGols;
    }

    

    

}