using Microsoft.FeatureManagement;

namespace FeatureFlags;

public sealed class EndpointFilter(string FeatureFlag) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManager>();
        var isEnabled = await featureManager.IsEnabledAsync(FeatureFlag);
        if (!isEnabled)
        {
            return TypedResults.NotFound();
        }

        return await next(context);
    }
}