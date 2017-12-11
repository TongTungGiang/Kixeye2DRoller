using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField]
    float lifeTime;

    float destructTime;

    /// <summary>
    /// Called when object is spawned from pool
    /// </summary>
    void OnSpawn()
    {
        destructTime = Time.time + lifeTime;
    }

    void Update()
    {
        if (Time.time > destructTime)
            gameObject.Despawn();
    }
}
