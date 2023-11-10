using AutoMapper;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApi.Controllers.Shared;
using eAgenda.WebApi.ViewModels.ModuloContato;

using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    public class ContatoController : ApiControllerBase
    {
        private ServicoContato servicoContato;
        private IMapper mapeador;

        public ContatoController(ServicoContato servicoContato, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoContato = servicoContato;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListarContatoViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarTodos(StatusFavoritoEnum statusFavorito)
        {
            var contatoResult = await servicoContato.SelecionarTodosAsync(statusFavorito);

            var viewModel = mapeador.Map<List<ListarContatoViewModel>>(contatoResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        [ProducesResponseType(typeof(VisualizarContatoViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var contatoResult = await servicoContato.SelecionarPorIdAsync(id);

            if (contatoResult.IsFailed)
                return NotFound(contatoResult.Errors);

            var viewModel = mapeador.Map<VisualizarContatoViewModel>(contatoResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormsContatoViewModel), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Inserir(FormsContatoViewModel contatoViewModel)
        {
            var contato = mapeador.Map<Contato>(contatoViewModel);

            var contatoResult = await servicoContato.InserirAsync(contato);

            return ProcessarResultado(contatoResult.ToResult(), contatoViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormsContatoViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Editar(Guid id, FormsContatoViewModel contatoViewModel)
        {
            var resultadoSelecao = await servicoContato.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var contato = mapeador.Map(contatoViewModel, resultadoSelecao.Value);

            var contatoResult = await servicoContato.EditarAsync(contato);

            return ProcessarResultado(contatoResult.ToResult(), contatoViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var resultadoSelecao = await servicoContato.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var contatoResult = await servicoContato.ExcluirAsync(resultadoSelecao.Value);

            return ProcessarResultado(contatoResult);
        }

        [HttpPut("favoritos/{id}")]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Favoritar(Guid id)
        {
            var resultadoSelecao = await servicoContato.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var contatoResult = await servicoContato.FavoritarAsync(resultadoSelecao.Value);

            return ProcessarResultado(contatoResult.ToResult());
        }
    }
}


        
    

