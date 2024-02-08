using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer weaponRenderer, characterRenderer;
    public Vector2 pointerPosition;
    public float damage;

    public Animator animator;
    public float delaySeconds;
    public bool canAttack = true;
    public Transform circlePos;
    public float radius;
    public bool isAttacking;
    ArrayList alreadyAttacked = new ArrayList();

    void Start()
    {
        characterRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (isAttacking) {
            DetectColliders();
        }
        
        if (isAttacking) return;

        alreadyAttacked = new ArrayList();

        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0){
            scale.y = -1;
        }else if (direction.x > 0){
            scale.y = 1;
        }

        transform.localScale = scale;   

        if (transform.localEulerAngles.z > 0 && transform.localEulerAngles.z < 180){
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }else{
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }

    }

    public void Attack(){
        if (!canAttack) return;
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(AttackDelay());
    }

    public void ResetIsAttacking(){
        isAttacking = false;
    }

    IEnumerator AttackDelay(){
        canAttack = false;
        yield return new WaitForSeconds(delaySeconds);
        canAttack = true;
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(circlePos.position, radius);
    }

    public void DetectColliders(){
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circlePos.position, radius)){
            if (collider.gameObject.CompareTag("Enemy") && !alreadyAttacked.Contains(collider.gameObject)){
                collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                alreadyAttacked.Add(collider.gameObject);
            }
        }
    }

}
