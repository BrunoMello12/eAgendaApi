using AutoMapper;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa.Categoria;

namespace eAgenda.WebApi.Config.AutoMapperConfig
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<FormsCategoriaViewModel, Categoria>();

            CreateMap<Categoria, ListarCategoriaViewModel>();

            CreateMap<Categoria, VisualizarCategoriaViewModel>();

            CreateMap<Categoria, CategoriaSelecionadaViewModel>();
        }
    }
}
