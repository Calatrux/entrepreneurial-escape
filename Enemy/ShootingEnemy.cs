using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootingCooldown;
    public int bulletSpeed;
    public int bulletDamage;
    bool canShoot = true;
    public float shootingRange;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Move(){
        if (isKnockedBack || spawning) return;
        rb.velocity = Vector2.zero;
        if (Vector2.Distance(transform.position, player.position) < shootingRange - 1){
            if (canShoot){
                Shoot();
                StartCoroutine(ShootingCooldown());
            }
        }else{
            base.Move();
        }

        if (player.position.x < transform.position.x) {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (player.position.x > transform.position.x) {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Calculate direction towards the player
        Vector2 shootDirection = (player.position - transform.position).normalized;

        rb.AddForce(shootDirection * bulletSpeed, ForceMode2D.Impulse);
    }

    IEnumerator ShootingCooldown(){
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

}
