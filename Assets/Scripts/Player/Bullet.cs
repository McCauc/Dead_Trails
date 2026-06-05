using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 4f;
    private bool hasHit = false;

    void Start()
    {
        // Destroy bullet after 4 seconds if it hasn't hit anything
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if bullet hit an enemy
        if (collision.CompareTag("Enemy") && !hasHit)
        {
            hasHit = true;
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            
            EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.Die();
                // Destroy bullet after a tiny delay to ensure death is registered
                Destroy(gameObject, 0.02f);
            }
        }
    }
}
