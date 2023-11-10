using eAgenda.Dominio;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloTarefa;
using FluentResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloTarefa
{
    public class ServicoTarefa : ServicoBase<Tarefa, ValidadorTarefa>
    {
        private IRepositorioTarefa repositorioTarefa;
        private IContextoPersistencia contextoPersistencia;

        public ServicoTarefa(IRepositorioTarefa repositorioTarefa,
                             IContextoPersistencia contexto)
        {
            this.repositorioTarefa = repositorioTarefa;
            this.contextoPersistencia = contexto;
        }

        public async Task<Result<Tarefa>> InserirAsync(Tarefa tarefa)
        {
            Result resultado = Validar(tarefa);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

                await repositorioTarefa.InserirAsync(tarefa);

                await contextoPersistencia.GravarDadosAsync();

                return Result.Ok(tarefa);
        }

        public async Task<Result<Tarefa>> EditarAsync(Tarefa tarefa)
        {
            var resultado = Validar(tarefa);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

                tarefa.CalcularPercentualConcluido();

                repositorioTarefa.Editar(tarefa);

                await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(tarefa);
        }

        public Task<Result<Tarefa>> AtualizarItens(Tarefa tarefa,
            List<ItemTarefa> itensConcluidos, List<ItemTarefa> itensPendentes)
        {
            foreach (var item in itensConcluidos)
                tarefa.ConcluirItem(item.Id);

            foreach (var item in itensPendentes)
                tarefa.MarcarPendente(item.Id);

            return EditarAsync(tarefa);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var tarefaResult = await SelecionarPorIdAsync(id);

            if (tarefaResult.IsSuccess)
                return await ExcluirAsync(tarefaResult.Value);

            return Result.Fail(tarefaResult.Errors);
        }

        public async Task<Result> ExcluirAsync(Tarefa tarefa)
        {
                repositorioTarefa.Excluir(tarefa);

                await contextoPersistencia.GravarDadosAsync();

                return Result.Ok();
        }

        public async Task<Result<List<Tarefa>>> SelecionarTodosAsync(StatusTarefaEnum status)
        {
            var tarefas = await repositorioTarefa.SelecionarTodosAsync(status);

            return Result.Ok(tarefas);
        }

        public async Task<Result<Tarefa>> SelecionarPorIdAsync(Guid id)
        {
            var tarefa = await repositorioTarefa.SelecionarPorIdAsync(id);

            if (tarefa == null)
            {
                Log.Logger.Warning($"Tarefa {id} não encontrada", id);

                return Result.Fail($"Tarefa {id} não encontrada");
            }

            return Result.Ok(tarefa);
        }
    }
}