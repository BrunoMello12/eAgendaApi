using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApi.ViewModels.ModuloTarefa;

namespace eAgenda.WebApi.Config.AutoMapperConfig
{
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            CreateMap<FormsTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .AfterMap(AdicionarItensMappingAction);

            CreateMap<FormsTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .AfterMap(EditarItensMappingAction);

            CreateMap<Tarefa, ListarTarefaViewModel>()
                               .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                               .ForMember(destino => destino.Situacao, opt =>
                                   opt.MapFrom(origem => origem.PercentualConcluido == 100 ? "Concluído" : "Pendente"));

            CreateMap<Tarefa, VisualizarTarefaViewModel>()
                .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))

                .ForMember(destino => destino.Situacao, opt =>
                    opt.MapFrom(origem => origem.PercentualConcluido == 100 ? "Concluído" : "Pendente"))

                .ForMember(destino => destino.QuantidadeItens, opt => opt.MapFrom(origem => origem.Itens.Count));

            CreateMap<ItemTarefa, VisualizarItemTarefaViewModel>()
                .ForMember(destino => destino.Situacao, opt =>
                    opt.MapFrom(origem => origem.Concluido ? "Concluído" : "Pendente"));


            CreateMap<ItemTarefa, ItemTarefaViewModel>();
        }

        private void AdicionarItensMappingAction(FormsTarefaViewModel viewModel, Tarefa tarefa, ResolutionContext context)
        {
            if (viewModel.Itens == null)
                return;

            foreach (var itemVM in viewModel.Itens)
            {
                var item = new ItemTarefa(itemVM.Titulo);

                tarefa.AdicionarItem(item);
            }
        }

        private void EditarItensMappingAction(FormsTarefaViewModel viewModel, Tarefa tarefa, ResolutionContext context)
        {
            foreach (var itemVM in viewModel.Itens)
            {
                if (itemVM.Concluido)
                    tarefa.ConcluirItem(itemVM.Id);

                else
                    tarefa.MarcarPendente(itemVM.Id);
            }

            foreach (var itemVM in viewModel.Itens)
            {
                if (itemVM.Status == StatusItemTarefa.Adicionado)
                {
                    var item = new ItemTarefa(itemVM.Titulo);
                    tarefa.AdicionarItem(item);
                }
                else if (itemVM.Status == StatusItemTarefa.Removido)
                {
                    tarefa.RemoverItem(itemVM.Id);
                }
            }
        }
    }
}
