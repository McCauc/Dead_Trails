using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float turnSpeed = 360f;

    private PlayerAwareness playerAwareness;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        playerAwareness = PlayerAwareness.instance;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;
        if (playerAwareness == null) return;

        Vector3 playerPos = playerAwareness.GetPlayerPosition();
        Vector2 direction = (playerPos - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        animator.SetBool("IsMoving", true);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        animator.SetBool("IsMoving", false);
        animator.SetTrigger("IsDead");

        // Shut down the EnemyAttack script component instantly so it stops ticking damage
        EnemyAttack attackScript = GetComponent<EnemyAttack>();
        if (attackScript != null)
        {
            attackScript.enabled = false;
        }

        // Turn off the physical physics collider completely so the player can walk over the body
        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Grab the SpriteRenderer and push its visual rendering order underneath the player
        SpriteRenderer enemySpriteRenderer = GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer != null)
        {
            enemySpriteRenderer.sortingOrder = -4; 
        }

        if (ScoreController.Instance != null)
        {
            ScoreController.Instance.AddScore(10);
        }

        enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}