using eAgenda.Dominio;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAgenda.Infra.Orm.ModuloDespesa
{
    public class RepositorioDespesaOrm : RepositorioBase<Despesa>, IRepositorioDespesa
    {
        public RepositorioDespesaOrm(IContextoPersistencia contextoPersistencia) : base(contextoPersistencia)
        {
        }

        public Task<List<Despesa>> SelecionarDespesasUltimos30Dias(DateTime dataAtual)
        {
            return registros
               .Where(x => x.Data >= dataAtual.AddDays(-30))               
               .ToListAsync();
        }

        public async Task<List<Despesa>> SelecionarDespesasAntigas(DateTime dataAtual)
        {
            return await registros
               .Where(x => x.Data <= dataAtual.AddDays(-30))               
               .ToListAsync();
        }

        public override Despesa SelecionarPorId(Guid id)
        {
            return registros
                .Include(x => x.Categorias)
                .SingleOrDefault(x => x.Id == id);
        }
    }
}