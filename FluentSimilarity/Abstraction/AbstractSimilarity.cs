using FluentSimilarity.Builder;
using FluentSimilarity.Models;
using System.Linq.Expressions;

namespace FluentSimilarity.Abstraction;

public abstract class AbstractSimilarity<T> : ISimilarity<T>
{
    private readonly List<(Func<T, T, double> Rule, string PropertyName)> _rules = new();

    protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertySelector)
    {
        var propertyName = GetPropertyName(propertySelector);
        var ruleBuilder = new RuleBuilder<T, TProperty>(propertySelector.Compile(), propertyName);
        _rules.Add((ruleBuilder.Build(), propertyName));
        return ruleBuilder;
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

        // Calculate average similarity based on all rules
        var totalScore = _rules.Select(rule => rule.Rule(obj1, obj2)).Average();
        return totalScore;
    }

    public ComparingResults CompareWithDetails(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return new ComparingResults { Highest = 0, Lowest = 0, AllResults = new ComparisonDetail[0] };
        }

        var results = _rules.Select(rule => new ComparisonDetail
        {
            PropertyName = rule.PropertyName,
            Score = rule.Rule(obj1, obj2)
        }).ToArray();

        return new ComparingResults
        {
            Highest = results.Max(r => r.Score),
            Lowest = results.Min(r => r.Score),
            AllResults = results
        };
    }
}