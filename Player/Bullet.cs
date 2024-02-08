using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    ParticleSystem bulletParticles;
    ParticleSystem.MainModule bulletParticlesSettings;
    SpriteRenderer spriteRenderer;
    public float lifetime;
    private void Start() {
        bulletParticles = GetComponentInChildren<ParticleSystem>();
        bulletParticlesSettings = bulletParticles.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Lifetime());
    }

    private void Update() {
        if (bulletParticles.isPlaying == false && spriteRenderer.enabled == false){
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Walls") || other.CompareTag("End")){
            PlayParticles(other);
        }

        if (other.CompareTag("Enemy")){
            other.GetComponent<Enemy>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
    

    IEnumerator Lifetime(){
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void PlayParticles(Collider2D other){
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        if (bulletParticles.isPlaying == false){
            if (other.gameObject.GetComponent<SpriteRenderer>() != null){
                bulletParticlesSettings.startColor = other.gameObject.GetComponent<SpriteRenderer>().color;
            }
            bulletParticles.Play();
            spriteRenderer.enabled = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }
    }
}
