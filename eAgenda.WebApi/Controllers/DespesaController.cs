using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.Controllers.Shared;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa.Categoria;
using eAgenda.WebApi.ViewModels.ModuloDespesa.Despesa;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [Route("api/despesas")]
    [ApiController]
    public class DespesaController : ApiControllerBase
    {
        private readonly ServicoDespesa servicoDespesa;
        private readonly IMapper mapeador;

        public DespesaController(ServicoDespesa servicoDespesa, IMapper mapeadorDespesas)
        {
            this.servicoDespesa = servicoDespesa;
            this.mapeador = mapeadorDespesas;
        }

        [HttpGet("ultimos-30-dias")]
        [ProducesResponseType(typeof(ListarDespesaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarDespesasUltimos30Dias()
        {
            var despesaResult = servicoDespesa.SelecionarDespesasUltimos30Dias(DateTime.Now);

            var viewModel = mapeador.Map<List<ListarDespesaViewModel>>(despesaResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("antigas")]
        [ProducesResponseType(typeof(ListarDespesaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarDespesasAntigas()
        {
            var despesaResult = servicoDespesa.SelecionarDespesasAntigas(DateTime.Now);

            var viewModel = mapeador.Map<List<ListarDespesaViewModel>>(despesaResult.Value);

            return Ok(viewModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListarDespesaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarTodos()
        {
            var despesaResult = await servicoDespesa.SelecionarTodosAsync();

            var viewModel = mapeador.Map<List<ListarDespesaViewModel>>(despesaResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        [ProducesResponseType(typeof(VisualizarDespesaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var despesaResult = await servicoDespesa.SelecionarPorIdAsync(id);

            if (despesaResult.IsFailed)
                return NotFound(despesaResult.Errors);

            var viewModel = mapeador.Map<VisualizarDespesaViewModel>(despesaResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormsCategoriaViewModel), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Inserir(FormsCategoriaViewModel despesaViewModel)
        {
            var despesa = mapeador.Map<Despesa>(despesaViewModel);

            var despesaResult = await servicoDespesa.InserirAsync(despesa);

            return ProcessarResultado(despesaResult.ToResult(), despesaViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormsCategoriaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Editar(Guid id, FormsCategoriaViewModel despesaViewModel)
        {
            var resultadoSelecao = await servicoDespesa.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var despesa = mapeador.Map(despesaViewModel, resultadoSelecao.Value);

            var despesaResult = await servicoDespesa.EditarAsync(despesa);

            return ProcessarResultado(despesaResult.ToResult(), despesaViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var resultadoSelecao = await servicoDespesa.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var despesaResult = await servicoDespesa.ExcluirAsync(resultadoSelecao.Value);

            return ProcessarResultado(despesaResult);
        }

    }
}
