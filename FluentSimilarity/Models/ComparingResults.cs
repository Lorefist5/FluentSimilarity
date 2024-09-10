namespace FluentSimilarity.Models;

public class ComparingResults
{
    public double Highest { get; set; }
    public double Lowest { get; set; }
    public ComparisonDetail[] AllResults { get; set; }
}