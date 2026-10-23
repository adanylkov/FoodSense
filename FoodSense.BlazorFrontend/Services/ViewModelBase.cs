namespace FoodSense.ViewModels;

public abstract class ViewModelBase(AlertService alerts)
{
    protected AlertService Alerts { get; } = alerts;

    public event Action? StateChanged;

    protected void NotifyStateChanged() => StateChanged?.Invoke();

    // Toggles the busy flag around the action and reports exceptions to the alert banner.
    protected async Task RunAsync(Func<Task> action, Action<bool>? setBusy = null)
    {
        setBusy?.Invoke(true);
        NotifyStateChanged();

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Alerts.Error = ex.Message;
        }
        finally
        {
            setBusy?.Invoke(false);
            NotifyStateChanged();
        }
    }
}
