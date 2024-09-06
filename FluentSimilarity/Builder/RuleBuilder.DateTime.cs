namespace FluentSimilarity.Builder;

public partial class RuleBuilder<T, TProperty>
{
    // Exact match for DateTime (returns 100% similarity if exactly equal)
    public RuleBuilder<T, TProperty> ExactDateMatch() 
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is DateTime date1 && value2 is DateTime date2)
            {
                return date1 == date2 ? 100.0 : 0.0;
            }
            return 0.0;
        });
    }

    // Check if two DateTime values are close within a specified number of days
    public RuleBuilder<T, TProperty> CloseDateMatch(int daysThreshold)
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is DateTime date1 && value2 is DateTime date2)
            {
                var difference = (date1 - date2).TotalDays;
                return Math.Abs(difference) <= daysThreshold ? 100.0 : 0.0;
            }
            return 0.0;
        });
    }

    // Check if two DateTime values are partially matching (e.g., same day/month, different years)
    public RuleBuilder<T, TProperty> PartialDateMatch()
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is DateTime date1 && value2 is DateTime date2)
            {
                double similarity = 0.0;

                // Check if day and month are the same
                if (date1.Day == date2.Day && date1.Month == date2.Month)
                {
                    similarity = 75.0; // High similarity for same day/month but different years
                }
                // Check if day and year are the same, but month is different
                else if (date1.Day == date2.Day && date1.Year == date2.Year)
                {
                    similarity = 50.0; // Medium similarity for same day/year but different months
                }
                // Check if month and year are the same, but day is different
                else if (date1.Month == date2.Month && date1.Year == date2.Year)
                {
                    similarity = 50.0; // Medium similarity for same month/year but different days
                }
                // Check if the year is the same but day and month are different
                else if (date1.Year == date2.Year)
                {
                    similarity = 25.0; // Low similarity for same year but different day/month
                }

                return similarity;
            }
            return 0.0;
        });
    }
}
