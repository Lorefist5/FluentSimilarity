namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    // Range similarity: calculates similarity based on how close two integers are within a given range.
    // If the absolute difference between the two numbers is within the specified range, it calculates the score.
    // The score is 100% when the numbers are equal, and decreases as the difference increases.
    // If the difference exceeds the range, the score is 0%.
    public RuleBuilder<T, TProperty> RangeSimilarity(int range)
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is int int1 && value2 is int int2)
            {
                var diff = Math.Abs(int1 - int2);
                return diff <= range ? 100.0 - (diff * 100.0 / range) : 0.0;
            }
            return 0.0;
        });
    }

    // Proximity match: checks if two integers are within a specific threshold.
    // If the absolute difference between the two numbers is less than or equal to the threshold, it returns 100%.
    // Otherwise, it returns 0%.
    public RuleBuilder<T, TProperty> ProximityMatch(int threshold)
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is int int1 && value2 is int int2)
            {
                return Math.Abs(int1 - int2) <= threshold ? 100.0 : 0.0;
            }
            return 0.0;
        });
    }
    // Percentage difference: calculates similarity based on the percentage difference between two integers.
    // The score decreases as the percentage difference increases.
    // If the percentage difference is within the maxPercentage, the score decreases linearly.
    // If the percentage difference exceeds maxPercentage, the score is 0%.
    // 
    // Example:
    // Given value1 = 80, value2 = 100, and maxPercentage = 25:
    //
    // 1. Calculate the absolute difference: 
    //    diff = Math.Abs(80 - 100) = 20
    //
    // 2. Calculate the percentage difference relative to the larger value:
    //    percentageDiff = (20 * 100.0) / 100 = 20%
    //
    // 3. Compare the percentage difference with maxPercentage (25%):
    //    Since 20% < 25%, calculate the score:
    //    score = 100.0 - (20 * 100.0 / 25) = 100.0 - 80 = 20.0%
    //
    // Therefore, the similarity score between 80 and 100 with a max percentage difference of 25% is 20.0%.
    //
    // If the percentage difference were larger than maxPercentage, the result would be 0%.
    public RuleBuilder<T, TProperty> PercentageDifference(int maxPercentage)
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is int int1 && value2 is int int2)
            {
                if (int1 == 0 || int2 == 0)
                {
                    return int1 == int2 ? 100.0 : 0.0;
                }

                var percentageDiff = (Math.Abs(int1 - int2) * 100.0) / Math.Max(int1, int2);
                return percentageDiff <= maxPercentage ? 100.0 - (percentageDiff * 100.0 / maxPercentage) : 0.0;
            }
            return 0.0;
        });
    }
}
