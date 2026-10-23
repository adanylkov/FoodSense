namespace FoodSense.ViewModels;

// Scoped so every view model in a circuit reports to the same banner.
public class AlertService
{
    private string? _success;
    private string? _error;

    public event Action? Changed;

    public string? Success
    {
        get => _success;
        set {
            if (_success != value)
            {
                _success = value;
                Changed?.Invoke();
            }
        }
    }

    public string? Error
    {
        get => _error;
        set {
            if (_error != value)
            {
                _error = value;
                Changed?.Invoke();
            }
        }
    }

    public void Clear()
    {
        _success = null;
        _error = null;
        Changed?.Invoke();
    }
}
