using eAgenda.Dominio;
using eAgenda.Dominio.ModuloDespesa;
using FluentResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloDespesa
{
    public class ServicoCategoria : ServicoBase<Categoria, ValidadorCategoria>
    {
        private IRepositorioCategoria repositorioCategoria;
        private IContextoPersistencia contextoPersistencia;

        public ServicoCategoria(IRepositorioCategoria repositorioCategoria, IContextoPersistencia contextoPersistencia)
        {
            this.repositorioCategoria = repositorioCategoria;
            this.contextoPersistencia = contextoPersistencia;
        }

        public async Task<Result<Categoria>> InserirAsync(Categoria categoria)
        {
            Result resultado = Validar(categoria);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            await repositorioCategoria.InserirAsync(categoria);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(categoria);
        }

        public async Task<Result<Categoria>> EditarAsync(Categoria categoria)
        {
            var resultado = Validar(categoria);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            repositorioCategoria.Editar(categoria);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(categoria);
        }

        public async Task<Result<Categoria>> ExcluirAsync(Guid id)
        {
            var categoriaResult = await SelecionarPorIdAsync(id);

            if (categoriaResult.IsSuccess)
                return await ExcluirAsync(categoriaResult.Value);

            return Result.Fail(categoriaResult.Errors);
        }

        public async Task<Result> ExcluirAsync(Categoria categoria)
        {
            repositorioCategoria.Excluir(categoria);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok();
        }

        public async Task<Result<List<Categoria>>> SelecionarTodosAsync()
        {
            var categorias = await repositorioCategoria.SelecionarTodosAsync();

            return Result.Ok(categorias);
        }

        public async Task<Result<Categoria>> SelecionarPorIdAsync(Guid id)
        {
            var categoria = await repositorioCategoria.SelecionarPorIdAsync(id);

            if (categoria == null)
            {
                Log.Logger.Warning($"Categoria {categoria.Id} não encontrada", id);

                return Result.Fail($"Categoria {id} não encontrada");
            }

            return Result.Ok(categoria);
        }
    }
}