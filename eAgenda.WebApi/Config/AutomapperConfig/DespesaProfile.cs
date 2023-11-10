using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa.Despesa;

namespace eAgenda.WebApi.Config.AutoMapperConfig
{
    public class DespesaProfile : Profile
    {
        public DespesaProfile()
        {
            CreateMap<FormsDespesaViewModel, Despesa>()
                .ForMember(destino => destino.Categorias, opt => opt.Ignore())
                .AfterMap<InserirCategoriasMappingAction>();

            CreateMap<FormsDespesaViewModel, Despesa>()
                .ForMember(destino => destino.Categorias, opt => opt.Ignore())
                .AfterMap(EditarCategoriasMappingAction);

            CreateMap<Despesa, ListarDespesaViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()));

            CreateMap<Despesa, VisualizarDespesaViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()))
                .ForMember(destino => destino.Categorias, opt => opt.MapFrom(origem => origem.Categorias.Select(x => x.Titulo)));

        }

        private void EditarCategoriasMappingAction(FormsDespesaViewModel viewModel, Despesa despesa)
        {
            viewModel.CategoriasSelecionadas = despesa.Categorias.Select(categoria => categoria.Id).ToList();
        }
    }

    public class InserirCategoriasMappingAction : IMappingAction<FormsDespesaViewModel, Despesa>
    {
        private readonly IRepositorioCategoria repositorioCategoria;

        public InserirCategoriasMappingAction(IRepositorioCategoria repositorioCategoria)
        {
            this.repositorioCategoria = repositorioCategoria;
        }

        public void Process(FormsDespesaViewModel source, Despesa destination, ResolutionContext context)
        {
            destination.Categorias = repositorioCategoria.SelecionarMuitos(source.CategoriasSelecionadas);
        }
    }
}
