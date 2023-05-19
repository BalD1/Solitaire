using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        List<T> result = enumerable.ToList();

        for (int i = 0; i < result.Count; i++)
        {
            T tmp = result[i];
            int randIndx = result.RandomIndex();
            result[i] = result[randIndx];
            result[randIndx] = tmp;
        }

        return result;
    }

    public static List<T> ShuffleToList<T>(this IEnumerable<T> enumerable)
    {
        List<T> result = enumerable.ToList();

        for (int i = 0; i < result.Count; i++)
        {
            T tmp = result[i];
            int randIndx = result.RandomIndex();
            result[i] = result[randIndx];
            result[randIndx] = tmp;
        }

        return result;
    }

    public static T[] ShuffleToArray<T>(this IEnumerable<T> enumerable)
    {
        T[] result = enumerable.ToArray();

        for (int i = 0; i < result.Length; i++)
        {
            T tmp = result[i];
            int randIndx = result.RandomIndex();
            result[i] = result[randIndx];
            result[randIndx] = tmp;
        }

        return result;
    }
}