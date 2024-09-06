using SimMetrics.Net.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSoundex;

namespace FluentSimilarity.Builder;


public partial class RuleBuilder<T, TProperty>
{
    // Levenshtein Distance Comparison using SimMetrics
    public RuleBuilder<T, TProperty> LevenshteinCompare()
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is string str1 && value2 is string str2)
            {
                var levenshtein = new Levenstein();
                var result = levenshtein.GetSimilarity(str1, str2) * 100;
                return result;// SimMetrics returns a score between 0 and 1, so multiply by 100
            }
            return 0.0;
        });
    }


    // Soundex Phonetic Matching using XSoundex
    public RuleBuilder<T, TProperty> SoundexCompare()
    {
        return AddComparison((value1, value2) =>
        {
            if (value1 is string str1 && value2 is string str2)
            {
                var soundex = new Soundex();
                var soundex1 = soundex.GenerateSoundex(str1);
                var soundex2 = soundex.GenerateSoundex(str2);

                // If Soundex codes are identical, return 100%
                if (soundex1 == soundex2)
                {
                    return 100.0;
                }

                // Uses Jaro-Winkler for partial similarity if Soundex doesn't match
                var jaroWinkler = new SimMetrics.Net.Metric.JaroWinkler();
                double jaroWinklerScore = jaroWinkler.GetSimilarity(str1, str2) * 100;

                // Return a blended score: weigh Soundex more heavily (e.g., 60% Soundex, 40% Jaro-Winkler)
                return jaroWinklerScore * 0.4; // If Soundex fails, relies on Jaro-Winkler
            }
            return 0.0;
        });
    }

}