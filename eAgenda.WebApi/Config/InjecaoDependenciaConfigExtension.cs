using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Infra.Orm;
using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infra.Orm.ModuloCompromisso;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Orm.ModuloDespesa;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.WebApi.Config.AutoMapperConfig;

namespace eAgenda.WebApi.Config
{
    public static class InjecaoDependenciaConfigExtension
    {
        public static void ConfigurarInjecaoDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer");

            services.AddDbContext<IContextoPersistencia, eAgendaDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });

            services.AddTransient<IRepositorioContato, RepositorioContatoOrm>();
            services.AddTransient<ServicoContato>();

            services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoOrm>();
            services.AddTransient<ServicoCompromisso>();

            services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
            services.AddTransient<ServicoCategoria>();

            services.AddScoped<IRepositorioDespesa, RepositorioDespesaOrm>();
            services.AddTransient<ServicoDespesa>();

            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
            services.AddTransient<ServicoTarefa>();

            services.AddTransient<InserirCategoriasMappingAction>();
        }
    }
}
