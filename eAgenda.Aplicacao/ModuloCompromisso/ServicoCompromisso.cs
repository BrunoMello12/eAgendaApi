using eAgenda.Dominio;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using FluentResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloCompromisso
{
    public class ServicoCompromisso : ServicoBase<Compromisso, ValidadorCompromisso>
    {
        private IRepositorioCompromisso repositorioCompromisso;
        private IContextoPersistencia contextoPersistencia;

        public ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso,
                             IContextoPersistencia contextoPersistencia)
        {
            this.repositorioCompromisso = repositorioCompromisso;
            this.contextoPersistencia = contextoPersistencia;
        }

        public async Task<Result<Compromisso>> InserirAsync(Compromisso compromisso)
        {
            Result resultado = Validar(compromisso);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

           
                await repositorioCompromisso.InserirAsync(compromisso);

                await contextoPersistencia.GravarDadosAsync();

                return Result.Ok(compromisso);
        }

        public async Task<Result<Compromisso>> EditarAsync(Compromisso compromisso)
        {
            var resultado = Validar(compromisso);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

                repositorioCompromisso.Editar(compromisso);

                await contextoPersistencia.GravarDadosAsync();

                return Result.Ok(compromisso);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var compromissoResult = await SelecionarPorIdAsync(id);

            if (compromissoResult.IsSuccess)
                return await ExcluirAsync(compromissoResult.Value);

            return Result.Fail(compromissoResult.Errors);
        }

        public async Task<Result> ExcluirAsync(Compromisso compromisso)
        {
            repositorioCompromisso.Excluir(compromisso);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok();
        }


        public Result<List<Compromisso>> SelecionarCompromissosPassados(DateTime hoje)
        {
            return repositorioCompromisso.SelecionarCompromissosPassados(hoje);
        }

        public Result<List<Compromisso>> SelecionarCompromissosFuturos(DateTime dataInicial, DateTime dataFinal)
        {
            return repositorioCompromisso.SelecionarCompromissosFuturos(dataInicial, dataFinal);
        }

        public async Task<Result<List<Compromisso>>> SelecionarTodosAsync()
        {
            var compromissos = await repositorioCompromisso.SelecionarTodosAsync();

            return Result.Ok(compromissos);
        }

        public async Task<Result<Compromisso>> SelecionarPorIdAsync(Guid id)
        {
            var compromisso = await repositorioCompromisso.SelecionarPorIdAsync(id);

            if (compromisso == null)
            {
                Log.Logger.Warning("Compromisso {CompromissoId} não encontrado", id);

                return Result.Fail($"Compromisso {id} não encontrado");
            }

            return Result.Ok(compromisso);
        }
    }
}
