using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumps = 2;
    private int remainingJumps;

    [Header("GroundCheck")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); // Este ti
    [SerializeField] LayerMask groundLayer; // Este para mirar quién tiene la tag ground

    [Header("Gravity")]
    [SerializeField] float baseGravity = 2;
    [SerializeField] float maxFallSpeed = 18f;
    [SerializeField] float fallSpeedMultipier = 2f;

    float horizontalMovement;
    // Start is called before the first frame update
    void Start()
    {
        remainingJumps = maxJumps;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        GroundCheck(); // Esto es terrorismo perdóname jefe
        Gravity();
    }

    public void Move(InputAction.CallbackContext context) { 
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (remainingJumps > 0)
        {

            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                remainingJumps--;
            }
            else if (context.canceled) // Salto chiquitin
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                remainingJumps--;
            }
        }
    }
    
    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultipier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            remainingJumps = maxJumps;
        }
    }

    private void OnDrawGizmosSelected() // Quiero ver el groundCheck
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
