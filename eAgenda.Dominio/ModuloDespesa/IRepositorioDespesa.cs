using eAgenda.Dominio.Compartilhado;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAgenda.Dominio.ModuloDespesa
{
    public interface IRepositorioDespesa : IRepositorio<Despesa>
    {
        Task<List<Despesa>> SelecionarDespesasAntigas(DateTime data);
        Task<List<Despesa>> SelecionarDespesasUltimos30Dias(DateTime dataBase);
    }
}
