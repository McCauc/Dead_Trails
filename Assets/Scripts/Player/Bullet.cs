using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float maxBackupLifeTime = 4f;
    private bool hasHit = false;

    void Start()
    {
        // Keep backup self-destruction as a safety net
        Destroy(gameObject, maxBackupLifeTime);
    }

    void Update()
    {
        CheckScreenBoundsDestruction();
    }

    private void CheckScreenBoundsDestruction()
    {
        if (Camera.main == null) return;

        // Convert the bullet's 2D world position directly into Viewport percentages (from 0.0 to 1.0)
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // If the bullet leaves the viewport ranges (0 to 1), it is officially off-camera!
        if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasHit)
        {
            hasHit = true;
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            
            EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.Die();
                Destroy(gameObject, 0.02f);
            }
        }
    }
}