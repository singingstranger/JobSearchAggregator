namespace JobSearchAPI.job_search_backend.Services;
public class ProviderRateLimiter
{
    private readonly SemaphoreSlim _semaphore = new(1,1);
    private DateTime _lastRequest = DateTime.MinValue;

    private readonly TimeSpan _delay;

    public ProviderRateLimiter(TimeSpan delay)
    {
        _delay = delay;
    }

    public async Task WaitAsync()
    {
        await _semaphore.WaitAsync();

        var elapsed = DateTime.UtcNow - _lastRequest;

        if (elapsed < _delay)
            await Task.Delay(_delay - elapsed);

        _lastRequest = DateTime.UtcNow;

        _semaphore.Release();
    }
}