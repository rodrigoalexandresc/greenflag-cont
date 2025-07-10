namespace GreenFlag.ConsolidaDiario.ConsolidadorWorker
{
    public interface IConsolidaService
    {
        Task ConsolidarLancamentosDia(DateTime dia, CancellationToken cancellationToken);
    }
}
