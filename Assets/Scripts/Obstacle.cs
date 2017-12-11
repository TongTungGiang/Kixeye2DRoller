using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayerMask;

    [SerializeField]
    int obstacleScore = 10;

    bool obstacleOvercome;

    PlayerController Player
    {
        get
        {
            if (_player == null)
                _player = FindObjectOfType<PlayerController>();

            return _player;
        }
    }

    void OnSpawn()
    {
        obstacleOvercome = false;
    }

    void Update()
    {
        if (obstacleOvercome)
            return;

        if (Physics2D.Raycast(transform.position, Vector2.up, 10, playerLayerMask))
        {
            Player.AddScore(obstacleScore);
            obstacleOvercome = true;
        }
    }

    /// <summary>
    /// Value holder, never meant to use directly;
    /// </summary>
    PlayerController _player = null;
}
