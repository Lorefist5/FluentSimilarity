using System.Reflection;
using FluentSimilarity.Abstraction;
using FluentSimilarity.Extensions;
using Microsoft.Extensions.DependencyInjection;

var app = new ServiceCollection()
    .AddScorersFromAssembly(Assembly.GetExecutingAssembly())
    .BuildServiceProvider();

//var firstTestP1 = new Person { FirstName = "John", LastName = "those", Age = 30, BirthDate = new DateTime(2003, 12, 25) };
//var firstTestP2 = new Person { FirstName = "John", LastName = "Does", Age = 30, BirthDate = new DateTime(2003, 12, 25) };

//var secondTestP1 = new Person { FirstName = "Andres", LastName = "Rodriguez", Age = 20, BirthDate = new DateTime(2003, 12, 25) };
//var secondTestP2 = new Person { FirstName = "Andress", LastName = "Rodriguez Jr", Age = 20, BirthDate = new DateTime(2004, 12, 25) };

//var jwTestP1 = new Person { FirstName = "Alice", LastName = "Smith", Age = 25, BirthDate = new DateTime(1998, 5, 15) };
//var jwTestP2 = new Person { FirstName = "Alice", LastName = "Smyth", Age = 25, BirthDate = new DateTime(1998, 5, 15) };

//var levTestP1 = new Person { FirstName = "Bob", LastName = "Johnson", Age = 40, BirthDate = new DateTime(1983, 3, 10) };
//var levTestP2 = new Person { FirstName = "Bob", LastName = "Johnsson", Age = 40, BirthDate = new DateTime(1983, 3, 10) };


var filtersTestP1 = new Person
{
    FirstName = "Bobs",
    LastName = "S",
    Age = 40,
    BirthDate = new DateTime(1983, 3, 10),
};
var filtersTestP2 = new Person
{
    FirstName = "Bob",
    LastName = "Shadi",
    Age = 40,
    BirthDate = new DateTime(1983, 3, 10),
};

var personSimilarity = app.GetRequiredService<ISimilarity<Person>>();

//double similarityScore = personSimilarity.Compare(firstTestP1, firstTestP2);
//double similarityScore2 = personSimilarity.Compare(secondTestP1, secondTestP2);
//double jwSimilarityScore = personSimilarity.Compare(jwTestP1, jwTestP2);
//var levSimilarityScore = personSimilarity.CompareWithDetails(levTestP1, levTestP2);
double filtersSimilarityScore = personSimilarity.Compare(filtersTestP1, filtersTestP2);

//Console.WriteLine($"Similarity Score: {similarityScore}%");
//Console.WriteLine($"Similarity Score 2: {similarityScore2}%");
//Console.WriteLine($"Jaro-Winkler Similarity Score: {jwSimilarityScore}%");
//Console.WriteLine($"Levenshtein Similarity Score: {levSimilarityScore.Highest}%");
Console.WriteLine($"Filters Similarity Score: {filtersSimilarityScore}%");
