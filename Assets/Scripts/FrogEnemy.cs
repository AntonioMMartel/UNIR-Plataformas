using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float horizontalSpeed = 2f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [SerializeField] float detectionRange = 10f;

    [SerializeField] Animator animator;

    [SerializeField] float jumpTimer;
    private float currentJumpTime;

    private bool isFacingRight = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumpTime = jumpTimer;
        
    }

    void Update()
    {
        GroundCheck();
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && currentJumpTime < 0)
        {
            rb.velocity = new Vector2(0, 0);
            currentJumpTime = jumpTimer;
            shouldJump = true;
        }
    }

    private void GroundCheck()
    {
        // Raycast no funciona porque se proyecta desde el centro de la transform
        // Necesitaría dos raycasts y ajustarlos con mucho sufrimiento -> mejor hitbox
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


    }
    private void OnDrawGizmosSelected() // Quiero ver el groundCheck
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    private void FixedUpdate()
    {
        currentJumpTime-= Time.deltaTime;
        if (isGrounded && shouldJump)
        {
            shouldJump = false;

            Vector2 direction = player.position - transform.position;

            // Flip towards player
            if (isFacingRight && direction.x < 0 || !isFacingRight && direction.x > 0)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            if (-detectionRange > direction.magnitude || direction.magnitude < detectionRange)
            {
                if (direction.x >= 0)
                {
                    rb.AddForce(new Vector2(horizontalSpeed, jumpForce), ForceMode2D.Impulse);

                }
                else
                {
                    rb.AddForce(new Vector2(-horizontalSpeed, jumpForce), ForceMode2D.Impulse);
                }
            }
        }   
    }


}
