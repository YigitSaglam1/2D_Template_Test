using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D playerRb;
    Animator animator;
    
    private Vector2 lastMotionVector;

    private float horizontal;
    private float vertical;
    private float loggingCD = 0.2f;
    private float moveLimiter = 0.7f;
    private float Maxhealth = 100;
    private float health;
    private float healthGainAmount = 10f;

    private bool canDash = true;
    private bool isDashing= false;
    private bool canLog = true;
    private bool isLogging = false;
    private bool coroutinePlaying = false;

    private readonly int horizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
    private readonly int verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private readonly int isLoggingHash = Animator.StringToHash("isLogging");

    [SerializeField] private GameObject toolTip;
    [SerializeField] private SO_Ally playerSO;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        toolTip.SetActive(false);
        health = Maxhealth;
    }

    private void Update()
    {
        ToolTipMove();
        horizontal = Input.GetAxisRaw("Horizontal");// -1 is left
        vertical = Input.GetAxisRaw("Vertical");// -1 is down
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !coroutinePlaying) 
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.F) && canLog && !coroutinePlaying && !(horizontal != 0 || vertical != 0))
        {
            StartCoroutine(Logging());
        }
        if (isDashing) { return; }
        if (isLogging) { return; }
        Movement();
    }

    private void Movement()
    {

        animator.SetFloat(horizontalSpeedHash, horizontal);
        animator.SetFloat(verticalSpeedHash, vertical);
        if (horizontal != 0 && vertical != 0) //Diagonal Speed Limiter
        {
            if (horizontal == -1){ spriteRenderer.flipX = true; }
            if (horizontal == 1) { spriteRenderer.flipX = false; }
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
            if (horizontal != 0 || vertical != 0)
            {
                lastMotionVector = new Vector2(horizontal, vertical);
                lastMotionVector = lastMotionVector * 0.75f;
            }
            if (horizontal ==-1)
            {
                spriteRenderer.flipX = true;
            }
        }
        if (horizontal == -1) { spriteRenderer.flipX = true; }
        if (horizontal == 1) { spriteRenderer.flipX = false; }
        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical);
            lastMotionVector = lastMotionVector * 0.75f;
        }
        playerRb.velocity = new Vector2(horizontal * playerSO.runSpeed, vertical * playerSO.runSpeed);

    }
    private IEnumerator ToolTipMove()
    {
        toolTip.SetActive(true);
        toolTip.transform.position = transform.position;
        yield return new WaitForSeconds(0.3f);
        toolTip.transform.position = transform.position + new Vector3(lastMotionVector.x,lastMotionVector.y, 0f);
        yield return new WaitForSeconds(0.1f);
        toolTip.SetActive(false);
    }

    private IEnumerator Dash()
    {
        coroutinePlaying = true;
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;
        playerRb.velocity = new Vector2(horizontal * playerSO.dashingPower, vertical * playerSO.dashingPower);
        yield return new WaitForSeconds(playerSO.dashingTime);
        isDashing=false;
        trailRenderer.emitting = false;
        coroutinePlaying = false;
        yield return new WaitForSeconds(playerSO.dashingCooldown);
        canDash = true;
        
    }
    private IEnumerator Logging()
    {
        StartCoroutine(ToolTipMove());
        coroutinePlaying = true;
        canLog = false;
        isLogging = true;
        animator.SetBool(isLoggingHash, true);
        yield return new WaitForSeconds(0.45f);
        isLogging = false;
        animator.SetBool(isLoggingHash, false);
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(loggingCD);
        canLog = true;
        coroutinePlaying = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealObject"))
        {
            health += healthGainAmount;
            Debug.Log(health);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("EnemyWeapon"))
        {
            health -= healthGainAmount; // REVÝZE EDÝLECEK !
            Debug.Log(health);
        }
    }

}
