using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private bool isGrounded = false;

    [Header("Gravity")]
    [SerializeField] float baseGravity = 2;
    [SerializeField] float maxFallSpeed = 18f;
    [SerializeField] float fallSpeedMultipier = 2f;

    [Header("WallCheck")]
    [SerializeField] Transform wallCheckPos;
    [SerializeField] Vector2 wallCheckSize = new Vector2(0.05f, 0.5f); // Este ti
    [SerializeField] LayerMask wallLayer; // Este para mirar quién tiene la tag ground

    [Header("GroundCheck")]
    [SerializeField] float wallSlideSpeed = 2f;
    [SerializeField] bool isWallSliding = false;

    float horizontalMovement;
    bool isFacingRight = true;
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
        Flip();
        ProcessWallSlide();
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
    
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        } else
        {
            isWallSliding = false;
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
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
        
    }

    private void Flip()
    {
        if(isFacingRight&& horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected() // Quiero ver el groundCheck
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
