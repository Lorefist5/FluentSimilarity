using System.Linq.Expressions;
using FluentSimilarity.Builder;
using FluentSimilarity.Models;

namespace FluentSimilarity.Abstraction;

public abstract class AbstractSimilarity<T> : ISimilarity<T>
{
    private readonly List<(Func<T, T, double> Rule, string PropertyName)> _rules = new();
    private readonly Dictionary<string, double> _weights = new(); // To store weights for properties

    /// <summary>
    /// Adds a rule for comparing a specific property of the object.
    /// </summary>
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

    /// <summary>
    /// Retrieves the property name from the provided property selector.
    /// </summary>
    private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> propertySelector)
    {
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        throw new ArgumentException("Invalid property selector expression");
    }

    /// <summary>
    /// Compares two objects using the defined rules and weights, excluding invalid scores (-1).
    /// </summary>
    public double Compare(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return 0;
        }

        // Filter out invalid scores (-1) and calculate total weight dynamically
        var validScoresAndWeights = _rules
            .Select(rule =>
            {
                var score = rule.Rule(obj1, obj2);
                if (score == -1)
                    return (Score: -1, Weight: 0); // Skip invalid scores
                var weight = _weights.ContainsKey(rule.PropertyName)
                    ? _weights[rule.PropertyName]
                    : 1;
                return (Score: score, Weight: weight);
            })
            .Where(x => x.Score != -1); // Exclude invalid scores

        // Calculate total weight
        var totalWeight = validScoresAndWeights.Sum(x => x.Weight);
        if (totalWeight == 0)
            return 0; // Prevent division by zero

        // Calculate weighted similarity
        var weightedScores = validScoresAndWeights.Sum(x => x.Score * x.Weight);
        return weightedScores / totalWeight; // Normalize by total weight
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
            .Select(rule => new ComparisonDetail
            {
                PropertyName = rule.PropertyName,
                Score = rule.Rule(obj1, obj2),
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
