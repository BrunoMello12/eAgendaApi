using eAgenda.Dominio.ModuloContato;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Dominio.Compartilhado
{
    public interface IRepositorio<T> where T : EntidadeBase<T>
    {
        void Inserir(T novoRegistro);

        void Editar(T registro);

        void Excluir(T registro);

        List<T> SelecionarTodos();

        T SelecionarPorId(Guid numero);

        Task<bool> InserirAsync(T novoRegistro);
        Task<List<T>> SelecionarTodosAsync();
        Task<T> SelecionarPorIdAsync(Guid numero);
    }
}
