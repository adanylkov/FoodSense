namespace FoodSense.ViewModels;

public static class FuncTaskExtensions
{
    // Awaits every subscriber, unlike a plain multicast invoke which only returns the last task.
    public static Task InvokeAllAsync(this Func<Task>? handlers) =>
        handlers is null
            ? Task.CompletedTask
            : Task.WhenAll(handlers.GetInvocationList().Cast<Func<Task>>().Select(h => h()));
}
