using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.WebApi.Controllers.Shared;

namespace eAgenda.WebApi.Controllers
{
    public class CategoriaController : ApiControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeador;
    }
}
