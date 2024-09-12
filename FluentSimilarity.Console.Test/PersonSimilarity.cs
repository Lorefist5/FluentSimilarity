using FluentSimilarity.Abstraction;

public class PersonSimilarity : AbstractSimilarity<Person>
{
    public PersonSimilarity()
    {
        RuleFor(x => x.FirstName)
            .LevenshteinCompare()
            .SoundexCompare()
            .NotLessThan(50);

        RuleFor(x => x.LastName)
            .SoundexCompare()
            .JaroWinklerCompare()
            .LevenshteinCompare()
            .When((s1, s2) => s1.Length / s2.Length <= 2 && s2.Length / s1.Length <= 2);

        RuleFor(x => x.Age)
            .ExactMatch()
            .PercentageDifference(80);


        RuleFor( x => x.BirthDate)
            .ExactDateMatch()
            .PartialDateMatch();
    }
}