namespace FeatureFlags;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder WithEndpointFeatureFilter(this IEndpointConventionBuilder endpoint, string featureFlag)
    {
        endpoint.AddEndpointFilter(new FeatureFilter(featureFlag));
        return endpoint;
    }
}