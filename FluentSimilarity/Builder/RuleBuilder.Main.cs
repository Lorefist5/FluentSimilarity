using SimMetrics.Net.Metric;
using XSoundex;

namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;
    private readonly string _propertyName;
    private readonly List<Func<T, T, double>> _comparisons = new();
    private readonly List<Func<double, bool>> _filters = new();

    public RuleBuilder(Func<T, TProperty> propertySelector, string propertyName)
    {
        _propertySelector = propertySelector;
        _propertyName = propertyName;
    }

    public RuleBuilder<T, TProperty> AddComparison(Func<TProperty, TProperty, double> comparison)
    {
        _comparisons.Add((obj1, obj2) => comparison(_propertySelector(obj1), _propertySelector(obj2)));
        return this;
    }

    public RuleBuilder<T, TProperty> Not(double amount)
    {
        _filters.Add(score => score != amount);
        return this;
    }

    public RuleBuilder<T, TProperty> NotLessThan(double amount)
    {
        _filters.Add(score => score >= amount);
        return this;
    }
    public RuleBuilder<T, TProperty> NotIn(string[] values)
    {
        _filters.Add(score => !values.Contains(score.ToString()));
        return this;
    }
    public RuleBuilder<T, TProperty> NotHigherThan(double amount)
    {
        _filters.Add(score => score <= amount);
        return this;
    }

    public Func<T, T, double> Build()
    {
        return (obj1, obj2) =>
        {
            var scores = _comparisons.Select(comp => comp(obj1, obj2));
            var filteredScores = scores.Where(score => _filters.All(filter => filter(score)));
            return filteredScores.Any() ? filteredScores.Max() : -1.0;
        };
    }

    public string GetPropertyName()
    {
        return _propertyName;
    }
}