FluentSimilarity Library
========================

The FluentSimilarity library provides a fluent API for comparing objects and calculating similarity between their properties. It supports a wide variety of comparison methods, including string similarity, numeric similarity, and date-based similarity. This library is designed to be easy to extend and integrate into any C# project.

Installation
------------

To use the FluentSimilarity library, you can install the NuGet package:

    Install-Package FluentSimilarity

Getting Started
---------------

To use the FluentSimilarity library, you need to create similarity rules for each property of the object you want to compare. The library allows you to define rules for strings, numbers, and dates. Each rule can apply multiple comparison methods, and the highest similarity score is used.

### Defining Similarity for a Class

Below is an example where we define similarity rules for a `Person` class with properties for `FirstName`, `LastName`, `Age`, and `BirthDate`.

    
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }
    
    public class PersonSimilarity : AbstractSimilarity<Person>
    {
        public PersonSimilarity()
        {
            RuleFor(x => x.FirstName)
                .LevenshteinCompare()
                .SoundexCompare();
    
            RuleFor(x => x.LastName)
                .JaroWinklerCompare()
                .LevenshteinCompare();
    
            RuleFor(x => x.Age)
                .ExactMatch()
                .RangeSimilarity(10)
                .PercentageDifference(15);
    
            RuleFor(x => x.BirthDate)
                .ExactDateMatch()
                .PartialDateMatch();
        }
    }
        

### Using the Similarity Rules

Once you've defined the similarity rules for your class, you can use the `Compare` method to calculate the similarity between two objects.

    
    var person1 = new Person { FirstName = "John", LastName = "Doe", Age = 30, BirthDate = new DateTime(1993, 5, 21) };
    var person2 = new Person { FirstName = "Jon", LastName = "Doe", Age = 35, BirthDate = new DateTime(1992, 5, 21) };
    
    var personSimilarity = new PersonSimilarity();
    double similarityScore = personSimilarity.Compare(person1, person2);
    
    Console.WriteLine($"Similarity Score: {similarityScore}%");
        

In this case, the similarity score will be calculated based on the rules defined for each property, using the highest similarity score from the comparison methods.

Comparison Methods
------------------

The library supports a wide range of comparison methods, including:

*   **String Comparison:** Levenshtein, Jaro-Winkler, Soundex
*   **Number Comparison:** ExactMatch, RangeSimilarity, ProximityMatch, PercentageDifference
*   **DateTime Comparison:** ExactDateMatch, PartialDateMatch

### String Comparison Example

The library provides several string comparison methods, including Levenshtein, Soundex, and Jaro-Winkler. These methods calculate similarity between strings based on different algorithms. Here's an example of how to use the `LevenshteinCompare` and `SoundexCompare` methods for the `FirstName` property.

    
    RuleFor(x => x.FirstName)
        .LevenshteinCompare()
        .SoundexCompare();
        

### Number Comparison Example

The library supports various number comparison methods, such as `ExactMatch`, `RangeSimilarity`, and `PercentageDifference`.

    
    // Range similarity: calculates similarity based on how close two integers are within a given range.
    RuleFor(x => x.Age)
        .RangeSimilarity(10);  // 10-year range
    
    // Percentage difference: calculates similarity based on the percentage difference between two integers.
    // The score decreases as the percentage difference increases. If the difference exceeds the maxPercentage, the score is 0.
    RuleFor(x => x.Age)
        .PercentageDifference(15);  // Max percentage difference allowed is 15%
        

### DateTime Comparison Example

For `DateTime` properties, you can use `ExactDateMatch` to check for exact matches and `PartialDateMatch` for partial matches (same day and month, but different year, for example).

    
    RuleFor(x => x.BirthDate)
        .ExactDateMatch()
        .PartialDateMatch();
        

Advanced Features
-----------------

The library allows chaining of comparison methods for each property. The highest similarity score from the chained methods is used for the final similarity calculation.

### Custom Comparisons

You can define your own custom comparison rules using the `CustomComparison` method. Here's an example:

    
    RuleFor(x => x.FirstName)
        .CustomComparison((firstName1, firstName2) =>
        {
            return firstName1.Equals(firstName2, StringComparison.OrdinalIgnoreCase) ? 100.0 : 0.0;
        });
        

Conclusion
----------

FluentSimilarity is a flexible and extensible library that allows you to define custom similarity rules for comparing objects. Whether you're comparing strings, numbers, or dates, this library provides a fluent API that simplifies the process of calculating similarity scores.