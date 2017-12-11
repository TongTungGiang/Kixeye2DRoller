using UnityEngine;
using System.Collections;

public class ObstacleBoxModifier : MonoBehaviour
{
    [SerializeField]
    MinMaxConfig localVerticalPositionRange;

    [SerializeField]
    MinMaxConfig localVerticalScaleRange;

    void OnSpawn()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, localVerticalPositionRange.RandomInsideRate);
        transform.localScale = new Vector3(transform.localScale.x, localVerticalScaleRange.RandomInsideRate);
    }
}
