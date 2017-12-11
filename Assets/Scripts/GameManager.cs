using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Configurations
    /// <summary>
    /// I use two floor objects, swap the back floor to the place in front of
    /// the forward floor to create the feeling of infinite floor.
    /// </summary>
    [SerializeField]
    GameObject[] floors;

    /// <summary>
    /// How far are two floor objects from each other?
    /// </summary>
    [SerializeField]
    float floorStep;

    /// <summary>
    /// Reference to the main camera.
    /// </summary>
    [SerializeField]
    Transform mainCamera;

    /// <summary>
    /// How frequently does the GM spawn new obstacle?
    /// </summary>
    [SerializeField]
    MinMaxConfig obstacleSpawnRate;

    /// <summary>
    /// How far the obstacle is located from the destined position, horizontally.
    /// </summary>
    [SerializeField]
    MinMaxConfig obstacleHorizontalOffset;

    /// <summary>
    /// How far the obstacle is located from the destined position, vertically.
    /// </summary>
    [SerializeField]
    MinMaxConfig obstacleVerticalOffset;

    /// <summary>
    /// Obstacle prefabs, and their possibilities.
    /// </summary>
    [SerializeField]
    ObstacleSpawnConfiguration[] obstacles;

    /// <summary>
    /// Destined obstacle spawn position, relative to the player (actually it's one of his child transform).
    /// </summary>
    [SerializeField]
    Transform obstacleSpawnPosition;

    [SerializeField]
    TimeScaleProgression timeScaleProgression;
    #endregion

    #region Properties
    Transform BackFloor
    {
        get
        {
            if (floors[0].transform.position.x < floors[1].transform.position.x)
                return floors[0].transform;

            return floors[1].transform;
        }
    }

    Transform ForwardFloor
    {
        get
        {
            if (floors[0].transform != BackFloor)
                return floors[0].transform;

            return floors[1].transform;
        }
    }

    bool IsBackFloorInsideCameraView
    {
        get
        {
            float floorSize = floorStep / 2;
            float maxCameraPosX = BackFloor.transform.position.x + floorSize;
            float minCameraPosX = BackFloor.transform.position.x - floorSize;
            float currentCameraPosX = mainCamera.transform.position.x;

            return currentCameraPosX <= maxCameraPosX && currentCameraPosX >= minCameraPosX;
        }
    }

    GameObject RandomObstaclePrefab
    {
        get
        {
            float random = Random.Range(0, ObstaclesTotalWeight);
            float totalWeightsSoFar = 0;
            for (int i = 0; i < obstacles.Length; i++)
            {
                totalWeightsSoFar += obstacles[i].possibility;
                if (totalWeightsSoFar >= random)
                    return obstacles[i].prefab;
            }

            // Mathematically, this line will never be executed, anyways.
            return obstacles[0].prefab;
        }
    }

    float ObstaclesTotalWeight
    {
        get
        {
            // not initialized
            if (_obstacleTotalRandomWeight == -1)
            {
                _obstacleTotalRandomWeight = 0;
                foreach (ObstacleSpawnConfiguration o in obstacles)
                {
                    _obstacleTotalRandomWeight += o.possibility;
                }
            }

            return _obstacleTotalRandomWeight;
        }
    }
    #endregion

    #region Private variables
    float nextObstacleSpawn;
    float currentTimeScale = 1.0f;
    #endregion

    public void StartGame()
    {
        Time.timeScale = currentTimeScale;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    // Use this for initialization
    void Start()
    {
        nextObstacleSpawn = Time.time + obstacleSpawnRate.RandomInsideRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsBackFloorInsideCameraView)
        {
            BackFloor.transform.position = ForwardFloor.transform.position + new Vector3(floorStep, 0);
        }

        if (Time.time >= nextObstacleSpawn)
        {
            nextObstacleSpawn = Time.time + obstacleSpawnRate.RandomInsideRate;
            GameObject obstaclePrefab = RandomObstaclePrefab;
            Vector3 spawnPos = new Vector3(obstacleSpawnPosition.transform.position.x + obstacleHorizontalOffset.RandomInsideRate,
                ForwardFloor.transform.position.y + obstacleVerticalOffset.RandomInsideRate);
            GameObject obstacle = obstaclePrefab.Spawn(spawnPos);
        }

        currentTimeScale = timeScaleProgression.GetTimeScale(Time.time);
        Time.timeScale = currentTimeScale;
    }

    /// <summary>
    /// Value holder, never meant to use directly.
    /// </summary>
    /// <see cref="ObstaclesTotalWeight"/>
    float _obstacleTotalRandomWeight = -1;
}

[System.Serializable]
class ObstacleSpawnConfiguration
{
    public GameObject prefab;
    public float possibility;
}

/// <summary>
/// When the game time reaches the first value (a.k.a x value), the time scale would gradually increase, 
/// until the time scale reaches its max value.
/// </summary>
[System.Serializable]
class TimeScaleProgression
{
    [SerializeField]
    Vector2 milestones;

    [SerializeField]
    Vector2 timeScale;

    public float GetTimeScale(float time)
    {
        if (time < milestones.x)
            return timeScale.x;

        if (time > milestones.y)
            return timeScale.y;

        return (time - milestones.x) / (milestones.y - milestones.x) * (timeScale.y - timeScale.x) + timeScale.x;
    }
}