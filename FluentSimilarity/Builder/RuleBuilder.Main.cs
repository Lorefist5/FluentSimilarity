using SimMetrics.Net.Metric;
using XSoundex;

namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;
    private readonly string _propertyName;
    private readonly List<Func<T, T, double>> _comparisons = new();

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

    public Func<T, T, double> Build()
    {
        return (obj1, obj2) => _comparisons.Select(comp => comp(obj1, obj2)).Average();
    }

    public string GetPropertyName()
    {
        return _propertyName;
    }
}