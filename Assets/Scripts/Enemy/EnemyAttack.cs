using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    private bool touchingPlayer = false;
    private Health playerHealth;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (touchingPlayer &&
            playerHealth != null &&
            attackTimer >= attackCooldown)
        {
            playerHealth.TakeDamage(damage);
            attackTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            playerHealth = collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            playerHealth = null;
        }
    }
}