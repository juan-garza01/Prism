using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true;
    public float moveSpeed = 5f, dashCooldown = 0f, dashTime = 2f;
    private bool stepping = false;
    private int teleport = 2;
    private bool dash;
    [SerializeField] private LayerMask dl;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource walking;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    Vector2 lastmove;
    public Transform swordAttack;
    private Vector3 dir;
    private GameManagerScript gm;
    private void Awake(){
        gm = GameObject.Find("GameManager").transform.GetComponent<GameManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        dash = false;
        lastmove = new Vector2(0,-1);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.isPaused){
            if(dashCooldown > 0){
                dashCooldown -= Time.deltaTime;
                if (dashCooldown < dashTime - .1)
                    tr.emitting = false;
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    dash = true;
                    tr.emitting = true;

                }
            }
            if(movement != new Vector2(0,0))
                lastmove = movement;
                
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");




            if (movement.x == 0 && movement.y == 0)
            {
                if (stepping == false)
                {
                    walking.Pause();
                    stepping = true;
                }
            }
            else
            {
                if (stepping == true)
                {
                    walking.Play();
                    stepping = false;
                }
            }
            
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if(Input.GetAxisRaw("Horizontal") > 0 && !facingRight){
                Flip();
                animator.SetFloat("Horizontal", movement.x);
                swordAttack.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if(Input.GetAxisRaw("Horizontal") < 0 && facingRight){
                Flip();
                animator.SetFloat("Horizontal", movement.x);
                swordAttack.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if(Input.GetAxisRaw("Horizontal") > 0){
                swordAttack.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if(Input.GetAxisRaw("Horizontal") < 0){
                swordAttack.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if(Input.GetAxisRaw("Vertical") > 0){
                animator.SetFloat("Vertical", movement.y);
                swordAttack.localRotation = Quaternion.Euler(0, 0, 90);
            }
            if(Input.GetAxisRaw("Vertical") < 0){
                animator.SetFloat("Vertical", movement.y);
                swordAttack.localRotation = Quaternion.Euler(0, 0, 270);
            }

            dir = new Vector3(movement.x, movement.y).normalized;
        }
        

    }
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void FixedUpdate()
    {
        rb.velocity = dir * moveSpeed;
        Debug.DrawRay(transform.position, dir*teleport, Color.green,0, true);
        if (dash){
            dash = false;
            dashCooldown = dashTime;
            
            if (movement == new Vector2(0,0))
                dir = new Vector3(lastmove.x, lastmove.y).normalized;

            Vector3 dp = transform.position + dir * teleport;
            RaycastHit2D rc = Physics2D.Raycast(transform.position+ new Vector3(0,.25f,0), dir*teleport, teleport, dl);
            
            if (rc.collider != null){
                dp = rc.point - (Vector2)dir/2;
            }
            else{
                dashSound.Play();
            }
            
            rb.MovePosition(dp);
        }
    }
}