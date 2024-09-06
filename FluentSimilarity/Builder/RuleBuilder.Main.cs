using SimMetrics.Net.Metric;
using XSoundex;

namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;
    private readonly List<Func<TProperty, TProperty, double>> _comparisonFuncs = new();

    public RuleBuilder(Func<T, TProperty> propertySelector)
    {
        _propertySelector = propertySelector;
    }

    public RuleBuilder<T, TProperty> AddComparison(Func<TProperty, TProperty, double> comparisonFunc)
    {
        _comparisonFuncs.Add(comparisonFunc);
        return this;
    }

    public Func<T, T, double> Build()
    {
        return (obj1, obj2) =>
        {
            var value1 = _propertySelector(obj1);
            var value2 = _propertySelector(obj2);

            if (_comparisonFuncs.Count > 0)
            {
                // Return the maximum similarity score among all comparisons for this property
                return _comparisonFuncs.Select(func => func(value1, value2)).Max();
            }

            return 0.0;
        };
    }
}
