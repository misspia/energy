using UnityEngine;
using System.Collections;
using System;

public class VizUtils
{
    public static float remap(float value, float min, float max, float newMin, float newMax)
    {
        return newMin + (newMax - newMin) * (value - min) / (max - min);
    }
    public static string Test(string str) 
    {
        return "Test string: " + str;
    }
}
