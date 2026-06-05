using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime = 1.5f;
    private bool isInvincible = false;
    private bool isDead = false;

    [Header("UI")]
    [SerializeField] private Slider healthBar;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Animator animator;
    private PlayerMovement movement;
    private PlayerShoot shoot;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shoot = GetComponent<PlayerShoot>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible)
            return;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        StartCoroutine(Flash());

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (movement != null)
            movement.enabled = false;

        if (shoot != null)
            shoot.enabled = false;

        if (animator != null)
            animator.SetTrigger("IsDead");

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        gameObject.layer = LayerMask.NameToLayer("Corpse");

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -5;
        }

        if (DeathMenuController.Instance != null)
        {
            DeathMenuController.Instance.TriggerDeathMenu();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}