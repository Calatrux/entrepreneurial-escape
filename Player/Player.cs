using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int id = 0;
    [Header("Movement")]
    public Camera mainCamera;
    public float moveSpeed;
    public Rigidbody2D rb;
    Vector2 moveDir;
    Vector2 mousePos;
    Vector3 origPos;

    [Header("Camera")]
    public Transform cameraFocus;
    public float cameraTargetDivider;

    [Header("Dashing")]

    public float dashSpeed;
    public float dashLength;
    public float dashCooldown;
    bool dashing;
    bool canDash = true;

    [Header("Components")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public WeaponParent weaponParent;
    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    public bool playerAlive = true;
    public bool playerWon = false;
    [Header("Effects")]
    public Material whiteFlashMaterial;
    public float invincibilityTime;
    bool invincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        origPos = transform.position;
        // if (id == 0)
        // {
        //     //print(instance.moveSpeed);
        //     Destroy(instance.gameObject);
        // }

        // instance = this;
        // DontDestroyOnLoad(gameObject);

        // if (id == 1)
        // {
        //     instance = this;
        // }else
        // {
        //     Destroy(gameObject);
        // }

        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!playerAlive || playerWon) return;
        ProcessInputs();
        MouseLookahead();
        Dash();
        HandleAnimations();
    }

    void FixedUpdate(){
        Move();
    }

    void ProcessInputs(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
        if (mainCamera == null) mainCamera = Camera.main;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        weaponParent.pointerPosition = mousePos;

        if (Input.GetMouseButtonDown(0)){
            Attack();
        }

        id = 1;
    }

    void Move(){
        if (dashing) return;
        rb.velocity = Vector2.zero;

        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed) * Time.fixedDeltaTime;
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg  + (-3 * Mathf.Sign(transform.rotation.z));
        
        FlipPlayer(angle);
        // rb.rotation = angle;


    }

    void Dash(){
        if (Input.GetKeyDown(KeyCode.Space) && !dashing && canDash){
            Vector2 lookDir = mousePos - rb.position;
            float pen = 1;
            if ((lookDir.x > 0.5f || lookDir.x < -0.5f) && (lookDir.y > 0.5f || lookDir.y < -0.5f)) {
                pen = 0.8f;
            }

            Vector2 dashDirVector = new Vector2(lookDir.x * pen, lookDir.y * pen).normalized;

            rb.velocity = dashDirVector * dashSpeed;

            StartCoroutine(DashRuntime());
        }
    }

    IEnumerator DashRuntime(){
        dashing = true;
        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(dashLength);

        dashing = false;
        
        rb.velocity = Vector2.zero;
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown(){
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void MouseLookahead(){
        Vector3 cameraTarget = (new Vector3(mousePos.x, mousePos.y, 0) + (cameraTargetDivider - 1) * transform.position) / cameraTargetDivider;
        cameraFocus.position = cameraTarget;
    }

    public void HandleAnimations(){
        if (moveDir.x != 0 || moveDir.y != 0){
            animator.SetBool("running", true);
        } else {
            animator.SetBool("running", false);
        }
    }

    public void FlipPlayer(float angle){
        if (angle < 90 && angle > -90){
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }

    public void Attack(){
        weaponParent.Attack();
    }

    public void TakeDamage(int damage){
        if (invincible) return;
        
        currentHealth -= damage;
        StartCoroutine(PlayerHitFlash());
        StartCoroutine(Invincibility());
        
        if (currentHealth <= 0){
            print("PLayer died!");
            playerAlive = false;
            
        }
    }

    IEnumerator PlayerHitFlash(){
        Material origMaterial = spriteRenderer.material;
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(0.045f);
        spriteRenderer.material = origMaterial;
    }

    IEnumerator Invincibility(){
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    public void ResetPlayerPosition()
    {
        transform.position = origPos;
    }

}
