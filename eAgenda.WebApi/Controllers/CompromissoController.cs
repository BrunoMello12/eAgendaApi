using AutoMapper;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.WebApi.ViewModels.ModuloCompromisso;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/compromissos")]
    public class CompromissoController : ControllerBase
    {
        private IMapper mapeador;
        private ServicoCompromisso servicoCompromisso;

        public CompromissoController(ServicoCompromisso servicoCompromisso, IMapper mapeador)
        {
            this.servicoCompromisso = servicoCompromisso;
            this.mapeador = mapeador;
        }

        [HttpGet]
        public List<ListarCompromissoViewModel> SelecionarTodos()
        {
            var compromissos = servicoCompromisso.SelecionarTodos().Value;

            return mapeador.Map<List<ListarCompromissoViewModel>>(compromissos);
        }

        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarCompromissoViewModel SeleciontarPorId(Guid id)
        {
            var compromisso = servicoCompromisso.SelecionarPorId(id).Value;

            return mapeador.Map<VisualizarCompromissoViewModel>(compromisso);
        }

        [HttpPost]
        public string Inserir(FormsCompromissoViewModel compromissoViewModel)
        {
            var compromisso = mapeador.Map<Compromisso>(compromissoViewModel);  

            var resultado = servicoCompromisso.Inserir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso inserido com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpPut("{id}")]
        public string Editar(Guid id, FormsCompromissoViewModel compromissoViewModel)
        {
            var compromissoEncontrado = servicoCompromisso.SelecionarPorId(id).Value;

            var compromisso = mapeador.Map(compromissoViewModel, compromissoEncontrado);

            var resultado = servicoCompromisso.Editar(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso editado com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpDelete("{id}")]
        public string Excluir(Guid id)
        {
            var resultadoBusca = servicoCompromisso.SelecionarPorId(id);

            if (resultadoBusca.IsFailed)
            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var compromisso = resultadoBusca.Value;

            var resultado = servicoCompromisso.Excluir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso excluído com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}
