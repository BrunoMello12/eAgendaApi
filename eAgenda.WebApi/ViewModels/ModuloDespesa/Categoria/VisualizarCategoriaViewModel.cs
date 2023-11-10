using eAgenda.WebApi.ViewModels.ModuloDespesa.Despesa;

namespace eAgenda.WebApi.ViewModels.ModuloDespesa.Categoria
{
    public class VisualizarCategoriaViewModel
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }

        public List<ListarDespesaViewModel> Despesas { get; set; }
    }
}
