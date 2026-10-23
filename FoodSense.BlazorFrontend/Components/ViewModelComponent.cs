using FoodSense.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FoodSense.BlazorFrontend.Components;

// Injects the view model and re-renders whenever it reports a state change.
public abstract class ViewModelComponent<TViewModel> : ComponentBase, IDisposable
    where TViewModel : ViewModelBase
{
    [Inject] protected TViewModel VM { get; set; } = default!;

    protected override void OnInitialized() => VM.StateChanged += HandleStateChanged;

    private void HandleStateChanged() => InvokeAsync(StateHasChanged);

    public void Dispose() => VM.StateChanged -= HandleStateChanged;
}
