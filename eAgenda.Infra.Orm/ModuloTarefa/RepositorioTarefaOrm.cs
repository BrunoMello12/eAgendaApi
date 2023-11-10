using eAgenda.Dominio;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAgenda.Infra.Orm.ModuloTarefa
{
    public class RepositorioTarefaOrm : RepositorioBase<Tarefa>, IRepositorioTarefa
    {
      
        public RepositorioTarefaOrm(IContextoPersistencia contextoPersistencia) : base (contextoPersistencia)
        {
        }

        public override Tarefa SelecionarPorId(Guid id)
        {
            return registros
                .Include(x => x.Itens)
                .SingleOrDefault(x => x.Id == id);
        }

        async Task<List<Tarefa>> IRepositorioTarefa.SelecionarTodosAsync(StatusTarefaEnum status)
        {
            if (status == StatusTarefaEnum.Concluidas)
                return await registros
                    .Where(x => x.PercentualConcluido == 100).ToListAsync();

            else if (status == StatusTarefaEnum.Pendentes)
                return await registros
                    .Where(x => x.PercentualConcluido < 100).ToListAsync();

            else
                return await registros.ToListAsync();
        }
    }
}
