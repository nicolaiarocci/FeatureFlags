using Microsoft.FeatureManagement;

namespace FeatureFlags;

[FilterAlias(nameof(MyCustomFilter))]

public class MyCustomFilter : IContextualFeatureFilter<MyCustomFilterContext>
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext evaluationContext, MyCustomFilterContext context)
    {
        var settings = evaluationContext.Parameters.Get<MyCustomFilterSettings>()
            ?? throw new ArgumentNullException(nameof(MyCustomFilterSettings));
        return Task.FromResult(settings.LuckyNumber == context.InputNumber);
    }
}

public class MyCustomFilterSettings
{
    public int LuckyNumber { get; set; }
}

public class MyCustomFilterContext
{
    public int InputNumber { get; set; }
}