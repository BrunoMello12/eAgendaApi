using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.Controllers.Shared;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa.Categoria;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ApiControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeador;

        public CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeadorCategorias)
        {
            this.servicoCategoria = servicoCategoria;
            this.mapeador = mapeadorCategorias;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListarCategoriaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarTodos()
        {
            var categoriaResult = await servicoCategoria.SelecionarTodosAsync();

            var viewModel = mapeador.Map<List<ListarCategoriaViewModel>>(categoriaResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        [ProducesResponseType(typeof(VisualizarCategoriaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var categoriaResult = await servicoCategoria.SelecionarPorIdAsync(id);

            if (categoriaResult.IsFailed)
                return NotFound(categoriaResult.Errors);

            var viewModel = mapeador.Map<VisualizarCategoriaViewModel>(categoriaResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormsCategoriaViewModel), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Inserir(FormsCategoriaViewModel categoriaViewModel)
        {
            var categoria = mapeador.Map<Categoria>(categoriaViewModel);

            var categoriaResult = await servicoCategoria.InserirAsync(categoria);

            return ProcessarResultado(categoriaResult.ToResult(), categoriaViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormsCategoriaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Editar(Guid id, FormsCategoriaViewModel categoriaViewModel)
        {
            var resultadoSelecao = await servicoCategoria.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var categoria = mapeador.Map(categoriaViewModel, resultadoSelecao.Value);

            var categoriaResult = await servicoCategoria.EditarAsync(categoria);

            return ProcessarResultado(categoriaResult.ToResult(), categoriaViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var resultadoSelecao = await servicoCategoria.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var categoriaResult = await servicoCategoria.ExcluirAsync(resultadoSelecao.Value);

            return ProcessarResultado(categoriaResult);
        }

    }
}
