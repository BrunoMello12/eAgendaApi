using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/saudacao")]
    [ApiController]
    public class SaudacaoController : ControllerBase
    {
        [HttpGet("bom-dia")]
        public string BomDia()
        {
            return "Bom dia Galera da Academia";
        }

        [HttpGet("boa-tarde")]
        public Saudacao BomTarde()
        {
            return new Saudacao
            {
                Data = DateTime.Now,
                Mensagem = "Bom tarde Galera da Academia"
            };
        }
    }

    public class Saudacao
    {
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }
    }
}
