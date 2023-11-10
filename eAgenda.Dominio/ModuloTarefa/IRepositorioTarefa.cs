using eAgenda.Dominio.Compartilhado;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Dominio.ModuloTarefa
{
    public interface IRepositorioTarefa : IRepositorio<Tarefa>
    {
        public Task<List<Tarefa>> SelecionarTodosAsync(StatusTarefaEnum status);
    }
}