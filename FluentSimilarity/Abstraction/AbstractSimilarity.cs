using FluentSimilarity.Builder;

namespace FluentSimilarity.Abstraction;

public abstract class AbstractSimilarity<T> : ISimilarity<T>
{
    private readonly List<Func<T, T, double>> _rules = new();

    protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> propertySelector)
    {
        var ruleBuilder = new RuleBuilder<T, TProperty>(propertySelector);
        _rules.Add(ruleBuilder.Build());
        return ruleBuilder;
    }

    public double Compare(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return 0;
        }

        // Calculate average similarity based on all rules
        var totalScore = _rules.Select(rule => rule(obj1, obj2)).Average();
        return totalScore;
    }
}
