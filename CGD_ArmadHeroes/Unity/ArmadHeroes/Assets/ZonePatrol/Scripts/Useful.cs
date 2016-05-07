using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Useful
{

    public static T[] shuffleArray<T>(T[] array, int seed)
    {
        // pseud random number generator
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}

