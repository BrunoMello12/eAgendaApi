using eAgenda.Dominio;
using eAgenda.Dominio.ModuloDespesa;
using FluentResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloDespesa
{
    public class ServicoDespesa : ServicoBase<Despesa, ValidadorDespesa>
    {
        private IRepositorioDespesa repositorioDespesa;
        private IContextoPersistencia contextoPersistencia;

        public ServicoDespesa(IRepositorioDespesa repositorioDespesa,
                             IContextoPersistencia contexto)
        {
            this.repositorioDespesa = repositorioDespesa;
            this.contextoPersistencia = contexto;
        }

        public async Task<Result<Despesa>> InserirAsync(Despesa despesa)
        {
            Result resultado = Validar(despesa);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

                await repositorioDespesa.InserirAsync(despesa);

                await contextoPersistencia.GravarDadosAsync();

                return Result.Ok(despesa);
        }

        public async Task<Result<Despesa>> EditarAsync(Despesa despesa)
        {
            var resultado = Validar(despesa);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

                repositorioDespesa.Editar(despesa);

                await contextoPersistencia.GravarDadosAsync();
            
            return Result.Ok(despesa);
        }

        public async Task<Result<Despesa>> ExcluirAsync(Guid id)
        {
            var despesaResult = await SelecionarPorIdAsync(id);

            if (despesaResult.IsSuccess)
                return await ExcluirAsync(despesaResult.Value);

            return Result.Fail(despesaResult.Errors);
        }

        public async Task<Result> ExcluirAsync(Despesa despesa)
        {
            repositorioDespesa.Excluir(despesa);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok();
        }

        public async Task<Result<List<Despesa>>> SelecionarTodosAsync()
        {
            var despesas = await repositorioDespesa.SelecionarTodosAsync();

            return Result.Ok(despesas);
        }

        public async Task<Result<Despesa>> SelecionarPorIdAsync(Guid id)
        {
            var despesa = await repositorioDespesa.SelecionarPorIdAsync(id);

            if (despesa == null)
            {
                Log.Logger.Warning($"Despesa {despesa.Id} não encontrada", id);

                return Result.Fail($"Despesa {id} não encontrada");
            }

            return Result.Ok(despesa);
        }

        public Result<List<Despesa>> SelecionarDespesasAntigas(DateTime dataAtual)
        {
            Log.Logger.Debug("Tentando selecionar despesas antigas...");

            try
            {
                var despesas = repositorioDespesa.SelecionarDespesasAntigas(dataAtual);

                Log.Logger.Information("Despesas antigas selecionadas com sucesso");

                return Result.Ok(despesas);
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar selecionar as despesas antigas";

                Log.Logger.Error(ex, msgErro);

                return Result.Fail(msgErro);
            }
        }

        public Result<List<Despesa>> SelecionarDespesasUltimos30Dias(DateTime dataAtual)
        {
            Log.Logger.Debug("Tentando selecionar despesas recentes...");

            try
            {
                var despesas = repositorioDespesa.SelecionarDespesasUltimos30Dias(dataAtual);

                Log.Logger.Information("Despesas recentes selecionadas com sucesso");

                return Result.Ok(despesas);
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar selecionar as despesas recentes";

                Log.Logger.Error(ex, msgErro);

                return Result.Fail(msgErro);
            }
        }
    }
}