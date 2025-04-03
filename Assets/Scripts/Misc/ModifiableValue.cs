using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


/// <summary>
/// °ø“¹, ¹æ“¹ µî
/// </summary>
public class IntegerModifier
{
    internal float multiplier;
	internal int? multipliableAddition;

	internal int? fixedAddition;

	internal int? fixedValue;

	internal int min;
	internal int max;

    public IntegerModifier(int min = int.MinValue, int max = int.MaxValue)
    {
		multiplier = 1;

        this.min = min;
        this.max = max;
    }

    public int Modify(int value)
    {
        if(fixedValue == null)
        {
            return Mathf.Clamp(Mathf.RoundToInt((value + (int)multipliableAddition) * multiplier + (int)fixedAddition), min, max);
        }
        return (int)fixedValue;
    }
}

public class FloatModifier
{
	internal float multiplier;
	internal float? multipliableAddition;

	internal float? fixedAddition;

	internal float? fixedValue;

	internal float min;
	internal float max;

	public FloatModifier(float min = float.MinValue, float max = float.MaxValue)
	{
		multiplier = 1;

		this.min = min;
		this.max = max;
	}

	public float Modify(float value)
	{
		if (fixedValue == null)
		{
			return Mathf.Clamp((value + (float)multipliableAddition) * multiplier + (float)fixedAddition, min, max);
		}
		return (float)fixedValue;
	}
}