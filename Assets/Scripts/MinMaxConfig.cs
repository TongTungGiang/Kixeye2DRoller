using UnityEngine;
using System.Collections;

[System.Serializable]
public class MinMaxConfig
{
    public float RandomInsideRate
    {
        get
        {
            return Random.Range(min, max);
        }
    }

    public bool IsInRange(float value)
    {
        return (value >= min && value <= max);
    }

    [SerializeField]
    float min;

    [SerializeField]
    float max;
}
