using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    private int currentHP;
    private SpriteRenderer spriteRenderer;

    [SerializeField] HealthUI healthUI;
    private bool isInvincible = false;

    [SerializeField] float invincibilityFrame = 1f;
    private float invincibilityTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        healthUI.SetMaxHearts(maxHP);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) return;

        IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
        if (enemy != null) 
        {
            isInvincible = true;
            invincibilityTimer = invincibilityFrame;
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHP--;
        healthUI.UpdateHearts(currentHP);
        StartCoroutine(PlayerSpritePulse());
        if (currentHP < 0)
        {
            // game over
        }
    }

    IEnumerator PlayerSpritePulse()
    {
        while (isInvincible)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleIFrame();
    }

    private void HandleIFrame()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                invincibilityTimer = invincibilityFrame;
            }
        }
    }
}
