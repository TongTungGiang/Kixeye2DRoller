using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    /// <summary>
    /// The camera maintains a constant offset to the player.
    /// </summary>
    [SerializeField]
    float horizontalOffset;

    /// <summary>
    /// Reference to player transform.
    /// </summary>
    [SerializeField]
    Transform player;
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x + horizontalOffset, transform.position.y, transform.position.z);
    }
}
