using FluentSimilarity.Abstraction;

public class PersonSimilarity : AbstractSimilarity<Person>
{
    public PersonSimilarity()
    {
        RuleFor(x => x.FirstName)
            .LevenshteinCompare()
            .SoundexCompare();

        RuleFor(x => x.LastName)
            .JaroWinklerCompare()
            .LevenshteinCompare()
            .SoundexCompare();

        RuleFor(x => x.Age)
            .ExactMatch()
            .PercentageDifference(80);


        RuleFor( x => x.BirthDate)
            .ExactDateMatch()
            .PartialDateMatch();
    }
}