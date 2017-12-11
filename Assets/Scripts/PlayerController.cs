using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to the player game object so that it can react to user input.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Configurations
    /// <summary>
    /// Serialize field for registering floor tag, I do not like magic strings.
    /// </summary>
    [SerializeField]
    string floor;

    /// <summary>
    /// Same as above, but for obstacle layer.
    /// </summary>
    [SerializeField]
    LayerMask obstacleLayer;

    /// <summary>
    /// How high the circle jumps?
    /// </summary>
    [SerializeField]
    float jumpHeight;

    /// <summary>
    /// How fast the player moves along the floor?
    /// </summary>
    [SerializeField]
    float horizontalMoveSpeed;
    #endregion

    #region Private variables
    Rigidbody2D rb;

    bool isDead;
    bool isJumping;
    #endregion

    public bool IsDead { get { return isDead; } }
    
    void Start()
    {
        isDead = false;
        isJumping = true;

        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (isDead)
            return;

        if (Input.GetAxis("Jump") > 0)
        {
            Jump();
        }

        MoveRight();
    }

    /// <summary>
    /// Player moves with a constant velocity along horizontal axis.
    /// This is not affected by his jumping state.
    /// </summary>
    void MoveRight()
    {
        transform.position += new Vector3(horizontalMoveSpeed * Time.deltaTime, 0);
    }

    void Jump()
    {
        if (isJumping)
            return;

        isJumping = true;
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == floor)
        {
            isJumping = false;
        }

        if (col.gameObject.layer == obstacleLayer)
        {
            isDead = true;
        }
    }
}
