using System.Linq.Expressions;
using FluentSimilarity.Builder;
using FluentSimilarity.Models;

namespace FluentSimilarity.Abstraction;

public abstract class AbstractSimilarity<T> : ISimilarity<T>
{
    private readonly List<(Func<T, T, double> Rule, string PropertyName)> _rules = new();
    private readonly Dictionary<string, double> _weights = new(); // To store weights for properties

    protected RuleBuilder<T, TProperty> RuleFor<TProperty>(
        Expression<Func<T, TProperty>> propertySelector
    )
    {
        var propertyName = GetPropertyName(propertySelector);
        var ruleBuilder = new RuleBuilder<T, TProperty>(propertySelector.Compile(), propertyName);
        _rules.Add((ruleBuilder.Build(), propertyName));
        return ruleBuilder;
    }

    /// <summary>
    /// Assigns a weight to a specific property.
    /// </summary>
    /// <typeparam name="TProperty">The property type</typeparam>
    /// <param name="propertySelector">The property to assign the weight to</param>
    /// <param name="weight">The weight amount</param>
    protected void WeightFor<TProperty>(
        Expression<Func<T, TProperty>> propertySelector,
        double weight
    )
    {
        var propertyName = GetPropertyName(propertySelector);
        if (_weights.ContainsKey(propertyName))
        {
            _weights[propertyName] = weight; // Update the weight if already exists
        }
        else
        {
            _weights.Add(propertyName, weight);
        }
    }

    private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> propertySelector)
    {
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        throw new ArgumentException("Invalid property selector expression");
    }

    public double Compare(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return 0;
        }

        // Get total weight sum for normalization
        var totalWeight = _weights.Values.DefaultIfEmpty(1).Sum(); // Default to 1 if no weights are defined

        // Calculate weighted similarity
        var weightedScores = _rules.Select(rule =>
        {
            var score = rule.Rule(obj1, obj2);
            if (score == -1)
                return 0; // Skip invalid scores

            // Apply weight if it exists, otherwise use 1 as default
            var weight = _weights.ContainsKey(rule.PropertyName) ? _weights[rule.PropertyName] : 1;
            return score * weight;
        });

        return totalWeight > 0 ? weightedScores.Sum() / totalWeight : 0;
    }

    public ComparingResults CompareWithDetails(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return new ComparingResults
            {
                Highest = 0,
                Lowest = 0,
                AllResults = new ComparisonDetail[0],
            };
        }

        var results = _rules
            .Select(rule =>
            {
                var score = rule.Rule(obj1, obj2);

                // Include the weight in the result details
                var weight = _weights.ContainsKey(rule.PropertyName)
                    ? _weights[rule.PropertyName]
                    : 1;

                return new ComparisonDetail { PropertyName = rule.PropertyName, Score = score };
            })
            .ToArray();

        return new ComparingResults
        {
            Highest = results.Max(r => r.Score),
            Lowest = results.Min(r => r.Score),
            AllResults = results,
        };
    }
}
