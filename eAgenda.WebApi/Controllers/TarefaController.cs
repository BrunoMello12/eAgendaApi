using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApi.Controllers.Shared;
using eAgenda.WebApi.ViewModels.ModuloTarefa;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [Route("api/tarefas")]
    [ApiController]
    public class TarefaController : ApiControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;
        private readonly IMapper mapeador;

        public TarefaController(ServicoTarefa servicoTarefa, IMapper mapeadorTarefas)
        {
            this.servicoTarefa = servicoTarefa;
            this.mapeador = mapeadorTarefas;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ListarTarefaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SeleciontarTodos(StatusTarefaEnum status)
        {
            var tarefaResult = await servicoTarefa.SelecionarTodosAsync(status);

            var viewModel = mapeador.Map<List<ListarTarefaViewModel>>(tarefaResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        [ProducesResponseType(typeof(VisualizarTarefaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> SeleciontarPorId(Guid id)
        {
            var tarefaResult = await servicoTarefa.SelecionarPorIdAsync(id);

            if (tarefaResult.IsFailed)
                return NotFound(tarefaResult.Errors);

            var viewModel = mapeador.Map<VisualizarTarefaViewModel>(tarefaResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormsTarefaViewModel), 201)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Inserir(FormsTarefaViewModel tarefaViewModel)
        {
            var tarefa = mapeador.Map<Tarefa>(tarefaViewModel);

            var tarefaResult = await servicoTarefa.InserirAsync(tarefa);

            return ProcessarResultado(tarefaResult.ToResult(), tarefaViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormsTarefaViewModel), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Editar(Guid id, FormsTarefaViewModel tarefaViewModel)
        {
            var resultadoSelecao = await servicoTarefa.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var tarefa = mapeador.Map(tarefaViewModel, resultadoSelecao.Value);

            var tarefaResult = await servicoTarefa.EditarAsync(tarefa);

            return ProcessarResultado(tarefaResult.ToResult(), tarefaViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [ProducesResponseType(typeof(string[]), 404)]
        [ProducesResponseType(typeof(string[]), 500)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var resultadoSelecao = await servicoTarefa.SelecionarPorIdAsync(id);

            if (resultadoSelecao.IsFailed)
                return NotFound(resultadoSelecao.Errors);

            var tarefaResult = await servicoTarefa.ExcluirAsync(resultadoSelecao.Value);

            return ProcessarResultado(tarefaResult);
        }

    }
}
