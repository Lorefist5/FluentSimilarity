namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;
    private readonly string _propertyName;
    private readonly List<Func<T, T, double>> _comparisons = new();
    private readonly List<Func<double, bool>> _filters = new();
    private Func<TProperty, bool> _propertyCondition = _ => true;

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

    public RuleBuilder<T, TProperty> NotHigherThan(double amount)
    {
        _filters.Add(score => score <= amount);
        return this;
    }

    public RuleBuilder<T, TProperty> When(Func<TProperty, bool> condition)
    {
        _propertyCondition = condition;
        return this;
    }

    public RuleBuilder<T, TProperty> WhenNot(Func<TProperty, bool> condition)
    {
        _propertyCondition = prop => !condition(prop);
        return this;
    }

    public Func<T, T, double> Build()
    {
        return (obj1, obj2) =>
        {
            var prop1 = _propertySelector(obj1);
            var prop2 = _propertySelector(obj2);

            if (!_propertyCondition(prop1) || !_propertyCondition(prop2))
            {
                return -1.0;
            }

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
