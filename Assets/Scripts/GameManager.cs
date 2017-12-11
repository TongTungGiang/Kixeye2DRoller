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

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    MinMaxConfig obstacleSpawnRate;

    [SerializeField]
    MinMaxConfig obstacleSpawnDistance;

    [SerializeField]
    MinMaxConfig obstacleVerticalOffset;

    [SerializeField]
    ObstacleSpawnConfiguration[] obstacles;

    [SerializeField]
    Transform obstacleSpawnPosition;
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
    #endregion

    public void StartGame()
    {
        Time.timeScale = 1.0f;
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
            Vector3 spawnPos = new Vector3(obstacleSpawnPosition.transform.position.x + obstacleSpawnDistance.RandomInsideRate,
                ForwardFloor.transform.position.y + obstacleVerticalOffset.RandomInsideRate);
            GameObject obstacle = obstaclePrefab.Spawn(spawnPos);
        }
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
