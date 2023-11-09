using eAgenda.Dominio;
using eAgenda.Dominio.ModuloContato;
using FluentResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloContato
{
    public class ServicoContato : ServicoBase<Contato, ValidadorContato>
    {
        private IRepositorioContato repositorioContato;
        private IContextoPersistencia contextoPersistencia;

        public ServicoContato(
            IRepositorioContato repositorioContato,
            IContextoPersistencia contexto)
        {
            this.repositorioContato = repositorioContato;
            this.contextoPersistencia = contexto;
        }

        public async Task<Result<Contato>> InserirAsync(Contato contato)
        {
            Result resultado = Validar(contato);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            await repositorioContato.InserirAsync(contato);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(contato);
        }

        public async Task<Result<Contato>> EditarAsync(Contato contato)
        {
            var resultado = Validar(contato);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            repositorioContato.Editar(contato);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(contato);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var contatoResult = await SelecionarPorIdAsync(id);

            if (contatoResult.IsSuccess)
                return await ExcluirAsync(contatoResult.Value);

            return Result.Fail(contatoResult.Errors);
        }

        public async Task<Result> ExcluirAsync(Contato contato)
        {
            repositorioContato.Excluir(contato);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok();
        }

        //Task<Result<List<Contato>>>
        //  -> List<>   = Retorna vários contatos de maneira fácil a manipulação
        //  -> Result<> = Numa estrutura que facilita a resposta de sucesso ou falha 
        //  -> Task<>   = De maneira assíncrona        
        public async Task<Result<List<Contato>>> SelecionarTodosAsync(StatusFavoritoEnum statusFavorito)
        {
            var contatos = await repositorioContato.SelecionarTodosAsync(statusFavorito);

            return Result.Ok(contatos);
        }

        public async Task<Result<Contato>> SelecionarPorIdAsync(Guid id)
        {
            var contato = await repositorioContato.SelecionarPorIdAsync(id);

            if (contato == null)
            {
                Log.Logger.Warning("Contato {ContatoId} não encontrado", id);

                return Result.Fail($"Contato {id} não encontrado");
            }

            return Result.Ok(contato);
        }

        public async Task<Result<Contato>> FavoritarAsync(Contato contato)
        {
            contato.ConfigurarFavorito();

            repositorioContato.Editar(contato);

            await contextoPersistencia.GravarDadosAsync();

            return Result.Ok(contato);
        }
    }
}