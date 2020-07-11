using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntRange 
{
    //sets a minimum and maximum integers for the random number generator
    public int mMin;
    public int mMax;

    
    public IntRange(int min, int max)
    {
        mMin = min;
        mMax = max;
    }

    public int Random
    {
        get { return UnityEngine.Random.Range(mMin, mMax); }
    }

}
