using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;

    [Header("Enemy Stats")]

    float health;
    public float maxEnemyHealth;
    public int enemyDamage; 
    public float speed;
    public float enemyAttackCooldown;
    public float enemyAttackWaitTime;
    public bool canDealMelee;
    public float spawnDelay;
    public bool spawning;
    public EnemyAttributes enemyAttributes;
    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Transform room;
    bool canDealDamage = true;
    public LayerMask layerMask;
    private SpriteRenderer spriteRenderer;
    public Material whiteFlashMaterial;
    [Header("Feedback")]
    public int knockBackStrength;
    public float knockbackDelay;
    [HideInInspector]
    public bool isKnockedBack = false;
    public bool invulnerable = false;
    public GameObject spawnHole;

    bool inRangeOfPlayer;
    bool isAttacking;

    public float stationaryTimeLimit = 5f; // The time limit for being stationary
    public float stationaryDamage = 1f; // The damage per second when stationary

    private float stationaryTimer;


    private void Start() {
        enemyAttributes = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemyAttributes>();

        health = maxEnemyHealth * enemyAttributes.currentHealthMultiplier;
        enemyAttackCooldown *= enemyAttributes.currentAttackTimeMultiplier;
        speed *= enemyAttributes.currentSpeedMultiplier;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }
    

    private void Update() {
        if (rb.velocity == Vector2.zero) {
            // The enemy is stationary
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer >= stationaryTimeLimit) {
                // The enemy has been stationary for too long, apply damage
                health -= stationaryDamage * Time.deltaTime;
                if (health <= 0){
                    Die();
                }
            }
        } else {
            // The enemy is moving, reset the timer
            stationaryTimer = 0;
        }
    }

    private void FixedUpdate() {
        Move();

        if (rb.velocity.magnitude > speed) {
            rb.velocity = rb.velocity.normalized * speed;
        }

        if (invulnerable){
            StartCoroutine(InvulnerableDelay());
        }

    }

    public virtual void Move(){
        if (isKnockedBack || isAttacking || spawning) return;
        rb.velocity = Vector2.zero;
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        
        if (targetPosition.x < transform.position.x) {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (targetPosition.x > transform.position.x) {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        rb.MovePosition(targetPosition);
    }
    

    public void TakeDamage(float damage){
        if (invulnerable) return;
        health -= damage;
        
        //Knockback
        Vector2 direction = (transform.position - player.position).normalized;
        rb.AddForce(direction * knockBackStrength, ForceMode2D.Impulse);
        StartCoroutine(KnockbackDelay());
        StartCoroutine(EnemyHitFlash());
    }

    public virtual void Die(){
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(){
        inRangeOfPlayer = true;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!canDealMelee || spawning) return;
        if (other.collider.gameObject.CompareTag("Player") && canDealDamage){
            //player.GetComponent<PlayerController>().TakeDamage(enemyDamage);
            StartCoroutine(Attack());
            //print("damaged!");
        }
    }

    private void OnCollisionExit2D(){
        inRangeOfPlayer = false;
    }

    IEnumerator Attack(){
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sign(transform.localScale.x) * 20);
        isAttacking = true;
        yield return new WaitForSeconds(enemyAttackWaitTime);
        isAttacking = false;
        transform.rotation = Quaternion.identity;
        if (inRangeOfPlayer && canDealDamage){
            player.GetComponent<PlayerController>().TakeDamage(enemyDamage);
            StartCoroutine(AttackCooldown());
            print("damaged!");
        }
    }

    IEnumerator AttackCooldown(){
        canDealDamage = false;
        yield return new WaitForSeconds(enemyAttackCooldown);
        canDealDamage = true;
    }

    IEnumerator EnemyHitFlash(){
        Material origMaterial = spriteRenderer.material;
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(0.045f);
        spriteRenderer.material = origMaterial;

        if (health <= 0){
            Die();
        }
    }

    IEnumerator KnockbackDelay(){
        isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDelay);
        isKnockedBack = false;
    }

    IEnumerator InvulnerableDelay(){
        invulnerable = true;
        yield return new WaitForSeconds(0.5f);
        invulnerable = false;
    }

    IEnumerator SpawnDelay(){
        spawning = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "SpawnHole") child.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(spawnDelay - 1.83f);
    
        spawnHole.GetComponent<Animator>().SetTrigger("Spawn");
        //StartCoroutine(PlaySpawnAnimation());

        yield return new WaitForSeconds(1.83f);
        spawning = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            if (child.gameObject.name == "SpawnHole") child.transform.SetParent(gameObject.transform.parent.parent);
        }
    }

    public void Spawn(){
        StartCoroutine(SpawnDelay());
    }

}
