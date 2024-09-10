using FluentSimilarity.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FluentSimilarity.Extensions;

public static class SimilarityCollectionExtension
{
    public static IServiceCollection AddScorersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var normalizerType = typeof(ISimilarity<>);

        // Find all classes that implement INormalizer<T> in the given assembly
        var similarities = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == normalizerType) && !t.IsAbstract && !t.IsInterface)
            .ToList();

        foreach (var similarity in similarities)
        {
            // Register the normalizer in the DI container
            var interfaces = similarity.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == normalizerType);

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, similarity);
            }
        }

        return services;

    }
}
