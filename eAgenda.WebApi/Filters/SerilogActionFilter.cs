using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Text.RegularExpressions;

namespace eAgenda.WebApi.Filters
{
    public class SerilogActionFilter : IActionFilter
    {
        private object nomeEndpoint;
        private object nomeModulo;


        public void OnActionExecuting(ActionExecutingContext context)
        {
            nomeEndpoint = context.RouteData.Values["action"]!
                .ToString()!.SepararPalavrasPorMaiusculas();

            nomeModulo = context.RouteData.Values["controller"];

            Log.Logger.Information($"[Módulo de {nomeModulo}] -> Tentando {nomeEndpoint}...");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                Log.Logger.Information($"[Módulo de {nomeModulo}] -> {nomeEndpoint} executado com sucesso");
            }
            else if (context.Exception != null)
            {
                Log.Logger.Error($"[Módulo de {nomeModulo}] -> Falha ao executar {nomeEndpoint}");
            }
        }
    }

    public static class StringExtensions
    {
        public static string SepararPalavrasPorMaiusculas(this string nomeMetodo)
        {
            string padraoParaSepararPorMaiusculas = @"([A-Z][a-z]*)";

            MatchCollection matches = Regex.Matches(nomeMetodo, padraoParaSepararPorMaiusculas);

            string nomeMetodoSeparado = "";

            foreach (Match m in matches)
                nomeMetodoSeparado += m.Value + " ";

            return nomeMetodoSeparado;
        }
    }
}
