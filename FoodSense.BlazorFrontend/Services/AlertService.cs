namespace FoodSense.ViewModels;

// Scoped so every view model in a circuit reports to the same banner.
public class AlertService
{
    public string? Success { get; set; }
    public string? Error { get; set; }

    public void Clear()
    {
        Success = null;
        Error = null;
    }
}
