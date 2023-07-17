using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SlimeBallBullet : MonoBehaviour
{
    private Vector3 dir, rotate, location;
    public Rigidbody2D rb;
    private float force = 4f;
    private float r;
    private GameObject player;
    private HealthBar pHealth;
    public Transform bulletParent;
    private bool once;
    public Animator animator;
    public RotationConstraint rotateconst;
    public LayerMask splash;
    public CircleCollider2D hitbox;
    //public Constrain constraints;
    // Start is called before the first frame update
    void Awake(){
        location = transform.localPosition;
    }
    void Start()
    {
        once = true;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rotateconst = this.GetComponent<RotationConstraint>();
        hitbox = gameObject.GetComponent<CircleCollider2D>();
        pHealth = player.GetComponent<HealthBar>();

    }

    // Update is called once per frame
    void Update()
    {

        if(once){
            once = false;
            bulletParent = transform.parent;
            transform.parent = null;
            dir = player.transform.position - transform.position;
            rotate = transform.position - player.transform.position;
            r = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
            rb.velocity = new Vector2(dir.x, dir.y+1).normalized * force;
            animator.SetFloat("X", dir.x);
            animator.SetFloat("Y", dir.y);
            hitbox.enabled = true;
            Invoke("Repool", 2);
            
        }
        
    }

    public void Repool(){
        rotateconst.enabled = true;
        hitbox.enabled = false;
        rb.velocity = new Vector2(0,0).normalized;
        if(!bulletParent)
            Destroy(gameObject);
        else
            transform.parent = bulletParent.transform;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = location;
        once = true;
        gameObject.SetActive(false);
        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            Debug.Log("hit Player");
            Debug.Log(other.gameObject.name);
            pHealth.hit(4);
        }
        if ((splash.value & (1 << other.transform.gameObject.layer)) > 0){
            hitbox.enabled = false;
            rotateconst.enabled = false;
            //transform.parent = other.transform;
            animator.SetBool("Hit", true);
            rb.velocity = new Vector2(0,0).normalized;
            transform.rotation = Quaternion.Euler(0, 0, r-90);
            CancelInvoke("Repool");
            Invoke("Repool", 1);
        }
    }

    
}
