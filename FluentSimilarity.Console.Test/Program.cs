var firstTestP1 = new Person { FirstName = "John", LastName = "those", Age = 30, BirthDate = new DateTime(2003, 12, 25) };
var firstTestP2 = new Person { FirstName = "John", LastName = "Does", Age = 30, BirthDate = new DateTime(2003, 12, 25) };

var secondTestP1 = new Person { FirstName = "Andres", LastName = "Rodriguez", Age = 20, BirthDate = new DateTime(2003, 12, 25) };
var secondTestP2 = new Person { FirstName = "Andress", LastName = "Rodriguez Jr", Age = 20, BirthDate = new DateTime(2004, 12, 25) };


var personSimilarity = new PersonSimilarity();
double similarityScore = personSimilarity.Compare(firstTestP1, firstTestP2);
double similarityScore2 = personSimilarity.Compare(secondTestP1, secondTestP2);

Console.WriteLine($"Similarity Score: {similarityScore}%");
Console.WriteLine($"Similarity Score 2: {similarityScore2}%");
