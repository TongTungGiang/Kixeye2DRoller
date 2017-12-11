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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsBackFloorInsideCameraView)
        {
            BackFloor.transform.position = ForwardFloor.transform.position + new Vector3(floorStep, 0);
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
}
