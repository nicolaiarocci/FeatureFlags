using Microsoft.FeatureManagement;

namespace FeatureFlags;

[FilterAlias(nameof(MyCustomFilter))]
public class MyCustomFilter : IFeatureFilter
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext evaluationContext)
    {
        var settings = evaluationContext.Parameters.Get<MyCustomFilterSettings>()
            ?? throw new ArgumentNullException(nameof(MyCustomFilterSettings));
        return Task.FromResult(settings.LuckyNumber == 47);
    }
}

public class MyCustomFilterSettings
{
    public int LuckyNumber { get; set; }
}