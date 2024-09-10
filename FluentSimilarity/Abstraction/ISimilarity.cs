using FluentSimilarity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSimilarity.Abstraction;

public interface ISimilarity<T>
{
    /// <summary>
    /// Compares two objects of type T and returns a similarity score between 0 and 100%.
    /// </summary>
    double Compare(T obj1, T obj2);

    /// <summary>
    /// Compares two objects of type T and returns a ComparingResults object.
    /// </summary>
    ComparingResults CompareWithDetails(T obj1, T obj2);
}