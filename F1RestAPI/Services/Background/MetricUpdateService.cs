using F1RestAPI.Services.IpConnectionCounts;
using F1RestAPI.Services.IpCountryStateServices;

namespace F1RestAPI.Services.Backgrounds;
public class MetricUpdateService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Timer _timer = null!;

    public MetricUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(UpdateMetrics, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    private async void UpdateMetrics(object? state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _ipCountryState = scope.ServiceProvider.GetRequiredService<IIpCountryStateService>();
            var _ipCountService = scope.ServiceProvider.GetRequiredService<IIpConnectionCount>();

            var countResult = await _ipCountService.Get15Connections();

            await _ipCountryState.SaveConnectionCountToCountryState(countResult.Value);

            await _ipCountryState.RecordConnection();
        }
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
