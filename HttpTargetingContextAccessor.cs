using Microsoft.FeatureManagement.FeatureFilters;

namespace FeatureFlags;

public class HttpTargetingContextAccessor : ITargetingContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ValueTask<TargetingContext> GetContextAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var targetingContext = new TargetingContext
        {
            UserId = httpContext?.User?.Identity?.Name ?? "anonymous",
            Groups = httpContext?.User?.Claims
                .Where(c => c.Type == "group")
                .Select(c => c.Value)
                .ToList()
        };

        return new ValueTask<TargetingContext>(targetingContext);
    }
}
