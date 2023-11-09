using Serilog;
using System.Text.Json;

namespace eAgenda.WebApi.Config
{
    public class ManipuladorExcecoes
    {
        private readonly RequestDelegate requestDelegate;

        public ManipuladorExcecoes(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await this.requestDelegate(ctx);
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";

                var problema = new
                {
                    Sucesso = false,
                    Erros = new List<string> { ex.Message }
                };

                Log.Logger.Error(ex, ex.Message);

                ctx.Response.WriteAsync(JsonSerializer.Serialize(problema));
            }
        }
    }
}