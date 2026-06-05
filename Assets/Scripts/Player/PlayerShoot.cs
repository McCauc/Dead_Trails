using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootCooldown = 0.2f;

    private float shootTimer = 0f;

    void Start()
    {
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogError("Bullet prefab or shoot point not assigned!");
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        
        Vector2 shootDirection = transform.right;
        
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 90f;
        newBullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootDirection * bulletSpeed;
        }

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript == null)
        {
            bulletScript = newBullet.AddComponent<Bullet>();
        }
    }
}
