namespace eAgenda.WebApi.ViewModels.ModuloDespesa.Despesa
{
    public class VisualizarDespesaViewModel
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; }

        public List<string> Categorias { get; set; }
    }
}
